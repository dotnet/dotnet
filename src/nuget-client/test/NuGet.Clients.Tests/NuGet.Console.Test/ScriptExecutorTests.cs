// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGetConsole;
using Xunit;

namespace NuGet.Console.Test
{
    [Collection(MockedVS.Collection)]
    public class ScriptExecutorTests
    {
        [Theory]
        [InlineData("init.ps1", false, 1, false)]
        [InlineData("INIT.PS1", false, 1, false)]
        [InlineData("init.ps1", true, 0, false)]
        [InlineData("install.ps1", false, 0, false)]
        [InlineData("init.ps1", false, 1, true)]
        public async Task ExecuteAsync_WithoutExecutingCmdlet_RaisesEventOnlyForUnvisitedInitScriptAsync(
            string scriptName, bool alreadyVisited, int expectedCount, bool changeSolutionWhileCreatingConsole)
        {
            using var directory = TestDirectory.Create();
            var relativeScriptPath = Path.Combine("tools", scriptName);
            var scriptPath = Path.Combine(directory.Path, relativeScriptPath);
            Directory.CreateDirectory(Path.GetDirectoryName(scriptPath)!);
            File.WriteAllText(scriptPath, string.Empty);
            var identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("1.0.0"));

            var host = new Mock<IHost>();
            host.SetupGet(h => h.IsCommandEnabled).Returns(true);
            host.Setup(h => h.Execute(It.IsAny<IConsole>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns(true);
            var dispatcher = new Mock<IConsoleDispatcher>();
            dispatcher.Setup(d => d.StartAsync()).Returns(Task.CompletedTask);
            var console = new Mock<IConsole>();
            console.SetupGet(c => c.Host).Returns(host.Object);
            console.SetupGet(c => c.Dispatcher).Returns(dispatcher.Object);
            var consoleCreated = new TaskCompletionSource<IConsole>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (!changeSolutionWhileCreatingConsole)
            {
                consoleCreated.SetResult(console.Object);
            }

            var provider = new Mock<IOutputConsoleProvider>();
            provider.Setup(p => p.CreatePowerShellConsoleAsync()).Returns(consoleCreated.Task);
            var executor = new ScriptExecutor { OutputConsoleProvider = provider.Object };
            if (alreadyVisited)
            {
                Assert.True(executor.TryMarkVisited(identity, PackageInitPS1State.FoundAndExecuted));
            }

            var count = 0;
            long? executionSessionId = null;
            void OnInitScriptExecuted(long solutionSessionId)
            {
                executionSessionId = solutionSessionId;
                Interlocked.Increment(ref count);
            }

            NuGetPowerShellUsage.InitPs1ExecuteEvent += OnInitScriptExecuted;
            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            var originalSessionId = NuGetPowerShellUsage.SolutionSessionId;
            try
            {
                var execution = executor.ExecuteAsync(
                    identity, directory.Path, relativeScriptPath, project: null,
                    Mock.Of<INuGetProjectContext>(), throwOnFailure: true);
                if (changeSolutionWhileCreatingConsole)
                {
                    Assert.False(execution.IsCompleted);
                    NuGetPowerShellUsage.RaiseSolutionCloseEvent();
                    NuGetPowerShellUsage.RaiseSolutionOpenEvent();
                    Assert.NotEqual(originalSessionId, NuGetPowerShellUsage.SolutionSessionId);
                    consoleCreated.SetResult(console.Object);
                }

                Assert.True(await execution);

                Assert.Equal(expectedCount, count);
                Assert.Equal(expectedCount == 1 ? originalSessionId : (long?)null, executionSessionId);
                host.Verify(h => h.Execute(console.Object, It.IsAny<string>(), It.IsAny<object[]>()),
                    Times.Exactly(alreadyVisited ? 0 : 1));

                if (expectedCount == 1)
                {
                    Assert.True(await executor.ExecuteAsync(
                        identity, directory.Path, relativeScriptPath, project: null,
                        Mock.Of<INuGetProjectContext>(), throwOnFailure: true));

                    Assert.Equal(1, count);
                    host.Verify(h => h.Execute(console.Object, It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
                }
            }
            finally
            {
                NuGetPowerShellUsage.InitPs1ExecuteEvent -= OnInitScriptExecuted;
                NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            }
        }
    }
}
