// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.VisualStudio.Razor;

internal static class WellKnownFeatureFlagNames
{
    public const string ShowAllCSharpCodeActions = "Razor.LSP.ShowAllCSharpCodeActions";
    public const string IncludeProjectKeyInGeneratedFilePath = "Razor.LSP.IncludeProjectKeyInGeneratedFilePath";
    public const string UsePreciseSemanticTokenRanges = "Razor.LSP.UsePreciseSemanticTokenRanges";
    public const string UseRazorCohostServer = "Razor.LSP.UseRazorCohostServer";
    public const string DisableRazorLanguageServer = "Razor.LSP.DisableRazorLanguageServer";
    public const string UseRoslynTokenizer = "Razor.LSP.UseRoslynTokenizer";
    public const string UseNewFormattingEngine = "Razor.LSP.UseNewFormattingEngine";
}
