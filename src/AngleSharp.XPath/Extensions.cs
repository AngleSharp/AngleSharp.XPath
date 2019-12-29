using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace AngleSharp.XPath
{
    /// <summary>
    /// Hosts the extension methods for XPath parsing.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates a new navigator for the given document.
        /// </summary>
        /// <param name="document">The document to extend.</param>
        /// <param name="ignoreNamespaces"></param>
        /// <returns>The navigator for XPath expressions.</returns>
        public static XPathNavigator CreateNavigator(this IDocument document, bool ignoreNamespaces = true)
        {
            var doc = document ?? throw new ArgumentNullException(nameof(document));
            return new HtmlDocumentNavigator(doc, doc.DocumentElement, ignoreNamespaces);
        }

        [DebuggerStepThrough]
        internal static string GetOrAdd(this XmlNameTable table, string array)
        {
            var s = table.Get(array);
            return s ?? table.Add(array);
        }

        /// <summary>
        /// Selects a single node (or returns null) matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="ignoreNamespaces"></param>
        /// <returns>The node matching <paramref name="xpath"/> query, if any.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static INode SelectSingleNode(this IElement element, string xpath, bool ignoreNamespaces = true)
        {
            return element.SelectSingleNode(xpath, new XmlNamespaceManager(new NameTable()), ignoreNamespaces);
        }

        /// <summary>
        /// Selects a single node (or returns null) matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="resolver"></param>
        /// <param name="ignoreNamespaces"></param>
        /// <returns>The node matching <paramref name="xpath"/> query, if any.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static INode SelectSingleNode(this IElement element, string xpath, IXmlNamespaceResolver resolver, bool ignoreNamespaces = true)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, ignoreNamespaces);
            var it = nav.SelectSingleNode(xp, resolver ?? new XmlNamespaceManager(new NameTable()));
            return ((HtmlDocumentNavigator) it)?.CurrentNode;
        }

        /// <summary>
        /// Selects a list of nodes matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="ignoreNamespaces"></param>
        /// <returns>List of nodes matching <paramref name="xpath"/> query.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static List<INode> SelectNodes(this IElement element, string xpath, bool ignoreNamespaces = true)
        {
            return element.SelectNodes(xpath, new XmlNamespaceManager(new NameTable()), ignoreNamespaces);
        }

        /// <summary>
        /// Selects a list of nodes matching the <see cref="XPath"/> expression.
        /// </summary>
        /// <param name="element">The element to start looking from.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="resolver"></param>
        /// <param name="ignoreNamespaces"></param>
        /// <returns>List of nodes matching <paramref name="xpath"/> query.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="element"/> or <paramref name="xpath"/> is <c>null</c></exception>
        public static List<INode> SelectNodes(this IElement element, string xpath, IXmlNamespaceResolver resolver, bool ignoreNamespaces = true)
        {
            var el = element ?? throw new ArgumentNullException(nameof(element));
            var xp = xpath ?? throw new ArgumentNullException(nameof(xpath));
            var nav = new HtmlDocumentNavigator(el.Owner, el, ignoreNamespaces);
            var it = nav.Select(xp, resolver ?? new XmlNamespaceManager(new NameTable()));
            var result = new List<INode>();

            while (it.MoveNext())
            {
                // ReSharper disable once IdentifierTypo
                var naviagtor = (HtmlDocumentNavigator) it.Current;
                // ReSharper disable once PossibleNullReferenceException
                var e = naviagtor.CurrentNode;
                result.Add(e);
            }

            return result;
        }
    }
}
