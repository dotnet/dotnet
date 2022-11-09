// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace NuGet.PackageManagement.UI
{
    public class ProjectsListView : ListView, ISelectableItemsControl
    {
        public bool IsItemSelectionEnabled
        {
            get => true;

            set => throw new NotImplementedException();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            var listViewPeer = new ListViewToggleableItemsAutomationPeer(this);
            if (View is GridView gridView)
            {
                // Hook up an automation peer for the GridView. If the ProjectsListView ever uses a different view,
                // this should be updated to create an AutomationPeer appropriate to that view.
                // Unfortunately, we can't call GetAutomationPeer on the ViewBase due to its restricted access level so we
                // create an automation peer for the appropriate view ourselves and hook it up.
                listViewPeer.UpdateViewAutomationPeer(new GridViewAutomationPeer(gridView, this));
            }

            return listViewPeer;
        }
    }
}
