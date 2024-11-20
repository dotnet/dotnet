// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Test.Utility;
using NuGet.XPlat.FuncTest;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetRestoreTLSCertificateValidationTests
    {
        private readonly DotnetIntegrationTestFixture _dotnetFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DotnetRestoreTLSCertificateValidationTests(DotnetIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _dotnetFixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withTLSCertificateValidationDisabled_DoesnotThrowException()
        {
            // Arrange
            using var pathContext = _dotnetFixture.CreateSimpleTestPathContext();
            var packageA100 = new SimpleTestPackageContext("A", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    Path.Combine(pathContext.SolutionRoot, "packages"),
                    PackageSaveMode.Defaultv3,
                    packageA100);
            var projectA = XPlatTestUtils.CreateProject("ProjectA", pathContext, packageA100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer = new SelfSignedCertificateMockServer(Path.Combine(pathContext.SolutionRoot, "packages"), _testOutputHelper);
            var serverTask = tcpListenerServer.StartServerAsync();
            pathContext.Settings.AddSource("https-feed", $"{tcpListenerServer.URI}v3/index.json", "disableTLSCertificateValidation", "true");

            // Act & Assert
            _dotnetFixture.RunDotnetExpectSuccess(workingDirectory, $"restore {projectA.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath}", testOutputHelper: _testOutputHelper);
            tcpListenerServer.StopServer();
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withTLSCertificateValidationEnabled_ThrowException()
        {
            // Arrange
            using var pathContext = _dotnetFixture.CreateSimpleTestPathContext();
            var packageB100 = new SimpleTestPackageContext("myPackg", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    Path.Combine(pathContext.SolutionRoot, "packages"),
                    PackageSaveMode.Defaultv3,
                    packageB100);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, packageB100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectB.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer = new SelfSignedCertificateMockServer(Path.Combine(pathContext.SolutionRoot, "packages"), _testOutputHelper);
            var serverTask = tcpListenerServer.StartServerAsync();
            pathContext.Settings.AddSource("https-feed", $"{tcpListenerServer.URI}v3/index.json");

            // Act & Assert
            var _result = _dotnetFixture.RunDotnetExpectFailure(workingDirectory, $"restore {projectB.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath} -v d", testOutputHelper: _testOutputHelper);
            tcpListenerServer.StopServer();
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withAnotherSourceTLSCertificateValidationDisbaled_ThrowException()
        {
            // Arrange
            using var pathContext = _dotnetFixture.CreateSimpleTestPathContext();
            var packageB100 = new SimpleTestPackageContext("myPackg", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   Path.Combine(pathContext.SolutionRoot, "packages"),
                    PackageSaveMode.Defaultv3,
                    packageB100);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, packageB100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectB.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer1 = new SelfSignedCertificateMockServer(Path.Combine(pathContext.SolutionRoot, "packages"), _testOutputHelper);
            SelfSignedCertificateMockServer tcpListenerServer2 = new SelfSignedCertificateMockServer(Path.Combine(pathContext.SolutionRoot, "packages"), _testOutputHelper);
            var serverTask = tcpListenerServer1.StartServerAsync();
            var serverTask2 = tcpListenerServer2.StartServerAsync();
            pathContext.Settings.AddSource("https-feed1", $"{tcpListenerServer1.URI}v3/index.json");
            pathContext.Settings.AddSource("https-feed2", $"{tcpListenerServer2.URI}v3/index.json", "disableTLSCertificateValidation", "true");

            // Act & Assert
            var _result = _dotnetFixture.RunDotnetExpectFailure(workingDirectory, $"restore {projectB.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath}", testOutputHelper: _testOutputHelper);
            tcpListenerServer1.StopServer();
            tcpListenerServer2.StopServer();
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetRestore_withAnotherSourceTLSCertificateValidationEnabled_DoesNotThrowException()
        {
            // Arrange
            using var pathContext = _dotnetFixture.CreateSimpleTestPathContext();
            var packageB100 = new SimpleTestPackageContext("myPackg", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    Path.Combine(pathContext.SolutionRoot, "packages"),
                    PackageSaveMode.Defaultv3,
                    packageB100);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, packageB100, "net472");
            var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectB.ProjectName);
            SelfSignedCertificateMockServer tcpListenerServer1 = new SelfSignedCertificateMockServer(Path.Combine(pathContext.SolutionRoot, "packages"), _testOutputHelper);
            SelfSignedCertificateMockServer tcpListenerServer2 = new SelfSignedCertificateMockServer(Path.Combine(pathContext.SolutionRoot, "packages"), _testOutputHelper);
            var serverTask = tcpListenerServer1.StartServerAsync();
            var serverTask2 = tcpListenerServer2.StartServerAsync();
            pathContext.Settings.AddSource("https-feed1", $"{tcpListenerServer1.URI}v3/index.json");
            pathContext.Settings.AddSource("https-feed2", $"{tcpListenerServer2.URI}v3/index.json", "disableTLSCertificateValidation", "true");

            // Act & Assert
            var _result = _dotnetFixture.RunDotnetExpectSuccess(workingDirectory, $"restore {projectB.ProjectName}.csproj --configfile {pathContext.Settings.ConfigPath} --source {tcpListenerServer2.URI}v3/index.json", testOutputHelper: _testOutputHelper);
            tcpListenerServer1.StopServer();
            tcpListenerServer2.StopServer();
        }
    }
}
