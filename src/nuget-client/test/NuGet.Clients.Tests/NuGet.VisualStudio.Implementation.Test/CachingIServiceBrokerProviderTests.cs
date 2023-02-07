// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test
{
    [Collection(MockedVS.Collection)]
    public class CachingIServiceBrokerProviderTests
    {
        public CachingIServiceBrokerProviderTests(GlobalServiceProvider serviceProvider)
        {
            serviceProvider.Reset();
        }

        [Fact]
        public async Task GetAsync_Always_IsIdempotent()
        {
            var provider = new CachingIServiceBrokerProvider();

            IServiceBroker serviceBroker1 = await provider.GetAsync();
            IServiceBroker serviceBroker2 = await provider.GetAsync();

            Assert.Same(serviceBroker1, serviceBroker2);
        }
    }
}
