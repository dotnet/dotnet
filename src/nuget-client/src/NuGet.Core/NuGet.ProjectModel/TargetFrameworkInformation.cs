// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class TargetFrameworkInformation : IEquatable<TargetFrameworkInformation>
    {
        // private fields to allow for validating initialization values
        private IReadOnlyDictionary<string, CentralPackageVersion> _centralPackageVersions;
        private ImmutableArray<LibraryDependency> _dependencies;
        private ImmutableArray<DownloadDependency> _downloadDependencies;
        private IReadOnlyCollection<FrameworkDependency> _frameworkReferences;
        private ImmutableArray<NuGetFramework> _imports;
        private IReadOnlyDictionary<string, PrunePackageReference> _packagesToPrune;

        public string TargetAlias { get; init; }

        public NuGetFramework FrameworkName { get; init; }

        public ImmutableArray<LibraryDependency> Dependencies
        {
            get => _dependencies;
            init
            {
                _dependencies = value.IsDefault ? ImmutableArray<LibraryDependency>.Empty : value;
            }
        }

        /// <summary>
        /// A fallback PCL framework to use when no compatible items
        /// were found for <see cref="FrameworkName"/>.
        /// </summary>
        public ImmutableArray<NuGetFramework> Imports
        {
            get => _imports;
            init
            {
                _imports = value.IsDefault ? ImmutableArray<NuGetFramework>.Empty : value;
            }
        }

        /// <summary>
        /// If True AssetTargetFallback behavior will be used for Imports.
        /// </summary>
        public bool AssetTargetFallback { get; init; }

        /// <summary>
        /// Display warnings when the Imports framework is used.
        /// </summary>
        public bool Warn { get; init; }

        /// <summary>
        /// List of dependencies that are not part of the graph resolution.
        /// </summary>
        public ImmutableArray<DownloadDependency> DownloadDependencies
        {
            get => _downloadDependencies;
            init
            {
                _downloadDependencies = value.IsDefault ? ImmutableArray<DownloadDependency>.Empty : value;
            }
        }

        /// <summary>
        /// Package versions defined in the Central package versions management file. 
        /// </summary>
        public IReadOnlyDictionary<string, CentralPackageVersion> CentralPackageVersions
        {
            get => _centralPackageVersions;
            init
            {
                _centralPackageVersions = value ?? ImmutableDictionary<string, CentralPackageVersion>.Empty;
            }
        }

        /// <summary>
        /// A set of unique FrameworkReferences
        /// </summary>
        public IReadOnlyCollection<FrameworkDependency> FrameworkReferences
        {
            get => _frameworkReferences;
            init
            {
                _frameworkReferences = value ?? ImmutableHashSet<FrameworkDependency>.Empty;
            }
        }

        /// <summary>
        /// A dictionary of packages to be pruned from the graph.
        /// An item existing in this dictionary means the pruning capability is enabled.
        /// If pruning is not enabled, this property return Empty regardless of what was specified in the items.
        /// </summary>
        public IReadOnlyDictionary<string, PrunePackageReference> PackagesToPrune
        {
            get => _packagesToPrune;
            init
            {
                _packagesToPrune = value ?? ImmutableDictionary<string, PrunePackageReference>.Empty;
            }
        }

        /// <summary>
        /// The project provided runtime.json
        /// </summary>
        public string RuntimeIdentifierGraphPath { get; init; }

        public TargetFrameworkInformation()
        {
            TargetAlias = string.Empty;
            Dependencies = [];
            Imports = [];
            DownloadDependencies = [];
            CentralPackageVersions = ImmutableDictionary<string, CentralPackageVersion>.Empty;
            FrameworkReferences = ImmutableHashSet<FrameworkDependency>.Empty;
            PackagesToPrune = ImmutableDictionary<string, PrunePackageReference>.Empty;
        }

        public TargetFrameworkInformation(TargetFrameworkInformation cloneFrom)
        {
            TargetAlias = cloneFrom.TargetAlias;
            FrameworkName = cloneFrom.FrameworkName;
            Dependencies = cloneFrom.Dependencies;
            Imports = cloneFrom.Imports;
            AssetTargetFallback = cloneFrom.AssetTargetFallback;
            Warn = cloneFrom.Warn;
            DownloadDependencies = cloneFrom.DownloadDependencies;
            CentralPackageVersions = cloneFrom.CentralPackageVersions;
            FrameworkReferences = cloneFrom.FrameworkReferences;
            RuntimeIdentifierGraphPath = cloneFrom.RuntimeIdentifierGraphPath;
            PackagesToPrune = cloneFrom.PackagesToPrune;
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
            hashCode.AddSequence((IReadOnlyList<NuGetFramework>)Imports);
            hashCode.AddObject(Warn);
            hashCode.AddUnorderedSequence(DownloadDependencies);
            hashCode.AddUnorderedSequence(FrameworkReferences);
            if (RuntimeIdentifierGraphPath != null)
            {
                hashCode.AddObject(PathUtility.GetStringComparerBasedOnOS().GetHashCode(RuntimeIdentifierGraphPath));
            }
            hashCode.AddUnorderedSequence(CentralPackageVersions.Values);
            hashCode.AddUnorderedSequence(PackagesToPrune.Values);
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
                   EqualityUtility.OrderedEquals(PackagesToPrune.Values, other.PackagesToPrune.Values, e => e.Name, StringComparer.OrdinalIgnoreCase) &&
                   PathUtility.GetStringComparerBasedOnOS().Equals(RuntimeIdentifierGraphPath, other.RuntimeIdentifierGraphPath) &&
                   StringComparer.OrdinalIgnoreCase.Equals(TargetAlias, other.TargetAlias);
        }
    }
}
