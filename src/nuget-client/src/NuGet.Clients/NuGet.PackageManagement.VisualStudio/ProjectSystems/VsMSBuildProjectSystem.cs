// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement.Utility;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using PathUtility = NuGet.Common.PathUtility;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    public class VsMSBuildProjectSystem
        : IMSBuildProjectSystem
        , IProjectSystemService
    {
        private const string BinDir = "bin";
        private const string NuGetImportStamp = "NuGetPackageImportStamp";

        private NuGetFramework _targetFramework;

        private IVsProjectBuildSystem _buildSystem;

        public IVsProjectAdapter VsProjectAdapter { get; }

        public INuGetProjectContext NuGetProjectContext { get; set; }

        private IVsProjectBuildSystem ProjectBuildSystem
        {
            get
            {
                if (_buildSystem == null)
                {
                    NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                    {
                        await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        _buildSystem = VsProjectAdapter.VsHierarchy as IVsProjectBuildSystem;
                    });
                }

                return _buildSystem;
            }
        }

        /// <summary>
        /// This does not contain the filename, just the path to the directory where the project file exists
        /// </summary>
        public string ProjectFullPath => VsProjectAdapter.ProjectDirectory;

        /// <summary>
        /// This contains the directory and the file name of the project file.
        /// </summary>
        public string ProjectFileFullPath => VsProjectAdapter.FullProjectPath;

        public virtual string ProjectName => VsProjectAdapter.ProjectName;


        public virtual string ProjectUniqueName => VsProjectAdapter.CustomUniqueName;

        public NuGetFramework TargetFramework
        {
            get
            {
                if (_targetFramework == null)
                {
                    _targetFramework = NuGetUIThreadHelper.JoinableTaskFactory.Run(VsProjectAdapter.GetTargetFrameworkAsync);
                }

                return _targetFramework;
            }
        }

        public VsMSBuildProjectSystem(
            IVsProjectAdapter vsProjectAdapter,
            INuGetProjectContext nuGetProjectContext)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(nuGetProjectContext);

            VsProjectAdapter = vsProjectAdapter;
            NuGetProjectContext = nuGetProjectContext;
        }

        public async Task InitializeProperties()
        {
            _targetFramework = await VsProjectAdapter.GetTargetFrameworkAsync();
        }

        public virtual void AddFile(string path, Stream stream)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                await AddFileCoreAsync(path, () => FileSystemUtility.AddFile(ProjectFullPath, path, stream, NuGetProjectContext));
            });
        }

        public virtual void AddFile(string path, Action<Stream> writeToStream)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                await AddFileCoreAsync(path, () => FileSystemUtility.AddFile(ProjectFullPath, path, writeToStream, NuGetProjectContext));
            });
        }

        private async Task AddFileCoreAsync(string path, Action addFile)
        {
            // Do not try to add file to project, if the path is null or empty.
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var fileExistsInProject = FileExistsInProject(path);

            // If the file exists on disk but not in the project then skip it.
            // One exception is the 'packages.config' file, in which case we want to include
            // it into the project.
            // Other exceptions are 'web.config' and 'app.config'
            var fileName = Path.GetFileName(path);
            var lockFileFullPath = PackagesConfigLockFileUtility.GetPackagesLockFilePath(ProjectFullPath, GetPropertyValue("NuGetLockFilePath")?.ToString(), ProjectName);
            if (File.Exists(Path.Combine(ProjectFullPath, path))
                && !fileExistsInProject
                && !fileName.Equals(ProjectManagement.Constants.PackageReferenceFile, StringComparison.Ordinal)
                && !fileName.Equals("packages." + ProjectName + ".config", StringComparison.Ordinal)
                && !fileName.Equals(EnvDteProjectExtensions.WebConfig, StringComparison.Ordinal)
                && !fileName.Equals(EnvDteProjectExtensions.AppConfig, StringComparison.Ordinal)
                && !fileName.Equals(Path.GetFileName(lockFileFullPath), StringComparison.Ordinal)
                )
            {
                NuGetProjectContext.Log(ProjectManagement.MessageLevel.Warning, Strings.Warning_FileAlreadyExists, path);
            }
            else
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                EnvDTEProjectUtility.EnsureCheckedOutIfExists(VsProjectAdapter.Project, ProjectFullPath, path);
                addFile();
                if (!fileExistsInProject)
                {
                    await AddFileToProjectAsync(path);
                }
            }
        }

        public void AddExistingFile(string path)
        {
            var fullPath = Path.Combine(ProjectFullPath, path);

            if (!File.Exists(fullPath))
            {
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, Strings.PathToExistingFileNotPresent, fullPath, ProjectName));
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                await AddFileCoreAsync(path, () => { });
            });
        }

        protected virtual bool ExcludeFile(string path)
        {
            // Exclude files from the bin directory.
            if (string.IsNullOrEmpty(path))
            {
                return true;
            }

            return Path.GetDirectoryName(path).Equals(BinDir, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// This method should be on the UI thread. The overrides should ensure that
        /// </summary>
        protected virtual async Task AddFileToProjectAsync(string path)
        {
            if (ExcludeFile(path))
            {
                return;
            }

            // Get the project items for the folder path
            var folderPath = Path.GetDirectoryName(path);
            var fullPath = FileSystemUtility.GetFullPath(ProjectFullPath, path);

            // Add the file to project or folder
            await AddProjectItemAsync(fullPath, folderPath, createFolderIfNotExists: true);

            NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_AddedFileToProject, path, ProjectName);
        }

        public virtual void AddImport(string targetFullPath, ImportLocation location)
        {
            Assumes.NotNullOrEmpty(targetFullPath);

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var relativeTargetPath = PathUtility.GetRelativePath(PathUtility.EnsureTrailingSlash(ProjectFullPath), targetFullPath);
                AddImportStatement(relativeTargetPath, location);
                await SaveProjectAsync();

                // notify the project system of the change
                UpdateImportStamp(VsProjectAdapter);
            });
        }

        private void AddImportStatement(string targetsPath, ImportLocation location)
        {
            // Need NOT be on the UI Thread
            MicrosoftBuildEvaluationProjectUtility.AddImportStatement(
                EnvDTEProjectUtility.AsMSBuildEvaluationProject(VsProjectAdapter.FullName), targetsPath, location);
        }

        private void RemoveImportStatement(string targetsPath)
        {
            // Need NOT be on the UI Thread
            MicrosoftBuildEvaluationProjectUtility.RemoveImportStatement(
                EnvDTEProjectUtility.AsMSBuildEvaluationProject(VsProjectAdapter.FullName), targetsPath);
        }

        private static bool IsSamePath(string path1, string path2)
        {
            // Exact match or match after normalizing both paths
            return StringComparer.OrdinalIgnoreCase.Equals(path1, path2)
                || StringComparer.OrdinalIgnoreCase.Equals(Path.GetFullPath(path1), Path.GetFullPath(path2));
        }

        private static bool AssemblyNamesMatch(AssemblyName name1, AssemblyName name2)
        {
            return name1.Name.Equals(name2.Name, StringComparison.OrdinalIgnoreCase) &&
                   EqualsIfNotNull(name1.Version, name2.Version) &&
                   EqualsIfNotNull(name1.CultureInfo, name2.CultureInfo) &&
                   EqualsIfNotNull(name1.GetPublicKeyToken(), name2.GetPublicKeyToken(), Enumerable.SequenceEqual);
        }

        private static bool EqualsIfNotNull<T>(T obj1, T obj2)
        {
            return EqualsIfNotNull(obj1, obj2, (a, b) => a.Equals(b));
        }

        private static bool EqualsIfNotNull<T>(T obj1, T obj2, Func<T, T, bool> equals)
        {
            // If both objects are non null do the equals
            if (obj1 != null
                && obj2 != null)
            {
                return equals(obj1, obj2);
            }

            // Otherwise consider them equal if either of the values are null
            return true;
        }

        public virtual void RemoveFile(string path)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var deleteProjectItem = await EnvDTEProjectUtility.DeleteProjectItemAsync(VsProjectAdapter.Project, path);
                if (deleteProjectItem)
                {
                    var folderPath = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_RemovedFileFromFolder, Path.GetFileName(path), folderPath);
                    }
                    else
                    {
                        NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_RemovedFile, Path.GetFileName(path));
                    }
                }
            });
        }

        public virtual void RemoveImport(string targetFullPath)
        {
            Assumes.NotNullOrEmpty(targetFullPath);

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var relativeTargetPath = PathUtility.GetRelativePath(PathUtility.EnsureTrailingSlash(ProjectFullPath), targetFullPath);
                RemoveImportStatement(relativeTargetPath);

                await SaveProjectAsync();

                // notify the project system of the change
                UpdateImportStamp(VsProjectAdapter);
            });
        }

        private static void TrySetCopyLocal(dynamic reference)
        {
            // Always set copy local to true for references that we add
            try
            {
                // Setting copyLocal to "true" only if it is "false".
                // This should trigger an event which will result in successful writing to msbuild.
                if (!reference.CopyLocal)
                {
                    reference.CopyLocal = true;
                }
            }
            catch (NotSupportedException)
            {
            }
            catch (NotImplementedException)
            {
            }
            catch (RuntimeBinderException)
            {
            }
            catch (COMException)
            {
            }
        }

        private static string GetReferencePath(dynamic reference)
        {
            try
            {
                return reference.Path;
            }
            catch (NotSupportedException)
            {
            }
            catch (NotImplementedException)
            {
            }
            catch (RuntimeBinderException)
            {
            }
            catch (COMException)
            {
            }

            return null;
        }

        // Set SpecificVersion to true
        private static void TrySetSpecificVersion(dynamic reference)
        {
            // Always set SpecificVersion to true for references that we add
            try
            {
                // Setting SpecificVersion to "true" only if it is "false".
                // This should trigger an event which will result in successful writing to msbuild.
                if (!reference.SpecificVersion)
                {
                    reference.SpecificVersion = true;
                }
            }
            catch (NotSupportedException)
            {
            }
            catch (NotImplementedException)
            {
            }
            catch (RuntimeBinderException)
            {
            }
            catch (COMException)
            {
            }
        }

        public virtual bool FileExistsInProject(string path)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var containsFile = await EnvDTEProjectUtility.ContainsFileAsync(VsProjectAdapter.Project, path);
                    return containsFile;
                });
        }

        public virtual dynamic GetPropertyValue(string propertyName)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                return VsProjectAdapter.BuildProperties.GetPropertyValueWithDteFallback(propertyName);

            });
        }

        public virtual bool IsSupportedFile(string path)
        {
            // Need NOT be on the UI thread

            var fileName = Path.GetFileName(path);

            // exclude all file names with the pattern as "web.*.config",
            // e.g. web.config, web.release.config, web.debug.config
            return !(fileName.StartsWith("web.", StringComparison.OrdinalIgnoreCase) &&
                     fileName.EndsWith(".config", StringComparison.OrdinalIgnoreCase));
        }

        public virtual string ResolvePath(string path)
        {
            return path;
        }

        /// <summary>
        /// This method should be on the UI thread. The overrides should ensure that
        /// Sets NuGetPackageImportStamp to a new random guid. This is a hack to let the project system know it is out
        /// of date.
        /// The value does not matter, it just needs to change.
        /// </summary>
        protected static void UpdateImportStamp(IVsProjectAdapter vsProjectAdapter)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var propStore = vsProjectAdapter.VsHierarchy as IVsBuildPropertyStorage;
            if (propStore != null)
            {
                // <NuGetPackageImportStamp>af617720</NuGetPackageImportStamp>
                var stamp = Guid.NewGuid().ToString().Split('-')[0];
                try
                {
                    propStore.SetPropertyValue(NuGetImportStamp, string.Empty, (uint)_PersistStorageType.PST_PROJECT_FILE, stamp);
                }
                catch (Exception ex1)
                {
                    ExceptionHelper.WriteErrorToActivityLog(ex1);
                }

                // Remove the NuGetImportStamp so that VC++ project file won't be updated with this stamp on disk,
                // which causes unnecessary source control pending changes.
                try
                {
                    propStore.RemoveProperty(NuGetImportStamp, string.Empty, (uint)_PersistStorageType.PST_PROJECT_FILE);
                }
                catch (Exception ex2)
                {
                    ExceptionHelper.WriteErrorToActivityLog(ex2);
                }
            }
        }

        #region Binding Redirects Stuff

        private const string SilverlightTargetFrameworkIdentifier = "Silverlight";

        protected virtual bool IsBindingRedirectSupported
        {
            get
            {
                // Silverlight projects and Windows Phone projects do not support binding redirect.
                // They both share the same identifier as "Silverlight"
                return !SilverlightTargetFrameworkIdentifier.Equals(TargetFramework.Framework, StringComparison.OrdinalIgnoreCase);
            }
        }

        public void AddBindingRedirects()
        {
            var settings = ServiceLocator.GetComponentModelService<Configuration.ISettings>();

            var behavior = new BindingRedirectBehavior(settings);

            if (!behavior.IsSkipped)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    try
                    {
                        await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                        await InitForBindingRedirectsAsync();
                        if (IsBindingRedirectSupported && VSSolutionManager != null)
                        {
                            await RuntimeHelpers.AddBindingRedirectsAsync(VSSolutionManager,
                                VsProjectAdapter,
                                VSFrameworkMultiTargeting,
                                NuGetProjectContext);
                        }
                    }
                    catch (Exception ex)
                    {
                        var fileName = VsProjectAdapter.UniqueName;

                        var level = behavior.FailOperations ?
                            ProjectManagement.MessageLevel.Error :
                            ProjectManagement.MessageLevel.Warning;

                        NuGetProjectContext.Log(level,
                            Strings.FailedToUpdateBindingRedirects,
                            fileName,
                            ex.Message);

                        if (behavior.FailOperations)
                        {
                            throw;
                        }
                    }
                });
            }
        }

        private readonly bool _bindingRedirectsRelatedInitialized = false;
        private VSSolutionManager VSSolutionManager { get; set; }
        private IVsFrameworkMultiTargeting VSFrameworkMultiTargeting { get; set; }

        private async Task InitForBindingRedirectsAsync()
        {
            if (!_bindingRedirectsRelatedInitialized)
            {
                var solutionManager = await ServiceLocator.GetComponentModelServiceAsync<ISolutionManager>();
                VSSolutionManager = (solutionManager != null) ? (solutionManager as VSSolutionManager) : null;
                VSFrameworkMultiTargeting = await ServiceLocator.GetGlobalServiceAsync<SVsFrameworkMultiTargeting, IVsFrameworkMultiTargeting>();
            }
        }

        #endregion Binding Redirects Stuff

        public virtual async Task BeginProcessingAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            ProjectBuildSystem?.StartBatchEdit();
        }

        public virtual void RegisterProcessedFiles(IEnumerable<string> files)
        {
            // No-op, this is implemented in other project systems, like website.
        }

        public virtual async Task EndProcessingAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            ProjectBuildSystem?.EndBatchEdit();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            // Only delete this folder if it is empty and we didn't specify that we want to recurse
            if (!recursive
                && (FileSystemUtility.GetFiles(ProjectFullPath, path, "*.*", recursive).Any() || FileSystemUtility.GetDirectories(ProjectFullPath, path).Any()))
            {
                NuGetProjectContext.Log(ProjectManagement.MessageLevel.Warning, Strings.Warning_DirectoryNotEmpty, path);
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                // Workaround for TFS update issue. If we're bound to TFS, do not try and delete directories.
                if (SourceControlUtility.GetSourceControlManager(NuGetProjectContext) == null)
                {
                    var deletedProjectItem = await EnvDTEProjectUtility.DeleteProjectItemAsync(VsProjectAdapter.Project, path);
                    if (deletedProjectItem)
                    {
                        NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_RemovedFolder, path);
                    }
                }
            });
        }

        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            if (recursive)
            {
                throw new NotSupportedException();
            }

            return GetChildItems(path, filter, VsProjectTypes.VsProjectItemKindPhysicalFile);
        }

        /// <summary>
        /// Returns the list of full paths of all files in the project whose name is
        /// <paramref name="fileName"/>. BFS algorithm is used. Thus, the files directly under
        /// the project are returned first. Then files one-level deep are returned, and so-on.
        /// </summary>
        /// <param name="fileName">The file name to search.</param>
        /// <returns>The list of full paths.</returns>
        public IEnumerable<string> GetFullPaths(string fileName)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var paths = new List<string>();
                var projectItemsQueue = new Queue<EnvDTE.ProjectItems>();
                projectItemsQueue.Enqueue(VsProjectAdapter.Project.ProjectItems);
                while (projectItemsQueue.Count > 0)
                {
                    var items = projectItemsQueue.Dequeue();
                    foreach (var item in items.Cast<EnvDTE.ProjectItem>())
                    {
                        if (item.Kind == VsProjectTypes.VsProjectItemKindPhysicalFile)
                        {
                            if (StringComparer.OrdinalIgnoreCase.Equals(item.Name, fileName))
                            {
                                paths.Add(item.get_FileNames(1));
                            }
                        }
                        else if (item.Kind == VsProjectTypes.VsProjectItemKindPhysicalFolder)
                        {
                            projectItemsQueue.Enqueue(item.ProjectItems);
                        }
                    }
                }

                return paths;
            });
        }

        public virtual IEnumerable<string> GetDirectories(string path)
        {
            return GetChildItems(path, "*.*", VsProjectTypes.VsProjectItemKindPhysicalFolder);
        }

        private IEnumerable<string> GetChildItems(string path, string filter, string desiredKind)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var childItems = await EnvDTEProjectUtility.GetChildItems(VsProjectAdapter.Project, path, filter, VsProjectTypes.VsProjectItemKindPhysicalFile);
                // Get all physical files
                return from p in childItems
                       select p.Name;
            });
        }

        #region IProjectReferencesService

        public VSLangProj.References References
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                dynamic projectObj = VsProjectAdapter.Project.Object;
                var references = (VSLangProj.References)projectObj.References;
                projectObj = null;
                return references;
            }
        }

        public VSLangProj157.References3 References3
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                dynamic projectObj = VsProjectAdapter.Project.Object;
                var references = (VSLangProj157.References3)projectObj.References;
                projectObj = null;
                return references;
            }
        }

        public async Task AddFrameworkReferenceAsync(string name, string packageId)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                AddGacReference(name);

                NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_AddGacReference, name, ProjectName);
            }
            catch (Exception e)
            {
                if (IsReferenceUnavailableException(e))
                {
                    var frameworkName = await VsProjectAdapter.GetDotNetFrameworkNameAsync();

                    if (FrameworkAssemblyResolver.IsFrameworkFacade(name, frameworkName))
                    {
                        NuGetProjectContext.Log(ProjectManagement.MessageLevel.Info, Strings.FailedToAddFacadeReference, name, packageId);
                        return;
                    }
                }

                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.FailedToAddGacReference, packageId, name), e);
            }
        }

        public virtual void AddGacReference(string name)
        {
            // This method should be on the UI thread. The overrides should ensure that
            ThreadHelper.ThrowIfNotOnUIThread();

            References.Add(name);
        }

        public virtual async Task AddReferenceAsync(string referencePath)
        {
            if (referencePath == null)
            {
                throw new ArgumentNullException(nameof(referencePath));
            }

            var name = Path.GetFileNameWithoutExtension(referencePath);
            var projectName = string.Empty;
            var projectFullPath = string.Empty;
            var assemblyFullPath = string.Empty;
            var dteProjectFullName = string.Empty;
            var dteOriginalPath = string.Empty;

            var resolvedToPackage = false;

            try
            {
                // Perform all DTE operations on the UI thread
                await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    // Read DTE properties from the UI thread
                    projectFullPath = ProjectFullPath;
                    projectName = ProjectName;
                    dteProjectFullName = VsProjectAdapter.FullName;

                    // Get the full path to the reference
                    assemblyFullPath = Path.Combine(projectFullPath, referencePath);

                    // Add a reference to the project
                    dynamic reference;
                    try
                    {
                        // First try the References3.AddFiles API, as that will incur fewer
                        // design-time builds.
                        References3.AddFiles(new[] { assemblyFullPath }, out var referencesArray);
                        var references = (VSLangProj.Reference[])referencesArray;
                        reference = references[0];
                    }
                    catch (Exception e)
                    {
                        if (e is InvalidCastException)
                        {
                            // We've encountered a project system that doesn't implement References3, or
                            // there's some sort of setup issue such that we can't find the library with
                            // the References3 type. Send a report about this.
                            TelemetryActivity.EmitTelemetryEvent(new TelemetryEvent("References3InvalidCastException"));
                        }

                        // If that didn't work, fall back to References.Add.
                        reference = References.Add(assemblyFullPath);
                    }

                    if (reference != null)
                    {
                        dteOriginalPath = GetReferencePath(reference);

                        // If path != fullPath, we need to set CopyLocal thru msbuild by setting Private
                        // to true.
                        // This happens if the assembly appears in any of the search paths that VS uses to
                        // locate assembly references.
                        // Most commonly, it happens if this assembly is in the GAC or in the output path.
                        // The path may be null or for some project system it can be "".
                        resolvedToPackage = !string.IsNullOrWhiteSpace(dteOriginalPath) && IsSamePath(dteOriginalPath, assemblyFullPath);

                        if (resolvedToPackage)
                        {
                            // Set reference properties (if needed)
                            TrySetCopyLocal(reference);
                            TrySetSpecificVersion(reference);
                        }
                    }
                });

                if (!resolvedToPackage)
                {
                    // This should be done off the UI thread

                    // Get the msbuild project for this project
                    var buildProject = EnvDTEProjectUtility.AsMSBuildEvaluationProject(dteProjectFullName);

                    if (buildProject != null)
                    {
                        // Get the assembly name of the reference we are trying to add
                        var assemblyName = AssemblyName.GetAssemblyName(assemblyFullPath);

                        // Try to find the item for the assembly name
                        var item = (from assemblyReferenceNode in buildProject.GetAssemblyReferences()
                                    where AssemblyNamesMatch(assemblyName, assemblyReferenceNode.Item2)
                                    select assemblyReferenceNode.Item1).FirstOrDefault();

                        if (item != null)
                        {
                            // Add the <HintPath> metadata item as a relative path
                            var projectPath = PathUtility.EnsureTrailingSlash(projectFullPath);
                            var relativePath = PathUtility.GetRelativePath(projectPath, referencePath);

                            item.SetMetadataValue("HintPath", relativePath);

                            // Set <Private> to true
                            item.SetMetadataValue("Private", "True");

                            FileSystemUtility.MakeWritable(dteProjectFullName);

                            // Change to the UI thread to save
                            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                            {
                                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                                // Save the project after we've modified it.
                                await SaveProjectAsync();
                            });
                        }
                    }
                    else
                    {
                        // The reference cannot be changed by modifying the project file.
                        // This could be a failure, however that could be a breaking
                        // change if there is a non-msbuild project system relying on this
                        // to skip references.
                        // Log a warning to let the user know that their reference may have failed.
                        NuGetProjectContext.Log(
                            ProjectManagement.MessageLevel.Warning,
                            Strings.FailedToAddReference,
                            name);
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Strings.FailedToAddReference, name), e);
            }

            NuGetProjectContext.Log(
                ProjectManagement.MessageLevel.Debug,
                Strings.Debug_AddedReferenceToProject,
                name, projectName, resolvedToPackage, dteOriginalPath, assemblyFullPath);
        }

        public virtual async Task RemoveReferenceAsync(string name)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                // Get the reference name without extension
                var referenceName = Path.GetFileNameWithoutExtension(name);

                // Remove the reference from the project
                // NOTE:- Project.Object.References.Item requires Reference.Identity
                //        which is, the Assembly name without path or extension
                //        But, we pass in the assembly file name. And, this works for
                //        almost all the assemblies since Assembly Name is the same as the assembly file name
                //        In case of F#, the input parameter is case-sensitive as well
                //        Hence, an override to THIS function is added to take care of that
                var reference = References.Item(referenceName);
                if (reference != null)
                {
                    reference.Remove();
                    NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_RemoveReference, name, ProjectName);
                }
            }
            catch (Exception e)
            {
                NuGetProjectContext.Log(ProjectManagement.MessageLevel.Warning, e.Message);
            }
        }

        public virtual async Task<bool> ReferenceExistsAsync(string name)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var referenceName = name;
                if (ProjectManagement.Constants.AssemblyReferencesExtensions.Contains(Path.GetExtension(name), StringComparer.OrdinalIgnoreCase))
                {
                    // Get the reference name without extension
                    referenceName = Path.GetFileNameWithoutExtension(name);
                }

                return References.Item(referenceName) != null;
            }
            catch
            {
            }

            return false;
        }

        private static bool IsReferenceUnavailableException(Exception e)
        {
            var comException = e as COMException;

            if (comException == null)
            {
                return false;
            }

            // If VSLangProj.References.Add(...) fails because it could not find the assembly,
            // the HRESULT will be E_FAIL (0x80004005) and the message will be "Reference unavailable."
            return comException.HResult == unchecked((int)0x80004005);
        }

        #endregion IProjectReferencesService

        #region IProjectSystemService

        protected async Task<EnvDTE.ProjectItems> GetProjectItemsAsync(string folderPath, bool createIfNotExists)
        {
            return await EnvDTEProjectUtility.GetProjectItemsAsync(VsProjectAdapter.Project, folderPath, createIfNotExists);
        }

        public async Task<EnvDTE.ProjectItem> GetProjectItemAsync(string path)
        {
            return await EnvDTEProjectUtility.GetProjectItemAsync(VsProjectAdapter.Project, path);
        }

        private async Task AddProjectItemAsync(string filePath, string folderPath, bool createFolderIfNotExists)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var container = await GetProjectItemsAsync(folderPath, createFolderIfNotExists);

            container.AddFromFileCopy(filePath);
        }

        public async Task SaveProjectAsync(CancellationToken _ = default(CancellationToken))
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                FileSystemUtility.MakeWritable(VsProjectAdapter.FullName);
                VsProjectAdapter.Project.Save();
            }
            catch (Exception ex)
            {
                NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, ex.Message);
                ExceptionHelper.WriteErrorToActivityLog(ex);
            }
        }

        #endregion IProjectSystemService
    }
}
