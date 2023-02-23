// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.RazorExtension.Options;

[Export(typeof(OptionsStorage))]
[Export(typeof(IAdvancedSettingsStorage))]
[Shared]
internal class OptionsStorage : IAdvancedSettingsStorage
{
    private readonly WritableSettingsStore _writableSettingsStore;
    private readonly ILanguageServiceBroker2 _languageServiceBroker;
    private const string Collection = "Razor";

    [ImportingConstructor]
    public OptionsStorage(SVsServiceProvider vsServiceProvider, ILanguageServiceBroker2 languageServiceBroker)
    {
        var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
        _writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

        _writableSettingsStore.CreateCollection(Collection);
        _languageServiceBroker = languageServiceBroker;
    }

    public event EventHandler<ClientAdvancedSettingsChangedEventArgs>? Changed;
    public ClientAdvancedSettings GetAdvancedSettings() => new(FormatOnType);

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
        NotifyChange();
    }

    private void NotifyChange()
    {
        Changed?.Invoke(this, new ClientAdvancedSettingsChangedEventArgs(GetAdvancedSettings()));
    }

    private const string FormatOnTypeName = "FormatOnType";

    public bool FormatOnType
    {
        get => GetBool(FormatOnTypeName, defaultValue: true);
        set => SetBool(FormatOnTypeName, value);
    }
}
