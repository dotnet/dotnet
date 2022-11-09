// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace NuGet.Protocol.Core.Types
{
    public class HttpSourceCacheContext
    {
        private HttpSourceCacheContext(string rootTempFolder, TimeSpan maxAge, bool directDownload, SourceCacheContext cacheContext)
        {
            if (maxAge <= TimeSpan.Zero)
            {
                if (rootTempFolder == null)
                {
                    throw new ArgumentNullException(nameof(rootTempFolder));
                }
            }
            else
            {
                Debug.Assert(
                    rootTempFolder == null,
                    $"{nameof(rootTempFolder)} should be null when {nameof(maxAge)} is not zero.");
            }

            RootTempFolder = rootTempFolder;
            MaxAge = maxAge;
            DirectDownload = directDownload;
            SourceCacheContext = cacheContext ?? throw new ArgumentNullException(nameof(cacheContext));
        }

        public TimeSpan MaxAge { get; }

        public bool DirectDownload { get; }

        /// <summary>
        /// A suggested root folder to drop temporary files under, it will get cleared by the
        /// disposal of the <see cref="SourceCacheContext"/> that was used to create this instance.
        /// </summary>
        public string RootTempFolder { get; }

        /// <summary>
        /// Inner cache context.
        /// </summary>
        public SourceCacheContext SourceCacheContext { get; }

        public static HttpSourceCacheContext Create(SourceCacheContext cacheContext, int retryCount)
        {
            return Create(cacheContext, retryCount == 0);
        }

        public static HttpSourceCacheContext Create(SourceCacheContext cacheContext, bool isFirstAttempt)
        {
            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (isFirstAttempt && cacheContext.MaxAgeTimeSpan > TimeSpan.Zero)
            {
                return new HttpSourceCacheContext(
                    rootTempFolder: null,
                    maxAge: cacheContext.MaxAgeTimeSpan,
                    directDownload: cacheContext.DirectDownload,
                    cacheContext: cacheContext);
            }
            else
            {
                return new HttpSourceCacheContext(
                    cacheContext.GeneratedTempFolder,
                    TimeSpan.Zero,
                    cacheContext.DirectDownload,
                    cacheContext: cacheContext);
            }
        }
    }
}
