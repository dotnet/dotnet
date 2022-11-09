// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement
{
    public interface IProductUpdateService
    {
        void CheckForAvailableUpdateAsync();
        void Update();
        void DeclineUpdate(bool doNotRemindAgain);
        event EventHandler<ProductUpdateAvailableEventArgs> UpdateAvailable;
    }

    public class ProductUpdateAvailableEventArgs : EventArgs
    {
        public ProductUpdateAvailableEventArgs(Version currentVersion, Version newVersion)
        {
            CurrentVersion = currentVersion;
            NewVersion = newVersion;
        }

        public Version CurrentVersion { get; private set; }
        public Version NewVersion { get; private set; }
    }
}
