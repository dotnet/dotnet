// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class IncludeExcludeFilesTests
    {
        private const long Seed = 0x1505L;

        [Fact]
        public void Constructor_Always_InitializesProperties()
        {
            var files = new IncludeExcludeFiles();

            Assert.Null(files.Exclude);
            Assert.Null(files.ExcludeFiles);
            Assert.Null(files.Include);
            Assert.Null(files.IncludeFiles);
        }

        [Fact]
        public void HandleIncludeExcludeFiles_WhenJsonObjectIsNull_Throws()
        {
            var files = new IncludeExcludeFiles();

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => files.HandleIncludeExcludeFiles(jsonObject: null));

            Assert.Equal("jsonObject", exception.ParamName);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"exclude\":null}")]
        [InlineData("{\"excludeFiles\":null}")]
        [InlineData("{\"include\":null}")]
        [InlineData("{\"includeFiles\":null}")]
        public void HandleIncludeExcludeFiles_WithNullForFiles_ReturnsFalse(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            var files = new IncludeExcludeFiles();

            Assert.False(files.HandleIncludeExcludeFiles(jsonObject));
            Assert.Null(files.Exclude);
            Assert.Null(files.ExcludeFiles);
            Assert.Null(files.Include);
            Assert.Null(files.IncludeFiles);
        }

        [Theory]
        [InlineData("{\"exclude\":[]}")]
        [InlineData("{\"exclude\":[\"a\"]}", "a")]
        public void HandleIncludeExcludeFiles_WithValidValueForExclude_ReturnsTrue(
            string json,
            params string[] expectedResults)
        {
            JObject jsonObject = JObject.Parse(json);
            var files = new IncludeExcludeFiles();

            Assert.True(files.HandleIncludeExcludeFiles(jsonObject));

            Assert.Equal(expectedResults, files.Exclude);
            Assert.Null(files.ExcludeFiles);
            Assert.Null(files.Include);
            Assert.Null(files.IncludeFiles);
        }

        [Theory]
        [InlineData("{\"excludeFiles\":[]}")]
        [InlineData("{\"excludeFiles\":[\"a\"]}", "a")]
        public void HandleIncludeExcludeFiles_WithValidValueForExcludeFiles_ReturnsTrue(
            string json,
            params string[] expectedResults)
        {
            JObject jsonObject = JObject.Parse(json);
            var files = new IncludeExcludeFiles();

            Assert.True(files.HandleIncludeExcludeFiles(jsonObject));

            Assert.Null(files.Exclude);
            Assert.Equal(expectedResults, files.ExcludeFiles);
            Assert.Null(files.Include);
            Assert.Null(files.IncludeFiles);
        }

        [Theory]
        [InlineData("{\"include\":[]}")]
        [InlineData("{\"include\":[\"a\"]}", "a")]
        public void HandleIncludeExcludeFiles_WithValidValueForInclude_ReturnsTrue(
            string json,
            params string[] expectedResults)
        {
            JObject jsonObject = JObject.Parse(json);
            var files = new IncludeExcludeFiles();

            Assert.True(files.HandleIncludeExcludeFiles(jsonObject));

            Assert.Null(files.Exclude);
            Assert.Null(files.ExcludeFiles);
            Assert.Equal(expectedResults, files.Include);
            Assert.Null(files.IncludeFiles);
        }

        [Theory]
        [InlineData("{\"includeFiles\":[]}")]
        [InlineData("{\"includeFiles\":[\"a\"]}", "a")]
        public void HandleIncludeExcludeFiles_WithValidValueForIncludeFiles_ReturnsTrue(
            string json,
            params string[] expectedResults)
        {
            JObject jsonObject = JObject.Parse(json);
            var files = new IncludeExcludeFiles();

            Assert.True(files.HandleIncludeExcludeFiles(jsonObject));

            Assert.Null(files.Exclude);
            Assert.Null(files.ExcludeFiles);
            Assert.Null(files.Include);
            Assert.Equal(expectedResults, files.IncludeFiles);
        }

        [Fact]
        public void GetHashCode_Always_HashesAllProperties()
        {
            IReadOnlyList<string> exclude = new[] { "a" };
            IReadOnlyList<string> excludeFiles = new[] { "b" };
            IReadOnlyList<string> include = new[] { "c" };
            IReadOnlyList<string> includeFiles = new[] { "d" };

            int expectedResult = GetExpectedHashCode(exclude, excludeFiles, include, includeFiles);

            var files = new IncludeExcludeFiles();

            files.Exclude = exclude;
            files.ExcludeFiles = excludeFiles;
            files.Include = include;
            files.IncludeFiles = includeFiles;

            int actualResult = files.GetHashCode();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Equals_WithOther_ReturnsCorrectValue()
        {
            var files1 = new IncludeExcludeFiles();
            var files2 = new IncludeExcludeFiles();

            Assert.False(files1.Equals(other: null));
            Assert.True(files1.Equals(other: files1));
            Assert.True(files1.Equals(other: files2));

            var files = new List<string>() { "a" };

            files1.Exclude = files;
            Assert.False(files1.Equals(files2));
            files2.Exclude = files;
            Assert.True(files1.Equals(files2));

            files1.ExcludeFiles = files;
            Assert.False(files1.Equals(files2));
            files2.ExcludeFiles = files;
            Assert.True(files1.Equals(files2));

            files1.Include = files;
            Assert.False(files1.Equals(files2));
            files2.Include = files;
            Assert.True(files1.Equals(files2));

            files1.IncludeFiles = files;
            Assert.False(files1.Equals(files2));
            files2.IncludeFiles = files;
            Assert.True(files1.Equals(files2));
        }

        [Fact]
        public void Equals_WithObj_ReturnsCorrectValue()
        {
            var files1 = new IncludeExcludeFiles();
            var files2 = new IncludeExcludeFiles();

            Assert.False(files1.Equals(obj: null));
            Assert.False(files1.Equals(obj: "b"));
            Assert.False(files1.Equals(obj: new object()));
            Assert.True(files1.Equals(obj: files1));
            Assert.True(files1.Equals(obj: files2));
        }

        [Fact]
        public void Clone_Always_CreatesDeepClone()
        {
            IReadOnlyList<string> exclude = new[] { "a" };
            IReadOnlyList<string> excludeFiles = new[] { "b" };
            IReadOnlyList<string> include = new[] { "c" };
            IReadOnlyList<string> includeFiles = new[] { "d" };

            var expectedResult = new IncludeExcludeFiles()
            {
                Exclude = exclude,
                ExcludeFiles = excludeFiles,
                Include = include,
                IncludeFiles = includeFiles
            };
            int expectedHashCode = expectedResult.GetHashCode();

            IncludeExcludeFiles actualResult = expectedResult.Clone();
            int actualHashCode = actualResult.GetHashCode();

            Assert.NotSame(expectedResult, actualResult);
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedHashCode, actualHashCode);
            Assert.NotSame(expectedResult.Exclude, actualResult.Exclude);
            Assert.Equal(exclude, actualResult.Exclude);
            Assert.NotSame(expectedResult.ExcludeFiles, actualResult.ExcludeFiles);
            Assert.Equal(excludeFiles, actualResult.ExcludeFiles);
            Assert.NotSame(expectedResult.Include, actualResult.Include);
            Assert.Equal(include, actualResult.Include);
            Assert.NotSame(expectedResult.IncludeFiles, actualResult.IncludeFiles);
            Assert.Equal(includeFiles, actualResult.IncludeFiles);
        }

        private static int GetExpectedHashCode(
            IReadOnlyList<string> exclude,
            IReadOnlyList<string> excludeFiles,
            IReadOnlyList<string> include,
            IReadOnlyList<string> includeFiles)
        {
            long hashCode = Seed;

            hashCode = AddHashCodes(hashCode, include);
            hashCode = AddHashCodes(hashCode, exclude);
            hashCode = AddHashCodes(hashCode, includeFiles);
            hashCode = AddHashCodes(hashCode, excludeFiles);

            return hashCode.GetHashCode();
        }

        private static long AddHashCode(long runningHashCode, int newHashCode)
        {
            return ((runningHashCode << 5) + runningHashCode) ^ newHashCode;
        }

        private static long AddHashCodes<T>(long hashCode, IEnumerable<T> items)
        {
            long combinedHashCode = hashCode;

            if (items != null)
            {
                foreach (T item in items)
                {
                    combinedHashCode = AddHashCode(combinedHashCode, item.GetHashCode());
                }
            }

            return combinedHashCode;
        }
    }
}
