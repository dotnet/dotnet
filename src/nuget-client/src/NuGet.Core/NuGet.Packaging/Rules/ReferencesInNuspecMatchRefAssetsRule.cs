// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.Packaging.Rules
{
    internal class ReferencesInNuspecMatchRefAssetsRule : IPackageRule
    {
        private string _addToRefFormat = AnalysisResources.ReferencesInNuspecAndRefFilesDontMatchWarningAddToRefListItemFormat;
        private string _addToNuspecFormat = AnalysisResources.ReferencesInNuspecAndRefFilesDontMatchWarningAddToNuspecListItemFormat;
        private string _addToNuspecNoTfmFormat = AnalysisResources.ReferencesInNuspecAndRefFilesDontMatchWarningAddToNuspecNoTfmListItemFormat;
        public string MessageFormat => AnalysisResources.ReferencesInNuspecAndRefFilesDontMatchWarning;

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            var refFiles = builder.GetFiles().Where(t => t.StartsWith(PackagingConstants.Folders.Ref));
            var nuspecReferences = GetReferencesFromNuspec(builder.GetNuspec());
            var missingItems = Compare(nuspecReferences, refFiles);
            return GenerateWarnings(missingItems);
        }

        internal IDictionary<string, IEnumerable<string>> GetReferencesFromNuspec(Stream nuspecStream)
        {
            var nuspecReferences = new Dictionary<string, IEnumerable<string>>();
            var packageNuspec = new NuspecReader(nuspecStream);
            var nuspec = packageNuspec.Xml;
            if (nuspec != null)
            {
                XNamespace name = nuspec.Root.Name.Namespace;
                var targetFrameworks = nuspec.Descendants(XName.Get("{" + name.NamespaceName + "}references")).Elements().Attributes("targetFramework");
                nuspecReferences = targetFrameworks.ToDictionary(k => NuGetFramework.Parse(k.Value).GetShortFolderName(),
                                                                k => k.Parent.Elements().Attributes("file").Select(f => f.Value));
                var filesWithoutTFM = nuspec.Descendants(XName.Get("{" + name.NamespaceName + "}references"))
                    .Elements().Attributes("file").Select(f => f.Value);
                nuspecReferences.Add("any", filesWithoutTFM);
            }
            return nuspecReferences;
        }

        internal IEnumerable<MissingReference> Compare(IDictionary<string, IEnumerable<string>> nuspecReferences, IEnumerable<string> refFiles)
        {
            var missingReferences = new List<MissingReference>();
            if (nuspecReferences.Any())
            {
                if (refFiles.Any())
                {
                    var filesByTFM = refFiles.Where(t => t.Count(m => m == '/') > 1)
                        .GroupBy(t => NuGetFramework.ParseFolder(t.Split('/')[1]).GetShortFolderName(), t => Path.GetFileName(t));
                    var keys = GetAllKeys(filesByTFM);
                    var missingSubfolderInFiles = nuspecReferences.Keys.Where(t => !keys.Contains(t) &&
                    !NuGetFramework.ParseFolder(t).GetShortFolderName().Equals("unsupported") &&
                    !NuGetFramework.ParseFolder(t).GetShortFolderName().Equals("any"));
                    if (missingSubfolderInFiles.Any())
                    {
                        var subfolder = nuspecReferences.Where(t => missingSubfolderInFiles.Contains(t.Key));
                        foreach (var item in subfolder)
                        {
                            missingReferences.Add(new MissingReference("ref", item.Key, item.Value.ToArray()));
                        }
                    }

                    foreach (var files in filesByTFM)
                    {
                        if (files.Key == "unsupported")
                        {
                            continue;
                        }

                        string[] missingNuspecReferences;
                        string[] missingFiles;
                        IEnumerable<string> anyReferences = null;
                        if (nuspecReferences.TryGetValue(files.Key, out var currentReferences) ||
                            nuspecReferences.TryGetValue("any", out anyReferences))
                        {
                            if (anyReferences != null && currentReferences == null)
                            {
                                missingNuspecReferences = files.Where(m => !anyReferences.Contains(m)).ToArray();
                                missingFiles = anyReferences.Where(t => !files.Contains(t)).ToArray();
                            }
                            else
                            {
                                missingNuspecReferences = files.Where(m => !currentReferences.Contains(m)).ToArray();
                                missingFiles = currentReferences.Where(t => !files.Contains(t)).ToArray();
                            }
                        }
                        else
                        {
                            missingNuspecReferences = files.ToArray();
                            missingFiles = Array.Empty<string>();
                        }

                        if (missingFiles.Length != 0)
                        {
                            missingReferences.Add(new MissingReference("ref", files.Key, missingFiles));
                        }

                        if (missingNuspecReferences.Length != 0)
                        {
                            missingReferences.Add(new MissingReference("nuspec", files.Key, missingNuspecReferences));
                        }
                    }
                }
                else
                {
                    foreach (var item in nuspecReferences)
                    {
                        var refs = item.Value.ToArray();
                        if (refs.Length != 0)
                        {
                            missingReferences.Add(new MissingReference("ref", item.Key, refs));
                        }
                    }
                }
            }

            return missingReferences;
        }

        internal IEnumerable<PackagingLogMessage> GenerateWarnings(IEnumerable<MissingReference> missingReferences)
        {
            var issues = new List<PackagingLogMessage>();
            if (missingReferences.Any())
            {
                var message = new StringBuilder();
                message.AppendLine(MessageFormat);
                var referencesMissing = missingReferences.Where(t => t.MissingFrom == "nuspec");
                var refFilesMissing = missingReferences.Where(t => t.MissingFrom == "ref");
                foreach (var file in refFilesMissing)
                {
                    foreach (var item in file.MissingItems)
                    {
                        message.AppendLine(string.Format(CultureInfo.CurrentCulture, _addToRefFormat, item, file.Tfm));
                    }
                }

                foreach (var reference in referencesMissing)
                {
                    foreach (var item in reference.MissingItems)
                    {
                        if (reference.Tfm.Equals("any"))
                        {
                            message.AppendLine(string.Format(CultureInfo.CurrentCulture, _addToNuspecNoTfmFormat, item));
                        }
                        else
                        {
                            message.AppendLine(string.Format(CultureInfo.CurrentCulture, _addToNuspecFormat, item, reference.Tfm));
                        }
                    }
                }
                issues.Add(PackagingLogMessage.CreateWarning(message.ToString(), NuGetLogCode.NU5131));
            }
            return issues;
        }

        internal class MissingReference
        {
            public string MissingFrom { get; }

            public string Tfm { get; }

            public string[] MissingItems { get; }

            public MissingReference(string missingFrom, string tfm, string[] missingItems)
            {
                MissingFrom = missingFrom;
                Tfm = tfm;
                MissingItems = missingItems;
            }
        }

        internal List<string> GetAllKeys(IEnumerable<IGrouping<string, string>> filesByTFM)
        {
            var keys = new List<string>();
            foreach (var item in filesByTFM)
            {
                keys.Add(item.Key);
            }
            return keys;
        }
    }
}
