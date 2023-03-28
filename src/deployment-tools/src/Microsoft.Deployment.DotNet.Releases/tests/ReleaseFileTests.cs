// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ReleaseFileTests : TestBase
    {
        [Fact]
        public void ItReturnsTrueIfReferenceEquals()
        {
            ReleaseFile f1 = ReleaseFile.Create("abcdef", "foo", "win-x86", "https://here.there.com");
            ReleaseFile f2 = f1;

            Assert.Equal(f1, f2);
        }

        [Fact]
        public void ItReturnsTrueIfValuesAreEquals()
        {
            ReleaseFile f1 = ReleaseFile.Create("abcdef", "foo", "win-x86", "https://here.there.com");
            ReleaseFile f2 = ReleaseFile.Create("abcdef", "foo", "win-x86", "https://here.there.com");

            Assert.Equal(f1, f2);
        }

        [Fact]
        public void ItReturnsFalseIfReferenceEquals2()
        {
            ReleaseFile f1 = ReleaseFile.Create("abcdef", "Foo", "win-x86", "https://here.there.com");
            ReleaseFile f2 = ReleaseFile.Create("abcdef", "foo", "win-x86", "https://here.there.com");

            Assert.NotEqual(f1, f2);
        }

        [Fact]
        public void ItComputesTheFileNameFromTheUrl()
        {
            ReleaseFile f1 = ReleaseFile.Create("abcdef", "Foo", "win-x86", "https://here.there.com/foo-win-x86.exe");

            Assert.Equal("foo-win-x86.exe", f1.FileName);
        }

        [Fact]
        public void ItCreatesReleaseComponents()
        {
            ProductRelease release = GetProductRelease("3.1", "3.1.6");

            Assert.Equal("16.4.11, 16.6.3", release.AspNetCoreRuntime.VisualStudioVersion);
            Assert.Equal(new Version("13.1.20169.6"), release.AspNetCoreRuntime.AspNetCoreModuleVersions[0]);

            // This is technically a typo in the releases data since the vs-version properties should line up.
            Assert.Equal("16.4.11, 16.6.4", release.Runtime.VisualStudioVersion);
            Assert.Equal("8.0", release.Sdks[0].CSharpVersion);
        }

        [Fact]
        public async Task ItThrowsIfDestinationPathIsNull()
        {
            ProductRelease release = GetProductRelease("2.1", "2.1.8");
            ReleaseFile file = release.Files.FirstOrDefault();
            Func<Task> f = async () => await file.DownloadAsync(null).ConfigureAwait(false);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(f).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItThrowsIfDestinationPathIsEmpty()
        {
            ProductRelease release = GetProductRelease("2.1", "2.1.8");
            ReleaseFile file = release.Files.FirstOrDefault();
            Func<Task> f = async () => await file.DownloadAsync("").ConfigureAwait(false);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(f).ConfigureAwait(false);

            Assert.Equal($"Value cannot be empty. (Parameter 'destinationPath')", exception.Message);
        }
    }
}
