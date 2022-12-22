// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace NuGet.CommandLine
{
    public static class ResourceHelper
    {
        private static Dictionary<Type, ResourceManager> _cachedManagers;

        public static string GetLocalizedString(Type resourceType, string resourceNames)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException(nameof(resourceType));
            }
            if (string.IsNullOrEmpty(resourceNames))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(resourceNames));
            }

            if (_cachedManagers == null)
            {
                _cachedManagers = new Dictionary<Type, ResourceManager>();
            }

            ResourceManager resourceManager;
            if (!_cachedManagers.TryGetValue(resourceType, out resourceManager))
            {
                PropertyInfo property = resourceType.GetProperty("ResourceManager", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

                if (property == null || property.GetGetMethod(nonPublic: true) == null)
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("ResourceTypeDoesNotHaveProperty"), resourceType, "ResourceManager"));
                }

                if (property.PropertyType != typeof(ResourceManager))
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("ResourcePropertyIncorrectType"), resourceNames, resourceType));
                }

                resourceManager = (ResourceManager)property.GetGetMethod(nonPublic: true)
                                                           .Invoke(obj: null, parameters: null);
            }

            var builder = new StringBuilder();
            foreach (var resource in resourceNames.Split(';'))
            {
                string value = LocalizedResourceManager.GetString(resource, resourceManager);
                if (String.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("ResourceTypeDoesNotHaveProperty"), resourceType, resource));
                }
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }
                builder.Append(value);
            }

            return builder.ToString();
        }
    }
}
