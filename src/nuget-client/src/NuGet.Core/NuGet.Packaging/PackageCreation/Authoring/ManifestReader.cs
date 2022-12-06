// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Packaging.PackageCreation.Resources;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    internal static class ManifestReader
    {
        private static readonly string[] RequiredElements = new string[] { "id", "version", "authors", "description" };

        public static Manifest ReadManifest(XDocument document)
        {
            var metadataElement = document.Root.ElementsNoNamespace("metadata").FirstOrDefault();
            if (metadataElement == null)
            {
                throw new InvalidDataException(
                    string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredElementMissing, "metadata"));
            }

            return new Manifest(
                ReadMetadata(metadataElement),
                ReadFilesList(document.Root.ElementsNoNamespace("files").FirstOrDefault()));
        }

        private static ManifestMetadata ReadMetadata(XElement xElement)
        {
            var manifestMetadata = new ManifestMetadata();
            manifestMetadata.MinClientVersionString = (string)xElement.Attribute("minClientVersion");

            // we store all child elements under <metadata> so that we can easily check for required elements.
            var allElements = new HashSet<string>();

            foreach (var element in xElement.Elements())
            {
                ReadMetadataValue(manifestMetadata, element, allElements);
            }

            manifestMetadata.PackageTypes = NuspecUtility.GetPackageTypes(xElement, useMetadataNamespace: false);

            // now check for required elements, which include <id>, <version>, <authors> and <description>
            foreach (var requiredElement in RequiredElements)
            {
                if (requiredElement.Equals("authors", StringComparison.Ordinal) && manifestMetadata.PackageTypes.Contains(PackageType.SymbolsPackage))
                {
                    continue;
                }
                if (!allElements.Contains(requiredElement))
                {
                    throw new InvalidDataException(
                        string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredElementMissing, requiredElement));
                }
            }

            return manifestMetadata;
        }

        private static void ReadMetadataValue(ManifestMetadata manifestMetadata, XElement element, HashSet<string> allElements)
        {
            if (element.Value == null)
            {
                return;
            }

            allElements.Add(element.Name.LocalName);

            string value = null;
            try
            {
                value = element.Value.SafeTrim();
                switch (element.Name.LocalName)
                {
                    case "id":
                        manifestMetadata.Id = value;
                        break;
                    case "version":
                        manifestMetadata.Version = NuGetVersion.Parse(value);
                        break;
                    case "authors":
                        manifestMetadata.Authors = value?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "owners":
                        manifestMetadata.Owners = value?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "licenseUrl":
                        manifestMetadata.SetLicenseUrl(value);
                        break;
                    case "projectUrl":
                        manifestMetadata.SetProjectUrl(value);
                        break;
                    case "iconUrl":
                        manifestMetadata.SetIconUrl(value);
                        break;
                    case "requireLicenseAcceptance":
                        manifestMetadata.RequireLicenseAcceptance = XmlConvert.ToBoolean(value);
                        break;
                    case "developmentDependency":
                        manifestMetadata.DevelopmentDependency = XmlConvert.ToBoolean(value);
                        break;
                    case "description":
                        manifestMetadata.Description = value;
                        break;
                    case "summary":
                        manifestMetadata.Summary = value;
                        break;
                    case "releaseNotes":
                        manifestMetadata.ReleaseNotes = value;
                        break;
                    case "copyright":
                        manifestMetadata.Copyright = value;
                        break;
                    case "language":
                        manifestMetadata.Language = value;
                        break;
                    case "title":
                        manifestMetadata.Title = value;
                        break;
                    case "tags":
                        manifestMetadata.Tags = value;
                        break;
                    case "readme":
                        manifestMetadata.Readme = value;
                        break;
                    case "serviceable":
                        manifestMetadata.Serviceable = XmlConvert.ToBoolean(value);
                        break;
                    case "dependencies":
                        manifestMetadata.DependencyGroups = ReadDependencyGroups(element);
                        break;
                    case "frameworkAssemblies":
                        manifestMetadata.FrameworkReferences = ReadFrameworkAssemblies(element);
                        break;
                    case "frameworkReferences":
                        manifestMetadata.FrameworkReferenceGroups = ReadFrameworkReferenceGroups(element);
                        break;
                    case "references":
                        manifestMetadata.PackageAssemblyReferences = ReadReferenceSets(element);
                        break;
                    case "contentFiles":
                        manifestMetadata.ContentFiles = ReadContentFiles(element);
                        break;
                    case "repository":
                        manifestMetadata.Repository = ReadRepository(element);
                        break;
                    case "license":
                        manifestMetadata.LicenseMetadata = ReadLicenseMetadata(element);
                        break;
                    case "icon":
                        manifestMetadata.Icon = value;
                        break;
                }
            }
            catch (Exception ex) when (!(ex is InvalidDataException))
            {
                // Wrap the exception to pinpoint the exact property that is problematic,
                // and include a hint about replacement tokens.
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_PropertyValueReadFailure, value, element.Name.LocalName), ex);
            }
        }

        private static IEnumerable<FrameworkReferenceGroup> ReadFrameworkReferenceGroups(XElement frameworkReferenceGroupsElement)
        {
            return NuspecUtility.GetFrameworkReferenceGroups(frameworkReferenceGroupsElement, DefaultFrameworkNameProvider.Instance, useMetadataNamespace: false);
        }

        private static LicenseMetadata ReadLicenseMetadata(XElement licenseNode)
        {
            var type = licenseNode.Attribute(NuspecUtility.Type).Value.SafeTrim();
            var license = licenseNode.Value.SafeTrim();
            var versionValue = licenseNode.Attribute(NuspecUtility.Version)?.Value.SafeTrim();

            if (!Enum.TryParse(type, ignoreCase: true, result: out LicenseType licenseType))
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicense_InvalidLicenseType, type));
            }

            if (string.IsNullOrWhiteSpace(license))
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicense_MissingRequiredValue));
            }

            Version version = null;
            if (string.IsNullOrWhiteSpace(versionValue))
            {
                version = LicenseMetadata.EmptyVersion;
            }
            else
            {
                if (!Version.TryParse(versionValue, out version))
                {
                    throw new PackagingException(NuGetLogCode.NU5034, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.NuGetLicense_InvalidLicenseExpressionVersion,
                        versionValue));
                }
            }

            switch (licenseType)
            {
                case LicenseType.Expression:

                    if (version.CompareTo(LicenseMetadata.CurrentVersion) <= 0)
                    {
                        try
                        {
                            var expression = NuGetLicenseExpression.Parse(license);
                            return new LicenseMetadata(licenseType, license, expression, warningsAndErrors: null, version: version);
                        }
                        catch (NuGetLicenseExpressionParsingException e)
                        {
                            throw new PackagingException(NuGetLogCode.NU5032, e.Message);
                        }

                    }
                    throw new PackagingException(NuGetLogCode.NU5034, string.Format(
                                   CultureInfo.CurrentCulture,
                                   Strings.InvalidLicenseExppressionVersion_VersionTooHigh,
                                   versionValue,
                                   LicenseMetadata.CurrentVersion));

                case LicenseType.File:
                    return new LicenseMetadata(type: licenseType, license: license, expression: null, warningsAndErrors: null, version: LicenseMetadata.EmptyVersion);

                default:
                    throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicense_InvalidLicenseType, type));
            }
        }

        private static List<ManifestContentFiles> ReadContentFiles(XElement contentFilesElement)
        {
            if (!contentFilesElement.HasElements)
            {
                return new List<ManifestContentFiles>(0);
            }

            var contentFileSets = (from element in contentFilesElement.ElementsNoNamespace("files")
                                   let includeAttribute = element.Attribute("include")
                                   where includeAttribute != null && !string.IsNullOrEmpty(includeAttribute.Value)
                                   let excludeAttribute = element.Attribute("exclude")
                                   let buildActionAttribute = element.Attribute("buildAction")
                                   let copyToOutputAttribute = element.Attribute("copyToOutput")
                                   let flattenAttribute = element.Attribute("flatten")
                                   select new ManifestContentFiles
                                   {
                                       Include = includeAttribute.Value.SafeTrim(),
                                       Exclude = excludeAttribute == null ? null : excludeAttribute.Value,
                                       BuildAction = buildActionAttribute == null ? null : buildActionAttribute.Value,
                                       CopyToOutput = copyToOutputAttribute == null ? null : copyToOutputAttribute.Value,
                                       Flatten = flattenAttribute == null ? null : flattenAttribute.Value
                                   }).ToList();

            return contentFileSets;
        }

        private static List<PackageReferenceSet> ReadReferenceSets(XElement referencesElement)
        {
            if (!referencesElement.HasElements)
            {
                return new List<PackageReferenceSet>(0);
            }

            if (referencesElement.ElementsNoNamespace("group").Any() &&
                referencesElement.ElementsNoNamespace("reference").Any())
            {
                throw new InvalidDataException(NuGetResources.Manifest_ReferencesHasMixedElements);
            }

            var references = ReadReference(referencesElement, throwIfEmpty: false);
            if (references.Count > 0)
            {
                // old format, <reference> is direct child of <references>
                var referenceSet = new PackageReferenceSet(references);
                return new List<PackageReferenceSet> { referenceSet };
            }
            else
            {
                var groups = referencesElement.ElementsNoNamespace("group");
                return (from element in groups
                        select new PackageReferenceSet(element.GetOptionalAttributeValue("targetFramework")?.Trim(),
                            ReadReference(element, throwIfEmpty: true))).ToList();
            }
        }

        public static List<string> ReadReference(XElement referenceElement, bool throwIfEmpty)
        {
            var references = referenceElement.ElementsNoNamespace("reference")
                                             .Select(element => ((string)element.Attribute("file"))?.Trim())
                                             .Where(file => file != null)
                                             .ToList();

            if (throwIfEmpty && references.Count == 0)
            {
                throw new InvalidDataException(NuGetResources.Manifest_ReferencesIsEmpty);
            }

            return references;
        }

        private static List<FrameworkAssemblyReference> ReadFrameworkAssemblies(XElement frameworkElement)
        {
            if (!frameworkElement.HasElements)
            {
                return new List<FrameworkAssemblyReference>(0);
            }

            return (from element in frameworkElement.ElementsNoNamespace("frameworkAssembly")
                    let assemblyNameAttribute = element.Attribute("assemblyName")
                    where assemblyNameAttribute != null && !string.IsNullOrEmpty(assemblyNameAttribute.Value)
                    select new FrameworkAssemblyReference(assemblyNameAttribute.Value?.Trim(),
                        string.IsNullOrEmpty(element.GetOptionalAttributeValue("targetFramework")) ?
                        new[] { NuGetFramework.AnyFramework } :
                        new[] { NuGetFramework.Parse(element.GetOptionalAttributeValue("targetFramework")?.Trim()) })
                    ).ToList();
        }

        private static List<PackageDependencyGroup> ReadDependencyGroups(XElement dependenciesElement)
        {
            if (!dependenciesElement.HasElements)
            {
                return new List<PackageDependencyGroup>();
            }

            // Disallow the <dependencies> element to contain both <dependency> and 
            // <group> child elements. Unfortunately, this cannot be enforced by XSD.
            if (dependenciesElement.ElementsNoNamespace("dependency").Any() &&
                dependenciesElement.ElementsNoNamespace("group").Any())
            {
                throw new InvalidDataException(NuGetResources.Manifest_DependenciesHasMixedElements);
            }

            var dependencies = ReadDependencies(dependenciesElement);
            if (dependencies.Any())
            {
                // old format, <dependency> is direct child of <dependencies>
                var dependencyGroup = new PackageDependencyGroup(NuGetFramework.AnyFramework, dependencies);
                return new List<PackageDependencyGroup> { dependencyGroup };
            }
            else
            {
                var groups = dependenciesElement.ElementsNoNamespace("group");

                return groups.Select(element =>
                {
                    var targetFrameworkName = element.GetOptionalAttributeValue("targetFramework")?.Trim();
                    NuGetFramework targetFramework = null;

                    if (targetFrameworkName != null)
                    {
                        targetFramework = NuGetFramework.Parse(targetFrameworkName);

                        if (targetFramework.IsUnsupported)
                        {
                            throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidTargetFramework, targetFrameworkName));
                        }
                    }

                    // REVIEW: Is UnsupportedFramework correct?
                    targetFramework = targetFramework ?? NuGetFramework.UnsupportedFramework;

                    return new PackageDependencyGroup(
                            targetFramework,
                            ReadDependencies(element));
                }).ToList();
            }
        }

        private static ISet<PackageDependency> ReadDependencies(XElement containerElement)
        {
            // element is <dependency>

            var dependency = (from element in containerElement.ElementsNoNamespace("dependency")
                              let idElement = element.Attribute("id")
                              where idElement != null && !string.IsNullOrEmpty(idElement.Value)
                              let elementVersion = element.GetOptionalAttributeValue("version")
                              select new PackageDependency(
                                  idElement.Value?.Trim(),
                                  // REVIEW: There isn't a PackageDependency constructor that allows me to pass in an invalid version
                                  elementVersion == null ? null : VersionRange.Parse(elementVersion.Trim()),
                                  element.GetOptionalAttributeValue("include")?.Trim()?.Split(',').Select(a => a.Trim()).ToArray(),
                                  element.GetOptionalAttributeValue("exclude")?.Trim()?.Split(',').Select(a => a.Trim()).ToArray()
                              )).ToList();
            return new HashSet<PackageDependency>(dependency);
        }

        private static List<ManifestFile> ReadFilesList(XElement xElement)
        {
            if (xElement == null)
            {
                return null;
            }

            var files = new List<ManifestFile>();
            foreach (var file in xElement.ElementsNoNamespace("file"))
            {
                var srcElement = file.Attribute("src");
                if (srcElement == null || string.IsNullOrEmpty(srcElement.Value))
                {
                    continue;
                }

                var slashes = new[] { '\\', '/' };
                var target = file.GetOptionalAttributeValue("target").SafeTrim()?.TrimStart(slashes);
                var exclude = file.GetOptionalAttributeValue("exclude").SafeTrim();

                // Multiple sources can be specified by using semi-colon separated values. 
                files.AddRange(srcElement.Value.Trim(';').Split(';').Select(s =>
                    new ManifestFile
                    {
                        Source = s.SafeTrim(),
                        Target = target,
                        Exclude = exclude
                    }));
            }
            return files;
        }

        private static RepositoryMetadata ReadRepository(XElement element)
        {
            var repositoryType = element.Attribute("type");
            var repositoryUrl = element.Attribute("url");
            var repositoryBranch = element.Attribute("branch");
            var repositoryCommit = element.Attribute("commit");
            var repository = new RepositoryMetadata();
            if (!string.IsNullOrEmpty(repositoryType?.Value))
            {
                repository.Type = repositoryType.Value;
            }
            if (!string.IsNullOrEmpty(repositoryUrl?.Value))
            {
                repository.Url = repositoryUrl.Value;
                repository.Branch = repositoryBranch?.Value;
                repository.Commit = repositoryCommit?.Value;
            }

            // Ensure the value is valid before returning it.
            if (!string.IsNullOrEmpty(repository.Type) && !string.IsNullOrEmpty(repository.Url))
            {
                return repository;
            }

            return null;
        }
    }
}
