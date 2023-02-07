// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.Experimentation;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.Common;
using Test.Utility;
using Test.Utility.VisualStudio;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetExperimentationServiceTests
    {
        private readonly Mock<IOutputConsoleProvider> _outputConsoleProviderMock;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;
        private readonly Mock<IOutputConsole> _outputConsoleMock;

        public NuGetExperimentationServiceTests(GlobalServiceProvider sp)
        {
            sp.Reset();

            var mockOutputConsoleUtility = OutputConsoleUtility.GetMock();
            _outputConsoleProviderMock = mockOutputConsoleUtility.mockIOutputConsoleProvider;
            _outputConsoleProvider = new Lazy<IOutputConsoleProvider>(() => _outputConsoleProviderMock.Object);
            _outputConsoleMock = mockOutputConsoleUtility.mockIOutputConsole;
        }

        [Fact]
        public void Constructor_WithNullEnvironmentVariableReader_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetExperimentationService(environmentVariableReader: null, Mock.Of<IExperimentationService>(), Mock.Of<Lazy<IOutputConsoleProvider>>()));
        }

        [Fact]
        public void Constructor_WithNullExperimentalService_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetExperimentationService(Mock.Of< IEnvironmentVariableReader>(), experimentationService: null, _outputConsoleProvider));
        }

        [Fact]
        public void Constructor_WithNullOutputConsoleProvider_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetExperimentationService(Mock.Of<IEnvironmentVariableReader>(), Mock.Of<IExperimentationService>(), outputConsoleProvider: null));
        }

        [Fact]
        public void IsEnabled_WithoutEnabledFlight_ReturnsFalse()
        {
            // Arrange
            var service = new NuGetExperimentationService(Mock.Of<IEnvironmentVariableReader>(), NuGetExperimentationServiceUtility.GetMock(), _outputConsoleProvider);
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(constant);

            // Assert
            isExperimentEnabled.Should().BeFalse();
            _outputConsoleProviderMock.Verify(_ => _.CreatePackageManagerConsoleAsync(), Times.Never);
        }

        [Fact]
        public void IsEnabled_WithEnabledFlightAndForcedEnabledEnvVar_ReturnsTrue()
        {
            // Arrange
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var envVars = new Dictionary<string, string>()
            {
                { constant.FlightEnvironmentVariable, "1" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var service = new NuGetExperimentationService(envVarWrapper, NuGetExperimentationServiceUtility.GetMock(), _outputConsoleProvider);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);

            // Assert
            isExperimentEnabled.Should().BeTrue();
            _outputConsoleMock.Verify(_ => _.WriteLineAsync(It.IsRegex(constant.FlightFlag + "(.*)" + constant.FlightEnvironmentVariable + "(.*)1")));
        }

        [Theory]
        [InlineData("2")]
        [InlineData("randomValue")]
        public void IsEnabled_WithEnvVarWithIncorrectValue_WithEnvironmentVariable_ReturnsFalse(string value)
        {
            // Arrange
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var envVars = new Dictionary<string, string>()
            {
                { constant.FlightEnvironmentVariable, value },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var service = new NuGetExperimentationService(envVarWrapper, NuGetExperimentationServiceUtility.GetMock(), _outputConsoleProvider);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);

            // Assert
            isExperimentEnabled.Should().BeFalse();
            _outputConsoleProviderMock.Verify(_ => _.CreatePackageManagerConsoleAsync(), Times.Never);
        }

        [Fact]
        public void IsEnabled_WithEnabledFlight_WithExperimentalService_ReturnsTrue()
        {
            // Arrange
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { constant.FlightFlag, true },
            };
            var service = new NuGetExperimentationService(Mock.Of<IEnvironmentVariableReader>(), NuGetExperimentationServiceUtility.GetMock(flightsEnabled), _outputConsoleProvider);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);

            // Assert
            isExperimentEnabled.Should().BeTrue();
            _outputConsoleProviderMock.Verify(_ => _.CreatePackageManagerConsoleAsync(), Times.Never);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsEnabled_WithEnvVarNotSetAndExperimentalService_ReturnsExpectedResult(bool isFlightEnabled, bool expectedResult)
        {
            // Arrange
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { constant.FlightFlag, isFlightEnabled },
            };
            var service = new NuGetExperimentationService(Mock.Of<IEnvironmentVariableReader>(), NuGetExperimentationServiceUtility.GetMock(flightsEnabled), _outputConsoleProvider);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);

            // Assert
            isExperimentEnabled.Should().Be(expectedResult);
            _outputConsoleProviderMock.Verify(_ => _.CreatePackageManagerConsoleAsync(), Times.Never);
        }

        [Fact]
        public void IsEnabled_WithEnvVarEnabled_WithExperimentalServiceDisabled_ReturnsTrue()
        {
            // Arrange
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { constant.FlightFlag, false },
            };

            var envVars = new Dictionary<string, string>()
            {
                { constant.FlightEnvironmentVariable, "1" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);

            var service = new NuGetExperimentationService(envVarWrapper, NuGetExperimentationServiceUtility.GetMock(flightsEnabled), _outputConsoleProvider);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);

            // Assert
            isExperimentEnabled.Should().BeTrue();
            _outputConsoleMock.Verify(_ => _.WriteLineAsync(It.IsRegex(constant.FlightFlag + "(.*)" + constant.FlightEnvironmentVariable + "(.*)1")));
        }

        [Fact]
        public void IsEnabled_WithEnvVarDisabled_WithExperimentalServiceEnabled_ReturnsFalse()
        {
            // Arrange
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { constant.FlightFlag, true },
            };

            var envVars = new Dictionary<string, string>()
            {
                { constant.FlightEnvironmentVariable, "0" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);

            var service = new NuGetExperimentationService(envVarWrapper, NuGetExperimentationServiceUtility.GetMock(flightsEnabled), _outputConsoleProvider);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor);

            // Assert
            isExperimentEnabled.Should().BeFalse();
            _outputConsoleMock.Verify(_ => _.WriteLineAsync(It.IsRegex(constant.FlightFlag + "(.*)" + constant.FlightEnvironmentVariable + "(.*)0")));
        }

        [Fact]
        public void IsEnabled_WithNullEnvironmentVariableForConstant_HandlesGracefully()
        {
            // Arrange
            var service = new NuGetExperimentationService(Mock.Of<IEnvironmentVariableReader>(), NuGetExperimentationServiceUtility.GetMock(), _outputConsoleProvider);
            var constant = new ExperimentationConstants("flag", null);

            // Act
            bool isExperimentEnabled = service.IsExperimentEnabled(constant);

            // Assert
            isExperimentEnabled.Should().BeFalse();
            _outputConsoleProviderMock.Verify(_ => _.CreatePackageManagerConsoleAsync(), Times.Never);
        }

        [Fact]
        public void IsEnabled_MultipleExperimentsOverriddenWithDifferentEnvVars_DoNotConflict()
        {
            // Arrange
            var forcedOffExperiment = new ExperimentationConstants("TestExp1", "TEST_EXP_1");
            var forcedOnExperiment = new ExperimentationConstants("TestExp2", "TEST_EXP_2");
            var noOverrideExperiment = new ExperimentationConstants("TestExp3", "TEST_EXP_3");
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { forcedOffExperiment.FlightFlag, true },
                { forcedOnExperiment.FlightFlag, true },
                { noOverrideExperiment.FlightFlag, true },
            };
            var envVars = new Dictionary<string, string>()
            {
                { forcedOnExperiment.FlightEnvironmentVariable, "1" },
                { forcedOffExperiment.FlightEnvironmentVariable, "0" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var service = new NuGetExperimentationService(envVarWrapper, NuGetExperimentationServiceUtility.GetMock(flightsEnabled), _outputConsoleProvider);

            // Act
            bool isForcedOffExperimentEnabled = service.IsExperimentEnabled(forcedOffExperiment);
            bool isForcedOnExperimentEnabled = service.IsExperimentEnabled(forcedOnExperiment);
            bool isNoOverrideExperimentEnabled = service.IsExperimentEnabled(noOverrideExperiment);

            // Assert
            isForcedOffExperimentEnabled.Should().BeFalse();
            isForcedOnExperimentEnabled.Should().BeTrue();
            isNoOverrideExperimentEnabled.Should().BeTrue();

            _outputConsoleMock.Verify(_ => _.WriteLineAsync(It.IsRegex(forcedOffExperiment.FlightFlag + "(.*)" + forcedOffExperiment.FlightEnvironmentVariable + "(.*)0")));
            _outputConsoleMock.Verify(_ => _.WriteLineAsync(It.IsRegex(forcedOnExperiment.FlightFlag + "(.*)" + forcedOnExperiment.FlightEnvironmentVariable + "(.*)1")));
            _outputConsoleMock.VerifyNoOtherCalls();
        }
    }
}
