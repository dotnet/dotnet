// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections;
using Microsoft.Build.Framework;
using NuGet.Common;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class RestoreTaskTests
    {
        // StaticState exposes process-global events, and there is no way to clear their subscriptions, so each test
        // subscribes a private marker handler, asserts only on its own local counter, and unsubscribes in a finally.

        [Fact]
        public void Execute_MultipleRestoresInOneBuild_DoesNotEndRestoreStateUntilTheBuildEnds()
        {
            var buildEngine = new TestBuildEngine();
            int ended = 0;
            Action handler = () => ended++;
            StaticState.BuildEnded += handler;

            try
            {
                // A build can restore more than once - the Arcade SDK restores its toolset and then the solution from
                // a single target - and the second restore is entitled to reuse the plugin processes the first one
                // started. Neither restore may end the shared restore state.
                Execute(buildEngine);
                Assert.Equal(0, ended);

                Execute(buildEngine);
                Assert.Equal(0, ended);

                // MSBuild disposes build-lifetime task objects when the build ends, including before it reuses the
                // node for the next build. That is when the state must be torn down, exactly once.
                buildEngine.DisposeRegisteredTaskObjects(RegisteredTaskObjectLifetime.Build);
                Assert.Equal(1, ended);
            }
            finally
            {
                StaticState.BuildEnded -= handler;
            }
        }

        [Fact]
        public void Execute_BuildEngineDoesNotSupportRegisteredTaskObjects_EndsRestoreStateImmediately()
        {
            var buildEngine = new LegacyTestBuildEngine();
            int ended = 0;
            Action handler = () => ended++;
            StaticState.BuildEnded += handler;

            try
            {
                // A host that cannot defer the teardown to the end of the build must still get one, so fall back to
                // ending the restore state here rather than leaking plugin processes into a reused process.
                Execute(buildEngine);

                Assert.Equal(1, ended);
            }
            finally
            {
                StaticState.BuildEnded -= handler;
            }
        }

        private static void Execute(IBuildEngine buildEngine)
        {
            using (var task = new RestoreTask
            {
                BuildEngine = buildEngine,
                RestoreGraphItems = Array.Empty<ITaskItem>(),
                HideWarningsAndErrors = true,
            })
            {
                Assert.True(task.Execute());
            }
        }

        /// <summary>
        /// A build engine that predates <see cref="IBuildEngine4" /> and so cannot hold an object for the lifetime of
        /// the build.
        /// </summary>
        private sealed class LegacyTestBuildEngine : IBuildEngine
        {
            public int ColumnNumberOfTaskNode => 0;

            public bool ContinueOnError => false;

            public int LineNumberOfTaskNode => 0;

            public string ProjectFileOfTaskNode => string.Empty;

            public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs) => true;

            public void LogCustomEvent(CustomBuildEventArgs e) { }

            public void LogErrorEvent(BuildErrorEventArgs e) { }

            public void LogMessageEvent(BuildMessageEventArgs e) { }

            public void LogWarningEvent(BuildWarningEventArgs e) { }
        }
    }
}
