// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.Extensions.Logging;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Options;

[Export(typeof(OptionsStorage))]
[Export(typeof(IAdvancedSettingsStorage))]
internal class OptionsStorage : IAdvancedSettingsStorage
{
    private readonly WritableSettingsStore _writableSettingsStore;
    private readonly Lazy<ITelemetryReporter> _telemetryReporter;
    private readonly JoinableTask _initializeTask;
    private ISettingsReader? _unifiedSettingsReader;
    private IDisposable? _unifiedSettingsSubscription;

    public bool FormatOnType
    {
        get => GetBool(SettingsNames.FormatOnType.LegacyName, defaultValue: true);
        set => SetBool(SettingsNames.FormatOnType.LegacyName, value);
    }

    public bool AutoClosingTags
    {
        get => GetBool(SettingsNames.AutoClosingTags.LegacyName, defaultValue: true);
        set => SetBool(SettingsNames.AutoClosingTags.LegacyName, value);
    }

    public bool AutoInsertAttributeQuotes
    {
        get => GetBool(SettingsNames.AutoInsertAttributeQuotes.LegacyName, defaultValue: true);
        set => SetBool(SettingsNames.AutoInsertAttributeQuotes.LegacyName, value);
    }

    public bool ColorBackground
    {
        get => GetBool(SettingsNames.ColorBackground.LegacyName, defaultValue: false);
        set => SetBool(SettingsNames.ColorBackground.LegacyName, value);
    }

    public bool CommitElementsWithSpace
    {
        get => GetBool(SettingsNames.CommitElementsWithSpace.LegacyName, defaultValue: true);
        set => SetBool(SettingsNames.CommitElementsWithSpace.LegacyName, value);
    }

    public SnippetSetting Snippets
    {
        get => (SnippetSetting)GetInt(SettingsNames.Snippets.LegacyName, (int)SnippetSetting.All);
        set => SetInt(SettingsNames.Snippets.LegacyName, (int)value);
    }

    public LogLevel LogLevel
    {
        get => (LogLevel)GetInt(SettingsNames.LogLevel.LegacyName, (int)LogLevel.Warning);
        set => SetInt(SettingsNames.LogLevel.LegacyName, (int)value);
    }

    [ImportingConstructor]
    public OptionsStorage(
        SVsServiceProvider synchronousServiceProvider,
        [Import(typeof(SAsyncServiceProvider))] IAsyncServiceProvider serviceProvider,
        Lazy<ITelemetryReporter> telemetryReporter,
        JoinableTaskContext joinableTaskContext)
    {
        var shellSettingsManager = new ShellSettingsManager(synchronousServiceProvider);
        _writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

        _writableSettingsStore.CreateCollection(SettingsNames.LegacyCollection);
        _telemetryReporter = telemetryReporter;

        _initializeTask = joinableTaskContext.Factory.RunAsync(async () =>
        {
            var unifiedSettingsManager = await serviceProvider.GetServiceAsync<SVsUnifiedSettingsManager, Utilities.UnifiedSettings.ISettingsManager>();
            _unifiedSettingsReader = unifiedSettingsManager.GetReader();
            _unifiedSettingsSubscription = _unifiedSettingsReader.SubscribeToChanges(OnUnifiedSettingsChanged, SettingsNames.AllSettings.Select(s => s.UnifiedName).ToArray());
        });
    }

    public async Task OnChangedAsync(Action<ClientAdvancedSettings> changed)
    {
        await _initializeTask.JoinAsync();

        _changed += (_, args) => changed(args.Settings);
    }

    private EventHandler<ClientAdvancedSettingsChangedEventArgs>? _changed;

    public ClientAdvancedSettings GetAdvancedSettings() => new(FormatOnType, AutoClosingTags, AutoInsertAttributeQuotes, ColorBackground, CommitElementsWithSpace, Snippets, LogLevel);

    public bool GetBool(string name, bool defaultValue)
    {
        if (_writableSettingsStore.PropertyExists(SettingsNames.LegacyCollection, name))
        {
            return _writableSettingsStore.GetBoolean(SettingsNames.LegacyCollection, name);
        }

        return defaultValue;
    }

    public void SetBool(string name, bool value)
    {
        _writableSettingsStore.SetBoolean(SettingsNames.LegacyCollection, name, value);
        _telemetryReporter.Value.ReportEvent("OptionChanged", Severity.Normal, new Property(name, value));

        NotifyChange();
    }

    public int GetInt(string name, int defaultValue)
    {
        if (_writableSettingsStore.PropertyExists(SettingsNames.LegacyCollection, name))
        {
            return _writableSettingsStore.GetInt32(SettingsNames.LegacyCollection, name);
        }

        return defaultValue;
    }

    public void SetInt(string name, int value)
    {
        _writableSettingsStore.SetInt32(SettingsNames.LegacyCollection, name, value);
        _telemetryReporter.Value.ReportEvent("OptionChanged", Severity.Normal, new Property(name, value));

        NotifyChange();
    }

    private void NotifyChange()
    {
        _initializeTask.Join();
        _changed?.Invoke(this, new ClientAdvancedSettingsChangedEventArgs(GetAdvancedSettings()));
    }

    private void OnUnifiedSettingsChanged(SettingsUpdate update)
    {
        NotifyChange();
    }

    public void Dispose()
    {
        _unifiedSettingsSubscription?.Dispose();
    }
}
