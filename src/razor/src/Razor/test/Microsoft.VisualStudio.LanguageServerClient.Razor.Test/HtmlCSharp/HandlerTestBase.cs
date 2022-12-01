// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

public abstract class HandlerTestBase : TestBase
{
    private protected TestLoggerProvider LoggerProvider { get; }

    protected HandlerTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        LoggerProvider = new();
    }

    internal static RazorCodeDocument CreateCodeDocument(
        string text,
        string filePath,
        params TagHelperDescriptor[] tagHelpers)
    {
        tagHelpers ??= Array.Empty<TagHelperDescriptor>();
        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder => { });
        var fileKind = filePath.EndsWith(".razor", StringComparison.Ordinal) ? FileKinds.Component : FileKinds.Legacy;
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, Array.Empty<RazorSourceDocument>(), tagHelpers);

        return codeDocument;
    }

    internal static CSharpVirtualDocumentSnapshot CreateCSharpVirtualDocumentSnapshot(
        RazorCodeDocument codeDocument,
        string virtualDocumentPath,
        long? hostDocumentSyncVersion = 1)
    {
        var textSnapshot = new StringTextSnapshot(codeDocument.GetCSharpDocument().GeneratedCode);
        textSnapshot.TextBuffer = new TestTextBuffer(textSnapshot);
        var virtualDocumentUri = new Uri(virtualDocumentPath);
        var virtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(virtualDocumentUri, textSnapshot, hostDocumentSyncVersion);

        return virtualDocumentSnapshot;
    }
}
