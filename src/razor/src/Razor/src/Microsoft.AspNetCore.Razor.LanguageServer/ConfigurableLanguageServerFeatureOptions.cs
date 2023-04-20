// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class ConfigurableLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    private readonly LanguageServerFeatureOptions _defaults = new DefaultLanguageServerFeatureOptions();

    private readonly bool? _supportsFileManipulation;
    private readonly string? _projectConfigurationFileName;
    private readonly string? _csharpVirtualDocumentSuffix;
    private readonly string? _htmlVirtualDocumentSuffix;
    private readonly bool? _singleServerCompletionSupport;
    private readonly bool? _singleServerSupport;
    private readonly bool? _supportsDelegatedCodeActions;
    private readonly bool? _returnCodeActionAndRenamePathsWithPrefixedSlash;
    private readonly bool? _showAllCSharpCodeActions;

    public override bool SupportsFileManipulation => _supportsFileManipulation ?? _defaults.SupportsFileManipulation;
    public override string ProjectConfigurationFileName => _projectConfigurationFileName ?? _defaults.ProjectConfigurationFileName;
    public override string CSharpVirtualDocumentSuffix => _csharpVirtualDocumentSuffix ?? _defaults.CSharpVirtualDocumentSuffix;
    public override string HtmlVirtualDocumentSuffix => _htmlVirtualDocumentSuffix ?? _defaults.HtmlVirtualDocumentSuffix;
    public override bool SingleServerCompletionSupport => _singleServerCompletionSupport ?? _defaults.SingleServerCompletionSupport;
    public override bool SingleServerSupport => _singleServerSupport ?? _defaults.SingleServerSupport;
    public override bool SupportsDelegatedCodeActions => _supportsDelegatedCodeActions ?? _defaults.SupportsDelegatedCodeActions;
    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash => _returnCodeActionAndRenamePathsWithPrefixedSlash ?? _defaults.ReturnCodeActionAndRenamePathsWithPrefixedSlash;
    public override bool ShowAllCSharpCodeActions => _showAllCSharpCodeActions ?? _defaults.ShowAllCSharpCodeActions;

    public ConfigurableLanguageServerFeatureOptions(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] is not ['-', '-', .. var option])
            {
                continue;
            }

            TryProcessBoolOption(nameof(SupportsFileManipulation), ref _supportsFileManipulation, option, args, i);
            TryProcessStringOption(nameof(ProjectConfigurationFileName), ref _projectConfigurationFileName, option, args, i);
            TryProcessStringOption(nameof(CSharpVirtualDocumentSuffix), ref _csharpVirtualDocumentSuffix, option, args, i);
            TryProcessStringOption(nameof(HtmlVirtualDocumentSuffix), ref _htmlVirtualDocumentSuffix, option, args, i);
            TryProcessBoolOption(nameof(SingleServerCompletionSupport), ref _singleServerCompletionSupport, option, args, i);
            TryProcessBoolOption(nameof(SingleServerSupport), ref _singleServerSupport, option, args, i);
            TryProcessBoolOption(nameof(SupportsDelegatedCodeActions), ref _supportsDelegatedCodeActions, option, args, i);
            TryProcessBoolOption(nameof(ReturnCodeActionAndRenamePathsWithPrefixedSlash), ref _returnCodeActionAndRenamePathsWithPrefixedSlash, option, args, i);
            TryProcessBoolOption(nameof(ShowAllCSharpCodeActions), ref _showAllCSharpCodeActions, option, args, i);
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
