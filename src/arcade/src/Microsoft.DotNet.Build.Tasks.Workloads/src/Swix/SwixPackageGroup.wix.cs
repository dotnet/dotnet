// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Deployment.DotNet.Releases;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Represents a Visual Studio package group. Package groups are non-selectable
    /// entities that can provide stable identities when inserting non-stable packages into Visual Studio. Other
    /// package types like components can then reference the stable package group even when the underlying
    /// package identity keeps changing.
    /// </summary>
    internal class SwixPackageGroup : SwixPackageBase
    {
        /// <summary>
        /// The SDK feature band associated with the package group. The feature band is used for partition builds
        /// and grouping SWIX manifests.
        /// </summary>
        public ReleaseVersion SdkFeatureBand
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="SwixPackageGroup"/> for a workload manifest package.
        /// </summary>
        /// <param name="package">The package to reference from the package group.</param>
        public SwixPackageGroup(WorkloadManifestPackage package) : this(package, package.SdkFeatureBand, package.SwixPackageGroupId)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SwixPackageGroup"/> for a workload set package.
        /// </summary>
        /// <param name="package">The package to reference from the package group.</param>
        public SwixPackageGroup(WorkloadSetPackage package) : this(package, package.SdkFeatureBand, package.SwixPackageGroupId)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SwixPackageGroup"/> and adds the specified package as a dependency.
        /// </summary>
        /// <param name="package">The package to reference from the package group.</param>
        /// <param name="sdkFeatureBand">The SDK feature band to associate with the group.</param>
        /// <param name="packageGroupName">The name (ID) of the package group.</param>
        private SwixPackageGroup(WorkloadPackageBase package, ReleaseVersion sdkFeatureBand, string packageGroupName) :
            base(packageGroupName, package.MsiVersion)
        {
            SdkFeatureBand = sdkFeatureBand;
            Dependencies.Add(new SwixDependency(package.SwixPackageId, package.MsiVersion));
        }
    }
}
