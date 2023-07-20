// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Options;
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

    [LocCategory(nameof(VSPackage.Formatting))]
    [LocDescription(nameof(VSPackage.Setting_ColorBackgroundDescription))]
    [LocDisplayName(nameof(VSPackage.Setting_ColorBackgroundDisplayName))]
    public bool ColorBackground
    {
        get => _colorBackground ?? _optionsStorage.Value.ColorBackground;
        set => _colorBackground = value;
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

        if (_colorBackground is not null)
        {
            _optionsStorage.Value.ColorBackground = _colorBackground.Value;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _formatOnType = null;
        _autoClosingTags = null;
        _autoInsertAttributeQuotes = null;
        _colorBackground = null;
    }
}
