// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Text.Adornments;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostFindAllReferencesEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public Task FindCSharpMember()
        => VerifyFindAllReferencesAsync("""
            @{
                string M()
                {
                    return [|MyName|];
                }
            }

            <p>@[|MyName|]</p>

            @code {
                private const string [|$$MyName|] = "David";
            }
            """);

    [Fact]
    public async Task ComponentAttribute()
    {
        TestCode input = """
            <SurveyPrompt [|Ti$$tle|]="InputValue" />
            """;

        TestCode surveyPrompt = """
            @namespace SomeProject

            <div></div>

            @code
            {
                [Parameter]
                public string [|Title|] { get; set; }
            }
            """;

        await VerifyFindAllReferencesAsync(input,
            (FilePath("SurveyPrompt.razor"), surveyPrompt));
    }

    [Fact]
    public async Task OtherCSharpFile()
    {
        TestCode input = """
            @code
            {
                public void M()
                {
                    var x = new OtherClass();
                    x.[|D$$ate|].ToString();
                }
            }
            """;

        TestCode otherClass = """
            using System;

            namespace SomeProject;

            public class OtherClass
            {
                public DateTime [|Date|] => DateTime.Now;
            }
            """;

        await VerifyFindAllReferencesAsync(input,
            (FilePath("OtherClass.cs"), otherClass));
    }

    [Fact]
    public async Task Component_DefinedInCSharp()
    {
        TestCode input = """
            <[|Surv$$eyPrompt|] Title="InputValue" />
            """;

        // lang=c#-test
        TestCode surveyPrompt = """
            using Microsoft.AspNetCore.Components;
            using Microsoft.AspNetCore.Components.Rendering;

            namespace SomeProject;

            public class [|SurveyPrompt|] : ComponentBase
            {
                [Parameter]
                public string Title { get; set; } = "Hello";

                protected override void BuildRenderTree(RenderTreeBuilder builder)
                {
                    builder.OpenElement(0, "div");
                    builder.AddContent(1, Title + " from a C#-defined component!");
                    builder.CloseElement();
                }
            }
            """;

        await VerifyFindAllReferencesAsync(input,
            (FilePath("SurveyPrompt.cs"), surveyPrompt));
    }

    [Fact]
    public async Task ComponentEndTag_DefinedInCSharp()
    {
        TestCode input = """
            <[|SurveyPrompt|] Title="InputValue"></Surv$$eyPrompt>
            """;

        // lang=c#-test
        TestCode surveyPrompt = """
            using Microsoft.AspNetCore.Components;
            using Microsoft.AspNetCore.Components.Rendering;

            namespace SomeProject;

            public class [|SurveyPrompt|] : ComponentBase
            {
                [Parameter]
                public string Title { get; set; } = "Hello";

                protected override void BuildRenderTree(RenderTreeBuilder builder)
                {
                    builder.OpenElement(0, "div");
                    builder.AddContent(1, Title + " from a C#-defined component!");
                    builder.CloseElement();
                }
            }
            """;

        await VerifyFindAllReferencesAsync(input,
            (FilePath("SurveyPrompt.cs"), surveyPrompt));
    }

    [Fact]
    public async Task Component_FromRazor()
    {
        TestCode input = """
            <[|Sur$$veyPrompt|] Title="InputValue" />

            @nameof([|SurveyPrompt|])
            """;

        TestCode surveyPrompt = """
            [||]<div>
            </div>
            """;

        await VerifyFindAllReferencesAsync(input,
            (FilePath("SurveyPrompt.razor"), surveyPrompt));
    }

    [Fact]
    public async Task Component_FromCSharp()
    {
        TestCode input = """
            <[|SurveyPrompt|] Title="InputValue" />

            @nameof([|Survey$$Prompt|])
            """;

        TestCode surveyPrompt = """
            [||]<div>
            </div>
            """;

        await VerifyFindAllReferencesAsync(input,
            (FilePath("SurveyPrompt.razor"), surveyPrompt));
    }

    private async Task VerifyFindAllReferencesAsync(TestCode input, params (string fileName, TestCode testCode)[] additionalFiles)
    {
        var document = CreateProjectAndRazorDocument(input.Text, additionalFiles: [.. additionalFiles.Select(f => (f.fileName, f.testCode.Text))]);
        var inputText = await document.GetTextAsync(DisposalToken);
        var position = inputText.GetPosition(input.Position);

        var endpoint = new CohostFindAllReferencesEndpoint(IncompatibleProjectService, RemoteServiceInvoker);

        var textDocumentPositionParams = new TextDocumentPositionParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
        };

        var results = await endpoint.GetTestAccessor().HandleRequestAsync(document, position, DisposalToken);

        Assumes.NotNull(results);

        var totalSpans = input.Spans.Length + additionalFiles.Sum(f => f.testCode.TryGetNamedSpans("", out var spans) ? spans.Length : 0);
        Assert.Equal(totalSpans, results.Length);

        var razorDocumentUri = document.CreateUri();

        foreach (var result in results)
        {
            if (result.TryGetFirst(out var referenceItem))
            {
                if (referenceItem.DisplayPath is not null)
                {
                    Assert.False(referenceItem.DisplayPath.EndsWith(".g.cs"));
                }

                if (referenceItem.DocumentName is not null)
                {
                    Assert.False(referenceItem.DocumentName.EndsWith(".g.cs"));
                }
            }
        }

        foreach (var result in results)
        {
            var location = GetLocation(result);
            string matchedText;
            if (razorDocumentUri.Equals(location.DocumentUri.GetRequiredParsedUri()))
            {
                matchedText = inputText.Lines[location.Range.Start.Line].ToString();
                Assert.Single(input.Spans.Where(s => inputText.GetRange(s).Equals(location.Range)));
            }
            else
            {
                var (fileName, testCode) = Assert.Single(additionalFiles.Where(f => FilePathNormalizingComparer.Instance.Equals(f.fileName, location.DocumentUri.GetRequiredParsedUri().AbsolutePath)));
                var text = SourceText.From(testCode.Text);
                matchedText = text.Lines[location.Range.Start.Line].ToString();
                Assert.Single(testCode.Spans.Where(s => text.GetRange(s).Equals(location.Range)));
            }

            if (result.TryGetFirst(out var referenceItem))
            {
                Assert.Equal(matchedText.Trim(), GetText(referenceItem));
            }
        }
    }

    private static string GetText(VSInternalReferenceItem referenceItem)
    {
        if (referenceItem.Text is ClassifiedTextElement classifiedText)
        {
            return string.Join("", classifiedText.Runs.Select(s => s.Text));
        }

        return referenceItem.Text.AssumeNotNull().ToString()!;
    }

    private static LspLocation GetLocation(SumType<VSInternalReferenceItem, LspLocation> r)
    {
        return r.TryGetFirst(out var refItem)
            ? refItem.Location ?? Assumed.Unreachable<LspLocation>()
            : r.Second;
    }
}
