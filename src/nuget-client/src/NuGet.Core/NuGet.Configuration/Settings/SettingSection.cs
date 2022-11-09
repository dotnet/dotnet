// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    public abstract class SettingSection : SettingsGroup<SettingItem>
    {
        private string _elementName;
        public override string ElementName
        {
            get => XmlConvert.DecodeName(_elementName);
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PropertyCannotBeNullOrEmpty, nameof(ElementName)));
                }

                _elementName = XmlUtility.GetEncodedXMLName(value);
            }
        }

        public IReadOnlyCollection<SettingItem> Items => Children.ToList();

        public T GetFirstItemWithAttribute<T>(string attributeName, string expectedAttributeValue) where T : SettingItem
        {
            return Items.OfType<T>().FirstOrDefault(c =>
                c.Attributes.TryGetValue(attributeName, out var attributeValue) &&
                string.Equals(attributeValue, expectedAttributeValue, StringComparison.OrdinalIgnoreCase));
        }

        protected SettingSection(string name, IReadOnlyDictionary<string, string> attributes, IEnumerable<SettingItem> children)
            : base(attributes, children)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            ElementName = name;
        }

        internal SettingSection(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        internal bool Update(SettingItem item)
        {
            if (item == null || (Origin != null && Origin.IsReadOnly))
            {
                return false;
            }

            if (TryGetChild(item, out var currentChild))
            {
                if (currentChild.Origin != null && currentChild.Origin.IsReadOnly)
                {
                    return false;
                }

                currentChild.Update(item);

                return true;
            }

            return false;
        }

        public override bool Equals(object other)
        {
            var section = other as SettingSection;

            if (section == null)
            {
                return false;
            }

            if (ReferenceEquals(this, section))
            {
                return true;
            }

            return string.Equals(ElementName, section.ElementName, StringComparison.Ordinal);
        }

        public override int GetHashCode() => ElementName.GetHashCode();
    }
}
