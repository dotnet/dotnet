// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Contracts
{
    /// <summary>Information about an installed package</summary>
    /// <remarks>To create an instance, use <see cref="NuGetContractsFactory.CreateNuGetInstalledPackage"/>.</remarks>
    public sealed class NuGetInstalledPackage
    {
        /// <summary>The package id.</summary>
        public string Id { get; }

        /// <summary>The project's requested package range for the package.</summary>
        /// <remarks>
        /// If the project uses packages.config, this will be same as the installed package version. <br/>
        /// If the project uses PackageReference, this is the version string in the project file, which may not match the resolved package version, and may be a range, not a single version.<br/>
        /// If the project uses PackageReference, and the package is a transitive dependency, the value will be null.
        /// </remarks>
        public string RequestedRange { get; }

        /// <summary>The installed package version</summary>
        /// <remarks>
        /// If the project uses packages.config, this will be the same as requested range.
        /// <para>If the project uses PackageReference, this will be the resolved version, if the project has been restored successfully.
        /// In some error conditions this may be an empty string.</para>
        /// </remarks>
        public string Version { get; }

        /// <summary>Path to the extracted package</summary>
        /// <remarks>
        /// This may be null if the package was not restored successfully.
        /// </remarks>
        public string InstallPath { get; }

        /// <summary>This package a direct dependency of the project</summary>
        /// <remarks>
        /// packages.config do not support transitive dependencies, so all packages will have this property set to true, even if the package was installed because it was a dependency of a package that the customer selected for install.
        /// </remarks>
        public bool DirectDependency { get; }

        // This class will hopefully use C# record types when that language feature becomes available, so make the constructor not-public, to prevent breaking change when records come out.
        internal NuGetInstalledPackage(string id, string requestedRange, string version, string installPath, bool directDependency)
        {
            Id = id;
            RequestedRange = requestedRange;
            Version = version;
            InstallPath = installPath;
            DirectDependency = directDependency;
        }
    }
}
