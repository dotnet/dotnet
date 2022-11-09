// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    internal class LoadingStatusIndicator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private LoadingStatus _status = LoadingStatus.Unknown;
        private string _errorMessage;
        private string _loadingMessage;

        public LoadingStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public string LoadingMessage
        {
            get
            {
                return _loadingMessage;
            }
            set
            {
                if (_loadingMessage != value)
                {
                    _loadingMessage = value;
                    OnPropertyChanged(nameof(LoadingMessage));
                }
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        public void Reset(string loadingMessage)
        {
            Status = LoadingStatus.Unknown;
            LoadingMessage = loadingMessage;
        }

        public void SetError(string message)
        {
            Status = LoadingStatus.ErrorOccurred;
            ErrorMessage = message;
        }

        internal string LocalizedStatus
        {
            get
            {
                switch (Status)
                {
                    case LoadingStatus.Loading:
                        return LoadingMessage;

                    case LoadingStatus.NoItemsFound:
                        return Resources.Text_NoPackagesFound;

                    case LoadingStatus.Cancelled:
                        return Resources.Status_Canceled;

                    case LoadingStatus.ErrorOccurred:
                        return Resources.Status_ErrorOccurred;

                    case LoadingStatus.NoMoreItems:
                        return Resources.Status_NoMoreItems;

                    case LoadingStatus.Ready:
                        return Resources.Status_Ready;

                    default:
                        return string.Empty;
                }
            }
        }
    }
}
