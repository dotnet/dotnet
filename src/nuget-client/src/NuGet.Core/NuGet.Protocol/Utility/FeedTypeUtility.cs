// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Protocol
{
    public static class FeedTypeUtility
    {
        /// <summary>
        /// Determine the type of a nuget source. This works for both offline and online sources.
        /// </summary>
        public static FeedType GetFeedType(PackageSource packageSource)
        {
            // If the feed type is already specified, use that value
            if (packageSource is FeedTypePackageSource feedTypePackageSource)
            {
                return feedTypePackageSource.FeedType;
            }

            // Default to unknown file system
            var type = FeedType.FileSystemUnknown;

            if (packageSource.IsHttp)
            {
                if (packageSource.Source.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    type = FeedType.HttpV3;
                }
                else
                {
                    type = FeedType.HttpV2;
                }
            }
            else if (packageSource.IsLocal)
            {
                var path = UriUtility.GetLocalPath(packageSource.Source);

                if (!Directory.Exists(path))
                {
                    // If the directory doesn't exist check again later
                    type = FeedType.FileSystemUnknown;
                }
                else
                {
                    // Try to determine the actual folder feed type by looking for nupkgs
                    type = LocalFolderUtility.GetLocalFeedType(path, NullLogger.Instance);
                }
            }

            return type;
        }
    }
}
