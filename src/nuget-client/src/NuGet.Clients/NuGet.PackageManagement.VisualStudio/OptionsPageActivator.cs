// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.VisualStudio.Options;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IOptionsPageActivator))]
    public class OptionsPageActivator : IOptionsPageActivator
    {
        /// <summary>
        /// GUID of the General page, declared by the `legacyOptionPageId` in registration.json
        /// with the Unified Settings `serviceId` implemented by <see cref="GeneralPage"/>.
        /// </summary>
        private const string GeneralGUID = "0F052CF7-BF62-4743-B190-87FA4D49421E";

        /// <summary>
        /// GUID of the Configuration Files page, declared by the `legacyOptionPageId` in registration.json
        /// with the Unified Settings `serviceId` implemented by <see cref="ConfigurationFilesPage"/>.
        /// </summary>
        private const string ConfigurationFilesGUID = "C17B308A-00BB-446E-9212-2D14E1005985";

        /// <summary>
        /// GUID of the Package Sources page, declared by the `legacyOptionPageId` in registration.json
        /// with the Unified Settings `serviceId` implemented by <see cref="PackageSourcesPage"/>.
        /// </summary>
        private const string PackageSourcesGUID = "2819C3B6-FC75-4CD5-8C77-877903DE864C";

        /// <summary>
        /// GUID of the Package Source Mapping page, declared by the `legacyOptionPageId` in registration.json
        /// with the Unified Settings `serviceId` implemented by <see cref="PackageSourceMappingPage"/>.
        /// </summary>
        private const string PackageSourceMappingGUID = "F175964E-89F5-4521-8FE2-C10C07BB968C";


        private Action _closeCallback;
        private readonly AsyncLazy<IVsUIShell> _vsUIShell;

        [ImportingConstructor]
        public OptionsPageActivator()
        {
            _vsUIShell = new AsyncLazy<IVsUIShell>(async () =>
            {
                return await AsyncServiceProvider.GlobalProvider.GetServiceAsync<IVsUIShell, IVsUIShell>(throwOnFailure: false);
            },
            NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public void NotifyOptionsDialogClosed()
        {
            if (_closeCallback != null)
            {
                // We want to clear the value of _closeCallback before invoking it.
                // Hence copying the value into a local variable.
                var callback = _closeCallback;
                _closeCallback = null;

                callback();
            }
        }

        public void ActivatePage(OptionsPage page, Action closeCallback)
        {
            _closeCallback = closeCallback;
            if (page == OptionsPage.General)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ShowOptionsPageAsync(GeneralGUID);
                }).PostOnFailure(nameof(OptionsPageActivator), nameof(ActivatePage));
            }
            else if (page == OptionsPage.PackageSources)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ShowOptionsPageAsync(PackageSourcesGUID);
                }).PostOnFailure(nameof(OptionsPageActivator), nameof(ActivatePage));
            }
            else if (page == OptionsPage.PackageSourceMapping)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ShowOptionsPageAsync(PackageSourceMappingGUID);
                }).PostOnFailure(nameof(OptionsPageActivator), nameof(ActivatePage));
            }
            else if (page == OptionsPage.ConfigurationFiles)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ShowOptionsPageAsync(ConfigurationFilesGUID);
                }).PostOnFailure(nameof(OptionsPageActivator), nameof(ActivatePage));
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }
        }

        private async Task ShowOptionsPageAsync(string optionsPageGuid)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            object targetGuid = optionsPageGuid;
            var toolsGroupGuid = VSConstants.GUID_VSStandardCommandSet97;
            IVsUIShell vsUIShell = await _vsUIShell.GetValueAsync();
            vsUIShell.PostExecCommand(
                ref toolsGroupGuid,
                VSConstants.cmdidToolsOptions,
                0,
                ref targetGuid);
        }
    }
}
