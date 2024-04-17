// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[Export(typeof(LanguageServerFeatureOptions))]
internal class RemoteLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    // It's okay to use default here because we expect the options to be set before the first real OOP call
    private static RemoteClientInitializationOptions s_options = default;

    public static void SetOptions(RemoteClientInitializationOptions options) => s_options = options;

    public override bool SupportsFileManipulation => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override string ProjectConfigurationFileName => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override string CSharpVirtualDocumentSuffix => s_options.CSharpVirtualDocumentSuffix;

    public override string HtmlVirtualDocumentSuffix => s_options.HtmlVirtualDocumentSuffix;

    public override bool SingleServerCompletionSupport => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool SingleServerSupport => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool DelegateToCSharpOnDiagnosticPublish => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool UsePreciseSemanticTokenRanges => s_options.UsePreciseSemanticTokenRanges;

    public override bool ShowAllCSharpCodeActions => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool UpdateBuffersForClosedDocuments => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool IncludeProjectKeyInGeneratedFilePath => s_options.IncludeProjectKeyInGeneratedFilePath;

    public override bool MonitorWorkspaceFolderForConfigurationFiles => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool UseRazorCohostServer => s_options.UseRazorCohostServer;

    public override bool DisableRazorLanguageServer => throw new InvalidOperationException("This option has not been synced to OOP.");

    public override bool ForceRuntimeCodeGeneration => throw new InvalidOperationException("This option has not been synced to OOP.");
}
