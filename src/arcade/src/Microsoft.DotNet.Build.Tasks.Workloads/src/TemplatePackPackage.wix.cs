// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Represents a template pack.
    /// </summary>
    internal class TemplatePackPackage : WorkloadPackPackage
    {
        /// <inheritdoc />
        public override PackageExtractionMethod ExtractionMethod => PackageExtractionMethod.Copy;

        public TemplatePackPackage(WorkloadPack pack, string packagePath, string[] platforms,
            string destinationBaseDirectory, ITaskItem[]? shortNames = null, TaskLoggingHelper? log = null) :
            base(pack, packagePath, platforms, destinationBaseDirectory, shortNames, log)
        { }
    }
}

#nullable disable
