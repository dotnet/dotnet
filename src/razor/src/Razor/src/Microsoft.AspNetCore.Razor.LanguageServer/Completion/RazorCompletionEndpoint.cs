// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class RazorCompletionEndpoint : IVSCompletionEndpoint
{
    private readonly CompletionListProvider _completionListProvider;
    private VSInternalClientCapabilities? _clientCapabilities;

    public RazorCompletionEndpoint(CompletionListProvider completionListProvider)
    {
        _completionListProvider = completionListProvider;
    }

    public bool MutatesSolutionState => false;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string AssociatedServerCapability = "completionProvider";

        _clientCapabilities = clientCapabilities;

        var registrationOptions = new CompletionOptions()
        {
            ResolveProvider = true,
            TriggerCharacters = _completionListProvider.AggregateTriggerCharacters.ToArray(),
            AllCommitCharacters = new[] { ":", ">", " ", "=" },
        };

        return new RegistrationExtensionResult(AssociatedServerCapability, registrationOptions);
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(CompletionParams request)
    {
        return request.TextDocument;
    }

    public async Task<VSInternalCompletionList?> HandleRequestAsync(CompletionParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;

        if (request.Context is null || documentContext is null)
        {
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!request.Position.TryGetAbsoluteIndex(sourceText, requestContext.Logger, out var hostDocumentIndex))
        {
            return null;
        }

        if (request.Context is not VSInternalCompletionContext completionContext)
        {
            Debug.Fail("Completion context should never be null in practice");
            return null;
        }

        var completionList = await _completionListProvider.GetCompletionListAsync(
            hostDocumentIndex,
            completionContext,
            documentContext,
            _clientCapabilities!,
            cancellationToken).ConfigureAwait(false);
        return completionList;
    }
}
