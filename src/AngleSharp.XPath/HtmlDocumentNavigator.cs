using AngleSharp.Dom;
using System;
using System.Xml;
using System.Xml.XPath;

namespace AngleSharp.XPath
{
	public class HtmlDocumentNavigator : XPathNavigator
	{
		private readonly IDocument _document;

		private INode _currentNode;

		private int _attrIndex;

		private readonly NameTable _nameTable;

		public HtmlDocumentNavigator(IDocument document, INode currentNode)
		{
			_document = document ?? throw new ArgumentNullException(nameof(document));
			_currentNode = currentNode ?? throw new ArgumentNullException(nameof(currentNode));
			_nameTable = new NameTable();
			_attrIndex = -1;
		}

		public override string BaseURI => _document.BaseUri;

		public INode CurrentNode => _currentNode;

		private IElement CurrentElement => CurrentNode as IElement;

		public override bool HasAttributes => CurrentElement != null && CurrentElement.Attributes.Length > 0;

		public override bool IsEmptyElement => !_currentNode.HasChildNodes;

		public override string LocalName
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

		public override string Name => NameTable.GetOrAdd(_currentNode.NodeName);

		public override string NamespaceURI => string.Empty;// NameTable.GetOrAdd(CurrentElement?.NamespaceUri ?? string.Empty);

		public override XmlNameTable NameTable => _nameTable;

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

					//case Dom.NodeType.DocumentFragment:
					//	return XPathNodeType.;

					//case Dom.NodeType.DocumentType:
					//	return XPathNodeType.;

					case Dom.NodeType.Element:
						if (_attrIndex != -1)
						{
							return XPathNodeType.Attribute;
						}

						return XPathNodeType.Element;

					//case Dom.NodeType.Entity:
					//	return XPathNodeType.en;

					//case Dom.NodeType.EntityReference:

					//case Dom.NodeType.Notation:
					//	return XPathNodeType.;

					case Dom.NodeType.ProcessingInstruction:
						return XPathNodeType.ProcessingInstruction;

					case Dom.NodeType.Text:
						return XPathNodeType.Text;

					default:
						throw new NotImplementedException();
				}
			}
		}

		public override string Prefix => string.Empty;// _nameTable.GetOrAdd(CurrentElement?.Prefix ?? string.Empty);

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

		public override XPathNavigator Clone()
		{
			return new HtmlDocumentNavigator(_document, _currentNode);
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			if (!(other is HtmlDocumentNavigator navigator))
			{
				return false;
			}

			return navigator._currentNode == _currentNode;
		}

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

		public override bool MoveToFirstAttribute()
		{
			if (HasAttributes)
			{
				_attrIndex = 0;
				return true;
			}

			return false;
		}

		public override bool MoveToFirstChild()
		{
			if (_currentNode.FirstChild == null)
			{
				return false;
			}

			_currentNode = _currentNode.FirstChild;
			return true;
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

		public override bool MoveToId(string id)
		{
			IElement elementById = _document.GetElementById(id);

			if (elementById == null)
			{
				return false;
			}

			_currentNode = elementById;
			return true;
		}

		public override bool MoveToNext()
		{
			if (_currentNode.NextSibling == null)
			{
				return false;
			}

			_currentNode = _currentNode.NextSibling;
			return true;
		}

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

		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

		public override bool MoveToParent()
		{
			if (_currentNode.Parent == null)
			{
				return false;
			}

			_currentNode = _currentNode.Parent;
			return true;
		}

		public override bool MoveToPrevious()
		{
			if (_currentNode.PreviousSibling == null)
			{
				return false;
			}

			_currentNode = _currentNode.PreviousSibling;
			return true;
		}

		public override void MoveToRoot()
		{
			_currentNode = _document.DocumentElement;
		}
	}
}
