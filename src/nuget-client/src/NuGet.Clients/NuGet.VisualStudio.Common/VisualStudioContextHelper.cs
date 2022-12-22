// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    public static class VisualStudioContextHelper
    {
        public static async Task<bool> IsInServerModeAsync(CancellationToken token)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(token);
            IVsShell shell = await ServiceLocator.GetGlobalServiceAsync<SVsShell, IVsShell>();
            return shell != null &&
                shell.GetProperty((int)__VSSPROPID11.VSSPROPID_ShellMode, out object value) == VSConstants.S_OK &&
                value is int shellMode &&
                shellMode == (int)__VSShellMode.VSSM_Server;
        }
    }
}
