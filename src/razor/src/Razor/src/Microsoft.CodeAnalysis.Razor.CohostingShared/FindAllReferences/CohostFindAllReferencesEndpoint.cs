// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Remote;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentReferencesName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportRazorStatelessLspService(typeof(CohostFindAllReferencesEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostFindAllReferencesEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker)
    : AbstractRazorCohostDocumentRequestHandler<ReferenceParams, SumType<VSInternalReferenceItem, LspLocation>[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.References?.DynamicRegistration == true)
        {
            return [new Registration
            {
                Method = Methods.TextDocumentReferencesName,
                RegisterOptions = new ReferenceRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(ReferenceParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SumType<VSInternalReferenceItem, LspLocation>[]?> HandleRequestAsync(ReferenceParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(
            context.TextDocument.AssumeNotNull(),
            request.Position,
            cancellationToken);

    private async Task<SumType<VSInternalReferenceItem, LspLocation>[]?> HandleRequestAsync(TextDocument razorDocument, Position position, CancellationToken cancellationToken)
    {
        var response = await _remoteServiceInvoker
            .TryInvokeAsync<IRemoteFindAllReferencesService, RemoteResponse<SumType<VSInternalReferenceItem, LspLocation>[]?>>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.FindAllReferencesAsync(solutionInfo, razorDocument.Id, position, cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        if (response.Result is SumType<VSInternalReferenceItem, LspLocation>[] results)
        {
            return results;
        }

        return null;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostFindAllReferencesEndpoint instance)
    {
        public Task<SumType<VSInternalReferenceItem, LspLocation>[]?> HandleRequestAsync(TextDocument razorDocument, Position position, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, position, cancellationToken);
    }
}
