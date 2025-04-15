// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

public class RazorMappingServiceTest(ITestOutputHelper testOutput) : WorkspaceTestBase(testOutput)
{
    private readonly HostProject _hostProject = TestProjectData.SomeProject;
    private readonly HostDocument _hostDocument = TestProjectData.SomeProjectFile1;

    [Fact]
    public async Task TryGetMappedSpans_SpanMatchesSourceMapping_ReturnsTrue()
    {
        // Arrange
        var sourceText = SourceText.From(@"
@SomeProperty
");

        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddDocument(_hostDocument, TestMocks.CreateTextLoader(sourceText));
        var project = new ProjectSnapshot(state);

        var document = project.GetRequiredDocument(_hostDocument.FilePath);

        var output = await document.GetGeneratedOutputAsync(DisposalToken);
        var generated = output.GetCSharpDocument();

        var symbol = "SomeProperty";
        var generatedCode = generated.Text.ToString();
        var span = new TextSpan(generatedCode.IndexOf(symbol, StringComparison.Ordinal), symbol.Length);

        // Act
        var text = await document.GetTextAsync(DisposalToken);
        var result = RazorMappingService.TryGetMappedSpans(span, text, generated, out var mappedLinePositionSpan, out var mappedSpan);

        // Assert
        Assert.True(result);
        Assert.Equal(new LinePositionSpan(new LinePosition(1, 1), new LinePosition(1, 13)), mappedLinePositionSpan);
        Assert.Equal(new TextSpan(Environment.NewLine.Length + 1, symbol.Length), mappedSpan);
    }

    [Fact]
    public async Task TryGetMappedSpans_SpanMatchesSourceMappingAndPosition_ReturnsTrue()
    {
        // Arrange
        var code = @"
@SomeProperty
@SomeProperty
@SomeProperty
";

        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddDocument(_hostDocument, TestMocks.CreateTextLoader(code));
        var project = new ProjectSnapshot(state);

        var document = project.GetRequiredDocument(_hostDocument.FilePath);

        var output = await document.GetGeneratedOutputAsync(DisposalToken);
        var generated = output.GetCSharpDocument();

        var symbol = "SomeProperty";
        // Second occurrence
        var generatedCode = generated.Text.ToString();
        var span = new TextSpan(generatedCode.IndexOf(symbol, generatedCode.IndexOf(symbol, StringComparison.Ordinal) + symbol.Length, StringComparison.Ordinal), symbol.Length);

        // Act
        var text = await document.GetTextAsync(DisposalToken);
        var result = RazorMappingService.TryGetMappedSpans(span, text, generated, out var mappedLinePositionSpan, out var mappedSpan);

        // Assert
        Assert.True(result);
        Assert.Equal(new LinePositionSpan(new LinePosition(2, 1), new LinePosition(2, 13)), mappedLinePositionSpan);
        Assert.Equal(new TextSpan(Environment.NewLine.Length + 1 + symbol.Length + Environment.NewLine.Length + 1, symbol.Length), mappedSpan);
    }

    [Fact]
    public async Task TryGetMappedSpans_SpanWithinSourceMapping_ReturnsTrue()
    {
        // Arrange
        var code = @"
@{
    var x = SomeClass.SomeProperty;
}
";

        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddDocument(_hostDocument, TestMocks.CreateTextLoader(code));
        var project = new ProjectSnapshot(state);

        var document = project.GetRequiredDocument(_hostDocument.FilePath);

        var output = await document.GetGeneratedOutputAsync(DisposalToken);
        var generated = output.GetCSharpDocument();

        var symbol = "SomeProperty";
        var generatedCode = generated.Text.ToString();
        var span = new TextSpan(generatedCode.IndexOf(symbol, StringComparison.Ordinal), symbol.Length);

        // Act
        var text = await document.GetTextAsync(DisposalToken);
        var result = RazorMappingService.TryGetMappedSpans(span, text, generated, out var mappedLinePositionSpan, out var mappedSpan);

        // Assert
        Assert.True(result);
        Assert.Equal(new LinePositionSpan(new LinePosition(2, 22), new LinePosition(2, 34)), mappedLinePositionSpan);
        Assert.Equal(new TextSpan(Environment.NewLine.Length + 2 + Environment.NewLine.Length + "    var x = SomeClass.".Length, symbol.Length), mappedSpan);
    }

    [Fact]
    public async Task TryGetMappedSpans_SpanOutsideSourceMapping_ReturnsFalse()
    {
        // Arrange
        var code = @"
@{
    var x = SomeClass.SomeProperty;
}
";

        var state = ProjectState
            .Create(_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddDocument(_hostDocument, TestMocks.CreateTextLoader(code));
        var project = new ProjectSnapshot(state);

        var document = project.GetRequiredDocument(_hostDocument.FilePath);

        var output = await document.GetGeneratedOutputAsync(DisposalToken);
        var generated = output.GetCSharpDocument();

        var symbol = "ExecuteAsync";
        var generatedCode = generated.Text.ToString();
        var span = new TextSpan(generatedCode.IndexOf(symbol, StringComparison.Ordinal), symbol.Length);

        // Act
        var text = await document.GetTextAsync(DisposalToken);
        var result = RazorMappingService.TryGetMappedSpans(span, text, generated, out _, out _);

        // Assert
        Assert.False(result);
    }
}
