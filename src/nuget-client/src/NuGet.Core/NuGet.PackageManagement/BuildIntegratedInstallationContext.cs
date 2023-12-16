// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement.Projects;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Information used by <see cref="BuildIntegratedNuGetProject"/> when installing a package.
    /// </summary>
    public class BuildIntegratedInstallationContext
    {
        public BuildIntegratedInstallationContext(
            IEnumerable<NuGetFramework> successfulFrameworks,
            IEnumerable<NuGetFramework> unsucessfulFrameworks,
            IDictionary<NuGetFramework, string> originalFrameworks,
            bool areAllPackagesConditional)
        {
            SuccessfulFrameworks = successfulFrameworks;
            UnsuccessfulFrameworks = unsucessfulFrameworks;
            OriginalFrameworks = originalFrameworks;
            AreAllPackagesConditional = areAllPackagesConditional;
        }

        public BuildIntegratedInstallationContext(
            IEnumerable<NuGetFramework> successfulFrameworks,
            IEnumerable<NuGetFramework> unsucessfulFrameworks,
            IDictionary<NuGetFramework, string> originalFrameworks)
            : this(successfulFrameworks, unsucessfulFrameworks, originalFrameworks, areAllPackagesConditional: false)
        { }

        /// <summary>
        /// Shows the frameworks for which a preview restore operation was successful. Only use it
        /// in case of single package install case, and only for CpsPackageReference projects.
        /// </summary>
        public IEnumerable<NuGetFramework> SuccessfulFrameworks { get; }

        /// <summary>
        /// Shows the frameworks for which a preview restore operation was unsuccessful. Only use it
        /// in case of single package install case, and only for CpsPackageReference projects.
        /// </summary>
        public IEnumerable<NuGetFramework> UnsuccessfulFrameworks { get; }

        /// <summary>
        /// A mapping to allow the original framework string to fetched. This is important because MSBuild target
        /// framework evaluation depends on the target framework string matching exactly.
        /// </summary>
        public IDictionary<NuGetFramework, string> OriginalFrameworks { get; }

        /// <summary>
        /// Indicates that all packages are suspected to be conditionally installed. In particular, a package is installed to all frameworks, but it is expected to use the conditional updating/unisntalling APIs.
        /// Indicators of packages being conditional is that a package contains different versions in different frameworks.
        /// This value is only relevant when <see cref="UnsuccessfulFrameworks"/> is empty, and <see cref="SuccessfulFrameworks"/> contains all frameworks.
        /// </summary>
        public bool AreAllPackagesConditional { get; }

        /// <summary>
        /// Define transitive behavior for each package dependency for the current project.
        /// </summary>
        public LibraryIncludeFlags SuppressParent { get; set; } = LibraryIncludeFlagUtils.DefaultSuppressParent;

        /// <summary>
        /// Define what all sections of the current package to include in this project.
        /// </summary>
        public LibraryIncludeFlags IncludeType { get; set; } = LibraryIncludeFlags.All;
    }
}
