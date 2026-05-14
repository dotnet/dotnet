// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Templating
{
    internal static class MSBuildListSplitter
    {
        public static IDictionary<string, string> GetNamedProperties(string[] input, TaskLoggingHelper log)
        {
            Dictionary<string, string> values = new(StringComparer.OrdinalIgnoreCase);
            if (input == null)
            {
                return values;
            }

            foreach (string item in input)
            {
                int splitIdx = item.IndexOf('=');
                if (splitIdx < 0)
                {
                    log.LogWarning($"Property: {item} does not have a valid '=' separator");
                    continue;
                }

                string key = item.Substring(0, splitIdx).Trim();
                if (string.IsNullOrEmpty(key))
                {
                    log.LogWarning($"Property: {item} does not have a valid property name");
                    continue;
                }

                string value = item.Substring(splitIdx + 1);
                values[key] = value;
            }

            return values;
        }
    }
}
