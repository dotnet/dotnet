// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Provides external project reference closures.
    /// </summary>
    public interface IExternalProjectReferenceProvider
    {
        /// <summary>
        /// Get the full p2p closure from an msbuild project path.
        /// </summary>
        IReadOnlyList<ExternalProjectReference> GetReferences(string entryPointPath);

        /// <summary>
        /// Returns all known entry points.
        /// </summary>
        IReadOnlyList<ExternalProjectReference> GetEntryPoints();
    }
}
