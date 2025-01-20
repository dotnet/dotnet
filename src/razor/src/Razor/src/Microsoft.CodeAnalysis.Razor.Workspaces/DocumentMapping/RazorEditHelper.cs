// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal static partial class RazorEditHelper
{
    /// <summary>
    /// Maps the given text edits for a razor file based on changes in csharp. It special
    /// cases usings directives to insure they are added correctly. All other edits
    /// are applied if they map to the razor document.
    /// </summary>
    /// <remarks>
    /// Note that the changes coming in are in the generated C# file. This method will map them appropriately.
    /// </remarks>
    internal static async Task<ImmutableArray<RazorTextChange>> MapCSharpEditsAsync(
        ImmutableArray<RazorTextChange> textChanges,
        IDocumentSnapshot snapshot,
        IDocumentMappingService documentMappingService,
        ITelemetryReporter telemetryReporter,
        CancellationToken cancellationToken)
    {
        var codeDocument = await snapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        using var textChangeBuilder = new TextChangeBuilder(documentMappingService);

        var originalSyntaxTree = await snapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var csharpSourceText = await originalSyntaxTree.GetTextAsync(cancellationToken).ConfigureAwait(false);

        var newText = csharpSourceText.WithChanges(textChanges.Select(c => c.ToTextChange()));
        var newSyntaxTree = originalSyntaxTree.WithChangedText(newText);

        textChangeBuilder.AddDirectlyMappedEdits(textChanges, codeDocument, cancellationToken);

        var oldUsings = await AddUsingsHelper.FindUsingDirectiveStringsAsync(originalSyntaxTree, cancellationToken).ConfigureAwait(false);
        var newUsings = await AddUsingsHelper.FindUsingDirectiveStringsAsync(newSyntaxTree, cancellationToken).ConfigureAwait(false);

        var addedUsings = Delta.Compute(oldUsings, newUsings);
        var removedUsings = Delta.Compute(newUsings, oldUsings);

        textChangeBuilder.AddUsingsChanges(codeDocument, addedUsings, removedUsings, cancellationToken);

        return NormalizeEdits(textChangeBuilder.DrainToOrderedImmutable(), telemetryReporter, cancellationToken);
    }

    /// <summary>
    /// Go through edits and make sure a few things are true:
    ///
    /// <list type="number">
    /// <item>
    ///  No edit is added twice. This can happen if a rename happens.
    /// </item>
    /// <item>
    ///  No edit overlaps with another edit. If they do throw to capture logs but choose the first
    ///  edit to at least not completely fail. It's possible this will need to be tweaked later.
    /// </item>
    /// </list>
    /// </summary>
    private static ImmutableArray<RazorTextChange> NormalizeEdits(ImmutableArray<RazorTextChange> changes, ITelemetryReporter telemetryReporter, CancellationToken cancellationToken)
    {
        // Ensure that the changes are sorted by start position otherwise
        // the normalization logic will not work.
        Debug.Assert(changes.SequenceEqual(changes.OrderBy(static c => c.Span.Start)));

        using var normalizedChanges = new PooledArrayBuilder<RazorTextChange>(changes.Length);
        var remaining = changes.AsSpan();

        var droppedEdits = 0;
        while (remaining is not [])
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (remaining is [var edit, var nextEdit, ..])
            {
                var editSpan = edit.Span.ToTextSpan();
                var nextEditSpan = nextEdit.Span.ToTextSpan();

                if (editSpan == nextEditSpan)
                {
                    normalizedChanges.Add(nextEdit);
                    remaining = remaining[1..];

                    if (edit.NewText != nextEdit.NewText)
                    {
                        droppedEdits++;
                    }
                }
                else if (editSpan.Contains(nextEditSpan))
                {
                    // Cases where there was a removal and addition on the same
                    // line err to taking the addition. This can happen in the
                    // case of a namespace rename
                    if (editSpan.Start == nextEditSpan.Start)
                    {
                        if (string.IsNullOrEmpty(edit.NewText) && !string.IsNullOrEmpty(nextEdit.NewText))
                        {
                            // Don't count this as a dropped edit, it is expected
                            // in the case of a rename
                            normalizedChanges.Add(new RazorTextChange()
                            {
                                Span = edit.Span,
                                NewText = nextEdit.NewText
                            });
                            remaining = remaining[1..];
                        }
                        else
                        {
                            normalizedChanges.Add(edit);
                            remaining = remaining[1..];
                            droppedEdits++;
                        }
                    }
                    else
                    {
                        normalizedChanges.Add(edit);

                        remaining = remaining[1..];
                        droppedEdits++;
                    }
                }
                else if (nextEditSpan.Contains(editSpan))
                {
                    // Add the edit that is contained in the other edit
                    // and skip the next edit.
                    normalizedChanges.Add(nextEdit);
                    remaining = remaining[1..];
                    if (edit.NewText != nextEdit.NewText)
                    {
                        droppedEdits++;
                    }
                }
                else
                {
                    normalizedChanges.Add(edit);
                }
            }
            else
            {
                normalizedChanges.Add(remaining[0]);
            }

            remaining = remaining[1..];
        }

        if (droppedEdits > 0)
        {
            telemetryReporter.ReportFault(
                new DroppedEditsException(),
                "Potentially dropped edits when trying to map",
                new Property("droppedEditCount", droppedEdits));
        }

        return normalizedChanges.ToImmutable();
    }

    private sealed class DroppedEditsException : Exception
    {
    }
}
