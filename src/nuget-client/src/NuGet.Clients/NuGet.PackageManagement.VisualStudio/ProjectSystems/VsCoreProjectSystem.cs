// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    internal class VsCoreProjectSystem : IProjectSystemService
    {
        private IVsProjectAdapter VsProjectAdapter { get; }

        public async Task SaveProjectAsync(CancellationToken token)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                FileSystemUtility.MakeWritable(VsProjectAdapter.FullName);
                VsProjectAdapter.Project.Save();
            }
            catch (Exception ex)
            {
                ExceptionHelper.WriteErrorToActivityLog(ex);
            }
        }

        public VsCoreProjectSystem(
            IVsProjectAdapter vsProjectAdapter)
        {
            Assumes.Present(vsProjectAdapter);

            VsProjectAdapter = vsProjectAdapter;
        }
    }
}
