using AngleSharp.Dom;
using System;
using System.Xml;
using System.Xml.XPath;

namespace AngleSharp.XPath
{
    /// <inheritdoc />
    public class HtmlDocumentNavigator : XPathNavigator
	{
		private readonly IDocument _document;
        private INode _currentNode;
		private int _attrIndex;

        /// <summary>
        /// Creates a new XPath navigator for the given document using the provided root node.
        /// </summary>
        /// <param name="document">The document to navigate.</param>
        /// <param name="currentNode">The node to start navigation.</param>
        public HtmlDocumentNavigator(IDocument document, INode currentNode)
		{
			_document = document ?? throw new ArgumentNullException(nameof(document));
            NameTable = new NameTable();
            _currentNode = currentNode ?? throw new ArgumentNullException(nameof(currentNode));
            _attrIndex = -1;
        }

        /// <inheritdoc />
        public override string BaseURI => _document.BaseUri;

        /// <summary>
        /// Gets the currently selected node.
        /// </summary>
		public INode CurrentNode => _currentNode;

        /// <summary>
        /// Gets the currently selected element.
        /// </summary>
		private IElement CurrentElement => CurrentNode as IElement;

        /// <inheritdoc />
        public override bool HasAttributes => CurrentElement != null && CurrentElement.Attributes.Length > 0;

        /// <inheritdoc />
        public override bool IsEmptyElement => !_currentNode.HasChildNodes;

        /// <inheritdoc />
        public override string LocalName =>
            _attrIndex != -1
                ? NameTable.GetOrAdd(CurrentElement.Attributes[_attrIndex].LocalName)
                : NameTable.GetOrAdd(CurrentNode is IElement e ? e.LocalName : string.Empty);

        /// <inheritdoc />
        public override string Name =>
            _attrIndex != -1
                ? NameTable.GetOrAdd(CurrentElement.Attributes[_attrIndex].Name)
                : NameTable.GetOrAdd(_currentNode.NodeName);

        /// <inheritdoc />
        public override string NamespaceURI => string.Empty
            /*_attrIndex != -1
                ? NameTable.GetOrAdd(CurrentElement.Attributes[_attrIndex].NamespaceUri ?? string.Empty)
                : NameTable.GetOrAdd(CurrentElement?.NamespaceUri ?? string.Empty)*/;

        /// <inheritdoc />
        public override string Prefix =>
            _attrIndex != 1
                ? NameTable.GetOrAdd(CurrentElement.Attributes[_attrIndex].Prefix ?? string.Empty)
                : NameTable.GetOrAdd(CurrentElement?.Prefix ?? string.Empty);

        /// <inheritdoc />
        public override XmlNameTable NameTable { get; }

        /// <inheritdoc />
        public override XPathNodeType NodeType
		{
			get
			{
				switch (_currentNode.NodeType)
				{
					case Dom.NodeType.Attribute:
						return XPathNodeType.Attribute;

					case Dom.NodeType.CharacterData:
						return XPathNodeType.Text;

					case Dom.NodeType.Comment:
						return XPathNodeType.Comment;

					case Dom.NodeType.Document:
						return XPathNodeType.Element;

					case Dom.NodeType.DocumentType:
						return XPathNodeType.Element;

                    case Dom.NodeType.Element:
						return _attrIndex != -1 ? XPathNodeType.Attribute : XPathNodeType.Element;

                    case Dom.NodeType.ProcessingInstruction:
						return XPathNodeType.ProcessingInstruction;

					case Dom.NodeType.Text:
						return XPathNodeType.Text;

                    case Dom.NodeType.Entity:
                    case Dom.NodeType.EntityReference:
                    case Dom.NodeType.Notation:
                    case Dom.NodeType.DocumentFragment:
                    default:
						throw new NotImplementedException();
				}
			}
		}

        /// <inheritdoc />
        public override string Value
		{
			get
			{
				switch (_currentNode.NodeType)
				{
					case Dom.NodeType.Attribute:
						var attr = (IAttr)_currentNode;
						return attr.Value;

					case Dom.NodeType.CharacterData:
						var cdata = (ICharacterData)_currentNode;
						return cdata.Data;

					case Dom.NodeType.Comment:
						var comment = (IComment)_currentNode;
						return comment.Data;

					case Dom.NodeType.Document:
						return _currentNode.TextContent;

					case Dom.NodeType.DocumentFragment:
						return _currentNode.TextContent;

					case Dom.NodeType.DocumentType:
						var documentType = (IDocumentType)_currentNode;
						return documentType.Name;

					case Dom.NodeType.Element:
						return _attrIndex != -1 ? CurrentElement.Attributes[_attrIndex].Value : _currentNode.TextContent;

                    case Dom.NodeType.Entity:
						return _currentNode.TextContent;

					case Dom.NodeType.EntityReference:
						return _currentNode.TextContent;

					case Dom.NodeType.Notation:
						return _currentNode.TextContent;

					case Dom.NodeType.ProcessingInstruction:
						var instruction = (IProcessingInstruction)_currentNode;
						return instruction.Target;

					case Dom.NodeType.Text:
						return _currentNode.TextContent;

					default:
						throw new NotImplementedException();
				}
			}
		}

        /// <inheritdoc />
        public override XPathNavigator Clone()
		{
			return new HtmlDocumentNavigator(_document, _currentNode);
		}

        /// <inheritdoc />
        public override bool IsSamePosition(XPathNavigator other)
		{
			if (!(other is HtmlDocumentNavigator navigator))
			{
				return false;
			}

			return navigator._currentNode == _currentNode;
		}

        /// <inheritdoc />
        public override bool MoveTo(XPathNavigator other)
		{
			if (!(other is HtmlDocumentNavigator navigator))
			{
				return false;
			}

			if (navigator._document == _document)
			{
				_currentNode = navigator._currentNode;
				_attrIndex = navigator._attrIndex;
				return true;
			}

			return false;
		}

        /// <inheritdoc />
        public override bool MoveToFirstAttribute()
		{
			if (HasAttributes)
			{
				_attrIndex = 0;
				return true;
			}

			return false;
		}

        /// <inheritdoc />
        public override bool MoveToFirstChild()
		{
			if (_currentNode.FirstChild == null)
			{
				return false;
			}

			_currentNode = _currentNode.FirstChild;
			return true;
		}

        /// <inheritdoc />
        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

        /// <inheritdoc />
        public override bool MoveToId(string id)
		{
			var elementById = _document.GetElementById(id);

			if (elementById == null)
			{
				return false;
			}

			_currentNode = elementById;
			return true;
		}

        /// <inheritdoc />
        public override bool MoveToNext()
		{
			if (_currentNode.NextSibling == null)
			{
				return false;
			}

			_currentNode = _currentNode.NextSibling;
			return true;
		}

        /// <inheritdoc />
        public override bool MoveToNextAttribute()
		{
			if (CurrentElement == null)
			{
				return false;
			}

			if (_attrIndex >= CurrentElement.Attributes.Length - 1)
			{
				return false;
			}

			_attrIndex++;
			return true;
		}

        /// <inheritdoc />
        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

        /// <inheritdoc />
        public override bool MoveToParent()
		{
			if (_currentNode.Parent == null)
			{
				return false;
			}

			_currentNode = _currentNode.Parent;
			return true;
		}

        /// <inheritdoc />
        public override bool MoveToPrevious()
		{
			if (_currentNode.PreviousSibling == null)
			{
				return false;
			}

			_currentNode = _currentNode.PreviousSibling;
			return true;
		}

        /// <inheritdoc />
        public override void MoveToRoot()
		{
			_currentNode = _document;
		}
    }
}
