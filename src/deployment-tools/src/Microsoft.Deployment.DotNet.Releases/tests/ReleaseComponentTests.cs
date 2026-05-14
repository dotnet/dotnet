// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ReleaseComponentTests : TestBase
    {
        [Theory]
        [InlineData("5.0", "5.0.0-preview.7")]
        [InlineData("3.1", "3.1.5")]
        [InlineData("3.0", "3.0.2")]
        [InlineData("2.2", "2.2.8")]
        [InlineData("2.1", "2.1.7")]
        [InlineData("2.0", "2.0.9")]
        [InlineData("1.1", "1.1.10")]
        [InlineData("1.0", "1.0.14")]
        public void ItDoesNotContainMarketingFiles(string productVersion, string releaseVersion)
        {
            var release = GetProductRelease(productVersion, releaseVersion);

            Assert.All(release.Files, f => Assert.True(!f.Name.Contains("-gs") && !f.Name.Contains("-nj")));
        }

        [Fact]
        public void ReleaseComponentNames()
        {
            var release = GetProductRelease("3.1", "3.1.5");

            var sdkComponent = release.Sdks.FirstOrDefault();
            var aspNetComponent = release.AspNetCoreRuntime;
            var runtimeComponent = release.Runtime;
            var desktopComponent = release.WindowsDesktopRuntime;

            Assert.Equal("SDK", sdkComponent.Name);
            Assert.Equal("ASP.NET Core Runtime", aspNetComponent.Name);
            Assert.Equal(".NET Core Runtime", runtimeComponent.Name);
            Assert.Equal("Desktop Runtime", desktopComponent.Name);
        }
    }
}
