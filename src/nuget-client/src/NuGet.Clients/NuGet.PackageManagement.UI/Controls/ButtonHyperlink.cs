// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using NuGet.PackageManagement.UI.Automation;

namespace NuGet.PackageManagement.UI.Controls
{
    /// <summary>
    /// Use this control when we need to display a <c>Hyperlink</c> in our UI, but need it to function as a button.
    /// The custom <see cref="ButtonHyperlinkAutomationPeer"/> indicates to assistive tooling (eg, screen readers) that interacting
    /// with this control will function as a <c>Button</c>, even though this class is implemented as a <c>Hyperlink</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class ButtonHyperlink : Hyperlink
    {
        public ButtonHyperlink() : base()
        { }

        public ButtonHyperlink(Inline childInline) : base(childInline)
        { }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ButtonHyperlinkAutomationPeer(this);
        }
    }
}
