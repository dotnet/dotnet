// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests;

public class SourceRepositoryTests
{
    [Fact]
    public void ProviderCacheTypes_EqualsV3ResourceCount()
    {
        Assert.Equal(SourceRepository.ProviderCacheTypes, Repository.Provider.GetCoreV3().GroupBy(p => p.Value.ResourceType).Count());
    }
}
