// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;

namespace Microsoft.Build.BackEnd
{
    /// <summary>
    /// Determines where a task should be executed in multi-threaded mode.
    /// In multi-threaded execution mode, tasks implementing IMultiThreadableTask or marked with
    /// MSBuildMultiThreadableTaskAttribute run in-process within thread nodes, while legacy tasks
    /// are routed to sidecar TaskHost processes for isolation.
    /// </summary>
    /// <remarks>
    /// This class should only be used when in multi-threaded mode. Traditional multi-proc builds
    /// have different semantics and should not use this routing logic.
    /// </remarks>
    internal static class TaskRouter
    {
        /// <summary>
        /// Cache of task types to their multi-threadable attribute status.
        /// This avoids repeated reflection calls for the same task types.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> s_multiThreadableTaskCache = new();

    /// <summary>
    /// Determines if a task needs to be routed to an out-of-process TaskHost sidecar
    /// in multi-threaded mode based on its thread-safety characteristics.
    /// </summary>
    /// <param name="taskType">The type of the task to evaluate.</param>
    /// <returns>
    /// True if the task should be executed in an out-of-process TaskHost sidecar;
    /// false if it can safely run in-process within a thread node.
    /// </returns>
    /// <remarks>
    /// This method only considers the task's thread-safety indicators.
    /// The caller is responsible for:
    /// - Only calling this in multi-threaded mode
    /// - Handling explicit out-of-proc requests (via TaskHostFactory or parameters)
    /// - Handling the isAlreadyOutOfProc scenario
    /// 
    /// In multi-threaded mode:
    /// - Tasks marked with MSBuildMultiThreadableTaskAttribute (non-inheritable) are considered
    ///   thread-safe and can run in-process (returns false)
    /// - Tasks without the attribute must run in a sidecar TaskHost for isolation (returns true)
    /// </remarks>
    public static bool NeedsTaskHostInMultiThreadedMode(Type taskType)
    {
        ArgumentNullException.ThrowIfNull(taskType);

        // NuGet's RestoreTask is not annotated as multi-threadable, but it now resets its own process-wide static
        // state at the start and end of every restore (via NuGet's reset registry). It therefore runs in the main
        // process rather than an isolated TaskHost: the reset keeps the long-lived process clean across restores,
        // which replaces the previous transient-TaskHost workaround for https://github.com/dotnet/msbuild/issues/13315.
        if (RunsInMainProcessDespiteNoAttribute(taskType))
        {
            return false;
        }

        // Tasks without the thread-safety attribute need isolation in a TaskHost sidecar
        return !HasMultiThreadableTaskAttribute(taskType);
    }

        /// <summary>
        /// Checks if a task type is marked with MSBuildMultiThreadableTaskAttribute.
        /// Detection is based on namespace and name only, ignoring the defining assembly,
        /// which allows customers to define the attribute in their own assemblies.
        /// Results are cached to avoid repeated reflection calls.
        /// </summary>
        /// <param name="taskType">The task type to check.</param>
        /// <returns>True if the task has the attribute; false otherwise.</returns>
        private static bool HasMultiThreadableTaskAttribute(Type taskType)
        {
            return s_multiThreadableTaskCache.GetOrAdd(
                taskType,
                static t =>
                {
                    const string attributeFullName = "Microsoft.Build.Framework.MSBuildMultiThreadableTaskAttribute";

                    // Check for the attribute by full name, not by type identity
                    // This allows custom-defined attributes from different assemblies
                    foreach (object attr in t.GetCustomAttributes(inherit: false))
                    {
                        if (attr.GetType().FullName == attributeFullName)
                        {
                            return true;
                        }
                    }

                    return false;
                });
        }

        /// <summary>
        /// Full name of a task that runs in the main process in multi-threaded / long-lived-host mode even though it
        /// carries no <c>MSBuildMultiThreadableTaskAttribute</c>. NuGet's <c>RestoreTask</c> qualifies because it
        /// resets its own process-wide static state around every restore, so it neither needs a sidecar TaskHost for
        /// isolation nor the transient (per-invocation) TaskHost that previously worked around its static-state leak
        /// (https://github.com/dotnet/msbuild/issues/13315). Matched by full name, not assembly identity, consistent
        /// with the attribute detection above.
        /// </summary>
        private const string MainProcessTaskFullName = "NuGet.Build.Tasks.RestoreTask";

        /// <summary>
        /// Determines whether the task is the known restore entry point that runs in the main process despite having
        /// no multi-threadable attribute (see <see cref="MainProcessTaskFullName" />).
        /// </summary>
        /// <param name="taskType">The type of the task to evaluate.</param>
        /// <returns>True if the task runs in the main process; false otherwise.</returns>
        private static bool RunsInMainProcessDespiteNoAttribute(Type taskType)
        {
            return string.Equals(taskType.FullName, MainProcessTaskFullName, StringComparison.Ordinal);
        }

        /// <summary>
        /// Clears the thread-safety cache. Used primarily for testing.
        /// </summary>
        internal static void ClearCache()
        {
            s_multiThreadableTaskCache.Clear();
        }
    }
}
