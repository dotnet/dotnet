// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.CommandLine.XPlat;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatTrustTests
    {
        private static readonly string DotnetCli = TestFileSystemUtility.GetDotnetCli();
        private static readonly string XplatDll = DotnetCliUtil.GetXplatDll();
        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatTrustTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("-config")]
        [InlineData("--h")]
        public void Trust_UnrecognizedOption_Fails(string unrecognizedOption)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange & Act
                CommandRunnerResult result = CommandRunner.Run(
                    DotnetCli,
                    Directory.GetCurrentDirectory(),
                    $"{XplatDll} trust {unrecognizedOption}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.True(result.AllOutput.Contains($"Unrecognized option '{unrecognizedOption}'"));
            }
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--algorithm")]
        [InlineData("--allow-untrusted-root")]
        [InlineData("--owners")]
        public void Trust_RecognizedOption_MissingValue_WrongCombination_Fails(string unrecognizedOption)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange & Act
                CommandRunnerResult result = CommandRunner.Run(
                    DotnetCli,
                    Directory.GetCurrentDirectory(),
                    $"{XplatDll} trust {unrecognizedOption}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.False(string.IsNullOrEmpty(result.AllOutput));
            }
        }

        [Theory]
        [InlineData("trust")]
        [InlineData("trust list")]
        public void Trust_List_Empty_Succeeds(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange
                var mockPackagesDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"packages"));

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    DotnetCli,
                    mockPackagesDirectory.FullName,
                    $"{XplatDll} {args}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                DotnetCliUtil.VerifyResultSuccess(result, "There are no trusted signers.");
            }
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
        public void Trust_VerbosityOption(string option, string verbosity, LogLevel logLevel)
        {
            TrustCommandArgs(
                (testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string> { "trust", "list", option, verbosity };

                    // Act
                    int result = testApp.Execute(argList.ToArray());

                    // Assert
                    Assert.Equal(logLevel, getLogLevel());
                    Assert.Equal(0, result);
                });
        }

        private void TrustCommandArgs(Action<CommandLineApplication, Func<LogLevel>> verify)
        {
            // Arrange
            var logLevel = LogLevel.Information;
            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var testApp = new CommandLineApplication();

            testApp.Name = "dotnet nuget_test";
            TrustedSignersCommand.Register(testApp,
                () => logger,
                ll => logLevel = ll);

            // Act & Assert
            verify(testApp, () => logLevel);
        }
    }
}
