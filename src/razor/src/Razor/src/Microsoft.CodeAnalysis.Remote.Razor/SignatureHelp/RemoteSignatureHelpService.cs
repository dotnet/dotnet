// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;

namespace Microsoft.CodeAnalysis.Remote.Razor;

using SignatureHelp = Roslyn.LanguageServer.Protocol.SignatureHelp;

internal sealed class RemoteSignatureHelpService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteSignatureHelpService
{
    internal sealed class Factory : FactoryBase<IRemoteSignatureHelpService>
    {
        protected override IRemoteSignatureHelpService CreateService(in ServiceArgs args)
            => new RemoteSignatureHelpService(in args);
    }

    private readonly IFilePathService _filePathService = args.ExportProvider.GetExportedValue<IFilePathService>();
    private readonly IRazorDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IRazorDocumentMappingService>();

    public ValueTask<SignatureHelp?> GetSignatureHelpAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId documentId, Position position, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetSignatureHelpsAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<SignatureHelp?> GetSignatureHelpsAsync(RemoteDocumentContext context, Position position, CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var linePosition = new LinePosition(position.Line, position.Character);
        var absoluteIndex = codeDocument.Source.Text.GetRequiredAbsoluteIndex(linePosition);

        var generatedDocument = await context.GetGeneratedDocumentAsync(_filePathService, cancellationToken).ConfigureAwait(false);

        if (_documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), absoluteIndex, out var mappedPosition, out _))
        {
            return await ExternalHandlers.SignatureHelp.GetSignatureHelpAsync(generatedDocument, mappedPosition, supportsVisualStudioExtensions: true, cancellationToken).ConfigureAwait(false);
        }

        return null;
    }
}
