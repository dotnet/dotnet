// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class TestRazorSemanticTokensLegend : RazorSemanticTokensLegend
{
    public static TestRazorSemanticTokensLegend Instance = new();

    private TestRazorSemanticTokensLegend()
        : base(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true })
    {
    }
}
