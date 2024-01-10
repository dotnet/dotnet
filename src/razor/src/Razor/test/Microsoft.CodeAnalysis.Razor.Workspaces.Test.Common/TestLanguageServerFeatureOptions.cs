// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    public static readonly LanguageServerFeatureOptions Instance = new TestLanguageServerFeatureOptions();

    private readonly bool _includeProjectKeyInGeneratedFilePath;
    private readonly bool _monitorWorkspaceFolderForConfigurationFiles;

    public TestLanguageServerFeatureOptions(
        bool includeProjectKeyInGeneratedFilePath = false,
        bool monitorWorkspaceFolderForConfigurationFiles = true)
    {
        _includeProjectKeyInGeneratedFilePath = includeProjectKeyInGeneratedFilePath;
        _monitorWorkspaceFolderForConfigurationFiles = monitorWorkspaceFolderForConfigurationFiles;
    }

    public override bool SupportsFileManipulation => false;

    public override string ProjectConfigurationFileName => "project.razor.bin";

    public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

    public override string HtmlVirtualDocumentSuffix => "__virtual.html";

    public override bool SingleServerCompletionSupport => false;

    public override bool SingleServerSupport => false;

    public override bool DelegateToCSharpOnDiagnosticPublish => true;

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => false;

    public override bool ShowAllCSharpCodeActions => false;

    public override bool UsePreciseSemanticTokenRanges => true;

    public override bool UpdateBuffersForClosedDocuments => false;

    public override bool IncludeProjectKeyInGeneratedFilePath => _includeProjectKeyInGeneratedFilePath;

    public override bool MonitorWorkspaceFolderForConfigurationFiles => _monitorWorkspaceFolderForConfigurationFiles;
}
