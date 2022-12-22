// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class PathUtilityTests
    {
        [PlatformFact(Platform.Windows)]
        public void PathUtility_RelativePathDifferenctRootCase()
        {
            // Arrange & Act
            var path1 = @"C:\foo\";
            var path2 = @"c:\foo\bar";
            var path = PathUtility.GetRelativePath(path1, path2);

            // Assert
            Assert.Equal("bar", path);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("LICENSE", "LICENSE")]
        [InlineData(@".\LICENSE", "LICENSE")]
        [InlineData(@".\\\LICENSE", "LICENSE")]
        public void PathUtility_StripLeadingDirectorySeparators_OnWindows(string given, string expected)
        {
            Assert.Equal(expected, PathUtility.StripLeadingDirectorySeparators(given));
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("LICENSE", "LICENSE")]
        [InlineData(@".///LICENSE", "LICENSE")]
        [InlineData(@"./LICENSE", "LICENSE")]
        public void PathUtility_StripLeadingDirectorySeparators_OnLinux(string given, string expected)
        {
            Assert.Equal(expected, PathUtility.StripLeadingDirectorySeparators(given));
        }
    }
}
