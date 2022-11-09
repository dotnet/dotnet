// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging.Xml
{
    internal static class PackageMetadataXmlExtensions
    {
        private const string References = "references";
        private const string Reference = "reference";
        private const string Group = "group";
        private const string File = "file";
        private const string TargetFramework = "targetFramework";
        private const string FrameworkAssemblies = "frameworkAssemblies";
        private const string FrameworkAssembly = "frameworkAssembly";
        private const string AssemblyName = "assemblyName";
        private const string Dependencies = "dependencies";
        private const string Files = "files";

        public static XElement ToXElement(this ManifestMetadata metadata, XNamespace ns)
        {
            return ToXElement(metadata, ns, generateBackwardsCompatible: true);
        }

        public static XElement ToXElement(this ManifestMetadata metadata, XNamespace ns, bool generateBackwardsCompatible = true)
        {
            var elem = new XElement(ns + "metadata");
            if (metadata.MinClientVersionString != null)
            {
                elem.SetAttributeValue("minClientVersion", metadata.MinClientVersionString);
            }

            elem.Add(new XElement(ns + "id", metadata.Id));
            AddElementIfNotNull(elem, ns, "version", metadata.Version?.ToFullString());
            AddElementIfNotNull(elem, ns, "title", metadata.Title);
            if (!metadata.PackageTypes.Contains(PackageType.SymbolsPackage))
            {
                AddElementIfNotNull(elem, ns, "authors", metadata.Authors, authors => string.Join(",", authors));
                AddElementIfNotEmpty(elem, ns, "owners", metadata.Owners, owners => string.Join(",", owners));
            }
            if (metadata.DevelopmentDependency)
            {
                elem.Add(new XElement(ns + "developmentDependency", metadata.DevelopmentDependency));
            }
            if (!metadata.PackageTypes.Contains(PackageType.SymbolsPackage))
            {
                if (metadata.EmitRequireLicenseAcceptance)
                {
                    elem.Add(new XElement(ns + "requireLicenseAcceptance", metadata.RequireLicenseAcceptance));
                }
                var licenseUrlToWrite = metadata.LicenseUrl?.ToString();
                if (metadata.LicenseMetadata != null)
                {
                    var licenseElement = GetXElementFromLicenseMetadata(ns, metadata.LicenseMetadata);
                    if (licenseElement != null)
                    {
                        elem.Add(licenseElement);
                    }
                    if (generateBackwardsCompatible)
                    {
                        licenseUrlToWrite = metadata.LicenseMetadata.LicenseUrl.OriginalString;
                    }
                }
                AddElementIfNotNull(elem, ns, "licenseUrl", licenseUrlToWrite);
                AddElementIfNotNull(elem, ns, "icon", metadata.Icon);
                AddElementIfNotNull(elem, ns, "readme", metadata.Readme);
            }
            AddElementIfNotNull(elem, ns, "projectUrl", metadata.ProjectUrl);
            AddElementIfNotNull(elem, ns, "iconUrl", metadata.IconUrl);
            AddElementIfNotNull(elem, ns, "description", metadata.Description);
            AddElementIfNotNull(elem, ns, "summary", metadata.Summary);
            AddElementIfNotNull(elem, ns, "releaseNotes", metadata.ReleaseNotes);
            AddElementIfNotNull(elem, ns, "copyright", metadata.Copyright);
            AddElementIfNotNull(elem, ns, "language", metadata.Language);
            AddElementIfNotNull(elem, ns, "tags", metadata.Tags);
            if (metadata.Serviceable)
            {
                elem.Add(new XElement(ns + "serviceable", metadata.Serviceable));
            }

            if (metadata.PackageTypes != null && metadata.PackageTypes.Any())
            {
                elem.Add(GetXElementFromManifestPackageTypes(ns, metadata.PackageTypes));
            }

            if (metadata.Repository != null)
            {
                var repoElement = GetXElementFromManifestRepository(ns, metadata.Repository);
                if (repoElement != null)
                {
                    elem.Add(repoElement);
                }
            }

            elem.Add(GetXElementFromGroupableItemSets(
                ns,
                metadata.DependencyGroups,
                set => set.TargetFramework.IsSpecificFramework ||
                       set.Packages.Any(dependency => dependency.Exclude.Count > 0 || dependency.Include.Count > 0),
                set => set.TargetFramework.IsSpecificFramework ? set.TargetFramework.GetFrameworkString() : null,
                set => set.Packages,
                GetXElementFromPackageDependency,
                Dependencies,
                TargetFramework));

            elem.Add(GetXElementFromGroupableItemSets(
                ns,
                metadata.PackageAssemblyReferences,
                set => set.TargetFramework?.IsSpecificFramework == true,
                set => set.TargetFramework?.GetFrameworkString(),
                set => set.References,
                GetXElementFromPackageReference,
                References,
                TargetFramework));

            elem.Add(GetXElementFromGroupableItemSets(
                ns: ns,
                objectSets: metadata.FrameworkReferenceGroups,
                isGroupable: set => true, // the TFM is required for framework references
                getGroupIdentifer: set => set.TargetFramework.GetFrameworkString(),
                getItems: set => set.FrameworkReferences,
                getXElementFromItem: GetXElementFromFrameworkReference,
                parentName: NuspecUtility.FrameworkReferences,
                identifierAttributeName: TargetFramework));

            elem.Add(GetXElementFromFrameworkAssemblies(ns, metadata.FrameworkReferences));
            elem.Add(GetXElementFromManifestContentFiles(ns, metadata.ContentFiles));

            return elem;
        }

        private static XElement GetXElementFromGroupableItemSets<TSet, TItem>(
            XNamespace ns,
            IEnumerable<TSet> objectSets,
            Func<TSet, bool> isGroupable,
            Func<TSet, string> getGroupIdentifer,
            Func<TSet, IEnumerable<TItem>> getItems,
            Func<XNamespace, TItem, XElement> getXElementFromItem,
            string parentName,
            string identifierAttributeName)
        {
            if (objectSets == null || !objectSets.Any())
            {
                return null;
            }

            var groupableSets = new List<TSet>();
            var ungroupableSets = new List<TSet>();

            foreach (var set in objectSets)
            {
                if (isGroupable(set))
                {
                    groupableSets.Add(set);
                }
                else
                {
                    ungroupableSets.Add(set);
                }
            }

            var childElements = new List<XElement>();
            if (!groupableSets.Any())
            {
                // none of the item sets are groupable, then flatten the items
                childElements.AddRange(objectSets.SelectMany(getItems).Select(item => getXElementFromItem(ns, item)));
            }
            else
            {
                // move the group with null target framework (if any) to the front just for nicer display in UI
                foreach (var set in ungroupableSets.Concat(groupableSets))
                {
                    var groupElem = new XElement(
                        ns + Group,
                        getItems(set).Select(item => getXElementFromItem(ns, item)).ToArray());

                    if (isGroupable(set))
                    {
                        var groupIdentifier = getGroupIdentifer(set);
                        if (groupIdentifier != null)
                        {
                            groupElem.SetAttributeValue(identifierAttributeName, groupIdentifier);
                        }
                    }

                    childElements.Add(groupElem);
                }
            }

            return new XElement(ns + parentName, childElements.ToArray());
        }

        private static XElement GetXElementFromFrameworkReference(XNamespace ns, FrameworkReference frameworkReference)
        {
            return new XElement(ns + NuspecUtility.FrameworkReference, new XAttribute(NuspecUtility.Name, frameworkReference.Name));
        }

        private static XElement GetXElementFromPackageReference(XNamespace ns, string reference)
        {
            return new XElement(ns + Reference, new XAttribute(File, reference));
        }

        private static XElement GetXElementFromPackageDependency(XNamespace ns, PackageDependency dependency)
        {
            var attributes = new List<XAttribute>();

            attributes.Add(new XAttribute("id", dependency.Id));

            if (dependency.VersionRange != null && dependency.VersionRange != VersionRange.All)
            {
                attributes.Add(new XAttribute("version", dependency.VersionRange.ToLegacyShortString()));
            }

            if (dependency.Include != null && dependency.Include.Any())
            {
                attributes.Add(new XAttribute("include", string.Join(",", dependency.Include)));
            }

            if (dependency.Exclude != null && dependency.Exclude.Any())
            {
                attributes.Add(new XAttribute("exclude", string.Join(",", dependency.Exclude)));
            }

            return new XElement(ns + "dependency", attributes);
        }

        private static XElement GetXElementFromFrameworkAssemblies(XNamespace ns, IEnumerable<FrameworkAssemblyReference> references)
        {
            if (references == null || !references.Any())
            {
                return null;
            }

            return new XElement(
                ns + FrameworkAssemblies,
                references.Select(reference =>
                    new XElement(ns + FrameworkAssembly,
                        new XAttribute(AssemblyName, reference.AssemblyName),
                        reference.SupportedFrameworks != null && reference.SupportedFrameworks.Any() ?
                            new XAttribute("targetFramework", string.Join(", ", reference.SupportedFrameworks.Where(f => f.IsSpecificFramework).Select(f => f.GetFrameworkString()))) :
                            null)));
        }

        private static XElement GetXElementFromManifestContentFiles(XNamespace ns, IEnumerable<ManifestContentFiles> contentFiles)
        {
            if (contentFiles == null || !contentFiles.Any())
            {
                return null;
            }

            return new XElement(ns + "contentFiles",
                contentFiles.Select(file => GetXElementFromManifestContentFile(ns, file)));
        }

        private static XElement GetXElementFromManifestContentFile(XNamespace ns, ManifestContentFiles file)
        {
            var attributes = new List<XAttribute>();

            attributes.Add(GetXAttributeFromNameAndValue("include", file.Include));
            attributes.Add(GetXAttributeFromNameAndValue("exclude", file.Exclude));
            attributes.Add(GetXAttributeFromNameAndValue("buildAction", file.BuildAction));
            attributes.Add(GetXAttributeFromNameAndValue("copyToOutput", file.CopyToOutput));
            attributes.Add(GetXAttributeFromNameAndValue("flatten", file.Flatten));

            attributes = attributes.Where(xAtt => xAtt != null).ToList();

            return new XElement(ns + Files, attributes);
        }

        private static XElement GetXElementFromLicenseMetadata(XNamespace ns, LicenseMetadata metadata)
        {
            var attributes = new List<XAttribute>();

            attributes.Add(GetXAttributeFromNameAndValue(NuspecUtility.Type, metadata.Type.ToString().ToLowerInvariant()));
            if (!metadata.Version.Equals(LicenseMetadata.EmptyVersion))
            {
                attributes.Add(GetXAttributeFromNameAndValue(NuspecUtility.Version, metadata.Version));
            }
            attributes = attributes.Where(xAtt => xAtt != null).ToList();

            return new XElement(ns + NuspecUtility.License, metadata.License, attributes);
        }

        private static XElement GetXElementFromManifestRepository(XNamespace ns, RepositoryMetadata repository)
        {
            var attributeList = new List<XAttribute>();
            if (repository != null && !string.IsNullOrEmpty(repository.Type))
            {
                attributeList.Add(new XAttribute(NuspecUtility.Type, repository.Type));
            }

            if (repository != null && !string.IsNullOrEmpty(repository.Url))
            {
                attributeList.Add(new XAttribute(NuspecUtility.RepositoryUrl, repository.Url));
            }

            if (!string.IsNullOrEmpty(repository?.Branch))
            {
                attributeList.Add(new XAttribute(NuspecUtility.RepositoryBranch, repository.Branch));
            }

            if (!string.IsNullOrEmpty(repository?.Commit))
            {
                attributeList.Add(new XAttribute(NuspecUtility.RepositoryCommit, repository.Commit));
            }

            if (attributeList.Count > 0)
            {
                return new XElement(ns + NuspecUtility.Repository, attributeList);
            }
            return null;
        }

        private static XElement GetXElementFromManifestPackageTypes(XNamespace ns, IEnumerable<PackageType> packageTypes)
        {
            var packageTypesElement = new XElement(ns + NuspecUtility.PackageTypes);

            foreach (var packageType in packageTypes)
            {
                var packageTypeElement = GetXElementFromManifestPackageType(ns, packageType);
                packageTypesElement.Add(packageTypeElement);
            }

            return packageTypesElement;
        }

        private static XElement GetXElementFromManifestPackageType(XNamespace ns, PackageType packageType)
        {
            var attributes = new List<XAttribute>();

            attributes.Add(GetXAttributeFromNameAndValue(NuspecUtility.Name, packageType.Name));
            if (packageType.Version != PackageType.EmptyVersion)
            {
                attributes.Add(GetXAttributeFromNameAndValue(NuspecUtility.Version, packageType.Version));
            }

            attributes = attributes.Where(xAtt => xAtt != null).ToList();

            return new XElement(ns + NuspecUtility.PackageType, attributes);
        }

        private static XAttribute GetXAttributeFromNameAndValue(string name, object value)
        {
            if (name == null || value == null)
            {
                return null;
            }

            return new XAttribute(name, value);
        }

        private static void AddElementIfNotNull<T>(XElement parent, XNamespace ns, string name, T value)
            where T : class
        {
            if (value != null)
            {
                parent.Add(new XElement(ns + name, value));
            }
        }

        private static void AddElementIfNotNull<T>(XElement parent, XNamespace ns, string name, T value, Func<T, object> process)
            where T : class
        {
            if (value != null)
            {
                var processed = process(value);
                if (processed != null)
                {
                    parent.Add(new XElement(ns + name, processed));
                }
            }
        }

        private static void AddElementIfNotEmpty<T>(XElement parent, XNamespace ns, string name, IEnumerable<T> value, Func<IEnumerable<T>, object> process)
        {
            if (value.Any())
            {
                var processed = process(value);
                if (processed != null)
                {
                    parent.Add(new XElement(ns + name, processed));
                }
            }
        }
    }
}
