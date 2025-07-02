// <copyright file="Tags.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Base class for tags backed context.
    /// </summary>
    internal static class Tags
    {
        internal static void SetStringValueOrRemove(this IDictionary<string, string> tags, string tagKey, string tagValue)
        {
            SetTagValueOrRemove(tags, tagKey, tagValue);
        }

        internal static void SetTagValueOrRemove<T>(this IDictionary<string, string> tags, string tagKey, T tagValue)
        {
            SetTagValueOrRemove(tags, tagKey, Convert.ToString(tagValue, CultureInfo.InvariantCulture));
        }

        internal static void CopyTagValue(bool? sourceValue, ref bool? targetValue)
        {
            if (sourceValue.HasValue && !targetValue.HasValue)
            {
                targetValue = sourceValue;
            }
        }

        internal static string GetTagValueOrNull(this IDictionary<string, string> tags, string tagKey)
        {
            string tagValue;
            if (tags.TryGetValue(tagKey, out tagValue))
            {
                return tagValue;
            }

            return null;
        }

        internal static void UpdateTagValue(this IDictionary<string, string> tags, string tagKey, string tagValue)
        {
            if (!string.IsNullOrEmpty(tagValue))
            {
                int limit;
                if (Property.TagSizeLimits.TryGetValue(tagKey, out limit) && tagValue.Length > limit)
                {
                    tagValue = Property.TrimAndTruncate(tagValue, limit);
                }

                tags.Add(tagKey, tagValue);
            }
        }

        internal static void CopyTagValue(string sourceValue, ref string targetValue)
        {
            if (!string.IsNullOrEmpty(sourceValue) && string.IsNullOrEmpty(targetValue))
            {
                targetValue = sourceValue;
            }
        }

        internal static void UpdateTagValue(this IDictionary<string, string> tags, string tagKey, bool? tagValue)
        {
            if (tagValue.HasValue)
            {
                string value = tagValue.Value.ToString(CultureInfo.InvariantCulture);
                tags.Add(tagKey, value);
            }
        }

        private static void SetTagValueOrRemove(this IDictionary<string, string> tags, string tagKey, string tagValue)
        {
            if (string.IsNullOrEmpty(tagValue))
            {
                tags.Remove(tagKey);
            }
            else
            {
                tags[tagKey] = tagValue;
            }
        }
    }
}
