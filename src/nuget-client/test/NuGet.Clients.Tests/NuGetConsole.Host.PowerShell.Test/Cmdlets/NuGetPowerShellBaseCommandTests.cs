// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.PowerShellCmdlets;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using Xunit;
using PSCommand = System.Management.Automation.Runspaces.Command;

namespace NuGetConsole.Host.PowerShell.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetPowerShellBaseCommandTests : IAsyncServiceProvider
    {
        public NuGetPowerShellBaseCommandTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();
            var componentModel = new Mock<IComponentModel>();
            componentModel.Setup(x => x.GetService<ISettings>()).Returns(Mock.Of<ISettings>());
            globalServiceProvider.AddService(typeof(SComponentModel), componentModel.Object);
            ServiceLocator.InitializePackageServiceProvider(this);
        }

        [Theory]
        [InlineData("init.ps1", false, 1, false)]
        [InlineData("INIT.PS1", false, 1, false)]
        [InlineData("init.ps1", true, 1, false)]
        [InlineData("install.ps1", false, 0, false)]
        [InlineData("uninstall.ps1", false, 0, false)]
        [InlineData("notinit.ps1", false, 0, false)]
        [InlineData("init.ps1", false, 2, true)]
        public void ExecutePSScriptInternal_CountsOnlyInitScriptAttempts(
            string scriptName, bool scriptThrows, int expectedCount, bool changeSolutionBetweenScripts)
        {
            using var directory = TestDirectory.Create();
            var scriptPath = Path.Combine(directory.Path, scriptName);
            File.WriteAllText(scriptPath, "$global:PackageScriptExecuted = $true" + (scriptThrows ? "; throw 'script failure'" : ""));

            var sessionState = InitialSessionState.CreateDefault();
            sessionState.Commands.Add(new SessionStateCmdletEntry("Invoke-TestPackageScript", typeof(InvokePackageScriptCommand), null));
            using var runspace = RunspaceFactory.CreateRunspace(new TestPSHost(directory.Path), sessionState);
            runspace.Open();
            using var pipeline = runspace.CreatePipeline();
            var command = new PSCommand("Invoke-TestPackageScript");
            command.Parameters.Add("ScriptPath", scriptPath);
            command.Parameters.Add("ChangeSolutionBetweenScripts", changeSolutionBetweenScripts);
            pipeline.Commands.Add(command);

            var executionSessions = new List<long>();
            void OnInitScriptExecuted(long solutionSessionId) => executionSessions.Add(solutionSessionId);
            NuGetPowerShellUsage.InitPs1ExecuteEvent += OnInitScriptExecuted;
            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            var originalSessionId = NuGetPowerShellUsage.SolutionSessionId;
            try
            {
                var results = pipeline.Invoke();

                Assert.True(Assert.IsType<bool>(Assert.Single(results).BaseObject));
                Assert.Equal(expectedCount, executionSessions.Count);
                Assert.All(executionSessions, id => Assert.Equal(originalSessionId, id));
                if (changeSolutionBetweenScripts)
                {
                    Assert.NotEqual(originalSessionId, NuGetPowerShellUsage.SolutionSessionId);
                }
            }
            finally
            {
                NuGetPowerShellUsage.InitPs1ExecuteEvent -= OnInitScriptExecuted;
                NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            }
        }

        public Task<object?> GetServiceAsync(Type serviceType)
        {
            return Task.FromResult<object?>(null);
        }

        [Cmdlet(VerbsLifecycle.Invoke, "TestPackageScript")]
        public sealed class InvokePackageScriptCommand : NuGetPowerShellBaseCommand
        {
            [Parameter(Mandatory = true)]
            public string ScriptPath { get; set; } = string.Empty;

            [Parameter]
            public bool ChangeSolutionBetweenScripts { get; set; }

            protected override bool IsLoggingTimeDisabled => true;

            protected override void ProcessRecordCore()
            {
                ExecutePSScriptInternal(ScriptPath);
                if (ChangeSolutionBetweenScripts)
                {
                    NuGetPowerShellUsage.RaiseSolutionCloseEvent();
                    NuGetPowerShellUsage.RaiseSolutionOpenEvent();
                    ExecutePSScriptInternal(ScriptPath);
                }

                WriteObject(SessionState.PSVariable.GetValue("PackageScriptExecuted"));
            }

            protected override void LogCore(MessageLevel level, string formattedMessage)
            {
            }
        }
    }
}
