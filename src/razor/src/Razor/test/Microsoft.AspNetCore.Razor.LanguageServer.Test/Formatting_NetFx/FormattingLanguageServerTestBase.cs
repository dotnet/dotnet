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
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit.Abstractions;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public abstract class FormattingLanguageServerTestBase(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    internal static RazorCodeDocument CreateCodeDocument(string content, ImmutableArray<SourceMapping> sourceMappings)
    {
        var sourceDocument = TestRazorSourceDocument.Create(content);
        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument, RazorParserOptions.CreateDefault());
        var razorCSharpDocument = new RazorCSharpDocument(
            codeDocument, content, RazorCodeGenerationOptions.Default, diagnostics: [], sourceMappings, linePragmas: []);
        codeDocument.SetSyntaxTree(syntaxTree);
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    internal class DummyRazorFormattingService : IRazorFormattingService
    {
        public bool Called { get; private set; }

        public Task<TextEdit[]> GetDocumentFormattingEditsAsync(DocumentContext documentContext, TextEdit[] htmlEdits, Range? range, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            Called = true;
            return SpecializedTasks.EmptyArray<TextEdit>();
        }

        public Task<TextEdit?> GetCSharpCodeActionEditAsync(DocumentContext documentContext, TextEdit[] formattedEdits, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TextEdit[]> GetCSharpOnTypeFormattingEditsAsync(DocumentContext documentContext, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TextEdit?> GetCSharpSnippetFormattingEditAsync(DocumentContext documentContext, TextEdit[] edits, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TextEdit[]> GetHtmlOnTypeFormattingEditsAsync(DocumentContext documentContext, TextEdit[] htmlEdits, RazorFormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        {
            return Task.FromResult(htmlEdits);
        }

        public Task<TextEdit?> GetSingleCSharpEditAsync(DocumentContext documentContext, TextEdit initialEdit, RazorFormattingOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
