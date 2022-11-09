// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Internal.Microsoft.DotNet.PlatformAbstractions
{
    internal static class ApplicationEnvironment
    {
        public static string ApplicationBasePath { get; } = GetApplicationBasePath();

        private static string GetApplicationBasePath()
        {
            var basePath = AppContext.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}
