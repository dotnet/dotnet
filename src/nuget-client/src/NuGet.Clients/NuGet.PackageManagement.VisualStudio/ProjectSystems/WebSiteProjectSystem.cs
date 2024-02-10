// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using VsWebSite;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    public class WebSiteProjectSystem : WebProjectSystem
    {
        private const string RootNamespace = "RootNamespace";
        private const string AppCodeFolder = "App_Code";
        private const string DefaultNamespace = "ASP";
        private const string GeneratedFilesFolder = "Generated___Files";
        private readonly HashSet<string> _excludedCodeFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static readonly string[] SourceFileExtensions = { ".cs", ".vb" };

        public WebSiteProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
            : base(vsProjectAdapter, nuGetProjectContext)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We want to catch all exceptions")]
        public override async Task AddReferenceAsync(string referencePath)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var name = Path.GetFileNameWithoutExtension(referencePath);
            try
            {
                EnvDTEProjectUtility.GetAssemblyReferences(VsProjectAdapter.Project).AddFromFile(PathUtility.GetAbsolutePath(ProjectFullPath, referencePath));

                // Always create a refresh file. Vs does this for us in most cases, however for GACed binaries, it resorts to adding a web.config entry instead.
                // This may result in deployment issues. To work around ths, we'll always attempt to add a file to the bin.
                RefreshFileUtility.CreateRefreshFile(ProjectFullPath, PathUtility.GetAbsolutePath(ProjectFullPath, referencePath), this);

                NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, $"Added reference '{name}' to project:'{ProjectName}' ");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.FailedToAddReference, name), e);
            }
        }

        public override void AddGacReference(string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            EnvDTEProjectUtility.GetAssemblyReferences(VsProjectAdapter.Project).AddFromGAC(name);
        }

        public override async Task RemoveReferenceAsync(string name)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Remove the reference via DTE.
            RemoveDTEReference(name);

            // For GACed binaries, VS would not clear the refresh files for us since it assumes the reference exists in web.config.
            // We'll clean up any remaining .refresh files.
            var refreshFilePath = Path.Combine("bin", Path.GetFileName(name) + ".refresh");
            var refreshFileFullPath = FileSystemUtility.GetFullPath(ProjectFullPath, refreshFilePath);
            if (File.Exists(refreshFileFullPath))
            {
                try
                {
                    FileSystemUtility.DeleteFile(refreshFileFullPath, NuGetProjectContext);
                }
                catch (Exception e)
                {
                    NuGetProjectContext.Log(ProjectManagement.MessageLevel.Warning, e.Message);
                }
            }
        }

        /// <summary>
        /// Removes a reference via the DTE.
        /// </summary>
        /// <remarks>This is identical to VsProjectSystem.RemoveReference except in the way we process exceptions.</remarks>
        private void RemoveDTEReference(string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Get the reference name without extension
            var referenceName = Path.GetFileNameWithoutExtension(name);

            // Remove the reference from the project
            AssemblyReference reference = null;
            try
            {
                reference = EnvDTEProjectUtility.GetAssemblyReferences(VsProjectAdapter.Project).Item(referenceName);
                if (reference != null)
                {
                    reference.Remove();
                    NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_RemoveReference, name, ProjectName);
                }
            }
            catch (Exception ex)
            {
                var messageLevel = ProjectManagement.MessageLevel.Warning;
                if (reference != null
                    && reference.ReferenceKind == AssemblyReferenceType.AssemblyReferenceConfig)
                {
                    // Bug 2319: Strong named assembly references are specified via config and may be specified in the root web.config. Attempting to remove these
                    // references always throws and there isn't an easy way to identify this. Instead, we'll attempt to lower the level of the message so it doesn't
                    // appear as readily.

                    messageLevel = ProjectManagement.MessageLevel.Debug;
                }
                NuGetProjectContext.Log(messageLevel, ex.Message);
            }
        }

        public override string ResolvePath(string path)
        {
            // If we're adding a source file that isn't already in the app code folder then add App_Code to the path
            if (RequiresAppCodeRemapping(path))
            {
                path = Path.Combine(AppCodeFolder, path);
            }

            return base.ResolvePath(path);
        }

        /// <summary>
        /// Determines if we need a source file to be under the App_Code folder
        /// </summary>
        private bool RequiresAppCodeRemapping(string path)
        {
            return !_excludedCodeFiles.Contains(path) && !IsUnderAppCode(path) && IsSourceFile(path);
        }

        private static bool IsUnderAppCode(string path)
        {
            return PathUtility.EnsureTrailingSlash(path).StartsWith(AppCodeFolder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsSourceFile(string path)
        {
            var extension = Path.GetExtension(path);
            return SourceFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        public override void RemoveImport(string targetFullPath)
        {
            // Web sites are not msbuild based and do not support imports.
        }

        public override void AddImport(string targetFullPath, ImportLocation location)
        {
            // Web sites are not msbuild based and do not support imports.
        }

        protected override bool ExcludeFile(string path)
        {
            // Exclude nothing from website projects
            return false;
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        // Website project properties are only available via DTE.
        public override dynamic GetPropertyValue(string propertyName)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            if (propertyName.Equals(RootNamespace, StringComparison.OrdinalIgnoreCase))
            {
                return DefaultNamespace;
            }
#pragma warning disable CS0618 // Type or member is obsolete
            return base.GetPropertyValue(propertyName);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public override IEnumerable<string> GetDirectories(string path)
        {
            if (IsUnderAppCode(path))
            {
                // There is an invisible folder called Generated___Files under app code that we want to exclude from our search
                return base.GetDirectories(path).Except(new[] { GeneratedFilesFolder }, StringComparer.OrdinalIgnoreCase);
            }

            return base.GetDirectories(path);
        }

        public override Task BeginProcessingAsync()
        {
            return Task.CompletedTask;
        }

        public override void RegisterProcessedFiles(IEnumerable<string> files)
        {
            // Need NOT be on the UI thread (but not thread safe)

            var orderedFiles = files.OrderBy(path => path)
                .ToList();

            foreach (var path1 in orderedFiles)
            {
                foreach (var path2 in orderedFiles)
                {
                    if (path1.Equals(path2, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (path1.StartsWith(path2, StringComparison.OrdinalIgnoreCase) &&
                        IsSourceFile(path1))
                    {
                        _excludedCodeFiles.Add(path1);
                    }
                }
            }
        }

        public override Task EndProcessingAsync()
        {
            _excludedCodeFiles.Clear();

            return Task.CompletedTask;
        }
    }
}
