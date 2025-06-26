// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
