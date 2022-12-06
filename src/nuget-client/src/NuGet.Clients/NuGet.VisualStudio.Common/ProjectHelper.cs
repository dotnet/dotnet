// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MsBuildProject = Microsoft.Build.Evaluation.Project;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio
{
    public static class ProjectHelper
    {
        public static async Task DoWorkInWriterLockAsync(Project project, IVsHierarchy hierarchy, Action<MsBuildProject> action)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsProject = (IVsProject)hierarchy;
            UnconfiguredProject unconfiguredProject = GetUnconfiguredProject(vsProject);
            if (unconfiguredProject != null)
            {
                var service = unconfiguredProject.ProjectService.Services.ProjectLockService;
                if (service != null)
                {
                    await service.WriteLockAsync(
                        async (x) =>
                        {
                            await x.CheckoutAsync(unconfiguredProject.FullPath);
                            ConfiguredProject configuredProject = await unconfiguredProject.GetSuggestedConfiguredProjectAsync();
                            MsBuildProject buildProject = await x.GetProjectAsync(configuredProject);

                            if (buildProject != null)
                            {
                                action(buildProject);
                            }

                            await x.ReleaseAsync();
                        });

                    await unconfiguredProject.ProjectService.Services.ThreadingPolicy.SwitchToUIThread();
                    project.Save();
                }
            }
        }

        private static UnconfiguredProject GetUnconfiguredProject(IVsProject project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsBrowseObjectContext context = project as IVsBrowseObjectContext;
            if (context == null)
            {
                IVsHierarchy hierarchy = project as IVsHierarchy;
                if (hierarchy != null)
                {
                    object extObject;
                    if (ErrorHandler.Succeeded(hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ExtObject, out extObject)))
                    {
                        Project dteProject = extObject as Project;
                        if (dteProject != null)
                        {
                            context = dteProject.Object as IVsBrowseObjectContext;
                        }
                    }
                }
            }

            return context != null ? context.UnconfiguredProject : null;
        }
    }
}
