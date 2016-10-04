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
		[Test]
		public async Task SelectSinleNodeTest()
		{
			var config = Configuration.Default.WithDefaultLoader();
			var address = "https://stackoverflow.com/questions/39471800/is-anglesharps-htmlparser-threadsafe";
			var document = await BrowsingContext.New(config).OpenAsync(address);

			var content = document.DocumentElement.SelectSingleNode("//div[@id='content']");
			Assert.That(content, Is.Not.Null);
		}
	}
}