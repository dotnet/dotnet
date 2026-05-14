// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.ProjectModel
{
    public enum PackageDependencyType
    {
        /// <summary>
        /// Package is directly installed into the project.
        /// </summary>
        Direct = 0,

        /// <summary>
        /// Package is transitively available to the project instead of directly installed.
        /// </summary>
        Transitive = 1,

        /// <summary>
        /// 
        /// </summary>
        Project = 2,

        /// <summary>
        /// Package is transitively available to the project instead of directly installed but is also enforced by the central package version file.
        /// </summary>
        CentralTransitive = 3
    }
}
