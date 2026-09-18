// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Provides <see cref="PackageStagingResourceV3"/> instances for V3 package sources.
    /// </summary>
    public class PackageStagingResourceV3Provider : ResourceProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageStagingResourceV3Provider"/> class.
        /// </summary>
        public PackageStagingResourceV3Provider()
            : base(
                  typeof(PackageStagingResourceV3),
                  nameof(PackageStagingResourceV3Provider),
                  NuGetResourceProviderPositions.Last)
        {
        }

        /// <inheritdoc />
        public override async Task<Tuple<bool, INuGetResource?>> TryCreate(
            SourceRepository source,
            CancellationToken token)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            ServiceIndexResourceV3? serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);
            Uri? endpoint = serviceIndex?.GetServiceEntryUri(ServiceTypes.PackageStaging);

            if (endpoint is null)
            {
                return new Tuple<bool, INuGetResource?>(false, null);
            }

            HttpSourceResource httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(token)
                ?? throw new InvalidOperationException($"The source '{source.PackageSource.Source}' does not provide {nameof(HttpSourceResource)}.");

            return new Tuple<bool, INuGetResource?>(true, new PackageStagingResourceV3(endpoint, httpSourceResource.HttpSource));
        }
    }
}
