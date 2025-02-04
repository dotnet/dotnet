// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

namespace System.Dynamic
{
    public sealed partial class ExpandoObject : Collections.Generic.IDictionary<string, object>, Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object>>, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object>>, Collections.IEnumerable, ComponentModel.INotifyPropertyChanged, IDynamicMetaObjectProvider
    {
        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged { add { } remove { } }
    }
}
