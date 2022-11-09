// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class TargetFrameworkInformation : IEquatable<TargetFrameworkInformation>
    {
        public string TargetAlias { get; set; } = string.Empty;

        public NuGetFramework FrameworkName { get; set; }

        public IList<LibraryDependency> Dependencies { get; set; } = new List<LibraryDependency>();

        /// <summary>
        /// A fallback PCL framework to use when no compatible items
        /// were found for <see cref="FrameworkName"/>.
        /// </summary>
        public IList<NuGetFramework> Imports { get; set; } = new List<NuGetFramework>();

        /// <summary>
        /// If True AssetTargetFallback behavior will be used for Imports.
        /// </summary>
        public bool AssetTargetFallback { get; set; }

        /// <summary>
        /// Display warnings when the Imports framework is used.
        /// </summary>
        public bool Warn { get; set; }

        /// <summary>
        /// List of dependencies that are not part of the graph resolution.
        /// </summary>
        public IList<DownloadDependency> DownloadDependencies { get; } = new List<DownloadDependency>();

        /// <summary>
        /// List of the package versions defined in the Central package versions management file. 
        /// </summary>
        public IDictionary<string, CentralPackageVersion> CentralPackageVersions { get; } = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// A set of unique FrameworkReferences
        /// </summary>
        public ISet<FrameworkDependency> FrameworkReferences { get; } = new HashSet<FrameworkDependency>();

        /// <summary>
        /// The project provided runtime.json
        /// </summary>
        public string RuntimeIdentifierGraphPath { get; set; }

        public override string ToString()
        {
            return FrameworkName.GetShortFolderName();
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(FrameworkName);
            hashCode.AddObject(AssetTargetFallback);
            hashCode.AddSequence(Dependencies.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase));
            hashCode.AddSequence(Imports);
            hashCode.AddObject(Warn);
            hashCode.AddSequence(DownloadDependencies.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase));
            hashCode.AddSequence(FrameworkReferences.OrderBy(s => s.Name, ComparisonUtility.FrameworkReferenceNameComparer));
            if (RuntimeIdentifierGraphPath != null)
            {
                hashCode.AddObject(PathUtility.GetStringComparerBasedOnOS().GetHashCode(RuntimeIdentifierGraphPath));
            }
            hashCode.AddSequence(CentralPackageVersions.Values.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase));
            hashCode.AddStringIgnoreCase(TargetAlias);
            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TargetFrameworkInformation);
        }

        public bool Equals(TargetFrameworkInformation other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityUtility.EqualsWithNullCheck(FrameworkName, other.FrameworkName) &&
                   EqualityUtility.OrderedEquals(Dependencies, other.Dependencies, dependency => dependency.Name, StringComparer.OrdinalIgnoreCase) &&
                   Imports.SequenceEqualWithNullCheck(other.Imports) &&
                   Warn == other.Warn &&
                   AssetTargetFallback == other.AssetTargetFallback &&
                   EqualityUtility.OrderedEquals(DownloadDependencies, other.DownloadDependencies, e => e.Name, StringComparer.OrdinalIgnoreCase) &&
                   EqualityUtility.OrderedEquals(FrameworkReferences, other.FrameworkReferences, e => e.Name, ComparisonUtility.FrameworkReferenceNameComparer) &&
                   EqualityUtility.OrderedEquals(CentralPackageVersions.Values, other.CentralPackageVersions.Values, e => e.Name, StringComparer.OrdinalIgnoreCase) &&
                   PathUtility.GetStringComparerBasedOnOS().Equals(RuntimeIdentifierGraphPath, other.RuntimeIdentifierGraphPath) &&
                   StringComparer.OrdinalIgnoreCase.Equals(TargetAlias, other.TargetAlias);
        }

        public TargetFrameworkInformation Clone()
        {
            var clonedObject = new TargetFrameworkInformation();
            clonedObject.FrameworkName = FrameworkName;
            clonedObject.Dependencies = Dependencies.Select(item => item.Clone()).ToList();
            clonedObject.Imports = new List<NuGetFramework>(Imports);
            clonedObject.AssetTargetFallback = AssetTargetFallback;
            clonedObject.Warn = Warn;
            clonedObject.DownloadDependencies.AddRange(DownloadDependencies.Select(item => item.Clone()));
            clonedObject.FrameworkReferences.AddRange(FrameworkReferences);
            clonedObject.RuntimeIdentifierGraphPath = RuntimeIdentifierGraphPath;
            clonedObject.CentralPackageVersions.AddRange(CentralPackageVersions.ToDictionary(item => item.Key, item => item.Value));
            clonedObject.TargetAlias = TargetAlias;
            return clonedObject;
        }
    }
}
