// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using NuGetConsole.Implementation;

namespace NuGetConsole
{
    public static class CommandUiUtilities
    {
        private static readonly AsyncLazy<IVsInvalidateCachedCommandState> CommandStateCacheInvalidator;

        static CommandUiUtilities()
        {
            CommandStateCacheInvalidator = new AsyncLazy<IVsInvalidateCachedCommandState>(
                () => ServiceLocator.GetGlobalServiceAsync<SVsInvalidateCachedCommandState, IVsInvalidateCachedCommandState>(),
                NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public static async Task InvalidateDefaultProjectAsync()
        {
            var invalidator = await CommandStateCacheInvalidator.GetValueAsync();

            var command = new VSCommandId()
            {
                CommandSet = GuidList.guidNuGetCmdSet,
                CommandId = PkgCmdIDList.cmdidProjects
            };

            // This interface is free-threaded.
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            invalidator.InvalidateSpecificCommandUIState(command);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }
    }
}
