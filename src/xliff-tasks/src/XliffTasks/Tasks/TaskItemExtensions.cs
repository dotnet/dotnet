// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
