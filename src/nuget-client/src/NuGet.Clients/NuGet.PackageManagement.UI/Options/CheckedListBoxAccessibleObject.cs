// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Forms;
using NuGet.Configuration;
using NuGet.VisualStudio.Internal.Contracts;
using static System.Windows.Forms.Control;

namespace NuGet.PackageManagement.UI.Options
{
    internal class CheckedListBoxAccessibleObject : ControlAccessibleObject
    {
        public CheckedListBoxAccessibleObject(PackageSourceCheckedListBox owner) : base(owner) { }

        private PackageSourceCheckedListBox CheckedListBox
        {
            get
            {
                return (PackageSourceCheckedListBox)Owner;
            }
        }

        internal void SelectChild(int index)
        {
            if (index >= 0 && index < CheckedListBox.Items.Count)
            {
                CheckedListBox.SetSelected(index, true);
            }
        }

        public override AccessibleObject GetChild(int index)
        {
            if (index >= 0 && index < CheckedListBox.Items.Count)
            {
                var item = (PackageSourceContextInfo)CheckedListBox.Items[index];
                PackageSource packageSource = new PackageSource(item.Source, item.Name);
                if (packageSource.IsHttp && !packageSource.IsHttps)
                {
                    var sourceMessage = string.Concat(
                        Resources.Warning_HTTPSource,
                        packageSource.Source);
                    return new CheckedListBoxItemAccessibleObject(this, packageSource.Name, index, sourceMessage);
                }

                return new CheckedListBoxItemAccessibleObject(this, packageSource.Name, index, packageSource.Source);
            }
            else
            {
                return null;
            }
        }

        public override int GetChildCount()
        {
            return CheckedListBox.Items.Count;
        }

        public override AccessibleObject GetFocused()
        {
            var index = CheckedListBox.FocusedIndex;
            if (index >= 0)
            {
                return GetChild(index);
            }

            return null;
        }

        public override AccessibleObject GetSelected()
        {
            var index = CheckedListBox.SelectedIndex;
            if (index >= 0)
            {
                return GetChild(index);
            }

            return null;
        }

        public override AccessibleObject HitTest(int x, int y)
        {
            var count = GetChildCount();
            for (var index = 0; index < count; ++index)
            {
                var child = GetChild(index);
                if (child.Bounds.Contains(x, y))
                {
                    return child;
                }
            }

            if (Bounds.Contains(x, y))
            {
                return this;
            }

            return null;
        }

        public override AccessibleObject Navigate(AccessibleNavigation direction)
        {
            if (GetChildCount() > 0)
            {
                if (direction == AccessibleNavigation.FirstChild)
                {
                    return GetChild(0);
                }
                if (direction == AccessibleNavigation.LastChild)
                {
                    return GetChild(GetChildCount() - 1);
                }
            }
            return base.Navigate(direction);
        }
    }
}
