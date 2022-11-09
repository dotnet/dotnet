// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Package project events relayed to the public IVsPackageInstallerProjectEvents.
    /// </summary>
    public class PackageProjectEvents
    {
        /// <summary>
        /// Raised when batch processing of install/ uninstall packages starts at a project level
        /// </summary>
        public event EventHandler<PackageProjectEventArgs> BatchStart;

        /// <summary>
        /// Raised when batch processing of install/ uninstall packages ends at a project level
        /// </summary>
        public event EventHandler<PackageProjectEventArgs> BatchEnd;

        internal void NotifyBatchStart(PackageProjectEventArgs e)
        {
            var handler = BatchStart;
            handler?.Invoke(this, e);
        }

        internal void NotifyBatchEnd(PackageProjectEventArgs e)
        {
            var handler = BatchEnd;
            handler?.Invoke(this, e);
        }
    }
}
