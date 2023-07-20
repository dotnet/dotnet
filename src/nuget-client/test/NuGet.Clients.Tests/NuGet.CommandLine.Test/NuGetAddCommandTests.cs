// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetAddCommandTests
    {
        [Fact]
        public void AddCommand_Fail_NoSourceSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultFailure(result, NuGetResources.AddCommand_SourceNotProvided);
            }
        }

        [Fact]
        public void AddCommand_Success_SpecifiedSource()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);
                Util.VerifyPackageExists(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Success_SpecifiedRelativeSource()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var sourceParamFullPath = Path.Combine(testInfo.WorkingPath, "relativePathOfflineFeed");
                testInfo.Init(sourceParamFullPath);

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    "relativePathOfflineFeed"
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                Util.VerifyPackageExists(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Success_SourceDoesNotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var currentlyNonExistentPath = Path.Combine(TestFileSystemUtility.NuGetTestFolder, Guid.NewGuid().ToString());

                testInfo.Init(currentlyNonExistentPath);

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);

                Util.VerifyPackageExists(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Success_PackageAlreadyExists()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);
                Util.VerifyPackageExists(testInfo.Package, testInfo.SourceParamFolder);

                // Main Act
                result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result, string.Format(
                    NuGetResources.AddCommand_PackageAlreadyExists,
                    testInfo.Package.ToString(),
                    testInfo.SourceParamFolder));
            }
        }

        [Fact]
        public void AddCommand_Fail_PackageAlreadyExistsAndInvalid()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);
                Util.VerifyPackageExists(testInfo.Package, testInfo.SourceParamFolder);

                var versionFolderPathResolver = new VersionFolderPathResolver(testInfo.SourceParamFolder);

                File.Delete(
                    versionFolderPathResolver.GetManifestFilePath(testInfo.Package.Id, testInfo.Package.Version));

                // Main Act
                result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultFailure(result, string.Format(
                    NuGetResources.AddCommand_ExistingPackageInvalid,
                    testInfo.Package.ToString(),
                    testInfo.SourceParamFolder));
            }
        }

        [Fact]
        public void AddCommand_Fail_HttpSource()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init("https://api.nuget.org/v3/index.json");

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                var expectedErrorMessage
                    = string.Format(NuGetResources.Path_Invalid_NotFileNotUnc, testInfo.SourceParamFolder);
                Util.VerifyResultFailure(result, expectedErrorMessage);

                Util.VerifyPackageDoesNotExist(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Fail_NupkgFileDoesNotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var nonExistentPath = Path.Combine(TestFileSystemUtility.NuGetTestFolder, Guid.NewGuid().ToString());

                var args = new string[]
                {
                    "add",
                    nonExistentPath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                var expectedErrorMessage
                    = string.Format(NuGetResources.NupkgPath_NotFound, nonExistentPath);

                Util.VerifyResultFailure(result, expectedErrorMessage);

                Util.VerifyPackageDoesNotExist(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Fail_NupkgFileIsAnHttpLink()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var invalidNupkgFilePath = "http://www.nuget.org/api/v2/package/EntityFramework/5.0.0";
                var args = new string[]
                {
                    "add",
                    invalidNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                var expectedErrorMessage
                    = string.Format(NuGetResources.Path_Invalid_NotFileNotUnc, invalidNupkgFilePath);

                Util.VerifyResultFailure(result, expectedErrorMessage);

                Util.VerifyPackageDoesNotExist(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Fail_NupkgPathIsNotANupkg()
        {
            // Arrange
            using (var tempDirectory = TestDirectory.Create())
            using (var testInfo = new TestInfo())
            {
                var testPackage = new FileInfo(Path.Combine(tempDirectory, "invalidFile.tmp"));

                using (StreamWriter writer = new StreamWriter(testPackage.FullName))
                {
                    writer.WriteLine("bad data");
                }

                testInfo.Init(testPackage);

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                var expectedErrorMessage
                    = string.Format(NuGetResources.NupkgPath_InvalidNupkg, testInfo.RandomNupkgFilePath);

                Util.VerifyResultFailure(result, expectedErrorMessage);

                Util.VerifyPackageDoesNotExist(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Fact]
        public void AddCommand_Fail_CorruptNupkgFile()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            using (var testFolder = TestDirectory.Create())
            {
                var testPackage = new FileInfo(Path.Combine(testFolder, "bad.nupkg"));

                using (StreamWriter writer = new StreamWriter(testPackage.FullName))
                {
                    writer.WriteLine("bad data");
                }

                testInfo.Init(testPackage);

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                var expectedErrorMessage
                    = string.Format(NuGetResources.NupkgPath_InvalidNupkg, testInfo.RandomNupkgFilePath);

                Util.VerifyResultFailure(result, expectedErrorMessage);

                Util.VerifyPackageDoesNotExist(testInfo.Package, testInfo.SourceParamFolder);
            }
        }

        [Theory]
        [InlineData("add nupkgPath -Source srcFolder extraArg")]
        [InlineData("add")]
        [InlineData("add -?")]
        public void AddCommand_Failure_InvalidArguments_HelpMessage(string args)
        {
            Util.TestCommandInvalidArguments(args);
        }

        [Fact]
        public void AddCommand_Success_ExpandSwitch()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.Init();

                var args = new string[]
                {
                    "add",
                    testInfo.RandomNupkgFilePath,
                    "-Source",
                    testInfo.SourceParamFolder,
                    "-Expand"
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args));

                // Assert
                Util.VerifyResultSuccess(result);
                var listOfPackages = new List<PackageIdentity>() { testInfo.Package };
                Util.VerifyExpandedLegacyTestPackagesExist(listOfPackages, testInfo.SourceParamFolder);
            }
        }

        private sealed class TestInfo : IDisposable
        {
            private TestDirectory _sourceDirectory;
            private TestDirectory _randomNupkgDirectory;

            public string NuGetExePath { get; }
            public string SourceParamFolder { get; set; }
            public string RandomNupkgFolder { get { return Path.GetDirectoryName(RandomNupkgFilePath); } }
            public PackageIdentity Package { get; }
            public FileInfo TestPackage { get; set; }
            public string RandomNupkgFilePath { get { return TestPackage.FullName; } }
            public TestDirectory WorkingPath { get; private set; }

            public TestInfo()
            {
                NuGetExePath = Util.GetNuGetExePath();

                WorkingPath = TestDirectory.Create();

                Package = new PackageIdentity("AbCd", new NuGetVersion("1.0.0.0"));
            }

            public void Init()
            {
                _sourceDirectory = TestDirectory.Create();

                Init(_sourceDirectory);
            }

            public void Init(string sourceParamFolder)
            {
                _randomNupkgDirectory = TestDirectory.Create();
                var testPackage = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    _randomNupkgDirectory,
                    Package.Id,
                    Package.Version.ToString());

                Init(sourceParamFolder, testPackage);
            }

            public void Init(FileInfo testPackage)
            {
                _sourceDirectory = TestDirectory.Create();

                Init(_sourceDirectory, testPackage);
            }

            public void Init(string sourceParamFolder, FileInfo testPackage)
            {
                SourceParamFolder = sourceParamFolder;
                TestPackage = testPackage;
            }

            public void Dispose()
            {
                WorkingPath.Dispose();
                TestFileSystemUtility.DeleteRandomTestFolder(SourceParamFolder);
                _sourceDirectory?.Dispose();
                _randomNupkgDirectory?.Dispose();
            }
        }
    }
}
