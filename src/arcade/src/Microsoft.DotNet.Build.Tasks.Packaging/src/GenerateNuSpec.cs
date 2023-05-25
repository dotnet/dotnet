// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GenerateNuSpec : Microsoft.Build.Utilities.Task
    {
        private static readonly XNamespace NuSpecXmlNamespace = @"http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd";

        public string InputFileName { get; set; }

        [Required]
        public string OutputFileName { get; set; }

        public string MinClientVersion { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Authors { get; set; }

        [Required]
        public string Owners { get; set; }

        [Required]
        public string Description { get; set; }

        public string ReleaseNotes { get; set; }

        public string Summary { get; set; }

        public string Language { get; set; }

        public string ProjectUrl { get; set; }

        public string IconUrl { get; set; }

        public string Icon { get; set; }

        public string LicenseUrl { get; set; }

        public string PackageLicenseExpression { get; set; }

        public string RepositoryType { get; set; }

        public string RepositoryUrl { get; set; }

        public string RepositoryBranch { get; set; }

        public string RepositoryCommit { get; set; }

        public string Copyright { get; set; }

        public bool RequireLicenseAcceptance { get; set; }

        public bool DevelopmentDependency { get; set; }

        public bool Serviceable { get; set; }

        public string Tags { get; set; }

        public string[] PackageTypes { get; set; }
        public ITaskItem[] Dependencies { get; set; }

        public ITaskItem[] References { get; set; }

        public ITaskItem[] FrameworkReferences { get; set; }

        public ITaskItem[] Files { get; set; }

        public override bool Execute()
        {
            try
            {
                WriteNuSpecFile();
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                Log.LogErrorFromException(ex);
            }

            return !Log.HasLoggedErrors;
        }

        private void WriteNuSpecFile()
        {
            var manifest = CreateManifest();

            if (!IsDifferent(manifest))
            {
                Log.LogMessage("Skipping generation of .nuspec because contents are identical.");
                return;
            }

            var directory = Path.GetDirectoryName(OutputFileName);
            if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var file = File.Create(OutputFileName))
            {
                Save(manifest, file);
            }
        }

        private bool IsDifferent(Manifest newManifest)
        {
            if (!File.Exists(OutputFileName))
                return true;

            // Note: don't use ReadAllText here, because it gets rid of the BOM
            // that is present at the beginning of the file.
            var oldSource = Encoding.UTF8.GetString(File.ReadAllBytes(OutputFileName));
            var newSource = "";
            using (var stream = new MemoryStream())
            {
                Save(newManifest, stream);
                stream.Seek(0, SeekOrigin.Begin);
                // UTF8.GetString preserves BOM.
                newSource = Encoding.UTF8.GetString(stream.ToArray());
            }

            return oldSource != newSource;
        }

        private void Save(Manifest manifest, Stream stream)
        {
            if (!string.IsNullOrEmpty(PackageLicenseExpression) && string.IsNullOrEmpty(LicenseUrl))
            {
                // nuget issue: https://github.com/NuGet/Home/issues/7894
                // remove licenseUrl that NuGet added from the expression.  It will still add the licenseUrl when packing, which won't break validation.
                using (var memStream = new MemoryStream())
                {
                    manifest.Save(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    var nuspec = XDocument.Load(memStream);

                    var licenseUrlElement = nuspec.Descendants(NuSpecXmlNamespace + "licenseUrl").Single();
                    licenseUrlElement?.Remove();
                    nuspec.Save(stream);
                }
            }
            else
            {
                manifest.Save(stream);
            }
        }

        private Manifest CreateManifest()
        {
            Manifest manifest;
            ManifestMetadata manifestMetadata;
            if (!string.IsNullOrEmpty(InputFileName))
            {
                using (var stream = File.OpenRead(InputFileName))
                {
                    manifest = Manifest.ReadFrom(stream, false);
                }
                if (manifest.Metadata == null)
                {
                    manifest = new Manifest(new ManifestMetadata(), manifest.Files);
                }
            }
            else
            {
                manifest = new Manifest(new ManifestMetadata());
            }


            manifestMetadata = manifest.Metadata;

            manifestMetadata.UpdateMember(x => x.Authors, Authors?.Split(';'));
            manifestMetadata.UpdateMember(x => x.Copyright, Copyright);
            manifestMetadata.UpdateMember(x => x.DependencyGroups, GetDependencySets());
            manifestMetadata.UpdateMember(x => x.Description, Description);
            manifestMetadata.DevelopmentDependency |= DevelopmentDependency;
            manifestMetadata.UpdateMember(x => x.FrameworkReferences, GetFrameworkAssemblies());
            if (!string.IsNullOrEmpty(IconUrl))
            {
                manifestMetadata.SetIconUrl(IconUrl);
            }
            if (!string.IsNullOrEmpty(Icon))
            {
                manifestMetadata.Icon = Icon;
            }
            manifestMetadata.UpdateMember(x => x.Id, Id);
            manifestMetadata.UpdateMember(x => x.Language, Language);

            if (!string.IsNullOrEmpty(PackageLicenseExpression))
            {
                manifestMetadata.LicenseMetadata = new LicenseMetadata(
                    type: LicenseType.Expression,
                    license: PackageLicenseExpression,
                    expression: NuGetLicenseExpression.Parse(PackageLicenseExpression),
                    warningsAndErrors: null,
                    LicenseMetadata.EmptyVersion);

            }
            else if (!string.IsNullOrEmpty(LicenseUrl))
            {
                manifestMetadata.SetLicenseUrl(LicenseUrl);
            }

            manifestMetadata.Repository = new RepositoryMetadata(RepositoryType ?? "", RepositoryUrl ?? "", RepositoryBranch ?? "", RepositoryCommit ?? "");

            manifestMetadata.UpdateMember(x => x.MinClientVersionString, MinClientVersion);
            manifestMetadata.UpdateMember(x => x.Owners, Owners?.Split(';'));
            if (!string.IsNullOrEmpty(ProjectUrl))
            {
                manifestMetadata.SetProjectUrl(ProjectUrl);
            }
            manifestMetadata.UpdateMember(x => x.PackageAssemblyReferences, GetReferenceSets());
            manifestMetadata.UpdateMember(x => x.ReleaseNotes, ReleaseNotes);
            manifestMetadata.RequireLicenseAcceptance |= RequireLicenseAcceptance;
            manifestMetadata.UpdateMember(x => x.Summary, Summary);
            manifestMetadata.UpdateMember(x => x.Tags, Tags);
            manifestMetadata.UpdateMember(x => x.Title, Title);
            manifestMetadata.UpdateMember(x => x.Version, Version != null ? new NuGetVersion(Version) : null);
            manifestMetadata.UpdateMember(x => x.PackageTypes, GetPackageTypes());
            manifestMetadata.Serviceable |= Serviceable;

            manifest.AddRangeToMember(x => x.Files, GetManifestFiles());

            return manifest;
        }

        private List<ManifestFile> GetManifestFiles()
        {
            IEnumerable<ManifestFile> manifestFiles =
                from f in Files.NullAsEmpty()
                where !f.GetMetadata(Metadata.FileTarget).StartsWith("$none$", StringComparison.OrdinalIgnoreCase)
                select new ManifestFile()
                {
                    Source = f.GetMetadata(Metadata.FileSource),
                    // Pattern matching in PathResolver requires that we standardize to OS specific directory separator characters
                    Target = f.GetMetadata(Metadata.FileTarget).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar),
                    Exclude = f.GetMetadata(Metadata.FileExclude)
                };

            return manifestFiles.OrderBy(f => f.Target, StringComparer.OrdinalIgnoreCase).ToList();
        }


        static FrameworkAssemblyReferenceComparer frameworkAssemblyReferenceComparer = new FrameworkAssemblyReferenceComparer();
        private List<FrameworkAssemblyReference> GetFrameworkAssemblies()
        {
            return (from fr in FrameworkReferences.NullAsEmpty()
                    orderby fr.ItemSpec, StringComparer.Ordinal
                    select new FrameworkAssemblyReference(fr.ItemSpec, new[] { fr.GetTargetFramework() })
                    ).Distinct(frameworkAssemblyReferenceComparer).ToList();
        }

        private class FrameworkAssemblyReferenceComparer : EqualityComparer<FrameworkAssemblyReference>
        {
            public override bool Equals(FrameworkAssemblyReference x, FrameworkAssemblyReference y)
            {
                return Object.Equals(x, y) ||
                    (   x != null && y != null &&
                        x.AssemblyName.Equals(y.AssemblyName) &&
                        x.SupportedFrameworks.SequenceEqual(y.SupportedFrameworks, NuGetFramework.Comparer)
                    );
            }

            public override int GetHashCode(FrameworkAssemblyReference obj)
            {
                return obj.AssemblyName.GetHashCode();
            }
        }

        private List<PackageDependencyGroup> GetDependencySets()
        {
            var dependencies = from d in Dependencies.NullAsEmpty()
                               select new Dependency
                               {
                                   Id = d.ItemSpec,
                                   Version = d.GetVersion(),
                                   TargetFramework = d.GetTargetFramework() ?? NuGetFramework.AnyFramework,
                                   Include = d.GetValueList("Include"),
                                   Exclude = d.GetValueList("Exclude")
                               };

            return (from dependency in dependencies
                    group dependency by dependency.TargetFramework into dependenciesByFramework
                    select new PackageDependencyGroup(
                        dependenciesByFramework.Key,
                        from dependency in dependenciesByFramework
                                        where dependency.Id != "_._"
                                        orderby dependency.Id, StringComparer.Ordinal
                                        group dependency by dependency.Id into dependenciesById
                                        select new PackageDependency(
                                            dependenciesById.Key,
                                            VersionRange.Parse(
                                                dependenciesById.Select(x => x.Version)
                                                .Aggregate(AggregateVersions)
                                                .ToStringSafe()),
                                            dependenciesById.Select(x => x.Include).Aggregate(AggregateInclude),
                                            dependenciesById.Select(x => x.Exclude).Aggregate(AggregateExclude)


                    ))).OrderBy(s => s?.TargetFramework?.GetShortFolderName(), StringComparer.Ordinal)
                    .ToList();
        }

        private IEnumerable<PackageReferenceSet> GetReferenceSets()
        {
            var references = from r in References.NullAsEmpty()
                             select new
                             {
                                 File = r.ItemSpec,
                                 TargetFramework = r.GetTargetFramework(),
                             };

            return (from reference in references
                    group reference by reference.TargetFramework into referencesByFramework
                    select new PackageReferenceSet(
                        referencesByFramework.Key,
                        from reference in referencesByFramework
                                       orderby reference.File, StringComparer.Ordinal
                                       select reference.File
                                       )
                    ).ToList();
        }

        private List<PackageType> GetPackageTypes()
        {
            var listOfPackageTypes = new List<PackageType>();

            // Copied and slightly modified from ParsePackageTypes():
            // https://github.com/NuGet/NuGet.Client/blob/50af5271b98ac5cb2896a707569bc4cd1e87a017/src/NuGet.Core/NuGet.Build.Tasks.Pack/PackTaskLogic.cs#L338

            foreach (var packageType in PackageTypes.TrimAndExcludeNullOrEmpty())
            {
                string[] packageTypeSplitInPart = packageType.Split(new char[] { ',' });
                string packageTypeName = packageTypeSplitInPart[0].Trim();
                var version = PackageType.EmptyVersion;
                if (packageTypeSplitInPart.Length > 1)
                {
                    string versionString = packageTypeSplitInPart[1];
                    System.Version.TryParse(versionString, out version);
                }
                listOfPackageTypes.Add(new PackageType(packageTypeName, version));
            }

            return listOfPackageTypes;
        }
        private static VersionRange AggregateVersions(VersionRange aggregate, VersionRange next)
        {
            var versionRange = new VersionRange();
            SetMinVersion(ref versionRange, aggregate);
            SetMinVersion(ref versionRange, next);
            SetMaxVersion(ref versionRange, aggregate);
            SetMaxVersion(ref versionRange, next);

            if (versionRange.MinVersion == null && versionRange.MaxVersion == null)
            {
                versionRange = null;
            }

            return versionRange;
        }

        private static IReadOnlyList<string> AggregateInclude(IReadOnlyList<string> aggregate, IReadOnlyList<string> next)
        {
            // include is a union
            if (aggregate == null)
            {
                return next;
            }

            if (next == null)
            {
                return aggregate;
            }

            return aggregate.Union(next).ToArray();
        }

        private static IReadOnlyList<string> AggregateExclude(IReadOnlyList<string> aggregate, IReadOnlyList<string> next)
        {
            // exclude is an intersection
            if (aggregate == null || next == null)
            {
                return null;
            }

            return aggregate.Intersect(next).ToArray();
        }

        private static void SetMinVersion(ref VersionRange target, VersionRange source)
        {
            if (source == null || source.MinVersion == null)
            {
                return;
            }

            bool update = false;
            NuGetVersion minVersion = target.MinVersion;
            bool includeMinVersion = target.IsMinInclusive;

            if (target.MinVersion == null)
            {
                update = true;
                minVersion = source.MinVersion;
                includeMinVersion = source.IsMinInclusive;
            }

            if (target.MinVersion < source.MinVersion)
            {
                update = true;
                minVersion = source.MinVersion;
                includeMinVersion = source.IsMinInclusive;
            }

            if (target.MinVersion == source.MinVersion)
            {
                update = true;
                includeMinVersion = target.IsMinInclusive && source.IsMinInclusive;
            }

            if (update)
            {
                target = new VersionRange(minVersion, includeMinVersion, target.MaxVersion, target.IsMaxInclusive, target.Float, target.OriginalString);
            }
        }

        private static void SetMaxVersion(ref VersionRange target, VersionRange source)
        {
            if (source == null || source.MaxVersion == null)
            {
                return;
            }

            bool update = false;
            NuGetVersion maxVersion = target.MaxVersion;
            bool includeMaxVersion = target.IsMaxInclusive;

            if (target.MaxVersion == null)
            {
                update = true;
                maxVersion = source.MaxVersion;
                includeMaxVersion = source.IsMaxInclusive;
            }

            if (target.MaxVersion > source.MaxVersion)
            {
                update = true;
                maxVersion = source.MaxVersion;
                includeMaxVersion = source.IsMaxInclusive;
            }

            if (target.MaxVersion == source.MaxVersion)
            {
                update = true;
                includeMaxVersion = target.IsMaxInclusive && source.IsMaxInclusive;
            }

            if (update)
            {
                target = new VersionRange(target.MinVersion, target.IsMinInclusive, maxVersion, includeMaxVersion, target.Float, target.OriginalString);
            }
        }

        private class Dependency
        {
            public string Id { get; set; }

            public NuGetFramework TargetFramework { get; set; }

            public VersionRange Version { get; set; }

            public IReadOnlyList<string> Exclude { get; set; }

            public IReadOnlyList<string> Include { get; set; }
        }
    }
}
