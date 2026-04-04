// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Tests;

// The tests can only be strong named if they only depend on assemblies that are strong named,
// therefore if the tests are strong named then the libraries we ship are strong named.

public static class StrongNameTests
{
    // If this test fails and the test project needs to opt-out of strong naming, then add a
    // <SkipStrongNameValidation>true</SkipStrongNameValidation> property to the tests' project file.

    [Fact]
    public static void Tests_Are_Strong_Named()
    {
        // Arrange
        var assembly = typeof(StrongNameTests).Assembly;
        var name = assembly.GetName();

        // Act
        var actual = name.GetPublicKey();

        // Assert
        Assert.NotNull(actual);
        Assert.NotEmpty(actual);

#if NET
        Assert.Equal(
            "002400000480000094000000060200000024000052534131000400000100010051c1562a090fb0c9f391012a32198b5e5d9a60e9b80fa2d7b434c9e5ccb7259bd606e66f9660676afc6692b8cdc6793d190904551d2103b7b22fa636dcbb8208839785ba402ea08fc00c8f1500ccef28bbf599aa64ffb1e1d5dc1bf3420a3777badfe697856e9d52070a50c3ea5821c80bef17ca3acffa28f89dd413f096f898",
            Convert.ToHexString(actual),
            ignoreCase: true);
#endif

        // Act
        actual = name.GetPublicKeyToken();

        // Assert
        Assert.NotNull(actual);
        Assert.NotEmpty(actual);

#if NET
        Assert.Equal("7BD6737FE5B67E3C", Convert.ToHexString(actual));
#endif
    }
}
