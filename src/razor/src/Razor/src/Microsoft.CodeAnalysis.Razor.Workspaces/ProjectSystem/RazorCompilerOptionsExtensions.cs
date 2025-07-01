// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
