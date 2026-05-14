// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Microsoft.VisualStudio.PlatformUI;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// This control is used as list items in the package list. Its DataContext is
    /// <see cref="PackageItemViewModel"/>.
    /// </summary>
    public partial class PackageItemControl : UserControl
    {
        public PackageItemControl()
        {
            InitializeComponent();
        }

        // Whenever the checkbox in the item is checked, the containing item will raise a 
        // UIA event to convey the fact that the toggle state of the item has changed. The 
        // event must be raised regardless of whether the toggle state of the item changed 
        // in response to keyboard, mouse, or programmatic input.
        private void CheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            var itemCheckBox = sender as CheckBox;
            var itemContainer = itemCheckBox?.FindAncestor<ListBoxItem>();
            if (itemContainer is null)
            {
                return;
            }

            var newValue = (e.RoutedEvent == CheckBox.CheckedEvent);
            var oldValue = !newValue; // Assume the state has actually toggled.
            AutomationPeer peer = UIElementAutomationPeer.FromElement(itemContainer);
            peer?.RaisePropertyChangedEvent(
                TogglePatternIdentifiers.ToggleStateProperty,
                oldValue ? ToggleState.On : ToggleState.Off,
                newValue ? ToggleState.On : ToggleState.Off);
        }
    }
}
