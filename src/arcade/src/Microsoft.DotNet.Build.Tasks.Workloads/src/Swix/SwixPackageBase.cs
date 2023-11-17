// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Base class representing a Visual Studio SWIX package.
    /// </summary>
    internal abstract class SwixPackageBase
    {
        public IList<SwixDependency> Dependencies
        {
            get;
        } = new List<SwixDependency>();

        public bool HasDependencies => Dependencies.Count > 0;

        /// <summary>
        /// The name (ID) of the SWIX package.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// The version of the SWIX package.
        /// </summary>
        public Version Version
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="SwixPackageBase"/> instance.
        /// </summary>
        /// <param name="name">The name (ID) of the package.</param>
        /// <param name="version">The version of the package.</param>
        public SwixPackageBase(string name, Version version)
        {
            Name = name;
            Version = version;
        }
    }
}
