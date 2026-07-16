// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Framework;
using NuGet.Common;
using Task = Microsoft.Build.Utilities.Task;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Raises <see cref="StaticState.StartMSBuildRestoreTasks" /> to refresh NuGet's environment-derived static
    /// caches at the start of a restore, so an MSBuild process reused across builds (Server, multithreaded) observes
    /// the current environment. Wired into restore by the <c>_RefreshNuGetStaticState</c> target in NuGet.targets.
    /// </summary>
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
                // Not expected to throw; if it does, surface it as a diagnosable build error instead of an unhandled crash.
                Log.LogErrorFromException(e, showStackTrace: true);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
