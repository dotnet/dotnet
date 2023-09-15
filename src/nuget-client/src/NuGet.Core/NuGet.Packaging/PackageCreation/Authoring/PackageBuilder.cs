// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using NuGet.Client;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageCreation.Resources;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    public class PackageBuilder : IPackageMetadata
    {
        private static readonly Uri DefaultUri = new Uri("http://defaultcontainer/");
        private static readonly DateTime ZipFormatMinDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime ZipFormatMaxDate = new DateTime(2107, 12, 31, 23, 59, 58, DateTimeKind.Utc);
        internal const string ManifestRelationType = "manifest";
        private readonly bool _includeEmptyDirectories;
        private readonly bool _deterministic;
        private readonly ILogger _logger;

        /// <summary>
        /// Maximum Icon file size: 1 megabyte
        /// </summary>
        public const int MaxIconFileSize = 1024 * 1024;

        public PackageBuilder(string path, Func<string, string> propertyProvider, bool includeEmptyDirectories)
            : this(path, propertyProvider, includeEmptyDirectories, deterministic: false)
        {
        }

        public PackageBuilder(string path, Func<string, string> propertyProvider, bool includeEmptyDirectories, bool deterministic)
            : this(path, Path.GetDirectoryName(path), propertyProvider, includeEmptyDirectories, deterministic)
        {
        }

        public PackageBuilder(string path, Func<string, string> propertyProvider, bool includeEmptyDirectories, bool deterministic, ILogger logger)
            : this(path, Path.GetDirectoryName(path), propertyProvider, includeEmptyDirectories, deterministic, logger)
        {
        }

        public PackageBuilder(string path, string basePath, Func<string, string> propertyProvider, bool includeEmptyDirectories)
            : this(path, basePath, propertyProvider, includeEmptyDirectories, deterministic: false)
        {
        }

        public PackageBuilder(string path, string basePath, Func<string, string> propertyProvider, bool includeEmptyDirectories, bool deterministic, ILogger logger)
            : this(path, basePath, propertyProvider, includeEmptyDirectories, deterministic)
        {
            _logger = logger;
        }

        public PackageBuilder(string path, string basePath, Func<string, string> propertyProvider, bool includeEmptyDirectories, bool deterministic)
            : this(includeEmptyDirectories, deterministic)
        {
            if (!File.Exists(path))
            {
                throw new PackagingException(
                    NuGetLogCode.NU5008,
                    string.Format(CultureInfo.CurrentCulture, Strings.ErrorManifestFileNotFound, path ?? "null"));
            }

            using (Stream stream = File.OpenRead(path))
            {
                ReadManifest(stream, basePath, propertyProvider);
            }
        }

        public PackageBuilder(Stream stream, string basePath)
            : this(stream, basePath, null)
        {
        }

        public PackageBuilder(Stream stream, string basePath, Func<string, string> propertyProvider)
            : this()
        {
            ReadManifest(stream, basePath, propertyProvider);
        }

        public PackageBuilder(bool deterministic) :
            this(includeEmptyDirectories: false, deterministic: deterministic)
        {

        }

        public PackageBuilder()
            : this(includeEmptyDirectories: false, deterministic: false)
        {
        }

        public PackageBuilder(bool deterministic, ILogger logger)
            : this(includeEmptyDirectories: false, deterministic: deterministic, logger)
        {
        }

        private PackageBuilder(bool includeEmptyDirectories, bool deterministic)
            : this(includeEmptyDirectories: false, deterministic: deterministic, logger: NullLogger.Instance)
        {
        }

        private PackageBuilder(bool includeEmptyDirectories, bool deterministic, ILogger logger)
        {
            _includeEmptyDirectories = includeEmptyDirectories;
            _deterministic = false; // fix in https://github.com/NuGet/Home/issues/8601
            _logger = logger;
            Files = new Collection<IPackageFile>();
            DependencyGroups = new Collection<PackageDependencyGroup>();
            FrameworkReferences = new Collection<FrameworkAssemblyReference>();
            FrameworkReferenceGroups = new Collection<FrameworkReferenceGroup>();
            ContentFiles = new Collection<ManifestContentFiles>();
            PackageAssemblyReferences = new Collection<PackageReferenceSet>();
            PackageTypes = new Collection<PackageType>();
            Authors = new HashSet<string>();
            Owners = new HashSet<string>();
            Tags = new HashSet<string>();
            TargetFrameworks = new List<NuGetFramework>();
            // Just like parameter replacements, these are also case insensitive, for consistency.
            Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Id
        {
            get;
            set;
        }

        public NuGetVersion Version
        {
            get;
            set;
        }

        public RepositoryMetadata Repository { get; set; }

        public LicenseMetadata LicenseMetadata { get; set; }

        public bool HasSnapshotVersion
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public ISet<string> Authors
        {
            get;
            private set;
        }

        public ISet<string> Owners
        {
            get;
            private set;
        }

        public Uri IconUrl
        {
            get;
            set;
        }

        public string Icon
        {
            get;
            set;
        }

        public Uri LicenseUrl
        {
            get;
            set;
        }

        public Uri ProjectUrl
        {
            get;
            set;
        }

        public bool RequireLicenseAcceptance
        {
            get;
            set;
        }

        public bool EmitRequireLicenseAcceptance
        {
            get;
            set;
        } = true;

        public bool Serviceable
        {
            get;
            set;
        }

        public bool DevelopmentDependency
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Summary
        {
            get;
            set;
        }

        public string ReleaseNotes
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public string OutputName
        {
            get;
            set;
        }

        public ISet<string> Tags
        {
            get;
            private set;
        }

        public string Readme { get; set; }

        /// <summary>
        /// Exposes the additional properties extracted by the metadata
        /// extractor or received from the command line.
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get;
            private set;
        }

        public string Copyright
        {
            get;
            set;
        }

        public Collection<PackageDependencyGroup> DependencyGroups
        {
            get;
            private set;
        }

        public ICollection<IPackageFile> Files
        {
            get;
            private set;
        }

        public Collection<FrameworkAssemblyReference> FrameworkReferences
        {
            get;
            private set;
        }

        public Collection<FrameworkReferenceGroup> FrameworkReferenceGroups
        {
            get;
            private set;
        }

        public IList<NuGetFramework> TargetFrameworks
        {
            get;
            set;
        }

        /// <summary>
        /// ContentFiles section from the manifest for content v2
        /// </summary>
        public ICollection<ManifestContentFiles> ContentFiles
        {
            get;
            private set;
        }

        public ICollection<PackageReferenceSet> PackageAssemblyReferences
        {
            get;
            set;
        }

        public ICollection<PackageType> PackageTypes
        {
            get;
            set;
        }

        IEnumerable<string> IPackageMetadata.Authors
        {
            get
            {
                return Authors;
            }
        }

        IEnumerable<string> IPackageMetadata.Owners
        {
            get
            {
                return Owners;
            }
        }

        string IPackageMetadata.Tags
        {
            get
            {
                return string.Join(" ", Tags);
            }
        }

        IEnumerable<PackageReferenceSet> IPackageMetadata.PackageAssemblyReferences
        {
            get
            {
                return PackageAssemblyReferences;
            }
        }

        IEnumerable<PackageDependencyGroup> IPackageMetadata.DependencyGroups
        {
            get
            {
                return DependencyGroups;
            }
        }

        IEnumerable<FrameworkAssemblyReference> IPackageMetadata.FrameworkReferences
        {
            get
            {
                return FrameworkReferences;
            }
        }

        IEnumerable<ManifestContentFiles> IPackageMetadata.ContentFiles
        {
            get
            {
                return ContentFiles;
            }
        }

        IEnumerable<PackageType> IPackageMetadata.PackageTypes
        {
            get
            {
                return PackageTypes;
            }
        }

        IEnumerable<FrameworkReferenceGroup> IPackageMetadata.FrameworkReferenceGroups => FrameworkReferenceGroups;

        public Version MinClientVersion
        {
            get;
            set;
        }

        public void Save(Stream stream)
        {
            // Make sure we're saving a valid package id
            PackageIdValidator.ValidatePackageId(Id);

            // Throw if the package doesn't contain any dependencies nor content
            if (!Files.Any() && !DependencyGroups.SelectMany(d => d.Packages).Any() && !FrameworkReferences.Any() && !FrameworkReferenceGroups.Any())
            {
                throw new PackagingException(NuGetLogCode.NU5017, NuGetResources.CannotCreateEmptyPackage);
            }

            ValidateDependencies(Version, DependencyGroups);
            ValidateFilesUnique(Files);
            ValidateReferenceAssemblies(Files, PackageAssemblyReferences);
            ValidateFrameworkAssemblies(FrameworkReferences, FrameworkReferenceGroups);
            ValidateLicenseFile(Files, LicenseMetadata);
            ValidateIconFile(Files, Icon);
            ValidateFileFrameworks(Files);
            ValidateReadmeFile(Files, Readme);

            using (var package = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                string psmdcpPath = $"package/services/metadata/core-properties/{CalcPsmdcpName()}.psmdcp";

                // Validate and write the manifest
                WriteManifest(package, DetermineMinimumSchemaVersion(Files, DependencyGroups), psmdcpPath);

                // Write the files to the package
                HashSet<string> filesWithoutExtensions = new HashSet<string>();
                var extensions = WriteFiles(package, filesWithoutExtensions);

                extensions.Add("nuspec");

                WriteOpcContentTypes(package, extensions, filesWithoutExtensions);

                WriteOpcPackageProperties(package, psmdcpPath);
            }
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private string CalcPsmdcpName()
        {
            if (_deterministic)
            {
                using (var hashFunc = new Sha512HashFunction())
                {
                    foreach (var file in Files)
                    {
                        var data = ReadAllBytes(file.GetStream());
                        hashFunc.Update(data, 0, data.Length);
                    }
                    return EncodeHexString(hashFunc.GetHashBytes()).Substring(0, 32);
                }
            }
            else
            {
                return Guid.NewGuid().ToString("N", provider: null);
            }
        }

        private static readonly char[] HexValues = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        // Reference https://github.com/dotnet/corefx/blob/2c2e4a599889652ec579a870054b0f8915ea70fd/src/System.Security.Cryptography.Xml/src/System/Security/Cryptography/Xml/Utils.cs#L736
        internal static string EncodeHexString(byte[] sArray)
        {
            uint start = 0;
            uint end = (uint)sArray.Length;
            string result = null;
            if (sArray != null)
            {
                char[] hexOrder = new char[(end - start) * 2];
                uint digit;
                for (uint i = start, j = 0; i < end; i++)
                {
                    digit = (uint)((sArray[i] & 0xf0) >> 4);
                    hexOrder[j++] = HexValues[digit];
                    digit = (uint)(sArray[i] & 0x0f);
                    hexOrder[j++] = HexValues[digit];
                }
                result = new string(hexOrder);
            }
            return result;
        }

        private static string CreatorInfo()
        {
            List<string> creatorInfo = new List<string>();
            var assembly = typeof(PackageBuilder).GetTypeInfo().Assembly;
            creatorInfo.Add(assembly.FullName);
#if !IS_CORECLR // CORECLR_TODO: Environment.OSVersion
            creatorInfo.Add(Environment.OSVersion.ToString());
#endif

            var attribute = assembly.GetCustomAttributes<System.Runtime.Versioning.TargetFrameworkAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                creatorInfo.Add(attribute.FrameworkDisplayName);
            }

            return string.Join(";", creatorInfo);
        }

        private static int DetermineMinimumSchemaVersion(
            ICollection<IPackageFile> Files,
            ICollection<PackageDependencyGroup> package)
        {
            if (HasContentFilesV2(Files) || HasIncludeExclude(package))
            {
                // version 5
                return ManifestVersionUtility.XdtTransformationVersion;
            }

            if (HasXdtTransformFile(Files))
            {
                // version 5
                return ManifestVersionUtility.XdtTransformationVersion;
            }

            if (RequiresV4TargetFrameworkSchema(Files))
            {
                // version 4
                return ManifestVersionUtility.TargetFrameworkSupportForDependencyContentsAndToolsVersion;
            }

            return ManifestVersionUtility.DefaultVersion;
        }

        private static bool RequiresV4TargetFrameworkSchema(ICollection<IPackageFile> files)
        {
            // check if any file under Content or Tools has TargetFramework defined
            bool hasContentOrTool = files.Any(
                f => f.NuGetFramework != null &&
                     !f.NuGetFramework.IsUnsupported &&
                     (f.Path.StartsWith(PackagingConstants.Folders.Content + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
                      f.Path.StartsWith(PackagingConstants.Folders.Tools + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)));

            if (hasContentOrTool)
            {
                return true;
            }

            // now check if the Lib folder has any empty framework folder
            bool hasEmptyLibFolder = files.Any(
                f => f.NuGetFramework != null &&
                     f.Path.StartsWith(PackagingConstants.Folders.Lib + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
                     f.EffectivePath == PackagingConstants.PackageEmptyFileName);

            return hasEmptyLibFolder;
        }

        private static bool HasContentFilesV2(ICollection<IPackageFile> contentFiles)
        {
            return contentFiles.Any(file =>
                file.Path != null &&
                file.Path.StartsWith(PackagingConstants.Folders.ContentFiles + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase));
        }

        private static bool HasIncludeExclude(IEnumerable<PackageDependencyGroup> dependencyGroups)
        {
            return dependencyGroups.Any(dependencyGroup =>
                dependencyGroup.Packages
                   .Any(dependency => dependency.Include != null || dependency.Exclude != null));
        }

        private static bool HasXdtTransformFile(ICollection<IPackageFile> contentFiles)
        {
            return contentFiles.Any(file =>
                file.Path != null &&
                file.Path.StartsWith(PackagingConstants.Folders.Content + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
                (file.Path.EndsWith(".install.xdt", StringComparison.OrdinalIgnoreCase) ||
                 file.Path.EndsWith(".uninstall.xdt", StringComparison.OrdinalIgnoreCase)));
        }

        private static void ValidateDependencies(SemanticVersion version,
            IEnumerable<PackageDependencyGroup> dependencies)
        {
            var frameworksMissingPlatformVersion = new HashSet<string>(dependencies
                .Select(group => group.TargetFramework)
                .Where(groupFramework => groupFramework.HasPlatform && groupFramework.PlatformVersion == FrameworkConstants.EmptyVersion)
                .Select(framework => framework.GetShortFolderName()));
            if (frameworksMissingPlatformVersion.Any())
            {
                throw new PackagingException(NuGetLogCode.NU1012, string.Format(CultureInfo.CurrentCulture, Strings.MissingTargetPlatformVersionsFromDependencyGroups, string.Join(", ", frameworksMissingPlatformVersion.OrderBy(str => str))));
            }

            if (version == null)
            {
                // We have independent validation for null-versions.
                return;
            }

            foreach (var dep in dependencies.SelectMany(s => s.Packages))
            {
                PackageIdValidator.ValidatePackageId(dep.Id);
            }
        }

        public static void ValidateReferenceAssemblies(IEnumerable<IPackageFile> files, IEnumerable<PackageReferenceSet> packageAssemblyReferences)
        {
            var frameworksMissingPlatformVersion = new HashSet<string>(packageAssemblyReferences
                .Select(group => group.TargetFramework)
                .Where(groupFramework => groupFramework != null && groupFramework.HasPlatform && groupFramework.PlatformVersion == FrameworkConstants.EmptyVersion)
                .Select(framework => framework.GetShortFolderName()));
            if (frameworksMissingPlatformVersion.Any())
            {
                throw new PackagingException(NuGetLogCode.NU1012, string.Format(CultureInfo.CurrentCulture, Strings.MissingTargetPlatformVersionsFromReferenceGroups, string.Join(", ", frameworksMissingPlatformVersion.OrderBy(str => str))));
            }

            var libFiles = new HashSet<string>(from file in files
                                               where !string.IsNullOrEmpty(file.Path) && file.Path.StartsWith("lib", StringComparison.OrdinalIgnoreCase)
                                               select Path.GetFileName(file.Path), StringComparer.OrdinalIgnoreCase);

            foreach (var reference in packageAssemblyReferences.SelectMany(p => p.References))
            {
                if (!libFiles.Contains(reference) &&
                    !libFiles.Contains(reference + ".dll") &&
                    !libFiles.Contains(reference + ".exe") &&
                    !libFiles.Contains(reference + ".winmd"))
                {
                    throw new PackagingException(NuGetLogCode.NU5018, string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_InvalidReference, reference));
                }
            }
        }

        private static void ValidateFrameworkAssemblies(IEnumerable<FrameworkAssemblyReference> references, IEnumerable<FrameworkReferenceGroup> referenceGroups)
        {
            // Check standalone references
            var frameworksMissingPlatformVersion = new HashSet<string>(references
                .SelectMany(reference => reference.SupportedFrameworks)
                .Where(framework => framework.HasPlatform && framework.PlatformVersion == FrameworkConstants.EmptyVersion)
                .Select(framework => framework.GetShortFolderName())
            );
            if (frameworksMissingPlatformVersion.Any())
            {
                throw new PackagingException(NuGetLogCode.NU1012, string.Format(CultureInfo.CurrentCulture, Strings.MissingTargetPlatformVersionsFromFrameworkAssemblyReferences, string.Join(", ", frameworksMissingPlatformVersion.OrderBy(str => str))));
            }

            // Check reference groups too
            frameworksMissingPlatformVersion = new HashSet<string>(referenceGroups
                .Select(group => group.TargetFramework)
                .Where(groupFramework => groupFramework.HasPlatform && groupFramework.PlatformVersion == FrameworkConstants.EmptyVersion)
                .Select(framework => framework.GetShortFolderName()));
            if (frameworksMissingPlatformVersion.Any())
            {
                throw new PackagingException(NuGetLogCode.NU1012, string.Format(CultureInfo.CurrentCulture, Strings.MissingTargetPlatformVersionsFromFrameworkAssemblyGroups, string.Join(", ", frameworksMissingPlatformVersion.OrderBy(str => str))));
            }
        }

        /// <summary>Looks for the specified file within the package</summary>
        /// <param name="filePath">The file path to search for</param>
        /// <param name="packageFiles">The list of files to search within</param>
        /// <param name="filePathIncorrectCase">If the file was not found, this will be a path which almost matched but had the incorrect case</param>
        /// <returns>An <see cref="IPackageFile"/> matching the specified path or <see langword="null" /></returns>
        private static IPackageFile FindFileInPackage(string filePath, IEnumerable<IPackageFile> packageFiles, out string filePathIncorrectCase)
        {
            filePathIncorrectCase = null;
            var strippedFilePath = PathUtility.StripLeadingDirectorySeparators(filePath);

            foreach (var packageFile in packageFiles)
            {
                var strippedPackageFilePath = PathUtility.StripLeadingDirectorySeparators(packageFile.Path);

                // This must use a case-sensitive string comparison, even on systems where file paths are normally case-sensitive.
                // This is because Zip files are treated as case-sensitive. (See https://github.com/NuGet/Home/issues/9817)
                if (strippedPackageFilePath.Equals(strippedFilePath, StringComparison.Ordinal))
                {
                    // Found the requested file in the package
                    filePathIncorrectCase = null;
                    return packageFile;
                }
                // Check for files that exist with the wrong file casing
                else if (filePathIncorrectCase is null && strippedPackageFilePath.Equals(strippedFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    filePathIncorrectCase = strippedPackageFilePath;
                }
            }

            // We searched all of the package files and didn't find what we were looking for
            return null;
        }

        private void ValidateFilesUnique(IEnumerable<IPackageFile> files)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var duplicates = new HashSet<string>(StringComparer.Ordinal);
            foreach (string destination in files.Where(t => t.Path != null).Select(t => PathUtility.GetPathWithDirectorySeparator(t.Path)))
            {
                if (!seen.Add(destination))
                {
                    duplicates.Add(destination);
                }
            }
            if (duplicates.Any())
            {
                throw new PackagingException(NuGetLogCode.NU5050, string.Format(CultureInfo.CurrentCulture, NuGetResources.FoundDuplicateFile, string.Join(", ", duplicates)));
            }

        }

        private void ValidateLicenseFile(IEnumerable<IPackageFile> files, LicenseMetadata licenseMetadata)
        {
            if (!PackageTypes.Contains(PackageType.SymbolsPackage) && licenseMetadata?.Type == LicenseType.File)
            {
                var ext = Path.GetExtension(licenseMetadata.License);
                if (!string.IsNullOrEmpty(ext) &&
                        !ext.Equals(".txt", StringComparison.OrdinalIgnoreCase) &&
                        !ext.Equals(".md", StringComparison.OrdinalIgnoreCase))
                {
                    throw new PackagingException(NuGetLogCode.NU5031, string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_LicenseFileExtensionIsInvalid, licenseMetadata.License));
                }

                if (FindFileInPackage(licenseMetadata.License, files, out var licenseFilePathWithIncorrectCase) is null)
                {
                    string errorMessage;
                    if (licenseFilePathWithIncorrectCase is null)
                    {
                        errorMessage = string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_LicenseFileIsNotInNupkg, licenseMetadata.License);
                    }
                    else
                    {
                        errorMessage = string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_LicenseFileIsNotInNupkgWithHint, licenseMetadata.License, licenseFilePathWithIncorrectCase);
                    }

                    throw new PackagingException(NuGetLogCode.NU5030, errorMessage);
                }
            }
        }

        /// <summary>
        /// Given a list of resolved files,
        /// determine which file will be used as the icon file and validate its size and extension.
        /// </summary>
        /// <param name="files">Files resolved from the file entries in the nuspec</param>
        /// <param name="iconPath">icon entry found in the .nuspec</param>
        /// <exception cref="PackagingException">When a validation rule is not met</exception>
        private void ValidateIconFile(IEnumerable<IPackageFile> files, string iconPath)
        {
            if (!PackageTypes.Contains(PackageType.SymbolsPackage) && !string.IsNullOrEmpty(iconPath))
            {
                var ext = Path.GetExtension(iconPath);
                if (string.IsNullOrEmpty(ext) || (
                        !ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                        !ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                        !ext.Equals(".png", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5045,
                        string.Format(CultureInfo.CurrentCulture, NuGetResources.IconInvalidExtension, iconPath));
                }

                // Validate entry
                IPackageFile iconFile = FindFileInPackage(iconPath, files, out var iconPathWithIncorrectCase);

                if (iconFile is null)
                {
                    string errorMessage;
                    if (iconPathWithIncorrectCase is null)
                    {
                        errorMessage = string.Format(CultureInfo.CurrentCulture, NuGetResources.IconNoFileElement, iconPath);
                    }
                    else
                    {
                        errorMessage = string.Format(CultureInfo.CurrentCulture, NuGetResources.IconNoFileElementWithHint, iconPath, iconPathWithIncorrectCase);
                    }

                    throw new PackagingException(NuGetLogCode.NU5046, errorMessage);
                }

                try
                {
                    // Validate Icon open file
                    using (var iconStream = iconFile.GetStream())
                    {
                        // Validate file size
                        long fileSize = iconStream.Length;

                        if (fileSize > MaxIconFileSize)
                        {
                            throw new PackagingException(Common.NuGetLogCode.NU5047, NuGetResources.IconMaxFileSizeExceeded);
                        }

                        if (fileSize == 0)
                        {
                            throw new PackagingException(Common.NuGetLogCode.NU5047, NuGetResources.IconErrorEmpty);
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5046,
                        string.Format(CultureInfo.CurrentCulture, NuGetResources.IconCannotOpenFile, iconPath, e.Message));
                }
            }
        }

        private static void ValidateFileFrameworks(IEnumerable<IPackageFile> files)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var file in files.Where(t => t.Path != null).Select(t => PathUtility.GetPathWithDirectorySeparator(t.Path)))
            {
                set.Add(file);
            }

            var managedCodeConventions = new ManagedCodeConventions(RuntimeGraph.Empty);
            var collection = new ContentItemCollection();
            collection.Load(set.Select(path => path.Replace('\\', '/')).ToArray());

            var patterns = managedCodeConventions.Patterns;

            var frameworkPatterns = new List<PatternSet>()
            {
                patterns.RuntimeAssemblies,
                patterns.CompileRefAssemblies,
                patterns.CompileLibAssemblies,
                patterns.NativeLibraries,
                patterns.ResourceAssemblies,
                patterns.MSBuildFiles,
                patterns.ContentFiles,
                patterns.ToolsAssemblies,
                patterns.EmbedAssemblies,
                patterns.MSBuildTransitiveFiles
            };
            var warnPaths = new HashSet<string>();

            var itemsWithFrameworkMissingPlatformVersion = new HashSet<string>();
            List<ContentItemGroup> targetedItemGroups = new();
            foreach (var pattern in frameworkPatterns)
            {
                targetedItemGroups.Clear();
                ContentExtractor.GetContentForPattern(collection, pattern, targetedItemGroups);
                foreach (ContentItemGroup group in targetedItemGroups)
                {
                    foreach (ContentItem item in group.Items.NoAllocEnumerate())
                    {
                        var framework = (NuGetFramework)item.Properties["tfm"];
                        if (framework == null)
                        {
                            continue;
                        }

                        if (framework.HasPlatform && framework.PlatformVersion == FrameworkConstants.EmptyVersion)
                        {
                            itemsWithFrameworkMissingPlatformVersion.Add(item.Path);
                        }
                    }
                }
            }

            if (itemsWithFrameworkMissingPlatformVersion.Any())
            {
                throw new PackagingException(NuGetLogCode.NU1012, string.Format(CultureInfo.CurrentCulture, Strings.MissingTargetPlatformVersionsFromIncludedFiles, string.Join(", ", itemsWithFrameworkMissingPlatformVersion.OrderBy(str => str))));
            }
        }

        /// <summary>
        /// Validate that the readme file is of the correct size/type and can be opened properly except for Symbol packages.
        /// </summary>
        /// <param name="files">Files resolved from the file entries in the nuspec</param>
        /// <param name="readmePath">readmepath found in the .nuspec</param>
        /// <exception cref="PackagingException">When a validation rule is not met</exception>
        private void ValidateReadmeFile(IEnumerable<IPackageFile> files, string readmePath)
        {
            if (!PackageTypes.Contains(PackageType.SymbolsPackage) && !string.IsNullOrEmpty(readmePath))
            {
                // Validate readme extension
                var extension = Path.GetExtension(readmePath);

                if (!string.IsNullOrEmpty(extension) &&
                    !extension.Equals(NuGetConstants.ReadmeExtension, StringComparison.OrdinalIgnoreCase))
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5038,
                        string.Format(CultureInfo.CurrentCulture, NuGetResources.ReadmeFileExtensionIsInvalid, readmePath));
                }

                // Validate entry
                var readmePathStripped = PathUtility.StripLeadingDirectorySeparators(readmePath);

                var readmeFileList = files.Where(f =>
                        readmePathStripped.Equals(
                            PathUtility.StripLeadingDirectorySeparators(f.Path),
                            PathUtility.GetStringComparisonBasedOnOS()));

                if (!readmeFileList.Any())
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5039,
                        string.Format(CultureInfo.CurrentCulture, NuGetResources.ReadmeNoFileElement, readmePath));
                }

                IPackageFile readmeFile = readmeFileList.First();

                try
                {
                    // Validate Readme open file
                    using (var readmeStream = readmeFile.GetStream())
                    {
                        // Validate file size is not 0
                        long fileSize = readmeStream.Length;

                        if (fileSize == 0)
                        {
                            throw new PackagingException(
                                NuGetLogCode.NU5040,
                                string.Format(CultureInfo.CurrentCulture, NuGetResources.ReadmeErrorEmpty, readmePath));
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5041,
                        string.Format(CultureInfo.CurrentCulture, NuGetResources.ReadmeCannotOpenFile, readmePath, e.Message));
                }
            }
        }

        private void ReadManifest(Stream stream, string basePath, Func<string, string> propertyProvider)
        {
            // Deserialize the document and extract the metadata
            Manifest manifest = Manifest.ReadFrom(stream, propertyProvider, validateSchema: true);

            Populate(manifest.Metadata);

            // If there's no base path then ignore the files node
            if (basePath != null)
            {
                if (!manifest.HasFilesNode)
                {
                    AddFiles(basePath, @"**\*", null);
                }
                else
                {
                    PopulateFiles(basePath, manifest.Files);
                }
            }
        }

        public void Populate(ManifestMetadata manifestMetadata)
        {
            IPackageMetadata metadata = manifestMetadata;
            Id = metadata.Id;
            Version = metadata.Version;
            Title = metadata.Title;
            Authors.AddRange(metadata.Authors);
            Owners.AddRange(metadata.Owners);
            IconUrl = metadata.IconUrl;
            LicenseUrl = metadata.LicenseUrl;
            ProjectUrl = metadata.ProjectUrl;
            RequireLicenseAcceptance = metadata.RequireLicenseAcceptance;
            DevelopmentDependency = metadata.DevelopmentDependency;
            Serviceable = metadata.Serviceable;
            Description = metadata.Description;
            Summary = metadata.Summary;
            ReleaseNotes = metadata.ReleaseNotes;
            Language = metadata.Language;
            Copyright = metadata.Copyright;
            MinClientVersion = metadata.MinClientVersion;
            Repository = metadata.Repository;
            ContentFiles = new Collection<ManifestContentFiles>(manifestMetadata.ContentFiles.ToList());
            LicenseMetadata = metadata.LicenseMetadata;
            Icon = metadata.Icon;
            Readme = metadata.Readme;

            if (metadata.Tags != null)
            {
                Tags.AddRange(ParseTags(metadata.Tags));
            }

            DependencyGroups.AddRange(metadata.DependencyGroups);
            FrameworkReferences.AddRange(metadata.FrameworkReferences);
            FrameworkReferenceGroups.AddRange(metadata.FrameworkReferenceGroups);

            if (manifestMetadata.PackageAssemblyReferences != null)
            {
                PackageAssemblyReferences.AddRange(manifestMetadata.PackageAssemblyReferences);
            }

            if (manifestMetadata.PackageTypes != null)
            {
                PackageTypes = new Collection<PackageType>(metadata.PackageTypes.ToList());
            }
        }

        public void PopulateFiles(string basePath, IEnumerable<ManifestFile> files)
        {
            foreach (var file in files)
            {
                AddFiles(basePath, file.Source, file.Target, file.Exclude);
            }
        }

        private ZipArchiveEntry CreateEntry(ZipArchive package, string entryName, CompressionLevel compressionLevel)
        {
            var entry = package.CreateEntry(entryName, compressionLevel);
            if (_deterministic)
            {
                entry.LastWriteTime = ZipFormatMinDate;
            }
            return entry;
        }

        private static ZipArchiveEntry CreatePackageFileEntry(ZipArchive package, string entryName, DateTimeOffset timeOffset, CompressionLevel compressionLevel, StringBuilder warningMessage)
        {
            var entry = package.CreateEntry(entryName, compressionLevel);

            if (timeOffset.UtcDateTime < ZipFormatMinDate)
            {
                warningMessage.AppendLine(StringFormatter.ZipFileTimeStampModifiedMessage(entryName, timeOffset.DateTime.ToShortDateString(), ZipFormatMinDate.ToShortDateString()));
                entry.LastWriteTime = ZipFormatMinDate;
            }
            else if (timeOffset.UtcDateTime > ZipFormatMaxDate)
            {
                warningMessage.AppendLine(StringFormatter.ZipFileTimeStampModifiedMessage(entryName, timeOffset.DateTime.ToShortDateString(), ZipFormatMaxDate.ToShortDateString()));
                entry.LastWriteTime = ZipFormatMaxDate;
            }
            else
            {
                entry.LastWriteTime = timeOffset.UtcDateTime;
            }

            return entry;
        }

        private void WriteManifest(ZipArchive package, int minimumManifestVersion, string psmdcpPath)
        {
            var path = Id + PackagingConstants.ManifestExtension;

            WriteOpcManifestRelationship(package, path, psmdcpPath);

            var entry = CreateEntry(package, path, CompressionLevel.Optimal);

            using (var stream = entry.Open())
            {
                var manifest = Manifest.Create(this);
                manifest.Save(stream, minimumManifestVersion);
            }
        }

        private HashSet<string> WriteFiles(ZipArchive package, HashSet<string> filesWithoutExtensions)
        {
            var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var warningMessage = new StringBuilder();

            // Add files that might not come from expanding files on disk
            foreach (IPackageFile file in new HashSet<IPackageFile>(Files))
            {
                using (Stream stream = file.GetStream())
                {
                    try
                    {
                        CreatePart(
                            package,
                            file.Path,
                            stream,
                            lastWriteTime: _deterministic ? ZipFormatMinDate : file.LastWriteTime,
                            warningMessage);
                        var fileExtension = Path.GetExtension(file.Path);

                        // We have files without extension (e.g. the executables for Nix)
                        if (!string.IsNullOrEmpty(fileExtension))
                        {
                            extensions.Add(fileExtension.Substring(1));
                        }
                        else
                        {
#if NETCOREAPP
                            filesWithoutExtensions.Add($"/{file.Path.Replace("\\", "/", StringComparison.Ordinal)}");
#else
                            filesWithoutExtensions.Add($"/{file.Path.Replace("\\", "/")}");
#endif
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            if (warningMessage.Length > Environment.NewLine.Length)
            {
                warningMessage.Length -= Environment.NewLine.Length;
            }

            var warningMessageString = warningMessage.ToString();

            if (!string.IsNullOrEmpty(warningMessageString))
            {
                _logger?.Log(PackagingLogMessage.CreateWarning(StringFormatter.ZipFileTimeStampModifiedWarning(warningMessageString), NuGetLogCode.NU5132));
            }

            return extensions;
        }

        public void AddFiles(string basePath, string source, string destination, string exclude = null)
        {
            exclude = exclude?.Replace('\\', Path.DirectorySeparatorChar);

            List<PhysicalPackageFile> searchFiles = ResolveSearchPattern(basePath, source.Replace('\\', Path.DirectorySeparatorChar), destination, _includeEmptyDirectories).ToList();

            if (_includeEmptyDirectories)
            {
                // we only allow empty directories which are under known root folders.
                searchFiles.RemoveAll(file => Path.GetFileName(file.TargetPath) == PackagingConstants.PackageEmptyFileName
                                             && !IsKnownFolder(file.TargetPath));
            }

            ExcludeFiles(searchFiles, basePath, exclude);

            // Don't throw if the exclude is what made this find no files. Adding files from
            // project.json ends up calling this one file at a time where some may be filtered out.
            if (!PathResolver.IsWildcardSearch(source) && !PathResolver.IsDirectoryPath(source) && !searchFiles.Any() && string.IsNullOrEmpty(exclude))
            {
                throw new PackagingException(NuGetLogCode.NU5019,
                    string.Format(CultureInfo.CurrentCulture, NuGetResources.PackageAuthoring_FileNotFound, source));
            }

            Files.AddRange(searchFiles);
        }

        internal static IEnumerable<PhysicalPackageFile> ResolveSearchPattern(string basePath, string searchPath, string targetPath, bool includeEmptyDirectories)
        {
            string normalizedBasePath;
            IEnumerable<PathResolver.SearchPathResult> searchResults = PathResolver.PerformWildcardSearch(basePath, searchPath, includeEmptyDirectories, out normalizedBasePath);

            return searchResults.Select(result =>
                result.IsFile
                    ? new PhysicalPackageFile
                    {
                        SourcePath = result.Path,
                        TargetPath = ResolvePackagePath(normalizedBasePath, searchPath, result.Path, targetPath)
                    }
                    : new EmptyFrameworkFolderFile(ResolvePackagePath(normalizedBasePath, searchPath, result.Path, targetPath))
                    {
                        SourcePath = result.Path
                    }
            );
        }

        /// <summary>
        /// Determins the path of the file inside a package.
        /// For recursive wildcard paths, we preserve the path portion beginning with the wildcard.
        /// For non-recursive wildcard paths, we use the file name from the actual file path on disk.
        /// </summary>
        internal static string ResolvePackagePath(string searchDirectory, string searchPattern, string fullPath, string targetPath)
        {
            string packagePath;
            bool isDirectorySearch = PathResolver.IsDirectoryPath(searchPattern);
            bool isWildcardSearch = PathResolver.IsWildcardSearch(searchPattern);
            bool isRecursiveWildcardSearch = isWildcardSearch && searchPattern.IndexOf("**", StringComparison.OrdinalIgnoreCase) != -1;

            if ((isRecursiveWildcardSearch || isDirectorySearch) && fullPath.StartsWith(searchDirectory, StringComparison.OrdinalIgnoreCase))
            {
                // The search pattern is recursive. Preserve the non-wildcard portion of the path.
                // e.g. Search: X:\foo\**\*.cs results in SearchDirectory: X:\foo and a file path of X:\foo\bar\biz\boz.cs
                // Truncating X:\foo\ would result in the package path.
                packagePath = fullPath.Substring(searchDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
            }
            else if (!isWildcardSearch && Path.GetExtension(searchPattern).Equals(Path.GetExtension(targetPath), StringComparison.OrdinalIgnoreCase))
            {
                // If the search does not contain wild cards, and the target path shares the same extension, copy it
                // e.g. <file src="ie\css\style.css" target="Content\css\ie.css" /> --> Content\css\ie.css
                return targetPath;
            }
            else
            {
                packagePath = Path.GetFileName(fullPath);
            }
            return Path.Combine(targetPath ?? string.Empty, packagePath);
        }

        /// <summary>
        /// Returns true if the path uses a known folder root.
        /// </summary>
        private static bool IsKnownFolder(string targetPath)
        {
            if (targetPath != null)
            {
                var parts = targetPath.Split(
                    new char[] { '\\', '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                // exclude things in the root of the directory, this is not allowed
                // for any of the v3 folders.
                // example: an empty 'native' folder does not have a TxM and cannot be used.
                if (parts.Length > 1)
                {
                    var topLevelDirectory = parts.FirstOrDefault();

                    return PackagingConstants.Folders.Known.Any(folder =>
                        folder.Equals(topLevelDirectory, StringComparison.OrdinalIgnoreCase));
                }
            }

            return false;
        }

        private static void ExcludeFiles(List<PhysicalPackageFile> searchFiles, string basePath, string exclude)
        {
            if (string.IsNullOrEmpty(exclude))
            {
                return;
            }

            // One or more exclusions may be specified in the file. Split it and prepend the base path to the wildcard provided.
            var exclusions = exclude.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in exclusions)
            {
                string wildCard = PathResolver.NormalizeWildcardForExcludedFiles(basePath, item);
                PathResolver.FilterPackageFiles(searchFiles, p => p.SourcePath, new[] { wildCard });
            }
        }

        private void CreatePart(ZipArchive package, string path, Stream sourceStream, DateTimeOffset lastWriteTime, StringBuilder warningMessage)
        {
            if (PackageHelper.IsNuspec(path))
            {
                return;
            }

            string entryName = CreatePartEntryName(path);
            var entry = CreatePackageFileEntry(package, entryName, lastWriteTime, CompressionLevel.Optimal, warningMessage);

            using (var stream = entry.Open())
            {
                sourceStream.CopyTo(stream);
            }
        }

        internal static string CreatePartEntryName(string path)
        {
            // Only the segments between the path separators should be escaped
            var segments = path.Split(new[] { '/', '\\', Path.DirectorySeparatorChar }, StringSplitOptions.None)
                .Select(Uri.EscapeDataString);

            var escapedPath = string.Join("/", segments);

            // retrieve only relative path with resolved . or ..
            return GetStringForPartUri(escapedPath);
        }

        internal static string GetStringForPartUri(string escapedPath)
        {
            //Create an absolute URI to get the refinement on the relative path
            var partUri = new Uri(DefaultUri, escapedPath);

            // Get the safe-unescaped form of the URI first. This will unescape all the characters
            Uri safeUnescapedUri = new Uri(partUri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped), UriKind.Relative);

            return safeUnescapedUri.GetComponents(UriComponents.SerializationInfoString, UriFormat.Unescaped);
        }

        /// <summary>
        /// Tags come in this format. tag1 tag2 tag3 etc..
        /// </summary>
        private static IEnumerable<string> ParseTags(string tags)
        {
            Debug.Assert(tags != null);
            return from tag in tags.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                   select tag.Trim();
        }

        private void WriteOpcManifestRelationship(ZipArchive package, string path, string psmdcpPath)
        {
            ZipArchiveEntry relsEntry = CreateEntry(package, "_rels/.rels", CompressionLevel.Optimal);

            XNamespace relationships = "http://schemas.openxmlformats.org/package/2006/relationships";

            XDocument document = new XDocument(
                new XElement(relationships + "Relationships",
                    new XElement(relationships + "Relationship",
                        new XAttribute("Type", "http://schemas.microsoft.com/packaging/2010/07/manifest"),
                        new XAttribute("Target", $"/{path}"),
                        new XAttribute("Id", GenerateRelationshipId($"/{path}"))),
                    new XElement(relationships + "Relationship",
                        new XAttribute("Type", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties"),
                        new XAttribute("Target", $"/{psmdcpPath}"),
                        new XAttribute("Id", GenerateRelationshipId($"/{psmdcpPath}")))
                    )
                );

            using (var writer = new StreamWriter(relsEntry.Open()))
            {
                document.Save(writer);
                writer.Flush();
            }
        }

        private void WriteOpcContentTypes(ZipArchive package, HashSet<string> extensions, HashSet<string> filesWithoutExtensions)
        {
            // OPC backwards compatibility
            ZipArchiveEntry relsEntry = CreateEntry(package, "[Content_Types].xml", CompressionLevel.Optimal);

            XNamespace content = "http://schemas.openxmlformats.org/package/2006/content-types";
            XElement element = new XElement(content + "Types",
                new XElement(content + "Default",
                    new XAttribute("Extension", "rels"),
                    new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
                new XElement(content + "Default",
                    new XAttribute("Extension", "psmdcp"),
                    new XAttribute("ContentType", "application/vnd.openxmlformats-package.core-properties+xml"))
                    );
            foreach (var extension in extensions)
            {
                element.Add(
                    new XElement(content + "Default",
                        new XAttribute("Extension", extension),
                        new XAttribute("ContentType", "application/octet")
                        )
                    );
            }
            foreach (var file in filesWithoutExtensions)
            {
                element.Add(
                    new XElement(content + "Override",
                        new XAttribute("PartName", file),
                        new XAttribute("ContentType", "application/octet")
                        )
                    );
            }

            XDocument document = new XDocument(element);

            using (var writer = new StreamWriter(relsEntry.Open()))
            {
                document.Save(writer);
                writer.Flush();
            }
        }

        // OPC backwards compatibility for package properties
        private void WriteOpcPackageProperties(ZipArchive package, string psmdcpPath)
        {
            ZipArchiveEntry packageEntry = CreateEntry(package, psmdcpPath, CompressionLevel.Optimal);

            var dcText = "http://purl.org/dc/elements/1.1/";
            XNamespace dc = dcText;
            var dctermsText = "http://purl.org/dc/terms/";
            XNamespace dcterms = dctermsText;
            var xsiText = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xsi = xsiText;
            XNamespace core = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";

            XDocument document = new XDocument(
                new XElement(core + "coreProperties",
                    new XAttribute(XNamespace.Xmlns + "dc", dcText),
                    new XAttribute(XNamespace.Xmlns + "dcterms", dctermsText),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsiText),
                    new XElement(dc + "creator", string.Join(", ", Authors)),
                    new XElement(dc + "description", Description),
                    new XElement(dc + "identifier", Id),
                    new XElement(core + "version", Version.ToString()),
                    //new XElement(core + "language", Language),
                    new XElement(core + "keywords", ((IPackageMetadata)this).Tags),
                    //new XElement(dc + "title", Title),
                    new XElement(core + "lastModifiedBy", CreatorInfo())
                    )
                );


            using (var writer = new StreamWriter(packageEntry.Open()))
            {
                document.Save(writer);
                writer.Flush();
            }
        }

        // Generate a relationship id for compatibility
        private string GenerateRelationshipId(string path)
        {
            using (var hashFunc = new Sha512HashFunction())
            {
                var data = System.Text.Encoding.UTF8.GetBytes(path);
                hashFunc.Update(data, 0, data.Length);
                var hash = hashFunc.GetHashBytes();
                var hex = EncodeHexString(hash);
                return "R" + hex.Substring(0, 16);
            }
        }
    }
}
