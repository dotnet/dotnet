// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.LanguageServer.Protocol;
using RoslynPosition = Roslyn.LanguageServer.Protocol.Position;
using VsPosition = Microsoft.VisualStudio.LanguageServer.Protocol.Position;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract class RazorDocumentServiceBase(in ServiceArgs args) : RazorBrokeredServiceBase(in args)
{
    protected DocumentSnapshotFactory DocumentSnapshotFactory { get; } = args.ExportProvider.GetExportedValue<DocumentSnapshotFactory>();
    protected IDocumentMappingService DocumentMappingService { get; } = args.ExportProvider.GetExportedValue<IDocumentMappingService>();

    protected virtual IDocumentPositionInfoStrategy DocumentPositionInfoStrategy { get; } = DefaultDocumentPositionInfoStrategy.Instance;

    protected DocumentPositionInfo GetPositionInfo(RazorCodeDocument codeDocument, int hostDocumentIndex)
    {
        return DocumentPositionInfoStrategy.GetPositionInfo(DocumentMappingService, codeDocument, hostDocumentIndex);
    }

    protected bool TryGetDocumentPositionInfo(RazorCodeDocument codeDocument, RoslynPosition position, out DocumentPositionInfo positionInfo)
    {
        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(position, out var hostDocumentIndex))
        {
            positionInfo = default;
            return false;
        }

        positionInfo = GetPositionInfo(codeDocument, hostDocumentIndex);
        return true;
    }

    protected bool TryGetDocumentPositionInfo(RazorCodeDocument codeDocument, VsPosition position, out DocumentPositionInfo positionInfo)
    {
        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(position, out var hostDocumentIndex))
        {
            positionInfo = default;
            return false;
        }

        positionInfo = GetPositionInfo(codeDocument, hostDocumentIndex);
        return true;
    }

    protected ValueTask<T> RunServiceAsync<T>(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        Func<RemoteDocumentContext, ValueTask<T>> implementation,
        CancellationToken cancellationToken)
    {
        return RunServiceAsync(
            solutionInfo,
            solution =>
            {
                var documentContext = CreateRazorDocumentContext(solution, razorDocumentId);
                if (documentContext is null)
                {
                    return default;
                }

                return implementation(documentContext);
            },
            cancellationToken);
    }

    private RemoteDocumentContext? CreateRazorDocumentContext(Solution solution, DocumentId razorDocumentId)
    {
        var razorDocument = solution.GetAdditionalDocument(razorDocumentId);
        if (razorDocument is null)
        {
            return null;
        }

        var documentSnapshot = DocumentSnapshotFactory.GetOrCreate(razorDocument);

        return new RemoteDocumentContext(razorDocument.CreateUri(), documentSnapshot);
    }
}
