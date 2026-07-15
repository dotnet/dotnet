// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class InstallDotNetCoreTests
    {
        [Fact]
        public void BuildInstallArgumentsPreservesQuotedWindowsRootDotNetPath()
        {
            var task = new InstallDotNetCore
            {
                DotNetPath = "C:\\",
            };

            MethodInfo buildInstallArgumentsMethod = typeof(InstallDotNetCore).GetMethod("BuildInstallArguments", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(buildInstallArgumentsMethod);
            string arguments = Assert.IsType<string>(buildInstallArgumentsMethod.Invoke(task, ["aspnetcore", "1.2.3", "x64", true]));

            Assert.Contains("-dotnetPath \"C:\\\\\"", arguments);
        }

        [Fact]
        public void BuildInstallArgumentsOmitsRuntimeSourceValuesWhenRequested()
        {
            var task = new InstallDotNetCore
            {
                DotNetPath = "C:\\",
                RuntimeSourceFeed = "https://example.test/feed",
                RuntimeSourceFeedKey = "secret-key",
            };

            MethodInfo buildInstallArgumentsMethod = typeof(InstallDotNetCore).GetMethod("BuildInstallArguments", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(buildInstallArgumentsMethod);
            string arguments = Assert.IsType<string>(buildInstallArgumentsMethod.Invoke(task, ["aspnetcore", "1.2.3", "x64", false]));

            Assert.DoesNotContain(task.RuntimeSourceFeed, arguments);
            Assert.DoesNotContain(task.RuntimeSourceFeedKey, arguments);
            Assert.Contains("-runtime \"aspnetcore\"", arguments);
            Assert.Contains("-version \"1.2.3\"", arguments);
            Assert.Contains("-architecture x64", arguments);
        }
    }
}
