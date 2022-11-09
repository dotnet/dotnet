// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains events which are raised when packages are installed or uninstalled from projects and the current
    /// solution.
    /// </summary>
    [ComImport]
    [Guid("65E435C1-6970-4BBF-8842-5DBCB0707711")]
    public interface IVsPackageInstallerEvents
    {
        /// <summary>
        /// Raised when a package is about to be installed into the current solution.
        /// </summary>
        event VsPackageEventHandler PackageInstalling;

        /// <summary>
        /// Raised after a package has been installed into the current solution.
        /// </summary>
        event VsPackageEventHandler PackageInstalled;

        /// <summary>
        /// Raised when a package is about to be uninstalled from the current solution.
        /// </summary>
        event VsPackageEventHandler PackageUninstalling;

        /// <summary>
        /// Raised after a package has been uninstalled from the current solution.
        /// </summary>
        event VsPackageEventHandler PackageUninstalled;

        /// <summary>
        /// Raised after a package has been installed into a project within the current solution.
        /// </summary>
        event VsPackageEventHandler PackageReferenceAdded;

        /// <summary>
        /// Raised after a package has been uninstalled from a project within the current solution.
        /// </summary>
        event VsPackageEventHandler PackageReferenceRemoved;
    }
}
