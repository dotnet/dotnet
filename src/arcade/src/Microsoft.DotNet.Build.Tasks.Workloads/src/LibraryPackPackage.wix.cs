// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Represents a library pack.
    /// </summary>
    internal class LibraryPackPackage : WorkloadPackPackage
    {
        /// <inheritdoc />
        public override PackageExtractionMethod ExtractionMethod => PackageExtractionMethod.Copy;

        public LibraryPackPackage(WorkloadPack pack, string packagePath, string[] platforms,
            string destinationBaseDirectory, ITaskItem[]? shortNames = null, TaskLoggingHelper? log = null) :
            base(pack, packagePath, platforms, destinationBaseDirectory, shortNames, log)
        {

        }
    }
}

#nullable disable
