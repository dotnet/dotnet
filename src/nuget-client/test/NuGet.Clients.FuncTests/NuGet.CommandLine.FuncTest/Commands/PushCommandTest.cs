// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.CommandLine.Test;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.FuncTest.Commands
{
    public class PushCommandTest
    {
        private const string MESSAGE_EXISTING_PACKAGE = "already exists at feed"; //Derived from resx: AddPackage_PackageAlreadyExists
        private const string MESSAGE_RESPONSE_NO_SUCCESS = "Response status code does not indicate success";
        private const string MESSAGE_PACKAGE_PUSHED = "Your package was pushed.";
        private const string TEST_PACKAGE_SHOULD_NOT_PUSH = "The package should not have been pushed";
        private const string TEST_PACKAGE_SHOULD_PUSH = "The package should have been pushed";
        private const string ADVERTISE_SKIPDUPLICATE_OPTION = "To skip already published packages, use the option -SkipDuplicate"; //PushCommandSkipDuplicateAdvertiseNuGetExe
        private const string WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST = "File does not exist";
        private const string MESSAGE_FILE_DOES_NOT_EXIST = WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST + " ({0})";
        private readonly ITestOutputHelper _testOutputHelper;

        public PushCommandTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// 100 seconds is significant because that is the default timeout on <see cref="HttpClient"/>.
        /// Related to https://github.com/NuGet/Home/issues/2785.
        /// </summary>
        [Fact(Skip = "https://github.com/NuGet/Home/issues/13843")]
        public void PushCommand_AllowsTimeoutToBeSpecifiedHigherThan100Seconds()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();
                var sourcePath = Util.CreateTestPackage("PackageA", "1.1.0", pathContext.WorkingDirectory);
                var outputPath = Path.Combine(pathContext.WorkingDirectory, "pushed.nupkg");
                CommandRunnerResult result = null;

                using (var server = new MockServer())
                {
                    server.Put.Add("/push", r =>
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(101));

                        byte[] buffer = MockServer.GetPushedPackage(r);
                        using (var outputStream = new FileStream(outputPath, FileMode.Create))
                        {
                            outputStream.Write(buffer, 0, buffer.Length);
                        }

                        return HttpStatusCode.Created;
                    });
                    pathContext.Settings.AddSource("http-feed", $"{server.Uri}push", "true");
                    server.Start();

                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath} -Source {server.Uri}push -Timeout 110",
                        timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }
                // Assert
                Assert.True(result.Success, $"{result.Output} {result.Errors}");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.True(File.Exists(outputPath), TEST_PACKAGE_SHOULD_PUSH);
                Assert.Equal(File.ReadAllBytes(sourcePath), File.ReadAllBytes(outputPath));
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13843")]
        public void PushCommand_AllowsTimeoutToBeSpecifiedLowerThan100Seconds()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();
                var sourcePath = Util.CreateTestPackage("PackageA", "1.1.0", pathContext.WorkingDirectory);
                var outputPath = Path.Combine(pathContext.WorkingDirectory, "pushed.nupkg");
                CommandRunnerResult result = null;

                using (var server = new MockServer())
                {
                    server.Put.Add("/push", r =>
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(5));

                        byte[] buffer = MockServer.GetPushedPackage(r);
                        using (var outputStream = new FileStream(outputPath, FileMode.Create))
                        {
                            outputStream.Write(buffer, 0, buffer.Length);
                        }

                        return HttpStatusCode.Created;
                    });
                    pathContext.Settings.AddSource("http-feed", $"{server.Uri}push", "true");
                    server.Start();

                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath} -Source {server.Uri}push -Timeout 1",
                        timeOutInMilliseconds: 20 * 1000,
                        testOutputHelper: _testOutputHelper); // 20 seconds
                }

                // Assert
                Assert.False(result.Success, $"{result.Output} {result.Errors}");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.False(File.Exists(outputPath), TEST_PACKAGE_SHOULD_NOT_PUSH);
            }
        }

        [Fact]
        public void PushCommand_Server_SkipDuplicate_NotSpecified_PushHalts()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();
                var sourcePath = Util.CreateTestPackage("PackageA", "1.1.0", pathContext.WorkingDirectory);
                var outputPath = Path.Combine(pathContext.WorkingDirectory, "pushed.nupkg");

                var sourcePath2 = Util.CreateTestPackage("PackageB", "1.1.0", pathContext.WorkingDirectory);
                var outputPath2 = Path.Combine(pathContext.WorkingDirectory, "pushed2.nupkg");

                CommandRunnerResult result = null;
                CommandRunnerResult result2 = null;
                CommandRunnerResult result3 = null;

                using (var server = new MockServer())
                {
                    SetupMockServerForSkipDuplicate(server,
                                                      FuncOutputPath_SwitchesOnThirdPush(outputPath, outputPath2),
                                                      FuncStatusDuplicate_OccursOnSecondPush());

                    server.Start();
                    pathContext.Settings.AddSource("http-feed", $"{server.Uri}push", "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath} -Source {server.Uri}push -Timeout 110",
                        timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds

                    //Run again so that it will be a duplicate push.
                    result2 = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath} -Source {server.Uri}push -Timeout 110",
                        timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds

                    result3 = CommandRunner.Run(
                       nuget,
                       pathContext.WorkingDirectory,
                       $"push {sourcePath2} -Source {server.Uri}push -Timeout 110",
                       timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert
                Assert.True(result.Success, $"{result.Output} {result.Errors}");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.True(File.Exists(outputPath), TEST_PACKAGE_SHOULD_PUSH);
                Assert.DoesNotContain(MESSAGE_RESPONSE_NO_SUCCESS, result.AllOutput);
                Assert.DoesNotContain(MESSAGE_EXISTING_PACKAGE, result.AllOutput);
                Assert.Equal(File.ReadAllBytes(sourcePath), File.ReadAllBytes(outputPath));

                // Second run of command is the duplicate.
                Assert.False(result2.Success, result2.AllOutput);
                Assert.Contains(MESSAGE_RESPONSE_NO_SUCCESS, result2.AllOutput);
                Assert.DoesNotContain(MESSAGE_EXISTING_PACKAGE, result2.AllOutput);
                Assert.Contains(ADVERTISE_SKIPDUPLICATE_OPTION, result2.AllOutput);
                Assert.Equal(File.ReadAllBytes(sourcePath), File.ReadAllBytes(outputPath));
            }
        }

        [Fact]
        public void PushCommand_Server_SkipDuplicate_IsSpecified_PushProceeds()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();
                var sourcePath = Util.CreateTestPackage("PackageA", "1.1.0", pathContext.WorkingDirectory);
                var outputPath = Path.Combine(pathContext.WorkingDirectory, "pushed.nupkg");

                var sourcePath2 = Util.CreateTestPackage("PackageB", "1.1.0", pathContext.WorkingDirectory);
                var outputPath2 = Path.Combine(pathContext.WorkingDirectory, "pushed2.nupkg");

                CommandRunnerResult result = null;
                CommandRunnerResult result2 = null;
                CommandRunnerResult result3 = null;

                using (var server = new MockServer())
                {
                    SetupMockServerForSkipDuplicate(server,
                                                      FuncOutputPath_SwitchesOnThirdPush(outputPath, outputPath2),
                                                      FuncStatusDuplicate_OccursOnSecondPush());
                    pathContext.Settings.AddSource("http-feed", $"{server.Uri}push", "true");
                    server.Start();

                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath} -Source {server.Uri}push -Timeout 110 -SkipDuplicate",
                        timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds

                    //Run again so that it will be a duplicate push but use the option to skip duplicate packages.
                    result2 = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath} -Source {server.Uri}push -Timeout 110 -SkipDuplicate",
                        timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds

                    //Third run with a different package.
                    result3 = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {sourcePath2} -Source {server.Uri}push -Timeout 110 -SkipDuplicate",
                        timeOutInMilliseconds: 120 * 1000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert
                Assert.True(result.Success, $"{result.Output} {result.Errors}");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result.AllOutput);
                Assert.True(File.Exists(outputPath), TEST_PACKAGE_SHOULD_PUSH);
                Assert.DoesNotContain(MESSAGE_RESPONSE_NO_SUCCESS, result.AllOutput);
                Assert.Equal(File.ReadAllBytes(sourcePath), File.ReadAllBytes(outputPath));

                // Second run of command is the duplicate.
                Assert.True(result2.Success, result2.AllOutput);
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result2.AllOutput);
                Assert.Contains(MESSAGE_EXISTING_PACKAGE, result2.AllOutput);
                Assert.DoesNotContain(MESSAGE_RESPONSE_NO_SUCCESS, result2.AllOutput);

                // Third run after a duplicate should be successful with the SkipDuplicate flag.
                Assert.True(result3.Success, $"{result3.Output} {result3.Errors}");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result3.AllOutput);
                Assert.True(File.Exists(outputPath2), TEST_PACKAGE_SHOULD_PUSH);

                Assert.Equal(File.ReadAllBytes(sourcePath2), File.ReadAllBytes(outputPath2));
            }
        }

        /// <summary>
        /// When pushing a snupkg filename that doesn't exist, show a File Not Found error.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Snupkg_ByFilename_DoesNotExist_FileNotFoundError()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();
                string snupkgToPush = "nonExistingPackage.snupkg";
                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {snupkgToPush} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                string expectedFileNotFoundErrorMessage = string.Format(MESSAGE_FILE_DOES_NOT_EXIST, snupkgToPush);

                Assert.False(result.Success, "File did not exist and should fail.");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.Contains(expectedFileNotFoundErrorMessage, result.Errors);
            }
        }

        /// <summary>
        /// When pushing a snupkg wildcard where no matching files exist, show a File Not Found error.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Snupkg_ByWildcard_FindsNothing_FileNotFoundError()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {

                var nuget = Util.GetNuGetExePath();
                string snupkgToPush = "*.snupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {snupkgToPush} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                //Assert
                string expectedFileNotFoundErrorMessage = string.Format(MESSAGE_FILE_DOES_NOT_EXIST, snupkgToPush);
                Assert.False(result.Success, "File did not exist and should fail.");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.Contains(expectedFileNotFoundErrorMessage, result.Errors);
            }
        }

        /// <summary>
        /// When pushing a nupkg by filename where no matching files exist, show a File Not Found error.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByFilename_FindsNothing_FileNotFoundError()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {

                var nuget = Util.GetNuGetExePath();
                string nupkgToPush = "filename.nupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {nupkgToPush} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                //Assert
                string expectedFileNotFoundErrorMessage = string.Format(MESSAGE_FILE_DOES_NOT_EXIST, nupkgToPush);
                Assert.False(result.Success, "File did not exist and should fail.");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.Contains(expectedFileNotFoundErrorMessage, result.Errors);
            }
        }

        /// <summary>
        /// When pushing a nupkg by wildcard where no matching files exist, show a File Not Found error.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByWildcard_FindsNothing_FileNotFoundError()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {

                var nuget = Util.GetNuGetExePath();
                string nupkgToPush = "*.nupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {nupkgToPush} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                //Assert
                string expectedFileNotFoundErrorMessage = string.Format(MESSAGE_FILE_DOES_NOT_EXIST, nupkgToPush);
                Assert.False(result.Success, "File did not exist and should fail.");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.Contains(expectedFileNotFoundErrorMessage, result.Errors);
            }
        }

        /// <summary>
        /// When pushing a nupkg by filename to a Symbol Server with no matching snupkg, do not show a File Not Found error.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByFilename_SnupkgDoesNotExist_NoFileNotFoundError()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "packageWithoutSnupkg";
                string version = "1.1.0";

                //Create Nupkg in test directory.
                string nupkgFullPath = Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);

                string nupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    SetupMockServerAlwaysCreate(server);
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {nupkgFullPath} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                Assert.True(result.Success, "Expected to successfully push a nupkg without a snupkg.");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.DoesNotContain(WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST, result.Errors);
            }
        }

        /// <summary>
        /// When pushing *.nupkg to a symbol server, but no snupkgs are selected with that wildcard, there is not a FileNotFound error about snupkgs.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByWildcard_SnupkgDoesNotExist_NoFileNotFoundError()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "packageWithoutSnupkg";
                string version = "1.1.0";

                //Create Nupkg in test directory.
                string nupkgFullPath = Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);

                string pushArgument = "*.nupkg";
                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    SetupMockServerAlwaysCreate(server);
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {pushArgument} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                string expectedFileNotFoundErrorMessage = string.Format(MESSAGE_FILE_DOES_NOT_EXIST, pushArgument);

                Assert.True(result.Success, "Snupkg File did not exist but should not fail a nupkg push.\n\n" + result.AllOutput);
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result.Output);
                Assert.DoesNotContain(WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST, result.Errors);
                Assert.DoesNotContain(NuGetConstants.SnupkgExtension, result.AllOutput); //Snupkgs should not be mentioned.
            }
        }

        /// <summary>
        /// When pushing a nupkg by filename to a Symbol Server with a matching snupkg, a 409 Conflict halts the push.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByFilename_SnupkgExists_Conflict()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "packageWithSnupkg";

                //Create nupkg in test directory.
                string version = "1.1.0";
                string nupkgFullPath = Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);
                //Create snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath);

                CommandRunnerResult result = null;
                CommandRunnerResult result2 = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    //Configure push to alternate returning Created and Conflict responses, which correspond to pushing the nupkg and snupkg, respectively.
                    SetupMockServerCreateNupkgDuplicateSnupkg(server, pathContext.WorkingDirectory, FuncStatus_Alternates_CreatedAndDuplicate());
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act

                    //Since this is V3, this will trigger 2 pushes: one for nupkgs, and one for snupkgs.
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {nupkgFullPath} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds

                    //Second run with SkipDuplicate
                    result2 = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {nupkgFullPath} -Source {sourceName} -Timeout 110 -SkipDuplicate",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                //Ignoring filename in File Not Found error since the error should not appear in any case.
                string genericFileNotFoundError = WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST;

                //Nupkg should push, but corresponding snupkg is a duplicate and errors.
                Assert.False(result.Success, "Expected to fail push a due to duplicate snupkg.");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result.Output); //nupkg pushed
                Assert.Contains(MESSAGE_RESPONSE_NO_SUCCESS, result.AllOutput); //snupkg duplicate
                Assert.DoesNotContain(MESSAGE_EXISTING_PACKAGE, result.AllOutput);
                Assert.DoesNotContain(genericFileNotFoundError, result.Errors);

                //Nupkg should push, and corresponding snupkg is a duplicate which is skipped.
                Assert.True(result2.Success, "Expected to successfully push with SkipDuplicate option and a duplicate snupkg.");
                Assert.Contains(MESSAGE_PACKAGE_PUSHED, result2.Output); //nupkg pushed
                Assert.DoesNotContain(MESSAGE_RESPONSE_NO_SUCCESS, result2.AllOutput); //snupkg duplicate
                Assert.Contains(MESSAGE_EXISTING_PACKAGE, result2.AllOutput);
                Assert.DoesNotContain(genericFileNotFoundError, result2.Errors);
            }
        }

        /// <summary>
        /// When pushing *.Nupkg, (no skip duplicate) a 409 Conflict is returned and halts the secondary symbols push.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByWildcard_FindsMatchingSnupkgs_Conflict()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "packageWithSnupkg";

                //Create a nupkg in test directory.
                string version = "1.1.0";
                Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);
                //Create snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath);

                string wildcardPush = "*.nupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    //Configure push to return a Conflict for the first push, then Created for all remaining pushes.
                    SetupMockServerCreateNupkgDuplicateSnupkg(server, pathContext.WorkingDirectory, FuncStatus_Duplicate_ThenAlwaysCreated());
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act

                    //Since this is V3, this will trigger 2 pushes: one for nupkgs, and one for snupkgs.
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {wildcardPush} -Source {sourceName} -SymbolSource {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                //Ignoring filename in File Not Found error since the error should not appear in any case.
                string genericFileNotFoundError = WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST;

                //Nupkg should be a conflict, so its snupkg should also not push.
                Assert.False(result.Success, "Expected to fail the push due to a duplicate nupkg.");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.AllOutput); //nothing pushed
                Assert.Contains(MESSAGE_RESPONSE_NO_SUCCESS, result.Errors); //nupkg duplicate
                Assert.DoesNotContain(genericFileNotFoundError, result.Errors);
                Assert.DoesNotContain(".snupkg", result.AllOutput); //snupkg not mentioned
            }
        }

        /// <summary>
        /// When pushing *.Nupkg with SkipDuplicate, a 409 Conflict is ignored and the corresponding symbols push is skipped.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Nupkg_ByWildcard_FindsMatchingSnupkgs_SkipDuplicate()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "packageWithSnupkg";

                //Create a nupkg in test directory.
                string version = "1.1.0";
                Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);
                //Create snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath);

                //Create another nupkg in test directory.
                version = "2.12.1";
                Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName2 = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName2 = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath2 = Path.Combine(pathContext.WorkingDirectory, snupkgFileName2);
                //Create another snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath2);

                string wildcardPush = "*.nupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    SetupMockServerAlwaysDuplicate(server);
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act

                    //Since this is V3, this will trigger 2 pushes: one for nupkgs, and one for snupkgs.
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {wildcardPush} -Source {sourceName} -Timeout 110 -SkipDuplicate",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                //Ignoring filename in File Not Found error since the error should not appear in any case.
                string genericFileNotFoundError = WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST;

                //Nupkg should be an ignored conflict, so its snupkg shouldn't push.
                Assert.True(result.Success, "Expected to skip pushing a snupkg with SkipDuplicate option when the nupkg is a duplicate.\n\n" + result.AllOutput);
                Assert.DoesNotContain(MESSAGE_RESPONSE_NO_SUCCESS, result.Errors); //nupkg duplicate
                Assert.Contains(MESSAGE_EXISTING_PACKAGE, result.AllOutput);
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.AllOutput); //nothing is pushed since nupkg/snupkgs are all skipped duplicates
                Assert.DoesNotContain(genericFileNotFoundError, result.Errors);

                Assert.DoesNotContain(snupkgFileName, result.AllOutput); //first snupkg is not attempted since nupkg was duplicate.
                Assert.DoesNotContain(snupkgFileName2, result.AllOutput); //second snupkg is not attempted since nupkg was duplicate.
            }
        }

        /// <summary>
        /// When pushing *.Snupkg, (no skip duplicate) a 409 Conflict is returned and halts the remaining symbols push.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Snupkg_ByWildcard_FindsMatchingSnupkgs_Conflict()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "symbolsPackage";

                //Create a nupkg in test directory.
                string version = "1.1.0";
                Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);

                //Create snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath);

                string wildcardPush = "*.snupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    //Configure push to return a Conflict for the first push, then Created for all remaining pushes.
                    SetupMockServerCreateNupkgDuplicateSnupkg(server, pathContext.WorkingDirectory, FuncStatus_Duplicate_ThenAlwaysCreated());
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act

                    //Since this is V3, this will trigger 2 pushes: one for nupkgs, and one for snupkgs.
                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {wildcardPush} -Source {sourceName} -Timeout 110",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert

                //Ignoring filename in File Not Found error since the error should not appear in any case.
                string genericFileNotFoundError = WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST;

                //Nupkg should be a conflict, so its snupkg should also not push.
                Assert.False(result.Success, "Expected to fail the push due to a duplicate snupkg.");
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output); //nothing pushed
                Assert.Contains(MESSAGE_RESPONSE_NO_SUCCESS, result.Errors); //nupkg duplicate
                Assert.DoesNotContain(genericFileNotFoundError, result.Errors);
                Assert.DoesNotContain(nupkgFileName, result.AllOutput); //nupkg not mentioned
            }
        }

        /// <summary>
        /// When pushing *.Snupkg with SkipDuplicate, a 409 Conflict is ignored and the remaining symbols push proceeds.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Snupkg_ByWildcard_FindsMatchingSnupkgs_SkipDuplicate()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string packageId = "packageWithSnupkg";

                //Create a nupkg in test directory.
                string version = "1.1.0";
                Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);
                //Create snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath);

                //Create another nupkg in test directory.
                version = "2.12.1";
                Util.CreateTestPackage(packageId, version, pathContext.WorkingDirectory);
                string nupkgFileName2 = Util.BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
                string snupkgFileName2 = Util.BuildPackageString(packageId, version, NuGetConstants.SnupkgExtension);
                string snupkgFullPath2 = Path.Combine(pathContext.WorkingDirectory, snupkgFileName2);
                //Create another snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath2);

                string wildcardPush = "*.snupkg";

                CommandRunnerResult result = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    SetupMockServerAlwaysDuplicate(server);
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act

                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {wildcardPush} -Source {sourceName} -Timeout 110 -SkipDuplicate",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }
                // Assert

                //Ignoring filename in File Not Found error since the error should not appear in any case.
                string genericFileNotFoundError = WITHOUT_FILENAME_MESSAGE_FILE_DOES_NOT_EXIST;

                //Nupkg and Snupkg duplicates should be an ignored conflicts, so its all snupkg should be attempted.
                Assert.True(result.Success, "Expected to successfully push all snupkgs with SkipDuplicate option when the snupkgs are duplicates.");
                Assert.DoesNotContain(MESSAGE_RESPONSE_NO_SUCCESS, result.Errors); //snupkg duplicate is ignored
                Assert.DoesNotContain(MESSAGE_PACKAGE_PUSHED, result.Output); //snupkgFileName and snupkgFileName2 are not pushed (just skipped conflicts)
                Assert.Contains(MESSAGE_EXISTING_PACKAGE, result.AllOutput);

                Assert.Contains(snupkgFileName, result.AllOutput); //first snupkg push is attempted
                Assert.Contains(snupkgFileName2, result.AllOutput); //second snupkg push is attempted

                Assert.DoesNotContain(genericFileNotFoundError, result.Errors);
                Assert.DoesNotContain(nupkgFileName, result.AllOutput); //nupkgs should not be attempted in push
                Assert.DoesNotContain(nupkgFileName2, result.AllOutput); //nupkgs should not be attempted in push
            }
        }


        /// <summary>
        /// When pushing a snupkg, a 409 Conflict is returned and any message from the server is shown appropriately.
        /// </summary>
        [Fact]
        public void PushCommand_Server_Snupkg_ByFilename_SnupkgExists_Conflict_ServerMessage()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = new SimpleTestPathContext())
            {
                var nuget = Util.GetNuGetExePath();

                string snupkgFileName = "fileName.snupkg";
                string snupkgFullPath = Path.Combine(pathContext.WorkingDirectory, snupkgFileName);
                //Create snupkg in test directory.
                WriteSnupkgFile(snupkgFullPath);

                CommandRunnerResult result = null;
                CommandRunnerResult result2 = null;

                using (var server = CreateAndStartMockV3Server(pathContext.WorkingDirectory, out string sourceName))
                {
                    SetupMockServerAlwaysDuplicate(server);
                    pathContext.Settings.AddSource(sourceName, sourceName, "true");
                    // Act

                    result = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {snupkgFileName} -Source {sourceName} -Timeout 110 -Verbosity detailed",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds

                    result2 = CommandRunner.Run(
                        nuget,
                        pathContext.WorkingDirectory,
                        $"push {snupkgFileName} -Source {sourceName} -Timeout 110 -SkipDuplicate -Verbosity detailed",
                        timeOutInMilliseconds: 120000,
                        testOutputHelper: _testOutputHelper); // 120 seconds
                }

                // Assert
                Assert.False(result.Success, "Expected a Duplicate response to fail the push.");
                Assert.Contains("Conflict", result.AllOutput);

                Assert.True(result2.Success, "Expected a Duplicate response to be skipped resulting in a successful push.");
                Assert.Contains("Conflict", result2.AllOutput);
            }
        }

        [Fact]
        public void PushCommand_WhenPushingToAnHttpServerV3_WithSymbols_Errors()
        {
            // Arrange
            using var packageDirectory = TestDirectory.Create();
            var nuget = Util.GetNuGetExePath();
            string snupkgFileName = "fileName.snupkg";
            string snupkgFullPath = Path.Combine(packageDirectory, snupkgFileName);
            //Create snupkg in test directory.
            WriteSnupkgFile(snupkgFullPath);

            CommandRunnerResult result = null;
            using var server = CreateAndStartMockV3Server(packageDirectory, out string sourceName);
            SetupMockServerAlwaysCreate(server);
            // Act
            result = CommandRunner.Run(
                nuget,
                packageDirectory,
                $"push {snupkgFileName} -Source {sourceName} -Timeout 110 -Verbosity detailed",
                timeOutInMilliseconds: 120000,
                testOutputHelper: _testOutputHelper); // 120 seconds

            // Assert
            Assert.False(result.Success, result.AllOutput);
            Assert.Contains(sourceName, result.Errors);
        }

        #region Helpers
        /// <summary>
        /// Sets up the server for the steps of running 3 Push commands. First is the initial push, followed by a duplicate push, followed by a new package push.
        /// Depending on the options of the push, the duplicate will either be a warning or an error and permit or prevent the third push.
        /// </summary>
        /// <param name="server">Server object to modify.</param>
        /// <param name="outputPathFunc">Function to determine path to output package.</param>
        /// <param name="responseCodeFunc">Function to determine which HttpStatusCode to return.</param>
        private static void SetupMockServerForSkipDuplicate(MockServer server,
                                                              Func<int, string> outputPathFunc,
                                                              Func<int, HttpStatusCode> responseCodeFunc)
        {
            int packageCounter = 0;
            server.Put.Add("/push", (Func<HttpListenerRequest, object>)((r) =>
            {
                packageCounter++;
                var outputPath = outputPathFunc(packageCounter);

                MockServer.SavePushedPackage(r, outputPath);

                return responseCodeFunc(packageCounter);
            }));
        }

        private static void SetupMockServerAlwaysDuplicate(MockServer server)
        {
            server.Put.Add("/push", (Func<HttpListenerRequest, object>)((r) =>
            {
                return HttpStatusCode.Conflict;
            }));
        }

        private static void SetupMockServerAlwaysCreate(MockServer server)
        {
            server.Put.Add("/push", (Func<HttpListenerRequest, object>)((r) =>
            {
                return HttpStatusCode.Created;
            }));
        }


        private static void WriteSnupkgFile(string snupkgFullPath)
        {
            FileStream fileSnupkg = null;
            try
            {
                fileSnupkg = File.Create(snupkgFullPath);
            }
            finally
            {
                if (fileSnupkg != null)
                {
                    fileSnupkg.Flush();
                    fileSnupkg.Close();
                }
            }
        }


        private static void SetupMockServerCreateNupkgDuplicateSnupkg(MockServer server,
                                                              string outputPath,
                                                              Func<int, HttpStatusCode> responseCodeFunc)
        {
            int packageCounter = 0;
            server.Put.Add("/push", (Func<HttpListenerRequest, object>)((r) =>
            {
                packageCounter++;
                var statusCode = responseCodeFunc(packageCounter);
                return statusCode;
            }));
        }

        /// <summary>
        /// Switches to the second path on the 3rd count.
        /// </summary>
        private static Func<int, string> FuncOutputPath_SwitchesOnThirdPush(string outputPath, string outputPath2)
        {
            return (count) =>
            {
                if (count >= 3)
                {
                    return outputPath2;
                }
                return outputPath;
            };
        }

        /// <summary>
        /// Status is Created except for 2nd count which is fixed as a Conflict.
        /// </summary>
        private static Func<int, HttpStatusCode> FuncStatusDuplicate_OccursOnSecondPush()
        {
            return (count) =>
            {
                //Second run will be treated as duplicate.
                if (count == 2)
                {
                    return HttpStatusCode.Conflict;
                }
                else
                {
                    return HttpStatusCode.Created;
                }
            };
        }

        /// <summary>
        /// Status alternates between Created and Conflict, (divisible by 2 is a Conflict by default).
        /// </summary>
        private static Func<int, HttpStatusCode> FuncStatus_Alternates_CreatedAndDuplicate(bool startWithCreated = true)
        {
            var firstResponse = startWithCreated ? HttpStatusCode.Created : HttpStatusCode.Conflict;
            var secondResponse = startWithCreated ? HttpStatusCode.Conflict : HttpStatusCode.Created;

            return (count) =>
            {
                //Every second run will be the opposite of the previous run.
                if (count % 2 == 0)
                {
                    return secondResponse;
                }
                else
                {
                    return firstResponse;
                }
            };
        }

        /// <summary>
        /// Status is first Duplicate followed by all Created.
        /// </summary>
        private static Func<int, HttpStatusCode> FuncStatus_Duplicate_ThenAlwaysCreated()
        {
            return (count) =>
            {
                if (count == 1)
                {
                    return HttpStatusCode.Conflict;
                }
                else
                {
                    return HttpStatusCode.Created;
                }
            };
        }

        /// <summary>
        /// Creates a V3 Mock Server that supports Publish and Symbol Server.
        /// </summary>
        /// <param name="packageDirectory">Path where this server should write (eg, nuget.config).</param>
        /// <param name="sourceName">URI for index.json</param>
        /// <returns></returns>
        private static MockServer CreateAndStartMockV3Server(string packageDirectory, out string sourceName)
        {
            var server = new MockServer();
            var indexJson = Util.CreateIndexJson();

            Util.AddPublishResource(indexJson, server);
            server.Get.Add("/", r =>
            {
                var path = server.GetRequestUrlAbsolutePath(r);
                if (path == "/index.json")
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.StatusCode = 200;
                        response.ContentType = "text/javascript";
                        MockServer.SetResponseContent(response, indexJson.ToString());
                    });
                }

                throw new Exception("This test needs to be updated to support: " + path);
            });

            server.Start();

            var sources = new List<string>();
            sourceName = $"{server.Uri}index.json";
            sources.Add(sourceName);

            if (!string.IsNullOrWhiteSpace(packageDirectory))
            {
                Util.CreateNuGetConfig(packageDirectory, sources);
            }
            Util.AddPublishSymbolsResource(indexJson, server);

            return server;
        }

        #endregion
    }
}
