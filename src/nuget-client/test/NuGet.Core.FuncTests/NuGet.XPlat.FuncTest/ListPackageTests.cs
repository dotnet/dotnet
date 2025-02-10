// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    [Collection("NuGet XPlat Test Collection")]
    public class ListPackageTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ListPackageTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void BasicListPackageParsing_Interactive()
        {
            VerifyCommand(
                (projectPath, mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string> { "list", "--interactive", projectPath };

                    // Act
                    var result = testApp.Execute(argList.ToArray());

                    // Assert
                    mockCommandRunner.Verify();
                    Assert.NotNull(HttpHandlerResourceV3.CredentialService);
                    Assert.Equal(0, result);
                });
        }

        [Fact]
        public void BasicListPackageParsing_InteractiveTakesNoArguments_ThrowsException()
        {
            VerifyCommand(
                (projectPath, mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string>() { "list", "--interactive", "no", projectPath };

                    // Act & Assert
                    Assert.Throws<CommandParsingException>(() => testApp.Execute(argList.ToArray()));
                });
        }

        [Theory]
        [InlineData("q", LogLevel.Warning)]
        [InlineData("quiet", LogLevel.Warning)]
        [InlineData("m", LogLevel.Minimal)]
        [InlineData("minimal", LogLevel.Minimal)]
        [InlineData("something-else", LogLevel.Minimal)]
        [InlineData("n", LogLevel.Information)]
        [InlineData("normal", LogLevel.Information)]
        [InlineData("d", LogLevel.Debug)]
        [InlineData("detailed", LogLevel.Debug)]
        [InlineData("diag", LogLevel.Debug)]
        [InlineData("diagnostic", LogLevel.Debug)]
        public void BasicListPackageParsing_VerbosityOption(string verbosity, LogLevel logLevel)
        {
            VerifyCommand(
                (projectPath, mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string> { "list", projectPath, "--verbosity", verbosity };

                    // Act
                    var result = testApp.Execute(argList.ToArray());

                    // Assert
                    Assert.Equal(logLevel, getLogLevel());
                    Assert.Equal(0, result);
                });
        }

        [Fact]
        public void BasicListPackageParsing_NoVerbosityOption()
        {
            VerifyCommand((projectPath, mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string> { "list", projectPath };

                    // Act
                    var result = testApp.Execute(argList.ToArray());

                    // Assert
                    Assert.Equal(LogLevel.Minimal, getLogLevel());
                    Assert.Equal(0, result);
                });
        }

        [Theory]
        [InlineData("")]
        [InlineData("--format json")]
        [InlineData("--format JSON")]
        [InlineData("--format json --output-version 1")]
        [InlineData("--format console")]
        public void BasicListPackage_OutputFormat_CorrectInput_Parsing_Succeeds(string outputFormatCommmand)
        {
            VerifyCommand(
                (projectPath, mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string>() { "list" };

                    if (!string.IsNullOrEmpty(outputFormatCommmand))
                    {
                        argList.AddRange(outputFormatCommmand.Split(' ').ToList());
                    }

                    argList.Add(projectPath);

                    // Act
                    var result = testApp.Execute(argList.ToArray());

                    // Assert
                    mockCommandRunner.Verify();
                    Assert.Equal(0, result);
                });
        }

        [Theory]
        [InlineData("--format xml")]
        [InlineData("--format json --output-version 0")]
        [InlineData("--format json --output-version 2")]
        [InlineData("--format console --output-version 1")]
        [InlineData("--output-version 0")]
        [InlineData("--output-version 1")]
        public void BasicListPackage_OutputFormat_BadInput_Parsing_Fails(string outputFormatCommmand)
        {
            VerifyCommand(
                (projectPath, mockCommandRunner, testApp, getLogLevel) =>
                {
                    // Arrange
                    var argList = new List<string>() { "list" };

                    if (!string.IsNullOrEmpty(outputFormatCommmand))
                    {
                        argList.AddRange(outputFormatCommmand.Split(' ').ToList());
                    }

                    argList.Add(projectPath);

                    // Act & Assert
                    Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));
                });
        }

        [PlatformFact(Platform.Windows, Skip = "https://github.com/NuGet/Home/issues/13874")]
        public async Task ListPackage_WithPrivateHttpSourceCredentialServiceIsInvokedAsNeeded_Succeeds()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA100 = new SimpleTestPackageContext("A", "1.0.0");
            var packageB100 = new SimpleTestPackageContext("B", "1.0.0");

            await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    packageA100,
                    packageB100);

            var projectA = SimpleTestProjectContext.CreateNETCore("ProjectA", pathContext.SolutionRoot, "net6.0");
            var projectB = SimpleTestProjectContext.CreateNETCore("ProjectB", pathContext.SolutionRoot, "net6.0");

            projectA.AddPackageToAllFrameworks(packageA100);
            projectB.AddPackageToAllFrameworks(packageB100);

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            solution.Projects.Add(projectA);
            solution.Projects.Add(projectB);
            solution.Create(pathContext.SolutionRoot);

            SimpleTestSettingsContext.RemoveSource(pathContext.Settings.XML, "source");

            using var mockServer = new FileSystemBackedV3MockServer(pathContext.PackageSource, isPrivateFeed: true);
            mockServer.Start();
            pathContext.Settings.AddSource(sourceName: "private-source", sourceUri: mockServer.ServiceIndexUri, allowInsecureConnectionsValue: bool.TrueString);

            var mockedCredentialService = new Mock<ICredentialService>();
            var expectedCredentials = new NetworkCredential("user", "password1");
            SetupCredentialServiceMock(mockedCredentialService, expectedCredentials, new Uri(mockServer.ServiceIndexUri));
            HttpHandlerResourceV3.CredentialService = new Lazy<ICredentialService>(() => mockedCredentialService.Object);

            // List package command requires restore to be run before it can list packages.
            await RestoreProjectsAsync(pathContext, projectA, projectB, _testOutputHelper);

            // Act
            var output = new StringBuilder();
            var error = new StringBuilder();
            using TextWriter consoleOut = new StringWriter(output);
            using TextWriter consoleError = new StringWriter(error);
            var logger = new TestLogger(_testOutputHelper);
            ListPackageCommandRunner listPackageCommandRunner = new();
            var packageRefArgs = new ListPackageArgs(
                                        path: Path.Combine(pathContext.SolutionRoot, "solution.sln"),
                                        packageSources: [new(mockServer.ServiceIndexUri)],
                                        frameworks: ["net6.0"],
                                        reportType: ReportType.Vulnerable,
                                        renderer: new ListPackageConsoleRenderer(consoleOut, consoleError),
                                        includeTransitive: false,
                                        prerelease: false,
                                        highestPatch: false,
                                        highestMinor: false,
                                        auditSources: null,
                                        logger: logger,
                                        cancellationToken: CancellationToken.None);

            int result = await listPackageCommandRunner.ExecuteCommandAsync(packageRefArgs);

            // Assert
            Assert.True(result == 0, userMessage: logger.ShowMessages());
            // GetCredentialsAsync should be called once during restore
            mockedCredentialService.Verify(x => x.GetCredentialsAsync(It.IsAny<Uri>(), It.IsAny<IWebProxy>(), It.IsAny<CredentialRequestType>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            // TryGetLastKnownGoodCredentialsFromCache should be called twice during restore and once during list package.Hence total 3 times.
            mockedCredentialService.Verify(x => x.TryGetLastKnownGoodCredentialsFromCache(It.IsAny<Uri>(), It.IsAny<bool>(), out It.Ref<ICredentials>.IsAny), Times.Exactly(3));

            static void SetupCredentialServiceMock(Mock<ICredentialService> mockedCredentialService, NetworkCredential expectedCredentials, Uri packageSourceUri)
            {
                NetworkCredential cachedCredentials = default;
                mockedCredentialService.SetupGet(x => x.HandlesDefaultCredentials).Returns(true);
                // Setup GetCredentialsAsync mock
                mockedCredentialService
                    .Setup(x => x.GetCredentialsAsync(packageSourceUri, It.IsAny<IWebProxy>(), CredentialRequestType.Unauthorized, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() =>
                    {
                        cachedCredentials = expectedCredentials;
                        return cachedCredentials;
                    });
                // Setup TryGetLastKnownGoodCredentialsFromCache mock
                mockedCredentialService
                    .Setup(x => x.TryGetLastKnownGoodCredentialsFromCache(packageSourceUri, It.IsAny<bool>(), out It.Ref<ICredentials>.IsAny))
                    .Returns((Uri sourceUri, bool isProxyRequest, out ICredentials outCredentials) =>
                    {
                        outCredentials = cachedCredentials;
                        return outCredentials != null;
                    });
            }
        }

        static async Task RestoreProjectsAsync(SimpleTestPathContext pathContext, SimpleTestProjectContext projectA, SimpleTestProjectContext projectB, ITestOutputHelper testOutputHelper)
        {
            var settings = Settings.LoadDefaultSettings(Path.GetDirectoryName(pathContext.SolutionRoot), Path.GetFileName(pathContext.NuGetConfig), null);
            var packageSourceProvider = new PackageSourceProvider(settings);

            var sources = packageSourceProvider.LoadPackageSources();

            await RestoreProjectAsync(settings, pathContext, projectA, sources, testOutputHelper);
            await RestoreProjectAsync(settings, pathContext, projectB, sources, testOutputHelper);

            static async Task RestoreProjectAsync(ISettings settings,
                SimpleTestPathContext pathContext,
                SimpleTestProjectContext project,
                IEnumerable<PackageSource> packageSources,
                ITestOutputHelper testOutputHelper)
            {
                var packageSpec = ProjectTestHelpers.WithSettingsBasedRestoreMetadata(project.PackageSpec, settings);

                var logger = new TestLogger(testOutputHelper);

                var command = new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec));
                var restoreResult = await command.ExecuteAsync(CancellationToken.None);
                await restoreResult.CommitAsync(logger, CancellationToken.None);
                Assert.True(restoreResult.Success, userMessage: logger.ShowMessages());
            }
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public async Task CanListPackagesForProjectsInSolutions(bool useSlnx)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA100 = new SimpleTestPackageContext("A", "1.0.0");
            var packageB100 = new SimpleTestPackageContext("B", "1.0.0");

            await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    packageA100,
                    packageB100);

            var projectA = SimpleTestProjectContext.CreateNETCore("ProjectA", pathContext.SolutionRoot, "net6.0");
            var projectB = SimpleTestProjectContext.CreateNETCore("ProjectB", pathContext.SolutionRoot, "net6.0");

            projectA.AddPackageToAllFrameworks(packageA100);
            projectB.AddPackageToAllFrameworks(packageB100);

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot, useSlnx);
            solution.Projects.Add(projectA);
            solution.Projects.Add(projectB);
            solution.Create(pathContext.SolutionRoot);

            using var mockServer = new FileSystemBackedV3MockServer(pathContext.PackageSource, isPrivateFeed: true);
            mockServer.Start();
            pathContext.Settings.AddSource(sourceName: "private-source", sourceUri: mockServer.ServiceIndexUri, allowInsecureConnectionsValue: bool.TrueString);

            // List package command requires restore to be run before it can list packages.
            await RestoreProjectsAsync(pathContext, projectA, projectB, _testOutputHelper);

            var output = new StringBuilder();
            var error = new StringBuilder();
            using TextWriter consoleOut = new StringWriter(output);
            using TextWriter consoleError = new StringWriter(error);
            var logger = new TestLogger(_testOutputHelper);
            ListPackageCommandRunner listPackageCommandRunner = new();
            var packageRefArgs = new ListPackageArgs(
                                        path: solution.SolutionPath,
                                        packageSources: [new(mockServer.ServiceIndexUri)],
                                        frameworks: ["net6.0"],
                                        reportType: ReportType.Vulnerable,
                                        renderer: new ListPackageConsoleRenderer(consoleOut, consoleError),
                                        includeTransitive: false,
                                        prerelease: false,
                                        highestPatch: false,
                                        highestMinor: false,
                                        auditSources: null,
                                        logger: logger,
                                        cancellationToken: CancellationToken.None);

            int result = await listPackageCommandRunner.ExecuteCommandAsync(packageRefArgs);
            Assert.True(result == 0, userMessage: logger.ShowMessages());
        }

        [Fact]
        public async Task GetReportDataAsync_WhenReportTypeIsVulnerable_ShouldUseAuditSources()
        {
            // Arrange
            using var mockServer = SetupMockServer();
            var auditSource = new PackageSource(mockServer.Uri + "v3/index.json") { AllowInsecureConnections = true };

            var mockRenderer = new Mock<IReportRenderer>();
            var mockLogger = new Mock<ILogger>();

            using var pathContext = new SimpleTestPathContext();
            var project = SetupTestProject(pathContext);
            SetupAssetsAndProps(project);

            var listPackageArgs = new ListPackageArgs(
                path: project.ProjectPath,
                packageSources: new List<PackageSource> { new PackageSource(pathContext.PackageSource) },
                frameworks: new List<string>(),
                ReportType.Vulnerable,
                mockRenderer.Object,
                includeTransitive: true,
                prerelease: false,
                highestPatch: false,
                highestMinor: false,
                new List<PackageSource> { auditSource },
                mockLogger.Object,
                CancellationToken.None
            );

            var listPackageCommandRunner = new ListPackageCommandRunner();


            // Act
            var result = await listPackageCommandRunner.GetReportDataAsync(listPackageArgs);

            // Assert
            Assert.Equal(1, result.Item2.Projects.Count);
            Assert.Equal(1, result.Item2.Projects.First().TargetFrameworkPackages.Count);
            Assert.Equal(1, result.Item2.Projects.First().TargetFrameworkPackages.First().TopLevelPackages.Count);
            Assert.Equal(1, result.Item2.Projects.First().TargetFrameworkPackages.First().TopLevelPackages.First().Vulnerabilities.Count);
            Assert.Equal(2, result.Item2.Projects[0].TargetFrameworkPackages[0].TopLevelPackages.First().Vulnerabilities.First().Severity);
            Assert.Equal("https://test/", result.Item2.Projects[0].TargetFrameworkPackages[0].TopLevelPackages.First().Vulnerabilities.First().AdvisoryUrl.ToString());
        }

        [Fact]
        public async Task GetReportDataAsync_WhenReportTypeIsVulnerableAuditSourcesWithNoVulnerabilityInfoResource_ShouldWarn()
        {
            // Arrange
            const string indexJson = """
    {
        "version": "3.0.0",
        "resources": [{}]
    }
    """;

            using var mockServer = new MockServer();
            mockServer.Get.Add("/v3/index.json", _ => indexJson);
            mockServer.Start();

            var auditSource = new PackageSource($"{mockServer.Uri}v3/index.json") { AllowInsecureConnections = true };

            using var pathContext = new SimpleTestPathContext();
            var project = SetupTestProject(pathContext);
            SetupAssetsAndProps(project);

            var mockRenderer = new Mock<IReportRenderer>();
            var mockLogger = new Mock<ILogger>();

            var listPackageArgs = new ListPackageArgs(
                project.ProjectPath,
                new List<PackageSource> { new PackageSource(pathContext.PackageSource) },
                new List<string>(),
                ReportType.Vulnerable,
                mockRenderer.Object,
                includeTransitive: true,
                prerelease: false,
                highestPatch: false,
                highestMinor: false,
                new List<PackageSource> { auditSource },
                mockLogger.Object,
                CancellationToken.None
            );

            var listPackageCommandRunner = new ListPackageCommandRunner();

            // Act
            var result = await listPackageCommandRunner.GetReportDataAsync(listPackageArgs);
            var projectResult = result.Item2.Projects.First();
            var warning = projectResult.ProjectProblems.First();

            // Assert
            Assert.Single(result.Item2.Projects);
            Assert.Single(projectResult.ProjectProblems);
            Assert.Equal(ProblemType.Warning, warning.ProblemType);
            Assert.Equal(
                string.Format(CultureInfo.CurrentCulture, CommandLine.XPlat.Strings.Warning_AuditSourceWithoutData, auditSource.Name),
                warning.Text
            );
            Assert.Empty(projectResult.TargetFrameworkPackages.First().TopLevelPackages);
            Assert.Empty(projectResult.TargetFrameworkPackages.First().TransitivePackages);
        }


        private void VerifyCommand(Action<string, Mock<IListPackageCommandRunner>, CommandLineApplication, Func<LogLevel>> verify)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectPath = Path.Combine(testDirectory, "project.csproj");
                File.WriteAllText(projectPath, string.Empty);

                var logLevel = LogLevel.Information;
                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var testApp = new CommandLineApplication();
                var mockCommandRunner = new Mock<IListPackageCommandRunner>();
                mockCommandRunner
                    .Setup(m => m.ExecuteCommandAsync(It.IsAny<ListPackageArgs>()))
                    .Returns(Task.FromResult(0));

                testApp.Name = "dotnet nuget_test";
                ListPackageCommand.Register(testApp,
                    () => logger,
                    ll => logLevel = ll,
                    () => mockCommandRunner.Object);

                // Act & Assert
                try
                {
                    verify(projectPath, mockCommandRunner, testApp, () => logLevel);
                }
                finally
                {
                    XPlatTestUtils.DisposeTemporaryFile(projectPath);
                }
            }
        }

        [Fact]
        public void JsonRenderer_ListPackageArgse_Verify_AllFields_Covered()
        {
            Type listPackageArgsType = typeof(ListPackageArgs);
            FieldInfo[] fields = listPackageArgsType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.True(13 == fields.Length, "Number of fields are changed in ListPackageArgs.cs. Please make sure this change is accounted for GetReportParameters method in that file.");
        }

        private static SimpleTestProjectContext SetupTestProject(SimpleTestPathContext pathContext)
        {
            var package = new SimpleTestPackageContext { Id = "task", Version = "1.0.0" };

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var project = SimpleTestProjectContext.CreateNETCore("ProjectA", pathContext.SolutionRoot, NuGetFramework.Parse("net8.0"));
            project.Type = ProjectStyle.PackageReference;
            project.SingleTargetFramework = true;
            project.AddPackageToAllFrameworks(package);

            solution.Projects.Add(project);
            solution.Create(pathContext.SolutionRoot);

            return project;
        }

        private void SetupAssetsAndProps(SimpleTestProjectContext project)
        {
            string objFolder = Path.Combine(Path.GetDirectoryName(project.ProjectPath), "obj");
            Directory.CreateDirectory(objFolder);

            string assetsPath = Path.Combine(objFolder, "project.assets.json");
            string propsPath = Path.Combine(objFolder, "ProjectA.csproj.nuget.g.props");

            string assetsContent = ResourceTestUtility.GetResource(
                "NuGet.XPlat.FuncTest.compiler.resources.Test.OnePackage.project.assets.json",
                GetType()
            );
            string propsContent = ResourceTestUtility.GetResource(
                "NuGet.XPlat.FuncTest.compiler.resources.Test.ProjectA.csproj.nuget.g.props",
                GetType()
            );

            File.WriteAllText(assetsPath, assetsContent);
            File.WriteAllText(propsPath, propsContent);
        }

        private static MockServer SetupMockServer()
        {
            var mockServer = new MockServer();

            string indexJson = $@"
    {{
        ""version"": ""3.0.0"",
        ""resources"": [
            {{
                ""@id"": ""{mockServer.Uri}v3/vulnerabilities/index.json"",
                ""@type"": ""VulnerabilityInfo/6.7.0"",
                ""comment"": ""This is a test feed for vulnerabilities""
            }}
        ]
    }}";

            string vulnerabilitiesJson = $@"
    [
        {{
            ""@name"": ""base"",
            ""@id"": ""{mockServer.Uri}v3-vulnerabilities/2024.12.21.05.12.11/vulnerability.base.json"",
            ""@updated"": ""2024-12-21T05:12:11.2008556Z"",
            ""comment"": ""The base data for vulnerability update periodically""
        }}
    ]";

            string baseVulnerabilityJson = $@"
    {{
        ""task"": [
            {{
                ""url"": ""https://test/"",
                ""severity"": 2,
                ""versions"": ""(, 10.0.3)""
            }}
        ]
    }}";

            mockServer.Get.Add("/v3/index.json", _ => indexJson);
            mockServer.Get.Add("/v3/vulnerabilities/index.json", _ => vulnerabilitiesJson);
            mockServer.Get.Add("/v3-vulnerabilities/2024.12.21.05.12.11/vulnerability.base.json", _ => baseVulnerabilityJson);
            mockServer.Start();

            return mockServer;
        }

    }
}
