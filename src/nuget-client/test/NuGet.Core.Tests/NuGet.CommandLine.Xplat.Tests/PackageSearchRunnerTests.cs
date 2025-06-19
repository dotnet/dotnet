// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class PackageSearchRunnerTests : PackageSearchTestInitializer, IClassFixture<PackageSearchRunnerFixture>
    {
        PackageSearchRunnerFixture _fixture;

        public PackageSearchRunnerTests(PackageSearchRunnerFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(0, 10, true)]
        [InlineData(0, 20, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 20, false)]
        public async Task RunAsync_TableFormatNormalVerbosity_OnePackageTableOutputted(int skip, int take, bool prerelease)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
                pathContext.WorkingDirectory,
                configFileName: pathContext.NuGetConfig,
                machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);
            var expectedDefaultColoredMessage =
                "| Package ID           | Latest Version | Owners            | Total Downloads |" +
                "| -------------------- | -------------- | ----------------- | --------------- |" +
                "| Fake.Newtonsoft. | 12.0.3         | James Newton-King | 531,607,259     |" +
                "| -------------------- | -------------- | ----------------- | --------------- |";
            var expectedRedColoredMessage = "Json";
            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = skip,
                Take = take,
                Prerelease = prerelease,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Normal,
                Format = PackageSearchFormat.Table
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            ColoredMessage[System.Console.ForegroundColor].Should().Be(expectedDefaultColoredMessage);
            ColoredMessage[ConsoleColor.Red].Should().Be(expectedRedColoredMessage);
        }

        [Theory]
        [InlineData(0, 10, true)]
        [InlineData(0, 20, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 20, false)]
        public async Task RunAsync_TableFormatMinimalVerbosity_OnePackageTableOutputted(int skip, int take, bool prerelease)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);
            var expectedDefaultColorMessage =
                "| Package ID           | Latest Version |" +
                "| -------------------- | -------------- |" +
                "| Fake.Newtonsoft. | 12.0.3         |" +
                "| -------------------- | -------------- |";
            var expectedRedColorMessage = "Json";
            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = skip,
                Take = take,
                Prerelease = prerelease,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Minimal,
                Format = PackageSearchFormat.Table
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            ColoredMessage[System.Console.ForegroundColor].Should().Be(expectedDefaultColorMessage);
            ColoredMessage[ConsoleColor.Red].Should().Be(expectedRedColorMessage);
        }

        [Theory]
        [InlineData(0, 10, true)]
        [InlineData(0, 20, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 20, false)]
        public async Task RunAsync_TableFormatDetailedVerbosity_OnePackageTableOutputted(int skip, int take, bool prerelease)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);
            var expectedDefaultColorMessage =
                "| Package ID           | Latest Version | Owners            | Total Downloads | Vulnerable | Deprecation                      | Project URL   | Description     |" +
                "| -------------------- | -------------- | ----------------- | --------------- | ---------- | -------------------------------- | ------------- | --------------- |" +
                "| Fake.Newtonsoft. | 12.0.3         | James Newton-King | 531,607,259     | N/A        | This package has been deprecated | http://myuri/ | My description. |" +
                "| -------------------- | -------------- | ----------------- | --------------- | ---------- | -------------------------------- | ------------- | --------------- |";
            var expectedRedColorMessage = "Json";
            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = skip,
                Take = take,
                Prerelease = prerelease,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Detailed,
                Format = PackageSearchFormat.Table
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            ColoredMessage[System.Console.ForegroundColor].Should().Be(expectedDefaultColorMessage);
            ColoredMessage[ConsoleColor.Red].Should().Be(expectedRedColorMessage);
        }

        [Theory]
        [InlineData(0, 10, true)]
        [InlineData(0, 20, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 20, false)]
        public async Task RunAsync_JsonFormatNormalVerbosity_OnePackageJsonOutputted(int skip, int take, bool prerelease)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = skip,
                Take = take,
                Prerelease = prerelease,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Normal,
                Format = PackageSearchFormat.Json
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            string message = _fixture.NormalizeNewlines(Message);
            message.Should().Contain(_fixture.ExpectedSearchResultNormal);
        }

        [Theory]
        [InlineData(0, 10, true)]
        [InlineData(0, 20, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 20, false)]
        public async Task RunAsync_JsonFormatMinimalVerbosity_OnePackageJsonOutputted(int skip, int take, bool prerelease)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = skip,
                Take = take,
                Prerelease = prerelease,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Minimal,
                Format = PackageSearchFormat.Json
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            string message = _fixture.NormalizeNewlines(Message);
            message.Should().Contain(_fixture.ExpectedSearchResultMinimal);
        }

        [Theory]
        [InlineData(0, 10, true)]
        [InlineData(0, 20, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 20, false)]
        public async Task RunAsync_JsonFormatDetailedVerbosity_OnePackageJsonOutputted(int skip, int take, bool prerelease)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = skip,
                Take = take,
                Prerelease = prerelease,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Detailed,
                Format = PackageSearchFormat.Json
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            string message = _fixture.NormalizeNewlines(Message);
            message.Should().Contain(_fixture.ExpectedSearchResultDetailed);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Minimal")]
        [InlineData("Normal")]
        [InlineData("Detailed")]
        public async Task RunAsync_ExactMatchOptionEnabled_WithTableFormatDifferentVerbosity_OnePackageTableOutputted(string verbosity)
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");

            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchVerbosity packageSearchVerbosity = verbosity == null
                ? PackageSearchVerbosity.Normal
                : Enum.Parse<PackageSearchVerbosity>(verbosity, ignoreCase: true);

            var expectedDefaultColorMessage =
                    "| Package ID      | Version | Owners | Total Downloads |" +
                    "| --------------- | ------- | ------ | --------------- |" +
                    "|  | 13.0.3  |        | N/A             |" +
                    "| --------------- | ------- | ------ | --------------- |";
            if (packageSearchVerbosity == PackageSearchVerbosity.Minimal)
            {
                expectedDefaultColorMessage =
               "| Package ID      | Version |" +
               "| --------------- | ------- |" +
               "|  | 13.0.3  |" +
               "| --------------- | ------- |";
            }
            else if (packageSearchVerbosity == PackageSearchVerbosity.Detailed)
            {
                expectedDefaultColorMessage =
                    "| Package ID      | Version | Owners | Total Downloads | Vulnerable | Deprecation | Project URL                     | Description                                                    |" +
                    "| --------------- | ------- | ------ | --------------- | ---------- | ----------- | ------------------------------- | -------------------------------------------------------------- |" +
                    "|  | 13.0.3  |        | N/A             | N/A        | N/A         | https://www.newtonsoft.com/json | Json.NET is a popular high-performance JSON framework for .NET |" +
                    "| --------------- | ------- | ------ | --------------- | ---------- | ----------- | ------------------------------- | -------------------------------------------------------------- |";
            }
            var expectedRedColorMessage = "Newtonsoft.Json";
            PackageSearchArgs packageSearchArgs = new PackageSearchArgs()
            {
                Prerelease = false,
                ExactMatch = true,
                Logger = GetLogger(),
                SearchTerm = "Newtonsoft.Json",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Format = PackageSearchFormat.Table
            };

            if (verbosity != null)
            {
                packageSearchArgs.Verbosity = packageSearchVerbosity;
            }

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            ColoredMessage[System.Console.ForegroundColor].Should().Be(expectedDefaultColorMessage);
            ColoredMessage[ConsoleColor.Red].Should().Be(expectedRedColorMessage);
        }

        [Fact]
        public async Task RunAsync_WhenSourceIsInvalid_ReturnsErrorExitCode()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: null,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);
            string source = "invalid-source";
            string expectedError = string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidSource, source);
            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = 0,
                Take = 10,
                Prerelease = true,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { source }
            };

            // Act
            int exitCode = await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            exitCode.Should().Be(ExitCodes.Error);
            StoredErrorMessage.Should().Contain(expectedError);
        }

        [Fact]
        public async Task RunAsync_WhenSourceHasNoSearchResource_LogsSearchServiceMissingError()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/indexWithNoSearchResource.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
            pathContext.WorkingDirectory,
            configFileName: pathContext.NuGetConfig,
            machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);
            string expectedError = Protocol.Strings.Protocol_MissingSearchService;
            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = 0,
                Take = 10,
                Prerelease = true,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { source }
            };

            // Act
            int exitCode = await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            StoredErrorMessage.Should().Contain(expectedError);
        }

        [Fact]
        public async Task RunAsync_HandlesOperationCanceledException_WhenCancellationIsRequested()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/indexWithNoSearchResource.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
                pathContext.WorkingDirectory,
                configFileName: pathContext.NuGetConfig,
                machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);
            var expectedError = "A task was canceled.";
            var cts = new CancellationTokenSource();
            PackageSearchArgs packageSearchArgs = new PackageSearchArgs
            {
                Skip = 0,
                Take = 10,
                Prerelease = true,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "json",
                Sources = new List<string> { source }
            };

            // Immediately request cancellation
            cts.Cancel();

            // Act
            var exitCode = await PackageSearchRunner.RunAsync(
                    sourceProvider: sourceProvider,
                    packageSearchArgs,
                    cancellationToken: cts.Token);

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            StoredErrorMessage.Should().Contain(expectedError);
        }

        [Fact]
        public async Task RunAsync_WhenPackageHasOnlyIdAndVersion_ReturnsValidNormalVerbosityOutput()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
                pathContext.WorkingDirectory,
                configFileName: pathContext.NuGetConfig,
                machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = 0,
                Take = 10,
                Prerelease = true,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "NullInfoPackage",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Normal,
                Format = PackageSearchFormat.Json
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            string message = _fixture.NormalizeNewlines(Message);
            message.Should().Contain(_fixture.ExpectedSearchResultNullInfoPackage);
        }

        [Fact]
        public async Task RunAsync_WhenPackageHasOnlyIdAndVersion_ReturnsValidMinimalVerbosityOutput()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
                pathContext.WorkingDirectory,
                configFileName: pathContext.NuGetConfig,
                machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = 0,
                Take = 10,
                Prerelease = true,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "NullInfoPackage",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Minimal,
                Format = PackageSearchFormat.Json
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            string message = _fixture.NormalizeNewlines(Message);
            message.Should().Contain(_fixture.ExpectedSearchResultNullInfoPackage);
        }

        [Fact]
        public async Task RunAsync_WhenPackageHasOnlyIdAndVersion_ReturnsValidDetailedVerbosityOutput()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new SimpleTestPathContext();
            string source = $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json";
            pathContext.Settings.AddSource(source, source, allowInsecureConnectionsValue: "true");
            ISettings settings = Settings.LoadDefaultSettings(
                pathContext.WorkingDirectory,
                configFileName: pathContext.NuGetConfig,
                machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            PackageSearchArgs packageSearchArgs = new()
            {
                Skip = 0,
                Take = 10,
                Prerelease = true,
                ExactMatch = false,
                Logger = GetLogger(),
                SearchTerm = "NullInfoPackage",
                Sources = new List<string> { $"{_fixture.ServerWithMultipleEndpoints.Uri}v3/index.json" },
                Verbosity = PackageSearchVerbosity.Detailed,
                Format = PackageSearchFormat.Json
            };

            // Act
            await PackageSearchRunner.RunAsync(
                sourceProvider: sourceProvider,
                packageSearchArgs,
                cancellationToken: System.Threading.CancellationToken.None);

            // Assert
            string message = _fixture.NormalizeNewlines(Message);
            message.Should().Contain(_fixture.ExpectedSearchResultNullInfoPackage);
        }
    }
}
