#region License
// MIT License
//
// Copyright (c) 2018 Denis Ivanov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using AngleSharp.Parser.Html;
using NUnit.Framework;

namespace AngleSharp.XPath.Tests
{
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
            var document = parser.Parse("<html></html>");
            var navigable = new HtmlDocumentNavigable(document);

            // Act
            var navigator = navigable.CreateNavigator();

            // Assert
            Assert.That(navigator, Is.TypeOf<HtmlDocumentNavigator>());
        }
    }
}
