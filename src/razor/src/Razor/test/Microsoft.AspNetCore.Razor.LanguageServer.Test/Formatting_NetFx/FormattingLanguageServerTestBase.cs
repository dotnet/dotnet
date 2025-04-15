// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public abstract class FormattingLanguageServerTestBase(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    internal static RazorCodeDocument CreateCodeDocument(string content, ImmutableArray<SourceMapping> sourceMappings)
    {
        var source = TestRazorSourceDocument.Create(content);
        var codeDocument = RazorCodeDocument.Create(source);
        var syntaxTree = RazorSyntaxTree.Parse(source, codeDocument.ParserOptions);
        var razorCSharpDocument = TestRazorCSharpDocument.Create(codeDocument, content, sourceMappings);
        codeDocument.SetSyntaxTree(syntaxTree);
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    internal class DummyRazorFormattingService(RazorLanguageKind? languageKind = null) : IRazorFormattingService
    {
        public bool Called { get; private set; }

        public Task<ImmutableArray<TextChange>> GetDocumentFormattingChangesAsync(DocumentContext documentContext, ImmutableArray<TextChange> htmlChanges, LinePositionSpan? span, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            Called = true;
            return SpecializedTasks.EmptyImmutableArray<TextChange>();
        }

        public Task<TextChange?> TryGetCSharpCodeActionEditAsync(DocumentContext documentContext, ImmutableArray<TextChange> formattedChanges, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ImmutableArray<TextChange>> GetCSharpOnTypeFormattingChangesAsync(DocumentContext documentContext, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        {
            Called = true;
            return SpecializedTasks.EmptyImmutableArray<TextChange>();
        }

        public Task<TextChange?> TryGetCSharpSnippetFormattingEditAsync(DocumentContext documentContext, ImmutableArray<TextChange> edits, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ImmutableArray<TextChange>> GetHtmlOnTypeFormattingChangesAsync(DocumentContext documentContext, ImmutableArray<TextChange> htmlChanges, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        {
            return Task.FromResult(htmlChanges);
        }

        public Task<TextChange?> TryGetSingleCSharpEditAsync(DocumentContext documentContext, TextChange initialEdit, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOnTypeFormattingTriggerKind(RazorCodeDocument codeDocument, int hostDocumentIndex, string triggerCharacter, out RazorLanguageKind triggerCharacterKind)
        {
            triggerCharacterKind = languageKind.GetValueOrDefault();
            return languageKind is not null;
        }
    }
}
