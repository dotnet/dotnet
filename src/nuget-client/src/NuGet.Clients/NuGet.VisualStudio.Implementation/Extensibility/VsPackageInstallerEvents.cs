// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsPackageInstallerEvents))]
    [Export(typeof(VsPackageInstallerEvents))]
    public class VsPackageInstallerEvents : IVsPackageInstallerEvents
    {
        private event VsPackageEventHandler _packageInstalled;
        const string VsPackageInstalledEventName = nameof(IVsPackageInstallerEvents) + "." + nameof(PackageInstalled);
        public event VsPackageEventHandler PackageInstalled
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(VsPackageInstalledEventName, NuGetETW.AddEventOptions);
                _packageInstalled += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(VsPackageInstalledEventName, NuGetETW.RemoveEventOptions);
                _packageInstalled -= value;
            }
        }

        public event VsPackageEventHandler _packageUninstalling;
        const string PackageUninstallingEventName = nameof(IVsPackageInstallerEvents) + "." + nameof(PackageUninstalling);
        public event VsPackageEventHandler PackageUninstalling
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageUninstallingEventName, NuGetETW.AddEventOptions);
                _packageUninstalling += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageUninstallingEventName, NuGetETW.RemoveEventOptions);
                _packageUninstalling -= value;
            }
        }

        public event VsPackageEventHandler _packageInstalling;
        const string PackageInstallingEventName = nameof(IVsPackageInstallerEvents) + "." + nameof(PackageInstalling);
        public event VsPackageEventHandler PackageInstalling
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageInstallingEventName, NuGetETW.AddEventOptions);
                _packageInstalling += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageInstallingEventName, NuGetETW.RemoveEventOptions);
                _packageInstalling -= value;
            }
        }

        public event VsPackageEventHandler _packageUninstalled;
        const string PackageUninstalledEvnentName = nameof(IVsPackageInstallerEvents) + "." + nameof(PackageUninstalled);
        public event VsPackageEventHandler PackageUninstalled
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageUninstalledEvnentName, NuGetETW.AddEventOptions);
                _packageUninstalled += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageUninstalledEvnentName, NuGetETW.RemoveEventOptions);
                _packageUninstalled -= value;
            }
        }

        public event VsPackageEventHandler _packageReferenceAdded;
        const string PackageReferenceAddedEventName = nameof(IVsPackageInstallerEvents) + "." + nameof(PackageReferenceAdded);
        public event VsPackageEventHandler PackageReferenceAdded
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageReferenceAddedEventName, NuGetETW.AddEventOptions);
                _packageReferenceAdded += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageReferenceAddedEventName, NuGetETW.RemoveEventOptions);
                _packageReferenceAdded -= value;
            }
        }

        public event VsPackageEventHandler _packageReferenceRemoved;
        const string PackageReferenceRemovedEventName = nameof(IVsPackageInstallerEvents) + "." + nameof(PackageReferenceRemoved);
        public event VsPackageEventHandler PackageReferenceRemoved
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageReferenceRemovedEventName, NuGetETW.AddEventOptions);
                _packageReferenceRemoved += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(PackageReferenceRemovedEventName, NuGetETW.RemoveEventOptions);
                _packageReferenceRemoved -= value;
            }
        }

        private readonly PackageEvents _eventSource;

        [ImportingConstructor]
        public VsPackageInstallerEvents(IPackageEventsProvider eventProvider, INuGetTelemetryProvider telemetryProvider)
        {
            _eventSource = eventProvider.GetPackageEvents();

            _eventSource.PackageInstalled += Source_PackageInstalled;
            _eventSource.PackageInstalling += Source_PackageInstalling;
            _eventSource.PackageReferenceAdded += Source_PackageReferenceAdded;
            _eventSource.PackageReferenceRemoved += Source_PackageReferenceRemoved;
            _eventSource.PackageUninstalled += Source_PackageUninstalled;
            _eventSource.PackageUninstalling += Source_PackageUninstalling;

            // MEF components do not participate in Visual Studio's Package extensibility,
            // hence importing INuGetTelemetryProvider ensures that the ETW collector is
            // set up correctly.
            _ = telemetryProvider;
        }

        // TODO: If the extra metadata fields are needed use: PackageManagementHelpers.CreateMetadata()

        private void Source_PackageUninstalling(object sender, PackageEventArgs e)
        {
            NotifyUninstalling(new PackageOperationEventArgs(e.InstallPath, e.Identity, null));
        }

        private void Source_PackageUninstalled(object sender, PackageEventArgs e)
        {
            NotifyUninstalled(new PackageOperationEventArgs(e.InstallPath, e.Identity, null));
        }

        private void Source_PackageReferenceRemoved(object sender, PackageEventArgs e)
        {
            NotifyReferenceRemoved(new PackageOperationEventArgs(e.InstallPath, e.Identity, null));
        }

        private void Source_PackageReferenceAdded(object sender, PackageEventArgs e)
        {
            NotifyReferenceAdded(new PackageOperationEventArgs(e.InstallPath, e.Identity, null));
        }

        private void Source_PackageInstalling(object sender, PackageEventArgs e)
        {
            NotifyInstalling(new PackageOperationEventArgs(e.InstallPath, e.Identity, null));
        }

        private void Source_PackageInstalled(object sender, PackageEventArgs e)
        {
            NotifyInstalled(new PackageOperationEventArgs(e.InstallPath, e.Identity, null));
        }

        private void NotifyDelegates(PackageOperationEventArgs e, Delegate[] delegates)
        {
            VsPackageMetadata eventData = new(e.Package, e.InstallPath);
            for (int i = 0; i < delegates.Length; i++)
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    var handler = (VsPackageEventHandler)(delegates[i]);
                    handler(eventData);
                }
                catch
                {
                    // someone else's code threw an exception, but we should keep notifying other event subscribers
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        internal void NotifyInstalling(PackageOperationEventArgs e)
        {
            if (_packageInstalling != null)
            {
                NotifyDelegates(e, _packageInstalling.GetInvocationList());
            }
        }

        internal void NotifyInstalled(PackageOperationEventArgs e)
        {
            if (_packageInstalled != null)
            {
                NotifyDelegates(e, _packageInstalled.GetInvocationList());
            }
        }

        internal void NotifyUninstalling(PackageOperationEventArgs e)
        {
            if (_packageUninstalling != null)
            {
                NotifyDelegates(e, _packageUninstalling.GetInvocationList());
            }
        }

        internal void NotifyUninstalled(PackageOperationEventArgs e)
        {
            if (_packageUninstalled != null)
            {
                NotifyDelegates(e, _packageUninstalled.GetInvocationList());
            }
        }

        internal void NotifyReferenceAdded(PackageOperationEventArgs e)
        {
            if (_packageReferenceAdded != null)
            {
                NotifyDelegates(e, _packageReferenceAdded.GetInvocationList());
            }
        }

        internal void NotifyReferenceRemoved(PackageOperationEventArgs e)
        {
            if (_packageReferenceRemoved != null)
            {
                NotifyDelegates(e, _packageReferenceRemoved.GetInvocationList());
            }
        }
    }
}
