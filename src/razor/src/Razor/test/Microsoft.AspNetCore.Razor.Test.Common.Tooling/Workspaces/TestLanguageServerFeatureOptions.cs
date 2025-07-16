// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.Test.Common.Workspaces;

internal class TestLanguageServerFeatureOptions(
    bool includeProjectKeyInGeneratedFilePath = false,
    bool updateBuffersForClosedDocuments = false,
    bool useNewFormattingEngine = false,
    bool supportsSoftSelectionInCompletion = true,
    bool vsCodeCompletionTriggerCharacters = false,
    bool doNotInitializeMiscFilesProjectWithWorkspaceFiles = false) : LanguageServerFeatureOptions
{
    public static readonly LanguageServerFeatureOptions Instance = new TestLanguageServerFeatureOptions();

    public override bool SupportsFileManipulation => false;

    public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

    public override string HtmlVirtualDocumentSuffix => "__virtual.html";

    public override bool SingleServerSupport => false;

    public override bool DelegateToCSharpOnDiagnosticPublish => true;

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => false;

    public override bool ShowAllCSharpCodeActions => false;

    public override bool UsePreciseSemanticTokenRanges => true;

    public override bool UpdateBuffersForClosedDocuments => updateBuffersForClosedDocuments;

    public override bool IncludeProjectKeyInGeneratedFilePath => includeProjectKeyInGeneratedFilePath;

    public override bool UseRazorCohostServer => false;

    public override bool UseNewFormattingEngine => useNewFormattingEngine;

    public override bool SupportsSoftSelectionInCompletion => supportsSoftSelectionInCompletion;

    public override bool UseVsCodeCompletionTriggerCharacters => vsCodeCompletionTriggerCharacters;

    public override bool DoNotInitializeMiscFilesProjectFromWorkspace => doNotInitializeMiscFilesProjectWithWorkspaceFiles;
}
