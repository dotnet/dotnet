// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.Test.Common.Workspaces;

internal class TestLanguageServerFeatureOptions(
    bool includeProjectKeyInGeneratedFilePath = false,
    bool forceRuntimeCodeGeneration = false,
    bool updateBuffersForClosedDocuments = false) : LanguageServerFeatureOptions
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

    public override bool DisableRazorLanguageServer => false;

    public override bool ForceRuntimeCodeGeneration => forceRuntimeCodeGeneration;
}
