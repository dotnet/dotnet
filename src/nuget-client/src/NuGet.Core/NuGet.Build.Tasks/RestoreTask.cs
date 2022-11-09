// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using NuGet.Commands;
using NuGet.Common;

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
            log.LogDebug($"(in) RestoreNoCache '{RestoreNoCache}'");
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
            catch (AggregateException ex) when (_cts.Token.IsCancellationRequested && ex.InnerException is TaskCanceledException)
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

            return await BuildTasksUtility.RestoreAsync(
                dependencyGraphSpec: dgFile,
                interactive: Interactive,
                recursive: RestoreRecursive,
                noCache: RestoreNoCache,
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
    }
}
