// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "config", "ConfigCommandDesc", MaxArgs = 1,
            UsageSummaryResourceName = "ConfigCommandSummary", UsageExampleResourceName = "ConfigCommandExamples")]
    public class ConfigCommand : Command
    {
        private const string HttpPasswordKey = "http_proxy.password";

        [Option(typeof(NuGetCommand), "ConfigCommandSetDesc")]
        public Dictionary<string, string> Set { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [Option(typeof(NuGetCommand), "ConfigCommandAsPathDesc")]
        public bool AsPath
        {
            get;
            set;
        }

        public override void ExecuteCommand()
        {
            if (Settings == null)
            {
                throw new InvalidOperationException(LocalizedResourceManager.GetString("Error_SettingsIsNull"));
            }

            var getKey = Arguments.FirstOrDefault();
            if (Set.Any())
            {
                foreach (var property in Set)
                {
                    if (string.IsNullOrEmpty(property.Value))
                    {
                        SettingsUtility.DeleteConfigValue(Settings, property.Key);
                    }
                    else
                    {
                        // Hack: Need a nicer way for the user to say encrypt this.
                        var encrypt = HttpPasswordKey.Equals(property.Key, StringComparison.OrdinalIgnoreCase);
                        SettingsUtility.SetConfigValue(Settings, property.Key, property.Value, encrypt);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(getKey))
            {
                var value = SettingsUtility.GetConfigValue(Settings, getKey, isPath: AsPath);
                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteError(LocalizedResourceManager.GetString("ConfigCommandKeyNotFound"), getKey);
                }
                else
                {
                    Console.WriteLine(value);
                }
            }
        }
    }
}
