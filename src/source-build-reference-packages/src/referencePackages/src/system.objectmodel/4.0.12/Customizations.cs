// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

namespace System.Collections.ObjectModel
{
    public partial class ObservableCollection<T> : Collection<T>, Specialized.INotifyCollectionChanged, ComponentModel.INotifyPropertyChanged
    {
        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged { add { } remove { } }
    }

        public partial class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, Specialized.INotifyCollectionChanged, ComponentModel.INotifyPropertyChanged
    {
        event System.Collections.Specialized.NotifyCollectionChangedEventHandler System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged { add { } remove { } }

        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged { add { } remove { } }
    }
}