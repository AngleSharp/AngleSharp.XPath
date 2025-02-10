![logo](https://raw.githubusercontent.com/AngleSharp/AngleSharp.XPath/master/header.png)

# AngleSharp.XPath

[![Build Status](https://travis-ci.org/AngleSharp/AngleSharp.XPath.svg?branch=master&style=flat-square)](https://travis-ci.org/AngleSharp/AngleSharp.XPath)
[![GitHub Tag](https://img.shields.io/github/tag/AngleSharp/AngleSharp.XPath.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.XPath/releases)
[![NuGet Count](https://img.shields.io/nuget/dt/AngleSharp.XPath.svg?style=flat-square)](https://www.nuget.org/packages/AngleSharp.XPath/)
[![Issues Open](https://img.shields.io/github/issues/AngleSharp/AngleSharp.XPath.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.XPath/issues)
[![Gitter Chat](http://img.shields.io/badge/gitter-AngleSharp/AngleSharp-blue.svg?style=flat-square)](https://gitter.im/AngleSharp/AngleSharp)
[![StackOverflow Questions](https://img.shields.io/stackexchange/stackoverflow/t/anglesharp.svg?style=flat-square)](https://stackoverflow.com/tags/anglesharp)
[![CLA Assistant](https://cla-assistant.io/readme/badge/AngleSharp/AngleSharp.XPath?style=flat-square)](https://cla-assistant.io/AngleSharp/AngleSharp.XPath)

AngleSharp.XPath extends AngleSharp with the ability to select nodes via XPath queries instead of CSS selector syntax. This is more powerful and potentially more common for .NET developers working with XML on a daily basis.

## Basic Use

With this library using XPath queries is as simple as writing

```cs
var contentNode = document.Body.SelectSingleNode("//div[@id='content']");
```

Besides `SelectSingleNode` we can also use `SelectNodes`. Both are extension methods defined in the `AngleSharp.XPath` namespace.

If wanted we can also use XPath directly in CSS selectors such as in `QuerySelector` or `QuerySelectorAll` calls. For this we only need to apply the following configuration:

```cs
var config = Configuration.Default.WithXPath();
```

Now we can write queries such as

```cs
var secondLi = document.QuerySelector("*[xpath>'//li[2]']");
```

It is important that the original selector has all elements (`*`) as the intersection of the ordinary CSS selector and the XPath attribute is considered. The XPath attribute consists of a head (`xpath>`) and a value - provided as a string, e.g., `//li[2]`.

## Features

- Uses `XPathNavigator` from `System.Xml.XPath`
- Includes XPath capabilities to CSS query selectors if wanted

## Participating

Participation in the project is highly welcome. For this project the same rules as for the AngleSharp core project may be applied.

If you have any question, concern, or spot an issue then please report it before opening a pull request. An initial discussion is appreciated regardless of the nature of the problem.

Live discussions can take place in our [Gitter chat](https://gitter.im/AngleSharp/AngleSharp), which supports using GitHub accounts.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.

For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

## License

The MIT License (MIT)

Copyright (c) 2018 - 2025 Denis Ivanov, AngleSharp

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
