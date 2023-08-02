// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using System.Xml.Xsl;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger;

/// <summary>
/// Html transformer transforms the xml file to html file using xslt file.
/// </summary>
internal class HtmlTransformer : IHtmlTransformer
{
    private readonly XslCompiledTransform _xslTransform;

    /// <summary>
    /// The following function invokes the compiled transform and Loads the xslt file.
    /// </summary>
    public HtmlTransformer()
    {
        _xslTransform = new XslCompiledTransform();
        _xslTransform.Load(XmlReader.Create(GetType().Assembly.GetManifestResourceStream("Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.Html.xslt") ?? throw new InvalidOperationException()));
    }

    /// <summary>
    /// It transforms the xml file to html file.
    /// </summary>
    public void Transform(string xmlFile, string htmlFile)
    {

        // DCS happily serializes characters into character references that are not strictly valid XML,
        // for example &#xFFFF;. DCS will load them, but for XSL to load them here we need to pass it
        // a reader that we've configured to be tolerant of such references.
        using XmlReader xr = XmlReader.Create(xmlFile, new XmlReaderSettings() { CheckCharacters = false });
        using XmlWriter xw = XmlWriter.Create(htmlFile, new XmlWriterSettings() { CheckCharacters = false });

        _xslTransform.Transform(xr, xw);
    }
}
