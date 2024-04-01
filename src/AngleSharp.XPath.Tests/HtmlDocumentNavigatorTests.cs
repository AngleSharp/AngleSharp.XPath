using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Xml.Parser;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace AngleSharp.XPath.Tests;

[TestFixture]
internal sealed class HtmlDocumentNavigatorTests
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
        content.Should().NotBeNull();
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
        nodes.Should().HaveCount(3);
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
        node.Should().NotBeNull();
    }

    [Test]
    public void SelectSingleNode_IgnoreNamespaces_ShouldReturnNode()
    {
        // Arrange
        const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"><url><loc>https://www.test.com/de/accounts/profile</loc><xhtml:link rel=\"alternate\" hreflang=\"fr\" href=\"https://www.test.com/fr/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.test.com/en/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"it\" href=\"https://www.test.com/it/accounts/profile\" /><changefreq>weekly</changefreq><priority>0.4</priority></url></urlset>";
        var parser = new XmlParser();
        var doc = parser.ParseDocument(xml);

        // Act
        var node = doc.DocumentElement.SelectSingleNode("/urlset/url/link");

        // Assert
        node.Should().NotBeNull();
        node.NodeName.Should().Be("xhtml:link");
    }

    [Test]
    public void SelectSingleNode_DontIgnoreNamespaces_ShouldReturnNode()
    {
        // Arrange
        const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"><url><loc>https://www.test.com/de/accounts/profile</loc><xhtml:link rel=\"alternate\" hreflang=\"fr\" href=\"https://www.test.com/fr/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.test.com/en/accounts/profile\" /><xhtml:link rel=\"alternate\" hreflang=\"it\" href=\"https://www.test.com/it/accounts/profile\" /><changefreq>weekly</changefreq><priority>0.4</priority></url></urlset>";
        var parser = new XmlParser();
        var doc = parser.ParseDocument(xml);
        var namespaceManager = new XmlNamespaceManager(new NameTable());

        namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
        namespaceManager.AddNamespace("d", "http://www.sitemaps.org/schemas/sitemap/0.9");

        // Act
        var node = doc.DocumentElement.SelectSingleNode("/d:urlset/d:url/xhtml:link", namespaceManager, false);

        // Assert
        node.Should().NotBeNull();
        node.NodeName.Should().Be("xhtml:link");
    }

    [Test]
    public void SelectNodes_CanReturnAttribute()
    {
        // Arrange
        const string html = "<!DOCTYPE html><html><body><div class=\"one\"><span class=\"two\">hello world</span></div></body></html>";
        var parser = new HtmlParser();
        var doc = parser.ParseDocument(html);

        // Act
        var nodes = doc.DocumentElement.SelectNodes("//@*");

        // Assert
        nodes.Should().HaveCount(2);
        nodes.Should().AllBeAssignableTo<IAttr>();
    }

    [Test]
    public void TestNameXPathFunctionOnXMLDoc()
    {
        // Arrange
        const string xml = @"<html><head><title>Test</title></head><body><h1>Test</h1></body></html>";
        var angleSharpXmlDoc = new XmlParser().ParseDocument(xml);

        // Act
        var xmlNav = angleSharpXmlDoc.CreateNavigator();

        // Assert
        xmlNav.Evaluate("name()").Should().Be(TagNames.Html);
    }

    [Test]
    public void TestNameXPathFunctionOnHTMLDoc()
    {
        // Arrange
        const string html = @"<html><head><title>Test</title></head><body><h1>Test</h1></body></html>";

        var angleSharpHtmlDoc = new HtmlParser().ParseDocument(html);

        // Act
        var htmlNav = angleSharpHtmlDoc.CreateNavigator();

        // Assert
        htmlNav.Evaluate("name()").Should().Be(TagNames.Html);
    }

    [Test]
    public void MoveToParent_CallWhenCurrentNodeIsAttr_ShouldBeMovedToAttrOwnerElement()
    {
        // Arrange
        const string xml = @"<root att1='value 1' att2='value 2'><child>foo</child></root>";
        var parser = new XmlParser();
        var doc = parser.ParseDocument(xml);
        var nav = doc.CreateNavigator(false);
        nav.MoveToChild("root", "");

        // Act

        if (nav.MoveToFirstAttribute())
        {
            do
            {
                nav.NodeType.Should().Be(XPathNodeType.Attribute);
            }
            while (nav.MoveToNextAttribute());
            nav.MoveToParent();
        }

        // Assert
        nav.Name.Should().Be("root");
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
        div1.Should().NotBeNull();
        div2.Should().NotBeNull();
        div3.Should().NotBeNull();
    }
}
