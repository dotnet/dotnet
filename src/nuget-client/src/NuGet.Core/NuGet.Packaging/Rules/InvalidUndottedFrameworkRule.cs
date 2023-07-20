// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NuGet.Client;
using NuGet.Common;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;

namespace NuGet.Packaging.Rules
{
    internal class InvalidUndottedFrameworkRule : IPackageRule
    {
        private const string TargetFramework = "targetFramework";

        private const string Metadata = "metadata";

        private const string Dependencies = "dependencies";

        private const string Group = "group";

        private const string References = "references";

        private const string FrameworkAssemblies = "frameworkAssemblies";

        private const string FrameworkAssembly = "frameworkAssembly";

        private static readonly char[] CommaArray = new char[] { ',' };

        // NOTE: We generate many different messages here, so we avoid using MessageFormat itself.
        public string MessageFormat => "";

        public InvalidUndottedFrameworkRule()
        {
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            return Validate(LoadXml(builder.GetNuspec()), builder.GetFiles());
        }

        internal static IEnumerable<PackagingLogMessage> Validate(XDocument xml, IEnumerable<string> files)
        {
            // NOTE: Most of these validators are partially extracted from
            // NuspecReader, because we need the raw framework strings, not
            // the frameworks themselves. That does end up with a bit of
            // duplicate code, but the alternative is to expand the scope of
            // NuspecReader by a lot.
            var metadataNode = xml.Root.Elements().Where(e => StringComparer.Ordinal.Equals(e.Name.LocalName, Metadata)).FirstOrDefault();
            if (metadataNode == null)
            {
                throw new PackagingException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MissingMetadataNode,
                    Metadata));
            }

            var logMessages = ValidateDependencyGroups(metadataNode)
                .Concat(ValidateReferenceGroups(metadataNode))
                .Concat(ValidateFrameworkAssemblies(xml, metadataNode))
                .Concat(ValidateFiles(files));

            if (logMessages.Any())
            {
                return logMessages.Concat(new List<PackagingLogMessage>()
                {
                    PackagingLogMessage.CreateWarning(string.Format(CultureInfo.CurrentCulture, AnalysisResources.InvalidUndottedFrameworkWarning), NuGetLogCode.NU5501)
                });
            }
            else
            {
                return logMessages;
            }
        }

        internal static IEnumerable<PackagingLogMessage> ValidateDependencyGroups(XElement metadataNode)
        {
            var ns = metadataNode.GetDefaultNamespace().NamespaceName;
            var dependencyNode = metadataNode
                .Elements(XName.Get(Dependencies, ns));

            var dependencyGroups = dependencyNode
                .Elements(XName.Get(Group, ns));

            var bads = new HashSet<string>();
            foreach (var depGroup in dependencyGroups)
            {
                var groupFramework = GetAttributeValue(depGroup, TargetFramework);
                if (!string.IsNullOrEmpty(groupFramework) && !FrameworkVersionHasDesiredDots(groupFramework))
                {
                    bads.Add(groupFramework.Trim());
                }
            }

            var messages = new List<PackagingLogMessage>();

            if (bads.Any())
            {
                messages.Add(
                    PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, AnalysisResources.InvalidUndottedFrameworkInDependencyGroupsWarning, string.Join(", ", bads)),
                        NuGetLogCode.NU5501
                    )
                );
            }

            return messages;
        }

        internal static IEnumerable<PackagingLogMessage> ValidateReferenceGroups(XElement metadataNode)
        {
            var ns = metadataNode.GetDefaultNamespace().NamespaceName;

            var bads = new HashSet<string>();
            foreach (var group in metadataNode.Elements(XName.Get(References, ns)).Elements(XName.Get(Group, ns)))
            {
                var groupFramework = GetAttributeValue(group, TargetFramework);
                if (!string.IsNullOrEmpty(groupFramework) && !FrameworkVersionHasDesiredDots(groupFramework))
                {
                    bads.Add(groupFramework.Trim());
                }
            }

            var messages = new List<PackagingLogMessage>();

            if (bads.Any())
            {
                messages.Add(
                    PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, AnalysisResources.InvalidUndottedFrameworkInReferenceGroupsWarning, string.Join(", ", bads)),
                        NuGetLogCode.NU5501
                    )
                );
            }

            return messages;
        }

        internal static IEnumerable<PackagingLogMessage> ValidateFrameworkAssemblies(XDocument xml, XElement metadataNode)
        {
            var ns = xml.Root.GetDefaultNamespace().NamespaceName;

            var frameworks = new HashSet<string>();

            foreach (var group in metadataNode.Elements(XName.Get(FrameworkAssemblies, ns)).Elements(XName.Get(FrameworkAssembly, ns))
                .GroupBy(n => GetAttributeValue(n, TargetFramework)))
            {
                // Framework references may have multiple comma delimited frameworks
                if (!string.IsNullOrEmpty(group.Key))
                {
                    foreach (var fwString in group.Key.Split(CommaArray, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(fwString))
                        {
                            frameworks.Add(fwString.Trim());
                        }
                    }
                }
            }

            var bads = new HashSet<string>();
            foreach (var framework in frameworks)
            {
                if (!string.IsNullOrEmpty(framework) && !FrameworkVersionHasDesiredDots(framework))
                {
                    bads.Add(framework);
                }

            }

            var messages = new List<PackagingLogMessage>();

            if (bads.Any())
            {
                messages.Add(
                    PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, AnalysisResources.InvalidUndottedFrameworkInFrameworkAssemblyGroupsWarning, string.Join(", ", bads)),
                        NuGetLogCode.NU5501
                    )
                );
            }

            return messages;
        }

        internal static IEnumerable<PackagingLogMessage> ValidateFiles(IEnumerable<string> files)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var file in files.Select(t => PathUtility.GetPathWithDirectorySeparator(t)))
            {
                set.Add(file);
            }

            var managedCodeConventions = new ManagedCodeConventions(RuntimeGraph.Empty);
            var collection = new ContentItemCollection();
            collection.Load(set.Select(path => path.Replace('\\', '/')).ToArray());

            var patterns = managedCodeConventions.Patterns;

            var frameworkPatterns = new List<PatternSet>()
            {
                patterns.RuntimeAssemblies,
                patterns.CompileRefAssemblies,
                patterns.CompileLibAssemblies,
                patterns.NativeLibraries,
                patterns.ResourceAssemblies,
                patterns.MSBuildFiles,
                patterns.ContentFiles,
                patterns.ToolsAssemblies,
                patterns.EmbedAssemblies,
                patterns.MSBuildTransitiveFiles
            };
            var warnPaths = new HashSet<string>();

            List<ContentItemGroup> targetedItemGroups = new();
            foreach (var pattern in frameworkPatterns)
            {
                targetedItemGroups.Clear();
                ContentExtractor.GetContentForPattern(collection, pattern, targetedItemGroups);
                foreach (ContentItemGroup group in targetedItemGroups)
                {
                    foreach (ContentItem item in group.Items.NoAllocEnumerate())
                    {
                        var exists = item.Properties.TryGetValue("tfm_raw", out var frameworkRaw);
                        string frameworkString = (string)frameworkRaw;
                        if (!exists || string.IsNullOrEmpty(frameworkString))
                        {
                            continue;
                        }

                        if (!FrameworkVersionHasDesiredDots(frameworkString))
                        {
                            warnPaths.Add(item.Path);
                        }
                    }
                }
            }

            var messages = new List<PackagingLogMessage>();

            if (warnPaths.Count > 0)
            {
                messages.Add(
                    PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, AnalysisResources.InvalidUndottedFrameworkInFilesWarning, string.Join(", ", warnPaths)),
                        NuGetLogCode.NU5501
                    )
                );
            }

            return messages;
        }

        private static XDocument LoadXml(Stream stream)
        {
            using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings
            {
                CloseInput = true,
                IgnoreWhitespace = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            }))
            {
                return XDocument.Load(xmlReader, LoadOptions.None);
            }
        }

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            var attribute = element.Attribute(XName.Get(attributeName));
            return attribute?.Value;
        }

        internal static bool FrameworkVersionHasDesiredDots(string frameworkString)
        {
            var framework = NuGetFramework.Parse(frameworkString);
            if (framework.Version.Major >= 5 &&
                StringComparer.OrdinalIgnoreCase.Equals(FrameworkConstants.FrameworkIdentifiers.NetCoreApp, framework.Framework))
            {
                var dotIndex = frameworkString.IndexOf(".", StringComparison.Ordinal);
                var dashIndex = frameworkString.IndexOf("-", StringComparison.Ordinal);
                return (dashIndex > -1 && dotIndex > -1 && dotIndex < dashIndex) || (dashIndex == -1 && dotIndex > -1);
            }
            else
            {
                return true;
            }
        }
    }
}

