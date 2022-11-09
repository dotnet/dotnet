// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Wrapper class consolidating common queries against a collection of packages
    /// </summary>
    public sealed class PackageCollection : IEnumerable<PackageCollectionItem>
    {
        private readonly PackageCollectionItem[] _packages;
        private readonly ISet<string> _uniqueIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public PackageCollection(PackageCollectionItem[] packages)
        {
            _packages = packages;
            _uniqueIds.UnionWith(_packages.Select(p => p.Id));
        }

        public IEnumerator<PackageCollectionItem> GetEnumerator()
        {
            return ((IEnumerable<PackageCollectionItem>)_packages).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<PackageCollectionItem>)_packages).GetEnumerator();
        }

        public bool ContainsId(string packageId) => _uniqueIds.Contains(packageId);

        public static async Task<PackageCollection> FromProjectsAsync(
            IServiceBroker serviceBroker,
            IEnumerable<IProjectContextInfo> projects,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(projects);

            // Read package references from all projects.
            IEnumerable<Task<IReadOnlyCollection<IPackageReferenceContextInfo>>> tasks = projects
                .Select(project => project.GetInstalledPackagesAsync(serviceBroker, cancellationToken).AsTask());
            IEnumerable<IPackageReferenceContextInfo>[] packageReferences = await Task.WhenAll(tasks);

            return FromPackageReferences(packageReferences.SelectMany(e => e));
        }

        public static PackageCollection FromPackageReferences(IEnumerable<IPackageReferenceContextInfo> packageReferences)
        {
            // Group all package references for an id/version into a single item.
            PackageCollectionItem[]? packages = packageReferences
                .GroupBy(e => e.Identity, (key, group) => new PackageCollectionItem(key.Id, key.Version, group))
                .ToArray();

            return new PackageCollection(packages);
        }
    }
}
