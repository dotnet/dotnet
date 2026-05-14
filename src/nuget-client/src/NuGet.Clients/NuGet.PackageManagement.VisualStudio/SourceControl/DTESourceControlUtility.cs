// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class DTESourceControlUtility
    {
        public static void EnsureCheckedOutIfExists(SourceControl sourceControl, string fullPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (File.Exists(fullPath))
            {
                FileSystemUtility.MakeWritable(fullPath);

                if (sourceControl != null
                    &&
                    sourceControl.IsItemUnderSCC(fullPath)
                    &&
                    !sourceControl.IsItemCheckedOut(fullPath))
                {
                    // Check out the item
                    sourceControl.CheckOutItem(fullPath);
                }
            }
        }
    }
}
