// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace NuGet.PackageManagement.UI
{
    internal class ToggleableItemAutomationPeer : ListBoxItemAutomationPeer, IToggleProvider
    {
        private readonly ISelectableItemsControl _ownerParent;
        private readonly ISelectableItem _owner;

        public ToggleableItemAutomationPeer(object item, SelectorAutomationPeer selectorAutomationPeer)
            : base(item, selectorAutomationPeer)
        {
            _owner = item as ISelectableItem;
            _ownerParent = selectorAutomationPeer.Owner as ISelectableItemsControl;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Toggle && _ownerParent?.IsItemSelectionEnabled == true)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        /// <summary>
        /// The default will be assumed to be toggled off (eg, Checked is false), therefore translating to `ToggleState.Off`.
        /// </summary>
        public ToggleState ToggleState
        {
            get
            {
                return _owner?.IsSelected == true ? ToggleState.On : ToggleState.Off;
            }
        }

        public void Toggle()
        {
            if (_owner != null)
            {
                _owner.IsSelected = !_owner.IsSelected;
            }
        }
    }
}
