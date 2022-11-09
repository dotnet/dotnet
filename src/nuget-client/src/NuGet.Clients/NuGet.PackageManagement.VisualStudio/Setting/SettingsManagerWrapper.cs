// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    internal class SettingsManagerWrapper : ISettingsManager
    {
        private readonly AsyncLazy<IVsSettingsManager> _settingsManager;

        public SettingsManagerWrapper(Microsoft.VisualStudio.Shell.IAsyncServiceProvider serviceProvider)
        {
            _settingsManager = new AsyncLazy<IVsSettingsManager>(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var settingsManager = await serviceProvider.GetServiceAsync<SVsSettingsManager, IVsSettingsManager>();
                Assumes.Present(settingsManager);

                return settingsManager;

            }, NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public ISettingsStore GetReadOnlySettingsStore()
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var settingsManager = await _settingsManager.GetValueAsync();

                IVsSettingsStore settingsStore;
                var hr = settingsManager.GetReadOnlySettingsStore((uint)__VsSettingsScope.SettingsScope_UserSettings, out settingsStore);
                if (ErrorHandler.Succeeded(hr)
                    && settingsStore != null)
                {
                    return new SettingsStoreWrapper(settingsStore);
                }

                return null;
            });
        }

        public IWritableSettingsStore GetWritableSettingsStore()
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var settingsManager = await _settingsManager.GetValueAsync();

                IVsWritableSettingsStore settingsStore;
                var hr = settingsManager.GetWritableSettingsStore((uint)__VsSettingsScope.SettingsScope_UserSettings, out settingsStore);
                if (ErrorHandler.Succeeded(hr)
                    && settingsStore != null)
                {
                    return new WritableSettingsStoreWrapper(settingsStore);
                }

                return null;
            });
        }
    }
}
