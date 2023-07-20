// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Shared;

namespace NuGet.Commands
{
    public static class SpecValidationUtility
    {
        /// <summary>
        /// Validate a dg file. This will throw a RestoreSpecException if there are errors.
        /// </summary>
        public static void ValidateDependencySpec(DependencyGraphSpec spec)
        {
            ValidateDependencySpec(spec, projectsToSkip: new HashSet<string>());
        }

        /// <summary>
        /// Validate a dg file. This will throw a RestoreSpecException if there are errors.
        /// </summary>
        public static void ValidateDependencySpec(DependencyGraphSpec spec, HashSet<string> projectsToSkip)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            try
            {
                // Verify projects
                foreach (var projectSpec in spec.Projects)
                {
                    if (!projectsToSkip.Contains(projectSpec.FilePath))
                    {
                        ValidateProjectSpec(projectSpec);
                    }
                }

                var restoreSet = new HashSet<string>(spec.Restore, StringComparer.Ordinal);
                var projectSet = new HashSet<string>(
                    spec.Projects.Select(p => p.RestoreMetadata?.ProjectUniqueName)
                    .Where(s => !string.IsNullOrEmpty(s)),
                    StringComparer.Ordinal);

                // Verify restore does not reference a project that does not exist
                foreach (var missing in restoreSet.Except(projectSet))
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.SpecValidationMissingProject,
                        missing);

                    throw RestoreSpecException.Create(message, Enumerable.Empty<string>());
                }

                // Verify restore contains at least one project
                if (restoreSet.Count < 1)
                {
                    throw RestoreSpecException.Create(Strings.SpecValidationZeroRestoreRequests, Enumerable.Empty<string>());
                }
            }
            catch (Exception ex) when (!(ex is RestoreSpecException))
            {
                // Catch and wrap any unexpected exceptions
                throw RestoreSpecException.Create(
                    ex.Message,
                    Enumerable.Empty<string>(),
                    ex);
            }
        }

        public static void ValidateProjectSpec(PackageSpec spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            // Track the spec path
            var files = new Stack<string>();
            files.Push(spec.FilePath);

            // restore metadata must exist for all project types
            var restoreMetadata = spec.RestoreMetadata;

            if (restoreMetadata == null)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Strings.MissingRequiredProperty, nameof(spec.RestoreMetadata));

                throw RestoreSpecException.Create(message, files);
            }

            var projectStyle = spec.RestoreMetadata?.ProjectStyle;

            // Verify required fields for all specs
            ValidateProjectMetadata(spec, files);

            if (projectStyle == ProjectStyle.Standalone)
            {
                ValidateStandaloneSpec(spec, files);
            }
            else if (projectStyle == ProjectStyle.DotnetCliTool)
            {
                // Verify tool properties
                ValidateToolSpec(spec, files);
            }
            else
            {
                // Track the project path
                files.Push(restoreMetadata.ProjectPath);

                // Verify project metadata
                ValidateProjectMSBuildMetadata(spec, files);

                // Verify based on the type.
                switch (projectStyle)
                {
                    case ProjectStyle.PackageReference:
                        ValidateProjectSpecPackageReference(spec, files);
                        break;

                    case ProjectStyle.DotnetToolReference:
                        ValidateProjectSpecPackageReference(spec, files);
                        break;

                    case ProjectStyle.ProjectJson:
                        ValidateProjectSpecUAP(spec, files);
                        break;

                    default:
                        ValidateProjectSpecOther(spec, files);
                        break;
                }
            }
        }

        private static void ValidateFrameworks(PackageSpec spec, IEnumerable<string> files)
        {
            var frameworks = spec.TargetFrameworks.Select(f => f.FrameworkName).ToArray();

            // Verify frameworks are valid
            foreach (var framework in frameworks.Where(f => !f.IsSpecificFramework))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.SpecValidationInvalidFramework,
                    framework.GetShortFolderName());

                throw RestoreSpecException.Create(message, files);
            }

            // Must have at least 1 framework
            if (frameworks.Length < 1)
            {
                throw RestoreSpecException.Create(Strings.SpecValidationNoFrameworks, files);
            }

            // Duplicate frameworks may not exist
            // Change in ATF should *not* affect our duplicate check, so we use the full framework comparer.
            if (frameworks.Length != frameworks.Distinct(NuGetFrameworkFullComparer.Instance).Count())
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.SpecValidationDuplicateFrameworks,
                    string.Join(", ", frameworks.Select(f => f.GetShortFolderName())));

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static void ValidateProjectSpecPackageReference(PackageSpec spec, IEnumerable<string> files)
        {
            // Verify frameworks
            ValidateFrameworks(spec, files);

            // NETCore may not specify a project.json file
            if (!string.IsNullOrEmpty(spec.RestoreMetadata.ProjectJsonPath))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PropertyNotAllowedForProjectType,
                    nameof(spec.RestoreMetadata.ProjectJsonPath),
                    ProjectStyle.PackageReference.ToString());

                throw RestoreSpecException.Create(message, files);
            }

            // Output path must be set for netcore
            if (string.IsNullOrEmpty(spec.RestoreMetadata.OutputPath))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredPropertyForProjectType,
                    nameof(spec.RestoreMetadata.OutputPath),
                    ProjectStyle.PackageReference.ToString());

                throw RestoreSpecException.Create(message, files);
            }

            // Original frameworks must be set for netcore
            if (spec.RestoreMetadata.OriginalTargetFrameworks.Count < 1)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredPropertyForProjectType,
                    nameof(spec.RestoreMetadata.OriginalTargetFrameworks),
                    ProjectStyle.PackageReference.ToString());

                throw RestoreSpecException.Create(message, files);
            }

            //OriginalTargetFrameworks must match the aliases.
            if (spec.RestoreMetadata.TargetFrameworks.Count > 1)
            {
                var aliases = spec.TargetFrameworks.Select(e => e.TargetAlias);

                if (!EqualityUtility.OrderedEquals(aliases, spec.RestoreMetadata.OriginalTargetFrameworks, e => e, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.SpecValidation_OriginalTargetFrameworksMustMatchAliases,
                        string.Join(";", spec.RestoreMetadata.OriginalTargetFrameworks),
                        string.Join(";", aliases)
                        );
                    throw RestoreSpecException.Create(message, files);
                }
            }
        }

        private static void ValidateProjectSpecUAP(PackageSpec spec, IEnumerable<string> files)
        {
            // Verify frameworks
            ValidateFrameworks(spec, files);

            // UAP may contain only 1 framework
            if (spec.TargetFrameworks.Count != 1)
            {
                throw RestoreSpecException.Create(Strings.SpecValidationUAPSingleFramework, files);
            }

            // UAP must specify a project.json file
            if (string.IsNullOrEmpty(spec.RestoreMetadata.ProjectJsonPath)
                || spec.RestoreMetadata.ProjectJsonPath != spec.FilePath)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredPropertyForProjectType,
                    nameof(spec.RestoreMetadata.ProjectJsonPath),
                    ProjectStyle.ProjectJson.ToString());

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static void ValidateStandaloneSpec(PackageSpec spec, IEnumerable<string> files)
        {
            // Output path must exist
            if (string.IsNullOrEmpty(spec.RestoreMetadata.OutputPath))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredPropertyForProjectType,
                    nameof(spec.RestoreMetadata.OutputPath),
                    ProjectStyle.Standalone.ToString());

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static void ValidateToolSpec(PackageSpec spec, IEnumerable<string> files)
        {
            var packageDependencies = GetAllDependencies(spec).ToList();

            if (packageDependencies.Count != 1
                || packageDependencies.All(e => e.LibraryRange.TypeConstraint != LibraryDependencyTarget.Package)
                || spec.TargetFrameworks.Count != 1)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.InvalidRestoreInput,
                    spec.Name);

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static void ValidateProjectSpecOther(PackageSpec spec, IEnumerable<string> files)
        {
            // Unknown project types may not have a project.json path
            if (spec.RestoreMetadata.ProjectJsonPath != null)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PropertyNotAllowed,
                    nameof(spec.RestoreMetadata.ProjectJsonPath));

                throw RestoreSpecException.Create(message, files);
            }

            // Unknown project types may not carry package dependencies
            var packageDependencies = GetAllDependencies(spec)
                .Where(d => d.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package));

            if (packageDependencies.Any())
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PropertyNotAllowed,
                    nameof(spec.Dependencies));

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static void ValidateProjectMetadata(PackageSpec spec, IEnumerable<string> files)
        {
            // spec file path must be set
            if (string.IsNullOrEmpty(spec.FilePath))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredProperty,
                    nameof(spec.FilePath));

                throw RestoreSpecException.Create(message, files);
            }

            // spec name must be set
            if (string.IsNullOrEmpty(spec.Name))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredProperty,
                    nameof(spec.Name));

                throw RestoreSpecException.Create(message, files);
            }

            // unique name must be set
            if (string.IsNullOrEmpty(spec.RestoreMetadata.ProjectUniqueName))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredProperty,
                    nameof(spec.RestoreMetadata.ProjectUniqueName));

                throw RestoreSpecException.Create(message, files);
            }

            // project name must be set
            if (string.IsNullOrEmpty(spec.RestoreMetadata.ProjectName))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredProperty,
                    nameof(spec.RestoreMetadata.ProjectName));

                throw RestoreSpecException.Create(message, files);
            }

            // spec.name and spec.RestoreMetadata.ProjectName should be the same
            if (!string.Equals(spec.Name, spec.RestoreMetadata.ProjectName, StringComparison.Ordinal))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.NonMatchingProperties,
                    nameof(spec.Name),
                    spec.Name,
                    nameof(spec.RestoreMetadata.ProjectName),
                    spec.RestoreMetadata.ProjectName);

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static void ValidateProjectMSBuildMetadata(PackageSpec spec, IEnumerable<string> files)
        {
            // msbuild project path must be set
            if (string.IsNullOrEmpty(spec.RestoreMetadata.ProjectPath))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingRequiredProperty,
                    nameof(spec.RestoreMetadata.ProjectPath));

                throw RestoreSpecException.Create(message, files);
            }

            // block xproj
            if (spec.RestoreMetadata.ProjectPath.EndsWith(".xproj", StringComparison.OrdinalIgnoreCase))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Error_XPROJNotAllowed,
                    nameof(spec.RestoreMetadata.ProjectPath));

                throw RestoreSpecException.Create(message, files);
            }
        }

        private static IEnumerable<LibraryDependency> GetAllDependencies(PackageSpec spec)
        {
            return spec.Dependencies
                .Concat(spec.TargetFrameworks.SelectMany(f => f.Dependencies));
        }
    }
}
