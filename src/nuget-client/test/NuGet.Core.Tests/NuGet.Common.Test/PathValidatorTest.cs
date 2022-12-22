// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Common
{
    public class PathValidatorTest
    {
        [Theory]
        [InlineData(@"C:\", "windows")]
        [InlineData(@"C:\path", "windows")]
        [InlineData(@"C:\path\to\", "windows")]
        [InlineData(@"/", "unix-base")]
        [InlineData(@"/users", "unix-base")]
        [InlineData(@"/users/path", "unix-base")]
        public void PathValidatorTest_ValidLocalPath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.True(PathValidator.IsValidLocalPath(path));
            }
        }

        [Theory]
        //[InlineData(@"C:\path\*\","windows")] TODO: Enabled once this issue is fixed: https://github.com/NuGet/Home/issues/7588
        [InlineData(@"\\share\packages", "windows")]
        [InlineData(@"packages\test", "windows")]
        [InlineData(@"https://test", "windows")]
        [InlineData(@"https://test", "unix-base")]
        [InlineData(@"./packages", "unix-base")]
        public void PathValidatorTest_InvalidLocalPath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.False(PathValidator.IsValidLocalPath(path));
            }
        }

        [Theory]
        [InlineData(@"\\server\share", "windows")]
        public void PathValidatorTest_ValidUncSharePath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.True(PathValidator.IsValidUncPath(path));
            }
        }

        [Theory]
        [InlineData(@"C:", "windows")]
        //[InlineData(@"\\server\invalid\*\","windows")] TODO: Enabled once this issue is fixed: https://github.com/NuGet/Home/issues/7588
        [InlineData(@"https://test", "windows")]
        [InlineData(@"..\packages", "windows")]
        public void PathValidatorTest_InvalidUncSharePath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.False(PathValidator.IsValidUncPath(path));
            }
        }

        [Theory]
        [InlineData(@"http://test/path")]
        [InlineData(@"ftp://test/path")]
        public void PathValidatorTest_ValidUrlPath(string path)
        {
            Assert.True(PathValidator.IsValidUrl(path));
        }

        [Theory]
        [InlineData(@"..\packages", "windows")]
        [InlineData(@"C:\", "windows")]
        [InlineData(@"\\test\packages", "windows")]
        [InlineData(@"/user/test", "unix-base")]
        public void PathValidatorTest_InvalidUrlPath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.False(PathValidator.IsValidUrl(path));
            }
        }

        [Theory]
        [InlineData(@"package\path", "windows")]
        [InlineData(@"package/path", "unix-base")]
        [InlineData(@"../package/path", "unix-base")]
        [InlineData(@"..\package\path", "windows")]
        [InlineData(@"./package/path", "unix-base")]
        [InlineData(@".\package\path", "windows")]
        public void PathValidatorTest_ValidRelativePath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.True(PathValidator.IsValidRelativePath(path));
            }
        }

        [Theory]
        //[InlineData(@"package\path\*","windows")] TODO: Enabled once this issue is fixed: https://github.com/NuGet/Home/issues/7588
        [InlineData(@"\\package\path", "windows")]
        [InlineData(@"https://test", "windows")]
        [InlineData(@"https://test", "unix-base")]
        [InlineData(@"/test/path", "unix-base")]
        public void PathValidatorTest_InvalidRelativePath(string path, string os)
        {
            if ((os == "windows" && RuntimeEnvironmentHelper.IsWindows) || (os == "unix-base" && !RuntimeEnvironmentHelper.IsWindows))
            {
                Assert.False(PathValidator.IsValidRelativePath(path));
            }
        }
    }
}
