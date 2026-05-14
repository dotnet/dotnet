// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Frameworks.Test
{
    public class NuGetFrameworkParseComponentsTests
    {
        [Theory]
        [InlineData(".NETFramework,Version=4.5,Profile=cf", null, "CompactFramework")]
        [InlineData(".NETFramework,Version=4.5,Profile=CF", null, "CompactFramework")]
        [InlineData(".NETFramework,Version=4.5,Profile=Full", null, "")]
        [InlineData(".NETFramework,Version=4.5", null, "")]
        [InlineData(".NETFramework,Version=4.5,Profile=WP71", null, "WindowsPhone71")]
        [InlineData(".NETFramework,Version=4.5,Profile=WP", null, "WindowsPhone")]
        public void NuGetFramework_ProfileName(string tfm, string tfp, string expected)
        {
            string actual = NuGetFramework.ParseComponents(tfm, tfp).Profile;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("net45", ".NETFramework,Version=4.5", null)]
        [InlineData("net10", ".NETFramework,Version=1.0", null)]
        [InlineData("net20", ".NETFramework,Version=2.0", null)]
        [InlineData("net40", ".NETFramework,Version=4.0", null)]
        [InlineData("net35", ".NETFramework,Version=3.5", null)]
        [InlineData("net40-client", ".NETFramework,Version=4.0,Profile=Client", null)]
        [InlineData("net5.0", ".NETCoreApp,Version=5.0", null)]
        [InlineData("net5.0", ".NETCoreApp,Version=v5.0", null)]
        [InlineData("net", ".NETFramework,Version=0.0", null)]
        [InlineData("net10.1.2.3", ".NETFramework,Version=10.1.2.3", null)]
        [InlineData("net10.0", ".NETFramework,Version=10.0", null)]
        [InlineData("net45-cf", ".NETFramework,Version=4.5,Profile=CompactFramework", null)]
        [InlineData("uap10.0", "UAP,Version=10.0", null)]
        [InlineData("dotnet", ".NETPlatform,Version=0.0", null)]
        [InlineData("dotnet", ".NETPlatform,Version=5.0", null)]
        [InlineData("netstandard", ".NETStandard", null)]
        [InlineData("netstandard1.0", ".NETStandard,Version=1.0", null)]
        [InlineData("netstandard1.0", ".NETStandard,Version=1.0.0", null)]
        [InlineData("netstandard0.9", ".NETStandard,Version=0.9", null)]
        [InlineData("netstandard1.1", ".NETStandard,Version=1.1", null)]
        [InlineData("netstandard1.2", ".NETStandard,Version=1.2", null)]
        [InlineData("netstandard1.3", ".NETStandard,Version=1.3", null)]
        [InlineData("netstandard1.4", ".NETStandard,Version=1.4", null)]
        [InlineData("netstandard1.5", ".NETStandard,Version=1.5", null)]
        [InlineData("netcoreapp", ".NETCoreApp", null)]
        [InlineData("netcoreapp1.0", ".NETCoreApp,Version=1.0", null)]
        [InlineData("netcoreapp1.5", ".NETCoreApp,Version=1.5", null)]
        [InlineData("netcoreapp2.0", ".NetCoreApp,Version=2.0", null)]
        [InlineData("netcoreapp3.0", ".NetCoreApp,Version=3.0", null)]
        [InlineData("net5.0-android", ".NETCoreApp,Version=5.0", "android")]
        [InlineData("net5.0-android", ".NETCoreApp,Version=5.0", "android,Version=0.0")]
        [InlineData("net5.0-ios14.0", ".NETCoreApp,Version=5.0", "ios,Version=14.0")]
        [InlineData("net5.0-macos10.0", ".NETCoreApp,Version=5.0", "macos,Version=10.0")]
        [InlineData("net5.0-watchos1.0", ".NETCoreApp,Version=5.0", "watchos,Version=1.0")]
        [InlineData("net5.0-tvos1.0", ".NETCoreApp,Version=5.0", "tvos,Version=1.0")]
        [InlineData("net5.0-windows10.0", ".NETCoreApp,Version=5.0", "windows,Version=10.0")]
        [InlineData("net5.0-macos10.15.2.3", ".NETCoreApp,Version=5.0", "macos,Version=10.15.2.3")]
        [InlineData("unsupported", "unsupported", null)]
        // Scenarios where certain properties are ignored.
        [InlineData("netcoreapp3.0", ".NETCoreApp,Version=3.0", "macos,Version=10.15.2.3")]
        [InlineData("netcoreapp3.0", ".NETCoreApp,Version=3.0", "macos")]
        [InlineData("netcoreapp3.1-client", ".NETCoreApp,Version=3.1,Profile=client", null)]
        [InlineData("netcoreapp3.0-client", ".NETCoreApp,Version=v3.0,Profile=client", "Windows,Version=7.0")]
        [InlineData("netcoreapp3.0", ".NETCoreApp,Version=v3.0", "Windows,Version=7.0")]
        public void NuGetFramework_ParseToShortName(string expected, string targetFrameworkMoniker, string targetPlatformMoniker)
        {
            // Arrange
            var framework = NuGetFramework.ParseComponents(targetFrameworkMoniker, targetPlatformMoniker);

            // Act
            var shortName = framework.GetShortFolderName();

            // Assert
            Assert.Equal(expected, shortName);
        }

        [Theory]
        // Net5.0 ERA
        [InlineData(".NETCoreApp,Version=v5.0", null, ".NETCoreApp,Version=v5.0")]
        [InlineData(".NETCoreApp,Version=v10.1.2.3", null, ".NETCoreApp,Version=v10.1.2.3")]
        [InlineData("netcoreapp,Version=5.0", null, ".NETCoreApp,Version=v5.0")]
        [InlineData("netcoreapp,Version=v5.0", null, ".NETCoreApp,Version=v5.0")]
        [InlineData(".NETCoreApp,Version=v5.0", "android", ".NETCoreApp,Version=v5.0")]
        [InlineData(".NETCoreApp,Version=5.0", "ios,Version=14.0", ".NETCoreApp,Version=v5.0")]

        // Pre-Net5.0 ERA
        [InlineData("net,Version=v10.1.2.3", null, ".NETFramework,Version=v10.1.2.3")]
        [InlineData("net,Version=v4.7.2", "ios,Version=14.0", ".NETFramework,Version=v4.7.2")]
        [InlineData(".NETCoreApp,Version=v3.0", "Windows,Version=7.0", ".NETCoreApp,Version=v3.0")]
        [InlineData(".NETFramework,Version=v4.5", null, ".NETFramework,Version=v4.5")]
        [InlineData(".NETFramework,Version=v2.0", null, ".NETFramework,Version=v2.0")]
        [InlineData(".NETFramework,Version=4.0", null, ".NETFramework,Version=v4.0")]
        [InlineData(".NETFramework,Version=3.5", null, ".NETFramework,Version=v3.5")]
        [InlineData(".NETFramework,Version=4.0,Profile=full", null, ".NETFramework,Version=v4.0")]
        [InlineData(".NETFramework,Version=4.0,Profile=client", null, ".NETFramework,Version=v4.0,Profile=Client")]
        [InlineData(".NETFramework", null, ".NETFramework,Version=v0.0")]
        [InlineData(".NETFramework,Version=4.5,Profile=cf", null, ".NETFramework,Version=v4.5,Profile=CompactFramework")]
        [InlineData("uap,Version=10.0", null, "UAP,Version=v10.0")]
        [InlineData("dotnet", null, ".NETPlatform,Version=v5.0")]
        [InlineData(".NETPlatform", null, ".NETPlatform,Version=v5.0")]
        [InlineData(".NETPlatform,Version=5", null, ".NETPlatform,Version=v5.0")]
        [InlineData(".NETPlatform,Version=5.0", null, ".NETPlatform,Version=v5.0")]
        [InlineData(".NETPlatform,Version=1.0", null, ".NETPlatform,Version=v1.0")]
        [InlineData(".NETPlatform,Version=10", null, ".NETPlatform,Version=v10.0")]
        [InlineData(".NETPlatform,Version=5.1", null, ".NETPlatform,Version=v5.1")]
        [InlineData(".NETPlatform,Version=5.2", null, ".NETPlatform,Version=v5.2")]
        [InlineData(".NETPlatform,Version=5.3", null, ".NETPlatform,Version=v5.3")]
        [InlineData(".NETPlatform,Version=5.4", null, ".NETPlatform,Version=v5.4")]
        [InlineData(".NETPlatform,Version=5.5", null, ".NETPlatform,Version=v5.5")]
        [InlineData("netstandard,Version=1.0", null, ".NETStandard,Version=v1.0")]
        [InlineData(".NETStandard,Version=1.0", null, ".NETStandard,Version=v1.0")]
        [InlineData(".NETStandard,Version=1.1", null, ".NETStandard,Version=v1.1")]
        [InlineData("netstandardapp", null, ".NETStandardApp,Version=v0.0")]
        [InlineData(".NETStandardApp", null, ".NETStandardApp,Version=v0.0")]
        [InlineData(".NETStandardApp,Version=0.0", null, ".NETStandardApp,Version=v0.0")]
        [InlineData(".NETStandardApp,Version=1", null, ".NETStandardApp,Version=v1.0")]
        [InlineData(".NETStandardApp,Version=1.5", null, ".NETStandardApp,Version=v1.5")]
        [InlineData(".NETStandardApp,Version=2", null, ".NETStandardApp,Version=v2.0")]
        [InlineData(".NETStandardApp,Version=2.1", null, ".NETStandardApp,Version=v2.1")]
        [InlineData("netcoreapp", null, ".NETCoreApp,Version=v0.0")]
        [InlineData(".NETCoreApp", null, ".NETCoreApp,Version=v0.0")]
        [InlineData(".NETCoreApp,Version=0.0", null, ".NETCoreApp,Version=v0.0")]
        [InlineData(".NETCoreApp,Version=1", null, ".NETCoreApp,Version=v1.0")]
        [InlineData(".NETCoreApp,Version=1.5", null, ".NETCoreApp,Version=v1.5")]
        [InlineData(".NETCoreApp,Version=2", null, ".NETCoreApp,Version=v2.0")]
        [InlineData(".NETCoreApp,Version=3", null, ".NETCoreApp,Version=v3.0")]
        [InlineData("unsupported", null, "Unsupported,Version=v0.0")]
        // Scenarios where certain properties are ignored.
        [InlineData(".NETCoreApp,Version=v3.0", "macos,Version=10.15.2.3", ".NETCoreApp,Version=v3.0")]
        [InlineData(".NETCoreApp,Version=v3.0", "macos", ".NETCoreApp,Version=v3.0")]
        [InlineData(".NETCoreApp,Version=v3.1", "10.15.2.3", ".NETCoreApp,Version=v3.1")]
        [InlineData(".NETCoreApp,Version=v3.1,Profile=client", "10.15.2.3", ".NETCoreApp,Version=v3.1,Profile=Client")]
        [InlineData(".NETCoreApp,Version=v3.1,Profile=client", null, ".NETCoreApp,Version=v3.1,Profile=Client")]
        [InlineData(".NETCoreApp,Version=v3.0,Profile=client", "Windows,Version=7.0", ".NETCoreApp,Version=v3.0,Profile=Client")]
        public void NuGetFramework_Basic(string targetFrameworkMoniker, string targetPlatformMoniker, string fullName)
        {
            string output = NuGetFramework.ParseComponents(targetFrameworkMoniker, targetPlatformMoniker).DotNetFrameworkName;

            Assert.Equal(fullName, output);
        }

        [Theory]
        [InlineData(".NETCoreApp,Version=vklmnfkjdfn5.0", null)]
        [InlineData(".NETCoreApp,Version=v5.0", "plat,Version=badversion")]
        public void NuGetFramework_WithInvalidProperties_Throws(string targetFrameworkMoniker, string targetPlatformMoniker)
        {
            Assert.ThrowsAny<Exception>(() => NuGetFramework.ParseComponents(targetFrameworkMoniker, targetPlatformMoniker));
        }
    }
}
