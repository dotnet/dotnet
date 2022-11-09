// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    internal class WritableSettingsStoreWrapper : SettingsStoreWrapper, IWritableSettingsStore
    {
        private readonly IVsWritableSettingsStore _store;

        public WritableSettingsStoreWrapper(IVsWritableSettingsStore store)
            : base(store)
        {
            _store = store;
        }

        public void DeleteCollection(string collection)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _store.DeleteCollection(collection);
            });
        }

        public void CreateCollection(string collection)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _store.CreateCollection(collection);
            });
        }

        public bool DeleteProperty(string collection, string propertyName)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                return ErrorHandler.Succeeded(_store.DeleteProperty(collection, propertyName));
            });
        }

        public void SetBoolean(string collection, string propertyName, bool value)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _store.SetBool(collection, propertyName, value ? 1 : 0);
            });
        }

        public void SetInt32(string collection, string propertyName, int value)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _store.SetInt(collection, propertyName, value);
            });
        }

        public void SetString(string collection, string propertyName, string value)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _store.SetString(collection, propertyName, value);
            });
        }
    }
}
