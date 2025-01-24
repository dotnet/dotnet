// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.UI.Options
{
    /// <summary>
    /// A stub legacy options General page for use when the Unified Settings feature is enabled.
    /// NuGet's General settings are now in the new UI.
    /// </summary>
    [Guid("41B6E760-DFED-44E3-8CA5-AAEC83FBC9DB")]
    public class StubGeneralOptionsPage : UIElementDialogPage
    {
        private Lazy<StubGeneralOptionControl> _stubGeneralOptionControl;

        /// <summary>
        /// Gets the Windows Presentation Foundation (WPF) child element to be hosted inside the Options dialog page.
        /// </summary>
        /// <returns>The WPF child element.</returns>
        protected override UIElement Child => _stubGeneralOptionControl.Value;

        public StubGeneralOptionsPage()
        {
            _stubGeneralOptionControl = new Lazy<StubGeneralOptionControl>(() => new StubGeneralOptionControl());
        }
    }
}
