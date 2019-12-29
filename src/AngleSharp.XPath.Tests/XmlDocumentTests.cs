using AngleSharp.Xml.Parser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AngleSharp.XPath.Tests
{
    [TestFixture]
    class XmlDocumentTests
    {
        public static string Xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"><url><loc>https://www.test.com/de/accounts/profile</loc><xhtml:link rel=\"alternate\" hreflang=\"fr\" href=\"https://www.test.com/fr/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.test.com/en/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"it\" href=\"https://www.test.com/it/accounts/profile\" /><changefreq>weekly</changefreq><priority>0.4</priority></url></urlset>";
        [Test]
        public static void Test1()
        {
            var xmldocument = new System.Xml.XmlDocument();
            xmldocument.LoadXml(Xml);
            var ms = new XmlNamespaceManager(new NameTable());
            ms.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            ms.AddNamespace("def", "http://www.sitemaps.org/schemas/sitemap/0.9");
            var node = xmldocument.SelectSingleNode("//def:urlset/def:url/xhtml:link", ms);
            Assert.AreEqual("https://www.test.com/fr/accounts/profile", node.Attributes.GetNamedItem("href").Value);
            var parser = new XmlParser();
            var doc = parser.ParseDocument(Xml);
            var node1 = (Dom.IElement)doc.SelectSingleNodeNS("//def:urlset/def:url/xhtml:link", ms);
            Assert.AreEqual("https://www.test.com/fr/accounts/profile",node1.GetAttribute("href"));


        }
    }
}
