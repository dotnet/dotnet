// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.CommandLine.XPlat.Commands.Package.Stage;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Stage
{
    public class StagePushCommandRunnerTests
    {
        public static TheoryData<string?, bool> GroupIdTestData => new()
        {
            { null, true },
            { "a", true },
            { "release", true },
            { "release-group", true },
            { "release_group", true },
            { "release.group", true },
            { new string('a', 64), true },
            { "", false },
            { " ", false },
            { "-release", false },
            { "release-", false },
            { ".release", false },
            { "release.", false },
            { "release group", false },
            { "rélease", false },
            { "release\nnext", false },
            { new string('a', 65), false },
        };

        [Theory]
        [MemberData(nameof(GroupIdTestData))]
        public void IsValidGroupId_ReturnsExpectedResult(
            string? groupId,
            bool expected)
        {
            // Act
            bool actual = StagePushCommandRunner.IsValidGroupId(groupId);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
