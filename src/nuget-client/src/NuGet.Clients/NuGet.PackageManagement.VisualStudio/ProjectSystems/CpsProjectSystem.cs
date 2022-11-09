// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public abstract class CpsProjectSystem : VsMSBuildProjectSystem
    {
        protected CpsProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
            : base(vsProjectAdapter, nuGetProjectContext)
        {
        }

        public override void AddGacReference(string name)
        {
            // Native & JS projects don't know about GAC
        }

        public override void AddImport(string targetFullPath, ImportLocation location)
        {
            // For VS 2012 or above, the operation has to be done inside the Writer lock
            if (string.IsNullOrEmpty(targetFullPath))
            {
                throw new ArgumentNullException(nameof(targetFullPath));
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                var root = VsProjectAdapter.ProjectDirectory;
                var relativeTargetPath = PathUtility.GetRelativePath(PathUtility.EnsureTrailingSlash(root), targetFullPath);
                await AddImportStatementAsync(location, relativeTargetPath);
            });
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private async Task AddImportStatementAsync(ImportLocation location, string relativeTargetPath)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await ProjectHelper.DoWorkInWriterLockAsync(
                VsProjectAdapter.Project,
                VsProjectAdapter.VsHierarchy,
                buildProject => MicrosoftBuildEvaluationProjectUtility.AddImportStatement(buildProject, relativeTargetPath, location));

            // notify the project system of the change
            UpdateImportStamp(VsProjectAdapter);
        }

        public override void RemoveImport(string targetFullPath)
        {
            if (string.IsNullOrEmpty(targetFullPath))
            {
                throw new ArgumentNullException(nameof(targetFullPath), CommonResources.Argument_Cannot_Be_Null_Or_Empty);
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                var root = VsProjectAdapter.ProjectDirectory;
                // For VS 2012 or above, the operation has to be done inside the Writer lock
                var relativeTargetPath = PathUtility.GetRelativePath(PathUtility.EnsureTrailingSlash(root), targetFullPath);
                await RemoveImportStatementAsync(relativeTargetPath);
            });
        }

        // IMPORTANT: The NoInlining is required to prevent CLR from loading VisualStudio12.dll assembly while running 
        // in VS2010 and VS2012
        [MethodImpl(MethodImplOptions.NoInlining)]
        private async Task RemoveImportStatementAsync(string relativeTargetPath)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await ProjectHelper.DoWorkInWriterLockAsync(
                VsProjectAdapter.Project,
                VsProjectAdapter.VsHierarchy,
                buildProject => MicrosoftBuildEvaluationProjectUtility.RemoveImportStatement(buildProject, relativeTargetPath));

            // notify the project system of the change
            UpdateImportStamp(VsProjectAdapter);
        }
    }
}
