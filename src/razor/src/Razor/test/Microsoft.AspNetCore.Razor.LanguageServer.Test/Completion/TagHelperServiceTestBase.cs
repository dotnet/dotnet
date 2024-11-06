// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public abstract class TagHelperServiceTestBase(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    protected static ImmutableArray<TagHelperDescriptor> DefaultTagHelpers => SimpleTagHelpers.Default;

    protected static string GetFileName(bool isRazorFile)
        => RazorCodeDocumentFactory.GetFileName(isRazorFile);

    protected static RazorCodeDocument CreateCodeDocument(string text, bool isRazorFile, params ImmutableArray<TagHelperDescriptor> tagHelpers)
        => RazorCodeDocumentFactory.CreateCodeDocument(text, isRazorFile, tagHelpers);

    protected static RazorCodeDocument CreateCodeDocument(string text, string filePath, params ImmutableArray<TagHelperDescriptor> tagHelpers)
        => RazorCodeDocumentFactory.CreateCodeDocument(text, filePath, tagHelpers);
}
