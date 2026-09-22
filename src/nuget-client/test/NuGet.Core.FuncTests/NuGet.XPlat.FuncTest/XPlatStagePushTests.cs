// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Stage;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatStagePushTests
    {
        private const string PackageFileContent = "package file content";
        private const string SymbolsFileContent = "symbols file content";

        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatStagePushTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static TheoryData<string> SymbolsPackageFileNames => new()
        {
            "Contoso.1.0.0.snupkg",
            "Contoso.1.0.0.symbols.nupkg",
        };

        public static TheoryData<string?, string?, string> ApiKeyPriorityTestData => new()
        {
            { "explicit-api-key", "environment-api-key", "explicit-api-key" },
            { null, "environment-api-key", "environment-api-key" },
            { null, null, "configured-api-key" },
        };

        [PlatformTheory(Platform.Windows)]
        [MemberData(nameof(ApiKeyPriorityTestData))]
        public async Task StagePush_ApiKeySources_UseExpectedPriority(
            string? explicitApiKey,
            string? environmentApiKey,
            string expectedApiKey)
        {
            // Arrange
            const string configuredApiKey = "configured-api-key";
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer(expectedApiKey);
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            ConfigureSource(pathContext, server, setDefaultPushSource: false);
            ISettings settings = Settings.LoadSpecificSettings(
                pathContext.WorkingDirectory,
                Path.GetFileName(pathContext.NuGetConfig));
            SettingsUtility.SetEncryptedValueForAddItem(
                settings,
                ConfigurationConstants.ApiKeys,
                server.StagingUrl,
                configuredApiKey);
            var packageSourceProvider = new PackageSourceProvider(settings);
            var sourceRepositoryProvider = new CachingSourceProvider(packageSourceProvider);
            IEnvironmentVariableReader environmentVariableReader = environmentApiKey is null
                ? TestEnvironmentVariableReader.EmptyInstance
                : new TestEnvironmentVariableReader(
                    new Dictionary<string, string>
                    {
                        ["NUGET_API_KEY"] = environmentApiKey,
                    });
            var args = new StagePushCommandArgs
            {
                PackagePath = packagePath,
                Source = "staging",
                ApiKey = explicitApiKey,
                ConfigFile = pathContext.NuGetConfig,
                Logger = new TestCommandOutputLogger(_testOutputHelper),
                CancellationToken = CancellationToken.None,
            };

            // Act
            int exitCode = await StagePushCommandRunner.RunAsync(
                args,
                settings,
                packageSourceProvider,
                sourceRepositoryProvider,
                environmentVariableReader,
                allowInsecureConnections: true);

            // Assert
            Assert.Equal(ExitCodes.Success, exitCode);
            Assert.Equal(expectedApiKey, Assert.Single(server.Requests).ApiKey);
        }

        [Fact]
        public void StagePush_PackageOnly_StagesPackage()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            StagePushRequest request = Assert.Single(server.Requests);
            Assert.Equal("package", request.Route);
            Assert.Equal("package", request.FormFieldName);
            Assert.Equal(Path.GetFileName(packagePath), request.FileName);
            Assert.Equal(PackageFileContent, request.FileContent);
            Assert.Contains(packagePath, log.ShowMessages());
        }

        [Fact]
        public void StagePush_PackageWithSnupkg_StagesPackageThenSymbols()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            string symbolsPath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.snupkg", content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.Equal(["package", "symbols"], server.Requests.Select(request => request.Route));
            Assert.Equal("package", server.Requests[0].FormFieldName);
            Assert.Equal("symbols", server.Requests[1].FormFieldName);
            Assert.Equal(Path.GetFileName(symbolsPath), server.Requests[1].FileName);
            Assert.Contains(packagePath, log.ShowMessages());
            Assert.Contains(symbolsPath, log.ShowMessages());
        }

        [Fact]
        public void StagePush_PackageWithLegacySymbols_StagesPackageThenSymbols()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            string symbolsPath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.symbols.nupkg", content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.Equal(["package", "symbols"], server.Requests.Select(request => request.Route));
            Assert.Equal(Path.GetFileName(symbolsPath), server.Requests[1].FileName);
        }

        [Fact]
        public void StagePush_WhenBothSymbolsFormatsExist_PrefersSnupkg()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            string snupkgPath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.snupkg", content: SymbolsFileContent);
            CreatePackage(pathContext, fileName: "Contoso.1.0.0.symbols.nupkg", content: "legacy symbols file content");
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.Equal(2, server.Requests.Count);
            Assert.Equal(Path.GetFileName(snupkgPath), server.Requests[1].FileName);
        }

        [Fact]
        public void StagePush_NoSymbols_StagesOnlyPackage()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            CreatePackage(pathContext, fileName: "Contoso.1.0.0.snupkg", content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = [.. CreateArgs(pathContext, server, packagePath), "--no-symbols"];

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.Single(server.Requests);
            Assert.Equal("package", server.Requests[0].Route);
        }

        [Theory]
        [MemberData(nameof(SymbolsPackageFileNames))]
        public void StagePush_DirectSymbols_StagesSymbols(string fileName)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string symbolsPath = CreatePackage(pathContext, fileName: fileName, content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, symbolsPath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            StagePushRequest request = Assert.Single(server.Requests);
            Assert.Equal("symbols", request.Route);
            Assert.Equal("symbols", request.FormFieldName);
            Assert.Equal(fileName, request.FileName);
        }

        [Fact]
        public void StagePush_WithGroup_StagesPackageAndSymbolsInGroup()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            CreatePackage(pathContext, fileName: "Contoso.1.0.0.snupkg", content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = [.. CreateArgs(pathContext, server, packagePath), "--group", "release-group"];

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.Equal(2, server.Requests.Count);
            Assert.All(server.Requests, request => Assert.Equal("release-group", request.GroupId));
        }

        [Fact]
        public void StagePush_WithInvalidGroupId_FailsBeforeUpload()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = [.. CreateArgs(pathContext, server, packagePath), "--group", "-release"];

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.Empty(server.Requests);
            Assert.Contains(
                CommandLine.XPlat.Strings.StagePushCommand_Error_InvalidGroup,
                log.ShowErrors());
        }

        [Fact]
        public void StagePush_ExplicitApiKey_AuthenticatesWithServer()
        {
            // Arrange
            const string expectedApiKey = "expected-api-key";
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer(expectedApiKey);
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = [.. CreateArgs(pathContext, server, packagePath), "--api-key", expectedApiKey];

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            StagePushRequest request = Assert.Single(server.Requests);
            Assert.Equal(expectedApiKey, request.ApiKey);
            Assert.Equal("package", request.Route);
            Assert.Equal("package", request.FormFieldName);
            Assert.Equal(Path.GetFileName(packagePath), request.FileName);
            Assert.Equal(PackageFileContent, request.FileContent);
            Assert.Contains(packagePath, log.ShowMessages());
        }

        [Fact]
        public void StagePush_InvalidApiKey_FailsAuthentication()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer(expectedApiKey: "expected-api-key");
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = [.. CreateArgs(pathContext, server, packagePath), "--api-key", "invalid-api-key"];

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.Equal("invalid-api-key", Assert.Single(server.Requests).ApiKey);
            Assert.Contains("403", log.ShowErrors());
        }

        [Fact]
        public void StagePush_DefaultPushSource_UsesConfiguredSource()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            ConfigureSource(pathContext, server, setDefaultPushSource: true);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args =
            [
                "package",
                "stage",
                "push",
                packagePath,
                "--configfile",
                pathContext.NuGetConfig,
            ];

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(0, exitCode);
            StagePushRequest request = Assert.Single(server.Requests);
            Assert.Equal("package", request.Route);
            Assert.Equal("package", request.FormFieldName);
            Assert.Equal(Path.GetFileName(packagePath), request.FileName);
            Assert.Equal(PackageFileContent, request.FileContent);
            Assert.Contains(packagePath, log.ShowMessages());
        }

        [Fact]
        public void StagePush_UnsupportedFile_FailsBeforeUpload()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.zip", content: PackageFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.Empty(server.Requests);
            Assert.Contains(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommandLine.XPlat.Strings.StagePushCommand_Error_UnsupportedPackage,
                    packagePath),
                log.ShowErrors());
        }

        [Fact]
        public void StagePush_SourceWithoutStagingResource_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer(advertiseStagingResource: false);
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.Empty(server.Requests);
            Assert.Contains(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommandLine.XPlat.Strings.StagePushCommand_Error_ResourceNotFound,
                    server.SourceUrl),
                log.ShowErrors());
        }

        [Fact]
        public void StagePush_PackageUploadFails_DoesNotUploadSymbols()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer
            {
                PackageStatusCode = HttpStatusCode.InternalServerError,
            };
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            CreatePackage(pathContext, fileName: "Contoso.1.0.0.snupkg", content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.NotEmpty(server.Requests);
            Assert.All(server.Requests, request => Assert.Equal("package", request.Route));
        }

        [Fact]
        public void StagePush_SymbolsUploadFails_ReportsPartialFailure()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer
            {
                SymbolsStatusCode = HttpStatusCode.InternalServerError,
            };
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            string symbolsPath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.snupkg", content: SymbolsFileContent);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args = CreateArgs(pathContext, server, packagePath);

            // Act
            int exitCode = RunStagePush(args, log);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.Equal("package", server.Requests[0].Route);
            Assert.All(server.Requests.Skip(1), request => Assert.Equal("symbols", request.Route));
            Assert.Contains(packagePath, log.ShowMessages());
            Assert.Contains(symbolsPath, log.ShowErrors());
        }

        [Fact]
        public void StagePush_HttpSourceWithoutOptIn_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            using var server = new StagePushTestServer();
            string packagePath = CreatePackage(pathContext, fileName: "Contoso.1.0.0.nupkg", content: PackageFileContent);
            pathContext.Settings.AddSource("staging", server.SourceUrl);
            var log = new TestCommandOutputLogger(_testOutputHelper);
            string[] args =
            [
                "package",
                "stage",
                "push",
                packagePath,
                "--source",
                "staging",
                "--configfile",
                pathContext.NuGetConfig,
            ];

            // Act
            int exitCode = RunStagePush(
                args,
                log,
                allowInsecureConnections: false);

            // Assert
            Assert.Equal(1, exitCode);
            Assert.Empty(server.Requests);
            Assert.Contains(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommandLine.XPlat.Strings.Error_HttpServerUsage,
                    "stage push",
                    server.SourceUrl),
                log.ShowErrors());
        }

        private static string[] CreateArgs(
            SimpleTestPathContext pathContext,
            StagePushTestServer server,
            string packagePath)
        {
            ConfigureSource(pathContext, server, setDefaultPushSource: false);

            return
            [
                "package",
                "stage",
                "push",
                packagePath,
                "--source",
                "staging",
                "--configfile",
                pathContext.NuGetConfig,
            ];
        }

        private static int RunStagePush(
            string[] args,
            TestCommandOutputLogger log,
            IEnvironmentVariableReader? environmentVariableReader = null,
            bool allowInsecureConnections = true)
        {
            environmentVariableReader ??= TestEnvironmentVariableReader.EmptyInstance;

            var rootCommand = new RootCommand();
            var packageCommand = new Command("package");
            var stageCommand = new Command("stage");
            rootCommand.Subcommands.Add(packageCommand);
            packageCommand.Subcommands.Add(stageCommand);

            StagePushCommand.Register(
                stageCommand,
                new Option<bool>("--interactive"),
                () => log,
                stageArgs =>
                {
                    ISettings settings = XPlatUtility.ProcessConfigFile(stageArgs.ConfigFile);
                    var packageSourceProvider = new PackageSourceProvider(settings);
                    var sourceRepositoryProvider = new CachingSourceProvider(packageSourceProvider);

                    return StagePushCommandRunner.RunAsync(
                        stageArgs,
                        settings,
                        packageSourceProvider,
                        sourceRepositoryProvider,
                        environmentVariableReader,
                        allowInsecureConnections);
                });

            ParseResult parseResult = rootCommand.Parse(args);
            var invocationConfig = new InvocationConfiguration
            {
                EnableDefaultExceptionHandler = false,
            };

            try
            {
                return parseResult.Invoke(invocationConfig);
            }
            catch (Exception ex)
            {
                Program.LogException(ex, log);
                return 1;
            }
        }

        private static void ConfigureSource(
            SimpleTestPathContext pathContext,
            StagePushTestServer server,
            bool setDefaultPushSource)
        {
            pathContext.Settings.AddSource(
                "staging",
                server.SourceUrl,
                allowInsecureConnectionsValue: "true");

            if (setDefaultPushSource)
            {
                pathContext.Settings.SetDefaultPushSource("staging");
            }
        }

        private static string CreatePackage(
            SimpleTestPathContext pathContext,
            string fileName,
            string content)
        {
            string packagePath = Path.Combine(pathContext.WorkingDirectory, fileName);
            File.WriteAllText(packagePath, content);
            return packagePath;
        }
    }
}
