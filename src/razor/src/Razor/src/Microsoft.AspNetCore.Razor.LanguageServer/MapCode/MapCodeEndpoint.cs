// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.MapCode.Mappers;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Text;
using SyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.AspNetCore.Razor.LanguageServer.MapCode;

/// <summary>
/// Maps requested code to a given Razor document.
/// </summary>
/// <remarks>
/// This class and its mapping heuristics will likely be constantly evolving as we receive
/// more advanced inputs from the client.
/// </remarks>
[RazorLanguageServerEndpoint(VSInternalMethods.WorkspaceMapCodeName)]
internal sealed class MapCodeEndpoint(
    IDocumentMappingService documentMappingService,
    IDocumentContextFactory documentContextFactory,
    IClientConnection clientConnection,
    ITelemetryReporter telemetryReporter) : IRazorDocumentlessRequestHandler<VSInternalMapCodeParams, WorkspaceEdit?>, ICapabilitiesProvider
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
    private readonly IClientConnection _clientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter ?? throw new ArgumentNullException(nameof(telemetryReporter));

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities _)
    {
        serverCapabilities.EnableMapCodeProvider();
    }

    public async Task<WorkspaceEdit?> HandleRequestAsync(
        VSInternalMapCodeParams mapperParams,
        RazorRequestContext context,
        CancellationToken cancellationToken)
    {
        // TO-DO: Apply updates to the workspace before doing mapping. This is currently
        // unimplemented by the client, so we won't bother doing anything for now until
        // we determine what kinds of updates the client will actually send us.
        Debug.Assert(mapperParams.Updates is null);

        if (mapperParams.Updates is not null)
        {
            return null;
        }

        var mapCodeCorrelationId = mapperParams.MapCodeCorrelationId ?? Guid.NewGuid();
        using var ts = _telemetryReporter.TrackLspRequest(VSInternalMethods.WorkspaceMapCodeName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.MapCodeRazorTelemetryThreshold, mapCodeCorrelationId);

        return await HandleMappingsAsync(mapperParams.Mappings, mapCodeCorrelationId, cancellationToken).ConfigureAwait(false);
    }

    private async Task<WorkspaceEdit?> HandleMappingsAsync(VSInternalMapCodeMapping[] mappings, Guid mapCodeCorrelationId, CancellationToken cancellationToken)
    {
        using var _ = ListPool<TextDocumentEdit>.GetPooledObject(out var changes);
        foreach (var mapping in mappings)
        {
            if (mapping.TextDocument is null || mapping.FocusLocations is null)
            {
                continue;
            }

            if (!_documentContextFactory.TryCreate(mapping.TextDocument.DocumentUri.GetRequiredParsedUri(), out var documentContext))
            {
                continue;
            }

            var snapshot = documentContext.Snapshot;

            foreach (var content in mapping.Contents)
            {
                if (content is null)
                {
                    continue;
                }

                // We create a new Razor file based on each content in each mapping order to get the syntax tree that we'll later use to map.
                var newSnapshot = snapshot.WithText(SourceText.From(content));
                var codeToMap = await newSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

                var mappingSuccess = await TryMapCodeAsync(
                    codeToMap, mapping.FocusLocations, changes, mapCodeCorrelationId, documentContext, cancellationToken).ConfigureAwait(false);

                // Mapping failed. Let the client's built-in fallback mapper handle mapping.
                if (!mappingSuccess)
                {
                    return null;
                }
            }
        }

        var workspaceEdits = new WorkspaceEdit
        {
            DocumentChanges = changes.ToArray()
        };

        return workspaceEdits;
    }

    private async Task<bool> TryMapCodeAsync(
        RazorCodeDocument codeToMap,
        LspLocation[][] locations,
        List<TextDocumentEdit> changes,
        Guid mapCodeCorrelationId,
        DocumentContext documentContext,
        CancellationToken cancellationToken)
    {
        if (!codeToMap.TryGetSyntaxTree(out var syntaxTree))
        {
            return false;
        }

        var nodesToMap = ExtractValidNodesToMap(syntaxTree.Root);
        if (nodesToMap.Length == 0)
        {
            return false;
        }

        var mappingSuccess = await TryMapCodeAsync(
            locations, nodesToMap, mapCodeCorrelationId, changes, documentContext, cancellationToken).ConfigureAwait(false);
        if (!mappingSuccess)
        {
            return false;
        }

        MergeEdits(changes);
        return true;
    }

    private async Task<bool> TryMapCodeAsync(
        LspLocation[][] focusLocations,
        ImmutableArray<SyntaxNode> nodesToMap,
        Guid mapCodeCorrelationId,
        List<TextDocumentEdit> changes,
        DocumentContext documentContext,
        CancellationToken cancellationToken)
    {
        var didCalculateCSharpFocusLocations = false;
        var csharpFocusLocations = new LspLocation[focusLocations.Length][];

        // We attempt to map the code using each focus location in order of priority.
        // The outer array is an ordered priority list (from highest to lowest priority),
        // and the inner array is a list of locations that have the same priority.
        // If we can successfully map using the first location, we'll stop and return.
        var mappingSuccess = false;
        foreach (var locationByPriority in focusLocations)
        {
            foreach (var location in locationByPriority)
            {
                // The current assumption is that all focus locations will always be in the same document
                // as the code to map. The client is currently implemented using this behavior, but if it
                // ever changes, we'll need to update this code to account for it (i.e., take into account
                // focus location URIs).
                Debug.Assert(location.DocumentUri.GetRequiredParsedUri() == documentContext.Uri);

                var syntaxTree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                if (syntaxTree is null)
                {
                    continue;
                }

                using var razorNodesToMap = new PooledArrayBuilder<SyntaxNode>();
                foreach (var nodeToMap in nodesToMap)
                {
                    // If node is C#, we send it to their language server to handle and ignore it from our end.
                    if (nodeToMap.IsCSharpNode(out var csharpBody))
                    {
                        if (!didCalculateCSharpFocusLocations)
                        {
                            csharpFocusLocations = await GetCSharpFocusLocationsAsync(focusLocations, cancellationToken).ConfigureAwait(false);
                            didCalculateCSharpFocusLocations = true;
                        }

                        var csharpMappingSuccessful = await TrySendCSharpDelegatedMappingRequestAsync(
                            documentContext.GetTextDocumentIdentifierAndVersion(),
                            csharpBody,
                            csharpFocusLocations,
                            mapCodeCorrelationId,
                            changes,
                            cancellationToken).ConfigureAwait(false);

                        // If C# delegation fails, we'll default to the client's fallback mapper.
                        if (!csharpMappingSuccessful)
                        {
                            return false;
                        }

                        mappingSuccess = true;
                        continue;
                    }

                    // If node already exists in the document, we'll ignore it.
                    if (nodeToMap.ExistsOnTarget(syntaxTree.Root))
                    {
                        continue;
                    }

                    razorNodesToMap.Add(nodeToMap);
                }

                var sourceText = await documentContext.Snapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);

                foreach (var nodeToMap in razorNodesToMap)
                {
                    var insertionSpan = InsertMapper.GetInsertionPoint(syntaxTree.Root, sourceText, location);
                    if (insertionSpan is not null)
                    {
                        var textSpan = new TextSpan(insertionSpan.Value, 0);
                        var edit = LspFactory.CreateTextEdit(sourceText.GetRange(textSpan), nodeToMap.ToString());

                        var textDocumentEdit = new TextDocumentEdit
                        {
                            TextDocument = new OptionalVersionedTextDocumentIdentifier
                            {
                                DocumentUri = new(documentContext.Uri)
                            },
                            Edits = [edit],
                        };

                        changes.Add(textDocumentEdit);
                        mappingSuccess = true;
                    }
                }

                // We were able to successfully map using this focusLocation.
                if (mappingSuccess)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static ImmutableArray<SyntaxNode> ExtractValidNodesToMap(SyntaxNode rootNode)
    {
        using var validNodesToMap = new PooledArrayBuilder<SyntaxNode>();
        using var _ = StackPool<SyntaxNode>.GetPooledObject(out var stack);
        stack.Push(rootNode);

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();

            if (s_validNodesToMap.Contains(currentNode.GetType()))
            {
                validNodesToMap.Add(currentNode);
                continue;
            }

            // Add child nodes to the stack in reverse order for depth-first search
            foreach (var childNode in currentNode.ChildNodes().Reverse())
            {
                stack.Push(childNode);
            }
        }

        return validNodesToMap.ToImmutable();
    }

    // These are the nodes that we currently support for mapping. We should update
    // this list as the client evolves to send more types of nodes.
    private readonly static List<Type> s_validNodesToMap =
    [
        typeof(CSharpCodeBlockSyntax),
        typeof(CSharpExplicitExpressionSyntax),
        typeof(CSharpImplicitExpressionSyntax),
        typeof(MarkupElementSyntax),
        typeof(MarkupTextLiteralSyntax),
        typeof(RazorDirectiveSyntax),
    ];

    private async Task<bool> TrySendCSharpDelegatedMappingRequestAsync(
        TextDocumentIdentifierAndVersion textDocumentIdentifier,
        SyntaxNode nodeToMap,
        LspLocation[][] focusLocations,
        Guid mapCodeCorrelationId,
        List<TextDocumentEdit> changes,
        CancellationToken cancellationToken)
    {
        var delegatedRequest = new DelegatedMapCodeParams(
            textDocumentIdentifier,
            RazorLanguageKind.CSharp,
            mapCodeCorrelationId,
            [nodeToMap.ToString()],
            FocusLocations: focusLocations);

        WorkspaceEdit? edits;
        try
        {
            edits = await _clientConnection.SendRequestAsync<DelegatedMapCodeParams, WorkspaceEdit?>(
                CustomMessageNames.RazorMapCodeEndpoint,
                delegatedRequest,
                cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            // C# hasn't implemented + merged their C# code mapper yet.
            return false;
        }

        if (edits is null)
        {
            // It's likely an error occurred during C# mapping.
            return false;
        }

        var success = await TryHandleDelegatedResponseAsync(edits, changes, cancellationToken).ConfigureAwait(false);
        return success;
    }

    private async Task<LspLocation[][]> GetCSharpFocusLocationsAsync(LspLocation[][] focusLocations, CancellationToken cancellationToken)
    {
        // If the focus locations are in a C# context, map them to the C# document.
        var csharpFocusLocations = new LspLocation[focusLocations.Length][];
        using var csharpLocations = new PooledArrayBuilder<LspLocation>();
        for (var i = 0; i < focusLocations.Length; i++)
        {
            csharpLocations.Clear();

            var locations = focusLocations[i];
            foreach (var potentialLocation in locations)
            {
                if (potentialLocation is null)
                {
                    continue;
                }

                if (!_documentContextFactory.TryCreate(potentialLocation.DocumentUri.GetRequiredParsedUri(), out var documentContext))
                {
                    continue;
                }

                var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
                var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
                var hostDocumentRange = potentialLocation.Range.ToLinePositionSpan();
                var csharpDocument = codeDocument.GetRequiredCSharpDocument();

                if (_documentMappingService.TryMapToCSharpDocumentRange(csharpDocument, hostDocumentRange, out var generatedDocumentRange))
                {
                    var csharpLocation = new LspLocation
                    {
                        // We convert the URI to the C# generated document URI later on in
                        // LanguageServer.Client since we're unable to retrieve it here.
                        DocumentUri = potentialLocation.DocumentUri,
                        Range = generatedDocumentRange.ToRange()
                    };

                    csharpLocations.Add(csharpLocation);
                }
            }

            csharpFocusLocations[i] = csharpLocations.ToArray();
        }

        return csharpFocusLocations;
    }

    // Map C# code back to Razor file
    private async Task<bool> TryHandleDelegatedResponseAsync(
        WorkspaceEdit edits,
        List<TextDocumentEdit> changes,
        CancellationToken cancellationToken)
    {
        using var _ = ListPool<TextDocumentEdit>.GetPooledObject(out var csharpChanges);
        if (edits.DocumentChanges is not null && edits.DocumentChanges.Value.TryGetFirst(out var documentEdits))
        {
            // We only support document edits for now. In the future once the client supports it, we should look
            // into also supporting file creation/deletion/rename.
            foreach (var edit in documentEdits)
            {
                var success = await TryProcessEditAsync(edit.TextDocument.DocumentUri.GetRequiredParsedUri(), edit.Edits, csharpChanges, cancellationToken).ConfigureAwait(false);
                if (!success)
                {
                    return false;
                }
            }
        }

        if (edits.Changes is not null)
        {
            foreach (var edit in edits.Changes)
            {
                var generatedUri = new Uri(edit.Key);
                var success = await TryProcessEditAsync(generatedUri, edit.Value.Select(e => (SumType<TextEdit, AnnotatedTextEdit>)e), csharpChanges, cancellationToken).ConfigureAwait(false);
                if (!success)
                {
                    return false;
                }
            }
        }

        changes.AddRange(csharpChanges);
        return true;

        async Task<bool> TryProcessEditAsync(
            Uri generatedUri,
            IEnumerable<SumType<TextEdit, AnnotatedTextEdit>> textEdits,
            List<TextDocumentEdit> csharpChanges,
            CancellationToken cancellationToken)
        {
            foreach (var documentEdit in textEdits.Select(e => (TextEdit)e))
            {
                var (hostDocumentUri, hostDocumentRange) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(
                    generatedUri, documentEdit.Range, cancellationToken).ConfigureAwait(false);

                if (hostDocumentUri == generatedUri)
                {
                    return false;
                }

                var textEdit = LspFactory.CreateTextEdit(hostDocumentRange, documentEdit.NewText);

                var textDocumentEdit = new TextDocumentEdit
                {
                    TextDocument = new OptionalVersionedTextDocumentIdentifier { DocumentUri = new(hostDocumentUri) },
                    Edits = [textEdit]
                };

                csharpChanges.Add(textDocumentEdit);
            }

            return true;
        }
    }

    // Resolve edits that are at the same start location by merging them together.
    private static void MergeEdits(List<TextDocumentEdit> changes)
    {
        var groupedChanges = changes.GroupBy(c => c.TextDocument.DocumentUri).ToImmutableArray();
        changes.Clear();
        foreach (var documentChanges in groupedChanges)
        {
            var edits = documentChanges.ToList();
            edits.Sort((x, y) => ((TextEdit)x.Edits.Single()).Range.Start.CompareTo(((TextEdit)y.Edits.Single()).Range.Start));

            for (var i = edits.Count - 1; i < edits.Count && i > 0; i--)
            {
                var previousEdit = (TextEdit)edits[i - 1].Edits.Single();
                var currentEdit = (TextEdit)edits[i].Edits.Single();
                if (currentEdit.Range.Start == previousEdit.Range.Start)
                {
                    // Append the text of the current edit to the previous edit
                    previousEdit.NewText += currentEdit.NewText;
                    previousEdit.Range.End = currentEdit.Range.End;
                    edits.RemoveAt(i);
                }
            }

            var finalEditsForDoc = new TextDocumentEdit
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier
                {
                    DocumentUri = documentChanges.Key,
                },
                Edits = edits.SelectMany(e => e.Edits).ToArray()
            };

            changes.Add(finalEditsForDoc);
        }
    }
}
