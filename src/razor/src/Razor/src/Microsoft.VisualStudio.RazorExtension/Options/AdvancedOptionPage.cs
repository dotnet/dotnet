// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Razor.LanguageClient.Options;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.RazorExtension.Options;

[Guid("8EBB7F64-5BF7-49E6-9023-7CD7B9912203")]
[ComVisible(true)]
internal class AdvancedOptionPage : DialogPage
{
    private readonly Lazy<OptionsStorage> _optionsStorage;

    private bool? _formatOnType;
    private bool? _autoClosingTags;
    private bool? _autoInsertAttributeQuotes;
    private bool? _colorBackground;
    private bool? _codeBlockBraceOnNextLine;
    private bool? _commitElementsWithSpace;
    private SnippetSetting? _snippets;
    private LogLevel? _logLevel;
    private bool? _formatOnPaste;

    public AdvancedOptionPage()
    {
        _optionsStorage = new Lazy<OptionsStorage>(() =>
        {
            var componentModel = (IComponentModel)Site.GetService(typeof(SComponentModel));
            Assumes.Present(componentModel);

            return componentModel.DefaultExportProvider.GetExportedValue<OptionsStorage>();
        });
    }

    [LocCategory(nameof(VSPackage.Formatting))]
    [LocDescription(nameof(VSPackage.Setting_FormattingOnTypeDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_FormattingOnTypeDisplayName))]
    public bool FormatOnType
    {
        get => _formatOnType ?? _optionsStorage.Value.FormatOnType;
        set => _formatOnType = value;
    }

    [LocCategory(nameof(VSPackage.Typing))]
    [LocDescription(nameof(VSPackage.Setting_AutoClosingTagsDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_AutoClosingTagsDisplayName))]
    public bool AutoClosingTags
    {
        get => _autoClosingTags ?? _optionsStorage.Value.AutoClosingTags;
        set => _autoClosingTags = value;
    }

    [LocCategory(nameof(VSPackage.Completion))]
    [LocDescription(nameof(VSPackage.Setting_AutoInsertAttributeQuotesDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_AutoInsertAttributeQuotesDisplayName))]
    public bool AutoInsertAttributeQuotes
    {
        get => _autoInsertAttributeQuotes ?? _optionsStorage.Value.AutoInsertAttributeQuotes;
        set => _autoInsertAttributeQuotes = value;
    }

    [LocCategory(nameof(VSPackage.Completion))]
    [LocDescription(nameof(VSPackage.Setting_CommitElementsWithSpaceDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_CommitElementsWithSpaceDisplayName))]
    public bool CommitElementsWithSpace
    {
        get => _commitElementsWithSpace ?? _optionsStorage.Value.CommitElementsWithSpace;
        set => _commitElementsWithSpace = value;
    }

    [LocCategory(nameof(VSPackage.Formatting))]
    [LocDescription(nameof(VSPackage.Setting_ColorBackgroundDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_ColorBackgroundDisplayName))]
    public bool ColorBackground
    {
        get => _colorBackground ?? _optionsStorage.Value.ColorBackground;
        set => _colorBackground = value;
    }

    [LocCategory(nameof(VSPackage.Formatting))]
    [LocDescription(nameof(VSPackage.Setting_CodeBlockBraceOnNextLineDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_CodeBlockBraceOnNextLineDisplayName))]
    public bool CodeBlockBraceOnNextLine
    {
        get => _codeBlockBraceOnNextLine ?? _optionsStorage.Value.CodeBlockBraceOnNextLine;
        set => _codeBlockBraceOnNextLine = value;
    }

    [LocCategory(nameof(VSPackage.Formatting))]
    [LocDescription(nameof(VSPackage.Setting_FormattingOnPasteDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_FormattingOnPasteDisplayName))]
    public bool FormatOnPaste
    {
        get => _formatOnPaste ?? _optionsStorage.Value.FormatOnPaste;
        set => _formatOnPaste = value;
    }

    [LocCategory(nameof(VSPackage.Completion))]
    [LocDescription(nameof(VSPackage.Setting_SnippetsDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_SnippetsDisplayName))]
    public SnippetSetting Snippets
    {
        get => _snippets ?? _optionsStorage.Value.Snippets;
        set => _snippets = value;
    }

    [LocCategory(nameof(VSPackage.Other))]
    [LocDescription(nameof(VSPackage.Setting_LogLevelDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_LogLevelDisplayName))]
    public LogLevel LogLevel
    {
        get => _logLevel ?? _optionsStorage.Value.LogLevel;
        set => _logLevel = value;
    }

    protected override void OnApply(PageApplyEventArgs e)
    {
        if (_formatOnType is not null)
        {
            _optionsStorage.Value.FormatOnType = _formatOnType.Value;
        }

        if (_autoClosingTags is not null)
        {
            _optionsStorage.Value.AutoClosingTags = _autoClosingTags.Value;
        }

        if (_autoInsertAttributeQuotes is not null)
        {
            _optionsStorage.Value.AutoInsertAttributeQuotes = _autoInsertAttributeQuotes.Value;
        }

        if (_commitElementsWithSpace is not null)
        {
            _optionsStorage.Value.CommitElementsWithSpace = _commitElementsWithSpace.Value;
        }

        if (_colorBackground is not null)
        {
            _optionsStorage.Value.ColorBackground = _colorBackground.Value;
        }

        if (_codeBlockBraceOnNextLine is not null)
        {
            _optionsStorage.Value.CodeBlockBraceOnNextLine = _codeBlockBraceOnNextLine.Value;
        }

        if (_formatOnPaste is not null)
        {
            _optionsStorage.Value.FormatOnPaste = _formatOnPaste.Value;
        }

        if (_snippets is SnippetSetting snippets)
        {
            _optionsStorage.Value.Snippets = snippets;
        }

        if (_logLevel is LogLevel logLevel)
        {
            _optionsStorage.Value.LogLevel = logLevel;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _formatOnType = null;
        _autoClosingTags = null;
        _autoInsertAttributeQuotes = null;
        _commitElementsWithSpace = null;
        _colorBackground = null;
        _codeBlockBraceOnNextLine = null;
        _formatOnPaste = null;
        _snippets = null;
        _logLevel = null;
    }
}
