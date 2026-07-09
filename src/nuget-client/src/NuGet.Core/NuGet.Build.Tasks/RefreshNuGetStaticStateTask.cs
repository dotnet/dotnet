// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Framework;
using NuGet.Common;
using Task = Microsoft.Build.Utilities.Task;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Refreshes NuGet's environment-derived static state at the very start of a restore by raising
    /// <see cref="StaticState.StartMSBuildRestoreTasks" />. It is wired as the first dependency of the
    /// <c>_GenerateRestoreGraph</c> target so it runs exactly once per restore, before any of the
    /// <c>Get*</c> collection tasks and before <c>RestoreTask</c> / <c>WriteRestoreGraphTask</c>, in the single
    /// MSBuild process that hosts them. In a process reused across builds (MSBuild Server, multithreaded MSBuild)
    /// a task can no longer rely on process exit to discard state; running this first lets the whole restore observe
    /// the current environment instead of caches frozen at the first build.
    /// </summary>
    /// <remarks>
    /// The task body touches no process-global state itself (no environment variables, current directory, spawned
    /// processes or file paths), so it carries <see cref="MSBuildMultiThreadableTaskAttribute" /> like the other
    /// restore graph tasks. The state it refreshes is process-global, which is only safe because MSBuild schedules
    /// this target ahead of every other restore task for the same restore; see the design notes for the ordering
    /// guarantees this relies on.
    /// </remarks>
    [MSBuildMultiThreadableTask]
    public sealed class RefreshNuGetStaticStateTask : Task
    {
        public RefreshNuGetStaticStateTask()
            : base(Strings.ResourceManager)
        {
        }

        public override bool Execute()
        {
            try
            {
                StaticState.RaiseStartMSBuildRestoreTasks();
            }
            catch (Exception e)
            {
                // Refreshing the environment-derived caches is not expected to throw. If it does it is an unexpected
                // scenario, so surface it as a message (not an error or warning) and continue: a failed refresh must
                // not fail the user's build. The restore proceeds with the existing cached state.
                Log.LogMessageFromResources(MessageImportance.High, nameof(Strings.RefreshNuGetStaticState_UnexpectedError), e.Message);
            }

            return true;
        }
    }
}
