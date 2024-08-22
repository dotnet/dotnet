// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace API.Test
{
    public static class InternalAPITestHook
    {
        public static void InstallLatestPackageApi(string id, bool prerelease)
        {
            ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = ServiceLocator.GetDTE();
                    var services = ServiceLocator.GetComponent<IVsPackageInstaller2>();

                    foreach (EnvDTE.Project project in dte.Solution.Projects)
                    {
                        services.InstallLatestPackage(null, project, id, prerelease, false);
                        return;
                    }
                });
        }

        public static void InstallPackageApi(string id, string version)
        {
            InstallPackageApi(source: null, id: id, version: version, prerelease: false);
        }

        public static void InstallPackageApi(string source, string id, string version, bool prerelease)
        {
            ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = ServiceLocator.GetDTE();
                    var services = ServiceLocator.GetComponent<IVsPackageInstaller>();

                    foreach (EnvDTE.Project project in dte.Solution.Projects)
                    {
                        services.InstallPackage(source, project, id, version, prerelease);
                        return;
                    }
                });
        }

        public static void UninstallPackageApi(string id, bool dependency)
        {
            ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = ServiceLocator.GetDTE();
                    var uninstaller = ServiceLocator.GetComponent<IVsPackageUninstaller>();

                    foreach (EnvDTE.Project project in dte.Solution.Projects)
                    {
                        uninstaller.UninstallPackage(project, id, dependency);
                        return;
                    }
                });
        }

        public static void RestorePackageApi()
        {
            ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = ServiceLocator.GetDTE();
                    var restorer = ServiceLocator.GetComponent<IVsPackageRestorer>();

                    foreach (EnvDTE.Project project in dte.Solution.Projects)
                    {
                        restorer.RestorePackages(project);
                        return;
                    }
                });
        }

        public static IVsPathContext GetVsPathContext(string projectUniqueName)
        {
            var provider = ServiceLocator.GetComponent<IVsPathContextProvider>();

            IVsPathContext context;
            if (provider.TryCreateContext(projectUniqueName, out context))
            {
                return context;
            }

            return null;
        }

        [SuppressMessage("Microsoft.VisualStudio.Threading.Analyzers", "VSTHRD002", Justification = "Task.Result for a completed task is safe here.")]
        public static bool ExecuteInitScript(string id, string version, int timeoutSec = 30)
        {
            var scriptExecutor = ServiceLocator.GetComponent<IVsGlobalPackagesInitScriptExecutor>();
            // It is important that this method does not wait on ExecuteInitScriptAsync on the calling thread.
            // Calling thread is powershell execution thread and ExecuteInitScriptAsync needs to switch to
            // Powershell execution thread to execute the scripts
            var task = Task.Run(() => scriptExecutor.ExecuteInitScriptAsync(id, version));
            Task.WaitAny(task, Task.Delay(TimeSpan.FromSeconds(timeoutSec)));
            if (task.IsCompleted)
            {
                return task.Result;
            }

            return false;
        }

        public static bool BatchEventsApi(string id, string version)
        {
            return ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = ServiceLocator.GetDTE();
                    var packageProjectEventService = ServiceLocator.GetComponent<IVsPackageInstallerProjectEvents>();
                    var installerServices = ServiceLocator.GetComponent<IVsPackageInstaller>();
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    packageProjectEventService.BatchStart += (args) =>
                    {
                        batchStartIds.Add(args.BatchId);
                    };

                    packageProjectEventService.BatchEnd += (args) =>
                    {
                        batchEndIds.Add(args.BatchId);
                    };

                    foreach (EnvDTE.Project project in dte.Solution.Projects)
                    {
                        installerServices.InstallPackage(null, project, id, version, false);
                    }

                    return batchStartIds.Count == 1 &&
                           batchEndIds.Count == 1 &&
                           batchStartIds[0].Equals(batchEndIds[0], StringComparison.Ordinal);
                });
        }

        public static IVsProjectJsonToPackageReferenceMigrateResult MigrateJsonProject(string projectName)
        {
            return ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                var migrator = ServiceLocator.GetComponent<IVsProjectJsonToPackageReferenceMigrator>();
                return (IVsProjectJsonToPackageReferenceMigrateResult)await migrator.MigrateProjectJsonToPackageReferenceAsync(projectName);
            });
        }

        public static bool IsFileExistsInProject(string projectUniqueName, string filePath)
        {
            return ThreadHelper.JoinableTaskFactory.Run(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = ServiceLocator.GetDTE();

                    foreach (EnvDTE.Project project in dte.Solution.Projects)
                    {
                        string solutionProjectPath = project.GetFullProjectPath();

                        if (!string.IsNullOrEmpty(solutionProjectPath) &&
                            PathUtility.GetStringComparerBasedOnOS().Equals(solutionProjectPath, projectUniqueName))
                        {
                            return await EnvDTEProjectUtility.ContainsFileAsync(project, filePath);
                        }
                    }

                    return false;
                });
        }
    }
}
