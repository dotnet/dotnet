// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class SharedServiceState : ISharedServiceState
    {
        private IReadOnlyCollection<SourceRepository>? _repositories;

        // Internal for testing purposes
        internal SharedServiceState(ISourceRepositoryProvider sourceRepositoryProvider)
        {
            SourceRepositoryProvider = sourceRepositoryProvider;
            SourceRepositoryProvider.PackageSourceProvider.PackageSourcesChanged += PackageSourcesChanged;

            SolutionManager = new AsyncLazy<IVsSolutionManager>(
                ServiceLocator.GetComponentModelServiceAsync<IVsSolutionManager>,
                NuGetUIThreadHelper.JoinableTaskFactory);

            SourceRepositories = new AsyncLazy<IReadOnlyCollection<SourceRepository>>(
                 GetSourceRepositoriesAsync,
                 NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public AsyncLazy<IReadOnlyCollection<SourceRepository>> SourceRepositories { get; private set; }
        public AsyncLazy<IVsSolutionManager> SolutionManager { get; }
        public ISourceRepositoryProvider SourceRepositoryProvider { get; }

        public static async ValueTask<SharedServiceState> CreateAsync(CancellationToken cancellationToken)
        {
            var sourceRepositoryProvider = await ServiceLocator.GetComponentModelServiceAsync<ISourceRepositoryProvider>();

            return new SharedServiceState(sourceRepositoryProvider);
        }

        public async ValueTask<NuGetPackageManager> GetPackageManagerAsync(CancellationToken cancellationToken)
        {
            IDeleteOnRestartManager deleteOnRestartManager = await ServiceLocator.GetComponentModelServiceAsync<IDeleteOnRestartManager>();
            ISettings settings = await ServiceLocator.GetComponentModelServiceAsync<ISettings>();
            IVsSolutionManager solutionManager = await SolutionManager.GetValueAsync(cancellationToken);
            IRestoreProgressReporter restoreProgressReporter = await ServiceLocator.GetComponentModelServiceAsync<IRestoreProgressReporter>();

            return new NuGetPackageManager(
                SourceRepositoryProvider,
                settings,
                solutionManager,
                deleteOnRestartManager,
                restoreProgressReporter);
        }

        public async ValueTask<IReadOnlyCollection<SourceRepository>> GetRepositoriesAsync(
            IReadOnlyCollection<PackageSourceContextInfo> packageSourceContextInfos,
            CancellationToken cancellationToken)
        {
            if (_repositories == null)
            {
                _repositories = await SourceRepositories.GetValueAsync();
            }

            Dictionary<string, SourceRepository>? sourceRepositories = _repositories.ToDictionary(repository => repository.PackageSource.Name, _ => _);
            var matchingSourceRepositories = new List<SourceRepository>(capacity: packageSourceContextInfos.Count);

            foreach (PackageSourceContextInfo packageSource in packageSourceContextInfos)
            {
                if (sourceRepositories.TryGetValue(packageSource.Name, out SourceRepository sourceRepository))
                {
                    matchingSourceRepositories.Add(sourceRepository);
                }
            }

            return matchingSourceRepositories;
        }

        public void Dispose()
        {
            SourceRepositoryProvider.PackageSourceProvider.PackageSourcesChanged -= PackageSourcesChanged;
        }

        private Task<IReadOnlyCollection<SourceRepository>> GetSourceRepositoriesAsync()
        {
            var packageSources = SourceRepositoryProvider.PackageSourceProvider.LoadPackageSources().ToList();
            _repositories = packageSources.Select(packageSource => SourceRepositoryProvider.CreateRepository(packageSource)).ToList();
            return Task.FromResult(_repositories);
        }

        private void PackageSourcesChanged(object sender, EventArgs e)
        {
            // if we have not lazily initialized yet we can skip the refresh
            if (!SourceRepositories.IsValueFactoryCompleted)
            {
                return;
            }

            _repositories = null;
            SourceRepositories = new AsyncLazy<IReadOnlyCollection<SourceRepository>>(
                GetSourceRepositoriesAsync,
                NuGetUIThreadHelper.JoinableTaskFactory);
        }
    }
}
