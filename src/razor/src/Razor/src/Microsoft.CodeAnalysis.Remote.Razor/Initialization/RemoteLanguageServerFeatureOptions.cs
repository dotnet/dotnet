// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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

    public override string CSharpVirtualDocumentSuffix => throw new InvalidOperationException("This property is not valid in OOP");

    public override string HtmlVirtualDocumentSuffix => _options.HtmlVirtualDocumentSuffix;

    public override bool SingleServerSupport => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool DelegateToCSharpOnDiagnosticPublish => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool UsePreciseSemanticTokenRanges => _options.UsePreciseSemanticTokenRanges;

    public override bool ShowAllCSharpCodeActions => _options.ShowAllCSharpCodeActions;

    public override bool UpdateBuffersForClosedDocuments => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => _options.ReturnCodeActionAndRenamePathsWithPrefixedSlash;

    public override bool IncludeProjectKeyInGeneratedFilePath => throw new InvalidOperationException("This option does not apply in cohosting.");

    public override bool UseRazorCohostServer => _options.UseRazorCohostServer;

    public override bool ForceRuntimeCodeGeneration => true;

    public override bool UseNewFormattingEngine => true;

    public override bool SupportsSoftSelectionInCompletion => _options.SupportsSoftSelectionInCompletion;

    public override bool UseVsCodeCompletionTriggerCharacters => _options.UseVsCodeCompletionTriggerCharacters;

    public override bool DoNotInitializeMiscFilesProjectFromWorkspace => throw new NotImplementedException("This option has not been synced to OOP.");
}
