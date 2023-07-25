// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public abstract class FormattingLanguageServerTestBase : LanguageServerTestBase
{
    internal DocumentContextFactory EmptyDocumentContextFactory { get; }

    public FormattingLanguageServerTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        EmptyDocumentContextFactory = new TestDocumentContextFactory();
    }

    internal static RazorCodeDocument CreateCodeDocument(string content, IReadOnlyList<SourceMapping> sourceMappings)
    {
        var sourceDocument = TestRazorSourceDocument.Create(content);
        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument, RazorParserOptions.CreateDefault());
        var razorCSharpDocument = RazorCSharpDocument.Create(
            codeDocument, content, RazorCodeGenerationOptions.CreateDefault(), Array.Empty<RazorDiagnostic>(), sourceMappings, Array.Empty<LinePragma>());
        codeDocument.SetSyntaxTree(syntaxTree);
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    internal class DummyRazorFormattingService : IRazorFormattingService
    {
        public bool Called { get; private set; }

        public Task<TextEdit[]> FormatAsync(VersionedDocumentContext documentContext, Range? range, FormattingOptions options, CancellationToken cancellationToken)
        {
            Called = true;
            return Task.FromResult(Array.Empty<TextEdit>());
        }

        public Task<TextEdit[]> FormatCodeActionAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, CancellationToken cancellationToken)
        {
            return Task.FromResult(formattedEdits);
        }

        public Task<TextEdit[]> FormatOnTypeAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        {
            return Task.FromResult(formattedEdits);
        }

        public Task<TextEdit[]> FormatSnippetAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, CancellationToken cancellationToken)
        {
            return Task.FromResult(formattedEdits);
        }
    }
}
