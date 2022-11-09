// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Defines the names of well-known WiX extensions that can be used by different tools.
    /// </summary>
    public static class WixExtensions
    {
        /// <summary>
        /// Provides custom actions and tables to support dependency provider keys used
        /// to manage MSI reference counts.
        /// </summary>
        public static readonly string WixDependencyExtension = nameof(WixDependencyExtension);

        /// <summary>
        /// Provides custom UI functionality such as different dialog sets for MSIs.
        /// </summary>
        public static readonly string WixUIExtension = nameof(WixUIExtension);

        /// <summary>
        /// Provides various custom actions and compiler extensions for MSIs and bundles.
        /// </summary>
        public static readonly string WixUtilExtension = nameof(WixUtilExtension);
    }
}
