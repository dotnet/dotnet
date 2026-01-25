// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Handles both external references and projects discovered through directories
    /// If the type is set to external project directory discovery will be disabled.
    /// </summary>
    public class PackageSpecReferenceDependencyProvider : IDependencyProvider
    {
        private readonly Dictionary<string, ExternalProjectReference> _externalProjectsByPath
            = new Dictionary<string, ExternalProjectReference>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, ExternalProjectReference> _externalProjectsByUniqueName
            = new Dictionary<string, ExternalProjectReference>(StringComparer.OrdinalIgnoreCase);

        private readonly bool _useLegacyAssetTargetFallbackBehavior;

        private readonly bool _useLegacyDependencyGraphResolution = false;

        public PackageSpecReferenceDependencyProvider(
            IEnumerable<ExternalProjectReference> externalProjects,
            ILogger logger) :
            this(externalProjects,
                environmentVariableReader: EnvironmentVariableWrapper.Instance,
                useLegacyDependencyGraphResolution: false)
        {
        }

        public PackageSpecReferenceDependencyProvider(
            IEnumerable<ExternalProjectReference> externalProjects,
            ILogger logger,
            bool useLegacyDependencyGraphResolution) :
            this(externalProjects,
                environmentVariableReader: EnvironmentVariableWrapper.Instance,
                useLegacyDependencyGraphResolution)
        {
        }

        internal PackageSpecReferenceDependencyProvider(
            IEnumerable<ExternalProjectReference> externalProjects,
            IEnvironmentVariableReader environmentVariableReader,
            bool useLegacyDependencyGraphResolution = false)
        {
            if (externalProjects == null)
            {
                throw new ArgumentNullException(nameof(externalProjects));
            }

            foreach (var project in externalProjects)
            {
                Debug.Assert(
                    !_externalProjectsByPath.ContainsKey(project.UniqueName),
                    $"Duplicate project {project.UniqueName}");

                if (!_externalProjectsByPath.ContainsKey(project.UniqueName))
                {
                    _externalProjectsByPath.Add(project.UniqueName, project);
                }

                Debug.Assert(
                    !_externalProjectsByUniqueName.ContainsKey(project.ProjectName),
                    $"Duplicate project {project.ProjectName}");

                if (!_externalProjectsByUniqueName.ContainsKey(project.ProjectName))
                {
                    _externalProjectsByUniqueName.Add(project.ProjectName, project);
                }

                if (!_externalProjectsByUniqueName.ContainsKey(project.UniqueName))
                {
                    _externalProjectsByUniqueName.Add(project.UniqueName, project);
                }
            }
            _useLegacyAssetTargetFallbackBehavior = MSBuildStringUtility.IsTrue(environmentVariableReader.GetEnvironmentVariable("NUGET_USE_LEGACY_ASSET_TARGET_FALLBACK_DEPENDENCY_RESOLUTION"));
            _useLegacyDependencyGraphResolution = useLegacyDependencyGraphResolution;
        }

        public bool SupportsType(LibraryDependencyTarget libraryType)
        {
            return (libraryType & (LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject)) != LibraryDependencyTarget.None;
        }

        public Library GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework)
        {
            if (libraryRange == null) throw new ArgumentNullException(nameof(libraryRange));
            if (targetFramework == null) throw new ArgumentNullException(nameof(targetFramework));

            PackageSpec packageSpec = null;

            // This must exist in the external references
            if (_externalProjectsByUniqueName.TryGetValue(libraryRange.Name, out ExternalProjectReference externalReference))
            {
                packageSpec = externalReference.PackageSpec;
            }

            if (externalReference == null && packageSpec == null)
            {
                // unable to find any projects
                return null;
            }

            List<LibraryDependency> dependencies;

            var projectStyle = packageSpec?.RestoreMetadata?.ProjectStyle ?? ProjectStyle.Unknown;

            if (projectStyle == ProjectStyle.PackageReference ||
                projectStyle == ProjectStyle.DotnetCliTool)
            {
                dependencies = GetDependenciesFromSpecRestoreMetadata(packageSpec, targetFramework);
            }
            else
            {
                dependencies = GetDependenciesFromExternalReference(externalReference);
            }

            List<LibraryDependency> uniqueDependencies = DeduplicateDependencies(dependencies);

            Library library = new Library
            {
                LibraryRange = libraryRange,
                Identity = new LibraryIdentity
                {
                    Name = externalReference?.ProjectName ?? packageSpec.Name,
                    Version = packageSpec?.Version ?? new NuGetVersion(1, 0, 0),
                    Type = LibraryType.Project,
                },
                Path = packageSpec?.FilePath,
                Dependencies = uniqueDependencies,
                Resolved = true
            };

            // Add msbuild path
            var msbuildPath = externalReference?.MSBuildProjectPath;
            if (msbuildPath != null)
            {
                library[KnownLibraryProperties.MSBuildProjectPath] = msbuildPath;
            }

            if (packageSpec != null)
            {
                // Additional library properties
                AddLibraryProperties(library, packageSpec, targetFramework);
            }

            return library;

            static List<LibraryDependency> DeduplicateDependencies(List<LibraryDependency> dependencies)
            {
                if (dependencies.Count == 0)
                {
                    return dependencies;
                }
                // Remove duplicate dependencies.
                // dependencies is already ordered by importance here
                var uniqueDependencies = new List<LibraryDependency>(dependencies.Count);
                var projectNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var project in dependencies)
                {
                    if (projectNames.Add(project.Name))
                    {
                        uniqueDependencies.Add(project);
                    }
                }

                return uniqueDependencies;
            }
        }

        private static void AddLibraryProperties(Library library, PackageSpec packageSpec, NuGetFramework targetFramework)
        {
            var projectStyle = packageSpec.RestoreMetadata?.ProjectStyle ?? ProjectStyle.Unknown;

            library[KnownLibraryProperties.PackageSpec] = packageSpec;
            library[KnownLibraryProperties.ProjectStyle] = projectStyle;

            if (packageSpec.RestoreMetadata?.Files != null)
            {
                // Record all files that would be in a nupkg
                library[KnownLibraryProperties.ProjectRestoreMetadataFiles]
                    = packageSpec.RestoreMetadata.Files.ToList();
            }

            // Avoid adding these properties for class libraries
            // and other projects which are not fully able to
            // participate in restore.
            if (packageSpec.RestoreMetadata == null
                || (projectStyle != ProjectStyle.Unknown
                    && projectStyle != ProjectStyle.PackagesConfig))
            {
                var frameworks = new List<NuGetFramework>(
                    packageSpec.TargetFrameworks.Select(fw => fw.FrameworkName)
                    .Where(fw => !fw.IsUnsupported));

                // Record all frameworks in the project
                library[KnownLibraryProperties.ProjectFrameworks] = frameworks;

                var targetFrameworkInfo = packageSpec.GetTargetFramework(targetFramework);

                // FrameworkReducer.GetNearest does not consider ATF since it is used for more than just compat
                if (targetFrameworkInfo.FrameworkName == null && targetFramework is AssetTargetFallbackFramework atfFramework)
                {
                    targetFrameworkInfo = packageSpec.GetTargetFramework(atfFramework.AsFallbackFramework());
                }

                if (targetFrameworkInfo.FrameworkName == null && targetFramework is DualCompatibilityFramework mcfFramework)
                {
                    targetFrameworkInfo = packageSpec.GetTargetFramework(mcfFramework.AsFallbackFramework());
                }

                library[KnownLibraryProperties.TargetFrameworkInformation] = targetFrameworkInfo;

                // Add framework assemblies
                var frameworkAssemblies = targetFrameworkInfo.Dependencies
                    .Where(d => d.LibraryRange.TypeConstraint == LibraryDependencyTarget.Reference)
                    .Select(d => d.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                library[KnownLibraryProperties.FrameworkAssemblies] = frameworkAssemblies;

                // Add framework references
                library[KnownLibraryProperties.FrameworkReferences] = targetFrameworkInfo.FrameworkReferences;
            }
        }

        private List<LibraryDependency> GetDependenciesFromSpecRestoreMetadata(PackageSpec packageSpec, NuGetFramework targetFramework)
        {
            var dependencies = GetSpecDependencies(packageSpec, targetFramework);

            // Get the nearest framework
            var referencesForFramework = packageSpec.GetRestoreMetadataFramework(targetFramework);

            if (!_useLegacyAssetTargetFallbackBehavior)
            {
                if (referencesForFramework.FrameworkName == null &&
                      targetFramework is AssetTargetFallbackFramework assetTargetFallbackFramework)
                {
                    referencesForFramework = packageSpec.GetRestoreMetadataFramework(assetTargetFallbackFramework.AsFallbackFramework());
                }
            }

            // Ensure that this project is compatible
            if (referencesForFramework?.FrameworkName?.IsSpecificFramework == true)
            {
                foreach (var reference in referencesForFramework.ProjectReferences)
                {
                    // Defaults, these will be used for missing projects
                    var dependencyName = reference.ProjectPath;
                    var range = VersionRange.All;

                    ExternalProjectReference externalProject;
                    if (_externalProjectsByUniqueName.TryGetValue(reference.ProjectUniqueName, out externalProject))
                    {
                        dependencyName = externalProject.ProjectName;

                        // Create a version range based on the project if it has a range
                        var version = externalProject.PackageSpec?.Version;
                        if (version != null)
                        {
                            range = new VersionRange(version);
                        }
                    }

                    var dependency = new LibraryDependency()
                    {
                        IncludeType = (reference.IncludeAssets & ~reference.ExcludeAssets),
                        SuppressParent = reference.PrivateAssets,
                        LibraryRange = new LibraryRange(
                            dependencyName,
                            range,
                            LibraryDependencyTarget.ExternalProject),
                    };

                    // Remove existing reference if one exists, projects override
                    dependencies.RemoveAll(e => StringComparer.OrdinalIgnoreCase.Equals(dependency.Name, e.Name));

                    // Add reference
                    dependencies.Add(dependency);
                }
            }

            return dependencies;
        }

        private List<LibraryDependency> GetDependenciesFromExternalReference(ExternalProjectReference externalReference)
        {
            if (externalReference != null)
            {
                var childReferences = GetChildReferences(externalReference);
                var childReferenceNames = childReferences.Select(reference => reference.ProjectName).ToList();

                // External references are created without pivoting on the TxM. Here we need to account for this
                // and filter out references except the nearest TxM.
                var filteredExternalDependencies = new HashSet<string>(
                    childReferenceNames,
                    StringComparer.OrdinalIgnoreCase);

                return [.. childReferences
                    .Where(reference => filteredExternalDependencies.Contains(reference.ProjectName))
                    .Select(reference => new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange
                        {
                            Name = reference.ProjectName,
                            VersionRange = VersionRange.Parse("1.0.0"),
                            TypeConstraint = LibraryDependencyTarget.ExternalProject
                        }
                    })];
            }

            return [];
        }

        internal List<LibraryDependency> GetSpecDependencies(
            PackageSpec packageSpec,
            NuGetFramework targetFramework)
        {
            var dependencies = new List<LibraryDependency>();

            if (packageSpec != null)
            {
                // Add framework specific dependencies
                var targetFrameworkInfo = packageSpec.GetTargetFramework(targetFramework);

                if (!_useLegacyAssetTargetFallbackBehavior)
                {
                    if (targetFrameworkInfo.FrameworkName == null && targetFramework is AssetTargetFallbackFramework atfFramework)
                    {
                        targetFrameworkInfo = packageSpec.GetTargetFramework(atfFramework.AsFallbackFramework());
                    }
                }

                dependencies.AddRange(targetFrameworkInfo.Dependencies);

                if (_useLegacyDependencyGraphResolution && packageSpec.RestoreMetadata?.CentralPackageVersionsEnabled == true &&
                    packageSpec.RestoreMetadata?.CentralPackageTransitivePinningEnabled == true)
                {
                    var dependencyNamesSet = new HashSet<string>(targetFrameworkInfo.Dependencies.Select(d => d.Name), StringComparer.OrdinalIgnoreCase);
                    dependencies.AddRange(targetFrameworkInfo.CentralPackageVersions
                        .Where(item => !dependencyNamesSet.Contains(item.Key))
                        .Select(item => new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange(item.Value.Name, item.Value.VersionRange, LibraryDependencyTarget.Package),
                            VersionCentrallyManaged = true,
                            ReferenceType = LibraryDependencyReferenceType.None,
                        }));
                }

                // Remove all framework assemblies
                dependencies.RemoveAll(d => d.LibraryRange.TypeConstraint == LibraryDependencyTarget.Reference);

                for (var i = 0; i < dependencies.Count; i++)
                {
                    // Do not push the dependency changes here upwards, as the original package
                    // spec should not be modified.

                    // Remove "project" from the allowed types for this dependency
                    // This will require that projects referenced by an msbuild project
                    // must be external projects.
                    var dependency = dependencies[i];
                    bool isPruned = IsDependencyPruned(dependency, targetFrameworkInfo.PackagesToPrune);
                    var libraryRange = new LibraryRange(dependency.LibraryRange) { TypeConstraint = dependency.LibraryRange.TypeConstraint & ~LibraryDependencyTarget.Project };
                    dependencies[i] = new LibraryDependency(dependency)
                    {
                        LibraryRange = libraryRange,
                        SuppressParent = isPruned ? LibraryIncludeFlags.All : dependency.SuppressParent,
                        IncludeType = isPruned ? LibraryIncludeFlags.None : dependency.IncludeType,
                    };
                }
            }

            return dependencies;

            static bool IsDependencyPruned(LibraryDependency dependency, IReadOnlyDictionary<string, PrunePackageReference> packagesToPrune)
            {
                if (packagesToPrune?.TryGetValue(dependency.Name, out PrunePackageReference packageToPrune) == true
                    && dependency.LibraryRange.VersionRange.Satisfies(packageToPrune.VersionRange.MaxVersion))
                {
                    return true;
                }
                return false;
            }
        }

        private List<ExternalProjectReference> GetChildReferences(ExternalProjectReference parent)
        {
            var children = new List<ExternalProjectReference>(parent.ExternalProjectReferences.Count);

            foreach (var reference in parent.ExternalProjectReferences)
            {
                ExternalProjectReference childReference;
                if (!_externalProjectsByUniqueName.TryGetValue(reference, out childReference))
                {
                    // Create a reference to mark that this project is unresolved here
                    childReference = new ExternalProjectReference(
                        uniqueName: reference,
                        packageSpec: null,
                        msbuildProjectPath: null,
                        projectReferences: Enumerable.Empty<string>());
                }

                children.Add(childReference);
            }

            return children;
        }
    }
}
