// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;

namespace Serialization;

/// <summary>
/// Tests to validate the parsing of Visual Studio versions use in the .sln file.
/// </summary>
public sealed class VSVersionParsingTests
{
    /// <summary>
    /// Validates the parsing of Visual Studio versions.
    /// General rules are ignoring the leading 'v' or 'V' and any trailing comments (using 'space' char to indicate a comment).
    /// If the version cannot be parsed, it last ".value" is trimmed and the version is tried again.
    /// </summary>
    [Fact]
    public void ParseVersion()
    {
        AssertTryParseVSVersion(string.Empty, expected: false);
        AssertTryParseVSVersion("NOT", expected: false);
        AssertTryParseVSVersion(" ", expected: false);
        AssertTryParseVSVersion(uint.MaxValue.ToString(CultureInfo.InvariantCulture), expected: false);
        AssertTryParseVSVersion(int.MaxValue.ToString(CultureInfo.InvariantCulture), int.MaxValue);
        AssertTryParseVSVersion("17.0.100.100000", 17, 0, 100, 100000);
        AssertTryParseVSVersion("17.0.100.100000 Comment", 17, 0, 100, 100000);
        AssertTryParseVSVersion("17.0.100", 17, 0, 100);
        AssertTryParseVSVersion("17.0", 17);
        AssertTryParseVSVersion("17", 17);
        AssertTryParseVSVersion("    17.0", 17);
        AssertTryParseVSVersion("17.0    ", 17);
        AssertTryParseVSVersion("v17.0", 17);
        AssertTryParseVSVersion("V17", 17);
        AssertTryParseVSVersion("17.0 Comment", 17);
        AssertTryParseVSVersion("17.0.100.+c48D7E9B0", 17, 0, 100);
        AssertTryParseVSVersion("0", 0);
        AssertTryParseVSVersion("0.0", 0, 0);

        // Negative numbers are not allowed.
        AssertTryParseVSVersion(" -5", expected: false);
        AssertTryParseVSVersion("17.0.-100.100000", 17);
        AssertTryParseVSVersion("17.0.100.-100000", 17, 0, 100);

        // This is strange behavior, -0 is treaded as 0.
        // We don't depend on it, but adding here for behavior documentation.
        AssertTryParseVSVersion("-0", 0);
        AssertTryParseVSVersion("-0.4", 0, 4);
        AssertTryParseVSVersion("-0.-0", 0, 0);
        AssertTryParseVSVersion("-0.-0.10", 0, 0, 10);
    }

    private static void AssertTryParseVSVersion(string version, int major, int minor = 0, int build = -1, int revision = -1)
    {
        AssertTryParseVSVersion(version, expected: true, major, minor, build, revision);
    }

    private static void AssertTryParseVSVersion(string value, bool expected, int major = 0, int minor = 0, int build = -1, int revision = -1)
    {
        Version? version = SlnV12Extensions.TryParseVSVersion(value);
        if (expected)
        {
            Assert.NotNull(version);
            Assert.Equal(major, version.Major);
            Assert.Equal(minor, version.Minor);
            Assert.Equal(build, version.Build);
            Assert.Equal(revision, version.Revision);
        }
        else
        {
            Assert.Null(version);
        }
    }
}
