// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NuGet.Client;
using NuGet.Common;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;

namespace NuGet.Packaging.Rules
{
    internal class DependenciesGroupsForEachTFMRule : IPackageRule
    {
        private static readonly NuGetFramework Net00 = NuGetFramework.Parse("net00");

        public string MessageFormat => AnalysisResources.DependenciesGroupsForEachTFMHasNoExactMatch;

        private string CompatMatchFoundWarningMessageFormat => AnalysisResources.DependenciesGroupsForEachTFMHasCompatMatch;

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader package)
        {
            var files = package.GetFiles().
                Where(t => PathUtility.GetPathWithDirectorySeparator(t).Count(m => m == Path.DirectorySeparatorChar) > 1);
            var packageNuspec = ExtractTFMsFromNuspec(package.GetNuspec());
            var (compatNotExactMatches, noExactMatchesFromFile, noExactMatchesFromNuspec) = Categorize(files, packageNuspec);
            return GenerateWarnings(compatNotExactMatches, noExactMatchesFromFile, noExactMatchesFromNuspec);
        }

        internal IEnumerable<PackagingLogMessage> GenerateWarnings(HashSet<NuGetFramework> compatNotExactMatches, HashSet<NuGetFramework> noExactMatchesFromFile, HashSet<NuGetFramework> noExactMatchesFromNuspec)
        {
            (string noExactMatchString, string compatMatchString) = GenerateWarningString(noExactMatchesFromFile, noExactMatchesFromNuspec, (ICollection<NuGetFramework>)compatNotExactMatches);

            var issues = new List<PackagingLogMessage>();

            if (noExactMatchesFromFile.Count != 0 || noExactMatchesFromNuspec.Count != 0)
            {
                issues.Add(PackagingLogMessage.CreateWarning(noExactMatchString, NuGetLogCode.NU5128));
            }

            if (compatNotExactMatches.Count != 0)
            {
                issues.Add(PackagingLogMessage.CreateWarning(compatMatchString, NuGetLogCode.NU5130));
            }

            if (issues.Count != 0)
            {
                return issues;
            }

            return Array.Empty<PackagingLogMessage>();
        }

        internal (HashSet<NuGetFramework>, HashSet<NuGetFramework>, HashSet<NuGetFramework>) Categorize(IEnumerable<string> files, IEnumerable<NuGetFramework> tfmsFromNuspec)
        {
            var managedCodeConventions = new ManagedCodeConventions(RuntimeGraph.Empty);
            Func<object, object, bool> isCompatible = managedCodeConventions.Properties["tfm"].CompatibilityTest;
            var collection = new ContentItemCollection();
            collection.Load(files);

            List<ContentItemGroup> libItems = new();
            List<ContentItemGroup> refItems = new();
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.CompileLibAssemblies, libItems);
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.CompileRefAssemblies, refItems);

            var tfmsFromFilesSet = new HashSet<NuGetFramework>();
            tfmsFromFilesSet.AddRange(ContentExtractor.GetGroupFrameworks(libItems));
            tfmsFromFilesSet.AddRange(ContentExtractor.GetGroupFrameworks(refItems));

            var tfmsFromFiles = tfmsFromFilesSet.Where(t => t != Net00).ToList();

            var noExactMatchesFromFile = new HashSet<NuGetFramework>();
            var noExactMatchesFromNuspec = new HashSet<NuGetFramework>();
            var compatNotExactMatches = new HashSet<NuGetFramework>();

            noExactMatchesFromFile.AddRange(tfmsFromFiles.Where(t => !tfmsFromNuspec.Contains(t)));
            noExactMatchesFromNuspec.AddRange(tfmsFromNuspec.Where(t => !tfmsFromFiles.Contains(t)));

            foreach (var fileTFM in noExactMatchesFromFile)
            {
                foreach (var nuspecTFM in noExactMatchesFromNuspec)
                {
                    if (isCompatible(fileTFM, nuspecTFM))
                    {
                        compatNotExactMatches.Add(fileTFM);
                        break;
                    }
                }
            }

            if (compatNotExactMatches.Count != 0)
            {
                noExactMatchesFromFile.RemoveWhere(p => compatNotExactMatches.Contains(p));
            }

            return (compatNotExactMatches, noExactMatchesFromFile, noExactMatchesFromNuspec);
        }

        internal (string noExactMatchString, string compatMatchString) GenerateWarningString(ICollection<NuGetFramework> noExactMatchesFromFile,
                ICollection<NuGetFramework> noExactMatchesFromNuspec, ICollection<NuGetFramework> compatNotExactMatches)
        {
            var noExactMatchString = new StringBuilder();
            var compatMatchString = new StringBuilder();

            if (noExactMatchesFromFile.Count != 0)
            {
                noExactMatchString.AppendLine(MessageFormat);

                foreach (var tfm in noExactMatchesFromFile)
                {
                    if (tfm == noExactMatchesFromFile.First())
                    {
                        noExactMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMBeginningToNuspec);
                        noExactMatchString.Append(" ");
                        noExactMatchString.Append(tfm.GetFrameworkString());
                        continue;
                    }
                    noExactMatchString.Append(" ");
                    noExactMatchString.AppendLine(AnalysisResources.DependenciesGroupsForEachTFMEndingToNuspec);

                    noExactMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMBeginningToNuspec);
                    noExactMatchString.Append(" ");
                    noExactMatchString.Append(tfm.GetFrameworkString());
                }
                noExactMatchString.Append(" ");
                noExactMatchString.AppendLine(AnalysisResources.DependenciesGroupsForEachTFMEndingToNuspec);
            }

            if (noExactMatchesFromNuspec.Count != 0)
            {
                if (noExactMatchString.Length == 0)
                {
                    noExactMatchString.AppendLine(MessageFormat);

                }

                foreach (var tfm in noExactMatchesFromNuspec)
                {
                    if (tfm == noExactMatchesFromNuspec.First())
                    {
                        noExactMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMBeginningToFiles);
                        noExactMatchString.Append(" ");
                        noExactMatchString.Append(tfm.GetShortFolderName());
                        continue;
                    }
                    noExactMatchString.Append(" ");
                    noExactMatchString.AppendLine(AnalysisResources.DependenciesGroupsForEachTFMEndingToFile);
                    noExactMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMBeginningToFiles);
                    noExactMatchString.Append(" ");
                    noExactMatchString.Append(tfm.GetShortFolderName());
                }
                noExactMatchString.Append(" ");
                noExactMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMEndingToFile);
            }

            if (compatNotExactMatches.Count != 0)
            {
                compatMatchString.AppendLine(CompatMatchFoundWarningMessageFormat);

                foreach (var tfm in compatNotExactMatches)
                {
                    if (tfm == compatNotExactMatches.First())
                    {
                        compatMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMBeginningToNuspec);
                        compatMatchString.Append(" ");
                        compatMatchString.Append(tfm.GetFrameworkString());
                        continue;
                    }
                    compatMatchString.Append(" ");
                    compatMatchString.AppendLine(AnalysisResources.DependenciesGroupsForEachTFMEndingToNuspec);
                    compatMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMBeginningToNuspec);
                    compatMatchString.Append(" ");
                    compatMatchString.Append(tfm.GetFrameworkString());
                }
                compatMatchString.Append(" ");
                compatMatchString.Append(AnalysisResources.DependenciesGroupsForEachTFMEndingToNuspec);
            }

            return (noExactMatchString.ToString().Trim(), compatMatchString.ToString());
        }

        private static IEnumerable<NuGetFramework> ExtractTFMsFromNuspec(Stream packageNuspecStream)
        {
            var packageNuspec = new NuspecReader(packageNuspecStream);
            var nuspec = packageNuspec.Xml;
            if (nuspec != null)
            {
                XNamespace name = nuspec.Root.Name.Namespace;
                return nuspec.Descendants(XName.Get("{" + name.NamespaceName + "}dependencies")).Elements().Attributes("targetFramework").Select(f => NuGetFramework.Parse(f.Value));
            }
            return Array.Empty<NuGetFramework>();
        }
    }
}
