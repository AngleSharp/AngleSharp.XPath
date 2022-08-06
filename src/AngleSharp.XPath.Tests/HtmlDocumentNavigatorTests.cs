using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Xml.Parser;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace AngleSharp.XPath.Tests
{
    [TestFixture]
    public class HtmlDocumentNavigatorTests
    {
        [Test, Retry(5)]
        public async Task SelectSingleNodeTest()
        {
            // Arrange
            const string address = "https://stackoverflow.com/questions/39471800/is-anglesharps-htmlparser-threadsafe";
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(address);

            // Act
            var content = document.DocumentElement.SelectSingleNode("//div[@id='content']");

            // Assert
            Assert.That(content, Is.Not.Null);
        }

        [Test]
        public void SelectNodes_SelectList_ShouldReturnList()
        {
            // Arrange
            const string html =
            @"<ol>
				<li>First</li>
				<li>Second</li>
				<li>Third</li>
			</ol>";
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            // Act
            var nodes = document.DocumentElement.SelectNodes("//li");

            // Assert
            Assert.That(nodes, Has.Count.EqualTo(3));
        }

        [Test]
        public void SelectPrecedingNodeInDocumentWithDoctype_ShouldReturnNode()
        {
            // Arrange
            const string html =
            @"<!DOCTYPE html>
			<body>
				<span></span>
				<div></div>
			</body>";
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            // Act
            var node = document.DocumentElement.SelectSingleNode("//div/preceding::span");

            // Assert
            Assert.That(node, Is.Not.Null);
        }

        [Test]
        public void SelectSingleNode_IgnoreNamespaces_ShouldReturnNode()
        {
            // Arrange
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"><url><loc>https://www.test.com/de/accounts/profile</loc><xhtml:link rel=\"alternate\" hreflang=\"fr\" href=\"https://www.test.com/fr/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.test.com/en/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"it\" href=\"https://www.test.com/it/accounts/profile\" /><changefreq>weekly</changefreq><priority>0.4</priority></url></urlset>";
            var parser = new XmlParser();
            var doc = parser.ParseDocument(xml);

            // Act
            var node = doc.DocumentElement.SelectSingleNode("/urlset/url/link");

            // Assert
            Assert.IsNotNull(node);
            Assert.That(node.NodeName, Is.EqualTo("xhtml:link"));
        }

        [Test]
        public void SelectSingleNode_DontIgnoreNamespaces_ShouldReturnNode()
        {
            // Arrange
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"><url><loc>https://www.test.com/de/accounts/profile</loc><xhtml:link rel=\"alternate\" hreflang=\"fr\" href=\"https://www.test.com/fr/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.test.com/en/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"it\" href=\"https://www.test.com/it/accounts/profile\" /><changefreq>weekly</changefreq><priority>0.4</priority></url></urlset>";
            var parser = new XmlParser();
            var doc = parser.ParseDocument(xml);
            var namespaceManager = new XmlNamespaceManager(new NameTable());

            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            namespaceManager.AddNamespace("d", "http://www.sitemaps.org/schemas/sitemap/0.9");

            // Act
            var node = doc.DocumentElement.SelectSingleNode("/d:urlset/d:url/xhtml:link", namespaceManager, false);

            // Assert
            Assert.IsNotNull(node);
            Assert.That(node.NodeName, Is.EqualTo("xhtml:link"));
        }

        [Test]
        public void SelectNodes_CanReturnAttribute()
        {
            // Arrange
            var html = "<!DOCTYPE html><html><body><div class=\"one\"><span class=\"two\">hello world</span></div></body></html>";
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(html);

            // Act
            var nodes = doc.DocumentElement.SelectNodes("//@*");

            // Assert
            Assert.IsNotNull(nodes);
            Assert.That(nodes, Has.Count.EqualTo(2));
            Assert.That(nodes, Is.All.InstanceOf<Dom.IAttr>());
        }

        [Test]
        public void TestNameXPathFunctionOnXMLDoc()
        {
            // Arrange
            var xml = @"<html><head><title>Test</title></head><body><h1>Test</h1></body></html>";
            var angleSharpXmlDoc = new XmlParser().ParseDocument(xml);

            // Act
            var xmlNav = angleSharpXmlDoc.CreateNavigator();

            // Assert
            Assert.AreEqual(TagNames.Html, xmlNav.Evaluate("name()"));
        }

        [Test]
        public void TestNameXPathFunctionOnHTMLDoc()
        {
            // Arrange
            var html = @"<html><head><title>Test</title></head><body><h1>Test</h1></body></html>";

            var angleSharpHtmlDoc = new HtmlParser().ParseDocument(html);

            // Act
            var htmlNav = angleSharpHtmlDoc.CreateNavigator();

            // Assert
            Assert.AreEqual(TagNames.Html, htmlNav.Evaluate("name()"));
        }

        [Test]
        public void MoveToParent_CallWhenCurrentNodeIsAttr_ShouldBeMovedToAttrOwnerElement()
        {
            // Arrange
            var xml = @"<root att1='value 1' att2='value 2'><child>foo</child></root>";
            var parser = new XmlParser();
            var doc = parser.ParseDocument(xml);
            var nav = doc.CreateNavigator(false);
            nav.MoveToChild("root", "");

            // Act

            if (nav.MoveToFirstAttribute())
            {
                do
                {
                    Assert.AreEqual(nav.NodeType, XPathNodeType.Attribute);
                }
                while (nav.MoveToNextAttribute());
                nav.MoveToParent();
            }

            // Assert
            Assert.AreEqual(nav.Name, "root");
        }

        [Test]
        public void SelectSingleNodeTest_AttributesOrder()
        {
            // Arrange
            const string html =
                @"<body>
			<div id='div1'>First</div>
			<div id='div2' class='mydiv'>Second</div>
			<div class='mydiv' id='div3'>Third</div>
		</body>";
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            // Act
            var div1 = document.DocumentElement.SelectSingleNode("//div[@id='div1']");
            var div2 = document.DocumentElement.SelectSingleNode("//div[@id='div2']");
            var div3 = document.DocumentElement.SelectSingleNode("//div[@id='div3']");

            // Assert
            Assert.That(div1, Is.Not.Null);
            Assert.That(div2, Is.Not.Null);
            Assert.That(div3, Is.Not.Null); // currently fails
        }

        [Test]
        public void Issue40()
        {
            var html = File.ReadAllText("40.html");
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);
            var node = document.Body.SelectSingleNode("(//table[@class='accountTable'])[2]//a/@data-miniprofile");
            var table = (((IAttr)node).OwnerElement).Ancestors<IHtmlTableElement>().Single();

            StringAssert.DoesNotContain("DEVICE NAME", table.Text());
            StringAssert.Contains("User Name", table.Text());
        }
    }
}
