// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class LoadingStatusExtensionMethods
    {
        public static LoadingStatus Aggregate(this IEnumerable<LoadingStatus> statuses)
        {
            var count = statuses?.Count() ?? 0;

            if (count == 0)
            {
                return LoadingStatus.Loading;
            }

            var first = statuses.First();
            if (count == 1 || statuses.All(x => x == first))
            {
                return first;
            }

            if (statuses.Contains(LoadingStatus.Loading))
            {
                return LoadingStatus.Loading;
            }

            if (statuses.Contains(LoadingStatus.ErrorOccurred))
            {
                return LoadingStatus.ErrorOccurred;
            }

            if (statuses.Contains(LoadingStatus.Cancelled))
            {
                return LoadingStatus.Cancelled;
            }

            if (statuses.Contains(LoadingStatus.Ready))
            {
                return LoadingStatus.Ready;
            }

            if (statuses.Contains(LoadingStatus.NoMoreItems))
            {
                return LoadingStatus.NoMoreItems;
            }

            if (statuses.Contains(LoadingStatus.NoItemsFound))
            {
                return LoadingStatus.NoItemsFound;
            }

            return first;
        }
    }
}
