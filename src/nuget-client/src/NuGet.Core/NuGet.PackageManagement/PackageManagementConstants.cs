// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement
{
    public static class PackageManagementConstants
    {
        /// <summary>
        /// Default MaxDegreeOfParallelism to use for restores and other threaded operations.
        /// </summary>
        public static readonly int DefaultMaxDegreeOfParallelism = Environment.ProcessorCount;

        /// <summary>
        /// Default amount of time a source request can take before timing out. This includes both UNC shares
        /// and online sources.
        /// </summary>
        public static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromMinutes(15);
    }
}
