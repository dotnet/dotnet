// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Windows.Documents;

namespace NuGet.PackageManagement.UI
{
    public class LicenseFileData : INotifyPropertyChanged
    {
        private string _header { get; set; }
        private FlowDocument _content { get; set; }

        public string LicenseHeader
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("LicenseHeader");
            }
        }

        public FlowDocument LicenseText
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged("LicenseText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
