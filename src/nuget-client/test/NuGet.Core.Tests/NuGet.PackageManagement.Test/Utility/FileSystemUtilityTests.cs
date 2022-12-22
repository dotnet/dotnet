// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class FileSystemUtilityTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ContentEquals_ThrowsForNullOrEmptyPath(string path)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => FileSystemUtility.ContentEquals(
                    path: path,
                    streamFactory: () => Stream.Null));

            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public void ContentEquals_ThrowsForNullStreamTaskFactory()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => FileSystemUtility.ContentEquals(
                    path: "a",
                    streamFactory: null));

            Assert.Equal("streamFactory", exception.ParamName);
        }

        [Fact]
        public void ContentEquals_ReturnsFalseIfNotEqual()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("a")))
            using (var testDirectory = TestDirectory.Create())
            {
                var filePath = Path.Combine(testDirectory.Path, "file");

                File.WriteAllText(filePath, "b");

                var areEqual = FileSystemUtility.ContentEquals(filePath, () => stream);

                Assert.False(areEqual);
            }
        }

        [Fact]
        public void ContentEquals_ReturnsTrueIfEqual()
        {
            var content = "a";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            using (var testDirectory = TestDirectory.Create())
            {
                var filePath = Path.Combine(testDirectory.Path, "file");

                File.WriteAllText(filePath, content);

                var areEqual = FileSystemUtility.ContentEquals(filePath, () => stream);

                Assert.True(areEqual);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ContentEqualsAsync_ThrowsForNullOrEmptyPath(string path)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => FileSystemUtility.ContentEqualsAsync(
                    path: path,
                    streamTaskFactory: () => Task.FromResult(Stream.Null)));

            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public async Task ContentEqualsAsync_ThrowsForNullStreamTaskFactory()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => FileSystemUtility.ContentEqualsAsync(
                    path: "a",
                    streamTaskFactory: null));

            Assert.Equal("streamTaskFactory", exception.ParamName);
        }

        [Fact]
        public async Task ContentEqualsAsync_ReturnsFalseIfNotEqual()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("a")))
            using (var testDirectory = TestDirectory.Create())
            {
                var filePath = Path.Combine(testDirectory.Path, "file");

                File.WriteAllText(filePath, "b");

                var areEqual = await FileSystemUtility.ContentEqualsAsync(
                    filePath,
                    () => Task.FromResult<Stream>(stream));

                Assert.False(areEqual);
            }
        }

        [Fact]
        public async Task ContentEqualsAsync_ReturnsTrueIfEqual()
        {
            var content = "a";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            using (var testDirectory = TestDirectory.Create())
            {
                var filePath = Path.Combine(testDirectory.Path, "file");

                File.WriteAllText(filePath, content);

                var areEqual = await FileSystemUtility.ContentEqualsAsync(
                    filePath,
                    () => Task.FromResult<Stream>(stream));

                Assert.True(areEqual);
            }
        }
    }
}
