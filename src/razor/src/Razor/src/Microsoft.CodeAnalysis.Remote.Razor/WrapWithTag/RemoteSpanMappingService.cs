// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed partial class RemoteSpanMappingService(in ServiceArgs args) : RazorBrokeredServiceBase(in args), IRemoteSpanMappingService
{
    internal sealed class Factory : FactoryBase<IRemoteSpanMappingService>
    {
        protected override IRemoteSpanMappingService CreateService(in ServiceArgs args)
            => new RemoteSpanMappingService(in args);
    }

    private readonly RemoteSnapshotManager _snapshotManager = args.ExportProvider.GetExportedValue<RemoteSnapshotManager>();
    private readonly IDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IDocumentMappingService>();
    private readonly ITelemetryReporter _telemetryReporter = args.ExportProvider.GetExportedValue<ITelemetryReporter>();

    public ValueTask<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId generatedDocumentId, ImmutableArray<TextSpan> spans, CancellationToken cancellationToken)
       => RunServiceAsync(
            solutionInfo,
            solution => MapSpansAsync(solution, generatedDocumentId, spans, cancellationToken),
            cancellationToken);

    private async ValueTask<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Solution solution, DocumentId generatedDocumentId, ImmutableArray<TextSpan> spans, CancellationToken cancellationToken)
    {
        var razorDocument = await TryGetRazorDocumentForGeneratedDocumentIdAsync(generatedDocumentId, solution, cancellationToken).ConfigureAwait(false);
        if (razorDocument is null)
        {
            return [];
        }

        var documentSnapshot = _snapshotManager.GetSnapshot(razorDocument);
        var output = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        var source = output.Source.Text;

        var csharpDocument = output.GetRequiredCSharpDocument();
        var filePath = output.Source.FilePath.AssumeNotNull();

        using var results = new PooledArrayBuilder<RazorMappedSpanResult>();

        foreach (var span in spans)
        {
            if (RazorEditHelper.TryGetMappedSpans(span, source, csharpDocument, out var linePositionSpan, out var mappedSpan))
            {
                results.Add(new(filePath, linePositionSpan, mappedSpan));
            }
            else
            {
                results.Add(default);
            }
        }

        return results.ToImmutableAndClear();
    }

    public ValueTask<ImmutableArray<RazorMappedEditResult>> MapTextChangesAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId generatedDocumentId, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            solution => MapTextChangesAsync(solution, generatedDocumentId, changes, cancellationToken),
            cancellationToken);

    private async ValueTask<ImmutableArray<RazorMappedEditResult>> MapTextChangesAsync(Solution solution, DocumentId generatedDocumentId, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        try
        {
            var razorDocument = await TryGetRazorDocumentForGeneratedDocumentIdAsync(generatedDocumentId, solution, cancellationToken).ConfigureAwait(false);
            if (razorDocument is null)
            {
                return [];
            }

            var documentSnapshot = _snapshotManager.GetSnapshot(razorDocument);
            var results = await RazorEditHelper.MapCSharpEditsAsync(
                changes.SelectAsArray(c => c.ToRazorTextChange()),
                documentSnapshot,
                _documentMappingService,
                _telemetryReporter,
                cancellationToken).ConfigureAwait(false);

            if (results.IsDefaultOrEmpty)
            {
                return [];
            }

            var razorSource = await razorDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
            var textChanges = results.SelectAsArray(te => te.ToTextChange());

            return [new RazorMappedEditResult() { FilePath = documentSnapshot.FilePath, TextChanges = [.. textChanges] }];
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Failed to map edits");
            return [];
        }
    }

    private async Task<TextDocument?> TryGetRazorDocumentForGeneratedDocumentIdAsync(DocumentId generatedDocumentId, Solution solution, CancellationToken cancellationToken)
    {
        var generatedDocument = await solution.GetSourceGeneratedDocumentAsync(generatedDocumentId, cancellationToken).ConfigureAwait(false);
        if (generatedDocument is null)
        {
            return null;
        }

        var identity = RazorGeneratedDocumentIdentity.Create(generatedDocument);
        if (!identity.IsRazorSourceGeneratedDocument())
        {
            return null;
        }

        var projectSnapshot = _snapshotManager.GetSnapshot(generatedDocument.Project);

        return await projectSnapshot.TryGetRazorDocumentFromGeneratedHintNameAsync(identity.HintName, cancellationToken).ConfigureAwait(false);
    }
}
