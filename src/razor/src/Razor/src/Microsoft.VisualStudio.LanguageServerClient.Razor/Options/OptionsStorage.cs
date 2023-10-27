// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Options;

[Shared]
[Export(typeof(OptionsStorage))]
[Export(typeof(IAdvancedSettingsStorage))]
internal class OptionsStorage : IAdvancedSettingsStorage
{
    private readonly WritableSettingsStore _writableSettingsStore;
    private readonly ITelemetryReporter _telemetryReporter;

    private const string Collection = "Razor";
    private const string FormatOnTypeName = "FormatOnType";
    private const string AutoClosingTagsName = "AutoClosingTags";
    private const string AutoInsertAttributeQuotesName = "AutoInsertAttributeQuotes";
    private const string ColorBackgroundName = "ColorBackground";
    private const string CommitElementsWithSpaceName = "CommitElementsWithSpace";
    private const string SnippetsName = "Snippets";

    public bool FormatOnType
    {
        get => GetBool(FormatOnTypeName, defaultValue: true);
        set => SetBool(FormatOnTypeName, value);
    }

    public bool AutoClosingTags
    {
        get => GetBool(AutoClosingTagsName, defaultValue: true);
        set => SetBool(AutoClosingTagsName, value);
    }

    public bool AutoInsertAttributeQuotes
    {
        get => GetBool(AutoInsertAttributeQuotesName, defaultValue: true);
        set => SetBool(AutoInsertAttributeQuotesName, value);
    }

    public bool ColorBackground
    {
        get => GetBool(ColorBackgroundName, defaultValue: false);
        set => SetBool(ColorBackgroundName, value);
    }

    public bool CommitElementsWithSpace
    {
        get => GetBool(CommitElementsWithSpaceName, defaultValue: true);
        set => SetBool(CommitElementsWithSpaceName, value);
    }

    public SnippetSetting Snippets
    {
        get => (SnippetSetting)GetInt(SnippetsName, (int)SnippetSetting.All);
        set => SetInt(SnippetsName, (int)value);
    }

    [ImportingConstructor]
    public OptionsStorage(SVsServiceProvider vsServiceProvider, ITelemetryReporter telemetryReporter)
    {
        var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
        _writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

        _writableSettingsStore.CreateCollection(Collection);
        _telemetryReporter = telemetryReporter;
    }

    public event EventHandler<ClientAdvancedSettingsChangedEventArgs>? Changed;

    public ClientAdvancedSettings GetAdvancedSettings() => new(FormatOnType, AutoClosingTags, AutoInsertAttributeQuotes, ColorBackground, CommitElementsWithSpace, Snippets);

    public bool GetBool(string name, bool defaultValue)
    {
        if (_writableSettingsStore.PropertyExists(Collection, name))
        {
            return _writableSettingsStore.GetBoolean(Collection, name);
        }

        return defaultValue;
    }

    public void SetBool(string name, bool value)
    {
        _writableSettingsStore.SetBoolean(Collection, name, value);
        _telemetryReporter.ReportEvent("OptionChanged", Severity.Normal, new Property(name, value));

        NotifyChange();
    }

    public int GetInt(string name, int defaultValue)
    {
        if (_writableSettingsStore.PropertyExists(Collection, name))
        {
            return _writableSettingsStore.GetInt32(Collection, name);
        }

        return defaultValue;
    }

    public void SetInt(string name, int value)
    {
        _writableSettingsStore.SetInt32(Collection, name, value);
        _telemetryReporter.ReportEvent("OptionChanged", Severity.Normal, new Property(name, value));

        NotifyChange();
    }

    private void NotifyChange()
    {
        Changed?.Invoke(this, new ClientAdvancedSettingsChangedEventArgs(GetAdvancedSettings()));
    }
}
