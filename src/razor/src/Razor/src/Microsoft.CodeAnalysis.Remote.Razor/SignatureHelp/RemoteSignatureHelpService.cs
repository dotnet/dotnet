// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using LspSignatureHelp = Roslyn.LanguageServer.Protocol.SignatureHelp;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteSignatureHelpService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteSignatureHelpService
{
    internal sealed class Factory : FactoryBase<IRemoteSignatureHelpService>
    {
        protected override IRemoteSignatureHelpService CreateService(in ServiceArgs args)
            => new RemoteSignatureHelpService(in args);
    }

    public ValueTask<LspSignatureHelp?> GetSignatureHelpAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId documentId, Position position, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetSignatureHelpsAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<LspSignatureHelp?> GetSignatureHelpsAsync(RemoteDocumentContext context, Position position, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var linePosition = new LinePosition(position.Line, position.Character);
        var absoluteIndex = codeDocument.Source.Text.GetRequiredAbsoluteIndex(linePosition);

        var generatedDocument = await context.Snapshot.GetGeneratedDocumentAsync().ConfigureAwait(false);

        if (DocumentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), absoluteIndex, out var mappedPosition, out _))
        {
            return await ExternalHandlers.SignatureHelp.GetSignatureHelpAsync(generatedDocument, mappedPosition, supportsVisualStudioExtensions: true, cancellationToken).ConfigureAwait(false);
        }

        return null;
    }
}
