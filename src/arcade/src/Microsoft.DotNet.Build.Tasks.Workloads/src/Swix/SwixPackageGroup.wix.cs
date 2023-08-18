// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.DotNet.Releases;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Represents a Visual Studio package group. Package groups are non-selectable
    /// entities
    /// </summary>
    internal class SwixPackageGroup : SwixPackageBase
    {
        /// <summary>
        /// The SDK feature band associated with this component.
        /// </summary>
        public ReleaseVersion SdkFeatureBand
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="SwixPackageGroup"/> instance.
        /// </summary>
        /// <param name="name">The name of the package group.</param>
        /// <param name="version">The version of the package group</param>
        public SwixPackageGroup(ReleaseVersion sdkFeatureBand, string name, Version version) : base(name, version)
        {
            SdkFeatureBand = sdkFeatureBand;
        }

        public static SwixPackageGroup Create(WorkloadManifestPackage manifestPackage)
        {
            var packageGroup = new SwixPackageGroup(manifestPackage.SdkFeatureBand, manifestPackage.SwixPackageGroupId, manifestPackage.MsiVersion);

            packageGroup.Dependencies.Add(new SwixDependency(manifestPackage.SwixPackageId, manifestPackage.MsiVersion));

            return packageGroup;
        }
    }
}
