// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Describes how a workload pack's contents should be handled when
    /// creating an installer from the underlying NuGet package.
    /// </summary>
    public enum PackageExtractionMethod
    {
        /// <summary>
        /// The package contents is extracted and package metadata files and folders will be removed.
        /// </summary>
        Unzip = 0,

        /// <summary>
        /// The package contents is not extracted. The underlying package will be copied to the destination
        /// location instead.
        /// </summary>
        Copy = 1,
    }
}
