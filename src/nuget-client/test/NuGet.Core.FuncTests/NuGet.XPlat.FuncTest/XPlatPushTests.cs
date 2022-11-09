// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatPushTests
    {
        [PackageSourceTheory]
        [PackageSourceData(TestSources.MyGet)]
        [PackageSourceData(TestSources.ProGet, Skip = "No such host is known")]
        [PackageSourceData(TestSources.Klondike, Skip = "401 (Invalid API key)")]
        [PackageSourceData(TestSources.NuGetServer, Skip = "No such host is known")]
        public async Task PushToServerSucceeds(PackageSource packageSource)
        {
            // Arrange
            using (var packageDir = TestDirectory.Create())
            using (TestFileSystemUtility.SetCurrentDirectory(packageDir))
            {
                var packageId = "XPlatPushTests.PushToServerSucceeds";
                var packageVersion = "1.0.0";
                var packageFile = await TestPackagesCore.GetRuntimePackageAsync(packageDir, packageId, packageVersion);
                var configFile = XPlatTestUtils.CopyFuncTestConfig(packageDir);
                var log = new TestCommandOutputLogger();

                var apiKey = XPlatTestUtils.ReadApiKey(packageSource.Name);
                Assert.False(string.IsNullOrEmpty(apiKey));

                var pushArgs = new List<string>
                {
                    "push",
                    packageFile.FullName,
                    "--source",
                    packageSource.Source,
                    "--api-key",
                    apiKey,
                    "--interactive"
                };

                // Act
                var exitCode = NuGet.CommandLine.XPlat.Program.MainInternal(pushArgs.ToArray(), log);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains($"PUT {packageSource.Source}", log.ShowMessages());
                Assert.Contains("Your package was pushed.", log.ShowMessages());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.MyGet, Skip = "MyGet is configured to always override duplicates")]
        [PackageSourceData(TestSources.ProGet, Skip = "No such host is known")]
        [PackageSourceData(TestSources.Klondike, Skip = "401 (Invalid API key)")]
        [PackageSourceData(TestSources.NuGetServer, Skip = "No such host is known")]
        public async Task PushToServerWhichRejectsDuplicates_SkipDuplicate_Succeeds(PackageSource packageSource)
        {
            // Arrange
            using (var packageDir = TestDirectory.Create())
            using (TestFileSystemUtility.SetCurrentDirectory(packageDir))
            {
                var packageId = "XPlatPushTests.PushToServerSucceeds";
                var packageVersion = "1.0.0";
                var packageFile = await TestPackagesCore.GetRuntimePackageAsync(packageDir, packageId, packageVersion);
                var configFile = XPlatTestUtils.CopyFuncTestConfig(packageDir);
                var logFirstPush = new TestCommandOutputLogger();
                var logSecondPush = new TestCommandOutputLogger();

                var apiKey = XPlatTestUtils.ReadApiKey(packageSource.Name);
                Assert.False(string.IsNullOrEmpty(apiKey));
                var pushArgs = new List<string>
                {
                    "push",
                    packageFile.FullName,
                    "--source",
                    packageSource.Source,
                    "--api-key",
                    apiKey,
                    "--skip-duplicate"
                };

                // Act
                var exitCodeFirstPush = NuGet.CommandLine.XPlat.Program.MainInternal(pushArgs.ToArray(), logFirstPush);
                var exitCodeSecondPush = NuGet.CommandLine.XPlat.Program.MainInternal(pushArgs.ToArray(), logSecondPush);

                // Assert First Push - it should happen without error.
                var outputMessagesFirstPush = logFirstPush.ShowMessages();
                Assert.Equal(string.Empty, logFirstPush.ShowErrors());
                Assert.Equal(0, exitCodeFirstPush);

                // Assert Second Push - it should happen without error, even though a duplicate is present.
                var outputMessagesSecondPush = logSecondPush.ShowMessages();

                Assert.Equal(string.Empty, logSecondPush.ShowErrors());
                Assert.Equal(0, exitCodeSecondPush);
                Assert.Contains($"PUT {packageSource.Source}", outputMessagesSecondPush);
                Assert.DoesNotContain("already exists at feed", outputMessagesSecondPush);
                Assert.Contains("Your package was pushed.", outputMessagesSecondPush);
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.MyGet)]
        [PackageSourceData(TestSources.Nexus, Skip = "No such host is known")]
        public async Task PushToServerSucceeds_DeleteFirst(PackageSource packageSource)
        {
            // Arrange
            using (var packageDir = TestDirectory.Create())
            using (TestFileSystemUtility.SetCurrentDirectory(packageDir))
            {
                var packageId = "XPlatPushTests.PushToServerSucceeds";
                var packageVersion = "1.0.0";
                var packageFile = await TestPackagesCore.GetRuntimePackageAsync(packageDir, packageId, packageVersion);
                var configFile = XPlatTestUtils.CopyFuncTestConfig(packageDir);
                var log = new TestCommandOutputLogger();

                var apiKey = XPlatTestUtils.ReadApiKey(packageSource.Name);
                Assert.False(string.IsNullOrEmpty(apiKey));

                DeletePackageBeforePush(packageId, packageVersion, packageSource.Source, apiKey);

                var pushArgs = new List<string>
                {
                    "push",
                    packageFile.FullName,
                    "--source",
                    packageSource.Source,
                    "--api-key",
                    apiKey
                };

                // Act
                var exitCode = NuGet.CommandLine.XPlat.Program.MainInternal(pushArgs.ToArray(), log);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains($"PUT {packageSource.Source}", log.ShowMessages());
                Assert.Contains("Your package was pushed.", log.ShowMessages());
            }
        }

        // Tests pushing multiple packages (multiple paths)
        [Fact]
        public async Task PushMultiplePathsToFileSystemSource()
        {
            using (var packageDirectory = TestDirectory.Create())
            using (var source = TestDirectory.Create())
            {
                // Arrange
                var log = new TestCommandOutputLogger();
                var packageInfoCollection = new[]
                {
                    await TestPackagesCore.GetRuntimePackageAsync(packageDirectory, "testPackageA", "1.1.0"),
                    await TestPackagesCore.GetRuntimePackageAsync(packageDirectory, "testPackageB", "1.1.0"),
                };

                var pushArgs = new List<string>
                {
                    "push",
                    packageInfoCollection[0].FullName,
                    packageInfoCollection[1].FullName,
                    "--source",
                    source,
                };

                // Act
                var exitCode = CommandLine.XPlat.Program.MainInternal(pushArgs.ToArray(), log);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);

                foreach (var packageInfo in packageInfoCollection)
                {
                    Assert.Contains($"Pushing {packageInfo.Name}", log.ShowMessages());
                    Assert.True(File.Exists(Path.Combine(source, packageInfo.Name)));
                }
            }
        }

        /// <summary>
        /// This is called when the package must be deleted before being pushed. It's ok if this
        /// fails, maybe the package was never pushed.
        /// </summary>
        private static void DeletePackageBeforePush(string packageId, string packageVersion, string sourceUri, string apiKey)
        {
            var packageUri = $"{sourceUri.TrimEnd('/')}/{packageId}/{packageVersion}";
            var log = new TestCommandOutputLogger();
            var args = new List<string>
            {
                "delete",
                packageId,
                packageVersion,
                "--source",
                sourceUri,
                "--api-key",
                apiKey,
                "--non-interactive"
            };

            var exitCode = NuGet.CommandLine.XPlat.Program.MainInternal(args.ToArray(), log);
            Assert.InRange(exitCode, 0, 1);

            Assert.Contains($"DELETE {packageUri}", log.ShowMessages());

            if (exitCode == 0)
            {
                Assert.Contains($"OK {packageUri}", log.ShowMessages());
            }
            else
            {
                Assert.Contains($"NotFound {packageUri}", log.ShowMessages());
            }
        }
    }
}
