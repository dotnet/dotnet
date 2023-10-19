// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Commands;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// .NET Core compatible restore task for PackageReference and UWP project.json projects.
    /// </summary>
    public class RestoreTask : Microsoft.Build.Utilities.Task, ICancelableTask, IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _disposed = false;

        /// <summary>
        /// DG file entries
        /// </summary>
        [Required]
        public ITaskItem[] RestoreGraphItems { get; set; }

        /// <summary>
        /// Disable parallel project restores and downloads
        /// </summary>
        public bool RestoreDisableParallel { get; set; }

        /// <summary>
        /// Disable the web cache
        /// </summary>
        public bool RestoreNoCache { get; set; }

        /// <summary>
        /// Disable the web cache
        /// </summary>
        public bool RestoreNoHttpCache { get; set; }

        /// <summary>
        /// Ignore errors from package sources
        /// </summary>
        public bool RestoreIgnoreFailedSources { get; set; }

        /// <summary>
        /// Restore all projects.
        /// </summary>
        public bool RestoreRecursive { get; set; }

        /// <summary>
        /// Force restore, skip no op
        /// </summary>
        public bool RestoreForce { get; set; }

        /// <summary>
        /// Do not display Errors and Warnings to the user. 
        /// The Warnings and Errors are written into the assets file and will be read by an sdk target.
        /// </summary>
        public bool HideWarningsAndErrors { get; set; }

        /// <summary>
        /// Set this property if you want to get an interactive restore
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Reevaluate resotre graph even with a lock file, skip no op as well.
        /// </summary>
        public bool RestoreForceEvaluate { get; set; }

        /// <summary>
        /// Restore projects using packages.config for dependencies.
        /// </summary>
        /// <returns></returns>
        public bool RestorePackagesConfig { get; set; }

        /// <summary>
        /// Gets or sets the paths for files to embed in the binary log.
        /// </summary>
        [Output]
        public ITaskItem[] EmbedInBinlog { get; set; }

        public override bool Execute()
        {
#if DEBUG
            var debugRestoreTask = Environment.GetEnvironmentVariable("DEBUG_RESTORE_TASK");
            if (!string.IsNullOrEmpty(debugRestoreTask) && debugRestoreTask.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                Debugger.Launch();
            }
#endif
            var log = new MSBuildLogger(Log);

            NuGet.Common.Migrations.MigrationRunner.Run();

            // Log inputs
            log.LogDebug($"(in) RestoreGraphItems Count '{RestoreGraphItems?.Count() ?? 0}'");
            log.LogDebug($"(in) RestoreDisableParallel '{RestoreDisableParallel}'");
            log.LogDebug($"(in) RestoreNoCache '{RestoreNoCache || RestoreNoHttpCache}'");
            log.LogDebug($"(in) RestoreIgnoreFailedSources '{RestoreIgnoreFailedSources}'");
            log.LogDebug($"(in) RestoreRecursive '{RestoreRecursive}'");
            log.LogDebug($"(in) RestoreForce '{RestoreForce}'");
            log.LogDebug($"(in) HideWarningsAndErrors '{HideWarningsAndErrors}'");
            log.LogDebug($"(in) RestoreForceEvaluate '{RestoreForceEvaluate}'");
            log.LogDebug($"(in) RestorePackagesConfig '{RestorePackagesConfig}'");

            try
            {
                return ExecuteAsync(log).Result;
            }
            catch (AggregateException ex) when (_cts.Token.IsCancellationRequested && ex.InnerException is OperationCanceledException)
            {
                // Canceled by user
                log.LogError(Strings.RestoreCanceled);
                return false;
            }
            catch (Exception e)
            {
                ExceptionUtilities.LogException(e, log);
                return false;
            }
        }

        private async Task<bool> ExecuteAsync(Common.ILogger log)
        {
            if (RestoreGraphItems.Length < 1 && !HideWarningsAndErrors)
            {
                log.LogWarning(Strings.NoProjectsProvidedToTask);
                return true;
            }

            // Convert to the internal wrapper
            var wrappedItems = RestoreGraphItems.Select(MSBuildUtility.WrapMSBuildItem);

            var dgFile = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);

            EmbedInBinlog = GetFilesToEmbedInBinlog(dgFile);

            if (RestoreNoCache)
            {
                //Inform users that NoCache option is just for disabling HttpCache and
                //suggest them to use NoHttpCache instead, which does the same thing.
                log.LogInformation(Strings.Log_RestoreNoCacheInformation);
            }

            return await BuildTasksUtility.RestoreAsync(
                dependencyGraphSpec: dgFile,
                interactive: Interactive,
                recursive: RestoreRecursive,
                noCache: RestoreNoCache || RestoreNoHttpCache,
                ignoreFailedSources: RestoreIgnoreFailedSources,
                disableParallel: RestoreDisableParallel,
                force: RestoreForce,
                forceEvaluate: RestoreForceEvaluate,
                hideWarningsAndErrors: HideWarningsAndErrors,
                restorePC: RestorePackagesConfig,
                log: log,
                cancellationToken: _cts.Token);
        }

        public void Cancel()
        {
            _cts.Cancel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _cts.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        /// Gets the list of files to embed in the MSBuild binary log.
        /// </summary>
        /// <param name="dependencyGraphSpec"></param>
        /// <returns>If the MSBuildBinaryLoggerEnabled environment variable is set, returns the paths to NuGet files to embed in the binlog, otherwise returns <see cref="Array.Empty{T}" />.</returns>
        private ITaskItem[] GetFilesToEmbedInBinlog(DependencyGraphSpec dependencyGraphSpec)
        {
            if (!string.Equals(Environment.GetEnvironmentVariable("MSBUILDBINARYLOGGERENABLED"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                return Array.Empty<ITaskItem>();
            }

            IReadOnlyList<PackageSpec> projects = dependencyGraphSpec.Projects;

            List<ITaskItem> restoredProjectOutputPaths = new List<ITaskItem>(projects.Count);

            foreach (PackageSpec project in projects)
            {
                if (project.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference)
                {
                    restoredProjectOutputPaths.Add(new TaskItem(Path.Combine(project.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName)));
                    restoredProjectOutputPaths.Add(new TaskItem(Path.Combine(project.RestoreMetadata.OutputPath, DependencyGraphSpec.GetDGSpecFileName(Path.GetFileName(project.RestoreMetadata.ProjectPath)))));
                    restoredProjectOutputPaths.Add(new TaskItem(BuildAssetsUtils.GetMSBuildFilePathForPackageReferenceStyleProject(project, BuildAssetsUtils.PropsExtension)));
                    restoredProjectOutputPaths.Add(new TaskItem(BuildAssetsUtils.GetMSBuildFilePathForPackageReferenceStyleProject(project, BuildAssetsUtils.TargetsExtension)));
                }
                else if (project.RestoreMetadata.ProjectStyle == ProjectStyle.PackagesConfig)
                {
                    string packagesConfigPath = BuildTasksUtility.GetPackagesConfigFilePath(project.RestoreMetadata.ProjectPath);

                    if (packagesConfigPath != null)
                    {
                        restoredProjectOutputPaths.Add(new TaskItem(packagesConfigPath));
                    }
                }
            }

            return restoredProjectOutputPaths.ToArray();
        }
    }
}
