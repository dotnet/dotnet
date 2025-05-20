// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class PathUtilitiesTests
{
    // This test data and the tests that use it are derived from the .NET Runtime:
    // - https://github.com/dotnet/runtime/blob/4eff254880789bf59bab922763446161b1f80640/src/libraries/System.Runtime/tests/System.Runtime.Extensions.Tests/System/IO/PathTests.cs

    public static TheoryData<string, string> TestData_GetExtension => new()
    {
        { @"file.exe", ".exe" },
        { @"file", "" },
        { @"file.", "" },
        { @"file.s", ".s" },
        { @"test/file", "" },
        { @"test/file.extension", ".extension" },
        { @"test\file", "" },
        { @"test\file.extension", ".extension" },
        { "file.e xe", ".e xe"},
        { "file. ", ". "},
        { " file. ", ". "},
        { " file.extension", ".extension"}
    };

    [Theory, MemberData(nameof(TestData_GetExtension))]
    public void GetExtension(string path, string expected)
    {
        Assert.Equal(expected, Path.GetExtension(path));
        Assert.Equal(!string.IsNullOrEmpty(expected), Path.HasExtension(path));
    }

    [Theory, MemberData(nameof(TestData_GetExtension))]
    public void GetExtension_Span(string path, string expected)
    {
        AssertEqual(expected, PathUtilities.GetExtension(path.AsSpan()));
        Assert.Equal(!string.IsNullOrEmpty(expected), PathUtilities.HasExtension(path.AsSpan()));
    }

    // The tests below are derived from the .NET Runtime:
    // - https://github.com/dotnet/runtime/blob/91195a7948a16c769ccaf7fd8ca84b1d210f6841/src/libraries/System.Runtime/tests/System.Runtime.Extensions.Tests/System/IO/Path.IsPathFullyQualified.cs

    [Fact]
    public static void IsPathFullyQualified_NullArgument()
    {
        Assert.Throws<ArgumentNullException>(() => PathUtilities.IsPathFullyQualified(null!));
    }

    [Fact]
    public static void IsPathFullyQualified_Empty()
    {
        Assert.False(PathUtilities.IsPathFullyQualified(""));
        Assert.False(PathUtilities.IsPathFullyQualified(ReadOnlySpan<char>.Empty));
    }

    [ConditionalTheory(Is.Windows)]
    [InlineData("/")]
    [InlineData(@"\")]
    [InlineData(".")]
    [InlineData("C:")]
    [InlineData("C:foo.txt")]
    public static void IsPathFullyQualified_Windows_Invalid(string path)
    {
        Assert.False(PathUtilities.IsPathFullyQualified(path));
        Assert.False(PathUtilities.IsPathFullyQualified(path.AsSpan()));
    }

    [ConditionalTheory(Is.Windows)]
    [InlineData(@"\\")]
    [InlineData(@"\\\")]
    [InlineData(@"\\Server")]
    [InlineData(@"\\Server\Foo.txt")]
    [InlineData(@"\\Server\Share\Foo.txt")]
    [InlineData(@"\\Server\Share\Test\Foo.txt")]
    [InlineData(@"C:\")]
    [InlineData(@"C:\foo1")]
    [InlineData(@"C:\\")]
    [InlineData(@"C:\\foo2")]
    [InlineData(@"C:/")]
    [InlineData(@"C:/foo1")]
    [InlineData(@"C://")]
    [InlineData(@"C://foo2")]
    public static void IsPathFullyQualified_Windows_Valid(string path)
    {
        Assert.True(PathUtilities.IsPathFullyQualified(path));
        Assert.True(PathUtilities.IsPathFullyQualified(path.AsSpan()));
    }

    [ConditionalTheory(Is.AnyUnix)]
    [InlineData(@"\")]
    [InlineData(@"\\")]
    [InlineData(".")]
    [InlineData("./foo.txt")]
    [InlineData("..")]
    [InlineData("../foo.txt")]
    [InlineData(@"C:")]
    [InlineData(@"C:/")]
    [InlineData(@"C://")]
    public static void IsPathFullyQualified_Unix_Invalid(string path)
    {
        Assert.False(PathUtilities.IsPathFullyQualified(path));
        Assert.False(PathUtilities.IsPathFullyQualified(path.AsSpan()));
    }

    [ConditionalTheory(Is.AnyUnix)]
    [InlineData("/")]
    [InlineData("/foo.txt")]
    [InlineData("/..")]
    [InlineData("//")]
    [InlineData("//foo.txt")]
    [InlineData("//..")]
    public static void IsPathFullyQualified_Unix_Valid(string path)
    {
        Assert.True(PathUtilities.IsPathFullyQualified(path));
        Assert.True(PathUtilities.IsPathFullyQualified(path.AsSpan()));
    }

    private static void AssertEqual(ReadOnlySpan<char> expected, ReadOnlySpan<char> actual)
    {
        if (!actual.SequenceEqual(expected))
        {
            throw Xunit.Sdk.EqualException.ForMismatchedValues(expected.ToString(), actual.ToString());
        }
    }
}
