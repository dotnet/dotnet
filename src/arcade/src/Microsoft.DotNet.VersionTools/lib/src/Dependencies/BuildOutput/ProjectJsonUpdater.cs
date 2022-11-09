// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput
{
    public class ProjectJsonUpdater : IDependencyUpdater
    {
        public IEnumerable<string> ProjectJsonPaths { get; }

        public ProjectJsonUpdater(IEnumerable<string> projectJsonPaths)
        {
            ProjectJsonPaths = projectJsonPaths;
        }

        public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos)
        {
            BuildDependencyInfo[] buildDependencyInfos = dependencyInfos
                .OfType<BuildDependencyInfo>()
                .ToArray();

            var tasks = new List<DependencyUpdateTask>();
            foreach (string projectJsonFile in ProjectJsonPaths)
            {
                try
                {
                    IEnumerable<DependencyChange> dependencyChanges = null;

                    Action update = FileUtils.GetUpdateFileContentsTask(
                        projectJsonFile,
                        contents => ReplaceAllDependencyVersions(
                            contents,
                            projectJsonFile,
                            buildDependencyInfos,
                            out dependencyChanges));

                    // The output json may be different even if there weren't any changes made.
                    if (update != null && dependencyChanges.Any())
                    {
                        tasks.Add(new DependencyUpdateTask(
                            update,
                            dependencyChanges.Select(change => change.Info),
                            dependencyChanges.Select(change => $"In '{projectJsonFile}', {change.ToString()}")));
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceWarning($"Non-fatal exception occurred processing '{projectJsonFile}'. Skipping file. Exception: {e}. ");
                }
            }
            return tasks;
        }

        private string ReplaceAllDependencyVersions(
            string input,
            string projectJsonFile,
            IEnumerable<BuildDependencyInfo> buildInfos,
            out IEnumerable<DependencyChange> dependencyChanges)
        {
            JObject projectRoot = JObject.Parse(input);

            dependencyChanges = FindAllDependencyProperties(projectRoot)
                    .Select(dependencyProperty => ReplaceDependencyVersion(projectJsonFile, dependencyProperty, buildInfos))
                    .Where(buildInfo => buildInfo != null)
                    .ToArray();

            return JsonConvert.SerializeObject(projectRoot, Formatting.Indented) + Environment.NewLine;
        }

        /// <summary>
        /// Replaces the single dependency with the updated version, if it matches any of the
        /// dependencies that need to be updated. Stops on the first updated value found.
        /// </summary>
        /// <returns>The BuildInfo used to change the value, or null if there was no change.</returns>
        private DependencyChange ReplaceDependencyVersion(
            string projectJsonFile,
            JProperty dependencyProperty,
            IEnumerable<BuildDependencyInfo> parsedBuildInfos)
        {
            string id = dependencyProperty.Name;
            foreach (BuildDependencyInfo info in parsedBuildInfos)
            {
                foreach (PackageIdentity packageInfo in info.Packages)
                {
                    if (id != packageInfo.Id)
                    {
                        continue;
                    }

                    string oldVersion;
                    if (dependencyProperty.Value is JObject)
                    {
                        oldVersion = (string)dependencyProperty.Value["version"];
                    }
                    else
                    {
                        oldVersion = (string)dependencyProperty.Value;
                    }
                    VersionRange parsedOldVersionRange;
                    if (!VersionRange.TryParse(oldVersion, out parsedOldVersionRange))
                    {
                        Trace.TraceWarning($"Couldn't parse '{oldVersion}' for package '{id}' in '{projectJsonFile}'. Skipping.");
                        continue;
                    }
                    NuGetVersion oldNuGetVersion = parsedOldVersionRange.MinVersion;

                    if (oldNuGetVersion == packageInfo.Version)
                    {
                        // Versions match, no update to make.
                        continue;
                    }

                    if (oldNuGetVersion.IsPrerelease || info.UpgradeStableVersions)
                    {
                        string newVersion = packageInfo.Version.ToNormalizedString();
                        if (dependencyProperty.Value is JObject)
                        {
                            dependencyProperty.Value["version"] = newVersion;
                        }
                        else
                        {
                            dependencyProperty.Value = newVersion;
                        }

                        return new DependencyChange
                        {
                            Info = info,
                            PackageId = id,
                            Before = oldNuGetVersion,
                            After = packageInfo.Version
                        };
                    }
                }
            }
            return null;
        }

        private static IEnumerable<JProperty> FindAllDependencyProperties(JObject projectJsonRoot)
        {
            return projectJsonRoot
                .Descendants()
                .OfType<JProperty>()
                .Where(property => property.Name == "dependencies")
                .Select(property => property.Value)
                .SelectMany(o => o.Children<JProperty>());
        }

        private class DependencyChange
        {
            public BuildDependencyInfo Info { get; set; }
            public string PackageId { get; set; }
            public NuGetVersion Before { get; set; }
            public NuGetVersion After { get; set; }

            public override string ToString()
            {
                return $"'{PackageId} {Before.ToNormalizedString()}' must be " +
                    $"'{After.ToNormalizedString()}' ({Info.BuildInfo.Name})";
            }
        }
    }
}
