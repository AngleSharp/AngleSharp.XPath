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
    // context. Each scope gets its own result, which is replaced as a whole when it is stale.
    private readonly ConditionalWeakTable<IElement, ResultHolder> _results = new();

    public XPathAttrSelector(String value) => _value = value;

    public Priority Specificity => Priority.OneClass;

    public String Text => $"[xpath>'{_value}']";

    public void Accept(ISelectorVisitor visitor)
    {
    }

    public Boolean Match(IElement element, IElement scope)
    {
        var holder = _results.GetOrCreateValue(scope);
        var version = GetMutationVersion(scope);
        var result = Volatile.Read(ref holder.Result);

        if (result == null || result.Version != version)
        {
            result = new Result(version, new HashSet<INode>(scope.SelectNodes(_value), ReferenceComparer.Instance));
            Volatile.Write(ref holder.Result, result);
        }

        return result.Nodes.Contains(element);
    }

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
        public Result(Int64 version, HashSet<INode> nodes)
        {
            Version = version;
            Nodes = nodes;
        }

        public Int64 Version { get; }

        public HashSet<INode> Nodes { get; }
    }

    // Node.Equals(INode) compares structure, so look-alike nodes must not count as the same one.
    private sealed class ReferenceComparer : IEqualityComparer<INode>
    {
        public static readonly ReferenceComparer Instance = new();

        public Boolean Equals(INode x, INode y) => ReferenceEquals(x, y);

        public Int32 GetHashCode(INode obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
