using NUnit.Framework;
using AngleSharp.Css.Parser;
using System.Threading;
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

    [Test]
    public async Task RunXPathQueryAgainAfterAppendingAnElement()
    {
        const string source = "<body><ul><li><li><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        document.QuerySelectorAll("*[xpath>'//li']").Length.ShouldBe(3);
        document.QuerySelector("ul").AppendChild(document.CreateElement("li"));
        document.QuerySelectorAll("*[xpath>'//li']").Length.ShouldBe(4);
    }

    [Test]
    public async Task RunXPathQueryAgainAfterChangingAnAttribute()
    {
        const string source = "<body><ul><li><li class=two><li></ul>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        document.QuerySelectorAll("*[xpath>'//li[@class=\"two\"]']").Length.ShouldBe(1);
        document.QuerySelector("li").ClassName = "two";
        document.QuerySelectorAll("*[xpath>'//li[@class=\"two\"]']").Length.ShouldBe(2);
    }

    [Test]
    public async Task RunXPathQueryMatchesOnlyTheSelectedOfLookAlikeElements()
    {
        const string source = "<body><p></p><p></p>";
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(res => res.Content(source));
        var elements = document.QuerySelectorAll("*[xpath>'(//p)[1]']");
        elements.Length.ShouldBe(1);
        elements[0].ShouldBeSameAs(document.QuerySelector("p"));
    }

    [Test]
    public async Task RunXPathQueryOnTwoDocumentsInParallel()
    {
        const int rounds = 200;
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var first = await context.OpenAsync(res => res.Content("<body><ul><li><li><li></ul>"));
        var second = await context.OpenAsync(res => res.Content("<body><ul><li><li></ul>"));
        using var barrier = new Barrier(2);

        int[] Query(Dom.IDocument document)
        {
            var counts = new int[rounds];

            for (var i = 0; i < rounds; i++)
            {
                barrier.SignalAndWait();
                counts[i] = document.QuerySelectorAll("*[xpath>'//li']").Length;
            }

            return counts;
        }

        var results = await Task.WhenAll(Task.Run(() => Query(first)), Task.Run(() => Query(second)));
        results[0].ShouldAllBe(count => count == 3);
        results[1].ShouldAllBe(count => count == 2);
    }

    [Test]
    public void XPathSelectorTextIsTheSelectorAsWritten()
    {
        var config = Configuration.Default.WithXPath();
        var context = BrowsingContext.New(config);
        var parser = context.GetService<ICssSelectorParser>();
        parser.ParseSelector("*[xpath>'//li']").Text.ShouldBe("*[xpath>'//li']");
    }
}
