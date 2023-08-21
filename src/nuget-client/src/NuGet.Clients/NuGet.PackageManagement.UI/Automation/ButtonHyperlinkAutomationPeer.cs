// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Automation.Peers;
using NuGet.PackageManagement.UI.Controls;

namespace NuGet.PackageManagement.UI.Automation
{
    internal class ButtonHyperlinkAutomationPeer : HyperlinkAutomationPeer
    {
        public ButtonHyperlinkAutomationPeer(ButtonHyperlink owner)
                : base(owner)
        { }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }
    }
}
