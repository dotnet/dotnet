// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Represents a Visual Studio setup dependency.
    /// </summary>
    public class SwixDependency
    {
        /// <summary>
        /// The SWIX package ID of the dependency.
        /// </summary>
        public string Id
        {
            get;            
        }

        /// <summary>
        /// The minimum dependency version.
        /// </summary>
        public Version? MinVersion
        {
            get;
        }

        /// <summary>
        /// The maxmimum dependency version.
        /// </summary>
        public Version? MaxVersion
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="SwixDependency" /> dependency with an exact version.
        /// </summary>
        /// <param name="id">The SWIX package ID. The ID applies to packages, components, component groups, etc.</param>
        /// <param name="version">The exact version of the dependency.</param>
        public SwixDependency(string id, Version version) : this(id, version, version)
        {

        }
        
        /// <summary>
        /// Creates a dependency with a minimum and/or maximum version.
        /// </summary>
        /// <param name="id">The SWIX package ID. The ID applies to packages, components, component groups, etc.</param>
        /// <param name="minVersion">The minimum required version, inclusive. May be <see langword="null"/> if only an upper bound is required.</param>
        /// <param name="maxVersion">The maximum version, exclusive. May be <see langword="null"/> if only a lower bound is required.
        /// If equal to <paramref name="minVersion"/>, an exact version requirement is created, e.g. [1.2.0].</param>
        public SwixDependency(string id, Version? minVersion, Version? maxVersion)
        {
            if ((minVersion == null) && (maxVersion == null))
            {
                throw new ArgumentException(Strings.SwixDependencyVersionRequired);
            }

            if ((maxVersion != null) && (minVersion != null) && (maxVersion < minVersion))
            {
                throw new ArgumentException(Strings.SwixDependencyMaxVersionLessThanMinVersion);
            }

            Id = id;
            MinVersion = minVersion;
            MaxVersion = maxVersion;
        }

        /// <summary>
        /// Gets a string describing the version range of the dependency.
        /// </summary>
        /// <returns></returns>
        public string GetVersionRange()
        {
            if ((MinVersion != null) && (MinVersion == MaxVersion))
            {
                return $"[{MinVersion}]";
            }


            return $"[{MinVersion?.ToString()},{MaxVersion?.ToString()})";
        }
    }
}

#nullable disable
