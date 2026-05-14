// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class CalculateAssemblyAndFileVersionsTests
    {
        [Theory]
        [InlineData("1.0.0", "20190102.3", "1.0.0.0", "1.0.19.5203")]
        [InlineData("2.1.800", "20190314.5", "2.1.800.0", "2.108.19.16405")]
        [InlineData("65535.654.9999", "20991231.99", "65535.654.9999.0", "65535.65499.9999.63199")]
        public void AutoGenerateAssemblyVersion_False(string prefix, string buildNumber, string expectedAssemblyVersion, string expectedFileVersion)
        {
            var task = new CalculateAssemblyAndFileVersions()
            {
                VersionPrefix = prefix,
                BuildNumber = buildNumber,
            };

            bool result = task.Execute();
            Assert.Equal(expectedAssemblyVersion, task.AssemblyVersion);
            Assert.Equal(expectedFileVersion, task.FileVersion);
            Assert.True(result);
        }

        [Theory]
        [InlineData("1.0.0", 12345, "1.0.0.12345")]
        [InlineData("2.1.2", 12345, "2.1.0.12345")]
        [InlineData("65535.65535.65535", int.MaxValue, "65535.65535.42949.33647")]
        public void AutoGenerateAssemblyVersion_True(string prefix, int patchNumber, string expectedVersion)
        {
            var task = new CalculateAssemblyAndFileVersions()
            {
                VersionPrefix = prefix,
                PatchNumber = patchNumber,
                AutoGenerateAssemblyVersion = true
            };

            bool result = task.Execute();
            Assert.Equal(expectedVersion, task.AssemblyVersion);
            Assert.Equal(expectedVersion, task.FileVersion);
            Assert.True(result);
        }
    }
}
