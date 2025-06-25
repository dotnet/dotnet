// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.SignatureHelp;

[RazorLanguageServerEndpoint(Methods.TextDocumentSignatureHelpName)]
internal sealed class SignatureHelpEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        RazorLSPOptionsMonitor optionsMonitor,
        ILoggerFactory loggerProvider)
    : AbstractRazorDelegatingEndpoint<SignatureHelpParams, LspSignatureHelp?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerProvider.GetOrCreateLogger<SignatureHelpEndpoint>()),
    ICapabilitiesProvider
{
    protected override string CustomMessageTarget => CustomMessageNames.RazorSignatureHelpEndpointName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableSignatureHelp();
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(SignatureHelpParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (request.Context is not null &&
            request.Context.TriggerKind != SignatureHelpTriggerKind.Invoked &&
            !optionsMonitor.CurrentValue.AutoListParams)
        {
            // Return nothing if "Parameter Information" option is disabled unless signature help is invoked explicitly via command as opposed to typing or content change
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            positionInfo.Position,
            positionInfo.LanguageKind));
    }
}
