// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Utilities;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostSemanticTokensRangeEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Theory]
    [CombinatorialData]
    public async Task RazorComponents(bool colorBackground, bool miscellaneousFile)
    {
        var input = """
            <InputText Value="someValue" />
            <InputText Value="someValue"></InputText>
            <Microsoft.AspNetCore.Components.Forms.InputText Value="someValue" />
            <Microsoft.AspNetCore.Components.Forms.InputText Value="someValue"></Microsoft.AspNetCore.Components.Forms.InputText>

            @typeof(InputText).ToString()
            @typeof(Microsoft.AspNetCore.Components.Forms.InputText).ToString()
            """;

        await VerifySemanticTokensAsync(input, colorBackground, miscellaneousFile);
    }

    [Theory]
    [CombinatorialData]
    public async Task Razor(bool colorBackground, bool miscellaneousFile)
    {
        var input = """
            @page "/"
            @using System
            @using System.Diagnostics

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
                [DebuggerDisplay("{GetDebuggerDisplay,nq}")]
                public class MyClass
                {
                }
            
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

        await VerifySemanticTokensAsync(input, colorBackground, miscellaneousFile);
    }

    [Theory]
    [CombinatorialData]
    public async Task Legacy(bool colorBackground, bool miscellaneousFile)
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

        await VerifySemanticTokensAsync(input, colorBackground, miscellaneousFile, fileKind: RazorFileKind.Legacy);
    }

    [Theory]
    [CombinatorialData]
    public async Task Legacy_Compatibility(bool colorBackground, bool miscellaneousFile)
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

        await VerifySemanticTokensAsync(input, colorBackground, miscellaneousFile, fileKind: RazorFileKind.Legacy);
    }

    [Theory]
    [CombinatorialData]
    public async Task RenderMode(bool colorBackground, bool miscellaneousFile)
    {
        var input = """
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer

            <!-- above and below should be classified the same -->

            @{
                var r = Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveAuto;
            }
            """;

        await VerifySemanticTokensAsync(input, colorBackground, miscellaneousFile);
    }

    [Theory]
    [CombinatorialData]
    public async Task RenderMode_Razor9(bool colorBackground, bool miscellaneousFile)
    {
        var input = """
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer

            <!-- above and below should NOT be classified the same in Razor 9.0 -->

            @{
                var r = Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveAuto;
            }
            """;

        await VerifySemanticTokensAsync(
            input,
            colorBackground,
            miscellaneousFile,
            projectConfigure: p => p.RazorLanguageVersion = RazorLanguageVersion.Version_9_0);
    }

    [Theory]
    [CombinatorialData]
    public async Task RenderMode_Expression(bool colorBackground, bool miscellaneousFile)
    {
        var input = """
            @rendermode @(Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer)

            <!-- above and below should be classified the same -->

            @{
                var r = Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveAuto;
            }
            """;

        await VerifySemanticTokensAsync(input, colorBackground, miscellaneousFile);
    }

    private async Task VerifySemanticTokensAsync(
        string input,
        bool colorBackground,
        bool miscellaneousFile,
        RazorFileKind? fileKind = null,
        Action<RazorProjectBuilder>? projectConfigure = null,
        [CallerMemberName] string? testName = null)
    {
        var document = CreateProjectAndRazorDocument(input, fileKind, miscellaneousFile: miscellaneousFile, projectConfigure: projectConfigure);
        var sourceText = await document.GetTextAsync(DisposalToken);

        // We need to manually initialize the OOP service so we can get semantic token info later
        UpdateClientLSPInitializationOptions(options =>
        {
            return options with
            {
                TokenTypes = SemanticTokensLegendService.TokenTypes.All,
                TokenModifiers = SemanticTokensLegendService.TokenModifiers.All,
            };
        });

        var clientSettingsManager = ClientSettingsManager;
        clientSettingsManager.Update(ClientAdvancedSettings.Default with { ColorBackground = colorBackground });

        var endpoint = new CohostSemanticTokensRangeEndpoint(IncompatibleProjectService, RemoteServiceInvoker, clientSettingsManager, NoOpTelemetryReporter.Instance);

        var span = new LinePositionSpan(new(0, 0), new(sourceText.Lines.Count, 0));

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, span, DisposalToken);

        var actualFileContents = GetTestOutput(sourceText, result?.Data, SemanticTokensLegendService);

        if (colorBackground)
        {
            testName += "_with_background";
        }

        if (miscellaneousFile)
        {
            testName += "_misc_file";
        }

        var baselineFileName = Path.Combine("TestFiles", "SemanticTokens", $"{testName}.txt");
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

        return semanticFile.ReadAllText()
            // CI seems to not checkout with auto-crlf, so normalize to Environment.NewLine
            .Replace("\r\n", Environment.NewLine);
    }

    private static void WriteBaselineFile(string fileContents, string baselineFileName)
    {
        var projectPath = TestProject.GetProjectDirectory(typeof(CohostSemanticTokensRangeEndpointTest), layer: TestProject.Layer.Tooling);
        var baselineFileFullPath = Path.Combine(projectPath, baselineFileName);
        File.WriteAllText(baselineFileFullPath, fileContents);
    }

    private static string GetTestOutput(SourceText sourceText, int[]? data, ISemanticTokensLegendService legend)
    {
        if (data == null)
        {
            return string.Empty;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.AppendLine("Line Δ, Char Δ, Length, Type, Modifier(s), Text");
        var tokenTypes = legend.TokenTypes.All;
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
            var modifier = GetTokenModifierString(data[i + 4], legend);
            var text = sourceText.GetSubTextString(new TextSpan(sourceText.Lines[lineIndex].Start + lineOffset, length));
            builder.AppendLine($"{lineDelta} {charDelta} {length} {type} {modifier} [{text}]");

            prevLength = length;
        }

        return builder.ToString();
    }

    private static string GetTokenModifierString(int tokenModifiers, ISemanticTokensLegendService legend)
    {
        var modifiers = legend.TokenModifiers.All;

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
