// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatLocalsTests
    {
        private static readonly string DotnetCli = TestFileSystemUtility.GetDotnetCli();
        private static readonly string XplatDll = DotnetCliUtil.GetXplatDll();
        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatLocalsTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("locals all --list")]
        [InlineData("locals all -l")]
        [InlineData("locals --list all")]
        [InlineData("locals -l all")]
        [InlineData("locals http-cache --list")]
        [InlineData("locals http-cache -l")]
        [InlineData("locals --list http-cache")]
        [InlineData("locals -l http-cache")]
        [InlineData("locals temp --list")]
        [InlineData("locals temp -l")]
        [InlineData("locals --list temp")]
        [InlineData("locals -l temp")]
        [InlineData("locals global-packages --list")]
        [InlineData("locals global-packages -l")]
        [InlineData("locals --list global-packages")]
        [InlineData("locals -l global-packages")]
        [InlineData("locals plugins-cache --list")]
        [InlineData("locals plugins-cache -l")]
        [InlineData("locals --list plugins-cache")]
        [InlineData("locals -l plugins-cache")]
        public void Locals_List_Succeeds(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange
                var mockGlobalPackagesDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"global-packages"));
                var mockHttpCacheDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"http-cache"));
                var mockTmpCacheDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"temp"));
                var mockPluginsCacheDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"plugins-cache"));

                DotnetCliUtil.CreateTestFiles(mockGlobalPackagesDirectory.FullName);
                DotnetCliUtil.CreateTestFiles(mockHttpCacheDirectory.FullName);
                DotnetCliUtil.CreateTestFiles(mockTmpCacheDirectory.FullName);
                DotnetCliUtil.CreateTestFiles(mockPluginsCacheDirectory.FullName);

                // Act
                var result = CommandRunner.Run(
                    DotnetCli,
                    mockBaseDirectory,
                    $"{XplatDll} {args}",
                    environmentVariables: new Dictionary<string, string>
                    {
                        { "NUGET_PACKAGES", mockGlobalPackagesDirectory.FullName },
                        { "NUGET_HTTP_CACHE_PATH", mockHttpCacheDirectory.FullName },
                        { "NUGET_PLUGINS_CACHE_PATH", mockPluginsCacheDirectory.FullName },
                        { RuntimeEnvironmentHelper.IsWindows ? "TMP" : "TMPDIR", mockTmpCacheDirectory.FullName }
                    },
                    testOutputHelper: _testOutputHelper);
                // Unix uses TMPDIR as environment variable as opposed to TMP on windows

                // Assert
                DotnetCliUtil.VerifyResultSuccess(result, string.Empty);
            }
        }

        [Theory]
        [InlineData("locals --clear all")]
        [InlineData("locals -c all")]
        [InlineData("locals http-cache --clear")]
        [InlineData("locals http-cache -c")]
        [InlineData("locals --clear http-cache")]
        [InlineData("locals -c http-cache")]
        [InlineData("locals temp --clear")]
        [InlineData("locals temp -c")]
        [InlineData("locals --clear temp")]
        [InlineData("locals -c temp")]
        [InlineData("locals global-packages --clear")]
        [InlineData("locals global-packages -c")]
        [InlineData("locals --clear global-packages")]
        [InlineData("locals -c global-packages")]
        [InlineData("locals -c plugins-cache")]
        [InlineData("locals --clear plugins-cache")]
        [InlineData("locals plugins-cache --clear")]
        [InlineData("locals plugins-cache -c")]
        public void Locals_Clear_Succeeds(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Arrange
                var mockGlobalPackagesDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"global-packages"));
                var mockHttpCacheDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"http-cache"));
                var mockTmpDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"temp"));
                var mockPluginsCacheDirectory = Directory.CreateDirectory(Path.Combine(mockBaseDirectory.Path, @"plugins-cache"));
                var mockTmpCacheDirectory = Directory.CreateDirectory(Path.Combine(mockTmpDirectory.FullName,
                    RuntimeEnvironmentHelper.IsLinux ? "NuGetScratch" + Environment.UserName : "NuGetScratch"));

                DotnetCliUtil.CreateTestFiles(mockGlobalPackagesDirectory.FullName);
                DotnetCliUtil.CreateTestFiles(mockHttpCacheDirectory.FullName);
                DotnetCliUtil.CreateTestFiles(mockTmpCacheDirectory.FullName);
                DotnetCliUtil.CreateTestFiles(mockPluginsCacheDirectory.FullName);

                var cacheType = args.Split(null)[1].StartsWith("-") ? args.Split(null)[2] : args.Split(null)[1];

                // Act
                var result = CommandRunner.Run(
                    DotnetCli,
                    mockBaseDirectory,
                    $"{XplatDll} {args}",
                    environmentVariables: new Dictionary<string, string>
                    {
                        { "NUGET_PACKAGES", mockGlobalPackagesDirectory.FullName },
                        { "NUGET_HTTP_CACHE_PATH", mockHttpCacheDirectory.FullName },
                        { RuntimeEnvironmentHelper.IsWindows ? "TMP" : "TMPDIR", mockTmpDirectory.FullName },
                        { "NUGET_PLUGINS_CACHE_PATH", mockPluginsCacheDirectory.FullName }
                    },
                    testOutputHelper: _testOutputHelper);
                // Unix uses TMPDIR as environment variable as opposed to TMP on windows

                // Assert
                if (cacheType == "all")
                {
                    DotnetCliUtil.VerifyClearSuccess(mockGlobalPackagesDirectory.FullName);
                    DotnetCliUtil.VerifyClearSuccess(mockHttpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyClearSuccess(mockTmpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyClearSuccess(mockPluginsCacheDirectory.FullName);

                    // Assert clear message
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet global packages folder:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet HTTP cache:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet Temp cache:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet plugins cache:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Local resources cleared.");
                }
                else if (cacheType == "global-packages")
                {
                    // Global packages cache should be cleared
                    DotnetCliUtil.VerifyClearSuccess(mockGlobalPackagesDirectory.FullName);

                    // Http cache and Temp cahce should be untouched
                    DotnetCliUtil.VerifyNoClear(mockHttpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockTmpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockPluginsCacheDirectory.FullName);

                    // Assert clear message
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet global packages folder:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Local resources cleared.");
                }
                else if (cacheType == "http-cache")
                {
                    // Http cache should be cleared
                    DotnetCliUtil.VerifyClearSuccess(mockHttpCacheDirectory.FullName);

                    // Global packages cache and temp cache should be untouched
                    DotnetCliUtil.VerifyNoClear(mockGlobalPackagesDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockTmpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockPluginsCacheDirectory.FullName);

                    // Assert clear message
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet HTTP cache:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Local resources cleared.");
                }
                else if (cacheType == "temp")
                {
                    // Temp cache should be cleared
                    DotnetCliUtil.VerifyClearSuccess(mockTmpCacheDirectory.FullName);

                    // Global packages cache and Http cache should be un touched
                    DotnetCliUtil.VerifyNoClear(mockGlobalPackagesDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockHttpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockPluginsCacheDirectory.FullName);

                    // Assert clear message
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet Temp cache:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Local resources cleared.");
                }
                else if (cacheType == "plugins-cache")
                {
                    DotnetCliUtil.VerifyClearSuccess(mockPluginsCacheDirectory.FullName);

                    // Global packages cache and Http cache should be un touched
                    DotnetCliUtil.VerifyNoClear(mockGlobalPackagesDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockHttpCacheDirectory.FullName);
                    DotnetCliUtil.VerifyNoClear(mockTmpCacheDirectory.FullName);

                    // Assert clear message
                    DotnetCliUtil.VerifyResultSuccess(result, "Clearing NuGet plugins cache:");
                    DotnetCliUtil.VerifyResultSuccess(result, "Local resources cleared.");
                }
            }
        }

        [Theory]
        [InlineData("locals --list")]
        [InlineData("locals -l")]
        [InlineData("locals --clear")]
        [InlineData("locals -c")]
        public void Locals_Success_InvalidArguments_HelpMessage(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            // Arrange
            var expectedResult = "No Cache Type was specified";

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Path.GetDirectoryName(XplatDll),
                $"{XplatDll} {args}",
                testOutputHelper: _testOutputHelper);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals --list unknownResource")]
        [InlineData("locals -l unknownResource")]
        [InlineData("locals --clear unknownResource")]
        [InlineData("locals -c unknownResource")]
        public void Locals_Success_InvalidResourceName_HelpMessage(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            // Arrange
            var expectedResult = "An invalid local resource name was provided. Provide one of the following values: http-cache, temp, global-packages, all.";

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Path.GetDirectoryName(XplatDll),
                $"{XplatDll} {args}",
                testOutputHelper: _testOutputHelper);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals -list")]
        [InlineData("locals -clear")]
        [InlineData("locals --l")]
        [InlineData("locals --c")]
        public void Locals_Success_InvalidFlags_HelpMessage(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            // Arrange
            var expectedResult = $"Unrecognized option '{args.Split(null)[1]}'";

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Path.GetDirectoryName(XplatDll),
                $"{XplatDll} {args}",
                testOutputHelper: _testOutputHelper);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals all")]
        [InlineData("locals http-cache")]
        [InlineData("locals global-packages")]
        [InlineData("locals temp")]
        [InlineData("locals plugins-cache")]
        public void Locals_Success_NoFlags_HelpMessage(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            // Arrange
            var expectedResult = "Please specify an operation i.e. --list or --clear.";

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Path.GetDirectoryName(XplatDll),
                $"{XplatDll} {args}",
                testOutputHelper: _testOutputHelper);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals -c -l all")]
        [InlineData("locals -c -l global-packages")]
        [InlineData("locals -c -l http-cache")]
        [InlineData("locals -c -l temp")]
        [InlineData("locals -c -l plugins-cache")]
        [InlineData("locals --clear --list all")]
        [InlineData("locals --clear --list global-packages")]
        [InlineData("locals --clear --list http-cache")]
        [InlineData("locals --clear --list temp")]
        [InlineData("locals --clear --list plugins-cache")]

        public void Locals_Success_BothFlags_HelpMessage(string args)
        {
            DotnetCli.Should().NotBeNull(because: "Could not locate the dotnet CLI");
            XplatDll.Should().NotBeNull(because: "Could not locate the Xplat dll");

            // Arrange
            var expectedResult = "Both operations, --list and --clear, are not supported in the same command. Please specify only one operation.";

            // Act
            var result = CommandRunner.Run(
                DotnetCli,
                Path.GetDirectoryName(XplatDll),
                $"{XplatDll} {args}",
                testOutputHelper: _testOutputHelper);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }
    }
}
