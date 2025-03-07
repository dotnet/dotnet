// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(LanguageServerFeatureOptions))]
internal class VisualStudioLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    private readonly ILspEditorFeatureDetector _lspEditorFeatureDetector;
    private readonly Lazy<bool> _showAllCSharpCodeActions;
    private readonly Lazy<bool> _includeProjectKeyInGeneratedFilePath;
    private readonly Lazy<bool> _usePreciseSemanticTokenRanges;
    private readonly Lazy<bool> _useRazorCohostServer;
    private readonly Lazy<bool> _forceRuntimeCodeGeneration;
    private readonly Lazy<bool> _useNewFormattingEngine;

    [ImportingConstructor]
    public VisualStudioLanguageServerFeatureOptions(ILspEditorFeatureDetector lspEditorFeatureDetector)
    {
        if (lspEditorFeatureDetector is null)
        {
            throw new ArgumentNullException(nameof(lspEditorFeatureDetector));
        }

        _lspEditorFeatureDetector = lspEditorFeatureDetector;

        _showAllCSharpCodeActions = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var showAllCSharpCodeActions = featureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.ShowAllCSharpCodeActions, defaultValue: false);
            return showAllCSharpCodeActions;
        });

        _includeProjectKeyInGeneratedFilePath = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var includeProjectKeyInGeneratedFilePath = featureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.IncludeProjectKeyInGeneratedFilePath, defaultValue: true);
            return includeProjectKeyInGeneratedFilePath;
        });

        _usePreciseSemanticTokenRanges = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var usePreciseSemanticTokenRanges = featureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.UsePreciseSemanticTokenRanges, defaultValue: false);
            return usePreciseSemanticTokenRanges;
        });

        _useRazorCohostServer = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var useRazorCohostServer = featureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.UseRazorCohostServer, defaultValue: false);
            return useRazorCohostServer;
        });

        _forceRuntimeCodeGeneration = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var forceRuntimeCodeGeneration = featureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.ForceRuntimeCodeGeneration, defaultValue: true);
            return forceRuntimeCodeGeneration;
        });

        _useNewFormattingEngine = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)Package.GetGlobalService(typeof(SVsFeatureFlags));
            var useNewFormattingEngine = featureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.UseNewFormattingEngine, defaultValue: true);
            return useNewFormattingEngine;
        });
    }

    // We don't currently support file creation operations on VS Codespaces or VS Liveshare
    public override bool SupportsFileManipulation => !IsCodespacesOrLiveshare;

    public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

    public override string HtmlVirtualDocumentSuffix => "__virtual.html";

    public override bool SingleServerSupport => true;

    public override bool DelegateToCSharpOnDiagnosticPublish => false;

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => false;

    public override bool UpdateBuffersForClosedDocuments => false;

    private bool IsCodespacesOrLiveshare => _lspEditorFeatureDetector.IsRemoteClient() || _lspEditorFeatureDetector.IsLiveShareHost();

    public override bool ShowAllCSharpCodeActions => _showAllCSharpCodeActions.Value;

    public override bool IncludeProjectKeyInGeneratedFilePath => _includeProjectKeyInGeneratedFilePath.Value;

    public override bool UsePreciseSemanticTokenRanges => _usePreciseSemanticTokenRanges.Value;

    public override bool UseRazorCohostServer => _useRazorCohostServer.Value;

    /// <inheritdoc />
    public override bool ForceRuntimeCodeGeneration => _forceRuntimeCodeGeneration.Value;

    public override bool UseNewFormattingEngine => _useNewFormattingEngine.Value;

    // VS actually needs explicit commit characters so don't avoid them.
    public override bool SupportsSoftSelectionInCompletion => true;

    public override bool UseVsCodeCompletionTriggerCharacters => false;

    // In VS, we do not want the language server to add all documents in the workspace root path
    // to the misc-files project when initialized.
    public override bool DoNotInitializeMiscFilesProjectFromWorkspace => true;
}
