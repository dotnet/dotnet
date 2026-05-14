// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
