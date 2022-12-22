// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public abstract class SettingItem : SettingElement
    {
        protected virtual bool CanHaveChildren => false;

        internal SettingItem MergedWith { get; set; }

        protected SettingItem()
            : base()
        {
        }

        protected SettingItem(IReadOnlyDictionary<string, string> attributes)
            : base(attributes)
        {
        }

        internal SettingItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            if (!CanHaveChildren && element.HasElements)
            {
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.ShowError_CannotHaveChildren, element.Name.LocalName, origin.ConfigFilePath));
            }
        }

        /// <remarks>
        /// This method is internal because it updates directly the xElement behind this abstraction.
        /// It should only be called whenever the underlying config file is intended to be changed.
        /// To persist changes to disk one must save the corresponding setting files
        /// </remarks>
        internal virtual void Update(SettingItem setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (Origin != null && Origin.IsMachineWide)
            {
                throw new InvalidOperationException(Resources.CannotUpdateMachineWide);
            }

            if (Origin != null && Origin.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.CannotUpdateReadOnlyConfig);
            }

            if (setting.GetType() != GetType())
            {
                throw new InvalidOperationException(Resources.CannotUpdateDifferentItems);
            }

            var xElement = Node as XElement;
            var otherAttributes = setting.Attributes.ToDictionary(a => a.Key, a => a.Value);
            var attributesImmutable = new Dictionary<string, string>(MutableAttributes);
            foreach (var attribute in attributesImmutable)
            {
                if (otherAttributes.TryGetValue(attribute.Key, out var otherValue))
                {
                    otherAttributes.Remove(attribute.Key);
                }

                string value = null;
                if (otherValue != null)
                {
                    value = otherValue;
                }

                if (!string.Equals(value, attribute.Value, StringComparison.Ordinal))
                {
                    if (xElement != null && Origin != null)
                    {
                        // Update or remove any existing item that has changed
                        xElement.SetAttributeValue(attribute.Key, value);
                        Origin.IsDirty = true;
                    }

                    AddOrUpdateAttribute(attribute.Key, value);
                }
            }

            foreach (var attribute in otherAttributes)
            {
                if (xElement != null && Origin != null)
                {
                    xElement.SetAttributeValue(attribute.Key, attribute.Value);
                    Origin.IsDirty = true;
                }

                AddOrUpdateAttribute(attribute.Key, attribute.Value);
            }
        }
    }
}
