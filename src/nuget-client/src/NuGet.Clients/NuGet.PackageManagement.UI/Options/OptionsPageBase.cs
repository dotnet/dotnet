// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI.Options
{
    [ComVisible(true)]
    public abstract class OptionsPageBase : DialogPage
    {
        protected OptionsPageBase()
        {
        }

        [SuppressMessage(
            "Microsoft.Mobility",
            "CA1601:DoNotUseTimersThatPreventPowerStateChanges",
            Justification = "This is a ridiculous rule.")]
        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The timer is disposed in the Tick event handler.")]
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // delay 5 milliseconds to give the Options dialog a chance to close itself
            var timer = new Timer
            {
                Interval = 5
            };
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var timer = (Timer)sender;
            timer.Stop();
            timer.Dispose();

            var optionsPageActivator = ServiceLocator.GetComponentModelService<IOptionsPageActivator>();
            if (optionsPageActivator != null)
            {
                optionsPageActivator.NotifyOptionsDialogClosed();
            }
        }

        // We override the base implementation of LoadSettingsFromStorage and SaveSettingsToStorage
        // since we already provide settings persistance using the SettingsManager. These two APIs
        // will read/write the tools/options properties to an alternate location, which can cause
        // incorrect behavior if the two copies of the data are out of sync.
        public override void LoadSettingsFromStorage()
        {
        }

        public override void SaveSettingsToStorage()
        {
        }
    }
}
