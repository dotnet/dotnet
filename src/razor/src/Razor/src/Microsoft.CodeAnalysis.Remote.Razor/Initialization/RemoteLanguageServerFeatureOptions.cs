// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[Export(typeof(LanguageServerFeatureOptions))]
[Export(typeof(RemoteLanguageServerFeatureOptions))]
internal class RemoteLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    private RemoteClientInitializationOptions _options = default;

    public void SetOptions(RemoteClientInitializationOptions options) => _options = options;

    public override bool SupportsFileManipulation => _options.SupportsFileManipulation;

    public override string CSharpVirtualDocumentSuffix => _options.CSharpVirtualDocumentSuffix;

    public override string HtmlVirtualDocumentSuffix => _options.HtmlVirtualDocumentSuffix;

    public override bool SingleServerSupport => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool DelegateToCSharpOnDiagnosticPublish => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool UsePreciseSemanticTokenRanges => _options.UsePreciseSemanticTokenRanges;

    public override bool ShowAllCSharpCodeActions => _options.ShowAllCSharpCodeActions;

    public override bool UpdateBuffersForClosedDocuments => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => _options.ReturnCodeActionAndRenamePathsWithPrefixedSlash;

    public override bool IncludeProjectKeyInGeneratedFilePath => _options.IncludeProjectKeyInGeneratedFilePath;

    public override bool UseRazorCohostServer => _options.UseRazorCohostServer;

    public override bool DisableRazorLanguageServer => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool ForceRuntimeCodeGeneration => _options.ForceRuntimeCodeGeneration;

    public override bool UseRoslynTokenizer => _options.UseRoslynTokenizer;
}
