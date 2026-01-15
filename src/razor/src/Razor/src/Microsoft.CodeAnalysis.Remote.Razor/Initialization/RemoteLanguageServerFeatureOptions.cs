// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.NET.Sdk.Razor.SourceGenerators;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[Export(typeof(LanguageServerFeatureOptions))]
[Export(typeof(RemoteLanguageServerFeatureOptions))]
internal class RemoteLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    private RemoteClientInitializationOptions _options = default;

    public void SetOptions(RemoteClientInitializationOptions options)
    {
        _options = options;

        // ensure the source generator is in the correct mode
        RazorCohostingOptions.UseRazorCohostServer = options.UseRazorCohostServer;
    }

    public override bool SupportsFileManipulation => _options.SupportsFileManipulation;

    public override bool ShowAllCSharpCodeActions => _options.ShowAllCSharpCodeActions;

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => _options.ReturnCodeActionAndRenamePathsWithPrefixedSlash;

    public override bool UseRazorCohostServer => _options.UseRazorCohostServer;
}
