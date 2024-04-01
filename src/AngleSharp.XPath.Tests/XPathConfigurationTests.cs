using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AngleSharp.XPath.Tests;

[TestFixture]
internal sealed class XPathConfigurationTests
{
    [Test]
    public async Task RunXPathQueryFromDocumentWithSelectAll()
    {
        const string source = "<body><ul><li><li><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        var elements = document.QuerySelectorAll("*[xpath>'//li']");
        elements.Should().HaveCount(3);
    }

    [Test]
    public async Task RunXPathQueryFromDocumentWithSelectSingle()
    {
        const string source = "<body><ul><li><li><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        var element = document.QuerySelector("*[xpath>'//li']");
        element.Should().NotBeNull();
        element.TagName.Should().Be("LI");
    }

    [Test]
    public async Task RunXPathQueryFromDocumentWithSelectSingleSpecialN()
    {
        const string source = "<body><ul><li><li class=two><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        var element = document.QuerySelector("*[xpath>'//li[2]']");
        element.Should().NotBeNull();
        element.TagName.Should().Be("LI");
        element.ClassName.Should().Be("two");
    }
}
