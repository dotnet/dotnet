// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Commands;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents a base class for reading properties from MSBuild objects.
    /// </summary>
    internal abstract class MSBuildItemBase : IMSBuildItem
    {
        /// <inheritdoc cref="IMSBuildItem.Identity" />
        public virtual string Identity { get; }

        /// <inheritdoc cref="IMSBuildItem.Properties" />
        public virtual IReadOnlyList<string> Properties { get; }

        /// <inheritdoc cref="IMSBuildItem.GetProperty(string)" />
        public string GetProperty(string property)
        {
            return GetProperty(property, trim: true);
        }

        /// <inheritdoc cref="IMSBuildItem.GetProperty(string, bool)" />
        public string GetProperty(string property, bool trim)
        {
            string value = GetPropertyValue(property);

            if (trim)
            {
                value = value.Trim();
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="name">The name of the property to get the value of.</param>
        /// <returns>The value of the property if one is defined, otherwise <see cref="string.Empty" />.</returns>
        protected abstract string GetPropertyValue(string name);
    }
}
