// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public abstract class SettingElement : SettingBase
    {
        /// <summary>
        /// Text that differentiates element tag
        /// </summary>
        public abstract string ElementName { get; }

        /// <summary>
        /// Specifies the keys for the attributes that the element can have
        /// </summary>
        /// <remarks>If null then all attributes are allowed</remarks>
        protected virtual IReadOnlyCollection<string>? AllowedAttributes { get; } = null;

        /// <summary>
        /// Specifies the keys for the attributes that the element should have
        /// </summary>
        /// <remarks>If null or empty then no attributes are required</remarks>
        protected virtual IReadOnlyCollection<string>? RequiredAttributes { get; } = null;

        /// <summary>
        /// Specifies which values are allowed for a specific attribute.
        /// If an attribute is not defined every value is allowed.
        /// Having allowed values does not imply that the attribute is required.
        /// </summary>
        protected virtual IReadOnlyDictionary<string, IReadOnlyCollection<string>>? AllowedValues { get; } = null;

        /// <summary>
        /// Specifies values that are explicitely disallowed for a specific attribute.
        /// If an attribute is not defined no value is disallowed.
        /// Having disallowed values does not imply that the attribute is required.
        /// </summary>
        protected virtual IReadOnlyDictionary<string, IReadOnlyCollection<string>>? DisallowedValues { get; } = null;

        /// <summary>
        ///  Key-value pairs that give more information about the element
        /// </summary>
        protected Dictionary<string, string> MutableAttributes { get; }

        /// <summary>
        /// Read only key-value pairs that give more information about the element
        /// </summary>
        internal IReadOnlyDictionary<string, string> Attributes => MutableAttributes;

        /// <summary>
        /// Specifies if the element is empty.
        /// Each element defines its own definition of empty.
        /// The default definition of empty is an element without attributes.
        /// </summary>
        public override bool IsEmpty() => !Attributes.Any();

        /// <summary>
        /// Settings file path file of the element.
        /// </summary>
        public string? ConfigPath => Origin?.ConfigFilePath;

        /// <summary>
        /// Default constructor
        /// </summary>
        protected SettingElement()
            : base()
        {
            MutableAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        protected SettingElement(IReadOnlyDictionary<string, string>? attributes)
            : this()
        {
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    if (IsAttributeValid(attribute.Key, attribute.Value))
                    {
                        MutableAttributes.Add(attribute.Key, attribute.Value);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_InvalidAttribute, attribute.Key, attribute.Value));
                    }
                }
            }
        }

        /// <summary>
        /// Constructor used when element is read from a file
        /// </summary>
        /// <param name="element">Xelement read from XML file document tree</param>
        /// <param name="origin">Settings file that this element was read from</param>
        internal SettingElement(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            ValidateAttributes(element, origin);

            MutableAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var existingAttribute in element.Attributes())
            {
                MutableAttributes.Add(existingAttribute.Name.LocalName, existingAttribute.Value);
            }
        }

        internal override XNode AsXNode()
        {
            if (Node != null && Node is XElement)
            {
                return Node;
            }

            var element = new XElement(ElementName);

            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        protected void AddOrUpdateAttribute(string attributeName, string? value)
        {
            if (!UpdateAttribute(attributeName, value))
            {
                if (value != null)
                {
                    AddAttribute(attributeName, value);
                }
            }
        }

        internal bool UpdateAttribute(string attributeName, string? newValue)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(attributeName));
            }

            if (!IsAttributeValid(attributeName, newValue))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_InvalidAttribute, attributeName, newValue));
            }

            if (Attributes.ContainsKey(attributeName))
            {
                if (newValue == null)
                {
                    MutableAttributes.Remove(attributeName);
                }
                else
                {
                    MutableAttributes[attributeName] = newValue;
                }

                return true;
            }

            return false;
        }

        protected void AddAttribute(string attributeName, string value)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(attributeName));
            }

            value = value ?? string.Empty;

            if (!IsAttributeValid(attributeName, value))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Error_InvalidAttribute, attributeName, value));
            }

            if (!Attributes.ContainsKey(attributeName))
            {
                MutableAttributes[attributeName] = value;
            }
        }

        private bool IsAttributeValid(string attributeName, string? value)
        {
            if (AllowedAttributes != null)
            {
                // No attributes are allowed
                if (!AllowedAttributes.Any())
                {
                    return false;
                }

                if (!AllowedAttributes.Contains(attributeName))
                {
                    return false;
                }
            }

            if (RequiredAttributes != null)
            {
                if (value == null && RequiredAttributes.Contains(attributeName))
                {
                    // Don't delete any required attributes
                    return false;
                }
            }

            if (AllowedValues != null)
            {
                if (AllowedValues.TryGetValue(attributeName, out var allowed) && (value is null || !allowed.Contains(value.Trim())))
                {
                    return false;
                }
            }

            if (DisallowedValues != null)
            {
                if (DisallowedValues.TryGetValue(attributeName, out var disallowed) && value is not null && disallowed.Contains(value.Trim()))
                {
                    return false;
                }
            }

            return true;
        }

        private void ValidateAttributes(XElement element, SettingsFile origin)
        {
            if (AllowedAttributes != null)
            {
                if (!AllowedAttributes.Any() && element.HasAttributes)
                {
                    throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                        string.Format(CultureInfo.CurrentCulture, Resources.NoAttributesAllowed, element.Name.LocalName, element.Attributes().Count()),
                        origin.ConfigFilePath));
                }

                foreach (var attribute in element.Attributes())
                {
                    if (!AllowedAttributes.Contains(attribute.Name.LocalName))
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                            string.Format(CultureInfo.CurrentCulture, Resources.AttributeNotAllowed, attribute.Name.LocalName, element.Name.LocalName),
                            origin.ConfigFilePath));
                    }
                }
            }

            if (RequiredAttributes != null)
            {
                foreach (var requireAttribute in RequiredAttributes)
                {
                    var attribute = element.Attribute(requireAttribute);
                    if (attribute == null)
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                            string.Format(CultureInfo.CurrentCulture, Resources.MissingRequiredAttribute, requireAttribute, element.Name.LocalName),
                            origin.ConfigFilePath));
                    }
                }
            }

            if (AllowedValues != null)
            {
                foreach (var attributeValues in AllowedValues)
                {
                    var attribute = element.Attribute(attributeValues.Key);
                    if (attribute != null && !attributeValues.Value.Contains(attribute.Value.Trim()))
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                              string.Format(CultureInfo.CurrentCulture, Resources.AttributeValueNotAllowed, attribute.Name.LocalName, attribute.Value.Trim(), element.Name.LocalName),
                            origin.ConfigFilePath));
                    }
                }
            }

            if (DisallowedValues != null)
            {
                foreach (var attributeValues in DisallowedValues)
                {
                    var attribute = element.Attribute(attributeValues.Key);
                    if (attribute != null && attributeValues.Value.Contains(attribute.Value.Trim()))
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                            string.Format(CultureInfo.CurrentCulture, Resources.AttributeValueNotAllowed, attribute.Name.LocalName, attribute.Value.Trim(), element.Name.LocalName),
                            origin.ConfigFilePath));
                    }
                }
            }
        }
    }
}
