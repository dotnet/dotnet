// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    /// <summary>
    /// A PackageSourceMappingSourceItem has only a key and at least 1 <see cref="PackagePatternItem"/> child item.
    ///     - [Required] Key
    /// </summary>
    public class PackageSourceMappingSourceItem : SettingItem
    {
        protected override bool CanHaveChildren => true;

        /// <summary>
        /// List of package pattern items part of this package source element.
        /// </summary>
        public IList<PackagePatternItem> Patterns { get; }

        public override string ElementName => ConfigurationConstants.PackageSourceAttribute;

        /// <summary>
        /// Each PackageSourceMappingSourceItem item needs a key.
        /// The key should correspond a package source key.
        /// </summary>
        public virtual string Key => Attributes[ConfigurationConstants.KeyAttribute];

        protected override IReadOnlyCollection<string> RequiredAttributes { get; } = new HashSet<string>(new[] { ConfigurationConstants.KeyAttribute });

        protected void SetKey(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PropertyCannotBeNullOrEmpty, nameof(Key)));
            }

            UpdateAttribute(ConfigurationConstants.KeyAttribute, value);
        }

        internal readonly IEnumerable<SettingBase> _parsedDescendants;

        /// <summary>
        /// Creates a package source mapping source item with the given name, which equals the key and non-empty list of package patters items.
        /// </summary>
        /// <param name="name">A non-empty name of the item which corresponds a package source name.</param>
        /// <param name="packagePatternItems">A non empty list of package pattern items.</param>
        /// <exception cref="ArgumentException">If <paramref name="name"/> is null or empty, or <paramref name="packagePatternItems"/> is null or empty.</exception>
        public PackageSourceMappingSourceItem(string name, IEnumerable<PackagePatternItem> packagePatternItems)
            : base()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            if (packagePatternItems == null || !packagePatternItems.Any())
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Error_ItemNeedsAtLeastOnePackagePattern, name));
            }

            AddAttribute(ConfigurationConstants.KeyAttribute, name);

            Patterns = new List<PackagePatternItem>();

            foreach (PackagePatternItem patternItem in packagePatternItems)
            {
                Patterns.Add(patternItem);
            }
        }

        internal PackageSourceMappingSourceItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            _parsedDescendants = element.Nodes().Where(n => n is XElement || n is XText text && !string.IsNullOrWhiteSpace(text.Value))
                .Select(e => SettingFactory.Parse(e, origin));

            var parsedPackagePatternItems = _parsedDescendants.OfType<PackagePatternItem>().ToList();

            if (parsedPackagePatternItems.Count == 0)
            {
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_ItemNeedsAtLeastOnePackagePatternWithPath, Key, origin.ConfigFilePath));
            }

            Patterns = parsedPackagePatternItems;
        }

        internal override void SetOrigin(SettingsFile origin)
        {
            base.SetOrigin(origin);

            foreach (PackagePatternItem packagePatternItem in Patterns)
            {
                packagePatternItem.SetOrigin(origin);
            }
        }

        internal override void RemoveFromSettings()
        {
            base.RemoveFromSettings();

            foreach (PackagePatternItem @namespace in Patterns)
            {
                @namespace.RemoveFromSettings();
            }
        }

        public override SettingBase Clone()
        {
            var newItem = new PackageSourceMappingSourceItem(
                Key,
                Patterns.Select(c => c.Clone() as PackagePatternItem).ToArray());

            if (Origin != null)
            {
                newItem.SetOrigin(Origin);
            }

            return newItem;
        }

        internal override XNode AsXNode()
        {
            if (Node is XElement)
            {
                return Node;
            }

            var element = new XElement(ElementName);

            foreach (PackagePatternItem packagePatternItem in Patterns)
            {
                element.Add(packagePatternItem.AsXNode());
            }

            foreach (KeyValuePair<string, string> attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        internal override void Update(SettingItem other)
        {
            var packageSourceMappingSourceItem = other as PackageSourceMappingSourceItem;

            if (!packageSourceMappingSourceItem.Patterns.Any())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_ItemNeedsAtLeastOnePackagePattern, packageSourceMappingSourceItem.Key));
            }

            base.Update(other);

            Dictionary<PackagePatternItem, PackagePatternItem> otherPatterns = packageSourceMappingSourceItem.Patterns.ToDictionary(c => c, c => c);
            var clonedPatterns = new List<PackagePatternItem>(Patterns);
            foreach (PackagePatternItem packagePatternItem in clonedPatterns)
            {
                if (otherPatterns.TryGetValue(packagePatternItem, out PackagePatternItem otherChild))
                {
                    otherPatterns.Remove(packagePatternItem);
                }

                if (otherChild == null)
                {
                    Patterns.Remove(packagePatternItem);
                    packagePatternItem.RemoveFromSettings();
                }
                else if (packagePatternItem is SettingItem item)
                {
                    item.Update(otherChild);
                }
            }

            foreach (KeyValuePair<PackagePatternItem, PackagePatternItem> newPackagePatternItem in otherPatterns)
            {
                var itemToAdd = newPackagePatternItem.Value;
                Patterns.Add(itemToAdd);

                if (Origin != null)
                {
                    itemToAdd.SetOrigin(Origin);

                    if (Node != null)
                    {
                        itemToAdd.SetNode(itemToAdd.AsXNode());

                        XElementUtility.AddIndented(Node as XElement, itemToAdd.Node);
                        Origin.IsDirty = true;
                    }
                }
            }
        }

        public override bool Equals(object other)
        {
            // It is important that equality on checks that the package source mapping source item is for the same `key. The content is not important. 
            // The equality here is used for updating patterns.
            if (other is PackageSourceMappingSourceItem item)
            {
                if (ReferenceEquals(this, item))
                {
                    return true;
                }

                return string.Equals(Key, item.Key, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(Key);

            return combiner.CombinedHash;
        }
    }
}
