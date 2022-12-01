// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.DragDrop;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

// The intention of this class is to disable dropping random files into the Razor language service content type without throwing. Ultimately
// this class serves as a workaround to a limitation in the core editor APIs where without it you get an error dialog. This class allows us
// to silently "do nothing" when a drop occurs on one of our documents.
[Export(typeof(IDropHandlerProvider))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[DropFormat(RazorVisualStudioWindowsConstants.VSProjectItemsIdentifier)]
[Name(nameof(RazorDisableDropHandlerProvider))]
[Order(Before = "LanguageServiceTextDropHandler")]
internal sealed class RazorDisableDropHandlerProvider : IDropHandlerProvider
{
    public IDropHandler GetAssociatedDropHandler(IWpfTextView wpfTextView) => new DisabledDropHandler();

    private sealed class DisabledDropHandler : IDropHandler
    {
        public DragDropPointerEffects HandleDataDropped(DragDropInfo dragDropInfo)
        {
            return DragDropPointerEffects.None;
        }

        public void HandleDragCanceled()
        {
        }

        public DragDropPointerEffects HandleDraggingOver(DragDropInfo dragDropInfo)
        {
            return DragDropPointerEffects.None;
        }

        public DragDropPointerEffects HandleDragStarted(DragDropInfo dragDropInfo)
        {
            return DragDropPointerEffects.None;
        }

        public bool IsDropEnabled(DragDropInfo dragDropInfo)
        {
            // We specifically return true here because the default handling (what would be used if we returned false) of drag & drop ends up resulting in an error dialog.
            return true;
        }
    }
}
