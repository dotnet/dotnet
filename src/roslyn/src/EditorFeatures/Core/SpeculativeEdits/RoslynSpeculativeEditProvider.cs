﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Editor;
using Microsoft.CodeAnalysis.Editor.Shared.Preview;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Internal.Proposals;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.CodeAnalysis.SpeculativeEdits;

// The entire SpeculativeEdit api is marked as obsolete since this is a preview API.  So we do the same here as well.
[Obsolete("This is a preview api and subject to change")]
[Export(typeof(SpeculativeEditProvider))]
[ContentType(ContentTypeNames.RoslynContentType)]
[method: ImportingConstructor]
[method: Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
internal sealed class RoslynSpeculativeEditProvider(
    ITextBufferFactoryService3 textBufferFactoryService) : SpeculativeEditProvider(textBufferFactoryService)
{
    public override ISpeculativeEditSession? TryStartSpeculativeEditSession(SpeculativeEditOptions options)
    {
        var oldTextSnapshot = options.SourceSnapshot;
        var document = oldTextSnapshot.GetOpenDocumentInCurrentContextWithChanges();
        if (document is null)
            return null;

        var documentId = document.Id;

        // Clone the existing text into a new editor snapshot/buffer that we can fork independently of the original.
        var clonedSnapshotBeforeEdits = this.CloneWithEdits(options);

        // Grab the ITextSnapshot of this cloned buffer before making any changes.  The SpeculativeEdit api needs it
        // as part of the returned set of values.
        var clonedBuffer = clonedSnapshotBeforeEdits.TextBuffer;

        // Now create a forked solution that takes the original document and updates it to point at the current state
        // of the text buffer with the edits applied.  Ensure that this properly updates linked files as well so everything
        // is consistent.
        var newSolution = document.Project.Solution.WithDocumentText(
            document.Project.Solution.GetRelatedDocumentIds(documentId),
            clonedBuffer.AsTextContainer().CurrentText,
            PreservationMode.PreserveIdentity);

        // Now, create a preview workspace with that forked document opened within it so that we can lightup features properly there.
        var previewWorkspace = new PreviewWorkspace(newSolution);
        previewWorkspace.OpenDocument(documentId, clonedBuffer.AsTextContainer());

        // Wrap everything we need into a final ISpeculativeEditSession for the caller.  It owns the lifetime of the data
        // and will dispose it when done. At that point, we can release the allocated preview workspace new 
        return new RoslynSpeculativeEditSession(
            options,
            clonedSnapshotBeforeEdits,
            previewWorkspace);
    }

    private sealed class RoslynSpeculativeEditSession(
        SpeculativeEditOptions options,
        ITextSnapshot clonedSnapshotBeforeEdits,
        PreviewWorkspace previewWorkspace) : ISpeculativeEditSession
    {
        public ITextSnapshot ClonedSnapshot { get; } = clonedSnapshotBeforeEdits;

        public SpeculativeEditOptions CreationOptions { get; } = options;

        public void Dispose()
            => previewWorkspace.Dispose();
    }
}
