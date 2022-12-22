// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.Telemetry
{
    /// <summary>
    /// The different types of NuGet projects.
    /// </summary>
    public enum NuGetProjectType
    {
        /// <summary>
        /// Used if the project does not support NuGet.
        /// </summary>
        Unsupported = 0,

        /// <summary>
        /// Used if the <see cref="NuGetProject"/> is not a recognized type.
        /// </summary>
        Unknown = 1,

        /// <summary>
        /// Corresponds to <see cref="MSBuildNuGetProject"/>.
        /// </summary>
        PackagesConfig = 2,

        /// <summary>
        /// Corresponds to <see cref="BuildIntegratedNuGetProject"/>.
        /// </summary>
        UwpProjectJson = 3,

        /// <summary>
        /// Corresponds to <see cref="MSBuildShellOutNuGetProject"/>.
        /// </summary>
        CPSBasedPackageRefs = 5,

        /// <summary>
        /// It will be used for legacy project system with package references.
        /// </summary>
        LegacyProjectSystemWithPackageRefs = 6,

        /// <summary>
        /// Used when no NuGet package has been installed into the project yet but it still a supported and known project type.
        /// and we by default create a <see cref="MSBuildNuGetProject"/>
        /// </summary>
        UnconfiguredNuGetType = 7,
    }
}
