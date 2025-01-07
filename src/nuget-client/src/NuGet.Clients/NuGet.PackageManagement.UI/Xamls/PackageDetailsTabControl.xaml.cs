// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows.Controls;
using NuGet.PackageManagement.UI.ViewModels;

namespace NuGet.PackageManagement.UI
{

    /// <summary>
    /// Interaction logic for PackageDetailsTabControl.xaml
    /// </summary>
    public partial class PackageDetailsTabControl : UserControl, IDisposable
    {
        public PackageDetailsTabViewModel PackageDetailsTabViewModel
        {
            get => DataContext as PackageDetailsTabViewModel;
        }

        private bool _disposed = false;

        public PackageDetailsTabControl()
        {
            InitializeComponent();
            DataContext = new PackageDetailsTabViewModel();
            PackageReadmeControl = new PackageReadmeControl();
        }

        public PackageReadmeControl PackageReadmeControl { get; private set; }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                PackageDetailsTabViewModel.Dispose();
                PackageReadmeControl.Dispose();
            }
            _disposed = true;
        }
    }
}
