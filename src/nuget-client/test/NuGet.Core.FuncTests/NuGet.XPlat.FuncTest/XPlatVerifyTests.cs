// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.CommandLine;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.Commands;
using NuGet.Common;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    [Collection(XPlatCollection.Name)]
    public class XPlatVerifyTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatVerifyTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void VerifyCommandArgsParsing_MissingPackagePath_ReturnsNonZero()
        {
            VerifyCommandArgs(
                (mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string>() { "verify" };

                    // Act
                    var result = testApp.Parse(argList.ToArray()).Invoke();

                    // Assert
                    Assert.NotEqual(0, result);
                });
        }

        [Theory]
        [InlineData("--verbosity", "q", LogLevel.Warning)]
        [InlineData("-v", "quiet", LogLevel.Warning)]
        [InlineData("--verbosity", "m", LogLevel.Minimal)]
        [InlineData("-v", "minimal", LogLevel.Minimal)]
        [InlineData("--verbosity", "something-else", LogLevel.Minimal)]
        [InlineData("-v", "n", LogLevel.Information)]
        [InlineData("--verbosity", "normal", LogLevel.Information)]
        [InlineData("-v", "d", LogLevel.Debug)]
        [InlineData("-v", "detailed", LogLevel.Debug)]
        [InlineData("--verbosity", "diag", LogLevel.Debug)]
        [InlineData("-v", "diagnostic", LogLevel.Debug)]
        public void VerifyCommandArgsParsing_VerbosityOption(string option, string verbosity, LogLevel logLevel)
        {
            VerifyCommandArgs(
                (mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange                   
                    var argList = new List<string> { "verify", "packageX.nupkg", option, verbosity };

                    // Act
                    var result = testApp.Parse(argList.ToArray()).Invoke();

                    // Assert
                    Assert.Equal(logLevel, getLogLevel());
                    Assert.Equal(0, result);
                });
        }

        private void VerifyCommandArgs(Action<Mock<IVerifyCommandRunner>, RootCommand, Func<LogLevel>> verify)
        {
            // Arrange
            var logLevel = LogLevel.Information;
            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var testApp = new RootCommand();
            var mockCommandRunner = new Mock<IVerifyCommandRunner>();
            mockCommandRunner
                .Setup(m => m.ExecuteCommandAsync(It.IsAny<VerifyArgs>()))
                .Returns(Task.FromResult(0));

            VerifyCommand.Register(testApp,
                () => logger,
                ll => logLevel = ll,
                () => mockCommandRunner.Object);

            // Act & Assert
            verify(mockCommandRunner, testApp, () => logLevel);
        }
    }
}
