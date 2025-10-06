// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Resources;

namespace NuGet.Protocol.Providers
{
    /// <summary>NuGet.Protocol resource provider for <see cref="OwnerDetailsUriTemplateResourceV3"/> in V3 HTTP feeds.</summary>
    /// <remarks>When successful, returns an instance of <see cref="OwnerDetailsUriTemplateResourceV3"/>.</remarks>
    public class OwnerDetailsUriResourceV3Provider : ResourceProvider
    {
        public OwnerDetailsUriResourceV3Provider()
            : base(typeof(OwnerDetailsUriTemplateResourceV3),
                  nameof(OwnerDetailsUriTemplateResourceV3),
                  NuGetResourceProviderPositions.Last)
        {
        }

        /// <inheritdoc cref="ResourceProvider.TryCreate(SourceRepository, CancellationToken)"/>
        public override async Task<Tuple<bool, INuGetResource?>> TryCreate(SourceRepository source, CancellationToken token)
        {
            OwnerDetailsUriTemplateResourceV3? resource = null;
            ServiceIndexResourceV3? serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);
            if (serviceIndex != null)
            {
                Uri? uriTemplate = serviceIndex.GetServiceEntryUri(ServiceTypes.OwnerDetailsUriTemplate);

                if (uriTemplate != null)
                {
                    resource = OwnerDetailsUriTemplateResourceV3.CreateOrNull(uriTemplate);
                }
            }

            return new Tuple<bool, INuGetResource?>(resource != null, resource);
        }
    }
}
