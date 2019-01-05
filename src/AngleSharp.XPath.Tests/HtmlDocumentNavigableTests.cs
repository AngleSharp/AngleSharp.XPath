namespace AngleSharp.XPath.Tests
{
    using AngleSharp.Html.Parser;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class HtmlDocumentNavigableTests
    {
        [Test]
        public void Ctor_NullDocumentArgument_DoesThrowException()
        {
            // Arrange
            
            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => new HtmlDocumentNavigable(null));
        }

        [Test]
        public void CreateNavigator_Call_ShouldReturnHtmlDocumentNavigator()
        {
            // Arrange
            var parser = new HtmlParser();
            var document = parser.ParseDocument("<html></html>");
            var navigable = new HtmlDocumentNavigable(document);

            // Act
            var navigator = navigable.CreateNavigator();

            // Assert
            Assert.That(navigator, Is.TypeOf<HtmlDocumentNavigator>());
        }
    }
}
