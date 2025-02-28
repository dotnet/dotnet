// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

public abstract class RazorOnAutoInsertProviderTestBase : LanguageServerTestBase
{
    protected RazorOnAutoInsertProviderTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    internal abstract IOnAutoInsertProvider CreateProvider();

    protected void RunAutoInsertTest(string input, string expected, int tabSize = 4, bool insertSpaces = true, bool enableAutoClosingTags = true, string fileKind = default, IReadOnlyList<TagHelperDescriptor> tagHelpers = default)
    {
        // Arrange
        TestFileMarkupParser.GetPosition(input, out input, out var location);

        var source = SourceText.From(input);
        var position = source.GetPosition(location);

        var path = "file:///path/to/document.razor";
        var uri = new Uri(path);
        var codeDocument = CreateCodeDocument(source, uri.AbsolutePath, tagHelpers, fileKind: fileKind);

        var provider = CreateProvider();

        // Act
        provider.TryResolveInsertion(position, codeDocument, enableAutoClosingTags: enableAutoClosingTags, out var edit);

        // Assert
        var edited = edit is null ? source : ApplyEdit(source, edit.TextEdit);
        var actual = edited.ToString();
        Assert.Equal(expected, actual);
    }

    private static SourceText ApplyEdit(SourceText source, TextEdit edit)
    {
        var change = source.GetTextChange(edit);
        return source.WithChanges(change);
    }

    private static RazorCodeDocument CreateCodeDocument(SourceText text, string path, IReadOnlyList<TagHelperDescriptor> tagHelpers = null, string fileKind = default)
    {
        fileKind ??= FileKinds.Component;
        tagHelpers ??= Array.Empty<TagHelperDescriptor>();
        var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(path, path));
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, importSources: default, tagHelpers);
        return codeDocument;
    }
}
