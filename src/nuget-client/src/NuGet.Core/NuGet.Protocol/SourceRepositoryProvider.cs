// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Configuration;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// SourceRepositoryProvider is the high level source for repository objects representing package sources.
    /// </summary>
    public class SourceRepositoryProvider : ISourceRepositoryProvider
    {
        // TODO: add support for reloading sources when changes occur
        private readonly IPackageSourceProvider _packageSourceProvider;
        private IEnumerable<Lazy<INuGetResourceProvider>> _resourceProviders;
        private List<SourceRepository> _repositories;

        [Obsolete("https://github.com/NuGet/Home/issues/8479")]
        public SourceRepositoryProvider(ISettings settings, IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders)
            : this(new PackageSourceProvider(settings, enablePackageSourcesChangedEvent: true), resourceProviders)
        {
        }

        /// <summary>
        /// Non-MEF constructor
        /// </summary>
        public SourceRepositoryProvider(IPackageSourceProvider packageSourceProvider, IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders)
        {
            _packageSourceProvider = packageSourceProvider;
            _resourceProviders = resourceProviders;
            _repositories = new List<SourceRepository>();

            // Refresh the package sources
            Init();

            // Hook up event to refresh package sources when the package sources changed
            packageSourceProvider.PackageSourcesChanged += (sender, e) => { Init(); };
        }

        /// <summary>
        /// Retrieve repositories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SourceRepository> GetRepositories()
        {
            return _repositories;
        }

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        public SourceRepository CreateRepository(PackageSource source)
        {
            return CreateRepository(source, FeedType.Undefined);
        }

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        public SourceRepository CreateRepository(PackageSource source, FeedType type)
        {
            return new SourceRepository(source, _resourceProviders, type);
        }

        public IPackageSourceProvider PackageSourceProvider
        {
            get { return _packageSourceProvider; }
        }

        private void Init()
        {
            _repositories.Clear();
            foreach (var source in _packageSourceProvider.LoadPackageSources())
            {
                if (source.IsEnabled)
                {
                    var sourceRepo = new SourceRepository(source, _resourceProviders);
                    _repositories.Add(sourceRepo);
                }
            }
        }
    }
}
