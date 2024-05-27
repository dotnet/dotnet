// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents metadata associated with a single reference item, e.g. project or package.
    /// </summary>
    public interface IVsReferenceItem2
    {
        /// <summary>
        /// The item's name. In MSBuild, this is the Identifier metadata.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Collection of item metadata.
        /// </summary>
        IReadOnlyDictionary<string, string?>? Metadata { get; }
    }
}
