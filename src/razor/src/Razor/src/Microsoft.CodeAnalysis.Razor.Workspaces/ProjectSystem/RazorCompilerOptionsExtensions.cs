// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class RazorCompilerOptionsExtensions
{
    public static RazorCompilerOptions ToCompilerOptions(this LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        var options = RazorCompilerOptions.None;

        if (languageServerFeatureOptions.ForceRuntimeCodeGeneration)
        {
            options.SetFlag(RazorCompilerOptions.ForceRuntimeCodeGeneration);
        }

        return options;
    }
}
