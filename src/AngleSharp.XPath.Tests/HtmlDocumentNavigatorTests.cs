namespace AngleSharp.XPath.Tests
{
    using AngleSharp.Html.Parser;
    using NUnit.Framework;
    using System.Threading.Tasks;

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
	}
}
