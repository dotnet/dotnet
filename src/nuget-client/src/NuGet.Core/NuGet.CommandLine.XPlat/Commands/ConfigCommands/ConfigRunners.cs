// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.CommandLine.XPlat
{
    internal static class ConfigPathsRunner
    {
        public static void Run(ConfigPathsArgs args, Func<ILogger> getLogger)
        {
            RunnerHelper.EnsureArgumentsNotNull(args, getLogger);

            if (string.IsNullOrEmpty(args.WorkingDirectory))
            {
                args.WorkingDirectory = Directory.GetCurrentDirectory();
            }

            var settings = RunnerHelper.GetSettingsFromDirectory(args.WorkingDirectory);
            ILogger logger = getLogger();

            var filePaths = settings.GetConfigFilePaths();
            foreach (var filePath in filePaths)
            {
                logger.LogMinimal(filePath);
            }
        }
    }

    internal static class ConfigGetRunner
    {
        public static void Run(ConfigGetArgs args, Func<ILogger> getLogger)
        {
            RunnerHelper.EnsureArgumentsNotNull(args, getLogger);

            if (args.AllOrConfigKey == null)
            {
                throw new CommandException(string.Format(CultureInfo.CurrentCulture, Strings.ConfigCommandKeyNotFound, args.AllOrConfigKey));
            }

            var settings = RunnerHelper.GetSettingsFromDirectory(args.WorkingDirectory) as Settings;
            if (settings == null)
            {
                return;
            }
            ILogger logger = getLogger();

            if (args.AllOrConfigKey.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<string> sections = settings.GetAllSettingSections();
                if (sections == null)
                {
                    return;
                }
                RunnerHelper.LogSections(sections, settings, logger, args.ShowPath);
            }
            else
            {
                var configValue = RunnerHelper.GetValueForConfigKey(settings, args.AllOrConfigKey, args.ShowPath);
                if (string.IsNullOrEmpty(configValue))
                {
                    throw new CommandException(string.Format(CultureInfo.CurrentCulture, Strings.ConfigCommandKeyNotFound, args.AllOrConfigKey));
                }

                logger.LogMinimal(configValue);
            }
        }
    }

    internal static class ConfigSetRunner
    {
        public static void Run(ConfigSetArgs args, Func<ILogger> getLogger)
        {
            RunnerHelper.EnsureArgumentsNotNull(args, getLogger);
            RunnerHelper.ValidateConfigKey(args.ConfigKey);
            ISettings settings = XPlatUtility.ProcessConfigFile(args.ConfigFile);

            bool encrypt = args.ConfigKey.Equals(ConfigurationConstants.PasswordKey, StringComparison.OrdinalIgnoreCase);
            SettingsUtility.SetConfigValue(settings, args.ConfigKey, args.ConfigValue, encrypt);
        }
    }

    internal static class ConfigUnsetRunner
    {
        public static void Run(ConfigUnsetArgs args, Func<ILogger> getLogger)
        {
            RunnerHelper.EnsureArgumentsNotNull(args, getLogger);
            RunnerHelper.ValidateConfigKey(args.ConfigKey);
            ISettings settings = XPlatUtility.ProcessConfigFile(args.ConfigFile);

            if (!SettingsUtility.DeleteConfigValue(settings, args.ConfigKey))
            {
                getLogger().LogMinimal(string.Format(CultureInfo.CurrentCulture, Strings.ConfigUnsetNonExistingKey, args.ConfigKey));
            }
        }
    }

    internal static class RunnerHelper
    {
        /// <summary>
        /// Creates a settings object using the directory argument,
        /// or using the current directory if no argument is passed.
        /// </summary>
        /// <exception cref="CommandException"></exception>
        public static ISettings GetSettingsFromDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(directory))
            {
                throw new CommandException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PathNotFound, directory));
            }

            return NuGet.Configuration.Settings.LoadDefaultSettings(
                directory,
                configFileName: null,
                machineWideSettings: new XPlatMachineWideSetting());
        }

        /// <summary>
        /// Returns a string holding the value of the key in the config section
        /// of the settings. If showPath is true, this will also return the path
        /// to the configuration file where the key is located.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetValueForConfigKey(ISettings settings, string key, bool showPath)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            SettingSection sectionElement = settings.GetSection(ConfigurationConstants.Config);
            AddItem item = sectionElement?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, key);

            if (item == null)
            {
                return null;
            }

            if (showPath)
            {
                return item.Value + "\tfile: " + item.ConfigPath;
            }
            return item.Value;
        }

        /// <summary>
        /// Logs each section of the configuration settings that will be applied.
        /// If showPath is true, these settings will be grouped by file path.
        /// </summary>
        public static void LogSections(IEnumerable<string> sections, Settings settings, ILogger logger, bool showPath)
        {
            foreach (string section in sections)
            {
                logger.LogMinimal(section + ":");
                IReadOnlyCollection<SettingItem> items = settings.GetSection(section)?.Items;

                IEnumerable<IGrouping<string, SettingItem>> groupByConfigPathsQuery =
                    from item in items
                    group item by item.ConfigPath into newItemGroup
                    select newItemGroup;

                var groupByConfigPathsQueryReverse = groupByConfigPathsQuery.Reverse();

                if (showPath)
                {
                    foreach (IGrouping<string, SettingItem> configPathsGroup in groupByConfigPathsQueryReverse)
                    {
                        logger.LogMinimal($" file: {configPathsGroup.Key}");
                        LogSectionItems(configPathsGroup, logger);
                        logger.LogMinimal(Environment.NewLine);
                    }
                }
                else
                {
                    foreach (IGrouping<string, SettingItem> configPathsGroup in groupByConfigPathsQueryReverse)
                    {
                        LogSectionItems(configPathsGroup, logger);
                    }
                    logger.LogMinimal(Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Combines the attributes from each item in a collection of SettingItems into a string, then logs the string.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="logger"></param>
        public static void LogSectionItems(IEnumerable<SettingItem> items, ILogger logger)
        {
            foreach (SettingItem item in items)
            {
                string setting = $"\t{item.ElementName}";
                IReadOnlyDictionary<string, string> attributes = item.GetAttributes();
                if (attributes != null)
                {
                    foreach (KeyValuePair<string, string> attribute in attributes)
                    {
                        setting += $" {attribute.Key}=\"{attribute.Value}\"";
                    }
                }

                logger.LogMinimal(setting);
            }
        }

        /// <summary>
        /// Throws an exception if any of the config runner arguments are null.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void EnsureArgumentsNotNull<TArgs>(TArgs args, Func<ILogger> logger)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));
            _ = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Throws an exception if the value passed in is not a valid config key.
        /// </summary>
        /// <exception cref="CommandException"></exception>
        public static void ValidateConfigKey(string configKey)
        {
            bool isValidKey = false;
            foreach (string key in ConfigurationConstants.GetConfigKeys())
            {
                if (key.Equals(configKey, StringComparison.OrdinalIgnoreCase))
                {
                    isValidKey = true;
                    break;
                }
            }

            if (!isValidKey)
            {
                throw new CommandException(string.Format(CultureInfo.CurrentCulture, Strings.Error_ConfigSetInvalidKey, configKey));
            }
        }
    }
}
