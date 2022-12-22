// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Execution;

// Borrowed from dotnet/sdk with some tweaks to allow testing
internal class UiLanguageOverride
{
    private const string DOTNET_CLI_UI_LANGUAGE = nameof(DOTNET_CLI_UI_LANGUAGE);
    private const string VSLANG = nameof(VSLANG);
    private const string PreferredUILang = nameof(PreferredUILang);
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;
    private readonly Action<CultureInfo> _setDefaultThreadCurrentUICulture;

    public UiLanguageOverride()
        : this(new EnvironmentVariableHelper(), language => CultureInfo.DefaultThreadCurrentUICulture = language)
    { }

    public UiLanguageOverride(IEnvironmentVariableHelper environmentVariableHelper, Action<CultureInfo> setDefaultThreadCurrentUICulture)
    {
        _environmentVariableHelper = environmentVariableHelper;
        _setDefaultThreadCurrentUICulture = setDefaultThreadCurrentUICulture;
    }

    internal void SetCultureSpecifiedByUser()
    {
        var language = GetOverriddenUiLanguage(_environmentVariableHelper);
        if (language == null)
        {
            return;
        }

        ApplyOverrideToCurrentProcess(language, _setDefaultThreadCurrentUICulture);
        FlowOverrideToChildProcesses(language, _environmentVariableHelper);
    }

    private static void ApplyOverrideToCurrentProcess(CultureInfo language, Action<CultureInfo> setDefaultThreadCurrentUICulture)
    {
        setDefaultThreadCurrentUICulture(language);
    }

    private static CultureInfo? GetOverriddenUiLanguage(IEnvironmentVariableHelper environmentVariableHelper)
    {
        // DOTNET_CLI_UI_LANGUAGE=<culture name> is the main way for users to customize the CLI's UI language.
        string? dotnetCliLanguage = environmentVariableHelper.GetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE);
        if (dotnetCliLanguage != null)
        {
            try
            {
                return new CultureInfo(dotnetCliLanguage);
            }
            catch (CultureNotFoundException) { }
        }

        // VSLANG=<lcid> is set by VS and we respect that as well so that we will respect the VS
        // language preference if we're invoked by VS.
        string? vsLang = environmentVariableHelper.GetEnvironmentVariable(VSLANG);
        if (vsLang != null && int.TryParse(vsLang, out int vsLcid))
        {
            try
            {
                return new CultureInfo(vsLcid);
            }
            catch (ArgumentOutOfRangeException) { }
            catch (CultureNotFoundException) { }
        }

        return null;
    }

    private static void FlowOverrideToChildProcesses(CultureInfo language, IEnvironmentVariableHelper environmentVariableHelper)
    {
        // Do not override any environment variables that are already set as we do not want to clobber a more granular setting with our global setting.
        SetIfNotAlreadySet(DOTNET_CLI_UI_LANGUAGE, language.Name, environmentVariableHelper);
        SetIfNotAlreadySet(VSLANG, language.LCID.ToString(CultureInfo.CurrentCulture), environmentVariableHelper); // for tools following VS guidelines to just work in CLI
        SetIfNotAlreadySet(PreferredUILang, language.Name, environmentVariableHelper); // for C#/VB targets that pass $(PreferredUILang) to compiler
    }

    private static void SetIfNotAlreadySet(string environmentVariableName, string value, IEnvironmentVariableHelper environmentVariableHelper)
    {
        string? currentValue = environmentVariableHelper.GetEnvironmentVariable(environmentVariableName);
        if (currentValue == null)
        {
            environmentVariableHelper.SetEnvironmentVariable(environmentVariableName, value);
        }
    }
}
