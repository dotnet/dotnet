// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.ProjectModel;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class VSRestoreSettingsUtilities
    {
        public const string AdditionalValue = "$Additional$";

        public static string GetPackagesPath(ISettings settings, PackageSpec project)
        {
            // Set from Settings if not given. Clear is not an option here.
            if (string.IsNullOrEmpty(project.RestoreMetadata.PackagesPath))
            {
                return SettingsUtility.GetGlobalPackagesFolder(settings);
            }

            // Resolve relative paths
            return UriUtility.GetAbsolutePathFromFile(
                sourceFile: project.RestoreMetadata.ProjectPath,
                path: project.RestoreMetadata.PackagesPath);
        }

        /// <summary>
        /// This method receives the entries in the following format
        /// [ [ [values..] AdditionalValue ] additionalValues..]
        /// It then outputs a tuple where the entries before AdditionalValue are in Item1 and the additionalValues are in item2.
        /// All values are optional.
        /// For correctness the input to this method should have been created by calling <see cref="GetEntriesWithAdditional(string[], string[])"/>
        /// </summary>
        private static Tuple<IList<string>, IList<string>> ProcessEntriesWithAdditional(IEnumerable<string> entries)
        {
            var actualEntries = new List<string>();
            var additionalEntries = new List<string>();

            var readingAdditional = false;
            foreach (var entry in entries)
            {
                if (StringComparer.Ordinal.Equals(AdditionalValue, entry))
                {
                    readingAdditional = true;
                }
                else
                {
                    if (readingAdditional)
                    {
                        additionalEntries.Add(entry);
                    }
                    else
                    {
                        actualEntries.Add(entry);
                    }
                }
            }

            return new Tuple<IList<string>, IList<string>>(actualEntries, additionalEntries);
        }

        public static IList<PackageSource> GetSources(ISettings settings, PackageSpec project)
        {
            var results = ProcessEntriesWithAdditional(project.RestoreMetadata.Sources.Select(e => e.Source).ToList());
            var sources = results.Item1;
            var additionalSources = results.Item2;

            var processedSources = (
                        ShouldReadFromSettings(sources) ?
                            SettingsUtility.GetEnabledSources(settings).Select(e => e.Source) :
                            HandleClear(sources))
                        .Concat(additionalSources);

            // Resolve relative paths
            return processedSources.Select(e => new PackageSource(
                UriUtility.GetAbsolutePathFromFile(
                    sourceFile: project.RestoreMetadata.ProjectPath,
                    path: e)))
                .ToList();
        }

        public static IList<string> GetFallbackFolders(ISettings settings, PackageSpec project)
        {
            var results = ProcessEntriesWithAdditional(project.RestoreMetadata.FallbackFolders);
            var fallbackFolders = results.Item1;
            var additionalFallbackFolders = results.Item2;


            var processedFallbackFolders = (
                        ShouldReadFromSettings(fallbackFolders) ?
                            SettingsUtility.GetFallbackPackageFolders(settings) :
                            HandleClear(fallbackFolders))
                        .Concat(additionalFallbackFolders);


            // Resolve relative paths
            return processedFallbackFolders.Select(e =>
                UriUtility.GetAbsolutePathFromFile(
                    sourceFile: project.RestoreMetadata.ProjectPath,
                    path: e))
                .ToList();
        }

        private static bool ShouldReadFromSettings(IEnumerable<string> values)
        {
            return !values.Any() && !MSBuildRestoreUtility.ContainsClearKeyword(values);
        }

        public static IEnumerable<string> HandleClear(IEnumerable<string> values)
        {

            if ((MSBuildRestoreUtility.ContainsClearKeyword(values)))
            {
                return Enumerable.Empty<string>();
            }

            return values;
        }


        /// <summary>
        /// This method combine the values and additionalValues into a format as below
        /// [ [ [values..] AdditionalValue ] additionalValues..]
        /// IF additionalValues does not have any elements then the additionalValue keyword will not be added, the return value will be equivalent to the first element, whatever it may be.
        /// The <see cref="ProcessEntriesWithAdditional(IEnumerable{string})"/>does the reverse.
        /// </summary>
        public static IEnumerable<string> GetEntriesWithAdditional(string[] values, string[] additional)
        {
            return additional.Length != 0 ?
                (values.Concat(new string[] { AdditionalValue }).Concat(additional)) :
                values;
        }
    }
}
