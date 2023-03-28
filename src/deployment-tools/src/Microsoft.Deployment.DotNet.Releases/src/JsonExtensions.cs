// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    internal static class JsonExtensions
    {
        /// <summary>
        /// Looks for a property named <paramref name="propertyName"/> and creates a <see cref="ReleaseVersion"/>
        /// using its value.
        /// </summary>
        /// <param name="value">The <see cref="JsonElement"/> to query.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>A <see cref="ReleaseVersion"/> or <see langword="null"/> if the property does not exist or contains a null value.</returns>
        internal static ReleaseVersion GetReleaseVersionOrDefault(this JsonElement value, string propertyName)
        {
            if (value.TryGetProperty(propertyName, out JsonElement p) && p.ValueKind != JsonValueKind.Null)
            {
                return new ReleaseVersion(p.GetString());
            }

            return null;
        }

        /// <summary>
        /// Looks for a property named <paramref name="propertyName"/> and creates a string
        /// using its value.
        /// </summary>
        /// <param name="value">The <see cref="JsonElement"/> to query.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>A string or <see langword="null"/> if the property does not exist or contains a null value.</returns>
        internal static string GetStringOrDefault(this JsonElement value, string propertyName)
        {
            if (value.TryGetProperty(propertyName, out JsonElement p) && p.ValueKind == JsonValueKind.String)
            {
                return p.GetString();
            }

            return null;
        }

        /// <summary>
        /// Looks for a property named <paramref name="propertyName"/> and creates a <see cref="Uri"/>
        /// using its value.
        /// </summary>
        /// <param name="value">The <see cref="JsonElement"/> to query.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>A <see cref="Uri"/> or <see langword="null"/> if the property does not exist or contains a null value.</returns>
        internal static Uri GetUriOrDefault(this JsonElement value, string propertyName)
        {
            if (value.TryGetProperty(propertyName, out JsonElement p) && p.ValueKind != JsonValueKind.Null)
            {
                return new Uri(p.GetString());
            }

            return null;
        }
    }
}
