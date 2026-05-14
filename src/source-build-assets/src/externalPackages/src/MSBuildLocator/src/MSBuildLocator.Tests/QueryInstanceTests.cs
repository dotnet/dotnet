// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shouldly;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Locator.Tests
{
    public class QueryInstancesTests
    {
        [Fact]
        public void DefaultInstanceTest()
        {
            VisualStudioInstance instance = MSBuildLocator.QueryVisualStudioInstances(VisualStudioInstanceQueryOptions.Default).FirstOrDefault();

            instance.ShouldNotBeNull();

#if NETCOREAPP
            instance.DiscoveryType.ShouldBe(DiscoveryType.DotNetSdk);
#else
            instance.DiscoveryType.ShouldNotBe(DiscoveryType.DotNetSdk);
#endif
        }
    }
}
