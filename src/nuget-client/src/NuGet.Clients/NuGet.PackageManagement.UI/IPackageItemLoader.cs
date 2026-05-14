// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// This enhance IItemLoader by adding package specific methods.
    /// </summary>
    internal interface IPackageItemLoader : IItemLoader<PackageItemViewModel>
    {
        Task<SearchResultContextInfo> SearchAsync(CancellationToken cancellationToken);

        Task UpdateStateAndReportAsync(SearchResultContextInfo searchResult,
            IProgress<IItemLoaderState> progress, CancellationToken cancellationToken);
    }
}
