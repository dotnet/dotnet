// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Replacement tokens used to generate MSI source files.
    /// </summary>
    internal class MsiTokens
    {        
        public const string __MANUFACTURER__ = nameof(__MANUFACTURER__);
        public const string __NAME__ = nameof(__NAME__);
        public const string __PACKAGE_ID__ = nameof(__PACKAGE_ID__);
        public const string __PACKAGE_VERSION__ = nameof(__PACKAGE_VERSION__);
        public const string __PRODUCTCODE__ = nameof(__PRODUCTCODE__);
        public const string __PROVIDER_KEY_NAME__ = nameof(__PROVIDER_KEY_NAME__);
        public const string __EULA_RTF__ = nameof(__EULA_RTF__);
        public const string __UPGRADECODE__ = nameof(__UPGRADECODE__);
        public const string __VERSION__ = nameof(__VERSION__);
    }
}
