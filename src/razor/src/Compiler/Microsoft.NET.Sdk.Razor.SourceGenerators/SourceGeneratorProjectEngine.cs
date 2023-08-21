// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class SourceGeneratorProjectEngine : DefaultRazorProjectEngine
{
    private readonly int _discoveryPhaseIndex = -1;

    private readonly int _rewritePhaseIndex = -1;

    public SourceGeneratorProjectEngine(DefaultRazorProjectEngine projectEngine)
        : base(projectEngine.Configuration, projectEngine.Engine, projectEngine.FileSystem, projectEngine.ProjectFeatures)
    {
        for (int i = 0; i < Engine.Phases.Count; i++)
        {
            if (Engine.Phases[i] is DefaultRazorTagHelperContextDiscoveryPhase)
            {
                _discoveryPhaseIndex = i;
            }
            else if (Engine.Phases[i] is DefaultRazorTagHelperRewritePhase)
            {
                _rewritePhaseIndex = i;
            }
            else if (_discoveryPhaseIndex >= 0 && _rewritePhaseIndex >= 0)
            {
                break;
            }
        }
        Debug.Assert(_discoveryPhaseIndex >= 0);
        Debug.Assert(_rewritePhaseIndex >= 0);
        Debug.Assert(_discoveryPhaseIndex < _rewritePhaseIndex);
    }

    public SourceGeneratorRazorCodeDocument ProcessInitialParse(RazorProjectItem projectItem, bool designTime)
    {
        var codeDocument = designTime 
                            ? CreateCodeDocumentDesignTimeCore(projectItem)
                            : CreateCodeDocumentCore(projectItem);


        ProcessPartial(codeDocument, 0, _discoveryPhaseIndex);

        // record the syntax tree, before the tag helper re-writing occurs
        codeDocument.SetPreTagHelperSyntaxTree(codeDocument.GetSyntaxTree());
        return new SourceGeneratorRazorCodeDocument(codeDocument);
    }

    public SourceGeneratorRazorCodeDocument ProcessTagHelpers(SourceGeneratorRazorCodeDocument sgDocument, IReadOnlyList<TagHelperDescriptor> tagHelpers, bool checkForIdempotency)
    {
        Debug.Assert(sgDocument.CodeDocument.GetPreTagHelperSyntaxTree() is not null);

        int startIndex = _discoveryPhaseIndex;
        var codeDocument = sgDocument.CodeDocument;
        var previousTagHelpers = codeDocument.GetTagHelpers();
        if (checkForIdempotency && previousTagHelpers is not null)
        {
            // compare the tag helpers with the ones the document last used
            if (Enumerable.SequenceEqual(tagHelpers, previousTagHelpers))
            {
                // tag helpers are the same, nothing to do!
                return sgDocument;
            }
            else
            {
                // tag helpers have changed, figure out if we need to re-write
                var previousTagHelpersInScope = codeDocument.GetTagHelperContext().TagHelpers;
                var previousUsedTagHelpers = codeDocument.GetReferencedTagHelpers();

                // re-run discovery to figure out which tag helpers are now in scope for this document
                codeDocument.SetTagHelpers(tagHelpers);
                Engine.Phases[_discoveryPhaseIndex].Execute(codeDocument);
                var tagHelpersInScope = codeDocument.GetTagHelperContext().TagHelpers;

                // Check if any new tag helpers were added or ones we previously used were removed
                var newVisibleTagHelpers = tagHelpersInScope.Except(previousTagHelpersInScope); 
                var newUnusedTagHelpers = previousUsedTagHelpers.Except(tagHelpersInScope);
                if (!newVisibleTagHelpers.Any() && !newUnusedTagHelpers.Any())
                {
                    // No newly visible tag helpers, and any that got removed weren't used by this document anyway
                    return sgDocument;
                }

                // We need to re-write the document, but can skip the scoping as we just performed it
                startIndex = _rewritePhaseIndex;
            }
        }
        else
        {
            codeDocument.SetTagHelpers(tagHelpers);
        }

        ProcessPartial(codeDocument, startIndex, _rewritePhaseIndex + 1);
        return new SourceGeneratorRazorCodeDocument(codeDocument);
    }

    public SourceGeneratorRazorCodeDocument ProcessRemaining(SourceGeneratorRazorCodeDocument sgDocument)
    {
        var codeDocument = sgDocument.CodeDocument;
        Debug.Assert(codeDocument.GetReferencedTagHelpers() is not null);

        ProcessPartial(sgDocument.CodeDocument, _rewritePhaseIndex, Engine.Phases.Count);
        return new SourceGeneratorRazorCodeDocument(codeDocument);
    }

    private void ProcessPartial(RazorCodeDocument codeDocument, int startIndex, int endIndex)
    {
        Debug.Assert(startIndex >= 0 && startIndex <= endIndex && endIndex <= Engine.Phases.Count);
        for (var i = startIndex; i < endIndex; i++)
        {
            Engine.Phases[i].Execute(codeDocument);
        }
    }
}