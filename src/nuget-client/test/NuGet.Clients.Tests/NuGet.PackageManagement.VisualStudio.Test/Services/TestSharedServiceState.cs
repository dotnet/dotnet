// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    internal sealed class TestSharedServiceState : ISharedServiceState
    {
        private readonly AsyncLazy<NuGetPackageManager> _packageManager;

        public AsyncLazy<IVsSolutionManager> SolutionManager { get; }
        public ISourceRepositoryProvider SourceRepositoryProvider { get; }
        public AsyncLazy<IReadOnlyCollection<SourceRepository>> SourceRepositories { get; }

        internal TestSharedServiceState(
            AsyncLazy<NuGetPackageManager> packageManager,
            AsyncLazy<IVsSolutionManager> solutionManager,
            ISourceRepositoryProvider sourceRepositoryProvider,
            AsyncLazy<IReadOnlyCollection<SourceRepository>> sourceRepositories)
        {
            _packageManager = packageManager;
            SolutionManager = solutionManager;
            SourceRepositoryProvider = sourceRepositoryProvider;
            SourceRepositories = sourceRepositories;
        }

        public async ValueTask<NuGetPackageManager> GetPackageManagerAsync(CancellationToken cancellationToken)
        {
            return await _packageManager.GetValueAsync(cancellationToken);
        }

        public async ValueTask<IReadOnlyCollection<SourceRepository>> GetRepositoriesAsync(IReadOnlyCollection<PackageSourceContextInfo> packageSourceContextInfos, CancellationToken cancellationToken)
        {
            var sourceRepositories = await SourceRepositories.GetValueAsync();
            Dictionary<string, SourceRepository> sourceRepositoriesDictionary = sourceRepositories.ToDictionary(repository => repository.PackageSource.Name, _ => _);
            var matchingSourceRepositories = new List<SourceRepository>(capacity: packageSourceContextInfos.Count);

            foreach (PackageSourceContextInfo packageSource in packageSourceContextInfos)
            {
                if (sourceRepositoriesDictionary.TryGetValue(packageSource.Name, out SourceRepository sourceRepository))
                {
                    matchingSourceRepositories.Add(sourceRepository);
                }
            }

            return matchingSourceRepositories;
        }

        public void Dispose()
        {

        }
    }
}
