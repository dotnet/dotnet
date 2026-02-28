// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackagesLockFileTargetTests
    {
        [Theory]
        [InlineData("net472", null, null, ".NETFramework,Version=v4.7.2")]
        [InlineData("netstandard2.0", null, null, ".NETStandard,Version=v2.0")]
        [InlineData("netcoreapp3.1", null, null, ".NETCoreApp,Version=v3.1")]
        [InlineData("netcoreapp3.1", "win-x64", null, ".NETCoreApp,Version=v3.1/win-x64")]
        [InlineData("net5.0", null, null, ".NETCoreApp,Version=v5.0")]
        [InlineData("net5.0", "win-x64", null, ".NETCoreApp,Version=v5.0/win-x64")]
        [InlineData("net5.0-windows7.0", null, null, "net5.0-windows7.0")]
        [InlineData("net5.0-windows7.0", "win-x64", null, "net5.0-windows7.0/win-x64")]
        [InlineData("net6.0", null, null, "net6.0")]
        [InlineData("net6.0", "win-x64", null, "net6.0/win-x64")]
        [InlineData("net6.0-windows7.0", null, null, "net6.0-windows7.0")]
        [InlineData("net6.0-windows7.0", "win-x64", null, "net6.0-windows7.0/win-x64")]
        [InlineData("net10.0", null, "netcoreapp10.0", "netcoreapp10.0")]
        [InlineData("net10.0", "win-x64", "netcoreapp10.0", "netcoreapp10.0/win-x64")]
        [InlineData("net6.0", null, "net6.0", "net6.0")]
        public void Name_DifferentTargetFrameworkRuntimeIdentifiersAndAliases_HasExpectedValue(string targetFramework, string runtimeIdentifier, string alias, string expectedName)
        {
            // Arrange
            NuGetFramework framework = NuGetFramework.Parse(targetFramework);

            PackagesLockFileTarget packagesLockFileTarget = new()
            {
                TargetFramework = framework,
                RuntimeIdentifier = runtimeIdentifier,
                TargetAlias = alias
            };

            // Act
            var actualName = packagesLockFileTarget.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }
    }
}
