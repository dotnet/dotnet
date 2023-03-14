// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Protocol.Core.Types;

namespace ensure_nupkg_dependencies_on_source
{
    internal class NuGetFeed
    {
        private readonly SourceRepository _sourceRepository;
        private FindPackageByIdResource? _findPackageByIdResource;

        public NuGetFeed(SourceRepository sourceRepository)
        {
            _sourceRepository = sourceRepository;
        }

        public async ValueTask<FindPackageByIdResource> GetFindPackageByIdResourceAsync()
        {
            if (_findPackageByIdResource == null)
            {
                _findPackageByIdResource = await _sourceRepository.GetResourceAsync<FindPackageByIdResource>();
            }

            return _findPackageByIdResource;
        }
    }
}
