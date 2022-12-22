// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using IBrokeredServiceContainer = Microsoft.VisualStudio.Shell.ServiceBroker.IBrokeredServiceContainer;
// Duplicate type declarations due to Microsoft.Internal.VisualStudio.Shell.Embeddable.
using ProvideBrokeredServiceAttribute = Microsoft.VisualStudio.Shell.ServiceBroker.ProvideBrokeredServiceAttribute;
using ServiceAudience = Microsoft.VisualStudio.Shell.ServiceBroker.ServiceAudience;
using SVsBrokeredServiceContainer = Microsoft.VisualStudio.Shell.ServiceBroker.SVsBrokeredServiceContainer;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Visual Studio extension package designed to bootstrap solution restore components.
    /// Loads on solution open to attach to build events.
    /// </summary>
    // Flag AllowsBackgroundLoading is set to True and Flag PackageAutoLoadFlags is set to BackgroundLoad
    // which will allow this package to be loaded asynchronously
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    // BackgroundLoad this package as soon as a Solution exists.
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    // Ensure that this package is loaded in time to listen to solution build events, in order to always be able to restore before build.
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionBuilding_string)]
    [ProvideBrokeredService(BrokeredServicesUtility.DeprecatedSolutionServiceName, BrokeredServicesUtility.DeprecatedSolutionServiceVersion, Audience = ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtility.SolutionServiceName, BrokeredServicesUtility.SolutionServiceVersion, Audience = ServiceAudience.RemoteExclusiveClient)]
    [Guid(PackageGuidString)]
    public sealed class RestoreManagerPackage : AsyncPackage
    {
        /// <summary>
        /// RestoreManagerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "2b52ac92-4551-426d-bd34-c6d7d9fdd1c5";

        private IDisposable _handler;

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            NuGetVSTelemetryService.Initialize();

            _handler = await SolutionRestoreBuildHandler.InitializeAsync(this);

            await SolutionRestoreCommand.InitializeAsync(this);

            // Set up brokered services - Do not reference NuGet.VisualStudio.Internals.Contract explicitly to avoid an unnecessary assembly load
            IBrokeredServiceContainer brokeredServiceContainer = await this.GetServiceAsync<SVsBrokeredServiceContainer, IBrokeredServiceContainer>();
            brokeredServiceContainer.Proffer(BrokeredServicesUtility.DeprecatedSolutionService, factory: BrokeredServicesUtility.GetNuGetSolutionServicesFactory());
            brokeredServiceContainer.Proffer(BrokeredServicesUtility.SolutionService, factory: BrokeredServicesUtility.GetNuGetSolutionServicesFactory());

            await base.InitializeAsync(cancellationToken, progress);
        }

        protected override void Dispose(bool disposing)
        {
            // disposing is true when called from IDispose.Dispose; false when called from Finalizer.
            if (disposing)
            {
                // Guarantees thread-safe execution of this method.
                ThreadHelper.ThrowIfNotOnUIThread();

                _handler?.Dispose();
                _handler = null;
            }

            base.Dispose(disposing);
        }
    }
}
