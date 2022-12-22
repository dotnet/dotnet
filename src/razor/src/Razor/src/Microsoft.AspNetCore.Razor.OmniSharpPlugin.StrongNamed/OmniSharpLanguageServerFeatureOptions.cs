// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin.StrongNamed;

public class OmniSharpLanguageServerFeatureOptions
{
    private LanguageServerFeatureOptions _internalLanguageServerFeatureOptions = new DefaultLanguageServerFeatureOptions();

    internal LanguageServerFeatureOptions InternalLanguageServerFeatureOptions => _internalLanguageServerFeatureOptions;
}
