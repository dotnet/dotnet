// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.Shell;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using EnvDTEProjectItems = EnvDTE.ProjectItems;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    public class JsProjectSystem : CpsProjectSystem
    {
        public JsProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
            : base(vsProjectAdapter, nuGetProjectContext)
        {
        }

        private string _projectName;

        public override string ProjectName
        {
            get
            {
                if (string.IsNullOrEmpty(_projectName))
                {
                    NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                        {
                            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                            _projectName = VsProjectAdapter.ProjectName;
                        });
                }
                return _projectName;
            }
        }

        public override void AddFile(string path, Stream stream)
        {
            // ensure the parent folder is created before adding file to the project
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await GetProjectItemsAsync(Path.GetDirectoryName(path), createIfNotExists: true);
                    base.AddFile(path, stream);
                });
        }

        public override void AddFile(string path, Action<Stream> writeToStream)
        {
            // ensure the parent folder is created before adding file to the project    
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await GetProjectItemsAsync(Path.GetDirectoryName(path), createIfNotExists: true);
                    base.AddFile(path, writeToStream);
                });
        }

        protected override async Task AddFileToProjectAsync(string path)
        {
            if (ExcludeFile(path))
            {
                return;
            }

            var folderPath = Path.GetDirectoryName(path);
            var fullPath = FileSystemUtility.GetFullPath(ProjectFullPath, path);

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Add the file to project or folder
            EnvDTEProjectItems container = await GetProjectItemsAsync(folderPath, createIfNotExists: true);
            if (container == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_FailedToCreateParentFolder,
                        path,
                        ProjectName));
            }
            container.AddFromFileCopy(fullPath);

            NuGetProjectContext.Log(ProjectManagement.MessageLevel.Debug, Strings.Debug_AddedFileToProject, path, ProjectName);
        }
    }
}
