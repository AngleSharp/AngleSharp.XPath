using System;
using System.IO;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;

namespace AngleSharp.XPath
{
    internal class AttrNodeWrapper : IAttr, INode
    {
        public AttrNodeWrapper(IAttr attribute, IElement parentElement)
        {
            Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            ParentElement = parentElement ?? throw new ArgumentNullException(nameof(parentElement));
        }

        public IAttr Attribute
        {
            get;
        }

        public string LocalName => Attribute.LocalName;

        public string Name => Attribute.Name;

        public string Value
        {
            get => Attribute.Value;
            set => Attribute.Value = value;
        }

        public string NamespaceUri => Attribute.NamespaceUri;

        public string Prefix => Attribute.Prefix;

        string INode.BaseUri => null;

        Url INode.BaseUrl => null;

        string INode.NodeName => Attribute.Name;

        INodeList INode.ChildNodes => null;

        IDocument INode.Owner => ParentElement.Owner;

        public IElement ParentElement
        {
            get;
        }

        INode INode.Parent => ParentElement;

        INode INode.FirstChild => null;

        INode INode.LastChild => null;

        INode INode.NextSibling => null;

        INode INode.PreviousSibling => null;

        NodeType INode.NodeType => NodeType.Attribute;

        string INode.NodeValue
        {
            get => Value;
            set => Value = value;
        }

        string INode.TextContent
        {
            get => Value;
            set => Value = value;
        }

        bool INode.HasChildNodes => false;

        NodeFlags INode.Flags => NodeFlags.None;

        public bool Equals(IAttr other)
        {
            return Attribute.Equals(other);
        }

        void IEventTarget.AddEventListener(string type, DomEventHandler callback, bool capture)
        {
            throw new NotSupportedException();
        }

        INode INode.AppendChild(INode child)
        {
            throw new NotSupportedException();
        }

        INode INode.Clone(bool deep)
        {
            throw new NotSupportedException();
        }

        DocumentPositions INode.CompareDocumentPosition(INode otherNode)
        {
            throw new NotSupportedException();
        }

        bool INode.Contains(INode otherNode) => false;

        bool IEventTarget.Dispatch(Event ev) => false;

        bool INode.Equals(INode otherNode)
        {
            return otherNode is AttrNodeWrapper attrNodeWrapper && Equals(attrNodeWrapper);
        }

        INode INode.InsertBefore(INode newElement, INode referenceElement)
        {
            throw new NotSupportedException();
        }

        void IEventTarget.InvokeEventListener(Event ev)
        {
            throw new NotSupportedException();
        }

        bool INode.IsDefaultNamespace(string namespaceUri)
        {
            throw new NotSupportedException();
        }

        string INode.LookupNamespaceUri(string prefix)
        {
            throw new NotSupportedException();
        }

        string INode.LookupPrefix(string namespaceUri)
        {
            throw new NotSupportedException();
        }

        void INode.Normalize()
        {
            throw new NotSupportedException();
        }

        INode INode.RemoveChild(INode child)
        {
            throw new NotSupportedException();
        }

        void IEventTarget.RemoveEventListener(string type, DomEventHandler callback, bool capture)
        {
            throw new NotSupportedException();
        }

        INode INode.ReplaceChild(INode newChild, INode oldChild)
        {
            throw new NotSupportedException();
        }

        void IMarkupFormattable.ToHtml(TextWriter writer, IMarkupFormatter formatter)
        {
            writer.Write($"{Name}=\"{Value}\"");
        }
    }
}
