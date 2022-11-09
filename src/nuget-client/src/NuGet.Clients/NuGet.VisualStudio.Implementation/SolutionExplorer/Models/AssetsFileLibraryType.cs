// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Enumeration of types of library found in the assets file.
    /// </summary>
    internal enum AssetsFileLibraryType : byte
    {
        Package,
        Project
    }
}
