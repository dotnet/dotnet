// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal class ConfigurableLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    private readonly LanguageServerFeatureOptions _defaults = new DefaultLanguageServerFeatureOptions();

    private readonly bool? _supportsFileManipulation;
    private readonly string? _csharpVirtualDocumentSuffix;
    private readonly string? _htmlVirtualDocumentSuffix;
    private readonly bool? _singleServerSupport;
    private readonly bool? _delegateToCSharpOnDiagnosticPublish;
    private readonly bool? _returnCodeActionAndRenamePathsWithPrefixedSlash;
    private readonly bool? _showAllCSharpCodeActions;
    private readonly bool? _usePreciseSemanticTokenRanges;
    private readonly bool? _updateBuffersForClosedDocuments;
    private readonly bool? _includeProjectKeyInGeneratedFilePath;
    private readonly bool? _useRazorCohostServer;
    private readonly bool? _forceRuntimeCodeGeneration;
    private readonly bool? _useNewFormattingEngine;
    private readonly bool? _doNotInitializeMiscFilesProjectFromWorkspace;

    public override bool SupportsFileManipulation => _supportsFileManipulation ?? _defaults.SupportsFileManipulation;
    public override string CSharpVirtualDocumentSuffix => _csharpVirtualDocumentSuffix ?? DefaultLanguageServerFeatureOptions.DefaultCSharpVirtualDocumentSuffix;
    public override string HtmlVirtualDocumentSuffix => _htmlVirtualDocumentSuffix ?? DefaultLanguageServerFeatureOptions.DefaultHtmlVirtualDocumentSuffix;
    public override bool SingleServerSupport => _singleServerSupport ?? _defaults.SingleServerSupport;
    public override bool DelegateToCSharpOnDiagnosticPublish => _delegateToCSharpOnDiagnosticPublish ?? _defaults.DelegateToCSharpOnDiagnosticPublish;
    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => _returnCodeActionAndRenamePathsWithPrefixedSlash ?? _defaults.ReturnCodeActionAndRenamePathsWithPrefixedSlash;
    public override bool ShowAllCSharpCodeActions => _showAllCSharpCodeActions ?? _defaults.ShowAllCSharpCodeActions;
    public override bool UsePreciseSemanticTokenRanges => _usePreciseSemanticTokenRanges ?? _defaults.UsePreciseSemanticTokenRanges;
    public override bool UpdateBuffersForClosedDocuments => _updateBuffersForClosedDocuments ?? _defaults.UpdateBuffersForClosedDocuments;
    public override bool IncludeProjectKeyInGeneratedFilePath => _includeProjectKeyInGeneratedFilePath ?? _defaults.IncludeProjectKeyInGeneratedFilePath;
    public override bool UseRazorCohostServer => _useRazorCohostServer ?? _defaults.UseRazorCohostServer;
    public override bool ForceRuntimeCodeGeneration => _forceRuntimeCodeGeneration ?? _defaults.ForceRuntimeCodeGeneration;
    public override bool UseNewFormattingEngine => _useNewFormattingEngine ?? _defaults.UseNewFormattingEngine;
    public override bool SupportsSoftSelectionInCompletion => false;
    public override bool UseVsCodeCompletionTriggerCharacters => true;

    // Note: This option is defined in the negative because the default behavior should be to add documents to misc files project
    // when the language server is initialized. Adding the option at the command-line should disable that behavior.
    //
    // This is a temporary option and should be removed as part of https://github.com/dotnet/razor/issues/11594.
    public override bool DoNotInitializeMiscFilesProjectFromWorkspace => _doNotInitializeMiscFilesProjectFromWorkspace ?? _defaults.DoNotInitializeMiscFilesProjectFromWorkspace;

    public ConfigurableLanguageServerFeatureOptions(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] is not ['-', '-', .. var option])
            {
                continue;
            }

            TryProcessBoolOption(nameof(SupportsFileManipulation), ref _supportsFileManipulation, option, args, i);
            TryProcessStringOption(nameof(CSharpVirtualDocumentSuffix), ref _csharpVirtualDocumentSuffix, option, args, i);
            TryProcessStringOption(nameof(HtmlVirtualDocumentSuffix), ref _htmlVirtualDocumentSuffix, option, args, i);
            TryProcessBoolOption(nameof(SingleServerSupport), ref _singleServerSupport, option, args, i);
            TryProcessBoolOption(nameof(DelegateToCSharpOnDiagnosticPublish), ref _delegateToCSharpOnDiagnosticPublish, option, args, i);
            TryProcessBoolOption(nameof(ReturnCodeActionAndRenamePathsWithPrefixedSlash), ref _returnCodeActionAndRenamePathsWithPrefixedSlash, option, args, i);
            TryProcessBoolOption(nameof(ShowAllCSharpCodeActions), ref _showAllCSharpCodeActions, option, args, i);
            TryProcessBoolOption(nameof(UsePreciseSemanticTokenRanges), ref _usePreciseSemanticTokenRanges, option, args, i);
            TryProcessBoolOption(nameof(UpdateBuffersForClosedDocuments), ref _updateBuffersForClosedDocuments, option, args, i);
            TryProcessBoolOption(nameof(IncludeProjectKeyInGeneratedFilePath), ref _includeProjectKeyInGeneratedFilePath, option, args, i);
            TryProcessBoolOption(nameof(UseRazorCohostServer), ref _useRazorCohostServer, option, args, i);
            TryProcessBoolOption(nameof(ForceRuntimeCodeGeneration), ref _forceRuntimeCodeGeneration, option, args, i);
            TryProcessBoolOption(nameof(UseNewFormattingEngine), ref _useNewFormattingEngine, option, args, i);
            TryProcessBoolOption(nameof(DoNotInitializeMiscFilesProjectFromWorkspace), ref _doNotInitializeMiscFilesProjectFromWorkspace, option, args, i);
        }
    }

    private static void TryProcessStringOption(string optionName, ref string? field, string option, string[] args, int i)
    {
        // String properties must have at least one option following this one
        if (i >= args.Length - 1)
        {
            return;
        }

        if (string.Equals(option, optionName, StringComparison.OrdinalIgnoreCase))
        {
            field = args[++i];
        }
    }

    private static void TryProcessBoolOption(string optionName, ref bool? field, string option, string[] args, int i)
    {
        if (string.Equals(option, optionName, StringComparison.OrdinalIgnoreCase))
        {
            // bool properties are true if they're the last thing in the args, or the next thing is another option
            if (i >= args.Length - 1 || args[i + 1] is ['-', '-', ..])
            {
                field = true;
            }
            else
            {
                field = bool.Parse(args[++i]);
            }
        }
    }
}
