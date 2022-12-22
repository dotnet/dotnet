// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger;

public interface IHtmlTransformer
{
    /// <summary>
    /// It transforms the xml file to html file.
    /// </summary>
    /// <param name="xmlFile"></param>
    /// <param name="htmlFile"></param>
    void Transform(string xmlFile, string htmlFile);
}
