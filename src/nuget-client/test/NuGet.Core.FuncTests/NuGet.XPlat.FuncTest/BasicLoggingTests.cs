// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    public class BasicLoggingTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BasicLoggingTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void BasicLogging_NoParams_ExitCode()
        {
            // Arrange
            var log = new TestCommandOutputLogger(_testOutputHelper);

            var args = new string[]
            {
                //empty
            };

            // Act
            var exitCode = NuGet.CommandLine.XPlat.Program.MainInternal(args, log, TestEnvironmentVariableReader.EmptyInstance);

            // Assert
            Assert.Equal(0, exitCode);
        }
    }
}
