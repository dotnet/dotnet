// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// SourceRepositoryProvider is the high level source for repository objects representing package sources.
    /// </summary>
    [Export(typeof(ISourceRepositoryProvider))]
    public sealed class ExtensibleSourceRepositoryProvider : ISourceRepositoryProvider, IDisposable
    {

        // TODO: add support for reloading sources when changes occur
        private IPackageSourceProvider _packageSourceProvider;
        private IEnumerable<Lazy<INuGetResourceProvider>> _resourceProviders;
        private Lazy<List<SourceRepository>> _repositories;

        private readonly Lazy<ISettings> _settings;

        private bool _initialized;

        private object _lockObj = new object();

        /// <summary>
        /// Public parameter-less constructor for SourceRepositoryProvider
        /// </summary>
        public ExtensibleSourceRepositoryProvider()
        {
        }

        /// <summary>
        /// Public importing constructor for SourceRepositoryProvider
        /// </summary>
        [ImportingConstructor]
        public ExtensibleSourceRepositoryProvider(
            [ImportMany]
            IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders,
            [Import]
            Lazy<ISettings> settings)
        {
            _settings = settings;
            _resourceProviders = Repository.Provider.GetVisualStudio().Concat(resourceProviders);
        }

        private void EnsureInitialized()
        {
            LazyInitializer.EnsureInitialized(ref _packageSourceProvider,
                ref _initialized,
                ref _lockObj,
                () =>
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        IPackageSourceProvider packageSourceProvider = new PackageSourceProvider(_settings.Value, enablePackageSourcesChangedEvent: true);
#pragma warning restore CS0618 // Type or member is obsolete
                        packageSourceProvider.PackageSourcesChanged += ResetRepositories;
                        return packageSourceProvider;
                    });
        }

        /// <summary>
        /// Retrieve repositories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SourceRepository> GetRepositories()
        {
            EnsureInitialized();

            if (_repositories == null)
            {
                // initialize repositories from package source 
                ResetRepositories();
            }

            return _repositories.Value;
        }

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        public SourceRepository CreateRepository(PackageSource source)
        {
            EnsureInitialized();

            return CreateRepository(source, FeedType.Undefined);
        }

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        public SourceRepository CreateRepository(PackageSource source, FeedType type)
        {
            EnsureInitialized();

            return new SourceRepository(source, _resourceProviders, type);
        }

        public IPackageSourceProvider PackageSourceProvider
        {
            get
            {
                EnsureInitialized();

                return _packageSourceProvider;
            }
        }

        private void ResetRepositories()
        {
            // initialize it lazy since it doesn't impact RPS test performance and evaluate
            // only when somebody reads repositories value.
            _repositories = new Lazy<List<SourceRepository>>(GetRepositoriesCore);
        }

        private void ResetRepositories(object sender, EventArgs e)
        {
            ResetRepositories();
        }

        private List<SourceRepository> GetRepositoriesCore()
        {
            var repositories = new List<SourceRepository>();
            foreach (var source in _packageSourceProvider.LoadPackageSources())
            {
                if (source.IsEnabled)
                {
                    var sourceRepo = new SourceRepository(source, _resourceProviders);
                    repositories.Add(sourceRepo);
                }
            }

            return repositories;
        }

        public void Dispose()
        {
            if (_packageSourceProvider != null)
            {
                _packageSourceProvider.PackageSourcesChanged -= ResetRepositories;
            }
        }
    }
}
