// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal static class TestRazorSemanticTokensLegendService
{
    public static RazorSemanticTokensLegendService Instance = new(new TestClientCapabilitiesService(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true }));
}
