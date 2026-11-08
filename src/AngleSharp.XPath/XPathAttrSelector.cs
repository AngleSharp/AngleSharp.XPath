using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AngleSharp.XPath;

sealed class XPathAttrSelector : ISelector
{
    private readonly String _value;

    // AngleSharp caches parsed selectors, so one instance serves every query and thread of a
    // context. The last result answers the elements of one query without a lookup; the table
    // keeps a result per scope once a second scope shows up, so queries on different scopes
    // do not keep replacing each other's result.
    private Result _last;
    private ConditionalWeakTable<IElement, ResultHolder> _results;

    public XPathAttrSelector(String value) => _value = value;

    public Priority Specificity => Priority.OneClass;

    public String Text => $"[xpath>'{_value}']";

    public void Accept(ISelectorVisitor visitor)
    {
    }

    public Boolean Match(IElement element, IElement scope)
    {
        var version = GetMutationVersion(scope);
        var result = Volatile.Read(ref _last);

        if (result == null || result.Scope != scope || result.Version != version)
        {
            result = GetResult(scope, version, result);
            Volatile.Write(ref _last, result);
        }

        return result.Contains(element);
    }

    private Result GetResult(IElement scope, Int64 version, Result last)
    {
        // A first scope, or the last one after a change: no other scope needs keeping yet
        if (last == null || (last.Scope == scope && Volatile.Read(ref _results) == null))
        {
            return Evaluate(scope, version);
        }

        var results = Volatile.Read(ref _results);

        if (results == null)
        {
            Interlocked.CompareExchange(ref _results, new ConditionalWeakTable<IElement, ResultHolder>(), null);
            results = Volatile.Read(ref _results);
            results.GetOrCreateValue(last.Scope).Result = last;
        }

        var holder = results.GetOrCreateValue(scope);
        var result = Volatile.Read(ref holder.Result);

        if (result == null || result.Version != version)
        {
            result = Evaluate(scope, version);
            Volatile.Write(ref holder.Result, result);
        }

        return result;
    }

    private Result Evaluate(IElement scope, Int64 version) => new(scope, version, scope.SelectNodes(_value));

    // The document's counter advances with every change to its tree, so an equal reading means the
    // result still holds. Other IDocument implementations have no counter and keep their result.
    private static Int64 GetMutationVersion(IElement scope) =>
        scope.Owner is Document document ? document.MutationVersion : 0;

    private sealed class ResultHolder
    {
        public Result Result;
    }

    private sealed class Result
    {
        // Up to this many nodes, comparing references one by one is cheaper than hashing
        private const Int32 MaxScanned = 8;

        private readonly INode[] _scanned;
        private readonly HashSet<INode> _hashed;

        public Result(IElement scope, Int64 version, List<INode> nodes)
        {
            Scope = scope;
            Version = version;

            if (nodes.Count <= MaxScanned)
            {
                _scanned = nodes.ToArray();
            }
            else
            {
                _hashed = new HashSet<INode>(nodes, ReferenceComparer.Instance);
            }
        }

        public IElement Scope { get; }

        public Int64 Version { get; }

        public Boolean Contains(INode node)
        {
            if (_hashed != null)
            {
                return _hashed.Contains(node);
            }

            foreach (var scanned in _scanned)
            {
                if (ReferenceEquals(scanned, node))
                {
                    return true;
                }
            }

            return false;
        }
    }

    // Node.Equals(INode) compares structure, so look-alike nodes must not count as the same one.
    private sealed class ReferenceComparer : IEqualityComparer<INode>
    {
        public static readonly ReferenceComparer Instance = new();

        public Boolean Equals(INode x, INode y) => ReferenceEquals(x, y);

        public Int32 GetHashCode(INode obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
