// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class PushCommandTests
    {
        // Skipping linux: https://github.com/NuGet/Home/issues/9299
        [PlatformFact(Platform.Windows, Platform.Darwin)]
        public async Task PushCommand_AbsolutePathSourceAsync()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange (create a test package)
                var packagePushDest = new DirectoryInfo(Path.Combine(workingDir, "packagePushDest"));
                packagePushDest.Create();

                var packageSources = new List<PackageSource>
                {
                    new PackageSource(packagePushDest.FullName)
                };

                var packageInfoCollection = new[]
                {
                    await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test1", "1.0.0"),
                    await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test2", "1.0.0")
                };

                // Act
                await PushRunner.Run(
                    Settings.LoadDefaultSettings(null, null, null),
                    new TestPackageSourceProvider(packageSources),
                    new[] { packageInfoCollection[0].FullName, packageInfoCollection[1].FullName },
                    packagePushDest.FullName,
                    null, // api key
                    null, // symbols source
                    null, // symbols api key
                    0, // timeout
                    false, // disable buffering
                    false, // no symbols,
                    false, // enable server endpoint
                    false, // no skip duplicate
                    false, // allow insecure connections
                    new TestLogger());

                // Assert
                foreach (var packageInfo in packageInfoCollection)
                {
                    var destFile = Path.Combine(packagePushDest.FullName, packageInfo.Name);
                    Assert.True(File.Exists(destFile));
                }
            }
        }

        [Fact]
        public async Task Run_WhenPushingToAnHttpServerWithAllowInsecureConnectionsOptionTrue_Succeeds()
        {
            // Arrange
            using var packageDirectory = TestDirectory.Create();
            var pkgA = new SimpleTestPackageContext("pkgA");
            await pkgA.CreateAsFileAsync(packageDirectory, "pkgA.1.0.0.nupkg");
            var outputFileName = Path.Combine(packageDirectory, "t1.nupkg");

            using var server = new MockServer();
            server.Get.Add("/push", r => "OK");
            server.Put.Add("/push", r =>
            {
                byte[] buffer = MockServer.GetPushedPackage(r);
                using (var of = new FileStream(outputFileName, FileMode.Create))
                {
                    of.Write(buffer, 0, buffer.Length);
                }
                return HttpStatusCode.Created;
            });
            var logger = new TestLogger();
            server.Start();

            // Act
            await PushRunner.Run(
                settings: Settings.LoadDefaultSettings(null, null, null),
                sourceProvider: new TestPackageSourceProvider(new List<PackageSource> { new PackageSource($"{server.Uri}push") }),
                packagePaths: [Path.Combine(packageDirectory.Path, "pkgA.1.0.0.nupkg")],
                source: $"{server.Uri}push",
                apiKey: null,
                symbolSource: null,
                symbolApiKey: null,
                timeoutSeconds: 0,
                disableBuffering: false,
                noSymbols: false,
                noServiceEndpoint: false,
                skipDuplicate: false,
                allowInsecureConnections: true,
                logger);

            // Assert
            logger.ErrorMessages.Should().NotContain(m => m.Contains(string.Format(Strings.Error_HttpSource_Single, "push", $"{server.Uri}push")));
            Assert.True(File.Exists(outputFileName), "The package file should exist after the push operation.");
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/14492")]
        public async Task Run_WhenPushingToAnHttpServerWithAllowInsecureConnectionsOptionFalse_Throws()
        {
            // Arrange
            using var packageDirectory = TestDirectory.Create();
            var outputFileName = Path.Combine(packageDirectory, "t1.nupkg");
            File.WriteAllText(outputFileName, "This is a test package content");

            using var server = new MockServer();
            server.Get.Add("/push", r => "OK");
            server.Put.Add("/push", r =>
            {
                return HttpStatusCode.Created;
            });
            var logger = new TestLogger();
            server.Start();

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await PushRunner.Run(
                    settings: Settings.LoadDefaultSettings(null, null, null),
                    sourceProvider: new TestPackageSourceProvider(new List<PackageSource> { new PackageSource($"{server.Uri}push") }),
                    packagePaths: new[] { outputFileName },
                    source: $"{server.Uri}push",
                    apiKey: null,
                    symbolSource: null,
                    symbolApiKey: null,
                    timeoutSeconds: 0,
                    disableBuffering: false,
                    noSymbols: false,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    allowInsecureConnections: false,
                    logger);
            });

            // Assert
            result.Message.Should().Contain(string.Format(Strings.Error_HttpSource_Single, "push", $"{server.Uri}push"));
        }
    }
}
