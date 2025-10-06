// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace Test.Utility
{
    public class TestDependencyInfoProvider : ResourceProvider
    {
        public List<SourcePackageDependencyInfo> Packages { get; set; }

        public TestDependencyInfoProvider(List<SourcePackageDependencyInfo> packages)
            : base(typeof(DependencyInfoResource))
        {
            Packages = packages;
        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            var nuGetResource = new TestDependencyInfo(source, Packages);
            return Task.FromResult(new Tuple<bool, INuGetResource>(true, nuGetResource));
        }
    }

    /// <summary>
    /// Resolves against a local set of packages
    /// </summary>
    internal class TestDependencyInfo : DependencyInfoResource
    {
        public SourceRepository Source { get; set; }

        public List<SourcePackageDependencyInfo> Packages { get; set; }

        public TestDependencyInfo(SourceRepository source, List<SourcePackageDependencyInfo> packages)
        {
            Source = source;
            Packages = packages;
        }

        public override Task<SourcePackageDependencyInfo> ResolvePackage(PackageIdentity package, NuGetFramework projectFramework, SourceCacheContext sourceCacheContext, NuGet.Common.ILogger log, CancellationToken token)
        {
            var matchingPackage = Packages.FirstOrDefault(e => PackageIdentity.Comparer.Equals(e, package));
            return Task.FromResult<SourcePackageDependencyInfo>(ApplySource(matchingPackage));
        }

        public override Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(string packageId, NuGetFramework projectFramework, SourceCacheContext sourceCacheContext, NuGet.Common.ILogger log, CancellationToken token)
        {
            var results = new HashSet<SourcePackageDependencyInfo>(
                Packages.Where(e => StringComparer.OrdinalIgnoreCase.Equals(packageId, e.Id)),
                PackageIdentity.Comparer);

            return Task.FromResult<IEnumerable<SourcePackageDependencyInfo>>(results.Select(p => ApplySource(p)));
        }

        SourcePackageDependencyInfo ApplySource(SourcePackageDependencyInfo original)
        {
            if (original == null)
            {
                return null;
            }

            return new SourcePackageDependencyInfo(
                original.Id,
                original.Version,
                original.Dependencies,
                original.Listed,
                Source);
        }
    }
}
