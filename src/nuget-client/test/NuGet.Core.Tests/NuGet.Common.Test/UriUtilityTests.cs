// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class UriUtilityTests
    {
        [Theory]
        [InlineData("file:///test", "test")]
        [InlineData("file://test", "test")]
        [InlineData("https://api.nuget.org/v3/index.json", "https://api.nuget.org/v3/index.json")]
        [InlineData("a/b/c", "a/b/c")]
        [InlineData("", "")]
        [InlineData("ftp://test", "ftp://test")]
        [InlineData("a", "a")]
        [InlineData("..\\a", "..\\a")]
        public void UriUtility_GetLocalPath(string input, string expected)
        {
            // Arrange & Act
            var local = UriUtility.GetLocalPath(input);

            // Assert
            // Trim for xplat
            Assert.Equal(expected, local.TrimStart('\\').TrimStart('/'));
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test/../test2", "test2")]
        [InlineData("../test", "../test")]
        [InlineData("a/b/c", "a/b/c")]
        public void UriUtility_GetAbsolutePath_VerifyRelativePathCombined(string source, string relative)
        {
            using (var root = TestDirectory.Create())
            {
                var expected = Path.GetFullPath(Path.Combine(root, relative));

                var path = UriUtility.GetAbsolutePath(root, source);

                Assert.Equal(expected, path);
            }
        }

        [Fact]
        public void UriUtility_GetAbsolutePath_VerifyUrlPathUnchanged()
        {
            using (var root = TestDirectory.Create())
            {
                var source = "https://api.nuget.org/v3/index.json";

                var path = UriUtility.GetAbsolutePath(root, source);

                Assert.Equal(source, path);
            }
        }

        [Fact]
        public void UriUtility_GetAbsolutePath_VerifyAbsolutePathUnchanged()
        {
            using (var root = TestDirectory.Create())
            using (var root2 = TestDirectory.Create())
            {
                var source = root2;

                var path = UriUtility.GetAbsolutePath(root, source);

                Assert.Equal(source, path);
            }
        }

        [Theory]
        [InlineData("http://nuget.org/api/v2", true)]
        [InlineData("http://NUGET.ORG/api/v2", true)]
        [InlineData("https://nuget.org/api/v2", true)]
        [InlineData("https://NUGET.ORG/api/v2", true)]
        [InlineData("http://www.nuget.org/api/v2", true)]
        [InlineData("http://WWW.NUGET.ORG/api/v2", true)]
        [InlineData("https://www.nuget.org/api/v2", true)]
        [InlineData("https://WWW.NUGET.ORG/api/v2", true)]
        [InlineData("http://api.nuget.org/v3/index.json", true)]
        [InlineData("http://API.NUGET.ORG/v3/index.json", true)]
        [InlineData("https://api.nuget.org/v3/index.json", true)]
        [InlineData("https://API.NUGET.ORG/v3/index.json", true)]
        [InlineData("http://notnuget.org/api/v2", false)]
        [InlineData("http://not.nuget.org", true)]
        [InlineData("https://not.nuget.org/", true)]
        [InlineData("https://nuget.org.internal/v3/index.json", false)]
        [InlineData("http://randommynuget.org/", false)]
        [InlineData("https://randommynuget.org/", false)]
        [InlineData("https://www.random-mynuget.org/", false)]
        [InlineData("https://nuget.smbsrc.net/", false)]
        [InlineData("https://nuget.smbsrc.net/nuget.org", false)]
        [InlineData("", false)]
        [InlineData("file://test", false)]
        [InlineData("..\\a", false)]
        public void IsNuGetOrg(string sourceUrl, bool expected)
        {
            // Act
            var actual = UriUtility.IsNuGetOrg(sourceUrl);

            // Assert
            Assert.Equal(expected, actual);
        }

        // A relative source is resolved against the fully qualified root, producing a deterministic absolute path that
        // does not depend on the process current directory. Windows-only for the backslash-normalized expected values.
        [PlatformTheory(Platform.Windows)]
        [InlineData("sub/child", @"C:\root\sub\child")]
        [InlineData("nested/../child", @"C:\root\child")]
        [InlineData("./relative", @"C:\root\relative")]
        public void UriUtility_GetAbsolutePath_WithFullyQualifiedRoot_ResolvesRelativeSourceToExpectedPath(string source, string expected)
        {
            Assert.Equal(expected, UriUtility.GetAbsolutePath(@"C:\root", source));
        }

#if !NETFRAMEWORK
        // Drive-relative ("C:") and root-relative ("\foo") sources are rooted but not fully qualified: historically they
        // resolved against the process current directory/drive. On .NET they must anchor to the fully qualified root.
        [PlatformTheory(Platform.Windows)]
        [InlineData("C:", @"C:\root")]
        [InlineData(@"\foo", @"C:\foo")]
        public void UriUtility_GetAbsolutePath_WithFullyQualifiedRoot_AnchorsRootedRelativeSourceToRoot(string source, string expected)
        {
            Assert.Equal(expected, UriUtility.GetAbsolutePath(@"C:\root", source));
        }
#endif

        // An already-absolute source path is returned canonicalized; the root is ignored.
        [PlatformTheory(Platform.Windows)]
        [InlineData(@"C:\absolute\source", @"C:\absolute\source")]
        [InlineData(@"C:\absolute\..\source", @"C:\source")]
        public void UriUtility_GetAbsolutePath_WithAbsoluteSource_ReturnsCanonicalSourceIgnoringRoot(string source, string expected)
        {
            Assert.Equal(expected, UriUtility.GetAbsolutePath(@"C:\other\root", source));
        }

        // Non-file sources (http/ftp) are returned unchanged regardless of the root.
        [Theory]
        [InlineData("https://api.nuget.org/v3/index.json")]
        [InlineData("ftp://example.org/path")]
        public void UriUtility_GetAbsolutePath_WithNonFileSource_ReturnsSourceUnchanged(string source)
        {
            Assert.Equal(source, UriUtility.GetAbsolutePath(@"C:\root", source));
        }
    }
}
