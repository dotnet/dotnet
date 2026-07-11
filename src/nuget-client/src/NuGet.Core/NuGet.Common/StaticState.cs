// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    /// <summary>
    /// Process-global hooks for refreshing or tearing down static and lingering process state around an
    /// MSBuild-driven restore. In a host that reuses its process across builds (MSBuild Server, multithreaded
    /// MSBuild) a task can no longer rely on process exit to discard state, so each cache or live resource
    /// subscribes a reset to one of these events - typically from its static constructor - and restore raises
    /// them, so the build behaves as if the process had started fresh. This is the only public surface for the
    /// feature; every contributing cache or resource stays internal to its own type.
    /// </summary>
    public static class StaticState
    {
        /// <summary>
        /// Raised at the start of an MSBuild-driven restore, before any restore work runs. Subscribe if your type
        /// caches process-global state, or a value derived from it (an environment variable, the current directory,
        /// machine/user configuration) - for example a static field initialized as
        /// <c>HomePath = NuGetEnvironment.GetFolderPath(...)</c>. Such a value is read once and would otherwise stay
        /// frozen for the life of a reused process; the handler must recompute it from the current environment so a
        /// later build observes the current value.
        /// </summary>
        public static event Action? StartMSBuildRestoreTasks;

        /// <summary>
        /// Raised at the end of an MSBuild-driven restore. Subscribe if your type holds a live OS resource (a child
        /// process, connection, timer or file handle) that the per-build "process dies after each build" model relied
        /// on process exit to reclaim; the handler must tear it down so it does not linger in a reused process.
        /// </summary>
        public static event Action? EndMSBuildRestoreTasks;

        /// <summary>
        /// Raises <see cref="StartMSBuildRestoreTasks" />. Handlers are expected not to throw; one that can fail
        /// (for example, one that re-reads a value that could be malformed) is responsible for guarding itself, so
        /// the contract here stays honest and a genuine bug in a reset surfaces rather than being silently
        /// swallowed.
        /// </summary>
        public static void RaiseStartMSBuildRestoreTasks() => StartMSBuildRestoreTasks?.Invoke();

        /// <summary>
        /// Raises <see cref="EndMSBuildRestoreTasks" />. Handlers are expected not to throw; one that tears down an
        /// external resource is responsible for guarding itself, so a genuine bug in a reset surfaces rather than
        /// being silently swallowed.
        /// </summary>
        public static void RaiseEndMSBuildRestoreTasks() => EndMSBuildRestoreTasks?.Invoke();
    }
}
