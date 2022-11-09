// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    internal static class SettingFactory
    {
        internal static SettingBase Parse(XNode node, SettingsFile origin)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node is XText textNode)
            {
                return new SettingText(textNode, origin);
            }

            if (node is XElement element)
            {
                var elementType = SettingElementType.Unknown;
                Enum.TryParse(element.Name.LocalName, ignoreCase: true, result: out elementType);

                var parentType = SettingElementType.Unknown;
                if (element.Parent != null)
                {
                    Enum.TryParse(element.Parent?.Name.LocalName, ignoreCase: true, result: out parentType);
                }

                switch (parentType)
                {
                    case SettingElementType.Configuration:
                        return new ParsedSettingSection(element, origin);

                    case SettingElementType.PackageSourceCredentials:
                        return new CredentialsItem(element, origin);

                    case SettingElementType.PackageSources:
                        if (elementType == SettingElementType.Add)
                        {
                            return new SourceItem(element, origin);
                        }
                        break;

                    case SettingElementType.PackageSourceMapping:
                        if (elementType == SettingElementType.PackageSource)
                        {
                            return new PackageSourceMappingSourceItem(element, origin);
                        }
                        break;

                    case SettingElementType.PackageSource:
                        if (elementType == SettingElementType.Package)
                        {
                            return new PackagePatternItem(element, origin);
                        }
                        break;
                }

                switch (elementType)
                {
                    case SettingElementType.Add:
                        return new AddItem(element, origin);

                    case SettingElementType.Author:
                        return new AuthorItem(element, origin);

                    case SettingElementType.Certificate:
                        return new CertificateItem(element, origin);

                    case SettingElementType.Clear:
                        return new ClearItem(element, origin);

                    case SettingElementType.Owners:
                        return new OwnersItem(element, origin);

                    case SettingElementType.Repository:
                        return new RepositoryItem(element, origin);

                    case SettingElementType.FileCert:
                        return new FileClientCertItem(element, origin);

                    case SettingElementType.StoreCert:
                        return new StoreClientCertItem(element, origin);
                }

                return new UnknownItem(element, origin);
            }

            return null;
        }

        private class SettingElementKeyComparer : IComparer<SettingElement>, IEqualityComparer<SettingElement>
        {
            public int Compare(SettingElement x, SettingElement y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (ReferenceEquals(x, null))
                {
                    return -1;
                }

                if (ReferenceEquals(y, null))
                {
                    return 1;
                }

                return StringComparer.OrdinalIgnoreCase.Compare(
                    x.ElementName + string.Join("", x.Attributes.Select(a => a.Value)),
                    y.ElementName + string.Join("", y.Attributes.Select(a => a.Value)));
            }

            public bool Equals(SettingElement x, SettingElement y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return StringComparer.OrdinalIgnoreCase.Equals(
                    x.ElementName + string.Join("", x.Attributes.Select(a => a.Value)),
                    y.ElementName + string.Join("", y.Attributes.Select(a => a.Value)));
            }

            public int GetHashCode(SettingElement obj)
            {
                if (ReferenceEquals(obj, null))
                {
                    return 0;
                }

                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.ElementName + string.Join("", obj.Attributes.Select(a => a.Value)));
            }
        }

        internal static IEnumerable<T> ParseChildren<T>(XElement xElement, SettingsFile origin, bool canBeCleared) where T : SettingElement
        {
            var children = new List<T>();
            IEnumerable<T> descendants = xElement.Elements().Select(d => Parse(d, origin)).OfType<T>();
            SettingElementKeyComparer comparer = new SettingElementKeyComparer();

            HashSet<T> distinctDescendants = new HashSet<T>(comparer);

            List<T> duplicatedDescendants = null;

            foreach (var item in descendants)
            {
                if (!distinctDescendants.Add(item))
                {
                    if (duplicatedDescendants == null)
                    {
                        duplicatedDescendants = new List<T>();
                    }

                    duplicatedDescendants.Add(item);
                }
            }

            if (xElement.Name.LocalName.Equals(ConfigurationConstants.PackageSourceMapping, StringComparison.OrdinalIgnoreCase) && duplicatedDescendants != null)
            {
                var duplicatedKey = string.Join(", ", duplicatedDescendants.Select(d => d.Attributes["key"]));
                var source = duplicatedDescendants.Select(d => d.Origin.ConfigFilePath).First();
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_DuplicatePackageSource, duplicatedKey, source));
            }

            foreach (var descendant in distinctDescendants)
            {
                if (canBeCleared && descendant is ClearItem)
                {
                    children.Clear();
                }

                children.Add(descendant);
            }

            return children;
        }
    }
}
