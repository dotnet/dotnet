// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public static class FeedTestUtils
    {
        internal static INuGetResourceProvider CreateTestResourceProvider(PackageMetadataResource _metadataResource)
        {
            var provider = Mock.Of<INuGetResourceProvider>();
            Mock.Get(provider)
                .Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(Tuple.Create(true, (INuGetResource)_metadataResource)));
            Mock.Get(provider)
                .Setup(x => x.ResourceType)
                .Returns(typeof(PackageMetadataResource));

            return provider;
        }

        internal static FeedSearchContinuationToken CreateInitialToken() => new()
        {
            SearchString = string.Empty,
            StartIndex = 0,
            SearchFilter = new SearchFilter(includePrerelease: false),
        };
    }
}
