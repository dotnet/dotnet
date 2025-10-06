// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.TestContract
{
    [Export(typeof(NuGetApexUITestService))]
    public class NuGetApexUITestService
    {
        public NuGetApexUITestService()
        {
        }

        public ApexTestUIProject GetApexTestUIProject(string project, TimeSpan timeout, TimeSpan interval)
        {
            PackageManagerControl packageManagerControl = null;

            var timer = Stopwatch.StartNew();

            while (packageManagerControl == null && timer.Elapsed < timeout)
            {
                packageManagerControl = GetProjectPackageManagerControl(project);

                if (packageManagerControl == null)
                {
                    System.Threading.Thread.Sleep(interval);
                }
            }

            if (packageManagerControl == null)
            {
                throw new TimeoutException($"The package manager control did not load within {timeout}");
            }

            return new ApexTestUIProject(packageManagerControl);
        }

        private PackageManagerControl GetProjectPackageManagerControl(string projectUniqueName)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var uiShell = await ServiceLocator.GetGlobalServiceAsync<SVsUIShell, IVsUIShell>();
                foreach (var windowFrame in VsUtility.GetDocumentWindows(uiShell))
                {
                    object docView;
                    var hr = windowFrame.GetProperty(
                        (int)__VSFPROPID.VSFPROPID_DocView,
                        out docView);
                    if (hr == VSConstants.S_OK
                        && docView is PackageManagerWindowPane)
                    {
                        var packageManagerWindowPane = (PackageManagerWindowPane)docView;
                        if (packageManagerWindowPane.Model.IsSolution)
                        {
                            // the window is the solution package manager
                            continue;
                        }

                        var projects = packageManagerWindowPane.Model.Context.Projects;
                        if (projects.Count() != 1)
                        {
                            continue;
                        }

                        IProjectContextInfo existingProject = projects.First();
                        IServiceBroker serviceBroker = packageManagerWindowPane.Model.Context.ServiceBroker;
                        IProjectMetadataContextInfo projectMetadata = await existingProject.GetMetadataAsync(
                            serviceBroker,
                            CancellationToken.None);

                        if (string.Equals(projectMetadata.Name, projectUniqueName, StringComparison.OrdinalIgnoreCase))
                        {
                            var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
                            if (packageManagerControl != null)
                            {
                                return packageManagerControl;
                            }
                        }
                    }
                }

                return null;
            });
        }
    }
}
