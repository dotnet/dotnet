// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Razor;

internal static class WellKnownFeatureFlagNames
{
    public const string ShowAllCSharpCodeActions = "Razor.LSP.ShowAllCSharpCodeActions";
    public const string IncludeProjectKeyInGeneratedFilePath = "Razor.LSP.IncludeProjectKeyInGeneratedFilePath";
    public const string UsePreciseSemanticTokenRanges = "Razor.LSP.UsePreciseSemanticTokenRanges";
    public const string UseRazorCohostServer = "Razor.LSP.UseRazorCohostServer";
    public const string DisableRazorLanguageServer = "Razor.LSP.DisableRazorLanguageServer";
    public const string ForceRuntimeCodeGeneration = "Razor.LSP.ForceRuntimeCodeGeneration";
    public const string UseRoslynTokenizer = "Razor.LSP.UseRoslynTokenizer";
    public const string UseNewFormattingEngine = "Razor.LSP.UseNewFormattingEngine";
}
