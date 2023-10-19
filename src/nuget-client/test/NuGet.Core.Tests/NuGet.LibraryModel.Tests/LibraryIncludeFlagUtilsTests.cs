// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class LibraryIncludeFlagUtilsTests
    {
        [Fact]
        public void GetFlags_WhenFlagsIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => LibraryIncludeFlagUtils.GetFlags(flags: null!));

            Assert.Equal("flags", exception.ParamName);
        }

        [Fact]
        public void GetFlags_WhenFlagsIsEmpty_ReturnsNone()
        {
            LibraryIncludeFlags flags = LibraryIncludeFlagUtils.GetFlags(Enumerable.Empty<string>());

            Assert.Equal(LibraryIncludeFlags.None, flags);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.All, LibraryIncludeFlags.All)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers)]
        [InlineData(LibraryIncludeFlags.Build, LibraryIncludeFlags.Build)]
        [InlineData(LibraryIncludeFlags.BuildTransitive, LibraryIncludeFlags.BuildTransitive | LibraryIncludeFlags.Build)]
        [InlineData(LibraryIncludeFlags.Compile, LibraryIncludeFlags.Compile)]
        [InlineData(LibraryIncludeFlags.ContentFiles, LibraryIncludeFlags.ContentFiles)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Native)]
        [InlineData(LibraryIncludeFlags.None, LibraryIncludeFlags.None)]
        [InlineData(LibraryIncludeFlags.Runtime, LibraryIncludeFlags.Runtime)]
        public void GetFlags_WhenFlagsIsSingleValue_ReturnsFlag(
            LibraryIncludeFlags input,
            LibraryIncludeFlags expectedResult)
        {
            LibraryIncludeFlags actualResult = LibraryIncludeFlagUtils.GetFlags(new[] { input.ToString() });

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("compile")]
        [InlineData("COMPILE")]
        public void GetFlags_WhenFlagsCasingDiffers_ReturnsFlag(string flag)
        {
            LibraryIncludeFlags flags = LibraryIncludeFlagUtils.GetFlags(new[] { flag });

            Assert.Equal(LibraryIncludeFlags.Compile, flags);
        }

        [Fact]
        public void GetFlags_WhenFlagsIsMultipleValues_ReturnsCombinationOfValues()
        {
            LibraryIncludeFlags[] expectedFlags = new[]
            {
                LibraryIncludeFlags.Runtime,
                LibraryIncludeFlags.Compile,
                LibraryIncludeFlags.Build
            };

            LibraryIncludeFlags flags = LibraryIncludeFlagUtils.GetFlags(expectedFlags.Select(flag => flag.ToString()));

            Assert.Equal("Runtime, Compile, Build", flags.ToString());
        }
    }
}
