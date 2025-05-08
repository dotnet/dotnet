// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Telemetry;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentDiagnosticName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportRazorStatelessLspService(typeof(DocumentPullDiagnosticsEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class DocumentPullDiagnosticsEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlRequestInvoker requestInvoker,
    IClientCapabilitiesService clientCapabilitiesService,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory)
    : CohostDocumentPullDiagnosticsEndpointBase<DocumentDiagnosticParams, FullDocumentDiagnosticReport?>(remoteServiceInvoker, requestInvoker, clientCapabilitiesService, telemetryReporter, loggerFactory), IDynamicRegistrationProvider
{
    protected override string LspMethodName => Methods.TextDocumentDiagnosticName;
    protected override bool SupportsHtmlDiagnostics => false;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Diagnostic?.DynamicRegistration is true)
        {
            return [new Registration()
            {
                Method = Methods.TextDocumentDiagnosticName,
                RegisterOptions = new DiagnosticRegistrationOptions()
                {
                    WorkspaceDiagnostics = false
                }
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(DocumentDiagnosticParams request)
        => request.TextDocument?.ToRazorTextDocumentIdentifier();

    protected async override Task<FullDocumentDiagnosticReport?> HandleRequestAsync(DocumentDiagnosticParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        var results = await GetDiagnosticsAsync(context.TextDocument.AssumeNotNull(), cancellationToken).ConfigureAwait(false);

        if (results is null)
        {
            return null;
        }

        return new()
        {
            Items = results,
            ResultId = Guid.NewGuid().ToString()
        };
    }
}

