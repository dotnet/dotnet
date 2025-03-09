// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Utilities;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostSemanticTokensRangeEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Theory]
    [CombinatorialData]
    public async Task Razor(bool colorBackground, bool precise)
    {
        var input = """
            @page "/"
            @using System

            <div>This is some HTML</div>

            <InputText Value="someValue" />

            @* hello there *@
            <!-- how are you? -->

            @if (true)
            {
                <text>Html!</text>
            }

            @code
            {
                // I am also good, thanks for asking

                /*
                    No problem.
                */

                private string someValue;

                public void M()
                {
                    RenderFragment x = @<div>This is some HTML in a render fragment</div>;
                }
            }
            """;

        await VerifySemanticTokensAsync(input, colorBackground, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task Legacy(bool colorBackground, bool precise)
    {
        var input = """
            @page "/"
            @model AppThing.Model
            @using System

            <div>This is some HTML</div>

            <component type="typeof(Component)" render-mode="ServerPrerendered" />

            @functions
            {
                public void M()
                {
                }
            }

            @section MySection {
                <div>Section content</div>
            }
            """;

        await VerifySemanticTokensAsync(input, colorBackground, precise, fileKind: FileKinds.Legacy);
    }

    [Theory]
    [CombinatorialData]
    public async Task Legacy_Compatibility(bool colorBackground, bool precise)
    {
        // Same test as above, but with only the things that work in FUSE and non-FUSE, to prevent regressions

        var input = """
            @page "/"
            @using System

            <div>This is some HTML</div>

            <component type="typeof(Component)" render-mode="ServerPrerendered" />

            @functions
            {
                public void M()
                {
                }
            }
            """;

        await VerifySemanticTokensAsync(input, colorBackground, precise, fileKind: FileKinds.Legacy);
    }

    private async Task VerifySemanticTokensAsync(string input, bool colorBackground, bool precise, string? fileKind = null, [CallerMemberName] string? testName = null)
    {
        var document = CreateProjectAndRazorDocument(input, fileKind);
        var sourceText = await document.GetTextAsync(DisposalToken);

        var legend = TestRazorSemanticTokensLegendService.Instance;

        // We need to manually initialize the OOP service so we can get semantic token info later
        UpdateClientLSPInitializationOptions(options => options with { TokenTypes = legend.TokenTypes.All, TokenModifiers = legend.TokenModifiers.All });

        // Update the client initialization options to control the precise ranges option
        UpdateClientInitializationOptions(c => c with { UsePreciseSemanticTokenRanges = precise });

        var clientSettingsManager = new ClientSettingsManager([], null, null);
        clientSettingsManager.Update(ClientAdvancedSettings.Default with { ColorBackground = colorBackground });

        var endpoint = new CohostSemanticTokensRangeEndpoint(RemoteServiceInvoker, clientSettingsManager, legend, NoOpTelemetryReporter.Instance);

        var span = new LinePositionSpan(new(0, 0), new(sourceText.Lines.Count, 0));

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, span, DisposalToken);

        var actualFileContents = GetTestOutput(sourceText, result?.Data);

        if (colorBackground)
        {
            testName += "_with_background";
        }

        var baselineFileName = $@"TestFiles\SemanticTokens\{testName}.txt";
        if (GenerateBaselines.ShouldGenerate)
        {
            WriteBaselineFile(actualFileContents, baselineFileName);
        }

        var expectedFileContents = GetBaselineFileContents(baselineFileName);
        AssertEx.EqualOrDiff(expectedFileContents, actualFileContents);
    }

    private string GetBaselineFileContents(string baselineFileName)
    {
        var semanticFile = TestFile.Create(baselineFileName, GetType().Assembly);
        if (!semanticFile.Exists())
        {
            return string.Empty;
        }

        return semanticFile.ReadAllText();
    }

    private static void WriteBaselineFile(string fileContents, string baselineFileName)
    {
        var projectPath = TestProject.GetProjectDirectory(typeof(CohostSemanticTokensRangeEndpointTest), layer: TestProject.Layer.Tooling);
        var baselineFileFullPath = Path.Combine(projectPath, baselineFileName);
        File.WriteAllText(baselineFileFullPath, fileContents);
    }

    private static string GetTestOutput(SourceText sourceText, int[]? data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.AppendLine("Line Δ, Char Δ, Length, Type, Modifier(s), Text");
        var tokenTypes = TestRazorSemanticTokensLegendService.Instance.TokenTypes.All;
        var prevLength = 0;
        var lineIndex = 0;
        var lineOffset = 0;
        for (var i = 0; i < data.Length; i += 5)
        {
            var lineDelta = data[i];
            var charDelta = data[i + 1];
            var length = data[i + 2];

            Assert.False(i != 0 && lineDelta == 0 && charDelta == 0, "line delta and character delta are both 0, which is invalid as we shouldn't be producing overlapping tokens");
            Assert.False(i != 0 && lineDelta == 0 && charDelta < prevLength, "Previous length is longer than char offset from previous start, meaning tokens will overlap");

            if (lineDelta != 0)
            {
                lineOffset = 0;
            }

            lineIndex += lineDelta;
            lineOffset += charDelta;

            var type = tokenTypes[data[i + 3]];
            var modifier = GetTokenModifierString(data[i + 4]);
            var text = sourceText.GetSubTextString(new TextSpan(sourceText.Lines[lineIndex].Start + lineOffset, length));
            builder.AppendLine($"{lineDelta} {charDelta} {length} {type} {modifier} [{text}]");

            prevLength = length;
        }

        return builder.ToString();
    }

    private static string GetTokenModifierString(int tokenModifiers)
    {
        var modifiers = TestRazorSemanticTokensLegendService.Instance.TokenModifiers.All;

        var modifiersBuilder = ArrayBuilder<string>.GetInstance();
        for (var i = 0; i < modifiers.Length; i++)
        {
            if ((tokenModifiers & (1 << (i % 32))) != 0)
            {
                modifiersBuilder.Add(modifiers[i]);
            }
        }

        return $"[{string.Join(", ", modifiersBuilder.ToArrayAndFree())}]";
    }
}
