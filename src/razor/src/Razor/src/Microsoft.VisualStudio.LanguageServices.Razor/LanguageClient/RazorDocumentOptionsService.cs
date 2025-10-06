// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using FormattingOptions = Microsoft.CodeAnalysis.Formatting.FormattingOptions;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(IRazorDocumentOptionsService))]
internal sealed class RazorDocumentOptionsService : IRazorDocumentOptionsService
{
    private readonly IClientSettingsManager _editorSettingsManager;

    [ImportingConstructor]
    public RazorDocumentOptionsService(IClientSettingsManager editorSettingsManager)
    {
        if (editorSettingsManager is null)
        {
            throw new ArgumentNullException(nameof(editorSettingsManager));
        }

        _editorSettingsManager = editorSettingsManager;
    }

    public Task<IRazorDocumentOptions> GetOptionsForDocumentAsync(Document document, CancellationToken cancellationToken)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        // TO-DO: We should switch to a per-document implementation once Razor starts supporting .editorconfig.
        var editorSettings = _editorSettingsManager.GetClientSettings().ClientSpaceSettings;
        return Task.FromResult<IRazorDocumentOptions>(new RazorDocumentOptions(document, editorSettings));
    }

    private sealed class RazorDocumentOptions : IRazorDocumentOptions
    {
        private readonly ClientSpaceSettings _editorSettings;
        private readonly OptionKey _useTabsOptionKey;
        private readonly OptionKey _tabSizeOptionKey;
        private readonly OptionKey _indentationSizeOptionKey;

        public RazorDocumentOptions(Document document, ClientSpaceSettings editorSettings)
        {
            _editorSettings = editorSettings;

            _useTabsOptionKey = new OptionKey(FormattingOptions.UseTabs, document.Project.Language);
            _tabSizeOptionKey = new OptionKey(FormattingOptions.TabSize, document.Project.Language);
            _indentationSizeOptionKey = new OptionKey(FormattingOptions.IndentationSize, document.Project.Language);
        }

        public bool TryGetDocumentOption(OptionKey option, [NotNullWhen(true)] out object? value)
        {
            if (option == _useTabsOptionKey)
            {
                value = _editorSettings.IndentWithTabs;
                return true;
            }
            else if (option == _tabSizeOptionKey || option == _indentationSizeOptionKey)
            {
                value = _editorSettings.IndentSize;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
