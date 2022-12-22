// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IPackageRestoreManager))]
    internal sealed class VSPackageRestoreManager : PackageRestoreManager
    {
        private ISolutionManager SolutionManager { get; }

        [ImportingConstructor]
        public VSPackageRestoreManager(
            ISourceRepositoryProvider sourceRepositoryProvider,
            Configuration.ISettings settings,
            ISolutionManager solutionManager)
            : base(sourceRepositoryProvider, settings, solutionManager)
        {
            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            SolutionManager = solutionManager;
            SolutionManager.NuGetProjectAdded += OnNuGetProjectAdded;
            SolutionManager.SolutionOpened += OnSolutionOpened;
            SolutionManager.SolutionClosed += OnSolutionClosed;
        }

        private void OnSolutionOpened(object sender, EventArgs e)
        {
            // This is a solution event. Should be on the UI thread
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionDirectory = SolutionManager.SolutionDirectory;

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await TaskScheduler.Default;
                await RaisePackagesMissingEventForSolutionAsync(solutionDirectory, CancellationToken.None);
            })
            .PostOnFailure(nameof(VSPackageRestoreManager), nameof(OnSolutionOpened));
        }

        private void OnSolutionClosed(object sender, EventArgs e)
        {
            // This is a solution event. Should be on the UI thread
            ThreadHelper.ThrowIfNotOnUIThread();
            ClearMissingEventForSolution();
        }

        private void OnNuGetProjectAdded(object sender, NuGetProjectEventArgs e)
        {
            // This is a solution event. Should be on the UI thread
            ThreadHelper.ThrowIfNotOnUIThread();
            var solutionDirectory = SolutionManager.SolutionDirectory;

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                // go off the UI thread to raise missing packages event
                await TaskScheduler.Default;

                await RaisePackagesMissingEventForSolutionAsync(solutionDirectory, CancellationToken.None);
            })
            .PostOnFailure(nameof(VSPackageRestoreManager), nameof(OnNuGetProjectAdded));
        }
    }
}
