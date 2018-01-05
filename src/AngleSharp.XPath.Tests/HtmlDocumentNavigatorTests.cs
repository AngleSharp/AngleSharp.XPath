using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using AngleSharp.XPath;

namespace AngleSharp.XPath.Tests
{
	[TestFixture]
	public class HtmlDocumentNavigatorTests
	{
		[Test, Retry(5)]
		public async Task SelectSinleNodeTest()
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
			var document = parser.Parse(html);

			// Act
			var nodes = document.DocumentElement.SelectNodes("//li");

			// Assert
			Assert.That(nodes, Has.Count.EqualTo(3));
		}
	}
}
