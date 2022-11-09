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
            ReleaseFile f1 = new ReleaseFile("abcdef", "foo", "win-x86", "https://here.there.com");
            ReleaseFile f2 = f1;

            Assert.Equal(f1, f2);
        }

        [Fact]
        public void ItReturnsTrueIfValuesAreEquals()
        {
            ReleaseFile f1 = new ReleaseFile("abcdef", "foo", "win-x86", "https://here.there.com");
            ReleaseFile f2 = new ReleaseFile("abcdef", "foo", "win-x86", "https://here.there.com");

            Assert.Equal(f1, f2);
        }

        [Fact]
        public void ItReturnsFalseIfReferenceEquals2()
        {
            ReleaseFile f1 = new ReleaseFile("abcdef", "Foo", "win-x86", "https://here.there.com");
            ReleaseFile f2 = new ReleaseFile("abcdef", "foo", "win-x86", "https://here.there.com");

            Assert.NotEqual(f1, f2);
        }

        [Fact]
        public void ItComputesTheFileNameFromTheUrl()
        {
            ReleaseFile f1 = new ReleaseFile("abcdef", "Foo", "win-x86", "https://here.there.com/foo-win-x86.exe");

            Assert.Equal("foo-win-x86.exe", f1.FileName);
        }

        [Fact]
        public async Task ItThrowsIfDestinationPathIsNull()
        {
            ProductRelease release = GetProductRelease("2.1", "2.1.8");
            ReleaseFile file = release.Files.FirstOrDefault();
            Func<Task> f = async () => await file.DownloadAsync(null);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(f);
        }

        [Fact]
        public async Task ItThrowsIfDestinationPathIsEmpty()
        {
            ProductRelease release = GetProductRelease("2.1", "2.1.8");
            ReleaseFile file = release.Files.FirstOrDefault();
            Func<Task> f = async () => await file.DownloadAsync("");

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(f);

            Assert.Equal($"Value cannot be empty.{Environment.NewLine}Parameter name: destinationPath", exception.Message);
        }
    }
}
