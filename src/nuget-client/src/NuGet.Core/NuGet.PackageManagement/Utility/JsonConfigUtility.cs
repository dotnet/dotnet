// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// project.json utils
    /// </summary>
    public static class JsonConfigUtility
    {
        private const string DEPENDENCIES_TAG = "dependencies";
        private const string FRAMEWORKS_TAG = "frameworks";

        /// <summary>
        /// Read dependencies from a project.json file
        /// </summary>
        public static IEnumerable<PackageDependency> GetDependencies(JObject json)
        {
            JToken node = null;
            if (json.TryGetValue(DEPENDENCIES_TAG, out node))
            {
                foreach (var dependency in node)
                {
                    yield return ParseDependency(dependency);
                }
            }
        }

        /// <summary>
        /// Convert a dependency entry into an id and version range
        /// </summary>
        public static PackageDependency ParseDependency(JToken dependencyToken)
        {
            var property = dependencyToken as JProperty;

            var id = property.Name;

            string version = null;
            if (property.Value.Type == JTokenType.String)
            {
                version = (string)property.Value;
            }
            else if (property.Value.Type == JTokenType.Object)
            {
                version = (string)property.Value["version"];
            }

            if (string.IsNullOrEmpty(version))
            {
                throw new FormatException(
                    string.Format(CultureInfo.CurrentCulture, Strings.DependencyDoesNotHaveValidVersion, dependencyToken.ToString()));
            }

            var range = VersionRange.Parse(version);
            return new PackageDependency(id, range);
        }

        /// <summary>
        /// Add a dependency to a project.json file
        /// </summary>
        public static void AddDependency(JObject json, PackageIdentity package)
        {
            var range = VersionRange.All;

            if (package.Version != null)
            {
                range = new VersionRange(
                    minVersion: package.Version,
                    includeMinVersion: true,
                    maxVersion: null,
                    includeMaxVersion: false);
            }

            var dependency = new PackageDependency(package.Id, range);

            AddDependency(json, dependency);
        }

        /// <summary>
        /// Add a dependency to a project.json file
        /// </summary>
        public static void AddDependency(JObject json, PackageDependency dependency)
        {
            // Removing the older package if it exists
            RemoveDependency(json, dependency.Id);

            JObject dependencySet = null;

            JToken node = null;
            if (json.TryGetValue(DEPENDENCIES_TAG, out node))
            {
                dependencySet = node as JObject;
            }

            if (dependencySet == null)
            {
                dependencySet = new JObject();
            }

            var packageProperty = new JProperty(dependency.Id, dependency.VersionRange.MinVersion.ToNormalizedString());
            dependencySet.Add(packageProperty);

            // order dependencies to reduce merge conflicts
            dependencySet = SortProperties(dependencySet);

            json[DEPENDENCIES_TAG] = dependencySet;
        }

        /// <summary>
        /// Remove a dependency from a project.json file
        /// </summary>
        public static void RemoveDependency(JObject json, string packageId)
        {
            JToken node = null;
            if (json.TryGetValue(DEPENDENCIES_TAG, out node))
            {
                foreach (var dependency in node.ToArray())
                {
                    var dependencyProperty = dependency as JProperty;
                    if (StringComparer.OrdinalIgnoreCase.Equals(dependencyProperty.Name, packageId))
                    {
                        dependency.Remove();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the NuGetFrameworks under frameworks
        /// </summary>
        public static IEnumerable<NuGetFramework> GetFrameworks(JObject json)
        {
            var results = new List<NuGetFramework>();

            JToken node = null;
            if (json.TryGetValue(FRAMEWORKS_TAG, out node))
            {
                foreach (var frameworkNode in node.ToArray())
                {
                    var frameworkProperty = frameworkNode as JProperty;

                    if (frameworkProperty != null)
                    {
                        results.Add(NuGetFramework.Parse(frameworkProperty.Name));
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Add the specified framework to JSON object
        /// </summary>
        public static void AddFramework(JObject json, NuGetFramework framework)
        {
            var frameworksList = GetFrameworks(json);
            if (HasFramework(frameworksList, framework))
            {
                return;
            }

            JObject frameworkSet = null;

            JToken node = null;
            if (json.TryGetValue(FRAMEWORKS_TAG, out node))
            {
                frameworkSet = node as JObject;
            }

            if (frameworkSet == null)
            {
                frameworkSet = new JObject();
            }

            var frameworkProperty = new JProperty(framework.GetShortFolderName(), new JObject());
            frameworkSet.Add(frameworkProperty);

            // order frameworks to reduce merge conflicts
            frameworkSet = SortProperties(frameworkSet);

            json[FRAMEWORKS_TAG] = frameworkSet;
        }

        /// <summary>
        ///  Clear all frameworks from the JSON object
        /// </summary>
        public static void ClearFrameworks(JObject json)
        {
            json[FRAMEWORKS_TAG] = new JObject();
        }

        /// <summary>
        ///  Sort child properties
        /// </summary>
        private static JObject SortProperties(JObject parent)
        {
            var sortedParent = new JObject();

            var sortedChildren = parent.Children().OrderByDescending(child => GetChildKey(child), StringComparer.OrdinalIgnoreCase);

            foreach (var child in sortedChildren)
            {
                sortedParent.AddFirst(child);
            }

            return sortedParent;
        }

        private static bool HasFramework(IEnumerable<NuGetFramework> list, NuGetFramework framework)
        {
            return list.Contains(framework);
        }

        private static string GetChildKey(JToken token)
        {
            var property = token as JProperty;

            if (property != null)
            {
                return property.Name;
            }

            return string.Empty;
        }
    }
}
