// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    public const string DefaultCSharpVirtualDocumentSuffix = ".ide.g.cs";
    public const string DefaultHtmlVirtualDocumentSuffix = "__virtual.html";

    public override bool SupportsFileManipulation => true;

    public override string CSharpVirtualDocumentSuffix => DefaultCSharpVirtualDocumentSuffix;

    public override string HtmlVirtualDocumentSuffix => DefaultHtmlVirtualDocumentSuffix;

    public override bool SingleServerSupport => false;

    public override bool DelegateToCSharpOnDiagnosticPublish => false;

    public override bool UpdateBuffersForClosedDocuments => false;

    // Code action and rename paths in Windows VS Code need to be prefixed with '/':
    // https://github.com/dotnet/razor/issues/8131
    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => PlatformInformation.IsWindows;

    public override bool ShowAllCSharpCodeActions => false;

    public override bool IncludeProjectKeyInGeneratedFilePath => false;

    public override bool UsePreciseSemanticTokenRanges => false;

    public override bool UseRazorCohostServer => false;

    public override bool UseNewFormattingEngine => true;

    public override bool SupportsSoftSelectionInCompletion => true;

    public override bool UseVsCodeCompletionTriggerCharacters => false;

    public override bool DoNotInitializeMiscFilesProjectFromWorkspace => false;
}
