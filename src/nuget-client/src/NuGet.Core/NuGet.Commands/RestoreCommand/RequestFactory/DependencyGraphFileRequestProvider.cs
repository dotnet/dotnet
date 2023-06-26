// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    public class DependencyGraphFileRequestProvider : IRestoreRequestProvider
    {
        private readonly RestoreCommandProvidersCache _providerCache;

        public DependencyGraphFileRequestProvider(RestoreCommandProvidersCache providerCache)
        {
            _providerCache = providerCache;
        }

        public virtual Task<IReadOnlyList<RestoreSummaryRequest>> CreateRequests(
            string inputPath,
            RestoreArgs restoreContext)
        {
            var paths = new List<string>();
            var requests = new List<RestoreSummaryRequest>();

            var dgSpec = DependencyGraphSpec.Load(inputPath);
            var dgProvider = new DependencyGraphSpecRequestProvider(_providerCache, dgSpec);

            return dgProvider.CreateRequests(restoreContext);
        }

        public virtual Task<bool> Supports(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // True if .dg file
            var result = (File.Exists(path) && path.EndsWith(".dg", StringComparison.OrdinalIgnoreCase));

            return TaskResult.Boolean(result);
        }
    }
}
