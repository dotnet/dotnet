// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Configuration;
using Xunit;
using static OpenTelemetry.Internal.DatabaseSemanticConventionHelper;

namespace OpenTelemetry.Internal.Tests;

public class DatabaseSemanticConventionHelperTests
{
    public static IEnumerable<object[]> TestCases =>
    [
        [null!, DatabaseSemanticConvention.Old],
        [string.Empty, DatabaseSemanticConvention.Old],
        [" ", DatabaseSemanticConvention.Old],
        ["junk", DatabaseSemanticConvention.Old],
        ["none", DatabaseSemanticConvention.Old],
        ["NONE", DatabaseSemanticConvention.Old],
        ["database", DatabaseSemanticConvention.New],
        ["DATABASE", DatabaseSemanticConvention.New],
        ["database/dup", DatabaseSemanticConvention.Dupe],
        ["DATABASE/DUP", DatabaseSemanticConvention.Dupe],
        ["junk,,junk", DatabaseSemanticConvention.Old],
        ["junk,JUNK", DatabaseSemanticConvention.Old],
        ["junk1,junk2", DatabaseSemanticConvention.Old],
        ["junk,database", DatabaseSemanticConvention.New],
        ["junk,database , database ,junk", DatabaseSemanticConvention.New],
        ["junk,database/dup", DatabaseSemanticConvention.Dupe],
        ["junk, database/dup ", DatabaseSemanticConvention.Dupe],
        ["database/dup,database", DatabaseSemanticConvention.Dupe],
        ["database,database/dup", DatabaseSemanticConvention.Dupe],
    ];

    [Fact]
    public void VerifyFlags()
    {
        var testValue = DatabaseSemanticConvention.Dupe;
        Assert.True(testValue.HasFlag(DatabaseSemanticConvention.Old));
        Assert.True(testValue.HasFlag(DatabaseSemanticConvention.New));

        testValue = DatabaseSemanticConvention.Old;
        Assert.True(testValue.HasFlag(DatabaseSemanticConvention.Old));
        Assert.False(testValue.HasFlag(DatabaseSemanticConvention.New));

        testValue = DatabaseSemanticConvention.New;
        Assert.False(testValue.HasFlag(DatabaseSemanticConvention.Old));
        Assert.True(testValue.HasFlag(DatabaseSemanticConvention.New));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void VerifyGetSemanticConventionOptIn_UsingEnvironmentVariable(string input, string expectedValue)
    {
        using (EnvironmentVariableScope.Create(SemanticConventionOptInKeyName, input))
        {
#if NET
            var expected = Enum.Parse<DatabaseSemanticConvention>(expectedValue);
#else
            var expected = Enum.Parse(typeof(DatabaseSemanticConvention), expectedValue);
#endif
            Assert.Equal(expected, GetSemanticConventionOptIn(new ConfigurationBuilder().AddEnvironmentVariables().Build()));
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void VerifyGetSemanticConventionOptIn_UsingIConfiguration(string input, string expectedValue)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { [SemanticConventionOptInKeyName] = input })
            .Build();

#if NET
        var expected = Enum.Parse<DatabaseSemanticConvention>(expectedValue);
#else
        var expected = Enum.Parse(typeof(DatabaseSemanticConvention), expectedValue);
#endif
        Assert.Equal(expected, GetSemanticConventionOptIn(configuration));
    }
}
