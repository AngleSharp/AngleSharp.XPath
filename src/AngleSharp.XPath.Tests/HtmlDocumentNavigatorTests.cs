using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	}
}
