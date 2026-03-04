// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public partial class CohostDocumentPullDiagnosticsTest
{
    private async Task VerifyDiagnosticsAsync(
        TestCode input,
        VSInternalDiagnosticReport[]? htmlResponse = null,
        RazorFileKind? fileKind = null,
        bool miscellaneousFile = false,
        (string fileName, string contents)[]? additionalFiles = null)
    {
        var document = CreateProjectAndRazorDocument(input.Text, fileKind, miscellaneousFile: miscellaneousFile, additionalFiles: additionalFiles);
        var inputText = await document.GetTextAsync(DisposalToken);

        var requestInvoker = new TestHtmlRequestInvoker([(VSInternalMethods.DocumentPullDiagnosticName, htmlResponse)]);

        var result = await MakeDiagnosticsRequestAsync(document, taskListRequest: false, requestInvoker, IncompatibleProjectService, RemoteServiceInvoker, ClientSettingsManager, ClientCapabilitiesService, LoggerFactory, DisposalToken);

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

    internal static Task<LspDiagnostic[]?> MakeDiagnosticsRequestAsync(
        TextDocument document,
        bool taskListRequest,
        TestHtmlRequestInvoker requestInvoker,
        IIncompatibleProjectService incompatibleProjectService,
        IRemoteServiceInvoker remoteServiceInvoker,
        IClientSettingsManager _,
        IClientCapabilitiesService clientCapabilitiesService,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        Assert.False(taskListRequest, "Not supported.");

        var endpoint = new DocumentPullDiagnosticsEndpoint(incompatibleProjectService, remoteServiceInvoker, requestInvoker, clientCapabilitiesService, NoOpTelemetryReporter.Instance, loggerFactory);

        return endpoint.GetTestAccessor().HandleRequestAsync(document, cancellationToken);
    }
}
