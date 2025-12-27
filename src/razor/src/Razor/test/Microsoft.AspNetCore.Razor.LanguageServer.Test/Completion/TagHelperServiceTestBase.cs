// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public abstract class TagHelperServiceTestBase(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    protected static TagHelperCollection DefaultTagHelpers => SimpleTagHelpers.Default;

    protected static string GetFileName(bool isRazorFile)
        => RazorCodeDocumentFactory.GetFileName(isRazorFile);

    protected static RazorCodeDocument CreateCodeDocument(string text, bool isRazorFile, params TagHelperCollection tagHelpers)
        => RazorCodeDocumentFactory.CreateCodeDocument(text, isRazorFile, tagHelpers);

    protected static RazorCodeDocument CreateCodeDocument(string text, string filePath, params TagHelperCollection tagHelpers)
        => RazorCodeDocumentFactory.CreateCodeDocument(text, filePath, tagHelpers);
}
