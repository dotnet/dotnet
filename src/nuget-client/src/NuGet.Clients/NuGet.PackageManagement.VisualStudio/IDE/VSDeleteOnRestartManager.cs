// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// An <see cref="DeleteOnRestartManager"/> manger which is used for surfacing errors and UI in VS.
    /// </summary>
    [Export(typeof(IDeleteOnRestartManager))]
    internal sealed class VsDeleteOnRestartManager : IDeleteOnRestartManager
    {
        // The file extension to add to the empty files which will be placed adjacent to partially uninstalled package
        // directories marking them for removal the next time the solution is opened.
        private const string DeletionMarkerSuffix = ".deleteme";
        private const string DeletionMarkerFilter = "*" + DeletionMarkerSuffix;

        private string _packagesFolderPath;

        public event EventHandler<PackagesMarkedForDeletionEventArgs> PackagesMarkedForDeletionFound;

        /// <summary>
        /// Creates a new instance of <see cref="DeleteOnRestartManager"/>.
        /// </summary>
        /// <param name="settings">The <see cref="ISettings"/> associated with the current solution.</param>
        /// <param name="solutionManager">The <see cref="ISolutionManager"/> associated with the current solution.
        /// </param>
        [ImportingConstructor]
        public VsDeleteOnRestartManager(
            Configuration.ISettings settings,
            ISolutionManager solutionManager)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            SolutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
            SolutionManager.SolutionOpened += OnSolutionOpenedOrClosed;
            SolutionManager.SolutionClosed += OnSolutionOpenedOrClosed;
        }

        private ISolutionManager SolutionManager { get; }

        private Configuration.ISettings Settings { get; }

        public string PackagesFolderPath
        {
            get
            {
                var solutionDirectory = SolutionManager.SolutionDirectory;
                if (solutionDirectory != null)
                {
                    _packagesFolderPath =
                        PackagesFolderPathUtility.GetPackagesFolderPath(solutionDirectory, Settings);
                }

                return _packagesFolderPath;
            }

            set
            {
                _packagesFolderPath = value;
            }
        }

        /// <summary>
        /// Gets the directories marked for deletion. Returns empty is <see cref="PackagesFolderPath"/> is <see langword="null" /> >
        /// </summary>
        public IReadOnlyList<string> GetPackageDirectoriesMarkedForDeletion()
        {
            // PackagesFolderPath reads the configs, reference the local variable to avoid reading the configs continously
            var packagesFolderPath = PackagesFolderPath;
            if (packagesFolderPath == null)
            {
                return Array.Empty<string>();
            }

            var candidates = FileSystemUtility
                .GetFiles(packagesFolderPath, path: "", filter: DeletionMarkerFilter, recursive: false)
                // strip the DeletionMarkerFilter at the end of the path to get the package path.
                .Select(path => Path.Combine(packagesFolderPath, Path.ChangeExtension(path, null)))
                .ToList();

            var filesWithoutFolders = candidates.Where(path => !Directory.Exists(path));
            foreach (var directory in filesWithoutFolders)
            {
                File.Delete(directory + DeletionMarkerSuffix);
            }

            return candidates.Where(path => Directory.Exists(path)).ToList();
        }

        /// <summary>
        /// Checks for any package directories that are pending to be deleted and raises the
        /// <see cref="PackagesMarkedForDeletionFound"/> event.
        /// </summary>
        public void CheckAndRaisePackageDirectoriesMarkedForDeletion()
        {
            var packages = GetPackageDirectoriesMarkedForDeletion();
            if (packages.Any() && PackagesMarkedForDeletionFound != null)
            {
                var eventArgs = new PackagesMarkedForDeletionEventArgs(packages);
                PackagesMarkedForDeletionFound(this, eventArgs);
            }
        }

        /// <summary>
        /// Marks package directory for future removal if it was not fully deleted during the normal uninstall process
        /// if the directory does not contain any added or modified files.
        /// The package directory will be marked by an adjacent *directory name*.deleteme file.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We want to log an exception as a warning and move on")]
        public void MarkPackageDirectoryForDeletion(
            PackageIdentity package,
            string packageDirectory,
            INuGetProjectContext projectContext)
        {
            if (PackagesFolderPath == null)
            {
                return;
            }

            try
            {
                // Use the overload that doesn't take the context, so the .deleteme file doesn't get added
                // to source control. See https://github.com/NuGet/Home/issues/1720
                using (FileSystemUtility.CreateFile(packageDirectory + DeletionMarkerSuffix))
                {
                }
            }
            catch (Exception e)
            {
                projectContext.Log(
                    MessageLevel.Warning,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Warning_FailedToMarkPackageDirectoryForDeletion,
                        packageDirectory,
                        e.Message));
            }
        }

        /// <summary>
        /// Attempts to remove package directories that were unable to be fully deleted during the original uninstall.
        /// These directories will be marked by an adjacent *directory name*.deleteme files in the local package
        /// repository.
        /// If the directory removal is successful, the .deleteme file will also be removed.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We want to log an exception as a warning and move on")]
        public async Task DeleteMarkedPackageDirectoriesAsync(INuGetProjectContext projectContext)
        {
            await TaskScheduler.Default;

            try
            {
                var packages = GetPackageDirectoriesMarkedForDeletion(); // returns empty if PackagesFolderPath is null. No need to check again.
                foreach (var package in packages)
                {
                    try
                    {
                        FileSystemUtility.DeleteDirectorySafe(package, true, projectContext);
                    }
                    finally
                    {
                        if (!Directory.Exists(package))
                        {
                            var deleteMeFilePath = package.TrimEnd('\\') + DeletionMarkerSuffix;
                            FileSystemUtility.DeleteFile(deleteMeFilePath, projectContext);
                        }
                        else
                        {
                            projectContext.Log(
                                MessageLevel.Warning,
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.Warning_FailedToDeleteMarkedPackageDirectory,
                                    package));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                projectContext.Log(
                               MessageLevel.Warning,
                               string.Format(
                                   CultureInfo.CurrentCulture,
                                   Strings.Warning_FailedToDeleteMarkedPackageDirectories,
                                   e.Message));
            }
        }

        private void OnSolutionOpenedOrClosed(object sender, EventArgs e)
        {
            // This is a solution event. Should be on the UI thread
            ThreadHelper.ThrowIfNotOnUIThread();

            // We need to do the check even on Solution Closed because, let's say if the yellow Update bar
            // is showing and the user closes the solution; in that case, we want to hide the Update bar.
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () => await DeleteMarkedPackageDirectoriesAsync(SolutionManager.NuGetProjectContext))
                                                   .PostOnFailure(nameof(VsDeleteOnRestartManager));
        }
    }
}
