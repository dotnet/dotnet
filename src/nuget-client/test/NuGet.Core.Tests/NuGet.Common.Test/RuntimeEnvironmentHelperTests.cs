// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Common.Test
{
    public class RuntimeEnvironmentHelperTests
    {
        [Fact]
        public void PlatformDetection_ShouldBeExclusive()
        {
            // Arrange & Act
            var isWindows = RuntimeEnvironmentHelper.IsWindows;
            var isMacOS = RuntimeEnvironmentHelper.IsMacOSX;
            var isLinux = RuntimeEnvironmentHelper.IsLinux;

            // Assert - Only one platform should be true
            var trueCount = (isWindows ? 1 : 0) + (isMacOS ? 1 : 0) + (isLinux ? 1 : 0);
            Assert.True(trueCount <= 1,
                $"Multiple platforms detected as true: Windows={isWindows}, macOS={isMacOS}, Linux={isLinux}");

            // At least one should be detected (we're running on some platform)
            Assert.True(trueCount >= 1,
                $"No platform detected as true: Windows={isWindows}, macOS={isMacOS}, Linux={isLinux}");
        }
    }
}
