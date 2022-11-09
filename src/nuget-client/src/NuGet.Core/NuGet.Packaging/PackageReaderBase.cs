// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    /// <summary>
    /// Abstract class that both the zip and folder package readers extend
    /// This class contains the path conventions for both zip and folder readers
    /// </summary>
    public abstract class PackageReaderBase : IPackageCoreReader, IPackageContentReader, IAsyncPackageCoreReader, IAsyncPackageContentReader, ISignedPackageReader
    {
        private NuspecReader _nuspecReader;

        protected IFrameworkNameProvider FrameworkProvider { get; set; }
        protected IFrameworkCompatibilityProvider CompatibilityProvider { get; set; }

        /// <summary>
        /// Instantiates a new <see cref="PackageReaderBase" /> class.
        /// </summary>
        /// <param name="frameworkProvider">A framework mapping provider.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="frameworkProvider" /> is <c>null</c>.</exception>
        public PackageReaderBase(IFrameworkNameProvider frameworkProvider)
            : this(frameworkProvider, new CompatibilityProvider(frameworkProvider))
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="PackageReaderBase" /> class.
        /// </summary>
        /// <param name="frameworkProvider">A framework mapping provider.</param>
        /// <param name="compatibilityProvider">A framework compatibility provider.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="frameworkProvider" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="compatibilityProvider" /> is <c>null</c>.</exception>
        public PackageReaderBase(IFrameworkNameProvider frameworkProvider, IFrameworkCompatibilityProvider compatibilityProvider)
        {
            if (frameworkProvider == null)
            {
                throw new ArgumentNullException(nameof(frameworkProvider));
            }

            if (compatibilityProvider == null)
            {
                throw new ArgumentNullException(nameof(compatibilityProvider));
            }

            FrameworkProvider = frameworkProvider;
            CompatibilityProvider = compatibilityProvider;
        }

        #region IPackageCoreReader implementation

        public abstract Stream GetStream(string path);

        public abstract IEnumerable<string> GetFiles();

        public abstract IEnumerable<string> GetFiles(string folder);

        public abstract IEnumerable<string> CopyFiles(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken token);

        public virtual PackageIdentity GetIdentity()
        {
            return NuspecReader.GetIdentity();
        }

        public virtual NuGetVersion GetMinClientVersion()
        {
            return NuspecReader.GetMinClientVersion();
        }

        public virtual IReadOnlyList<PackageType> GetPackageTypes()
        {
            return NuspecReader.GetPackageTypes();
        }

        public virtual Stream GetNuspec()
        {
            // This is the default implementation. It is overridden and optimized in
            // PackageArchiveReader and PackageFolderReader.
            return GetStream(GetNuspecFile());
        }

        public virtual string GetNuspecFile()
        {
            var files = GetFiles();

            return GetNuspecFile(files);
        }

        /// <summary>
        /// Nuspec reader
        /// </summary>
        public virtual NuspecReader NuspecReader
        {
            get
            {
                if (_nuspecReader == null)
                {
                    _nuspecReader = new NuspecReader(GetNuspec());
                }

                return _nuspecReader;
            }
        }

        #endregion

        #region IAsyncPackageCoreReader implementation

        public virtual Task<PackageIdentity> GetIdentityAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetIdentity());
        }

        public virtual Task<NuGetVersion> GetMinClientVersionAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetMinClientVersion());
        }

        public virtual Task<IReadOnlyList<PackageType>> GetPackageTypesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetPackageTypes());
        }

        public virtual Task<Stream> GetStreamAsync(string path, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetStream(path));
        }

        public virtual Task<IEnumerable<string>> GetFilesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetFiles());
        }

        public virtual Task<IEnumerable<string>> GetFilesAsync(string folder, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetFiles(folder));
        }

        public virtual Task<Stream> GetNuspecAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetNuspec());
        }

        public virtual Task<string> GetNuspecFileAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetNuspecFile());
        }

        public virtual Task<IEnumerable<string>> CopyFilesAsync(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(CopyFiles(destination, packageFiles, extractFile, logger, cancellationToken));
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        #endregion

        #region IPackageContentReader implementation

        public virtual IEnumerable<FrameworkSpecificGroup> GetFrameworkItems()
        {
            return NuspecReader.GetFrameworkAssemblyGroups();
        }

        public virtual IEnumerable<FrameworkSpecificGroup> GetBuildItems()
        {
            var id = GetIdentity().Id;

            var results = new List<FrameworkSpecificGroup>();

            foreach (var group in GetFileGroups(PackagingConstants.Folders.Build))
            {
                var filteredGroup = group;

                if (group.Items.Any(e => !IsAllowedBuildFile(id, e)))
                {
                    // create a new group with only valid files
                    filteredGroup = new FrameworkSpecificGroup(group.TargetFramework, group.Items.Where(e => IsAllowedBuildFile(id, e)));

                    if (!filteredGroup.Items.Any())
                    {
                        // nothing was useful in the folder, skip this group completely
                        filteredGroup = null;
                    }
                }

                if (filteredGroup != null)
                {
                    results.Add(filteredGroup);
                }
            }

            return results;
        }

        public virtual IEnumerable<FrameworkSpecificGroup> GetToolItems()
        {
            return GetFileGroups(PackagingConstants.Folders.Tools);
        }

        public virtual IEnumerable<FrameworkSpecificGroup> GetContentItems()
        {
            return GetFileGroups(PackagingConstants.Folders.Content);
        }

        public virtual IEnumerable<PackageDependencyGroup> GetPackageDependencies()
        {
            return NuspecReader.GetDependencyGroups();
        }

        public virtual IEnumerable<FrameworkSpecificGroup> GetLibItems()
        {
            return GetFileGroups(PackagingConstants.Folders.Lib);
        }

        public virtual IEnumerable<FrameworkSpecificGroup> GetReferenceItems()
        {
            var referenceGroups = NuspecReader.GetReferenceGroups();
            var fileGroups = new List<FrameworkSpecificGroup>();

            // filter out non reference assemblies
            foreach (var group in GetLibItems())
            {
                fileGroups.Add(new FrameworkSpecificGroup(group.TargetFramework, group.Items.Where(e => IsReferenceAssembly(e))));
            }

            // results
            var libItems = new List<FrameworkSpecificGroup>();

            if (referenceGroups.Any())
            {
                // the 'any' group from references, for pre2.5 nuspecs this will be the only group
                var fallbackGroup = referenceGroups.Where(g => g.TargetFramework.Equals(NuGetFramework.AnyFramework)).FirstOrDefault();

                foreach (var fileGroup in fileGroups)
                {
                    // check for a matching reference group to use for filtering
                    var referenceGroup = NuGetFrameworkUtility.GetNearest(
                        items: referenceGroups,
                        framework: fileGroup.TargetFramework,
                        frameworkMappings: FrameworkProvider,
                        compatibilityProvider: CompatibilityProvider);

                    if (referenceGroup == null)
                    {
                        referenceGroup = fallbackGroup;
                    }

                    if (referenceGroup == null)
                    {
                        // add the lib items without any filtering
                        libItems.Add(fileGroup);
                    }
                    else
                    {
                        var filteredItems = new List<string>();

                        foreach (var path in fileGroup.Items)
                        {
                            // reference groups only have the file name, not the path
                            var file = Path.GetFileName(path);

                            if (referenceGroup.Items.Any(s => StringComparer.OrdinalIgnoreCase.Equals(s, file)))
                            {
                                filteredItems.Add(path);
                            }
                        }

                        if (filteredItems.Any())
                        {
                            libItems.Add(new FrameworkSpecificGroup(fileGroup.TargetFramework, filteredItems));
                        }
                    }
                }
            }
            else
            {
                libItems.AddRange(fileGroups);
            }

            return libItems;
        }

        #endregion

        #region IAsyncPackageContentReader

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetFrameworkItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetFrameworkItems());
        }

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetBuildItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetBuildItems());
        }

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetToolItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetToolItems());
        }

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetContentItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetContentItems());
        }

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetLibItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetLibItems());
        }

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetReferenceItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetReferenceItems());
        }

        public virtual Task<IEnumerable<PackageDependencyGroup>> GetPackageDependenciesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetPackageDependencies());
        }

        #endregion

        /// <summary>
        /// Frameworks mentioned in the package.
        /// </summary>
        /// <remarks>
        /// This method returns the target frameworks supported for packages.config projects.
        /// For PackageReference compatibility, use <see cref="NuGet.Client.ManagedCodeConventions"/>
        /// </remarks>
        public virtual IEnumerable<NuGetFramework> GetSupportedFrameworks()
        {
            var frameworks = new HashSet<NuGetFramework>(new NuGetFrameworkFullComparer());

            frameworks.UnionWith(GetLibItems().Select(g => g.TargetFramework));

            frameworks.UnionWith(GetBuildItems().Select(g => g.TargetFramework));

            frameworks.UnionWith(GetContentItems().Select(g => g.TargetFramework));

            frameworks.UnionWith(GetToolItems().Select(g => g.TargetFramework));

            frameworks.UnionWith(GetFrameworkItems().Select(g => g.TargetFramework));

            return frameworks.Where(f => !f.IsUnsupported).OrderBy(f => f, new NuGetFrameworkSorter());
        }

        /// <summary>
        /// Frameworks mentioned in the package.
        /// </summary>
        /// <remarks>
        /// This method returns the target frameworks supported for packages.config projects.
        /// For PackageReference compatibility, use <see cref="NuGet.Client.ManagedCodeConventions"/>
        /// </remarks>
        public virtual Task<IEnumerable<NuGetFramework>> GetSupportedFrameworksAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetSupportedFrameworks());
        }

        public virtual bool IsServiceable()
        {
            return NuspecReader.IsServiceable();
        }

        public virtual Task<bool> IsServiceableAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(IsServiceable());
        }

        public virtual IEnumerable<FrameworkSpecificGroup> GetItems(string folderName)
        {
            return GetFileGroups(folderName);
        }

        public virtual Task<IEnumerable<FrameworkSpecificGroup>> GetItemsAsync(string folderName, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetItems(folderName));
        }

        public virtual bool GetDevelopmentDependency()
        {
            return NuspecReader.GetDevelopmentDependency();
        }

        public virtual Task<bool> GetDevelopmentDependencyAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetDevelopmentDependency());
        }

        protected IEnumerable<FrameworkSpecificGroup> GetFileGroups(string folder)
        {
            var groups = new Dictionary<NuGetFramework, List<string>>(new NuGetFrameworkFullComparer());
            var allowSubFolders = true;

            foreach (var path in GetFiles(folder))
            {
                // Use the known framework or if the folder did not parse, use the Any framework and consider it a sub folder
                var framework = GetFrameworkFromPath(path, allowSubFolders);

                List<string> items = null;
                if (!groups.TryGetValue(framework, out items))
                {
                    items = new List<string>();
                    groups.Add(framework, items);
                }

                items.Add(path);
            }

            // Sort the groups by framework, and the items by ordinal string compare to keep things deterministic
            foreach (var framework in groups.Keys.OrderBy(e => e, new NuGetFrameworkSorter()))
            {
                yield return new FrameworkSpecificGroup(framework, groups[framework].OrderBy(e => e, StringComparer.OrdinalIgnoreCase));
            }
        }

        protected NuGetFramework GetFrameworkFromPath(string path, bool allowSubFolders = false)
        {
            var framework = NuGetFramework.AnyFramework;

            var parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // ignore paths that are too short, and ones that have additional sub directories
            if (parts.Length == 3
                || (parts.Length > 3 && allowSubFolders))
            {
                var folderName = parts[1];

                NuGetFramework parsedFramework;
                try
                {
                    parsedFramework = NuGetFramework.ParseFolder(folderName, FrameworkProvider);
                }
                catch (ArgumentException e)
                {
                    // Include package name context in the exception.
                    throw new PackagingException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.InvalidPackageFrameworkFolderName,
                            path,
                            GetIdentity()),
                        e);
                }

                if (parsedFramework.IsSpecificFramework)
                {
                    // the folder name is a known target framework
                    framework = parsedFramework;
                }
            }

            return framework;
        }

        /// <summary>
        /// only packageId.targets and packageId.props should be used from the build folder
        /// </summary>
        protected static bool IsAllowedBuildFile(string packageId, string path)
        {
            var file = Path.GetFileName(path);

            return StringComparer.OrdinalIgnoreCase.Equals(file, string.Format(CultureInfo.InvariantCulture, "{0}.targets", packageId))
                   || StringComparer.OrdinalIgnoreCase.Equals(file, string.Format(CultureInfo.InvariantCulture, "{0}.props", packageId));
        }

        /// <summary>
        /// True only for assemblies that should be added as references to msbuild projects
        /// </summary>
        protected static bool IsReferenceAssembly(string path)
        {
            var result = false;

            var extension = Path.GetExtension(path);

            if (StringComparer.OrdinalIgnoreCase.Equals(extension, ".dll"))
            {
                if (!path.EndsWith(".resources.dll", StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                }
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(extension, ".winmd"))
            {
                result = true;
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(extension, ".exe"))
            {
                result = true;
            }

            return result;
        }

        protected static string GetNuspecFile(IEnumerable<string> files)
        {
            // Find all nuspecs in the root folder.
            var nuspecPaths = files
                .Where(entryPath => PackageHelper.IsManifest(entryPath))
                .ToList();

            if (nuspecPaths.Count == 0)
            {
                throw new PackagingException(NuGetLogCode.NU5037, Strings.Error_MissingNuspecFile);
            }
            else if (nuspecPaths.Count > 1)
            {
                throw new PackagingException(Strings.MultipleNuspecFiles);
            }

            return nuspecPaths.Single();
        }

        /// <summary>
        /// Validate file entry in package is not traversed outside of the expected extraction path.
        /// Eg: file entry like ../../foo.dll can get outside of the expected extraction path.
        /// </summary>
        protected static void ValidatePackageEntry(string normalizedDestination, string normalizedFilePath, PackageIdentity packageIdentity)
        {
            // Destination and filePath must be normalized.
            var fullPath = Path.GetFullPath(Path.Combine(normalizedDestination, normalizedFilePath));

            if (!fullPath.StartsWith(normalizedDestination, PathUtility.GetStringComparisonBasedOnOS()) || fullPath.Length == normalizedDestination.Length)
            {
                throw new UnsafePackageEntryException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorUnsafePackageEntry,
                    packageIdentity,
                    normalizedFilePath));
            }
        }

        protected string NormalizeDirectoryPath(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
            {
                path += Path.DirectorySeparatorChar;
            }

            return Path.GetFullPath(path);
        }

        protected static void ValidatePackageEntries(string normalizedDestination, IEnumerable<string> packageFiles, PackageIdentity packageIdentity)
        {
            // Check all package entries.
            packageFiles.ForEach(p =>
            {
                var normalizedPath = Uri.UnescapeDataString(p.Replace('/', Path.DirectorySeparatorChar));
                ValidatePackageEntry(normalizedDestination, normalizedPath, packageIdentity);
            });
        }

        public virtual Task<NuspecReader> GetNuspecReaderAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(NuspecReader);
        }

        public virtual Task<string> CopyNupkgAsync(string nupkgFilePath, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public abstract Task<PrimarySignature> GetPrimarySignatureAsync(CancellationToken token);

        public abstract Task<bool> IsSignedAsync(CancellationToken token);

        public abstract Task ValidateIntegrityAsync(SignatureContent signatureContent, CancellationToken token);

        public abstract Task<byte[]> GetArchiveHashAsync(HashAlgorithmName hashAlgorithm, CancellationToken token);

        public abstract bool CanVerifySignedPackages(SignedPackageVerifierSettings verifierSettings);

        /// <summary>
        /// Get contenthash for a package.
        /// </summary>
        public abstract string GetContentHash(CancellationToken token, Func<string> GetUnsignedPackageHash = null);
    }
}
