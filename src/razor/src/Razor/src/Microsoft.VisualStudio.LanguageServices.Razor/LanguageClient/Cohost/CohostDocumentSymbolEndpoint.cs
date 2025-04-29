// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentDocumentSymbolName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostDocumentSymbolEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostDocumentSymbolEndpoint(IRemoteServiceInvoker remoteServiceInvoker)
    : AbstractRazorCohostDocumentRequestHandler<DocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private bool _useHierarchicalSymbols;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.DocumentSymbol?.DynamicRegistration == true)
        {
            _useHierarchicalSymbols = clientCapabilities.TextDocument.DocumentSymbol.HierarchicalDocumentSymbolSupport;

            return [new Registration
            {
                Method = Methods.TextDocumentDocumentSymbolName,
                RegisterOptions = new DocumentSymbolRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(DocumentSymbolParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SumType<DocumentSymbol[], SymbolInformation[]>?> HandleRequestAsync(DocumentSymbolParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(context.TextDocument.AssumeNotNull(), _useHierarchicalSymbols, cancellationToken);

    private async Task<SumType<DocumentSymbol[], SymbolInformation[]>?> HandleRequestAsync(TextDocument razorDocument, bool useHierarchicalSymbols, CancellationToken cancellationToken)
    {
        // Normally we could remove the await here, but in this case it neatly converts from ValueTask to Task for us,
        // and more importantly this method is essentially a public API entry point (via LSP) so having it appear in
        // call stacks is desirable
        return await _remoteServiceInvoker.TryInvokeAsync<IRemoteDocumentSymbolService, SumType<DocumentSymbol[], SymbolInformation[]>?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetDocumentSymbolsAsync(solutionInfo, razorDocument.Id, useHierarchicalSymbols, cancellationToken),
            cancellationToken).ConfigureAwait(false);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentSymbolEndpoint instance)
    {
        public Task<SumType<DocumentSymbol[], SymbolInformation[]>?> HandleRequestAsync(TextDocument razorDocument, bool useHierarchicalSymbols, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, useHierarchicalSymbols, cancellationToken);
    }
}
