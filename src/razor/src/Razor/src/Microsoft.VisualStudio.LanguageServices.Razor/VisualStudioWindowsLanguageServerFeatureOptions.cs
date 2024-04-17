// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.Editor.Razor;

[Export(typeof(LanguageServerFeatureOptions))]
internal class VisualStudioWindowsLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    private const string ShowAllCSharpCodeActionsFeatureFlag = "Razor.LSP.ShowAllCSharpCodeActions";
    private const string IncludeProjectKeyInGeneratedFilePathFeatureFlag = "Razor.LSP.IncludeProjectKeyInGeneratedFilePath";
    private const string UsePreciseSemanticTokenRangesFeatureFlag = "Razor.LSP.UsePreciseSemanticTokenRanges";
    private const string UseRazorCohostServerFeatureFlag = "Razor.LSP.UseRazorCohostServer";
    private const string DisableRazorLanguageServerFeatureFlag = "Razor.LSP.DisableRazorLanguageServer";
    private const string ForceRuntimeCodeGenerationFeatureFlag = "Razor.LSP.ForceRuntimeCodeGeneration";

    private readonly LSPEditorFeatureDetector _lspEditorFeatureDetector;
    private readonly Lazy<bool> _showAllCSharpCodeActions;
    private readonly Lazy<bool> _includeProjectKeyInGeneratedFilePath;
    private readonly Lazy<bool> _usePreciseSemanticTokenRanges;
    private readonly Lazy<bool> _useRazorCohostServer;
    private readonly Lazy<bool> _disableRazorLanguageServer;
    private readonly Lazy<bool> _forceRuntimeCodeGeneration;

    [ImportingConstructor]
    public VisualStudioWindowsLanguageServerFeatureOptions(LSPEditorFeatureDetector lspEditorFeatureDetector)
    {
        if (lspEditorFeatureDetector is null)
        {
            throw new ArgumentNullException(nameof(lspEditorFeatureDetector));
        }

        _lspEditorFeatureDetector = lspEditorFeatureDetector;

        _showAllCSharpCodeActions = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var showAllCSharpCodeActions = featureFlags.IsFeatureEnabled(ShowAllCSharpCodeActionsFeatureFlag, defaultValue: false);
            return showAllCSharpCodeActions;
        });

        _includeProjectKeyInGeneratedFilePath = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var includeProjectKeyInGeneratedFilePath = featureFlags.IsFeatureEnabled(IncludeProjectKeyInGeneratedFilePathFeatureFlag, defaultValue: true);
            return includeProjectKeyInGeneratedFilePath;
        });

        _usePreciseSemanticTokenRanges = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var usePreciseSemanticTokenRanges = featureFlags.IsFeatureEnabled(UsePreciseSemanticTokenRangesFeatureFlag, defaultValue: false);
            return usePreciseSemanticTokenRanges;
        });

        _useRazorCohostServer = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var useRazorCohostServer = featureFlags.IsFeatureEnabled(UseRazorCohostServerFeatureFlag, defaultValue: false);
            return useRazorCohostServer;
        });

        _disableRazorLanguageServer = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var disableRazorLanguageServer = featureFlags.IsFeatureEnabled(DisableRazorLanguageServerFeatureFlag, defaultValue: false);
            return disableRazorLanguageServer;
        });

        _forceRuntimeCodeGeneration = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var disableRazorLanguageServer = featureFlags.IsFeatureEnabled(ForceRuntimeCodeGenerationFeatureFlag, defaultValue: false);
            return disableRazorLanguageServer;
        });
    }

    // We don't currently support file creation operations on VS Codespaces or VS Liveshare
    public override bool SupportsFileManipulation => !IsCodespacesOrLiveshare;

    // In VS we override the project configuration file name because we don't want our serialized state to clash with other platforms (VSCode)
    public override string ProjectConfigurationFileName => "project.razor.vs.bin";

    public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

    public override string HtmlVirtualDocumentSuffix => "__virtual.html";

    public override bool SingleServerCompletionSupport => true;

    public override bool SingleServerSupport => true;

    public override bool DelegateToCSharpOnDiagnosticPublish => false;

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => false;

    public override bool UpdateBuffersForClosedDocuments => false;

    private bool IsCodespacesOrLiveshare => _lspEditorFeatureDetector.IsRemoteClient() || _lspEditorFeatureDetector.IsLiveShareHost();

    public override bool ShowAllCSharpCodeActions => _showAllCSharpCodeActions.Value;

    public override bool IncludeProjectKeyInGeneratedFilePath => _includeProjectKeyInGeneratedFilePath.Value;

    public override bool UsePreciseSemanticTokenRanges => _usePreciseSemanticTokenRanges.Value;

    public override bool MonitorWorkspaceFolderForConfigurationFiles => false;

    public override bool UseRazorCohostServer => _useRazorCohostServer.Value;

    public override bool DisableRazorLanguageServer => _disableRazorLanguageServer.Value;

    /// <inheritdoc />
    public override bool ForceRuntimeCodeGeneration => _forceRuntimeCodeGeneration.Value;
}
