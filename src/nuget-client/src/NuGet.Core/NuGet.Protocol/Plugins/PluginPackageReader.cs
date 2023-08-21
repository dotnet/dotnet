// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A package reader that delegates package read operations to a plugin.
    /// </summary>
    public sealed class PluginPackageReader : PackageReaderBase
    {
        private readonly ConcurrentDictionary<string, Lazy<Task<FileStreamCreator>>> _fileStreams;
        private IEnumerable<string> _files;
        private readonly SemaphoreSlim _getFilesSemaphore;
        private readonly SemaphoreSlim _getNuspecReaderSemaphore;
        private bool _isDisposed;
        private NuspecReader _nuspecReader;
        private readonly PackageIdentity _packageIdentity;
        private readonly string _packageSourceRepository;
        private readonly IPlugin _plugin;
        private readonly Lazy<string> _tempDirectoryPath;

        /// <summary>
        /// Initializes a new <see cref="PluginPackageReader" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="packageSourceRepository">A package source repository location.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageSourceRepository" />
        /// is either <c>null</c> or an empty string.</exception>
        public PluginPackageReader(IPlugin plugin, PackageIdentity packageIdentity, string packageSourceRepository)
            : base(DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            _plugin = plugin;
            _packageIdentity = packageIdentity;
            _packageSourceRepository = packageSourceRepository;
            _getFilesSemaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            _getNuspecReaderSemaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            _fileStreams = new ConcurrentDictionary<string, Lazy<Task<FileStreamCreator>>>(StringComparer.OrdinalIgnoreCase);
            _tempDirectoryPath = new Lazy<string>(GetTemporaryDirectoryPath);
        }

        /// <summary>
        /// Gets a stream for a file in the package.
        /// </summary>
        /// <param name="path">The file path in the package.</param>
        /// <returns>A stream.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override Stream GetStream(string path)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets a stream for a file in the package.
        /// </summary>
        /// <param name="path">The file path in the package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Stream" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="path" />
        /// is either <c>null</c> or an empty string.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<Stream> GetStreamAsync(string path, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(path));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var lazyCreator = _fileStreams.GetOrAdd(
                path,
                p => new Lazy<Task<FileStreamCreator>>(
                    () => GetStreamInternalAsync(p)));

            await lazyCreator.Value;

            if (lazyCreator.Value.Result == null)
            {
                return null;
            }

            return lazyCreator.Value.Result.Create();
        }

        /// <summary>
        /// Gets files in the package.
        /// </summary>
        /// <returns>An enumerable of files in the package.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<string> GetFiles()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets files in the package.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{String}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<string>> GetFilesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_files != null)
            {
                return _files;
            }

            await _getFilesSemaphore.WaitAsync(cancellationToken);

            try
            {
                if (_files != null)
                {
                    return _files;
                }

                _files = await GetFilesInternalAsync(cancellationToken);
            }
            finally
            {
                _getFilesSemaphore.Release();
            }

            return _files;
        }

        /// <summary>
        /// Gets files in the package.
        /// </summary>
        /// <param name="folder">A folder in the package.</param>
        /// <returns>An enumerable of files in the package.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<string> GetFiles(string folder)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets files in the package under the specified folder.
        /// </summary>
        /// <param name="folder">A folder in the package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{String}" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="folder" /> is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<string>> GetFilesAsync(
            string folder,
            CancellationToken cancellationToken)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            var files = await GetFilesAsync(cancellationToken);

            return files.Where(f => f.StartsWith(folder + "/", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Copies specified files in the package to the destination location.
        /// </summary>
        /// <param name="destination">A directory path to copy files to.</param>
        /// <param name="packageFiles">An enumerable of files in the package to copy.</param>
        /// <param name="extractFile">A package file extraction delegate.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>An enumerable of file paths in the destination location.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<string> CopyFiles(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken token)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously copies specified files in the package to the destination location.
        /// </summary>
        /// <param name="destination">A directory path to copy files to.</param>
        /// <param name="packageFiles">An enumerable of files in the package to copy.</param>
        /// <param name="extractFile">A package file extraction delegate.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{String}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="destination" />
        /// is either <c>null</c> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageFiles" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<string>> CopyFilesAsync(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(destination));
            }

            if (packageFiles == null)
            {
                throw new ArgumentNullException(nameof(packageFiles));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (!packageFiles.Any())
            {
                return Enumerable.Empty<string>();
            }

            // Normalized destination path 
            var normalizedDestination = NormalizeDirectoryPath(destination);

            ValidatePackageEntries(normalizedDestination, packageFiles, _packageIdentity);

            var packageId = _packageIdentity.Id;
            var packageVersion = _packageIdentity.Version.ToNormalizedString();
            var request = new CopyFilesInPackageRequest(
                _packageSourceRepository,
                packageId,
                packageVersion,
                packageFiles,
                destination);
            var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                MessageMethod.CopyFilesInPackage,
                request,
                cancellationToken);

            if (response != null)
            {
                switch (response.ResponseCode)
                {
                    case MessageResponseCode.Success:
                        return response.CopiedFiles;

                    case MessageResponseCode.Error:
                        throw new PluginException(
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.Plugin_FailedOperationForPackage,
                                _plugin.Name,
                                MessageMethod.CopyFilesInPackage,
                                packageId,
                                packageVersion));

                    case MessageResponseCode.NotFound:
                    // This class is only created if a success response code is received for a
                    // PrefetchPackageRequest, meaning that the plugin has already confirmed
                    // that the package exists.

                    default:
                        break;
                }
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the package identity.
        /// </summary>
        /// <returns>A package identity.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override PackageIdentity GetIdentity()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets the package identity.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="PackageIdentity" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<PackageIdentity> GetIdentityAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.GetIdentity();
        }

        /// <summary>
        /// Gets the minimum client version in the .nuspec.
        /// </summary>
        /// <returns>A NuGet version.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override NuGetVersion GetMinClientVersion()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets the minimum client version in the .nuspec.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="NuGetVersion" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<NuGetVersion> GetMinClientVersionAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.GetMinClientVersion();
        }

        /// <summary>
        /// Gets the package types.
        /// </summary>
        /// <returns>A read-only list of package types.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IReadOnlyList<PackageType> GetPackageTypes()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets the package types.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IReadOnlyList{PackageType}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IReadOnlyList<PackageType>> GetPackageTypesAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.GetPackageTypes();
        }

        /// <summary>
        /// Gets a stream for the .nuspec file.
        /// </summary>
        /// <returns>A stream.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override Stream GetNuspec()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets a stream for the .nuspec file.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Stream" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<Stream> GetNuspecAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecFile = await GetNuspecFileAsync(cancellationToken);

            return await GetStreamAsync(nuspecFile, cancellationToken);
        }

        /// <summary>
        /// Gets the .nuspec file path in the package.
        /// </summary>
        /// <returns>The .nuspec file path in the package.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override string GetNuspecFile()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets the .nuspec file path in the package.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="string" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<string> GetNuspecFileAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var files = await GetFilesAsync(cancellationToken);

            return GetNuspecFile(files);
        }

        /// <summary>
        /// Gets the .nuspec reader.
        /// </summary>
        public override NuspecReader NuspecReader => throw new NotSupportedException();

        /// <summary>
        /// Asynchronously gets the .nuspec reader.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="NuspecReader" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<NuspecReader> GetNuspecReaderAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_nuspecReader != null)
            {
                return _nuspecReader;
            }

            await _getNuspecReaderSemaphore.WaitAsync(cancellationToken);

            try
            {
                if (_nuspecReader != null)
                {
                    return _nuspecReader;
                }

                var stream = await GetNuspecAsync(cancellationToken);

                _nuspecReader = new NuspecReader(stream);
            }
            finally
            {
                _getNuspecReaderSemaphore.Release();
            }

            return _nuspecReader;
        }

        /// <summary>
        /// Gets supported frameworks.
        /// </summary>
        /// <returns>An enumerable of NuGet frameworks.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<NuGetFramework> GetSupportedFrameworks()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="PackageReaderBase.GetSupportedFrameworksAsync(CancellationToken)"/>
        public override async Task<IEnumerable<NuGetFramework>> GetSupportedFrameworksAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var frameworks = new HashSet<NuGetFramework>(NuGetFrameworkFullComparer.Instance);

            frameworks.UnionWith((await GetLibItemsAsync(cancellationToken)).Select(g => g.TargetFramework));

            frameworks.UnionWith((await GetBuildItemsAsync(cancellationToken)).Select(g => g.TargetFramework));

            frameworks.UnionWith((await GetContentItemsAsync(cancellationToken)).Select(g => g.TargetFramework));

            frameworks.UnionWith((await GetToolItemsAsync(cancellationToken)).Select(g => g.TargetFramework));

            frameworks.UnionWith((await GetFrameworkItemsAsync(cancellationToken)).Select(g => g.TargetFramework));

            return frameworks.Where(f => !f.IsUnsupported).OrderBy(f => f, NuGetFrameworkSorter.Instance);
        }

        /// <summary>
        /// Gets framework items.
        /// </summary>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetFrameworkItems()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets framework items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<FrameworkSpecificGroup>> GetFrameworkItemsAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.GetFrameworkAssemblyGroups();
        }

        /// <summary>
        /// Gets a flag indicating whether or not the package is serviceable.
        /// </summary>
        /// <returns>A flag indicating whether or not the package is serviceable.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override bool IsServiceable()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets a flag indicating whether or not the package is serviceable.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<bool> IsServiceableAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.IsServiceable();
        }

        /// <summary>
        /// Gets build items.
        /// </summary>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetBuildItems()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets build items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<FrameworkSpecificGroup>> GetBuildItemsAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);
            var id = nuspecReader.GetIdentity().Id;

            var results = new List<FrameworkSpecificGroup>();

            foreach (var group in await GetFileGroupsAsync(PackagingConstants.Folders.Build, cancellationToken))
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

        /// <summary>
        /// Gets tool items.
        /// </summary>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetToolItems()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets tool items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override Task<IEnumerable<FrameworkSpecificGroup>> GetToolItemsAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return GetFileGroupsAsync(PackagingConstants.Folders.Tools, cancellationToken);
        }

        /// <summary>
        /// Gets content items.
        /// </summary>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetContentItems()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets content items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override Task<IEnumerable<FrameworkSpecificGroup>> GetContentItemsAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return GetFileGroupsAsync(PackagingConstants.Folders.Content, cancellationToken);
        }

        /// <summary>
        /// Gets items in the specified folder in the package.
        /// </summary>
        /// <param name="folderName">A folder in the package.</param>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetItems(string folderName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets items in the specified folder in the package.
        /// </summary>
        /// <param name="folderName">A folder in the package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="folderName" /> is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override Task<IEnumerable<FrameworkSpecificGroup>> GetItemsAsync(
            string folderName,
            CancellationToken cancellationToken)
        {
            if (folderName == null)
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            cancellationToken.ThrowIfCancellationRequested();

            return GetFileGroupsAsync(folderName, cancellationToken);
        }

        /// <summary>
        /// Gets package dependencies.
        /// </summary>
        /// <returns>An enumerable of package dependency groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<PackageDependencyGroup> GetPackageDependencies()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets package dependencies.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{PackageDependencyGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<PackageDependencyGroup>> GetPackageDependenciesAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.GetDependencyGroups();
        }

        /// <summary>
        /// Gets lib items.
        /// </summary>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetLibItems()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets lib items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override Task<IEnumerable<FrameworkSpecificGroup>> GetLibItemsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return GetFileGroupsAsync(PackagingConstants.Folders.Lib, cancellationToken);
        }

        /// <summary>
        /// Gets reference items.
        /// </summary>
        /// <returns>An enumerable of framework specific groups.</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override IEnumerable<FrameworkSpecificGroup> GetReferenceItems()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets reference items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<IEnumerable<FrameworkSpecificGroup>> GetReferenceItemsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);
            var referenceGroups = nuspecReader.GetReferenceGroups();
            var fileGroups = new List<FrameworkSpecificGroup>();

            // filter out non reference assemblies
            foreach (var group in await GetLibItemsAsync(cancellationToken))
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

        /// <summary>
        /// Gets a flag indicating whether or not the package is a development dependency.
        /// </summary>
        /// <returns>A flag indicating whether or not the package is a development dependency</returns>
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public override bool GetDevelopmentDependency()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronously gets a flag indicating whether or not the package is a development dependency.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<bool> GetDevelopmentDependencyAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var nuspecReader = await GetNuspecReaderAsync(cancellationToken);

            return nuspecReader.GetDevelopmentDependency();
        }

        /// <summary>
        /// Asynchronously copies a package to the specified destination file path.
        /// </summary>
        /// <param name="nupkgFilePath">The destination file path.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="string" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nupkgFilePath" />
        /// is either <c>null</c> or an empty string.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<string> CopyNupkgAsync(
            string nupkgFilePath,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(nupkgFilePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(nupkgFilePath));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var packageId = _packageIdentity.Id;
            var packageVersion = _packageIdentity.Version.ToNormalizedString();
            var request = new CopyNupkgFileRequest(
                _packageSourceRepository,
                packageId,
                packageVersion,
                nupkgFilePath);
            var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                MessageMethod.CopyNupkgFile,
                request,
                cancellationToken);

            if (response != null)
            {
                switch (response.ResponseCode)
                {
                    case MessageResponseCode.Success:
                        return nupkgFilePath;

                    case MessageResponseCode.NotFound:
                        CreatePackageDownloadMarkerFile(nupkgFilePath);
                        break;

                    default:
                        break;
                }
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                foreach (var pair in _fileStreams)
                {
                    if (pair.Value.Value.Status == TaskStatus.RanToCompletion)
                    {
                        var fileStream = pair.Value.Value.Result;

                        if (fileStream != null)
                        {
                            fileStream.Dispose();
                        }
                    }
                }

                _fileStreams.Clear();

                _plugin.Dispose();

                _getFilesSemaphore.Dispose();
                _getNuspecReaderSemaphore.Dispose();

                if (_tempDirectoryPath.IsValueCreated)
                {
                    LocalResourceUtils.DeleteDirectoryTree(_tempDirectoryPath.Value, new List<string>());
                }

                _isDisposed = true;
            }
        }

        private async Task<IEnumerable<FrameworkSpecificGroup>> GetFileGroupsAsync(
            string folder,
            CancellationToken cancellationToken)
        {
            var groups = new Dictionary<NuGetFramework, List<string>>(NuGetFrameworkFullComparer.Instance);

            var isContentFolder = StringComparer.OrdinalIgnoreCase.Equals(folder, PackagingConstants.Folders.Content);
            var allowSubFolders = true;

            foreach (var path in await GetFilesAsync(folder, cancellationToken))
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
            return groups.Keys.OrderBy(e => e, NuGetFrameworkSorter.Instance)
                .Select(framework => new FrameworkSpecificGroup(framework, groups[framework].OrderBy(e => e, StringComparer.OrdinalIgnoreCase)));
        }

        private async Task<FileStreamCreator> GetStreamInternalAsync(
            string pathInPackage)
        {
            var packageId = _packageIdentity.Id;
            var packageVersion = _packageIdentity.Version.ToNormalizedString();

            var payload = new CopyFilesInPackageRequest(
                _packageSourceRepository,
                packageId,
                packageVersion,
                new[] { pathInPackage },
                _tempDirectoryPath.Value);

            var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                MessageMethod.CopyFilesInPackage,
                payload,
                CancellationToken.None);

            if (response != null)
            {
                switch (response.ResponseCode)
                {
                    case MessageResponseCode.Success:
                        return new FileStreamCreator(response.CopiedFiles.Single());

                    case MessageResponseCode.Error:
                        throw new PluginException(
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.Plugin_FailedOperationForPackage,
                                _plugin.Name,
                                MessageMethod.CopyFilesInPackage,
                                packageId,
                                packageVersion));

                    case MessageResponseCode.NotFound:
                    // This class is only created if a success response code is received for a
                    // PrefetchPackageRequest, meaning that the plugin has already confirmed
                    // that the package exists.

                    default:
                        break;
                }
            }

            return null;
        }

        private async Task<IEnumerable<string>> GetFilesInternalAsync(CancellationToken cancellationToken)
        {
            var packageId = _packageIdentity.Id;
            var packageVersion = _packageIdentity.Version.ToNormalizedString();
            var request = new GetFilesInPackageRequest(_packageSourceRepository, packageId, packageVersion);
            var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                MessageMethod.GetFilesInPackage,
                request,
                cancellationToken);

            if (response != null)
            {
                switch (response.ResponseCode)
                {
                    case MessageResponseCode.Success:
                        return response.Files;

                    case MessageResponseCode.Error:
                        throw new PluginException(
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.Plugin_FailedOperationForPackage,
                                _plugin.Name,
                                MessageMethod.GetFilesInPackage,
                                packageId,
                                packageVersion));

                    case MessageResponseCode.NotFound:
                    // This class is only created if a success response code is received for a
                    // PrefetchPackageRequest, meaning that the plugin has already confirmed
                    // that the package exists.

                    default:
                        break;
                }
            }

            return Enumerable.Empty<string>();
        }

        private void CreatePackageDownloadMarkerFile(string nupkgFilePath)
        {
            var directory = Path.GetDirectoryName(nupkgFilePath);
            var resolver = new VersionFolderPathResolver(directory);
            var fileName = resolver.GetPackageDownloadMarkerFileName(_packageIdentity.Id);
            var filePath = Path.Combine(directory, fileName);

            File.WriteAllText(filePath, string.Empty);
        }

        private static string GetTemporaryDirectoryPath()
        {
            var tempDirectoryPath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), Path.GetRandomFileName());

            Directory.CreateDirectory(tempDirectoryPath);

            return tempDirectoryPath;
        }

        public override Task<PrimarySignature> GetPrimarySignatureAsync(CancellationToken token)
        {
            return TaskResult.Null<PrimarySignature>();
        }

        public override Task<bool> IsSignedAsync(CancellationToken token)
        {
            return TaskResult.False;
        }

        public override Task ValidateIntegrityAsync(SignatureContent signatureContent, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public override Task<byte[]> GetArchiveHashAsync(HashAlgorithmName hashAlgorithm, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public override bool CanVerifySignedPackages(SignedPackageVerifierSettings verifierSettings)
        {
#if IS_SIGNING_SUPPORTED
            if (!verifierSettings.AllowUnsigned)
            {
                throw new SignatureException(NuGetLogCode.NU3041, Strings.Plugin_DownloadNotSupportedSinceUnsignedNotAllowed);
            }
#endif
            return false;
        }

        public override string GetContentHash(CancellationToken token, Func<string> GetUnsignedPackageHash = null)
        {
            // Plugin Download doesn't support signed packages so simply return null... and even then they aren't always packages.
            return null;
        }

        private sealed class FileStreamCreator : IDisposable
        {
            private readonly string _filePath;
            private bool _isDisposed;

            internal FileStreamCreator(string filePath)
            {
                _filePath = filePath;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    try
                    {
                        File.Delete(_filePath);
                    }
                    catch (Exception)
                    {
                    }

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal FileStream Create()
            {
                return new FileStream(
                     _filePath,
                     FileMode.Open,
                     FileAccess.Read,
                     FileShare.Read);
            }
        }
    }
}
