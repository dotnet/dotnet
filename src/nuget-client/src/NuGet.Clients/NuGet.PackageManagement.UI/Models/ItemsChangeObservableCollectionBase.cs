// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// When we bind to an <see cref="ObservableCollection{T}">ObservableCollection</see>, and change Filter criteria based on user input, the WPF binding mechanism doesn't know to refresh the View.
    /// This collection provides an event to say the entire collection needs to be re-evaluated, without raising events for each item, and without refreshing the entire View.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemsChangeObservableCollection<T> : ObservableCollection<T>
    {
        public ItemsChangeObservableCollection()
            : base() { }

        public ItemsChangeObservableCollection(IEnumerable<T> collection)
            : base(collection) { }
        public void Refresh()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddRange(IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                Items.Add(item);
            }
            Refresh();
        }
    }
}
