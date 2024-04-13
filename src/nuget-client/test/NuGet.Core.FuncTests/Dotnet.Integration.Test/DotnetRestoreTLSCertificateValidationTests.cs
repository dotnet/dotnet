// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Test.Utility;
using NuGet.XPlat.FuncTest;
using Test.Utility;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetRestoreTLSCertificateValidationTests
    {
        private readonly DotnetIntegrationTestFixture _msbuildFixture;

        public DotnetRestoreTLSCertificateValidationTests(DotnetIntegrationTestFixture fixture)
        {
            _msbuildFixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withTLSCertificateValidationDisabled_DoesnotThrowException()
        {
            // Arrange
            using var pathContext = _msbuildFixture.CreateSimpleTestPathContext();
            TestDirectory packageSourceDirectory = TestDirectory.Create();
            var packageA100 = new SimpleTestPackageContext("A", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSourceDirectory,
                    PackageSaveMode.Defaultv3,
                    packageA100);
            var projectA = XPlatTestUtils.CreateProject("ProjectA", pathContext, packageA100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer = new SelfSignedCertificateMockServer(packageSourceDirectory);
            var serverTask = tcpListenerServer.StartServerAsync();
            pathContext.Settings.AddSource("https-feed", $"{tcpListenerServer.URI}v3/index.json", "disableTLSCertificateValidation", "true");

            // Act & Assert
            _msbuildFixture.RunDotnetExpectSuccess(workingDirectory, $"restore {projectA.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath}");
            tcpListenerServer.StopServer();
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withTLSCertificateValidationEnabled_ThrowException()
        {
            // Arrange
            using var pathContext = _msbuildFixture.CreateSimpleTestPathContext();
            TestDirectory packageSourceDirectory = TestDirectory.Create();
            var packageB100 = new SimpleTestPackageContext("myPackg", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSourceDirectory,
                    PackageSaveMode.Defaultv3,
                    packageB100);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, packageB100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectB.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer = new SelfSignedCertificateMockServer(packageSourceDirectory);
            var serverTask = tcpListenerServer.StartServerAsync();
            pathContext.Settings.AddSource("https-feed", $"{tcpListenerServer.URI}v3/index.json");

            // Act & Assert
            var _result = _msbuildFixture.RunDotnetExpectFailure(workingDirectory, $"restore {projectB.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath} -v d");
            tcpListenerServer.StopServer();
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withAnotherSourceTLSCertificateValidationDisbaled_ThrowException()
        {
            // Arrange
            using var pathContext = _msbuildFixture.CreateSimpleTestPathContext();
            TestDirectory packageSourceDirectory = TestDirectory.Create();
            var packageB100 = new SimpleTestPackageContext("myPackg", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   packageSourceDirectory,
                    PackageSaveMode.Defaultv3,
                    packageB100);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, packageB100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectB.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer1 = new SelfSignedCertificateMockServer(packageSourceDirectory);
            SelfSignedCertificateMockServer tcpListenerServer2 = new SelfSignedCertificateMockServer(packageSourceDirectory);
            var serverTask = tcpListenerServer1.StartServerAsync();
            var serverTask2 = tcpListenerServer2.StartServerAsync();
            pathContext.Settings.AddSource("https-feed1", $"{tcpListenerServer1.URI}v3/index.json");
            pathContext.Settings.AddSource("https-feed2", $"{tcpListenerServer2.URI}v3/index.json", "disableTLSCertificateValidation", "true");

            // Act & Assert
            var _result = _msbuildFixture.RunDotnetExpectFailure(workingDirectory, $"restore {projectB.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath}");
            tcpListenerServer1.StopServer();
            tcpListenerServer2.StopServer();
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withAnotherSourceTLSCertificateValidationEnabled_DoesNotThrowException()
        {
            // Arrange
            using var pathContext = _msbuildFixture.CreateSimpleTestPathContext();
            TestDirectory packageSourceDirectory = TestDirectory.Create();
            var packageB100 = new SimpleTestPackageContext("myPackg", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSourceDirectory,
                    PackageSaveMode.Defaultv3,
                    packageB100);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, packageB100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectB.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer1 = new SelfSignedCertificateMockServer(packageSourceDirectory);
            SelfSignedCertificateMockServer tcpListenerServer2 = new SelfSignedCertificateMockServer(packageSourceDirectory);
            var serverTask = tcpListenerServer1.StartServerAsync();
            var serverTask2 = tcpListenerServer2.StartServerAsync();
            pathContext.Settings.AddSource("https-feed1", $"{tcpListenerServer1.URI}v3/index.json");
            pathContext.Settings.AddSource("https-feed2", $"{tcpListenerServer2.URI}v3/index.json", "disableTLSCertificateValidation", "true");

            // Act & Assert
            var _result = _msbuildFixture.RunDotnetExpectSuccess(workingDirectory, $"restore {projectB.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath} --source {tcpListenerServer2.URI}v3/index.json");
            tcpListenerServer1.StopServer();
            tcpListenerServer2.StopServer();
        }
    }
}
