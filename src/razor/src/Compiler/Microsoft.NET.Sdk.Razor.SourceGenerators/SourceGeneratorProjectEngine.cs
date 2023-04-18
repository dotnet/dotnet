// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal class SourceGeneratorProjectEngine : DefaultRazorProjectEngine
{
    private readonly int discoveryPhaseIndex = -1;

    private readonly int rewritePhaseIndex = -1;

    public SourceGeneratorProjectEngine(DefaultRazorProjectEngine projectEngine)
        : base(projectEngine.Configuration, projectEngine.Engine, projectEngine.FileSystem, projectEngine.ProjectFeatures)
    {
        for (int i = 0; i < Engine.Phases.Count; i++)
        {
            if (Engine.Phases[i] is DefaultRazorTagHelperContextDiscoveryPhase)
            {
                discoveryPhaseIndex = i;
            }
            else if (Engine.Phases[i] is DefaultRazorTagHelperRewritePhase)
            {
                rewritePhaseIndex = i;
            }
            else if (discoveryPhaseIndex >= 0 && rewritePhaseIndex >= 0)
            {
                break;
            }
        }
        Debug.Assert(discoveryPhaseIndex >= 0);
        Debug.Assert(rewritePhaseIndex >= 0);
    }

    public SourceGeneratorRazorCodeDocument ProcessInitialParse(RazorProjectItem projectItem)
    {
        var codeDocument = CreateCodeDocumentCore(projectItem);
        ProcessPartial(codeDocument, 0, discoveryPhaseIndex);

        // record the syntax tree, before the tag helper re-writing occurs
        codeDocument.SetPreTagHelperSyntaxTree(codeDocument.GetSyntaxTree());
        return new SourceGeneratorRazorCodeDocument(codeDocument);
    }

    public SourceGeneratorRazorCodeDocument ProcessTagHelpers(SourceGeneratorRazorCodeDocument sgDocument, IReadOnlyList<TagHelperDescriptor> tagHelpers, bool checkForIdempotency)
    {
        Debug.Assert(sgDocument.CodeDocument.GetPreTagHelperSyntaxTree() is not null);

        int startIndex = discoveryPhaseIndex;
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
                var oldContextHelpers = codeDocument.GetTagHelperContext().TagHelpers;

                // re-run the scope check to figure out which tag helpers this document can see
                codeDocument.SetTagHelpers(tagHelpers);
                Engine.Phases[discoveryPhaseIndex].Execute(codeDocument);

                // Check if any new tag helpers were added or ones we previously used were removed
                var newContextHelpers = codeDocument.GetTagHelperContext().TagHelpers;
                var added = newContextHelpers.Except(oldContextHelpers);
                var referencedByRemoved = codeDocument.GetReferencedTagHelpers().Except(newContextHelpers);
                if (!added.Any() && !referencedByRemoved.Any())
                {
                    //  Either nothing new, or any that got removed weren't used by this document anyway
                    return sgDocument;
                }

                // We need to re-write the document, but can skip the scoping as we just performed it
                startIndex = rewritePhaseIndex;
            }
        }
        else
        {
            codeDocument.SetTagHelpers(tagHelpers);
        }

        ProcessPartial(codeDocument, startIndex, rewritePhaseIndex + 1);
        return new SourceGeneratorRazorCodeDocument(codeDocument);
    }

    public SourceGeneratorRazorCodeDocument ProcessRemaining(SourceGeneratorRazorCodeDocument sgDocument)
    {
        var codeDocument = sgDocument.CodeDocument;
        Debug.Assert(codeDocument.GetReferencedTagHelpers() is not null);

        ProcessPartial(sgDocument.CodeDocument, rewritePhaseIndex, Engine.Phases.Count);
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
