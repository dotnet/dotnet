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

    private static void AssertEqual(ReadOnlySpan<char> expected, ReadOnlySpan<char> actual)
    {
        if (!actual.SequenceEqual(expected))
        {
            throw Xunit.Sdk.EqualException.ForMismatchedValues(expected.ToString(), actual.ToString());
        }
    }
}
