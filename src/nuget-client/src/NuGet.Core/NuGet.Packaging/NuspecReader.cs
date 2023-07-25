// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    /// <summary>
    /// Reads .nuspec files
    /// </summary>
    public class NuspecReader : NuspecCoreReaderBase
    {
        // node names
        private const string Dependencies = "dependencies";
        private const string Group = "group";
        private const string TargetFramework = "targetFramework";
        private const string Dependency = "dependency";
        private const string References = "references";
        private const string Reference = "reference";
        private const string File = "file";
        private const string FrameworkAssemblies = "frameworkAssemblies";
        private const string FrameworkAssembly = "frameworkAssembly";
        private const string AssemblyName = "assemblyName";
        private const string Language = "language";
        private const string ContentFiles = "contentFiles";
        private const string Files = "files";
        private const string BuildAction = "buildAction";
        private const string Flatten = "flatten";
        private const string CopyToOutput = "copyToOutput";
        private const string IncludeFlags = "include";
        private const string ExcludeFlags = "exclude";
        private const string LicenseUrl = "licenseUrl";
        private const string Repository = "repository";
        private const string Icon = "icon";
        private const string Readme = "readme";

        private static readonly char[] CommaArray = new char[] { ',' };
        private readonly IFrameworkNameProvider _frameworkProvider;

        /// <summary>
        /// Nuspec file reader.
        /// </summary>
        public NuspecReader(string path)
            : this(path, DefaultFrameworkNameProvider.Instance)
        {
        }

        /// <summary>
        /// Nuspec file reader.
        /// </summary>
        public NuspecReader(string path, IFrameworkNameProvider frameworkProvider)
            : base(path)
        {
            _frameworkProvider = frameworkProvider;
        }


        /// <summary>
        /// Nuspec file reader
        /// </summary>
        /// <param name="stream">Nuspec file stream.</param>
        public NuspecReader(Stream stream)
            : this(stream, DefaultFrameworkNameProvider.Instance, leaveStreamOpen: false)
        {

        }

        /// <summary>
        /// Nuspec file reader
        /// </summary>
        /// <param name="xml">Nuspec file xml data.</param>
        public NuspecReader(XDocument xml)
            : this(xml, DefaultFrameworkNameProvider.Instance)
        {

        }

        /// <summary>
        /// Nuspec file reader
        /// </summary>
        /// <param name="stream">Nuspec file stream.</param>
        /// <param name="frameworkProvider">Framework mapping provider for NuGetFramework parsing.</param>
        public NuspecReader(Stream stream, IFrameworkNameProvider frameworkProvider, bool leaveStreamOpen)
            : base(stream, leaveStreamOpen)
        {
            _frameworkProvider = frameworkProvider;
        }

        /// <summary>
        /// Nuspec file reader
        /// </summary>
        /// <param name="xml">Nuspec file xml data.</param>
        /// <param name="frameworkProvider">Framework mapping provider for NuGetFramework parsing.</param>
        public NuspecReader(XDocument xml, IFrameworkNameProvider frameworkProvider)
            : base(xml)
        {
            _frameworkProvider = frameworkProvider;
        }

        /// <summary>
        /// Read package dependencies for all frameworks
        /// </summary>
        public IEnumerable<PackageDependencyGroup> GetDependencyGroups()
        {
            return GetDependencyGroups(useStrictVersionCheck: false);
        }

        /// <summary>
        /// Read package dependencies for all frameworks
        /// </summary>
        public IEnumerable<PackageDependencyGroup> GetDependencyGroups(bool useStrictVersionCheck)
        {
            var ns = MetadataNode.GetDefaultNamespace().NamespaceName;
            var dependencyNode = MetadataNode
                .Elements(XName.Get(Dependencies, ns));

            var groupFound = false;
            var dependencyGroups = dependencyNode
                .Elements(XName.Get(Group, ns));

            foreach (var depGroup in dependencyGroups)
            {
                groupFound = true;

                var groupFramework = GetAttributeValue(depGroup, TargetFramework);

                var dependencies = depGroup
                    .Elements(XName.Get(Dependency, ns));

                var packages = GetPackageDependencies(dependencies, useStrictVersionCheck);

                var framework = string.IsNullOrEmpty(groupFramework)
                    ? NuGetFramework.AnyFramework
                    : NuGetFramework.Parse(groupFramework, _frameworkProvider);

                yield return new PackageDependencyGroup(framework, packages);
            }

            // legacy behavior
            if (!groupFound)
            {
                var legacyDependencies = dependencyNode
                    .Elements(XName.Get(Dependency, ns));

                var packages = GetPackageDependencies(legacyDependencies, useStrictVersionCheck);

                if (packages.Any())
                {
                    yield return new PackageDependencyGroup(NuGetFramework.AnyFramework, packages);
                }
            }
        }

        /// <summary>
        /// Reference item groups
        /// </summary>
        public IEnumerable<FrameworkSpecificGroup> GetReferenceGroups()
        {
            var ns = MetadataNode.GetDefaultNamespace().NamespaceName;

            var groupFound = false;

            foreach (var group in MetadataNode.Elements(XName.Get(References, ns)).Elements(XName.Get(Group, ns)))
            {
                groupFound = true;

                var groupFramework = GetAttributeValue(group, TargetFramework);

                var items = group.Elements(XName.Get(Reference, ns)).Select(n => GetAttributeValue(n, File)).Where(n => !string.IsNullOrEmpty(n)).ToArray();

                var framework = string.IsNullOrEmpty(groupFramework) ? NuGetFramework.AnyFramework : NuGetFramework.Parse(groupFramework, _frameworkProvider);

                yield return new FrameworkSpecificGroup(framework, items);
            }

            // pre-2.5 flat list of references, this should only be used if there are no groups
            if (!groupFound)
            {
                var items = MetadataNode.Elements(XName.Get(References, ns))
                    .Elements(XName.Get(Reference, ns)).Select(n => GetAttributeValue(n, File)).Where(n => !string.IsNullOrEmpty(n)).ToArray();

                if (items.Length > 0)
                {
                    yield return new FrameworkSpecificGroup(NuGetFramework.AnyFramework, items);
                }
            }

            yield break;
        }

        /// <summary>
        /// Framework assembly groups
        /// </summary>
        [Obsolete("GetFrameworkReferenceGroups() is deprecated. Please use GetFrameworkAssemblyGroups() instead.")]
        public IEnumerable<FrameworkSpecificGroup> GetFrameworkReferenceGroups()
        {
            return GetFrameworkAssemblyGroups();
        }

        /// <summary>
        /// Framework assembly groups
        /// </summary>
        public IEnumerable<FrameworkSpecificGroup> GetFrameworkAssemblyGroups()
        {
            var results = new List<FrameworkSpecificGroup>();

            var ns = Xml.Root.GetDefaultNamespace().NamespaceName;

            var groups = new Dictionary<NuGetFramework, HashSet<string>>(NuGetFrameworkFullComparer.Instance);

            foreach (var group in MetadataNode.Elements(XName.Get(FrameworkAssemblies, ns)).Elements(XName.Get(FrameworkAssembly, ns))
                .GroupBy(n => GetAttributeValue(n, TargetFramework)))
            {
                // Framework references may have multiple comma delimited frameworks
                var frameworks = new List<NuGetFramework>();

                // Empty frameworks go under Any
                if (string.IsNullOrEmpty(group.Key))
                {
                    frameworks.Add(NuGetFramework.AnyFramework);
                }
                else
                {
                    foreach (var fwString in group.Key.Split(CommaArray, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(fwString))
                        {
                            frameworks.Add(NuGetFramework.Parse(fwString.Trim(), _frameworkProvider));
                        }
                    }
                }

                // apply items to each framework
                foreach (var framework in frameworks)
                {
                    HashSet<string> items = null;
                    if (!groups.TryGetValue(framework, out items))
                    {
                        items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        groups.Add(framework, items);
                    }

                    // Merge items and ignore duplicates
                    items.UnionWith(group.Select(item => GetAttributeValue(item, AssemblyName)).Where(item => !string.IsNullOrEmpty(item)));
                }
            }

            // Sort items to make this deterministic for the caller
            foreach ((var framework, var items) in groups.OrderBy(e => e.Key, NuGetFrameworkSorter.Instance))
            {
                var group = new FrameworkSpecificGroup(framework, items.OrderBy(item => item, StringComparer.OrdinalIgnoreCase));

                results.Add(group);
            }

            return results;
        }

        /// <summary>
        /// Package language
        /// </summary>
        public string GetLanguage()
        {
            var node = MetadataNode.Elements(XName.Get(Language, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node?.Value;
        }

        /// <summary>
        /// Package License Url
        /// </summary>
        public string GetLicenseUrl()
        {
            var node = MetadataNode.Elements(XName.Get(LicenseUrl, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node?.Value;
        }

        /// <summary>
        /// Build action groups
        /// </summary>
        public IEnumerable<ContentFilesEntry> GetContentFiles()
        {
            var ns = MetadataNode.GetDefaultNamespace().NamespaceName;

            foreach (var filesNode in MetadataNode
                .Elements(XName.Get(ContentFiles, ns))
                .Elements(XName.Get(Files, ns)))
            {
                var include = GetAttributeValue(filesNode, "include");

                if (include == null)
                {
                    // Invalid include
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidNuspecEntry,
                        filesNode.ToString().Trim(),
                        GetIdentity());

                    throw new PackagingException(message);
                }

                var exclude = GetAttributeValue(filesNode, "exclude");

                if (string.IsNullOrEmpty(exclude))
                {
                    exclude = null;
                }

                var buildAction = GetAttributeValue(filesNode, BuildAction);
                var flatten = AttributeAsNullableBool(filesNode, Flatten);
                var copyToOutput = AttributeAsNullableBool(filesNode, CopyToOutput);

                yield return new ContentFilesEntry(include, exclude, buildAction, copyToOutput, flatten);
            }

            yield break;
        }

        /// <summary>
        /// Package title.
        /// </summary>
        public string GetTitle()
        {
            return GetMetadataValue("title");
        }

        /// <summary>
        /// Package authors.
        /// </summary>
        public string GetAuthors()
        {
            return GetMetadataValue("authors");
        }

        /// <summary>
        /// Package tags.
        /// </summary>
        public string GetTags()
        {
            return GetMetadataValue("tags");
        }

        /// <summary>
        /// Package owners.
        /// </summary>
        public string GetOwners()
        {
            return GetMetadataValue("owners");
        }

        /// <summary>
        /// Package description.
        /// </summary>
        public string GetDescription()
        {
            return GetMetadataValue("description");
        }

        /// <summary>
        /// Package release notes.
        /// </summary>
        public string GetReleaseNotes()
        {
            return GetMetadataValue("releaseNotes");
        }

        /// <summary>
        /// Package summary.
        /// </summary>
        public string GetSummary()
        {
            return GetMetadataValue("summary");
        }

        /// <summary>
        /// Package project url.
        /// </summary>
        public string GetProjectUrl()
        {
            return GetMetadataValue("projectUrl");
        }

        /// <summary>
        /// Package icon url.
        /// </summary>
        public string GetIconUrl()
        {
            return GetMetadataValue("iconUrl");
        }

        /// <summary>
        /// Copyright information.
        /// </summary>
        public string GetCopyright()
        {
            return GetMetadataValue("copyright");
        }

        /// <summary>
        /// Source control repository information.
        /// </summary>
        public RepositoryMetadata GetRepositoryMetadata()
        {
            var repository = new RepositoryMetadata();
            var node = MetadataNode.Elements(XName.Get(Repository, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();

            if (node != null)
            {
                repository.Type = GetAttributeValue(node, "type") ?? string.Empty;
                repository.Url = GetAttributeValue(node, "url") ?? string.Empty;
                repository.Branch = GetAttributeValue(node, "branch") ?? string.Empty;
                repository.Commit = GetAttributeValue(node, "commit") ?? string.Empty;
            }

            return repository;
        }

        /// <summary>
        /// Parses the license object if specified.
        /// The metadata can be of 2 types, Expression and File.
        /// The method will not fail if it sees values that invalid (empty/unparseable license etc), but it will rather add validation errors/warnings. 
        /// </summary>
        /// <remarks>This method never throws. Bad data is still parsed. </remarks>
        /// <returns>The licensemetadata if specified</returns>
        public LicenseMetadata GetLicenseMetadata()
        {
            var licenseNode = MetadataNode.Elements(XName.Get(NuspecUtility.License, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();

            if (licenseNode != null)
            {
                var type = licenseNode.Attribute(NuspecUtility.Type)?.Value.SafeTrim();
                var license = licenseNode.Value.SafeTrim();
                var versionValue = licenseNode.Attribute(NuspecUtility.Version)?.Value.SafeTrim();

                var isKnownType = Enum.TryParse(type, ignoreCase: true, result: out LicenseType licenseType);

                List<string> errors = null;

                if (isKnownType)
                {
                    Version version = null;
                    if (versionValue != null)
                    {
                        if (!System.Version.TryParse(versionValue, out version))
                        {
                            errors = new List<string>
                            {
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.NuGetLicense_InvalidLicenseExpressionVersion,
                                    versionValue)
                            };
                        }
                    }
                    version = version ?? LicenseMetadata.EmptyVersion;

                    if (string.IsNullOrEmpty(license))
                    {
                        if (errors == null)
                        {
                            errors = new List<string>();
                        }
                        errors.Add(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.NuGetLicense_LicenseElementMissingValue));
                    }
                    else
                    {
                        if (licenseType == LicenseType.Expression)
                        {
                            if (version.CompareTo(LicenseMetadata.CurrentVersion) <= 0)
                            {
                                try
                                {
                                    var expression = NuGetLicenseExpression.Parse(license);

                                    var invalidLicenseIdentifiers = GetNonStandardLicenseIdentifiers(expression);
                                    if (invalidLicenseIdentifiers != null)
                                    {
                                        if (errors == null)
                                        {
                                            errors = new List<string>();
                                        }
                                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_NonStandardIdentifier, string.Join(", ", invalidLicenseIdentifiers)));
                                    }
                                    if (expression.IsUnlicensed())
                                    {
                                        if (errors == null)
                                        {
                                            errors = new List<string>();
                                        }
                                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_UnlicensedPackageWarning));
                                    }

                                    return new LicenseMetadata(type: licenseType, license: license, expression: expression, warningsAndErrors: errors, version: version);
                                }
                                catch (NuGetLicenseExpressionParsingException e)
                                {
                                    if (errors == null)
                                    {
                                        errors = new List<string>();
                                    }
                                    errors.Add(e.Message);
                                }
                                return new LicenseMetadata(type: licenseType, license: license, expression: null, warningsAndErrors: errors, version: version);
                            }
                            else
                            {
                                if (errors == null)
                                {
                                    errors = new List<string>();
                                }

                                errors.Add(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Strings.NuGetLicense_LicenseExpressionVersionTooHigh,
                                        version,
                                        LicenseMetadata.CurrentVersion));

                                return new LicenseMetadata(type: licenseType, license: license, expression: null, warningsAndErrors: errors, version: version);
                            }
                        }
                    }
                    return new LicenseMetadata(type: licenseType, license: license, expression: null, warningsAndErrors: errors, version: version);
                }
            }
            return null;
        }

        private static IList<string> GetNonStandardLicenseIdentifiers(NuGetLicenseExpression expression)
        {
            IList<string> invalidLicenseIdentifiers = null;
            Action<NuGetLicense> licenseProcessor = delegate (NuGetLicense nugetLicense)
            {
                if (!nugetLicense.IsStandardLicense)
                {
                    if (invalidLicenseIdentifiers == null)
                    {
                        invalidLicenseIdentifiers = new List<string>();
                    }
                    invalidLicenseIdentifiers.Add(nugetLicense.Identifier);
                }
            };
            expression.OnEachLeafNode(licenseProcessor, null);

            return invalidLicenseIdentifiers;
        }

        /// <summary>
        /// Require license acceptance when installing the package.
        /// </summary>
        public bool GetRequireLicenseAcceptance()
        {
            return StringComparer.OrdinalIgnoreCase.Equals(bool.TrueString, GetMetadataValue("requireLicenseAcceptance"));
        }

        /// <summary>
        /// Read package dependencies for all frameworks
        /// </summary>
        public IEnumerable<FrameworkReferenceGroup> GetFrameworkRefGroups()
        {
            return NuspecUtility.GetFrameworkReferenceGroups(MetadataNode, _frameworkProvider, useMetadataNamespace: true);
        }

        /// <summary>
        /// Gets the icon metadata from the .nuspec
        /// </summary>
        /// <returns>A string containing the icon path or null if no icon entry is found</returns>
        public string GetIcon()
        {
            var node = MetadataNode.Elements(XName.Get(Icon, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node?.Value;
        }

        /// <summary>
        /// Gets the readme metadata from the .nuspec
        /// </summary>
        /// <returns>A string containing the readme path or null if no readme entry is found</returns>
        public string GetReadme()
        {
            var node = MetadataNode.Elements(XName.Get(Readme, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node?.Value;
        }

        private bool? AttributeAsNullableBool(XElement element, string attributeName)
        {
            bool? result = null;

            var attributeValue = GetAttributeValue(element, attributeName);

            if (attributeValue != null)
            {
                if (bool.TrueString.Equals(attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                }
                else if (bool.FalseString.Equals(attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                }
                else
                {
                    var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.InvalidNuspecEntry,
                            element.ToString().Trim(),
                            GetIdentity());

                    throw new PackagingException(message);
                }
            }

            return result;
        }

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            var attribute = element.Attribute(XName.Get(attributeName));
            return attribute == null ? null : attribute.Value;
        }

        private static readonly List<string> EmptyList = new List<string>();

        private static List<string> GetFlags(string flags)
        {
            if (string.IsNullOrEmpty(flags))
            {
                return EmptyList;
            }

            var set = new HashSet<string>(
                flags.Split(CommaArray, StringSplitOptions.RemoveEmptyEntries)
                    .Select(flag => flag.Trim()),
                StringComparer.OrdinalIgnoreCase);

            return set.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
        }

        private HashSet<PackageDependency> GetPackageDependencies(IEnumerable<XElement> nodes, bool useStrictVersionCheck)
        {
            var packages = new HashSet<PackageDependency>();

            foreach (var depNode in nodes)
            {
                VersionRange range = null;

                var rangeNode = GetAttributeValue(depNode, Version);

                if (!string.IsNullOrEmpty(rangeNode))
                {
                    var versionParsedSuccessfully = VersionRange.TryParse(rangeNode, out range);
                    if (!versionParsedSuccessfully && useStrictVersionCheck)
                    {
                        // Invalid version
                        var dependencyId = GetAttributeValue(depNode, Id);
                        var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.ErrorInvalidPackageVersionForDependency,
                            dependencyId,
                            GetIdentity(),
                            rangeNode);

                        throw new PackagingException(message);
                    }
                }
                else if (useStrictVersionCheck)
                {
                    // Invalid version
                    var dependencyId = GetAttributeValue(depNode, Id);
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.ErrorInvalidPackageVersionForDependency,
                        dependencyId,
                        GetIdentity(),
                        rangeNode);

                    throw new PackagingException(message);
                }

                var includeFlags = GetFlags(GetAttributeValue(depNode, IncludeFlags));
                var excludeFlags = GetFlags(GetAttributeValue(depNode, ExcludeFlags));

                var dependency = new PackageDependency(
                    GetAttributeValue(depNode, Id),
                    range,
                    includeFlags,
                    excludeFlags);

                packages.Add(dependency);
            }

            return packages;
        }
    }
}
