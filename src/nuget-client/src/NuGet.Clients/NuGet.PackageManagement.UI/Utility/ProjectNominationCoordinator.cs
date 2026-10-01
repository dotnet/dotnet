// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NuGet.PackageManagement.VisualStudio;
using NuGet.SolutionRestoreManager;

namespace NuGet.PackageManagement.UI.Utility
{
    /// <summary>
    /// Waits for project-system nominations to settle before the Package Manager UI refreshes.
    /// </summary>
    internal sealed class ProjectNominationCoordinator
    {
        internal static readonly TimeSpan DefaultNominationSettleTimeout = TimeSpan.FromMinutes(5);

        private readonly IVsSolutionManager _solutionManager;
        private readonly TimeSpan _nominationSettleTimeout;

        internal ProjectNominationCoordinator(IVsSolutionManager solutionManager)
            : this(solutionManager, DefaultNominationSettleTimeout)
        {
        }

        internal ProjectNominationCoordinator(IVsSolutionManager solutionManager, TimeSpan nominationSettleTimeout)
        {
            _solutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
            _nominationSettleTimeout = nominationSettleTimeout;
        }

        internal async Task<TimeSpan?> WaitForNominationsToSettleAsync(string? projectFullPath, CancellationToken cancellationToken)
        {
            Stopwatch? waitStopwatch = null;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                bool allProjectsReady = true;
                bool awaitedIncompleteNomination = false;

                foreach (object source in _solutionManager.GetAllProjectRestoreInfoSources().NoAllocEnumerate())
                {
                    var restoreInfoSource = (IVsProjectRestoreInfoSource)source;

                    if (projectFullPath != null
                        && !string.Equals(restoreInfoSource.Name, projectFullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (restoreInfoSource.HasPendingNomination)
                    {
                        allProjectsReady = false;
                        Task whenNominated = restoreInfoSource.WhenNominated(cancellationToken);

                        if (whenNominated.IsCompleted)
                        {
                            ObserveNominationException(whenNominated);
                            continue;
                        }

                        awaitedIncompleteNomination = true;
                        if (waitStopwatch == null)
                        {
                            waitStopwatch = Stopwatch.StartNew();
                        }

                        TimeSpan remainingBudget = _nominationSettleTimeout - waitStopwatch.Elapsed;
                        if (remainingBudget < TimeSpan.Zero)
                        {
                            remainingBudget = TimeSpan.Zero;
                        }

                        Task timeoutTask = Task.Delay(remainingBudget, cancellationToken);
                        Task finished = await Task.WhenAny(whenNominated, timeoutTask);

                        cancellationToken.ThrowIfCancellationRequested();

                        if (finished == timeoutTask)
                        {
                            return waitStopwatch.Elapsed;
                        }

                        ObserveNominationException(whenNominated);
                    }
                }

                if (allProjectsReady || !awaitedIncompleteNomination)
                {
                    return waitStopwatch?.Elapsed;
                }
            }
        }

        private static void ObserveNominationException(Task whenNominated)
        {
            // The contract allows nomination failures and PMUI intentionally proceeds.
            // Observe the fault so the host does not later report it as unobserved.
            if (whenNominated.IsFaulted)
            {
                _ = whenNominated.Exception;
            }
        }
    }
}
