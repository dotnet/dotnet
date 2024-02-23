// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class HttpFileSystemBasedFindPackageByIdResourceProvider : ResourceProvider
    {
        public HttpFileSystemBasedFindPackageByIdResourceProvider()
            : base(typeof(FindPackageByIdResource),
                nameof(HttpFileSystemBasedFindPackageByIdResourceProvider),
                before: nameof(RemoteV3FindPackageByIdResourceProvider))
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository sourceRepository, CancellationToken token)
        {
            INuGetResource resource = null;
            var serviceIndexResource = await sourceRepository.GetResourceAsync<ServiceIndexResourceV3>(token);
            var packageBaseAddress = serviceIndexResource?.GetServiceEntryUris(ServiceTypes.PackageBaseAddress);

            if (packageBaseAddress != null
                && packageBaseAddress.Count > 0)
            {
                //Repository signature information init
                var repositorySignatureResource = await sourceRepository.GetResourceAsync<RepositorySignatureResource>(token);
                repositorySignatureResource?.UpdateRepositorySignatureInfo();

                var httpSourceResource = await sourceRepository.GetResourceAsync<HttpSourceResource>(token);

                resource = new HttpFileSystemBasedFindPackageByIdResource(
                    packageBaseAddress,
                    httpSourceResource.HttpSource);
            }

            return Tuple.Create(resource != null, resource);
        }
    }
}
