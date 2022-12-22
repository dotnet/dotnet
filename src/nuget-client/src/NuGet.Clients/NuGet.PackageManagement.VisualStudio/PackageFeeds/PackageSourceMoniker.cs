// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class PackageSourceMoniker : IEquatable<PackageSourceMoniker>
    {
        private readonly string _stringRepresentation;
        private readonly string _tooltip;

        public PackageSourceMoniker(string sourceName, IEnumerable<PackageSourceContextInfo> packageSources, uint priorityOrder)
        {
            SourceName = sourceName;

            if (packageSources == null)
            {
                throw new ArgumentNullException(nameof(packageSources));
            }
            if (!packageSources.Any())
            {
                throw new ArgumentException("List of sources cannot be empty", nameof(packageSources));
            }

            PackageSources = packageSources.ToArray();
            PackageSourceNames = PackageSources.Select(s => s.Name).ToList();

            _stringRepresentation = $"{SourceName}: [{string.Join("; ", PackageSourceNames)}]";
            _tooltip = PackageSources.Count() == 1
                ? GetTooltip(PackageSources.First())
                : string.Join("; ", PackageSourceNames);
            PriorityOrder = priorityOrder;
        }

        public IReadOnlyCollection<PackageSourceContextInfo> PackageSources { get; }

        public IReadOnlyList<string> PackageSourceNames { get; }

        public string SourceName { get; }

        public bool IsAggregateSource => PackageSources.Count > 1;

        public uint PriorityOrder { get; }

        public override string ToString() => _stringRepresentation;

        public string GetTooltip()
        {
            return _tooltip;
        }

        private static string GetTooltip(PackageSourceContextInfo packageSource)
        {
            return string.IsNullOrEmpty(packageSource.Description)
                ? $"{packageSource.Name} - {packageSource.Source}"
                : $"{packageSource.Name} - {packageSource.Description} - {packageSource.Source}";
        }

        public bool Equals(PackageSourceMoniker other) => StringComparer.OrdinalIgnoreCase.Equals(ToString(), other.ToString());

        public override bool Equals(object obj)
        {
            return obj is PackageSourceMoniker && this == (PackageSourceMoniker)obj;
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public static bool operator ==(PackageSourceMoniker x, PackageSourceMoniker y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(PackageSourceMoniker x, PackageSourceMoniker y) => !(x == y);

        public static async ValueTask<IReadOnlyCollection<PackageSourceMoniker>> PopulateListAsync(
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(serviceBroker);

            using (INuGetSourcesService nugetSourcesService = await serviceBroker.GetProxyAsync<INuGetSourcesService>(
                NuGetServices.SourceProviderService,
                cancellationToken))
            {
                Assumes.NotNull(nugetSourcesService);
                IReadOnlyList<PackageSourceContextInfo> packageSources = await nugetSourcesService.GetPackageSourcesAsync(cancellationToken);

                return await PopulateListAsync(packageSources, cancellationToken);
            }
        }

        public static ValueTask<IReadOnlyCollection<PackageSourceMoniker>> PopulateListAsync(IReadOnlyCollection<PackageSourceContextInfo> packageSources, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<PackageSourceContextInfo> enabledSources = packageSources
                .Where(source => source.IsEnabled)
                .ToList();

            var packageSourceMonikers = new List<PackageSourceMoniker>();
            if (enabledSources.Count > 1) // If more than 1, add 'All'
            {
                packageSourceMonikers.Add(new PackageSourceMoniker(Strings.AggregateSourceName, enabledSources, priorityOrder: 0));
            }

            packageSourceMonikers.AddRange(enabledSources.Select(s => new PackageSourceMoniker(s.Name, new[] { s }, priorityOrder: 1)));

            return new ValueTask<IReadOnlyCollection<PackageSourceMoniker>>(packageSourceMonikers);
        }
    }
}
