// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// List of package search statuses as presented to a user in a package manager UI.
    /// </summary>
    internal enum PackageSearchStatus
    {
        Unknown,
        [Description(nameof(Resources.Text_UserCanceled))]
        Cancelled,
        [Description(nameof(Resources.Text_ErrorOccurred))]
        ErrorOccurred,
        [Description(nameof(Resources.Text_Loading))]
        Loading,
        [Description(nameof(Resources.Text_NoPackagesFound))]
        NoPackagesFound,
        [Description(nameof(Resources.Text_PackagesFound))]
        PackagesFound
    }
}
