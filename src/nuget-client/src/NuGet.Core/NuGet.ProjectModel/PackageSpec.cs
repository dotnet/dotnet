// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NuGet.LibraryModel;
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
        public static readonly string PackageSpecFileName = "project.json";
        public static readonly NuGetVersion DefaultVersion = new NuGetVersion(1, 0, 0);

        public PackageSpec(IList<TargetFrameworkInformation> frameworks)
            : this(frameworks, dependencies: null, runtimeGraph: null, restoreSettings: null)
        {
        }

        public PackageSpec() : this(new List<TargetFrameworkInformation>())
        {
        }

        internal PackageSpec(
            IList<TargetFrameworkInformation> frameworks,
            IList<LibraryDependency> dependencies,
            RuntimeGraph runtimeGraph,
            ProjectRestoreSettings restoreSettings,
            string[] authors = null,
            string[] owners = null,
            string[] tags = null,
            IList<string> contentFiles = null,
            IDictionary<string, IEnumerable<string>> scripts = null,
            IDictionary<string, string> packInclude = null,
            PackOptions packOptions = null
            )
        {
            TargetFrameworks = frameworks;
            Dependencies = dependencies ?? new List<LibraryDependency>();
            RuntimeGraph = runtimeGraph ?? RuntimeGraph.Empty;
            RestoreSettings = restoreSettings ?? new ProjectRestoreSettings();
#pragma warning disable CS0612 // Type or member is obsolete
            Authors = authors ?? Array.Empty<string>();
            Owners = owners ?? Array.Empty<string>();
            Tags = tags ?? Array.Empty<string>();
            ContentFiles = contentFiles ?? new List<string>();
            Scripts = scripts ?? new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);
            PackInclude = packInclude ?? new Dictionary<string, string>();
            PackOptions = packOptions ?? new PackOptions();
#pragma warning restore CS0612 // Type or member is obsolete
        }

        public string FilePath { get; set; }

        public string BaseDirectory => Path.GetDirectoryName(FilePath);

        public string Name { get; set; }

        public string Title { get; set; }

        private NuGetVersion _version = DefaultVersion;
        public NuGetVersion Version
        {
            get => _version;
            set
            {
                _version = value;
#pragma warning disable CS0612 // Type or member is obsolete
                IsDefaultVersion = false;
#pragma warning restore CS0612 // Type or member is obsolete
            }
        }

        [Obsolete]
        public bool IsDefaultVersion { get; set; } = true;

        [Obsolete]
        public bool HasVersionSnapshot { get; set; }

        [Obsolete]
        public string Description { get; set; }

        [Obsolete]
        public string Summary { get; set; }

        [Obsolete]
        public string ReleaseNotes { get; set; }

        [Obsolete]
        public string[] Authors { get; set; }

        [Obsolete]
        public string[] Owners { get; set; }

        [Obsolete]
        public string ProjectUrl { get; set; }

        [Obsolete]
        public string IconUrl { get; set; }

        [Obsolete]
        public string LicenseUrl { get; set; }

        [Obsolete]
        public bool RequireLicenseAcceptance { get; set; }

        [Obsolete]
        public string Copyright { get; set; }

        [Obsolete]
        public string Language { get; set; }

        [Obsolete]
        public BuildOptions BuildOptions { get; set; }

        [Obsolete]
        public string[] Tags { get; set; }

        [Obsolete]
        public IList<string> ContentFiles { get; set; }

        [Obsolete]
        public IDictionary<string, IEnumerable<string>> Scripts { get; private set; }

        [Obsolete]
        public IDictionary<string, string> PackInclude { get; private set; }

        [Obsolete]
        public PackOptions PackOptions { get; set; }

        /// <summary>
        /// List of dependencies that apply to all frameworks.
        /// <see cref="ProjectStyle.PackageReference"/> based projects must not use this list and instead use the one in the <see cref="TargetFrameworks"/> property which is a list of the <see cref="TargetFrameworkInformation"/> type.
        /// </summary>
        public IList<LibraryDependency> Dependencies { get; set; }

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

            hashCode.AddObject(Title);
            hashCode.AddObject(Version);
#pragma warning disable CS0612 // Type or member is obsolete
            hashCode.AddObject(IsDefaultVersion);
            hashCode.AddObject(HasVersionSnapshot);
            hashCode.AddObject(Description);
            hashCode.AddObject(Summary);
            hashCode.AddObject(ReleaseNotes);
            hashCode.AddSequence(Authors);
            hashCode.AddSequence(Owners);
            hashCode.AddObject(ProjectUrl);
            hashCode.AddObject(IconUrl);
            hashCode.AddObject(LicenseUrl);
            hashCode.AddObject(RequireLicenseAcceptance);
            hashCode.AddObject(Copyright);
            hashCode.AddObject(Language);
            hashCode.AddObject(BuildOptions);
            hashCode.AddSequence(Tags);
            hashCode.AddSequence(ContentFiles);
            hashCode.AddDictionary(Scripts);
            hashCode.AddDictionary(PackInclude);
            hashCode.AddObject(PackOptions);
#pragma warning restore CS0612 // Type or member is obsolete
            hashCode.AddSequence(Dependencies);
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

            return Title == other.Title &&
                   EqualityUtility.EqualsWithNullCheck(Version, other.Version) &&
#pragma warning disable CS0612 // Type or member is obsolete
                   IsDefaultVersion == other.IsDefaultVersion &&
                   HasVersionSnapshot == other.HasVersionSnapshot &&
                   Description == other.Description &&
                   Summary == other.Summary &&
                   ReleaseNotes == other.ReleaseNotes &&
                   EqualityUtility.SequenceEqualWithNullCheck(Authors, other.Authors) &&
                   EqualityUtility.SequenceEqualWithNullCheck(Owners, other.Owners) &&
                   ProjectUrl == other.ProjectUrl &&
                   IconUrl == other.IconUrl &&
                   LicenseUrl == other.LicenseUrl &&
                   RequireLicenseAcceptance == other.RequireLicenseAcceptance &&
                   Copyright == other.Copyright &&
                   Language == other.Language &&
                   EqualityUtility.EqualsWithNullCheck(BuildOptions, other.BuildOptions) &&
                   EqualityUtility.SequenceEqualWithNullCheck(Tags, other.Tags) &&
                   EqualityUtility.SequenceEqualWithNullCheck(ContentFiles, other.ContentFiles) &&
                   EqualityUtility.DictionaryOfSequenceEquals(Scripts, other.Scripts) &&
                   EqualityUtility.DictionaryEquals(PackInclude, other.PackInclude, (s, o) => StringComparer.Ordinal.Equals(s, o)) &&
                   EqualityUtility.EqualsWithNullCheck(PackOptions, other.PackOptions) &&
#pragma warning restore CS0612 // Type or member is obsolete
                   EqualityUtility.OrderedEquals(Dependencies, other.Dependencies, dep => dep.Name, StringComparer.OrdinalIgnoreCase) &&
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
                targetFrameworks = new List<TargetFrameworkInformation>(TargetFrameworks.Count);
                targetFrameworks.AddRange(TargetFrameworks.Select(item => item.Clone()));
            }

            return new PackageSpec(
                targetFrameworks,
                Dependencies?.Select(item => item.Clone()).ToList(),
                RuntimeGraph?.Clone(),
                RestoreSettings?.Clone(),
#pragma warning disable CS0612 // Type or member is obsolete
                (string[])Authors?.Clone(),
                (string[])Owners?.Clone(),
                (string[])Tags?.Clone(),
                ContentFiles != null ? new List<string>(ContentFiles) : null,
                CloneScripts(Scripts),
                PackInclude != null ? new Dictionary<string, string>(PackInclude) : null,
                PackOptions?.Clone()
#pragma warning restore CS0612 // Type or member is obsolete
                )
            {
                Name = Name,
                FilePath = FilePath,
                Title = Title,
#pragma warning disable CS0612 // Type or member is obsolete
                HasVersionSnapshot = HasVersionSnapshot,
                Description = Description,
                Summary = Summary,
                ReleaseNotes = ReleaseNotes,
                ProjectUrl = ProjectUrl,
                IconUrl = IconUrl,
                LicenseUrl = LicenseUrl,
                RequireLicenseAcceptance = RequireLicenseAcceptance,
                Language = Language,
                Copyright = Copyright,
                Version = Version,
                IsDefaultVersion = IsDefaultVersion,
                BuildOptions = BuildOptions?.Clone(),
#pragma warning restore CS0612 // Type or member is obsolete
                RestoreMetadata = RestoreMetadata?.Clone()
            };
        }

        private IDictionary<string, IEnumerable<string>> CloneScripts(IDictionary<string, IEnumerable<string>> toBeCloned)
        {
            if (toBeCloned != null)
            {
                var clone = new Dictionary<string, IEnumerable<string>>(toBeCloned.Count);
                foreach (var kvp in toBeCloned)
                {
                    clone.Add(kvp.Key, new List<string>(kvp.Value));
                }
                return clone;
            }
            return null;
        }
    }
}
