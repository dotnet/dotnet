// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using LocalResources = NuGet.PackageManagement.PowerShellCmdlets.Resources;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    public static class ProjectExtensions
    {
        /// <summary>
        /// This method is used for the ProjectName CodeProperty in Types.ps1xml
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ps")]
        public static string GetCustomUniqueName(PSObject psObject)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    return await ((EnvDTE.Project)psObject.BaseObject).GetCustomUniqueNameAsync();
                });
        }

        /// <summary>
        /// DO NOT delete this. This method is only called from PowerShell functional test.
        /// </summary>
        public static void RemoveProject(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException(LocalResources.Argument_Cannot_Be_Null_Or_Empty, nameof(projectName));
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var solutionManager = await ServiceLocator.GetComponentModelServiceAsync<IVsSolutionManager>();
                if (solutionManager != null)
                {
                    var project = await solutionManager.GetVsProjectAdapterAsync(projectName);
                    if (project == null)
                    {
                        throw new InvalidOperationException();
                    }

                    var dte = await ServiceLocator.GetGlobalServiceAsync<SDTE, DTE>();
                    dte.Solution.Remove(project.Project);
                }
            });
        }
    }
}
