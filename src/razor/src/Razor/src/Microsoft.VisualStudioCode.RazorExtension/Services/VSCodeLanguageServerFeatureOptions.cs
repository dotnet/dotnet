// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[Shared]
[Export(typeof(LanguageServerFeatureOptions))]
[method: ImportingConstructor]
internal class VSCodeLanguageServerFeatureOptions() : LanguageServerFeatureOptions
{
    // Options that are set to their defaults
    public override bool SupportsFileManipulation => true;
    public override bool SingleServerSupport => false;
    public override bool ShowAllCSharpCodeActions => false;
    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => PlatformInformation.IsWindows;
    public override bool IncludeProjectKeyInGeneratedFilePath => false;
    public override bool DoNotInitializeMiscFilesProjectFromWorkspace => false;

    // Options that differ from the default
    public override string CSharpVirtualDocumentSuffix => "__virtual.cs";
    public override string HtmlVirtualDocumentSuffix => "__virtual.html";
    public override bool UpdateBuffersForClosedDocuments => true;
    public override bool DelegateToCSharpOnDiagnosticPublish => true;
    public override bool SupportsSoftSelectionInCompletion => false;
    public override bool UseVsCodeCompletionCommitCharacters => true;

    // User configurable options
    public override bool UseRazorCohostServer => true;
}
