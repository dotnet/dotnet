// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    /// <summary>
    /// Helper class for reading dependency information from Version.Details.xml files.
    /// </summary>
    internal static class VersionDetailsHelper
    {
        private const string PinnedAttributeName = "Pinned";
        private const string DependencyAttributeName = "Dependency";
        private const string NameAttributeName = "Name";

        /// <summary>
        /// Retrieve the set of non-pinned dependencies from a Version.Details.xml file.
        /// </summary>
        /// <param name="versionDetailsPath">Path to the Version.Details.xml file.</param>
        /// <param name="log">TaskLoggingHelper for logging errors.</param>
        /// <returns>Hash set of dependency names, or null if an error occurred.</returns>
        public static HashSet<string> GetDependencies(string versionDetailsPath, TaskLoggingHelper log)
        {
            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(versionDetailsPath);
            }
            catch (Exception e)
            {
                log.LogErrorFromException(e);
                return null;
            }

            HashSet<string> dependencyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Load the nodes and filter those that are pinned
            XmlNodeList dependencyNodes = document.DocumentElement.SelectNodes($"//{DependencyAttributeName}");

            foreach (XmlNode dependency in dependencyNodes)
            {
                if (dependency.NodeType == XmlNodeType.Comment || dependency.NodeType == XmlNodeType.Whitespace)
                {
                    continue;
                }

                bool isPinned = false;
                XmlAttribute pinnedAttribute = dependency.Attributes[PinnedAttributeName];
                if (pinnedAttribute != null && !bool.TryParse(pinnedAttribute.Value, out isPinned))
                {
                    log.LogError($"The '{PinnedAttributeName}' attribute is set but the value " +
                        $"'{pinnedAttribute.Value}' is not a valid boolean.");
                    return null;
                }

                if (isPinned)
                {
                    continue;
                }

                var name = dependency.Attributes[NameAttributeName]?.Value?.Trim();

                if (string.IsNullOrEmpty(name))
                {
                    log.LogError($"The '{NameAttributeName}' attribute must be specified.");
                    return null;
                }

                dependencyNames.Add(name);
            }

            return dependencyNames;
        }
    }
}
