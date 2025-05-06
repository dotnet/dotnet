// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.VisualStudio.LanguageServices.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentSignatureHelpName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportRazorStatelessLspService(typeof(CohostSignatureHelpEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostSignatureHelpEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientSettingsManager clientSettingsManager,
    IHtmlRequestInvoker requestInvoker)
    : AbstractRazorCohostDocumentRequestHandler<SignatureHelpParams, SignatureHelp?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly IHtmlRequestInvoker _requestInvoker = requestInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.SignatureHelp?.DynamicRegistration == true)
        {
            return [new Registration()
            {
                Method = Methods.TextDocumentSignatureHelpName,
                RegisterOptions = new SignatureHelpRegistrationOptions()
                    .EnableSignatureHelp()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(SignatureHelpParams request)
        => new RazorTextDocumentIdentifier(request.TextDocument.Uri, (request.TextDocument as VSTextDocumentIdentifier)?.ProjectContext?.Id);

    // NOTE: The use of SumType here is a little odd, but it allows us to return Roslyn LSP types from the Roslyn call, and VS LSP types from the Html
    //       call. It works because both sets of types are attributed the right way, so the Json ends up looking the same and the client doesn't
    //       care. Ideally eventually we will be able to move all of this to just Roslyn LSP types, but we might have to wait for Web Tools
    protected override Task<SignatureHelp?> HandleRequestAsync(SignatureHelpParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<SignatureHelp?> HandleRequestAsync(SignatureHelpParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        // Return nothing if "Parameter Information" option is disabled unless signature help is invoked explicitly via command as opposed to typing or content change
        if (request.Context is { TriggerKind: not SignatureHelpTriggerKind.Invoked } &&
            !_clientSettingsManager.GetClientSettings().ClientCompletionSettings.AutoListParams)
        {
            return null;
        }

        var data = await _remoteServiceInvoker.TryInvokeAsync<IRemoteSignatureHelpService, SignatureHelp?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) =>
                service.GetSignatureHelpAsync(solutionInfo, razorDocument.Id, request.Position, cancellationToken),
            cancellationToken)
            .ConfigureAwait(false);

        // If we got a response back, then either Razor or C# wants to do something with this, so we're good to go
        if (data is { } signatureHelp)
        {
            return signatureHelp;
        }

        return await _requestInvoker.MakeHtmlLspRequestAsync<SignatureHelpParams, SignatureHelp>(
            razorDocument,
            Methods.TextDocumentSignatureHelpName,
            request,
            cancellationToken).ConfigureAwait(false);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostSignatureHelpEndpoint instance)
    {
        internal Task<SignatureHelp?> HandleRequestAsync(SignatureHelpParams request, TextDocument document, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, document, cancellationToken);
    }
}
