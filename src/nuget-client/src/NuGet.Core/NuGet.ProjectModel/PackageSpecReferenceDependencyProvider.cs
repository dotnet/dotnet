// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            var name = libraryRange.Name;

            PackageSpec packageSpec = null;

            // This must exist in the external references
            if (_externalProjectsByUniqueName.TryGetValue(name, out ExternalProjectReference externalReference))
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

            // Read references from external project - we don't care about dotnettool projects, since they don't have project refs
            if (projectStyle == ProjectStyle.PackageReference)
            {
                // NETCore
                dependencies = GetDependenciesFromSpecRestoreMetadata(packageSpec, targetFramework);
            }
            else
            {
                // UWP
                dependencies = GetDependenciesFromExternalReference(externalReference, packageSpec, targetFramework);
            }

            // Remove duplicate dependencies. A reference can exist both in csproj and project.json
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
        }

        private void AddLibraryProperties(Library library, PackageSpec packageSpec, NuGetFramework targetFramework)
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

        /// <summary>
        /// UWP Project.json
        /// </summary>
        private List<LibraryDependency> GetDependenciesFromExternalReference(
            ExternalProjectReference externalReference,
            PackageSpec packageSpec,
            NuGetFramework targetFramework)
        {
            var dependencies = GetSpecDependencies(packageSpec, targetFramework);

            if (externalReference != null)
            {
                var childReferences = GetChildReferences(externalReference);
                var childReferenceNames = childReferences.Select(reference => reference.ProjectName).ToList();

                // External references are created without pivoting on the TxM. Here we need to account for this
                // and filter out references except the nearest TxM.
                var filteredExternalDependencies = new HashSet<string>(
                    childReferenceNames,
                    StringComparer.OrdinalIgnoreCase);

                // Set all dependencies from project.json to external if an external match was passed in
                // This is viral and keeps p2ps from looking into directories when we are going down
                // a path already resolved by msbuild.
                for (int i = 0; i < dependencies.Count; i++)
                {
                    var d = dependencies[i];
                    if (IsProject(d) && filteredExternalDependencies.Contains(d.Name))
                    {
                        var libraryRange = new LibraryRange(d.LibraryRange) { TypeConstraint = LibraryDependencyTarget.ExternalProject };

                        // Do not push the dependency changes here upwards, as the original package
                        // spec should not be modified.
                        dependencies[i] = new LibraryDependency(d) { LibraryRange = libraryRange };
                    }
                }

                // Add dependencies passed in externally
                // These are usually msbuild references which have less metadata, they have
                // the lowest priority.
                // Note: Only add in dependencies that are in the filtered list to avoid getting the wrong TxM
                dependencies.AddRange(childReferences
                    .Where(reference => filteredExternalDependencies.Contains(reference.ProjectName))
                    .Select(reference => new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange
                        {
                            Name = reference.ProjectName,
                            VersionRange = VersionRange.Parse("1.0.0"),
                            TypeConstraint = LibraryDependencyTarget.ExternalProject
                        }
                    }));
            }

            return dependencies;
        }

        internal List<LibraryDependency> GetSpecDependencies(
            PackageSpec packageSpec,
            NuGetFramework targetFramework)
        {
            var dependencies = new List<LibraryDependency>();

            if (packageSpec != null)
            {
                // Add dependencies section
                dependencies.AddRange(packageSpec.Dependencies);

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
                    var libraryRange = new LibraryRange(dependency.LibraryRange) { TypeConstraint = dependency.LibraryRange.TypeConstraint & ~LibraryDependencyTarget.Project };
                    dependencies[i] = new LibraryDependency(dependency) { LibraryRange = libraryRange };
                }
            }

            return dependencies;
        }

        private bool IsProject(LibraryDependency dependency)
        {
            var type = dependency.LibraryRange.TypeConstraint;

            return SupportsType(type);
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
