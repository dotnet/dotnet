// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents all necessary configuration for installing a list of preinstalled packages.
    /// </summary>
    internal sealed class PreinstalledPackageConfiguration
    {
        /// <summary>
        /// Creates a preinstalled package configuration.
        /// </summary>
        /// <param name="repositoryPath">The absolute path to the packages repository on disk.</param>
        /// <param name="packages">The list of packages to be installed.</param>
        /// <param name="isPreunzipped">
        /// A boolean indicating whether the packages are preunzipped within the repository
        /// path.
        /// </param>
        /// <param name="forceDesignTimeBuild">If true, forces a design time build after installing
        /// the packages</param>
        public PreinstalledPackageConfiguration(string repositoryPath,
            IEnumerable<PreinstalledPackageInfo> packages,
            bool isPreunzipped,
            bool forceDesignTimeBuild)
        {
            Packages = packages.ToList().AsReadOnly();
            RepositoryPath = repositoryPath;
            IsPreunzipped = isPreunzipped;
            ForceDesignTimeBuild = forceDesignTimeBuild;
        }

        public IReadOnlyList<PreinstalledPackageInfo> Packages { get; }
        public string RepositoryPath { get; }
        public bool IsPreunzipped { get; }
        public bool ForceDesignTimeBuild { get; }
    }
}
