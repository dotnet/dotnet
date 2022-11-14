// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using NuGet.PackageManagement.UI;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.Options
{
    public class PackageSourceViewModel : INotifyPropertyChanged, ISelectableItem
    {
        private bool _isSelected;

        public PackageSourceViewModel(PackageSourceContextInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public PackageSourceContextInfo SourceInfo { get; private set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
