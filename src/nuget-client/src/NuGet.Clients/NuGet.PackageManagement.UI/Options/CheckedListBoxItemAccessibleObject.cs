// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft;

namespace NuGet.PackageManagement.UI.Options
{
    internal class CheckedListBoxItemAccessibleObject : AccessibleObject
    {
        private readonly string _helpText;
        private readonly int _index;
        private readonly CheckedListBoxAccessibleObject _parent;
        private string _name;

        public CheckedListBoxItemAccessibleObject(CheckedListBoxAccessibleObject parent, string name, int index, string helpText) : base()
        {
            Assumes.Present(parent);

            _name = name;
            _parent = parent;
            _index = index;
            _helpText = helpText;
        }

        public override Rectangle Bounds
        {
            get
            {
                var rect = ParentCheckedListBox.GetItemRectangle(_index);

                var pt = new NativeMethods.POINT(rect.X, rect.Y);
                _ = NativeMethods.ClientToScreen(new HandleRef(ParentCheckedListBox, ParentCheckedListBox.Handle), pt);

                return new Rectangle(pt.x, pt.y, rect.Width, rect.Height);
            }
        }

        public override string DefaultAction
        {
            get
            {
                return ParentCheckedListBox.GetItemChecked(_index)
                    ? Resources.CheckBox_DefaultAction_Uncheck
                    : Resources.CheckBox_DefaultAction_Check;
            }
        }

        private CheckedListBox ParentCheckedListBox
        {
            get
            {
                return (CheckedListBox)_parent.Owner;
            }
        }

        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override string Help
        {
            get { return _helpText; }
        }

        public override AccessibleObject Parent
        {
            get
            {
                return _parent;
            }
        }

        public override AccessibleRole Role
        {
            get
            {
                return AccessibleRole.CheckButton;
            }
        }

        public override AccessibleStates State
        {
            get
            {
                var state = AccessibleStates.Selectable | AccessibleStates.Focusable;

                switch (ParentCheckedListBox.GetItemCheckState(_index))
                {
                    case CheckState.Checked:
                        state |= AccessibleStates.Checked;
                        break;
                    case CheckState.Indeterminate:
                        state |= AccessibleStates.Indeterminate;
                        break;
                    case CheckState.Unchecked:
                        break;
                }

                if (ParentCheckedListBox.SelectedIndex == _index)
                {
                    state |= AccessibleStates.Selected | AccessibleStates.Focused;
                }

                return state;

            }
        }

        public override string Value
        {
            get
            {
                return ParentCheckedListBox.GetItemChecked(_index).ToString(CultureInfo.CurrentCulture);
            }
        }

        public override void DoDefaultAction()
        {
            ParentCheckedListBox.SetItemChecked(_index, !ParentCheckedListBox.GetItemChecked(_index));
        }

        public override AccessibleObject Navigate(AccessibleNavigation direction)
        {
            if (direction == AccessibleNavigation.Down ||
                direction == AccessibleNavigation.Next)
            {
                if (_index < _parent.GetChildCount() - 1)
                {
                    return _parent.GetChild(_index + 1);
                }
            }

            if (direction == AccessibleNavigation.Up ||
                direction == AccessibleNavigation.Previous)
            {
                if (_index > 0)
                {
                    return _parent.GetChild(_index - 1);
                }
            }

            return base.Navigate(direction);
        }

        public override void Select(AccessibleSelection flags)
        {
            (ParentCheckedListBox.AccessibilityObject as CheckedListBoxAccessibleObject)?.SelectChild(_index);
        }
    }
}
