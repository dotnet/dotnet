// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Defines replacement token names used by SWIX project templates.
    /// </summary>
    public static class SwixTokens
    {
        public static readonly string __VS_COMPONENT_CATEGORY__ = nameof(__VS_COMPONENT_CATEGORY__);
        public static readonly string __VS_COMPONENT_DESCRIPTION__ = nameof(__VS_COMPONENT_DESCRIPTION__);
        public static readonly string __VS_COMPONENT_TITLE__ = nameof(__VS_COMPONENT_TITLE__);
        public static readonly string __VS_IS_UI_GROUP__ = nameof(__VS_IS_UI_GROUP__);
        public static readonly string __VS_IS_ADVERTISED_PACKAGE__ = nameof(__VS_IS_ADVERTISED_PACKAGE__);
        public static readonly string __VS_PACKAGE_CHIP__ = nameof(__VS_PACKAGE_CHIP__);
        public static readonly string __VS_PACKAGE_INSTALL_SIZE_SYSTEM_DRIVE__ = nameof(__VS_PACKAGE_INSTALL_SIZE_SYSTEM_DRIVE__);
        public static readonly string __VS_PACKAGE_NAME__ = nameof(__VS_PACKAGE_NAME__);
        public static readonly string __VS_PACKAGE_OUT_OF_SUPPORT__ = nameof(__VS_PACKAGE_OUT_OF_SUPPORT__);
        public static readonly string __VS_PACKAGE_PRODUCT_ARCH__ = nameof(__VS_PACKAGE_PRODUCT_ARCH__);
        public static readonly string __VS_PAYLOAD_SIZE__ = nameof(__VS_PAYLOAD_SIZE__);
        public static readonly string __VS_PAYLOAD_SOURCE__ = nameof(__VS_PAYLOAD_SOURCE__);
        public static readonly string __VS_PACKAGE_VERSION__ = nameof(__VS_PACKAGE_VERSION__);
    }
}
