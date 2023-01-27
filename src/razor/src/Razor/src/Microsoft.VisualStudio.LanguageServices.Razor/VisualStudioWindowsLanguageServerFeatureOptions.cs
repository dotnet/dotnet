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
    private const string SingleServerCompletionFeatureFlag = "Razor.LSP.SingleServerCompletion";
    private const string SingleServerFeatureFlag = "Razor.LSP.SingleServer";

    private readonly LSPEditorFeatureDetector _lspEditorFeatureDetector;
    private readonly Lazy<bool> _singleServerCompletionSupport;
    private readonly Lazy<bool> _singleServerSupport;

    [ImportingConstructor]
    public VisualStudioWindowsLanguageServerFeatureOptions(LSPEditorFeatureDetector lspEditorFeatureDetector)
    {
        if (lspEditorFeatureDetector is null)
        {
            throw new ArgumentNullException(nameof(lspEditorFeatureDetector));
        }

        _lspEditorFeatureDetector = lspEditorFeatureDetector;

        _singleServerCompletionSupport = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)AsyncPackage.GetGlobalService(typeof(SVsFeatureFlags));
            var singleServerCompletionEnabled = featureFlags.IsFeatureEnabled(SingleServerCompletionFeatureFlag, defaultValue: false);
            return singleServerCompletionEnabled;
        });

        _singleServerSupport = new Lazy<bool>(() =>
        {
            var featureFlags = (IVsFeatureFlags)AsyncPackage.GetGlobalService(typeof(SVsFeatureFlags));
            var singleServerEnabled = featureFlags.IsFeatureEnabled(SingleServerFeatureFlag, defaultValue: false);
            return singleServerEnabled;
        });
    }

    // We don't currently support file creation operations on VS Codespaces or VS Liveshare
    public override bool SupportsFileManipulation => !IsCodespacesOrLiveshare;

    // In VS we override the project configuration file name because we don't want our serialized state to clash with other platforms (VSCode)
    public override string ProjectConfigurationFileName => "project.razor.vs.json";

    public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

    public override string HtmlVirtualDocumentSuffix => "__virtual.html";

    public override bool SingleServerCompletionSupport => _singleServerCompletionSupport.Value;

    public override bool SingleServerSupport => _singleServerSupport.Value;

    public override bool SupportsDelegatedCodeActions => true;

    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => false;

    private bool IsCodespacesOrLiveshare => _lspEditorFeatureDetector.IsRemoteClient() || _lspEditorFeatureDetector.IsLiveShareHost();
}
