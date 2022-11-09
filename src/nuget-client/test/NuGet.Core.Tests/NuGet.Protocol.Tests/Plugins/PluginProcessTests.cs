// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginProcessTests
    {
        [Fact]
        public void Constructor_WhenStartInfoIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PluginProcess(startInfo: null));

            Assert.Equal("startInfo", exception.ParamName);
        }

        [PlatformFact(Platform.Windows)]
        public void Exited_WhenProcessExits_Fires()
        {
            ProcessStartInfo startInfo = CreateProcessStartInfo();

            using (var exitedEvent = new ManualResetEventSlim(initialState: false))
            using (var pluginProcess = new PluginProcess(startInfo))
            {
                IPluginProcess argument = null;

                pluginProcess.Exited += (object sender, IPluginProcess e) =>
                {
                    argument = e;

                    exitedEvent.Set();
                };

                pluginProcess.Start();

                exitedEvent.Wait();

                Assert.Same(pluginProcess, argument);
            }
        }

        [Fact]
        public void ExitCode_WithUnstartedProcess_IsNull()
        {
            TestWithUnstartedProcess(pluginProcess => Assert.Null(pluginProcess.ExitCode));
        }

        [Fact]
        public void ExitCode_WithRunningProcess_IsNull()
        {
            TestWithRunningProcess((process, pluginProcess) => Assert.Null(pluginProcess.ExitCode));
        }

        [PlatformFact(Platform.Windows)]
        public void ExitCode_WithExitedProcess_IsNotNull()
        {
            TestWithExitedProcess(pluginProcess => Assert.NotNull(pluginProcess.ExitCode));
        }

        [Fact]
        public void FilePath_WithUnstartedProcess_Throws()
        {
            TestWithUnstartedProcess(pluginProcess => Assert.Throws<InvalidOperationException>(() => pluginProcess.FilePath));
        }

        [Fact]
        public void FilePath_WithRunningProcess_IsValid()
        {
            TestWithRunningProcess((process, pluginProcess) => Assert.Equal(process.MainModule.FileName, pluginProcess.FilePath));
        }

        [Fact]
        public void Id_WithUnstartedProcess_IsNull()
        {
            TestWithUnstartedProcess(pluginProcess => Assert.Null(pluginProcess.Id));
        }

        [Fact]
        public void Id_WithRunningProcess_IsValid()
        {
            TestWithRunningProcess((process, pluginProcess) => Assert.Equal(process.Id, pluginProcess.Id));
        }

        [PlatformFact(Platform.Windows)]
        public void Id_WithExitedProcess_IsNotNull()
        {
            TestWithExitedProcess(pluginProcess => Assert.NotNull(pluginProcess.Id));
        }

        [PlatformFact(Platform.Windows)]
        public void Kill_WhenCalled_TerminatesProcess()
        {
            var startInfo = new ProcessStartInfo("timeout")
            {
                Arguments = "1000", // seconds
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var exitedEvent = new ManualResetEventSlim(initialState: false))
            using (var pluginProcess = new PluginProcess(startInfo))
            {
                pluginProcess.Exited += (object sender, IPluginProcess e) =>
                {
                    exitedEvent.Set();
                };

                pluginProcess.Start();

                pluginProcess.Kill();

                exitedEvent.Wait();

                Assert.NotNull(pluginProcess.ExitCode);
            }
        }

        private static ProcessStartInfo CreateProcessStartInfo()
        {
            return new ProcessStartInfo("find")
            {
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        private static void TestWithUnstartedProcess(Action<PluginProcess> verify)
        {
            using (var pluginProcess = new PluginProcess(new ProcessStartInfo()))
            {
                verify(pluginProcess);
            }
        }

        private static void TestWithRunningProcess(Action<Process, PluginProcess> verify)
        {
            using (var process = Process.GetCurrentProcess())
            using (var pluginProcess = new PluginProcess())
            {
                verify(process, pluginProcess);
            }
        }

        private static void TestWithExitedProcess(Action<PluginProcess> verify)
        {
            ProcessStartInfo startInfo = CreateProcessStartInfo();

            using (var pluginProcess = new PluginProcess(startInfo))
            {
                pluginProcess.Start();

                pluginProcess.Kill();

                verify(pluginProcess);
            }
        }
    }
}
