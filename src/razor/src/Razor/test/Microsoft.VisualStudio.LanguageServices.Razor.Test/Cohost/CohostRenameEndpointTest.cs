// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostRenameEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact(Skip = "Cannot edit source generated documents")]
    public Task CSharp_Method()
        => VerifyRenamesAsync(
            input: """
                This is a Razor document.

                <h1>@MyMethod()</h1>

                @code
                {
                    public string MyMe$$thod()
                    {
                        return $"Hi from {nameof(MyMethod)}";
                    }
                }

                The end.
                """,
            newName: "CallThisFunction",
            expected: """
                This is a Razor document.
                
                <h1>@CallThisFunction()</h1>
                
                @code
                {
                    public string CallThisFunction()
                    {
                        return $"Hi from {nameof(CallThisFunction)}";
                    }
                }
                
                The end.
                """);

    [Theory(Skip = "Cannot edit source generated documents")]
    [InlineData("$$Component")]
    [InlineData("Com$$ponent")]
    [InlineData("Component$$")]
    public Task Component_StartTag(string startTag)
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Component />

                <div>
                    <{startTag} />
                    <Component>
                    </Component>
                    <div>
                        <Component />
                        <Component>
                        </Component>
                    </div>
                </div>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), "")
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />
                
                <div>
                    <DifferentName />
                    <DifferentName>
                    </DifferentName>
                    <div>
                        <DifferentName />
                        <DifferentName>
                        </DifferentName>
                    </div>
                </div>

                The end.
                """,
            renames: [("Component.razor", "DifferentName.razor")]);

    [Theory(Skip = "Cannot edit source generated documents")]
    [InlineData("$$Component")]
    [InlineData("Com$$ponent")]
    [InlineData("Component$$")]
    public Task Component_EndTag(string endTag)
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Component />

                <div>
                    <Component />
                    <Component>
                    </Component>
                    <div>
                        <Component />
                        <Component>
                        </{endTag}>
                    </div>
                </div>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), "")
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />

                <div>
                    <DifferentName />
                    <DifferentName>
                    </DifferentName>
                    <div>
                        <DifferentName />
                        <DifferentName>
                        </DifferentName>
                    </div>
                </div>

                The end.
                """,
            renames: [("Component.razor", "DifferentName.razor")]);

    [Fact]
    public Task Mvc()
       => VerifyRenamesAsync(
           input: """
                This is a Razor document.

                <Com$$ponent />

                The end.
                """,
           additionalFiles: [
                (FilePath("Component.razor"), "")
           ],
           newName: "DifferentName",
           expected: "",
           fileKind: FileKinds.Legacy);

    private async Task VerifyRenamesAsync(string input, string newName, string expected, string? fileKind = null, (string fileName, string contents)[]? additionalFiles = null, (string oldName, string newName)[]? renames = null)
    {
        TestFileMarkupParser.GetPosition(input, out var source, out var cursorPosition);
        var document = CreateProjectAndRazorDocument(source, fileKind, additionalFiles);
        var inputText = await document.GetTextAsync(DisposalToken);
        var position = inputText.GetPosition(cursorPosition);

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentRenameName, null)]);

        var endpoint = new CohostRenameEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker);

        var renameParams = new RenameParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier { Uri = document.CreateUri() },
            NewName = newName,
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(renameParams, document, DisposalToken);

        if (expected.Length == 0)
        {
            Assert.True(renames is null or []);
            Assert.Null(result);
            return;
        }

        Assumes.NotNull(result);

        if (result.DocumentChanges.AssumeNotNull().TryGetSecond(out var changes))
        {
            Assert.NotNull(renames);

            foreach (var change in changes)
            {
                if (change.TryGetThird(out var renameEdit))
                {
                    Assert.Contains(renames,
                        r => renameEdit.OldUri.GetDocumentFilePath().EndsWith(r.oldName) &&
                             renameEdit.NewUri.GetDocumentFilePath().EndsWith(r.newName));
                }
            }
        }

        var actual = ProcessRazorDocumentEdits(inputText, document.CreateUri(), result);

        AssertEx.EqualOrDiff(expected, actual);
    }

    private static string ProcessRazorDocumentEdits(SourceText inputText, Uri razorDocumentUri, WorkspaceEdit result)
    {
        Assert.True(result.TryGetTextDocumentEdits(out var textDocumentEdits));
        foreach (var textDocumentEdit in textDocumentEdits)
        {
            if (textDocumentEdit.TextDocument.Uri == razorDocumentUri)
            {
                foreach (var edit in textDocumentEdit.Edits)
                {
                    inputText = inputText.WithChanges(inputText.GetTextChange(edit));
                }
            }
        }

        return inputText.ToString();
    }
}
