// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IProductUpdateService))]
    internal class VSProductUpdateService : IProductUpdateService
    {
        public void CheckForAvailableUpdateAsync()
        {
            throw new NotSupportedException();
        }

        public void DeclineUpdate(bool doNotRemindAgain)
        {
            throw new NotSupportedException();
        }

        public void Update()
        {
            throw new NotSupportedException();
        }

        public event EventHandler<ProductUpdateAvailableEventArgs> UpdateAvailable = delegate { };
    }
}
