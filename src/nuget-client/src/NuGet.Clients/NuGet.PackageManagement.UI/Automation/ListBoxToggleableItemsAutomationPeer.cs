// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Automation peer to represent ListBox items which are used as toggles for an associated checkbox.
    /// Specifically, this is used for the representation of <see cref="PackageItemViewModel"/> in
    /// the <see cref="InfiniteScrollList"/> control for selecting packages to be Updated (eg, Updates tab).
    /// </summary>
    internal class ListBoxToggleableItemsAutomationPeer : ListBoxAutomationPeer
    {
        public ListBoxToggleableItemsAutomationPeer(ListBox owner)
            : base(owner)
        {
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Toggle)
            {
                return this;
            }
            return base.GetPattern(patternInterface);
        }

        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new ToggleableItemAutomationPeer(item, this);
        }
    }
}
