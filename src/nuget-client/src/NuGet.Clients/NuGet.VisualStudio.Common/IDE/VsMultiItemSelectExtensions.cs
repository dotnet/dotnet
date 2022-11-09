// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    public static class VsMultiItemSelectExtensions
    {
        public static VSITEMSELECTION[] GetSelectedItemsInSingleHierachy(this IVsMultiItemSelect multiItemSelect)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!ErrorHandler.Succeeded(multiItemSelect.GetSelectionInfo(out uint numberOfSelectedItems, out int isSingleHierarchyInt)) || isSingleHierarchyInt == 0)
            {
                return null;
            }

            VSITEMSELECTION[] vsItemSelections = new VSITEMSELECTION[numberOfSelectedItems];
            uint flags = 0; // No flags, which will give us back a hierarchy for each item
            if (!ErrorHandler.Succeeded(multiItemSelect.GetSelectedItems(flags, numberOfSelectedItems, vsItemSelections)))
            {
                return null;
            }

            return vsItemSelections;
        }
    }
}
