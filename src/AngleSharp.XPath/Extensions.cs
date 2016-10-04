using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace AngleSharp.XPath
{
	public static class Extensions
	{
		public static XPathNavigator CreateNavigator(this IHtmlDocument document)
		{
			if (document == null)
			{
				throw new ArgumentNullException(nameof(document));
			}

			return new HtmlDocumentNavigator(document, document.DocumentElement);
		}

		[DebuggerStepThrough]
		public static string GetOrAdd(this XmlNameTable table, string array)
		{
			string s = table.Get(array);

			if (s == null)
			{
				return table.Add(array);
			}

			return s;
		}

		public static INode SelectSingleNode(this IElement element, string xpath)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			if (xpath == null)
			{
				throw new ArgumentNullException(nameof(xpath));
			}

			HtmlDocumentNavigator nav = new HtmlDocumentNavigator(element.Owner, element);
			XPathNodeIterator it = nav.Select(xpath);

			if (!it.MoveNext())
			{
				return null;
			}

			HtmlDocumentNavigator node = (HtmlDocumentNavigator)it.Current;
			return node.CurrentNode;
		}
	}
}