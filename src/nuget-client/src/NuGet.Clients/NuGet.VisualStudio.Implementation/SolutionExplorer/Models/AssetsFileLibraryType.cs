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

        Project,

        /// <summary>
        /// When it is unknown whether the library represents a package or a project.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This occurs, for example, when a referenced item is not found. It will be present in the lock file's
        /// messages, but not elsewhere in the lock file.
        /// </para>
        /// <para>
        /// This enum member exists only to support attaching diagnostic items to their package or project reference
        /// node in the tree.
        /// </para>
        /// </remarks>
        Unknown
    }
}
