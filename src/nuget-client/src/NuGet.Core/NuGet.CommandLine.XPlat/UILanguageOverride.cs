// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    /// <summary>
    /// Copied from https://github.com/dotnet/sdk/blob/49d9b4148c4f65fd3f691186a4533375c3a83c97/src/Cli/dotnet/UILanguageOverride.cs#L9
    /// </summary>
    internal static class UILanguageOverride
    {
        private const string DOTNET_CLI_UI_LANGUAGE = nameof(DOTNET_CLI_UI_LANGUAGE);
        private const string VSLANG = nameof(VSLANG);
        private const string PreferredUILang = nameof(PreferredUILang);
        private static ILogger Logger;

        public static void Setup(ILogger logger, IEnvironmentVariableReader environmentVariableReader)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            Logger = logger;
            CultureInfo language = GetOverriddenUILanguage(environmentVariableReader);
            if (language != null)
            {
                ApplyOverrideToCurrentProcess(language);
                FlowOverrideToChildProcesses(language, environmentVariableReader);
            }
        }

        private static void ApplyOverrideToCurrentProcess(CultureInfo language)
        {
            CultureInfo.DefaultThreadCurrentUICulture = language;
        }

        private static void FlowOverrideToChildProcesses(CultureInfo language, IEnvironmentVariableReader environmentVariableReader)
        {
            // Do not override any environment variables that are already set as we do not want to clobber a more granular setting with our global setting.
            SetIfNotAlreadySet(DOTNET_CLI_UI_LANGUAGE, language.Name, environmentVariableReader);
            SetIfNotAlreadySet(VSLANG, language.LCID, environmentVariableReader); // for tools following VS guidelines to just work in CLI
            SetIfNotAlreadySet(PreferredUILang, language.Name, environmentVariableReader); // for C#/VB targets that pass $(PreferredUILang) to compiler
        }

        private static CultureInfo GetOverriddenUILanguage(IEnvironmentVariableReader environmentVariableReader)
        {
            // DOTNET_CLI_UI_LANGUAGE=<culture name> is the main way for users to customize the CLI's UI language.
            string dotnetCliLanguage = environmentVariableReader.GetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE);
            if (dotnetCliLanguage != null)
            {
                try
                {
                    return new CultureInfo(dotnetCliLanguage);
                }
                catch (CultureNotFoundException)
                {
                    Logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidCultureInfo, DOTNET_CLI_UI_LANGUAGE, dotnetCliLanguage));
                }
            }

            // VSLANG=<lcid> is set by VS and we respect that as well so that we will respect the VS 
            // language preference if we're invoked by VS. 
            string vsLang = environmentVariableReader.GetEnvironmentVariable(VSLANG);
            if (vsLang != null && int.TryParse(vsLang, out int vsLcid))
            {
                try
                {
                    return new CultureInfo(vsLcid);
                }
                catch (Exception e) when (e is CultureNotFoundException || e is ArgumentOutOfRangeException)
                {
                    Logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidCultureInfo, VSLANG, vsLang));
                }
            }

            return null;
        }

        private static void SetIfNotAlreadySet(string environmentVariableName, string value, IEnvironmentVariableReader environmentVariableReader)
        {
            string currentValue = environmentVariableReader.GetEnvironmentVariable(environmentVariableName);
            if (currentValue == null)
            {
#pragma warning disable RS0030 // Do not used banned APIs
                Environment.SetEnvironmentVariable(environmentVariableName, value);
#pragma warning restore RS0030 // Do not used banned APIs
            }
        }

        private static void SetIfNotAlreadySet(string environmentVariableName, int value, IEnvironmentVariableReader environmentVariableReader)
        {
            SetIfNotAlreadySet(environmentVariableName, value.ToString(), environmentVariableReader);
        }
    }
}
