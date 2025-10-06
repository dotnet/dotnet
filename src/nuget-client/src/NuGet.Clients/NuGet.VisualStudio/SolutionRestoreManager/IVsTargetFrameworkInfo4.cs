// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// A type representing all project inputs (items and properties) for the given target framework.
    /// It is implemented by project systems sending nominations to NuGet.
    /// </summary>
    public interface IVsTargetFrameworkInfo4
    {
        /// <summary>
        /// Collection of item types. The dictionary key is the MSBuild item type (for example, PackageReference), and
        /// the value is the list of items of that item type.
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyList<IVsReferenceItem2>>? Items { get; }

        /// <summary>
        /// Collection of project level properties evaluated per each Target Framework,
        /// e.g. PackageTargetFallback.
        /// </summary>
        /// <remarks>
        /// The dictionary should be <see cref="System.StringComparer.OrdinalIgnoreCase"/> for key comparisons to match MSBuild's behaviour.
        /// </remarks>
        IReadOnlyDictionary<string, string> Properties { get; }
    }
}
