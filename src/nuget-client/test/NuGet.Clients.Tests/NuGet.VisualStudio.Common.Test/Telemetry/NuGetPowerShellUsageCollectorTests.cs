// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Moq;
using NuGet.Common;
using NuGet.VisualStudio.Common.Telemetry;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.Telemetry
{
    [CollectionDefinition(nameof(NuGetPowerShellUsageCollection), DisableParallelization = true)]
    public sealed class NuGetPowerShellUsageCollection
    {
    }

    [Collection(nameof(NuGetPowerShellUsageCollection))]
    public class NuGetPowerShellUsageCollectorTests
    {
        private const string PmcCommands = "PowerShellHost.PmcExecuteCommandCount";
        private const string InitScripts = "PowerShellHost.InitPs1ExecuteCount";
        private const string PmuiCommands = "PowerShellHost.PmuiExecuteCommandCount";

        [Fact]
        public void InitPs1Execute_AutomaticExecutions_CountsAttemptsWithoutAddingCommands()
        {
            var events = new List<TelemetryEvent>();
            var telemetryService = new Mock<INuGetTelemetryService>();
            telemetryService.Setup(s => s.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(events.Add);
            using var collector = new NuGetPowerShellUsageCollector(telemetryService.Object);

            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            var solutionSessionId = NuGetPowerShellUsage.SolutionSessionId;
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(solutionSessionId);
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(solutionSessionId);
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            var instanceEvent = InstanceCloseTelemetryEmitter.CreateTelemetryEvent();

            var solutionEvent = Assert.Single(events);
            Assert.Equal("SolutionClose", solutionEvent.Name);
            Assert.Equal(2, solutionEvent[InitScripts]);
            Assert.Equal(2, instanceEvent[InitScripts]);
            Assert.Equal(0, solutionEvent[PmcCommands]);
            Assert.Equal(0, instanceEvent[PmcCommands]);
            Assert.Equal(0, solutionEvent[PmuiCommands]);
            Assert.Equal(0, instanceEvent[PmuiCommands]);
        }

        [Fact]
        public void SolutionClose_ResetsSolutionCountAndRetainsInstanceCount()
        {
            var events = new List<TelemetryEvent>();
            var telemetryService = new Mock<INuGetTelemetryService>();
            telemetryService.Setup(s => s.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(events.Add);
            using var collector = new NuGetPowerShellUsageCollector(telemetryService.Object);

            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(NuGetPowerShellUsage.SolutionSessionId);
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(NuGetPowerShellUsage.SolutionSessionId);
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(NuGetPowerShellUsage.SolutionSessionId);
            NuGetPowerShellUsage.RaiseCommandExecuteEvent(isPMC: true);
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            var instanceEvent = InstanceCloseTelemetryEmitter.CreateTelemetryEvent();

            Assert.Equal(3, events.Count);
            Assert.Equal(1, events[0][InitScripts]);
            Assert.Equal(2, events[1][InitScripts]);
            Assert.Equal(0, events[2][InitScripts]);
            Assert.Equal(0, events[0][PmcCommands]);
            Assert.Equal(1, events[1][PmcCommands]);
            Assert.Equal(0, events[2][PmcCommands]);
            Assert.Equal(3, instanceEvent[InitScripts]);
            Assert.Equal(1, instanceEvent[PmcCommands]);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void InitPs1Execute_AfterOriginatingSolutionCloses_CountsOnlyForInstance(bool openAnotherSolution)
        {
            var events = new List<TelemetryEvent>();
            var telemetryService = new Mock<INuGetTelemetryService>();
            telemetryService.Setup(s => s.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(events.Add);
            using var collector = new NuGetPowerShellUsageCollector(telemetryService.Object);

            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            var originalSessionId = NuGetPowerShellUsage.SolutionSessionId;
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(originalSessionId);
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            var firstInstanceSnapshot = InstanceCloseTelemetryEmitter.CreateTelemetryEvent();
            if (openAnotherSolution)
            {
                NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            }

            var currentSessionId = NuGetPowerShellUsage.SolutionSessionId;
            Assert.NotEqual(originalSessionId, currentSessionId);
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(originalSessionId);
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            var finalInstanceSnapshot = InstanceCloseTelemetryEmitter.CreateTelemetryEvent();

            Assert.Equal(2, events.Count);
            Assert.Equal(1, events[0][InitScripts]);
            Assert.Equal(0, events[1][InitScripts]);
            Assert.Equal(1, firstInstanceSnapshot[InitScripts]);
            Assert.Equal(2, finalInstanceSnapshot[InitScripts]);
        }

        [Fact]
        public void InitPs1Execute_BeforeSolutionOpens_DoesNotContributeToSolution()
        {
            var events = new List<TelemetryEvent>();
            var telemetryService = new Mock<INuGetTelemetryService>();
            telemetryService.Setup(s => s.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(events.Add);
            using var collector = new NuGetPowerShellUsageCollector(telemetryService.Object);

            var noSolutionSessionId = NuGetPowerShellUsage.SolutionSessionId;
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(noSolutionSessionId);
            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(noSolutionSessionId);
            NuGetPowerShellUsage.RaiseInitPs1ExecuteEvent(NuGetPowerShellUsage.SolutionSessionId);
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            var instanceEvent = InstanceCloseTelemetryEmitter.CreateTelemetryEvent();

            Assert.Equal(1, Assert.Single(events)[InitScripts]);
            Assert.Equal(3, instanceEvent[InitScripts]);
        }
    }
}
