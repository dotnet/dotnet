// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Test;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal static class TestRazorSemanticTokensLegendService
{
    private static RazorSemanticTokensLegendService s_vsInstance = new(new TestClientCapabilitiesService(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true }));
    private static RazorSemanticTokensLegendService s_vsCodeInstance = new(new TestClientCapabilitiesService(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = false }));

    public static RazorSemanticTokensLegendService GetInstance(bool supportsVSExtensions)
        => supportsVSExtensions
            ? s_vsInstance
            : s_vsCodeInstance;
}
