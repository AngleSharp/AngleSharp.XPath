namespace AngleSharp.XPath.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class XPathNSConfigurationTests
    {
        [Test]
        public async Task RunXPathQueryFromDocumentWithSelectAll()
        {
            var source = @"<body xmlns='http://www.w3.org/1999/xhtml'><ul><li><li><li></ul>";
            var config = Configuration.Default.WithXPath();

            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(res => res.Content(source));
            var elements = document.QuerySelectorAll("*[xpathns>'//*[local-name()=\\'li\\' and namespace-uri()=\\'http://www.w3.org/1999/xhtml\\']']");
            Assert.AreEqual(3, elements.Length);
        }

        [Test]
        public async Task RunXPathQueryFromDocumentWithSelectSingle()
        {
            var source = @"<body><ul><li><li><li></ul>";
            var config = Configuration.Default.WithXPath();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(res => res.Content(source));
            var element = document.QuerySelector("*[xpathns>'//*[local-name()=\\'li\\' and namespace-uri()=\\'http://www.w3.org/1999/xhtml\\']']");
            Assert.IsNotNull(element);
            Assert.AreEqual("LI", element.TagName);
        }

        [Test]
        public async Task RunXPathQueryFromDocumentWithSelectSingleSpecialN()
        {
            var source = @"<body><ul><li><li class=two><li></ul>";
            var config = Configuration.Default.WithXPath();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(res => res.Content(source));
            var element = document.QuerySelector("*[xpathns>'//*[local-name()=\\'li\\' and namespace-uri()=\\'http://www.w3.org/1999/xhtml\\'][2]']");
            Assert.IsNotNull(element);
            Assert.AreEqual("LI", element.TagName);
            Assert.AreEqual("two", element.ClassName);
        }


    }
}
