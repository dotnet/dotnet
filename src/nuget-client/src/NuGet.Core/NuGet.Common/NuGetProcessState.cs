// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;

namespace NuGet.Common
{
    /// <summary>
    /// A small, generic registry in the lowest-common NuGet assembly. Any NuGet code can contribute a reset
    /// action for a <see cref="ResetKey" />, and a caller (restore) can execute all actions for one key. This is
    /// the only public surface for the feature: each cache or resource stays internal to its own type and simply
    /// registers a delegate (typically from its static constructor); restore invokes them by key so a host that
    /// reuses the process across builds behaves as if it had started fresh.
    /// </summary>
    public static class NuGetProcessState
    {
        /// <summary>The reset points restore drives.</summary>
        public enum ResetKey
        {
            /// <summary>
            /// State that must be refreshed at the start of a restore - chiefly caches derived from
            /// environment variables, which may have changed since a previous build in a reused process.
            /// </summary>
            StartRestore,

            /// <summary>
            /// State that must be torn down at the end of a restore - chiefly live OS resources (such as plugin
            /// processes) that the "process dies after each build" model relied on process exit to reclaim.
            /// </summary>
            EndRestore,
        }

        private static readonly ConcurrentDictionary<ResetKey, ConcurrentBag<Action>> ResetActions =
            new ConcurrentDictionary<ResetKey, ConcurrentBag<Action>>();

        /// <summary>
        /// Registers a reset action for <paramref name="key" />. Actions accumulate (a keyed list), so each
        /// contributor registers once - typically from a type's static constructor.
        /// </summary>
        /// <param name="key">The reset point, <see cref="ResetKey.StartRestore" /> or <see cref="ResetKey.EndRestore" />.</param>
        /// <param name="resetAction">The action that re-reads / clears / tears down the state.</param>
        public static void RegisterResetAction(ResetKey key, Action resetAction)
        {
            if (resetAction == null)
            {
                throw new ArgumentNullException(nameof(resetAction));
            }

            ResetActions.GetOrAdd(key, _ => new ConcurrentBag<Action>()).Add(resetAction);
        }

        /// <summary>
        /// Executes every reset action registered for <paramref name="key" />. Best-effort and isolated: a
        /// failure in one action does not prevent the others from running.
        /// </summary>
        public static void Reset(ResetKey key)
        {
            if (!ResetActions.TryGetValue(key, out ConcurrentBag<Action>? actions))
            {
                return;
            }

            foreach (Action action in actions)
            {
                try
                {
                    action();
                }
#pragma warning disable CA1031 // Do not catch general exception types - resets are best-effort and isolated.
                catch (Exception ex)
#pragma warning restore CA1031
                {
                    System.Diagnostics.Debug.WriteLine($"NuGetProcessState reset action for '{key}' failed: {ex}");
                }
            }
        }
    }
}
