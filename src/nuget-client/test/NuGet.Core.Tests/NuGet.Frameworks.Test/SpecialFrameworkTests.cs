// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class SpecialFrameworkTests
    {
        [Fact]
        public void SpecialFramework_Test()
        {
            var any = NuGetFramework.Parse("any");
            var agnostic = NuGetFramework.Parse("agnostic");
            var unsupported = NuGetFramework.Parse("unsupported");

            Assert.Equal(NuGetFramework.AnyFramework, any);
            Assert.Equal(NuGetFramework.AgnosticFramework, agnostic);
            Assert.Equal(NuGetFramework.UnsupportedFramework, unsupported);
        }

        [Fact]
        public void SpecialFramework_UnsupportedCompat()
        {
            var net45 = NuGetFramework.Parse("net45");
            var any = NuGetFramework.Parse("any");
            var agnostic = NuGetFramework.Parse("agnostic");
            var unsupported = NuGetFramework.Parse("unsupported");

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.True(compat.IsCompatible(any, agnostic));
            Assert.True(compat.IsCompatible(agnostic, any));

            Assert.True(compat.IsCompatible(any, unsupported));
            Assert.True(compat.IsCompatible(unsupported, any));

            Assert.False(compat.IsCompatible(unsupported, agnostic));
            Assert.False(compat.IsCompatible(agnostic, unsupported));

            Assert.True(compat.IsCompatible(net45, agnostic));
            Assert.False(compat.IsCompatible(agnostic, net45));

            Assert.True(compat.IsCompatible(net45, any));
            Assert.True(compat.IsCompatible(any, net45));

            Assert.False(compat.IsCompatible(net45, unsupported));
            Assert.False(compat.IsCompatible(unsupported, net45));
        }
    }
}
