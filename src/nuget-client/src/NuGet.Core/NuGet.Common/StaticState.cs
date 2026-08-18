// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    /// <summary>
    /// Process-global hook for discarding static and lingering process state when an MSBuild-driven build ends. In a
    /// host that reuses its process across builds (MSBuild Server, multithreaded MSBuild) a task can no longer rely on
    /// process exit to discard state, so each cache or live resource subscribes a reset - typically from its static
    /// constructor - and restore raises the event once the build is over, so the next build behaves as if the process
    /// had started fresh. This is the only public surface for the feature; every contributing cache or resource stays
    /// internal to its own type.
    /// </summary>
    public static class StaticState
    {
        /// <summary>
        /// Raised once when an MSBuild-driven build ends, before the process may be reused for another build.
        /// Subscribe if your type caches process-global state, or a value derived from it (an environment variable, the
        /// current directory, machine/user configuration), or holds a live OS resource (a child process, connection,
        /// timer or file handle) that the per-build "process dies after each build" model relied on process exit to
        /// reclaim.
        /// </summary>
        /// <remarks>
        /// Two rules bind every handler, and both come from bugs that shipped when they were not followed:
        /// <list type="bullet">
        /// <item><description>
        /// <b>Invalidate; do not recompute.</b> A handler must null its cache or install a fresh
        /// <see cref="Lazy{T}" /> so the value is rebuilt on first use in the next build. It must not read the
        /// environment here: the process still holds the ending build's environment, and the next build's is applied
        /// only when that build starts, so recomputing now caches the value that is on its way out.
        /// </description></item>
        /// <item><description>
        /// <b>Do not swap a resource that has work in flight.</b> Replacing and disposing a live object - a semaphore,
        /// an open writer, a child process - breaks callers that captured the previous instance, and for a
        /// synchronization primitive it also defeats the guarantee it exists for, since holders of the old instance run
        /// alongside acquirers of the new one. Such state belongs to the operation that owns it and should be scoped
        /// there rather than shared in a static and periodically replaced.
        /// </description></item>
        /// </list>
        /// </remarks>
        public static event Action? BuildEnded;

        /// <summary>
        /// Raises <see cref="BuildEnded" />. Handlers are expected not to throw; one that tears down an external
        /// resource is responsible for guarding itself, so a genuine bug in a reset surfaces rather than being silently
        /// swallowed.
        /// </summary>
        public static void RaiseBuildEnded() => BuildEnded?.Invoke();
    }
}
