// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.CommandLine.XPlat
{
    /// <summary>
    /// A class to simplify holding all of the information
    /// of a framework packages when using list
    /// </summary>
    internal class FrameworkPackages
    {
        public string Framework { get; }
        public IEnumerable<InstalledPackageReference> TopLevelPackages { get; set; }
        public IEnumerable<InstalledPackageReference> TransitivePackages { get; set; }

        /// <summary>
        /// A constructor that takes a framework name, and
        /// initializes the top-level and transitive package
        /// lists
        /// </summary>
        /// <param name="framework">Framework name</param>
        public FrameworkPackages(string framework) : this(framework, new List<InstalledPackageReference>(), new List<InstalledPackageReference>())
        {

        }

        /// <summary>
        /// A constructor that takes a framework name, a list
        /// of top-level packages, and a list of transitive
        /// packages
        /// </summary>
        /// <param name="framework">Framework name that we have packages for</param>
        /// <param name="topLevelPackages">Top-level packages. Shouldn't be null</param>
        /// <param name="transitivePackages">Transitive packages. Shouldn't be null</param>
        public FrameworkPackages(string framework,
            IEnumerable<InstalledPackageReference> topLevelPackages,
            IEnumerable<InstalledPackageReference> transitivePackages)
        {
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            TopLevelPackages = topLevelPackages ?? throw new ArgumentNullException(nameof(topLevelPackages));
            TransitivePackages = transitivePackages ?? throw new ArgumentNullException(nameof(transitivePackages));
        }
    }
}
