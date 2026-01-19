// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public partial class CohostDocumentPullDiagnosticsTest
{
    private async Task VerifyDiagnosticsAsync(TestCode input, VSInternalDiagnosticReport[]? htmlResponse = null, bool miscellaneousFile = false)
    {
        var document = CreateProjectAndRazorDocument(input.Text, miscellaneousFile: miscellaneousFile);
        var inputText = await document.GetTextAsync(DisposalToken);

        var requestInvoker = new TestHtmlRequestInvoker([(VSInternalMethods.DocumentPullDiagnosticName, htmlResponse)]);

        var endpoint = new DocumentPullDiagnosticsEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker, ClientCapabilitiesService, NoOpTelemetryReporter.Instance, LoggerFactory);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, DisposalToken);

        Assert.NotNull(result);

        var markers = result.SelectMany(d =>
            new[] {
                (index: inputText.GetTextSpan(d.Range).Start, text: $"{{|{d.Code!.Value.Second}:"),
                (index: inputText.GetTextSpan(d.Range).End, text:"|}")
            });

        var testOutput = input.Text;
        // Ordering by text last means start tags get sorted before end tags, for zero width ranges
        foreach (var (index, text) in markers.OrderByDescending(i => i.index).ThenByDescending(i => i.text))
        {
            testOutput = testOutput.Insert(index, text);
        }

        AssertEx.EqualOrDiff(input.OriginalInput, testOutput);
    }
}
