// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using EnvDTE;
using NuGet.Common;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsPackageRestorer))]
    public class VsPackageRestorer : IVsPackageRestorer
    {
        private readonly Configuration.ISettings _settings;
        private readonly ISolutionManager _solutionManager;
        private readonly IPackageRestoreManager _restoreManager;
        private readonly IVsProjectThreadingService _threadingService;
        private readonly INuGetTelemetryProvider _telemetryProvider;

        [ImportingConstructor]
        public VsPackageRestorer(
            Configuration.ISettings settings,
            ISolutionManager solutionManager,
            IPackageRestoreManager restoreManager,
            IVsProjectThreadingService threadingService,
            INuGetTelemetryProvider telemetryProvider)
        {
            _settings = settings;
            _solutionManager = solutionManager;
            _restoreManager = restoreManager;
            _threadingService = threadingService;
            _telemetryProvider = telemetryProvider;
        }

        public bool IsUserConsentGranted()
        {
            const string eventName = nameof(IVsPackageRestorer) + "." + nameof(IsUserConsentGranted);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            try
            {
                var packageRestoreConsent = new PackageManagement.PackageRestoreConsent(_settings);
                return packageRestoreConsent.IsGranted;
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(IVsPackageRestorer).FullName);
                throw;
            }
        }

        public void RestorePackages(Project project)
        {
            const string eventName = nameof(IVsPackageRestorer) + "." + nameof(RestorePackages);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            try
            {
                var solutionDirectory = _solutionManager.SolutionDirectory;
                var nuGetProjectContext = new EmptyNuGetProjectContext();

                // We simply use ThreadHelper.JoinableTaskFactory.Run instead of PumpingJTF.Run, unlike,
                // VsPackageInstaller and VsPackageUninstaller. Because, no powershell scripts get executed
                // as part of the operations performed below. Powershell scripts need to be executed on the
                // pipeline execution thread and they might try to access DTE. Doing that under
                // ThreadHelper.JoinableTaskFactory.Run will consistently result in the UI stop responding
                _threadingService.JoinableTaskFactory.Run(() =>
                    _restoreManager.RestoreMissingPackagesInSolutionAsync(solutionDirectory,
                    nuGetProjectContext,
                    NullLogger.Instance,
                    CancellationToken.None));
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPackageRestorer).FullName);
            }
        }
    }
}
