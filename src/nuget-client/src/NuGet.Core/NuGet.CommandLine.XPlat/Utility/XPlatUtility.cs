// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

#if IS_CORECLR
using System.Runtime.InteropServices;
#endif

using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal static class XPlatUtility
    {
        public const string HelpOption = "-h|--help";

        /// <summary>
        /// Note that the .NET CLI itself has parameter parsing which limits the values that will be passed here by the
        /// user. In other words, the default case should only be hit with <c>m</c> or <c>minimal</c> but we use <see cref="Common.LogLevel.Minimal"/>
        /// as the default case to avoid errors.
        /// </summary>
        public static LogLevel MSBuildVerbosityToNuGetLogLevel(string verbosity)
        {
            switch (verbosity?.ToUpperInvariant())
            {
                case "Q":
                case "QUIET":
                    return LogLevel.Warning;
                case "N":
                case "NORMAL":
                    return LogLevel.Information;
                case "D":
                case "DETAILED":
                case "DIAG":
                case "DIAGNOSTIC":
                    return LogLevel.Debug;
                default:
                    return LogLevel.Minimal;
            }
        }

        public static ISettings GetSettingsForCurrentWorkingDirectory()
        {
            return Settings.LoadDefaultSettings(
                Directory.GetCurrentDirectory(),
                configFileName: null,
                machineWideSettings: new XPlatMachineWideSetting());
        }

        public static void ConfigureProtocol()
        {
            // Set connection limit
            NetworkProtocolUtility.SetConnectionLimit();

            // Set user agent string used for network calls
            SetUserAgent();
        }

        public static void SetUserAgent()
        {
#if IS_CORECLR
            UserAgent.SetUserAgentString(new UserAgentStringBuilder("NuGet xplat")
                .WithOSDescription(RuntimeInformation.OSDescription));
#else
            UserAgent.SetUserAgentString(new UserAgentStringBuilder("NuGet xplat"));
#endif
        }

        internal static ISettings ProcessConfigFile(string configFile)
        {
            if (string.IsNullOrEmpty(configFile))
            {
                return GetSettingsForCurrentWorkingDirectory();
            }

            var configFileFullPath = Path.GetFullPath(configFile);
            var directory = Path.GetDirectoryName(configFileFullPath);
            var configFileName = Path.GetFileName(configFileFullPath);
            return Settings.LoadDefaultSettings(
                directory,
                configFileName,
                machineWideSettings: new XPlatMachineWideSetting());
        }
    }
}
