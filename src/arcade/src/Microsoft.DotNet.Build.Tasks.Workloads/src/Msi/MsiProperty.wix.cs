// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines MSI property names that can be used to query the Property table.
    /// </summary>
    internal static class MsiProperty
    {
        public static readonly string ProductCode = nameof(ProductCode);
        public static readonly string ProductLanguage = nameof(ProductLanguage);
        public static readonly string ProductName = nameof(ProductName);
        public static readonly string ProductVersion = nameof(ProductVersion);
        public static readonly string UpgradeCode = nameof(UpgradeCode);
    }
}
