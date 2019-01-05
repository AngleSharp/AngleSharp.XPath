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
            return new HtmlDocumentNavigator(doc, doc.DocumentElement);
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
			var nav = new HtmlDocumentNavigator(el.Owner, el);
			var it = nav.Select(xp);

			if (it.MoveNext())
            {
                var node = (HtmlDocumentNavigator)it.Current;
                return node.CurrentNode;
            }

            return null;
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
            var nav = new HtmlDocumentNavigator(el.Owner, el);
			var it = nav.Select(xp);
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
