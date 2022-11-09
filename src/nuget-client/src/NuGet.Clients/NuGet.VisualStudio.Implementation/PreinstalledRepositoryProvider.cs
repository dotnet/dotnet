// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Implementation.Resources;

namespace NuGet.VisualStudio
{
    public sealed class PreinstalledRepositoryProvider : ISourceRepositoryProvider
    {
        public const string DefaultRegistryKeyRoot = @"SOFTWARE\NuGet\Repository";

        private readonly string _registryKeyRoot;
        private readonly List<SourceRepository> _repositories;
        private readonly Action<string> _errorHandler;
        private readonly ISourceRepositoryProvider _provider;

        // Cache sources for the life of this provider
        private readonly ConcurrentDictionary<Configuration.PackageSource, SourceRepository> _sourceCache
             = new ConcurrentDictionary<Configuration.PackageSource, SourceRepository>();

        public PreinstalledRepositoryProvider(
            Action<string> errorHandler,
            ISourceRepositoryProvider provider)
            : this(DefaultRegistryKeyRoot, errorHandler, provider)
        {
        }

        public PreinstalledRepositoryProvider(
            string registryKeyRoot,
            Action<string> errorHandler,
            ISourceRepositoryProvider provider)
        {
            if (registryKeyRoot == null)
            {
                throw new ArgumentNullException(nameof(registryKeyRoot));
            }

            if (errorHandler == null)
            {
                throw new ArgumentNullException(nameof(errorHandler));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _registryKeyRoot = registryKeyRoot;
            _repositories = new List<SourceRepository>();
            _errorHandler = errorHandler;
            _provider = provider;
        }

        public void AddFromRegistry(string keyName, bool isPreUnzipped)
        {
            var path = GetRegistryRepositoryPath(keyName);

            // Override the feed type as unzipped if specified, otherwise allow the source to determine what it is.
            var feedType = isPreUnzipped ? FeedType.FileSystemUnzipped : FeedType.Undefined;

            var source = CreateRepository(new Configuration.PackageSource(path), feedType);
            _repositories.Add(source);
        }

        public void AddFromExtension(ISourceRepositoryProvider provider, string extensionId)
        {
            var path = GetExtensionRepositoryPath(extensionId);

            var source = new Configuration.PackageSource(path);
            _repositories.Add(CreateRepository(source));
        }

        public void AddFromSource(SourceRepository repo)
        {
            _repositories.Add(repo);
        }

        public SourceRepository CreateRepository(Configuration.PackageSource source)
        {
            return CreateRepository(source, FeedType.Undefined);
        }

        public SourceRepository CreateRepository(Configuration.PackageSource source, FeedType type)
        {
            return _sourceCache.GetOrAdd(source, (packageSource) => _provider.CreateRepository(packageSource, type));
        }

        public IEnumerable<SourceRepository> GetRepositories()
        {
            return _repositories;
        }

        public Configuration.IPackageSourceProvider PackageSourceProvider
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return null;
            }
        }

        /// <summary>
        /// Gets the folder location where packages have been laid down for the specified extension.
        /// </summary>
        /// <param name="extensionId">The installed extension.</param>
        /// <returns>The absolute path to the extension's packages folder.</returns>
        private string GetExtensionRepositoryPath(string extensionId)
        {
            var extensionManagerShim = new ExtensionManagerShim(extensionManager: null, errorHandler: _errorHandler);
            string installPath;

            if (!extensionManagerShim.TryGetExtensionInstallPath(extensionId, out installPath))
            {
                var errorMessage = string.Format(CultureInfo.CurrentCulture, VsResources.PreinstalledPackages_InvalidExtensionId, extensionId);
                _errorHandler(errorMessage);

                // The error is fatal, cannot continue
                throw new InvalidOperationException(errorMessage);
            }

            return Path.Combine(installPath, "Packages");
        }

        /// <summary>
        /// Gets the folder location where packages have been laid down in a registry-specified location.
        /// </summary>
        /// <param name="keyName">The registry key name that specifies the packages location.</param>
        /// <returns>The absolute path to the packages folder specified in the registry.</returns>
        private string GetRegistryRepositoryPath(string keyName)
        {
            IRegistryKey repositoryKey = null;
            string repositoryValue = null;

            // When pulling the repository from the registry, use CurrentUser first, falling back onto LocalMachine
            // Documented here: https://docs.microsoft.com/nuget/visual-studio-extensibility/visual-studio-templates#registry-specified-folder-path
            var registryKeys = new[]
                               {
                                   new RegistryKeyWrapper(RegistryHive.CurrentUser),
                                   new RegistryKeyWrapper(RegistryHive.LocalMachine, RegistryView.Registry32)
                               };

            // Find the first registry key that supplies the necessary subkey/value
            foreach (var registryKey in registryKeys)
            {
                repositoryKey = registryKey.OpenSubKey(_registryKeyRoot);

                if (repositoryKey != null)
                {
                    repositoryValue = repositoryKey.GetValue(keyName) as string;

                    if (!string.IsNullOrEmpty(repositoryValue))
                    {
                        break;
                    }

                    repositoryKey.Close();
                }
            }

            if (repositoryKey == null)
            {
                var errorMessage = string.Format(CultureInfo.CurrentCulture, VsResources.PreinstalledPackages_RegistryKeyError, _registryKeyRoot);
                _errorHandler(errorMessage);

                // The error is fatal, cannot continue
                throw new InvalidOperationException(errorMessage);
            }

            if (string.IsNullOrEmpty(repositoryValue))
            {
                var errorMessage = string.Format(CultureInfo.CurrentCulture, VsResources.PreinstalledPackages_InvalidRegistryValue, keyName, _registryKeyRoot);
                _errorHandler(errorMessage);

                // The error is fatal, cannot continue
                throw new InvalidOperationException(errorMessage);
            }

            // Ensure a trailing slash so that the path always gets read as a directory
            repositoryValue = PathUtility.EnsureTrailingSlash(repositoryValue);

            return Path.GetDirectoryName(repositoryValue);
        }
    }
}
