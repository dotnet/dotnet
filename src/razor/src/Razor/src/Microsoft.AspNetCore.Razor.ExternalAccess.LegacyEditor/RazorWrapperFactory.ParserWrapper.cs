// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using DocumentStructureChangedEventArgsInternal = Microsoft.VisualStudio.LegacyEditor.Razor.Parsing.DocumentStructureChangedEventArgs;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class ParserWrapper(IVisualStudioRazorParser obj) : Wrapper<IVisualStudioRazorParser>(obj), IRazorParser
    {
        private EventHandler<DocumentStructureChangedEventArgs>? _documentStructureChanged;

        public bool HasPendingChanges => Object.HasPendingChanges;

        public event EventHandler<DocumentStructureChangedEventArgs>? DocumentStructureChanged
        {
            add
            {
                // If this is the first handler, hook the inner event.
                if (_documentStructureChanged is null)
                {
                    Object.DocumentStructureChanged += OnDocumentStructureChanged;
                }

                _documentStructureChanged += value;
            }

            remove
            {
                _documentStructureChanged -= value;

                // If there are no more handlers, unhook the inner event.
                if (_documentStructureChanged is null)
                {
                    Object.DocumentStructureChanged -= OnDocumentStructureChanged;
                }
            }
        }

        private void OnDocumentStructureChanged(object sender, DocumentStructureChangedEventArgsInternal e)
        {
            // Be sure to use our wrapper as the sender to avoid leaking the inner object.
            if (_documentStructureChanged is { } handler)
            {
                handler(sender: this, new DocumentStructureChangedEventArgs(
                    ConvertSourceChange(e.SourceChange), e.Snapshot, WrapCodeDocument(e.CodeDocument)));
            }
        }

        public void QueueReparse() => Object.QueueReparse();
    }
}
