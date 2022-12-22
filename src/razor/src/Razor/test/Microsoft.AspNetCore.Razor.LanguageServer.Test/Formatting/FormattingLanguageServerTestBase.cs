// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
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
        EmptyDocumentContextFactory = Mock.Of<DocumentContextFactory>(
            r => r.TryCreateAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()) == Task.FromResult<DocumentContext?>(null),
            MockBehavior.Strict);
    }

    internal static RazorCodeDocument CreateCodeDocument(string content, IReadOnlyList<SourceMapping> sourceMappings)
    {
        var sourceDocument = TestRazorSourceDocument.Create(content);
        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument, RazorParserOptions.CreateDefault());
        var razorCSharpDocument = RazorCSharpDocument.Create(
            content, RazorCodeGenerationOptions.CreateDefault(), Array.Empty<RazorDiagnostic>(), sourceMappings, Array.Empty<LinePragma>());
        codeDocument.SetSyntaxTree(syntaxTree);
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    internal static IOptionsMonitor<RazorLSPOptions> GetOptionsMonitor(bool enableFormatting)
    {
        var monitor = new Mock<IOptionsMonitor<RazorLSPOptions>>(MockBehavior.Strict);
        monitor.SetupGet(m => m.CurrentValue).Returns(new RazorLSPOptions(default, enableFormatting, true, insertSpaces: true, tabSize: 4));
        return monitor.Object;
    }

    internal class DummyRazorFormattingService : RazorFormattingService
    {
        public bool Called { get; private set; }

        public override Task<TextEdit[]> FormatAsync(DocumentContext documentContext, Range? range, FormattingOptions options, CancellationToken cancellationToken)
        {
            Called = true;
            return Task.FromResult(Array.Empty<TextEdit>());
        }

        public override Task<TextEdit[]> FormatCodeActionAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, CancellationToken cancellationToken)
        {
            return Task.FromResult(formattedEdits);
        }

        public override Task<TextEdit[]> FormatOnTypeAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, int hostDocumentIndex, char triggerCharacter, CancellationToken cancellationToken)
        {
            return Task.FromResult(formattedEdits);
        }

        public override Task<TextEdit[]> FormatSnippetAsync(DocumentContext documentContext, RazorLanguageKind kind, TextEdit[] formattedEdits, FormattingOptions options, CancellationToken cancellationToken)
        {
            return Task.FromResult(formattedEdits);
        }
    }
}
