// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Test;

namespace NuGet.Commands.Test
{
    public class TestRestoreRequest : RestoreRequest
    {
        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            ILogger log)
            : this(
                  project,
                  sources,
                  packagesDirectory,
                  new TestSourceCacheContext(),
                  log)
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            ClientPolicyContext clientPolicyContext,
            ILogger log)
            : this(
                  project,
                  sources.Select(Repository.Factory.GetCoreV3).ToList(),
                  packagesDirectory,
                  new List<string>(),
                  new TestSourceCacheContext(),
                  clientPolicyContext,
                  log)
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            SourceCacheContext cacheContext,
            ILogger log)
            : this(
                  project,
                  sources,
                  packagesDirectory,
                  new List<string>(),
                  cacheContext,
                  log)
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            SourceCacheContext cacheContext,
            ClientPolicyContext clientPolicyContext,
            ILogger log) : base(
                project,
                new RestoreCommandProvidersCache().GetOrCreate(
                    packagesDirectory,
                    fallbackPackagesPaths: new List<string>(),
                    sources: sources.Select(Repository.Factory.GetCoreV3).ToList(),
                    cacheContext: cacheContext,
                    log: log),
                cacheContext,
                clientPolicyContext,
                packageSourceMapping: PackageSourceMapping.GetPackageSourceMapping(NullSettings.Instance),
                log,
                new LockFileBuilderCache())
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            IEnumerable<string> fallbackPackageFolders,
            ILogger log)
            : this(
                  project,
                  sources,
                  packagesDirectory,
                  fallbackPackageFolders,
                  new TestSourceCacheContext(),
                  log)
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            IEnumerable<string> fallbackPackageFolders,
            SourceCacheContext cacheContext,
            ILogger log)
            : this(
                  project,
                  sources.Select(Repository.Factory.GetCoreV3).ToList(),
                  packagesDirectory,
                  fallbackPackageFolders,
                  cacheContext,
                  ClientPolicyContext.GetClientPolicy(NullSettings.Instance, log),
                  log)
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IReadOnlyList<SourceRepository> sources,
            string packagesDirectory,
            IEnumerable<string> fallbackPackageFolders,
            ILogger log) : this(
                project,
                sources,
                packagesDirectory,
                fallbackPackageFolders,
                new TestSourceCacheContext(),
                ClientPolicyContext.GetClientPolicy(NullSettings.Instance, log),
                log)
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IReadOnlyList<SourceRepository> sources,
            string packagesDirectory,
            IEnumerable<string> fallbackPackageFolders,
            SourceCacheContext cacheContext,
            ClientPolicyContext clientPolicyContext,
            ILogger log) : this(
            project,
            sources,
            packagesDirectory,
            fallbackPackageFolders,
            cacheContext,
            clientPolicyContext,
            log,
            new LockFileBuilderCache())
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IEnumerable<PackageSource> sources,
            string packagesDirectory,
            SourceCacheContext cacheContext,
            PackageSourceMapping packageSourceMappingConfiguration,
            ILogger log) : base(
                project,
                new RestoreCommandProvidersCache().GetOrCreate(
                    packagesDirectory,
                    Array.Empty<string>(),
                    sources: sources.Select(Repository.Factory.GetCoreV3).ToList(),
                    cacheContext: cacheContext,
                    log: log),
                cacheContext,
                ClientPolicyContext.GetClientPolicy(NullSettings.Instance, log),
                packageSourceMappingConfiguration,
                log,
                new LockFileBuilderCache())
        {
        }

        public TestRestoreRequest(
            PackageSpec project,
            IReadOnlyList<SourceRepository> sources,
            string packagesDirectory,
            IEnumerable<string> fallbackPackageFolders,
            SourceCacheContext cacheContext,
            ClientPolicyContext clientPolicyContext,
            ILogger log,
            LockFileBuilderCache lockFileBuilderCache) : base(
            project,
            new RestoreCommandProvidersCache().GetOrCreate(
                packagesDirectory,
                fallbackPackagesPaths: fallbackPackageFolders.ToList(),
                sources: sources,
                cacheContext: cacheContext,
                log: log),
            cacheContext,
            clientPolicyContext,
            packageSourceMapping: PackageSourceMapping.GetPackageSourceMapping(NullSettings.Instance),
            log,
            lockFileBuilderCache)
        {
            // We need the dependency graph spec to go through the proper no-op code paths
            DependencyGraphSpec = new DependencyGraphSpec();
            DependencyGraphSpec.AddProject(project);
            DependencyGraphSpec.AddRestore(project.RestoreMetadata.ProjectUniqueName);
            AllowNoOp = true;
        }
    }
}
