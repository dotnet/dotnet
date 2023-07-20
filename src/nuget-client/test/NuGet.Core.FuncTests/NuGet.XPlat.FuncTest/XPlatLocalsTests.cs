// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatLocalsTests
    {
        private static readonly string DotnetCli = TestFileSystemUtility.GetDotnetCli();
        private static readonly string XplatDll = DotnetCliUtil.GetXplatDll();

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
        public static void Locals_List_Succeeds(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

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
                      Directory.GetCurrentDirectory(),
                      $"{XplatDll} {args}",
                      waitForExit: true,
                    environmentVariables: new Dictionary<string, string>
                    {
                        { "NUGET_PACKAGES", mockGlobalPackagesDirectory.FullName },
                        { "NUGET_HTTP_CACHE_PATH", mockHttpCacheDirectory.FullName },
                        { "NUGET_PLUGINS_CACHE_PATH", mockPluginsCacheDirectory.FullName },
                        { RuntimeEnvironmentHelper.IsWindows ? "TMP" : "TMPDIR", mockTmpCacheDirectory.FullName }
                    });
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
        public static void Locals_Clear_Succeeds(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

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
                      Directory.GetCurrentDirectory(),
                      $"{XplatDll} {args}",
                      waitForExit: true,
                    environmentVariables: new Dictionary<string, string>
                    {
                        { "NUGET_PACKAGES", mockGlobalPackagesDirectory.FullName },
                        { "NUGET_HTTP_CACHE_PATH", mockHttpCacheDirectory.FullName },
                        { RuntimeEnvironmentHelper.IsWindows ? "TMP" : "TMPDIR", mockTmpDirectory.FullName },
                        { "NUGET_PLUGINS_CACHE_PATH", mockPluginsCacheDirectory.FullName }
                    });
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
        public static void Locals_Success_InvalidArguments_HelpMessage(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            // Arrange
            var expectedResult = string.Concat("error: No Cache Type was specified.",
                                               Environment.NewLine,
                                               "error: usage: NuGet locals <all | http-cache | global-packages | temp | plugins-cache> [--clear | -c | --list | -l]",
                                               Environment.NewLine,
                                               "error: For more information, visit https://docs.nuget.org/docs/reference/command-line-reference");

            // Act
            var result = CommandRunner.Run(
              DotnetCli,
              Directory.GetCurrentDirectory(),
              $"{XplatDll} {args}",
              waitForExit: true);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals --list unknownResource")]
        [InlineData("locals -l unknownResource")]
        [InlineData("locals --clear unknownResource")]
        [InlineData("locals -c unknownResource")]
        public static void Locals_Success_InvalidResourceName_HelpMessage(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            // Arrange
            var expectedResult = string.Concat("error: An invalid local resource name was provided. " +
                                               "Provide one of the following values: http-cache, temp, global-packages, all.");

            // Act
            var result = CommandRunner.Run(
              DotnetCli,
              Directory.GetCurrentDirectory(),
              $"{XplatDll} {args}",
              waitForExit: true);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals -list")]
        [InlineData("locals -clear")]
        [InlineData("locals --l")]
        [InlineData("locals --c")]
        public static void Locals_Success_InvalidFlags_HelpMessage(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            // Arrange
            var expectedResult = string.Concat("Specify --help for a list of available options and commands.",
                                               Environment.NewLine, "error: Unrecognized option '", args.Split(null)[1], "'");

            // Act
            var result = CommandRunner.Run(
              DotnetCli,
              Directory.GetCurrentDirectory(),
              $"{XplatDll} {args}",
              waitForExit: true);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }

        [Theory]
        [InlineData("locals all")]
        [InlineData("locals http-cache")]
        [InlineData("locals global-packages")]
        [InlineData("locals temp")]
        [InlineData("locals plugins-cache")]
        public static void Locals_Success_NoFlags_HelpMessage(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            // Arrange
            var expectedResult = string.Concat("error: Please specify an operation i.e. --list or --clear.",
                                               Environment.NewLine,
                                               "error: usage: NuGet locals <all | http-cache | global-packages | temp | plugins-cache> [--clear | -c | --list | -l]",
                                               Environment.NewLine,
                                               "error: For more information, visit https://docs.nuget.org/docs/reference/command-line-reference");

            // Act
            var result = CommandRunner.Run(
              DotnetCli,
              Directory.GetCurrentDirectory(),
              $"{XplatDll} {args}",
              waitForExit: true);

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

        public static void Locals_Success_BothFlags_HelpMessage(string args)
        {
            Assert.NotNull(DotnetCli);
            Assert.NotNull(XplatDll);

            // Arrange
            var expectedResult = string.Concat("error: Both operations, --list and --clear, are not supported in the same command. Please specify only one operation.",
                                               Environment.NewLine,
                                               "error: usage: NuGet locals <all | http-cache | global-packages | temp | plugins-cache> [--clear | -c | --list | -l]",
                                               Environment.NewLine,
                                               "error: For more information, visit https://docs.nuget.org/docs/reference/command-line-reference");

            // Act
            var result = CommandRunner.Run(
              DotnetCli,
              Directory.GetCurrentDirectory(),
              $"{XplatDll} {args}",
              waitForExit: true);

            // Assert
            DotnetCliUtil.VerifyResultFailure(result, expectedResult);
        }
    }
}
