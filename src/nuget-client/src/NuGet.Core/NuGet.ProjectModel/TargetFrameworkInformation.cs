// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class TargetFrameworkInformation : IEquatable<TargetFrameworkInformation>
    {
        public string TargetAlias { get; set; }

        public NuGetFramework FrameworkName { get; set; }

        public IList<LibraryDependency> Dependencies { get; set; }

        /// <summary>
        /// A fallback PCL framework to use when no compatible items
        /// were found for <see cref="FrameworkName"/>.
        /// </summary>
        public IList<NuGetFramework> Imports { get; set; }

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
        public IList<DownloadDependency> DownloadDependencies { get; }

        /// <summary>
        /// List of the package versions defined in the Central package versions management file. 
        /// </summary>
        public IDictionary<string, CentralPackageVersion> CentralPackageVersions { get; }

        /// <summary>
        /// A set of unique FrameworkReferences
        /// </summary>
        public ISet<FrameworkDependency> FrameworkReferences { get; }

        /// <summary>
        /// The project provided runtime.json
        /// </summary>
        public string RuntimeIdentifierGraphPath { get; set; }

        public TargetFrameworkInformation()
        {
            TargetAlias = string.Empty;
            Dependencies = new List<LibraryDependency>();
            Imports = new List<NuGetFramework>();
            DownloadDependencies = new List<DownloadDependency>();
            CentralPackageVersions = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase);
            FrameworkReferences = new HashSet<FrameworkDependency>();
        }

        internal TargetFrameworkInformation(TargetFrameworkInformation cloneFrom)
        {
            TargetAlias = cloneFrom.TargetAlias;
            FrameworkName = cloneFrom.FrameworkName;
            Dependencies = CloneList(cloneFrom.Dependencies, static dep => dep.Clone());
            Imports = new List<NuGetFramework>(cloneFrom.Imports);
            AssetTargetFallback = cloneFrom.AssetTargetFallback;
            Warn = cloneFrom.Warn;
            DownloadDependencies = cloneFrom.DownloadDependencies.ToList();
            CentralPackageVersions = new Dictionary<string, CentralPackageVersion>(cloneFrom.CentralPackageVersions, StringComparer.OrdinalIgnoreCase);
            FrameworkReferences = new HashSet<FrameworkDependency>(cloneFrom.FrameworkReferences);
            RuntimeIdentifierGraphPath = cloneFrom.RuntimeIdentifierGraphPath;

            static IList<T> CloneList<T>(IList<T> source, Func<T, T> cloneFunc)
            {
                var clone = new List<T>(capacity: source.Count);
                for (int i = 0; i < source.Count; i++)
                {
                    clone.Add(cloneFunc(source[i]));
                }
                return clone;
            }
        }

        public override string ToString()
        {
            return FrameworkName.GetShortFolderName();
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(FrameworkName);
            hashCode.AddObject(AssetTargetFallback);
            hashCode.AddUnorderedSequence(Dependencies);
            hashCode.AddSequence(Imports);
            hashCode.AddObject(Warn);
            hashCode.AddUnorderedSequence(DownloadDependencies);
            hashCode.AddUnorderedSequence(FrameworkReferences);
            if (RuntimeIdentifierGraphPath != null)
            {
                hashCode.AddObject(PathUtility.GetStringComparerBasedOnOS().GetHashCode(RuntimeIdentifierGraphPath));
            }
            hashCode.AddUnorderedSequence(CentralPackageVersions.Values);
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
            return new TargetFrameworkInformation(this);
        }
    }
}
