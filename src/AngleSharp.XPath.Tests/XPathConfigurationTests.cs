using NUnit.Framework;
using System.Threading.Tasks;
using Shouldly;

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
        elements.Length.ShouldBe(3);
    }

    [Test]
    public async Task RunXPathQueryFromDocumentWithSelectSingle()
    {
        const string source = "<body><ul><li><li><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        var element = document.QuerySelector("*[xpath>'//li']");
        element.ShouldNotBeNull();
        element.TagName.ShouldBe("LI");
    }

    [Test]
    public async Task RunXPathQueryFromDocumentWithSelectSingleSpecialN()
    {
        const string source = "<body><ul><li><li class=two><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        var element = document.QuerySelector("*[xpath>'//li[2]']");
        element.ShouldNotBeNull();
        element.TagName.ShouldBe("LI");
        element.ClassName.ShouldBe("two");
    }
}
