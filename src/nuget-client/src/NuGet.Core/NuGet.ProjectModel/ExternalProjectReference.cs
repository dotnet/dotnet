// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Represents a reference to a project produced by an external build system, such as msbuild.
    /// </summary>
    public class ExternalProjectReference : IEquatable<ExternalProjectReference>, IComparable<ExternalProjectReference>
    {
        private PackageSpec _packageSpec;

        /// <summary>
        /// Represents a reference to a project produced by an external build system, such as msbuild.
        /// </summary>
        /// <param name="uniqueName">unique project name or full path</param>
        /// <param name="packageSpec">project.json package spec.</param>
        /// <param name="msbuildProjectPath">project file if one exists</param>
        /// <param name="projectReferences">unique names of the referenced projects</param>
        public ExternalProjectReference(
            string uniqueName,
            PackageSpec packageSpec,
            string msbuildProjectPath,
            IEnumerable<string> projectReferences)
            : this(uniqueName, packageSpec?.Name, packageSpec?.FilePath, msbuildProjectPath, projectReferences)
        {
            _packageSpec = packageSpec;
        }

        /// <summary>
        /// Represents a reference to a project produced by an external build system, such as msbuild.
        /// </summary>
        /// <param name="uniqueName">unique project name or full path</param>
        /// <param name="packageSpecPath">project.json file path or null if none exists</param>
        /// <param name="msbuildProjectPath">project file if one exists</param>
        /// <param name="projectReferences">unique names of the referenced projects</param>
        public ExternalProjectReference(
            string uniqueName,
            string packageSpecProjectName,
            string packageSpecPath,
            string msbuildProjectPath,
            IEnumerable<string> projectReferences)
        {
            if (uniqueName == null)
            {
                throw new ArgumentNullException(nameof(uniqueName));
            }

            if (projectReferences == null)
            {
                throw new ArgumentNullException(nameof(projectReferences));
            }

            UniqueName = uniqueName;
            ProjectJsonPath = packageSpecPath;
            MSBuildProjectPath = msbuildProjectPath;
            PackageSpecProjectName = packageSpecProjectName;
            ExternalProjectReferences = projectReferences.ToList();
        }

        /// <summary>
        /// Unique name of the external project
        /// </summary>
        public string UniqueName { get; }

        /// <summary>
        /// The path to the project.json file representing the NuGet dependencies of the project
        /// </summary>
        public PackageSpec PackageSpec
        {
            get
            {
                if (_packageSpec == null && ProjectJsonPath != null && PackageSpecProjectName != null)
                {
                    _packageSpec = JsonPackageSpecReader.GetPackageSpec(PackageSpecProjectName, ProjectJsonPath);
                }

                return _packageSpec;
            }
        }

        /// <summary>
        /// A list of other external projects this project references. Uses the UniqueName.
        /// </summary>
        public IReadOnlyList<string> ExternalProjectReferences { get; }

        /// <summary>
        /// Path to msbuild project file. Ex: xproj, csproj
        /// </summary>
        public string MSBuildProjectPath { get; }

        /// <summary>
        /// Path to project.json
        /// </summary>
        /// <remarks>This may be null for projects that do not contain project.json.</remarks>
        public string ProjectJsonPath { get; }

        /// <summary>
        /// Project name used for project.json
        /// </summary>
        /// <remarks>This may be null for projects that do not contain project.json.</remarks>
        public string PackageSpecProjectName { get; }

        /// <summary>
        /// Project name from the package spec or msbuild file.
        /// </summary>
        public string ProjectName
        {
            get
            {
                // project.json name goes first
                // use the msbuild file path for non-project.json projects
                // fallback to the given unique name
                return PackageSpecProjectName
                        ?? Path.GetFileNameWithoutExtension(MSBuildProjectPath)
                        ?? UniqueName;
            }
        }

        public override string ToString()
        {
            return UniqueName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExternalProjectReference);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(UniqueName);
        }

        public bool Equals(ExternalProjectReference other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return UniqueName.Equals(other.UniqueName, StringComparison.Ordinal);
        }

        public int CompareTo(ExternalProjectReference other)
        {
            return StringComparer.Ordinal.Compare(UniqueName, other?.UniqueName);
        }
    }
}
