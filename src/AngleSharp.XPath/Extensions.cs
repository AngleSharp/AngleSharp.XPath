namespace AngleSharp.XPath
{
    using AngleSharp.Dom;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Hosts the extension methods for XPath parsing.
    /// </summary>
    public static class Extensions
	{
        /// <summary>
        /// Creates a new navigator for the given document.
        /// </summary>
        /// <param name="document">The document to extend.</param>
        /// <returns>The navigator for XPath expressions.</returns>
		public static XPathNavigator CreateNavigator(this IDocument document)
		{
            var doc = document ?? throw new ArgumentNullException(nameof(document));
            return new HtmlDocumentNavigator(doc, doc,false);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static XPathNavigator CreateNavigatorNS(this IDocument document)
        {
            var doc = document ?? throw new ArgumentNullException(nameof(document));
            return new HtmlDocumentNavigator(doc, doc, true);
        }

        [DebuggerStepThrough]
		internal static String GetOrAdd(this XmlNameTable table, String array)
		{
			var s = table.Get(array);

			if (s == null)
			{
				return table.Add(array);
			}

			return s;
		}

        /// <inheritdoc />
        public static List<INode> SelectNodes(HtmlDocumentNavigator navigator, XPathExpression xPathExpression)
        {
            var it = navigator.Select(xPathExpression);
            var result = new List<INode>();

            while (it.MoveNext())
            {
                var naviagtor = (HtmlDocumentNavigator)it.Current;
                var e = naviagtor.CurrentNode;
                result.Add(e);
            }

            return result;
        }
        /// <inheritdoc />
        public static INode SelectSingleNode(HtmlDocumentNavigator navigator, XPathExpression xPathExpression)
        {
            var it = navigator.Select(xPathExpression);
            var result = new List<INode>();

            if (it.MoveNext())
            {
                var node = (HtmlDocumentNavigator)it.Current;
                return node.CurrentNode;
            }

            return null;
        }
        /// <inheritdoc />
        public static INode SelectSingleNodeNS(this IDocument element, String xpath, IXmlNamespaceResolver xmlNamespaceResolver)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el, el, true);
            return SelectSingleNode(nav, XPathExpression.Compile(xp, xmlNamespaceResolver));
        }
        /// <inheritdoc />
        public static INode SelectSingleNodeNS(this IDocument element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el, el, true);
            return SelectSingleNode(nav, XPathExpression.Compile(xp));
        }
        /// <inheritdoc />
        public static INode SelectSingleNodeNS(this IElement element, String xpath,IXmlNamespaceResolver xmlNamespaceResolver)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, true);
            return SelectSingleNode(nav, XPathExpression.Compile(xp, xmlNamespaceResolver));
        }
        /// <inheritdoc />
        public static INode SelectSingleNodeNS(this IElement element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, true);
            return SelectSingleNode(nav, XPathExpression.Compile(xp));
        }
        /// <inheritdoc />
        public static List<INode> SelectNodesNS(this IDocument element, String xpath, IXmlNamespaceResolver xmlNamespaceResolver)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el, el, true);
            return SelectNodes(nav, XPathExpression.Compile(xp, xmlNamespaceResolver));
        }
        /// <inheritdoc />
        public static List<INode> SelectNodesNS(this IDocument element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el, el, true);
            return SelectNodes(nav, XPathExpression.Compile(xp));
        }
        /// <inheritdoc />
        public static List<INode> SelectNodesNS(this IElement element, String xpath, IXmlNamespaceResolver xmlNamespaceResolver)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, true);
            return SelectNodes(nav, XPathExpression.Compile(xp, xmlNamespaceResolver));
        }
        /// <inheritdoc />
        public static List<INode> SelectNodesNS(this IElement element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, true);
            return SelectNodes(nav, XPathExpression.Compile(xp));
        }

        /// <summary>
        /// Selects a single node (or returns null) matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns>The node matching <paramref name="xpath"/> query, if any.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static INode SelectSingleNode(this IElement element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, false);
            return SelectSingleNode(nav, XPathExpression.Compile(xp));
        }
        /// <summary>
        /// Selects a single node (or returns null) matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns>The node matching <paramref name="xpath"/> query, if any.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static INode SelectSingleNode(this IDocument element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el, el, false);
            return SelectSingleNode(nav, XPathExpression.Compile(xp));
        }

        /// <summary>
        /// Selects a list of nodes matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns>List of nodes matching <paramref name="xpath"/> query.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static List<INode> SelectNodes(this IElement element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, false);
            return SelectNodes(nav, XPathExpression.Compile(xp));
		}
        /// <summary>
        /// Selects a list of nodes matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns>List of nodes matching <paramref name="xpath"/> query.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static List<INode> SelectNodes(this IDocument element, String xpath)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el, el, false);
            return SelectNodes(nav, XPathExpression.Compile(xp));
        }
    }
}
