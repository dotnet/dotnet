// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft;

namespace NuGet.PackageManagement.UI
{
    internal static class SortableColumnHeaderAttachedProperties
    {
        public static readonly DependencyProperty SortPropertyNameProperty =
    DependencyProperty.RegisterAttached("SortPropertyName", typeof(string), typeof(SortableColumnHeaderAttachedProperties), new PropertyMetadata(null));

        public static readonly DependencyProperty SortDirectionProperty =
            DependencyProperty.RegisterAttached("SortDirectionProperty", typeof(ListSortDirection?), typeof(SortableColumnHeaderAttachedProperties), new PropertyMetadata(null, SortDirectionProperty_PropertyChanged));


        public static string GetSortPropertyName(DependencyObject obj)
        {
            Assumes.Present(obj);
            return (string)obj?.GetValue(SortPropertyNameProperty);
        }

        public static void SetSortPropertyName(DependencyObject obj, string value)
        {
            Assumes.Present(obj);
            obj?.SetValue(SortPropertyNameProperty, value);
        }

        public static ListSortDirection? GetSortDirectionProperty(DependencyObject obj)
        {
            Assumes.Present(obj);
            return (ListSortDirection?)obj?.GetValue(SortDirectionProperty);
        }

        public static void SetSortDirectionProperty(DependencyObject obj, ListSortDirection value)
        {
            Assumes.Present(obj);
            obj?.SetValue(SortDirectionProperty, value);
        }

        public static void RemoveSortDirectionProperty(DependencyObject obj)
        {
            Assumes.Present(obj);
            obj?.SetValue(SortDirectionProperty, null);
        }

        private static void SortDirectionProperty_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var columnHeader = d as GridViewColumnHeader;
            if (columnHeader == null)
            {
                return;
            }

            var oldValue = (ListSortDirection?)e.OldValue;
            var newValue = (ListSortDirection?)e.NewValue;

            var layer = AdornerLayer.GetAdornerLayer(columnHeader);

            if (layer == null || oldValue == newValue)
            {
                return;
            }

            if (oldValue != null)
            {
                var adorners = layer.GetAdorners(columnHeader);
                if (adorners != null)
                {
                    foreach (var adorner in adorners)
                    {
                        if (adorner is ArrowGlyphAdorner)
                        {
                            layer.Remove(adorner);
                        }
                    }
                }
            }

            if (newValue == null)
            {
                return;
            }

            layer.Add(new ArrowGlyphAdorner(columnHeader, newValue.Value));
        }
    }
}
