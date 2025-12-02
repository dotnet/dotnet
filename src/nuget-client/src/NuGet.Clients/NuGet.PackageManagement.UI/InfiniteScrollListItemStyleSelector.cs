// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Windows;
using System.Windows.Controls;

namespace NuGet.PackageManagement.UI
{
    public class InfiniteScrollListItemStyleSelector : StyleSelector
    {
        private Style _packageItemStyle;
        private Style _loadingStatusIndicatorStyle;

        private void Init(ItemsControl infiniteScrollList)
        {
            if (_packageItemStyle == null && _loadingStatusIndicatorStyle == null)
            {
                _packageItemStyle = (Style)infiniteScrollList.FindResource("packageItemStyle");
                _loadingStatusIndicatorStyle = (Style)infiniteScrollList.FindResource("loadingStatusIndicatorStyle");

                if (_packageItemStyle.Setters.Count == 0)
                {
                    _packageItemStyle.Setters.Add(new Setter(InfiniteScrollList.FocusVisualStyleProperty, infiniteScrollList.FindResource("MarginFocusVisualStyle")));
                    _packageItemStyle.Setters.Add(new Setter(InfiniteScrollList.TemplateProperty, infiniteScrollList.FindResource("ListBoxItemTemplate")));
                }
            }
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            Init(ItemsControl.ItemsControlFromItemContainer(container));

            if (item is LoadingStatusIndicator)
            {
                return _loadingStatusIndicatorStyle;
            }

            return _packageItemStyle;
        }
    }
}
