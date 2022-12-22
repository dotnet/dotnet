// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;

namespace NuGet.Commands.Test
{
    public static class CommandsTestUtility
    {
        /// <summary>
        /// Restore a DependencyGraphSpec
        /// </summary>
        public static async Task<RestoreSummary> RunSingleRestore(DependencyGraphSpec input, SimpleTestPathContext pathContext, ILogger logger)
        {
            var summaries = await RunRestore(input, pathContext, logger);
            return summaries.Single();
        }

        /// <summary>
        /// Restore a DependencyGraphSpec
        /// </summary>
        public static async Task<List<RestoreSummary>> RunRestore(DependencyGraphSpec input, SimpleTestPathContext pathContext, ILogger logger)
        {
            var providerCache = new RestoreCommandProvidersCache();
            var sources = new List<string>() { pathContext.PackageSource };

            using (var cacheContext = new SourceCacheContext())
            {
                var restoreContext = new RestoreArgs()
                {
                    CacheContext = cacheContext,
                    DisableParallel = false,
                    GlobalPackagesFolder = pathContext.UserPackagesFolder,
                    Sources = sources,
                    Log = logger,
                    CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources.Select(s => new PackageSource(s)))),
                    PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                    {
                        new DependencyGraphSpecRequestProvider(providerCache, input)
                    },
                    AllowNoOp = true
                };

                var summaries = await RestoreRunner.RunAsync(restoreContext);
                return summaries.ToList();
            }
        }
    }
}
