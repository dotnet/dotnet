// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class MultiSourceAutoCompleteProvider : IAutoCompleteProvider
    {
        private readonly IEnumerable<SourceRepository> _sourceRepositories;
        private readonly Common.ILogger _logger;

        public MultiSourceAutoCompleteProvider(
            IEnumerable<SourceRepository> sourceRepositories,
            Common.ILogger logger)
        {
            _sourceRepositories = sourceRepositories;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> IdStartsWithAsync(string packageIdPrefix, bool includePrerelease, CancellationToken cancellationToken)
        {
            var tasks = _sourceRepositories
                .Select(r => r.IdStartsWithAsync(packageIdPrefix, includePrerelease, cancellationToken))
                .ToList();

            var ignored = tasks
                .Select(task => task.ContinueWith(LogError, TaskContinuationOptions.OnlyOnFaulted))
                .ToArray();

            var completed = await Task.WhenAll(tasks);

            return completed.SelectMany(r => r).Distinct(StringComparer.OrdinalIgnoreCase);
        }

        public async Task<IEnumerable<NuGetVersion>> VersionStartsWithAsync(string packageId, string versionPrefix, bool includePrerelease, CancellationToken cancellationToken)
        {
            var tasks = _sourceRepositories
                .Select(r => r.VersionStartsWithAsync(packageId, versionPrefix, includePrerelease, cancellationToken))
                .ToList();

            var ignored = tasks
                .Select(task => task.ContinueWith(LogError, TaskContinuationOptions.OnlyOnFaulted))
                .ToArray();

            var completed = await Task.WhenAll(tasks);

            return completed.SelectMany(r => r).Distinct();
        }

        private void LogError(Task task)
        {
            foreach (var ex in task.Exception.Flatten().InnerExceptions)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
