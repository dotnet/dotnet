// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class DefaultRazorCSharpLoweringPhaseTest
{
    [Fact]
    public void Execute_ThrowsForMissingDependency_IRDocument()
    {
        // Arrange
        var phase = new DefaultRazorCSharpLoweringPhase();

        var engine = RazorProjectEngine.CreateEmpty(b => b.Phases.Add(phase));

        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(codeDocument.Source));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => phase.Execute(codeDocument));

        Assert.Equal(
            $"The '{nameof(DefaultRazorCSharpLoweringPhase)}' phase requires a '{nameof(DocumentIntermediateNode)}' " +
            $"provided by the '{nameof(RazorCodeDocument)}'.",
             exception.Message);
    }

    [Fact]
    public void Execute_ThrowsForMissingDependency_CodeTarget()
    {
        // Arrange
        var phase = new DefaultRazorCSharpLoweringPhase();

        var engine = RazorProjectEngine.CreateEmpty(b => b.Phases.Add(phase));

        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(codeDocument.Source));

        var irDocument = new DocumentIntermediateNode()
        {
            DocumentKind = "test",
        };
        codeDocument.SetDocumentIntermediateNode(irDocument);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => phase.Execute(codeDocument));
        Assert.Equal(
            $"The document of kind 'test' does not have a '{nameof(CodeTarget)}'. " +
            $"The document classifier must set a value for '{nameof(DocumentIntermediateNode.Target)}'.",
            exception.Message);
    }

    [Fact]
    public void Execute_CollatesIRDocumentDiagnosticsFromSourceDocument()
    {
        // Arrange
        var phase = new DefaultRazorCSharpLoweringPhase();
        var engine = RazorProjectEngine.CreateEmpty(b => b.Phases.Add(phase));
        var codeDocument = TestRazorCodeDocument.Create("<p class=@(");
        var options = RazorCodeGenerationOptions.Default;
        var irDocument = new DocumentIntermediateNode()
        {
            DocumentKind = "test",
            Target = CodeTarget.CreateDefault(codeDocument, options),
            Options = options,
        };
        var expectedDiagnostic = RazorDiagnostic.Create(
                new RazorDiagnosticDescriptor("1234", "I am an error.", RazorDiagnosticSeverity.Error),
                new SourceSpan("SomeFile.cshtml", 11, 0, 11, 1));
        irDocument.Diagnostics.Add(expectedDiagnostic);
        codeDocument.SetDocumentIntermediateNode(irDocument);

        // Act
        phase.Execute(codeDocument);

        // Assert
        var csharpDocument = codeDocument.GetCSharpDocument();
        var diagnostic = Assert.Single(csharpDocument.Diagnostics);
        Assert.Same(expectedDiagnostic, diagnostic);
    }
}
