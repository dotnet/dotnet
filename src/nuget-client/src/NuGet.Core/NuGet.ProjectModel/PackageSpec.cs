// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NuGet.RuntimeModel;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Represents the specification of a package that can be built.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class PackageSpec
    {
        public static readonly NuGetVersion DefaultVersion = new NuGetVersion(1, 0, 0);

        public PackageSpec(IList<TargetFrameworkInformation> frameworks)
            : this(frameworks, runtimeGraph: null, restoreSettings: null)
        {
        }

        public PackageSpec() : this(new List<TargetFrameworkInformation>())
        {
        }

        internal PackageSpec(
            IList<TargetFrameworkInformation> frameworks,
            RuntimeGraph runtimeGraph,
            ProjectRestoreSettings restoreSettings
            )
        {
            TargetFrameworks = frameworks;
            RuntimeGraph = runtimeGraph ?? RuntimeGraph.Empty;
            RestoreSettings = restoreSettings ?? new ProjectRestoreSettings();
        }

        public string FilePath { get; set; }

        public string BaseDirectory => Path.GetDirectoryName(FilePath);

        public string Name { get; set; }

        private NuGetVersion _version = DefaultVersion;
        public NuGetVersion Version
        {
            get => _version;
            set
            {
                _version = value;
                IsDefaultVersion = false;
            }
        }

        public bool IsDefaultVersion { get; private set; } = true;

        public IList<TargetFrameworkInformation> TargetFrameworks { get; private set; }

        public RuntimeGraph RuntimeGraph { get; set; }

        /// <summary>
        /// Project Settings is used to pass settings like HideWarningsAndErrors down to lower levels.
        /// Currently they do not include any settings that affect the final result of restore.
        /// This should not be part of the Equals and GetHashCode.
        /// Don't write this to the package spec
        /// </summary>
        public ProjectRestoreSettings RestoreSettings { get; set; }

        /// <summary>
        /// Additional MSBuild properties.
        /// </summary>
        /// <remarks>Optional. This is normally set for internal use only.</remarks>
        public ProjectRestoreMetadata RestoreMetadata { get; set; }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(Version);
            hashCode.AddObject(IsDefaultVersion);
            hashCode.AddSequence(TargetFrameworks);
            hashCode.AddObject(RuntimeGraph);
            hashCode.AddObject(RestoreMetadata);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackageSpec);
        }

        public bool Equals(PackageSpec other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Name and FilePath are not used for comparison since they are not serialized to JSON.

            return EqualityUtility.EqualsWithNullCheck(Version, other.Version) &&
                   IsDefaultVersion == other.IsDefaultVersion &&
                   EqualityUtility.OrderedEquals(TargetFrameworks, other.TargetFrameworks, tfm => tfm.TargetAlias, StringComparer.OrdinalIgnoreCase) &&
                   EqualityUtility.EqualsWithNullCheck(RuntimeGraph, other.RuntimeGraph) &&
                   EqualityUtility.EqualsWithNullCheck(RestoreMetadata, other.RestoreMetadata);
        }

        /// <summary>
        /// Clone a PackageSpec
        /// </summary>
        public PackageSpec Clone()
        {
            List<TargetFrameworkInformation> targetFrameworks;
            if (TargetFrameworks is null)
            {
                targetFrameworks = null;
            }
            else
            {
                targetFrameworks = [.. TargetFrameworks];
            }

            return new PackageSpec(
                targetFrameworks,
                RuntimeGraph?.Clone(),
                RestoreSettings?.Clone()
                )
            {
                Name = Name,
                FilePath = FilePath,
                Version = Version,
                IsDefaultVersion = IsDefaultVersion,
                RestoreMetadata = RestoreMetadata?.Clone()
            };
        }
    }
}
