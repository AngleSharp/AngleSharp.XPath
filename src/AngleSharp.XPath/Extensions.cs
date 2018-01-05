using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
			var s = table.Get(array);

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

			var nav = new HtmlDocumentNavigator(element.Owner, element);
			var it = nav.Select(xpath);

			if (!it.MoveNext())
			{
				return null;
			}

			var node = (HtmlDocumentNavigator)it.Current;
			return node.CurrentNode;
		}

		/// <summary>
		/// Selects a list of nodes matching the <see cref="XPath"/> expression.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="xpath">The XPath expression.</param>
		/// <returns>List of nodes matching <paramref name="xpath"/> query.</returns>
		/// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
		public static List<INode> SelectNodes(this IElement element, string xpath)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			if (xpath == null)
			{
				throw new ArgumentNullException(nameof(xpath));
			}
			
			var nav = new HtmlDocumentNavigator(element.Owner, element);
			var it = nav.Select(xpath);
			var result = new List<INode>();

			while (it.MoveNext())
			{
				var naviagtor = (HtmlDocumentNavigator) it.Current;
				var e = naviagtor.CurrentNode;
				result.Add(e);
			}

			return result;
		}
	}
}
