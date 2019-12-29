namespace AngleSharp.XPath
{
    using AngleSharp.Dom;
    using System;
    using System.Xml;
    using System.Xml.XPath;

    /// <inheritdoc />
    public class HtmlDocumentNavigator : XPathNavigator
	{
        private bool _enableNamespaces = false;
		private readonly IDocument _document;
        private readonly NameTable _nameTable;
        private INode _currentNode;
		private Int32 _attrIndex;

        /// <summary>
        /// Creates a new XPath navigator for the given document using the provided root node.
        /// </summary>
        /// <param name="document">The document to navigate.</param>
        /// <param name="currentNode">The node to start navigation.</param>
        /// <param name="enableNamespaces">Enable namespaces or not</param>
		public HtmlDocumentNavigator(IDocument document, INode currentNode,bool enableNamespaces)
		{
			_document = document ?? throw new ArgumentNullException(nameof(document));
            _nameTable = new NameTable();
            _currentNode = currentNode ?? throw new ArgumentNullException(nameof(currentNode));
			_attrIndex = -1;
            _enableNamespaces = enableNamespaces;
		}
        /// <summary>
        /// Creates a new XPath navigator for the given document using the provided root node.
        /// </summary>
        /// <param name="document">The document to navigate.</param>
        /// <param name="currentNode">The node to start navigation.</param>
        public HtmlDocumentNavigator(IDocument document, INode currentNode):this(document,currentNode,false)
        {

        }

        /// <inheritdoc />
        public override String BaseURI => _document.BaseUri;

        /// <summary>
        /// Gets the currently selected node.
        /// </summary>
		public INode CurrentNode => _currentNode;

        /// <summary>
        /// Gets the currently selected element.
        /// </summary>
		private IElement CurrentElement => CurrentNode as IElement;

        /// <inheritdoc />
        public override Boolean HasAttributes => CurrentElement != null && CurrentElement.Attributes.Length > 0;

        /// <inheritdoc />
        public override Boolean IsEmptyElement => !_currentNode.HasChildNodes;

        /// <inheritdoc />
        public override String LocalName
		{
			get
			{
				if (_attrIndex != -1)
				{
					return NameTable.GetOrAdd(CurrentElement.Attributes[_attrIndex].Name);
				}

				if (CurrentNode is IElement)
				{
					return NameTable.GetOrAdd(CurrentElement.LocalName);
				}

				return NameTable.GetOrAdd(CurrentNode.NodeName);
			}
		}

        /// <inheritdoc />
        public override String Name => NameTable.GetOrAdd(_currentNode.NodeName);

        /// <inheritdoc />
        public override String NamespaceURI => _enableNamespaces ? NameTable.GetOrAdd(CurrentElement?.NamespaceUri ?? string.Empty) : string.Empty;

        /// <inheritdoc />
        public override XmlNameTable NameTable => _nameTable;

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
						if (_attrIndex != -1)
						{
							return XPathNodeType.Attribute;
						}

						return XPathNodeType.Element;

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
        public override String Prefix => _enableNamespaces ? _nameTable.GetOrAdd(CurrentElement?.Prefix ?? string.Empty) : string.Empty;

        /// <inheritdoc />
        public override String Value
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
						if (_attrIndex != -1)
						{
							return CurrentElement.Attributes[_attrIndex].Value;
						}

						return _currentNode.TextContent;

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
			return new HtmlDocumentNavigator(_document, _currentNode,_enableNamespaces);
		}

        /// <inheritdoc />
        public override Boolean IsSamePosition(XPathNavigator other)
		{
			if (!(other is HtmlDocumentNavigator navigator))
			{
				return false;
			}

			return navigator._currentNode == _currentNode;
		}

        /// <inheritdoc />
        public override Boolean MoveTo(XPathNavigator other)
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
        public override Boolean MoveToFirstAttribute()
		{
			if (HasAttributes)
			{
				_attrIndex = 0;
				return true;
			}

			return false;
		}

        /// <inheritdoc />
        public override Boolean MoveToFirstChild()
		{
			if (_currentNode.FirstChild == null)
			{
				return false;
			}

			_currentNode = _currentNode.FirstChild;
			return true;
		}

        /// <inheritdoc />
        public override Boolean MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

        /// <inheritdoc />
        public override Boolean MoveToId(String id)
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
        public override Boolean MoveToNext()
		{
			if (_currentNode.NextSibling == null)
			{
				return false;
			}

			_currentNode = _currentNode.NextSibling;
			return true;
		}

        /// <inheritdoc />
        public override Boolean MoveToNextAttribute()
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
        public override Boolean MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

        /// <inheritdoc />
        public override Boolean MoveToParent()
		{
			if (_currentNode.Parent == null)
			{
				return false;
			}

			_currentNode = _currentNode.Parent;
			return true;
		}

        /// <inheritdoc />
        public override Boolean MoveToPrevious()
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
