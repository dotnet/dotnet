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
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.Settings;
using Roslyn.LanguageServer.Protocol;
using VSLSP = Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.TextDocumentInlineCompletionName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostInlineCompletionEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostInlineCompletionEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientSettingsManager clientSettingsManager)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalInlineCompletionRequest, VSInternalInlineCompletionList?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<VSLSP.Registration> GetRegistrations(VSLSP.VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.CodeAction?.DynamicRegistration == true)
        {
            return [new VSLSP.Registration
            {
                Method = VSInternalMethods.TextDocumentInlineCompletionName,
                RegisterOptions = new VSLSP.VSInternalInlineCompletionRegistrationOptions().EnableInlineCompletion()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalInlineCompletionRequest request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<VSInternalInlineCompletionList?> HandleRequestAsync(VSInternalInlineCompletionRequest request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(context, context.TextDocument.AssumeNotNull(), request.Position.ToLinePosition(), request.Options, cancellationToken);

    private async Task<VSInternalInlineCompletionList?> HandleRequestAsync(RazorCohostRequestContext? context, TextDocument razorDocument, LinePosition linePosition, FormattingOptions formattingOptions, CancellationToken cancellationToken)
    {
        var requestInfo = await _remoteServiceInvoker.TryInvokeAsync<IRemoteInlineCompletionService, InlineCompletionRequestInfo?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetInlineCompletionInfoAsync(solutionInfo, razorDocument.Id, linePosition, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (requestInfo is not InlineCompletionRequestInfo(var generatedDocumentUri, var position))
        {
            return null;
        }

        if (!razorDocument.Project.TryGetCSharpDocument(generatedDocumentUri, out var generatedDocument))
        {
            return null;
        }

        var result = await Completion.GetInlineCompletionItemsAsync(context, generatedDocument, position, formattingOptions, cancellationToken).ConfigureAwait(false);

        if (result is null)
        {
            return null;
        }

        if (result.Range is not null)
        {
            var options = RazorFormattingOptions.From(formattingOptions, _clientSettingsManager.GetClientSettings().AdvancedSettings.CodeBlockBraceOnNextLine);
            var span = result.Range.ToLinePositionSpan();
            var formattedInfo = await _remoteServiceInvoker.TryInvokeAsync<IRemoteInlineCompletionService, FormattedInlineCompletionInfo?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) => service.FormatInlineCompletionAsync(solutionInfo, razorDocument.Id, options, span, result.Text, cancellationToken),
                cancellationToken).ConfigureAwait(false);

            if (formattedInfo is { } formatted)
            {
                result.Range = formatted.Span.ToRange();
                result.Text = formatted.FormattedText;
            }
            else
            {
                return null;
            }
        }

        return new VSInternalInlineCompletionList { Items = [result] };
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostInlineCompletionEndpoint instance)
    {
        public Task<VSInternalInlineCompletionList?> HandleRequestAsync(TextDocument razorDocument, LinePosition position, FormattingOptions formattingOptions, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(context: null, razorDocument, position, formattingOptions, cancellationToken);
    }
}
