// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>NuGet.Protocol resource provider for <see cref="ReadmeUriTemplateResource"/>.</summary>
    /// <remarks>When successful, returns an instance of <see cref="ReadmeUriTemplateResource"/>.</remarks>
    internal class ReadmeUriTemplateResourceProvider : ResourceProvider
    {
        public ReadmeUriTemplateResourceProvider()
            : base(typeof(ReadmeUriTemplateResource),
                  nameof(ReadmeUriTemplateResource),
                  NuGetResourceProviderPositions.Last)
        {
        }

        /// <inheritdoc cref="ResourceProvider.TryCreate(SourceRepository, CancellationToken)"/>
        public override async Task<Tuple<bool, INuGetResource?>> TryCreate(SourceRepository source, CancellationToken token)
        {
            ReadmeUriTemplateResource? resource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);
            if (serviceIndex != null)
            {
                var uriTemplate = serviceIndex.GetServiceEntryUri(ServiceTypes.ReadmeFileUrl)?.OriginalString;

                // construct a new resource
                resource = string.IsNullOrWhiteSpace(uriTemplate) ? null : new ReadmeUriTemplateResource(uriTemplate);
            }

            return new Tuple<bool, INuGetResource?>(resource != null, resource);
        }
    }
}
