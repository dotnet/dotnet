// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace NuGet.PackageManagement.UI
{
    internal class ListViewToggleableItemsAutomationPeer : ListViewAutomationPeer
    {
        public ListViewToggleableItemsAutomationPeer(ListView owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Allows access to update the ViewAutomationPeer of the ListViewAutomationPeer.
        /// This allows us to override the ListView automation peer without having to also
        /// reimplement automation peers for the View and its Items.
        /// </summary>
        /// <param name="viewAutomationPeer">The automation peer for the ListView's View</param>
        public void UpdateViewAutomationPeer(IViewAutomationPeer viewAutomationPeer)
        {
            ViewAutomationPeer = viewAutomationPeer;
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
