// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Microsoft.VisualStudio.Threading;

namespace NuGet.PackageManagement.UI
{
    internal class InfiniteScrollListBox : ListBox, ISelectableItemsControl
    {
        public bool IsItemSelectionEnabled { get; set; }

        public ReentrantSemaphore ItemsLock { get; set; }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ListBoxToggleableItemsAutomationPeer(this);
        }
    }
}
