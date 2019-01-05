namespace AngleSharp.XPath
{
    using AngleSharp.Dom;
    using System;
    using System.Xml.XPath;

    /// <inheritdoc />
    public class HtmlDocumentNavigable : IXPathNavigable
    {
        private readonly IDocument _document;

        /// <summary>
        /// Creates a new wrapper for navigations regarding the provided document.
        /// </summary>
        /// <param name="document">The document to create navigators for.</param>
        public HtmlDocumentNavigable(IDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <inheritdoc />
        public XPathNavigator CreateNavigator()
        {
            return _document.CreateNavigator();
        }
    }
}
