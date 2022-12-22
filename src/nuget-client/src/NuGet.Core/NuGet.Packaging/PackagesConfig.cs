// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Packaging
{
    public static class PackagesConfig
    {
        public static readonly string PackagesNodeName = "packages";
        public static readonly string PackageNodeName = "package";
        public static readonly string IdAttributeName = "id";
        public static readonly string VersionAttributeName = "version";
        public static readonly string TargetFrameworkAttributeName = "targetFramework";
        public static readonly string MinClientAttributeName = "minClientVersion";
        public static readonly string developmentDependencyAttributeName = "developmentDependency";
        public static readonly string allowedVersionsAttributeName = "allowedVersions";
        public static readonly string RequireInstallAttributeName = "requireReinstallation";
        public static readonly string UserInstalledAttributeName = "userInstalled";

        // Get an attribute that may or may not be present
        public static bool TryGetAttribute(XElement node, string name, out string value)
        {
            var attribute = node.Attributes(XName.Get(name)).FirstOrDefault();

            if (attribute != null
                && !string.IsNullOrEmpty(attribute.Value))
            {
                value = attribute.Value;
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Determine if the package node has the attribute value as the targetValue.
        /// </summary>
        public static bool HasAttributeValue(XElement node, string attributeName, string targetValue, out XElement element)
        {
            foreach (var package in node.Elements(XName.Get(PackageNodeName)))
            {
                string value;
                bool hasValue = TryGetAttribute(package, attributeName, out value);
                if (hasValue && string.Equals(targetValue, value, StringComparison.OrdinalIgnoreCase))
                {
                    element = package;
                    return true;
                }
            }

            element = null;
            return false;
        }

        /// <summary>
        /// Get a boolean attribute value, or false if it does not exist
        /// </summary>
        public static bool BoolAttribute(XElement node, string name, bool defaultValue = false)
        {
            string value = null;
            if (PackagesConfig.TryGetAttribute(node, name, out value))
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(value, "true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return defaultValue;
        }
    }
}
