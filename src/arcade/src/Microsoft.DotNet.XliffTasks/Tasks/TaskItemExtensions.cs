// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;

namespace XliffTasks.Tasks
{
    internal static class TaskItemExtensions
    {
        public static string GetMetadataOrThrow(this ITaskItem item, string key)
        {
            string value = item.GetMetadata(key);

            if (string.IsNullOrEmpty(value))
            {
                throw new BuildErrorException($"Item '{item}' is missing '{key}' metadata.");
            }

            return value;
        }

        public static string GetMetadataOrDefault(this ITaskItem item, string key, string defaultValue)
        {
            string value = item.GetMetadata(key);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}
