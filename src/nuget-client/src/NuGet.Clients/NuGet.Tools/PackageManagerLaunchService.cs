// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

#nullable enable

namespace NuGetVSExtension
{
    [Export(typeof(IPackageManagerLaunchService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class PackageManagerLaunchService : IPackageManagerLaunchService
    {
        public void LaunchSolutionPackageManager()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                IVsUIShell vsUIShell = await AsyncServiceProvider.GlobalProvider.GetServiceAsync<IVsUIShell, IVsUIShell>();

                object targetGuid = Guid.Empty;
                var guidNuGetDialog = GuidList.guidNuGetDialogCmdSet;
                vsUIShell.PostExecCommand(
                    ref guidNuGetDialog,
                    PkgCmdIDList.cmdidAddPackageDialogForSolution,
                    0,
                    ref targetGuid);
            }).PostOnFailure(nameof(PackageManagerLaunchService));
        }
    }
}
