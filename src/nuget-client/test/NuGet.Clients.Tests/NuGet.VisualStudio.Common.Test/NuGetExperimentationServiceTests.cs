// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using FluentAssertions;
using Microsoft.VisualStudio.Experimentation;
using NuGet.Common;
using Test.Utility;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    [Collection(nameof(TestJoinableTaskFactoryCollection))]
    public class NuGetExperimentationServiceTests
    {
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;

        public NuGetExperimentationServiceTests()
        {
            _outputConsoleProvider = new Lazy<IOutputConsoleProvider>(() => new TestOutputConsoleProvider());
        }

        private IList<string> OutputMessages => ((TestOutputConsoleProvider)_outputConsoleProvider.Value).TestOutputConsole.Messages;

        [Fact]
        public void Constructor_WithNullWrapper_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetExperimentationService(null, new TestVisualStudioExperimentalService(), _outputConsoleProvider));
        }

        [Fact]
        public void Constructor_WithNullExperimentalService_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetExperimentationService(new TestEnvironmentVariableReader(new Dictionary<string, string>()), null, _outputConsoleProvider));
        }

        [Fact]
        public void Constructor_WithNullOutputConsoleProvider_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetExperimentationService(new TestEnvironmentVariableReader(new Dictionary<string, string>()), new TestVisualStudioExperimentalService(), null));
        }

        [Fact]
        public void IsEnabled_WithoutEnabledFlight_ReturnsFalse()
        {
            var service = new NuGetExperimentationService(new EnvironmentVariableWrapper(), new TestVisualStudioExperimentalService(), _outputConsoleProvider);
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            service.IsExperimentEnabled(constant).Should().BeFalse();
            OutputMessages.Should().NotContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*");
        }

        [Fact]
        public void IsEnabled_WithEnabledFlightAndForcedEnabledEnvVar_ReturnsTrue()
        {
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var envVars = new Dictionary<string, string>()
            {
                { constant.FlightEnvironmentVariable, "1" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var service = new NuGetExperimentationService(envVarWrapper, new TestVisualStudioExperimentalService(), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor).Should().BeTrue();
            OutputMessages.Should().ContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*1*");
        }

        [Theory]
        [InlineData("2")]
        [InlineData("randomValue")]
        public void IsEnabled_WithEnvVarWithIncorrectValue_WithEnvironmentVariable__ReturnsFalse(string value)
        {
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var envVars = new Dictionary<string, string>()
            {
                { constant.FlightEnvironmentVariable, value },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var service = new NuGetExperimentationService(envVarWrapper, new TestVisualStudioExperimentalService(), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor).Should().BeFalse();
            OutputMessages.Should().NotContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*");
        }

        [Fact]
        public void IsEnabled_WithEnabledFlight_WithExperimentalService_ReturnsTrue()
        {
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { constant.FlightFlag, true },
            };
            var service = new NuGetExperimentationService(new TestEnvironmentVariableReader(new Dictionary<string, string>()), new TestVisualStudioExperimentalService(flightsEnabled), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor).Should().BeTrue();
            OutputMessages.Should().NotContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsEnabled_WithEnvVarNotSetAndExperimentalService_ReturnsExpectedResult(bool isFlightEnabled, bool expectedResult)
        {
            var constant = ExperimentationConstants.PackageManagerBackgroundColor;
            var flightsEnabled = new Dictionary<string, bool>()
            {
                { constant.FlightFlag, isFlightEnabled },
            };
            var service = new NuGetExperimentationService(new TestEnvironmentVariableReader(new Dictionary<string, string>()), new TestVisualStudioExperimentalService(flightsEnabled), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor).Should().Be(expectedResult);
            OutputMessages.Should().NotContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*");
        }

        [Fact]
        public void IsEnabled_WithEnvVarEnabled_WithExperimentalServiceDisabled_ReturnsTrue()
        {
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

            var service = new NuGetExperimentationService(envVarWrapper, new TestVisualStudioExperimentalService(flightsEnabled), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor).Should().BeTrue();
            OutputMessages.Should().ContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*1*");
        }

        [Fact]
        public void IsEnabled_WithEnvVarDisabled_WithExperimentalServiceEnabled_ReturnsFalse()
        {
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

            var service = new NuGetExperimentationService(envVarWrapper, new TestVisualStudioExperimentalService(flightsEnabled), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.PackageManagerBackgroundColor).Should().BeFalse();
            OutputMessages.Should().ContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*0*");
        }

        [Fact]
        public void IsEnabled_WithNullEnvironmentVariableForConstant_HandlesGracefully()
        {
            var service = new NuGetExperimentationService(new EnvironmentVariableWrapper(), new TestVisualStudioExperimentalService(), _outputConsoleProvider);
            var constant = new ExperimentationConstants("flag", null);
            service.IsExperimentEnabled(constant).Should().BeFalse();
            OutputMessages.Should().NotContainMatch($"*{constant.FlightFlag}*{constant.FlightEnvironmentVariable}*");
        }

        [Fact]
        public void IsEnabled_MultipleExperimentsOverriddenWithDifferentEnvVars_DoNotConflict()
        {
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
            var service = new NuGetExperimentationService(envVarWrapper, new TestVisualStudioExperimentalService(flightsEnabled), _outputConsoleProvider);

            service.IsExperimentEnabled(forcedOffExperiment).Should().BeFalse();
            service.IsExperimentEnabled(forcedOnExperiment).Should().BeTrue();
            service.IsExperimentEnabled(noOverrideExperiment).Should().BeTrue();
            OutputMessages.Should().ContainMatch($"*{forcedOffExperiment.FlightFlag}*{forcedOffExperiment.FlightEnvironmentVariable}*0*");
            OutputMessages.Should().ContainMatch($"*{forcedOnExperiment.FlightFlag}*{forcedOnExperiment.FlightEnvironmentVariable}*1*");
            OutputMessages.Should().NotContainMatch($"*{noOverrideExperiment.FlightFlag}*{noOverrideExperiment.FlightEnvironmentVariable}*");
        }
    }

    public class TestVisualStudioExperimentalService : IExperimentationService
    {
        private readonly Dictionary<string, bool> _flights;

        public TestVisualStudioExperimentalService()
            : this(new Dictionary<string, bool>())
        {
        }

        public TestVisualStudioExperimentalService(Dictionary<string, bool> flights)
        {
            _flights = flights ?? throw new ArgumentNullException(nameof(flights));
        }

        public void Dispose()
        {
            // do nothing
        }

        public bool IsCachedFlightEnabled(string flight)
        {
            _flights.TryGetValue(flight, out bool result);
            return result;
        }

        public Task<bool> IsFlightEnabledAsync(string flight, CancellationToken token)
        {
            return Task.FromResult(IsCachedFlightEnabled(flight));
        }

        public void Start()
        {
            // do nothing.
        }
    }

    public class TestOutputConsoleProvider : IOutputConsoleProvider
    {
        public TestOutputConsole TestOutputConsole { get; } = new TestOutputConsole();

        public Task<IOutputConsole> CreateBuildOutputConsoleAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IOutputConsole> CreatePackageManagerConsoleAsync()
        {
            return Task.FromResult((IOutputConsole)TestOutputConsole);
        }

        public Task<IConsole> CreatePowerShellConsoleAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class TestOutputConsole : IOutputConsole
    {
        public IList<string> Messages { get; } = new List<string>();

        public int ConsoleWidth => throw new NotImplementedException();

        public Task ActivateAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string text, Color? foreground, Color? background)
        {
            throw new NotImplementedException();
        }

        public Task WriteBackspaceAsync()
        {
            throw new NotImplementedException();
        }

        public Task WriteLineAsync(string text)
        {
            Messages.Add(text);
            return Task.CompletedTask;
        }

        public Task WriteLineAsync(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task WriteProgressAsync(string currentOperation, int percentComplete)
        {
            throw new NotImplementedException();
        }
    }
}
