// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    /// <summary>
    /// A CredentialsItem has a name and it can have between 2 or 3 children:
    ///     - [Required] Username (AddItem)
    ///     - [Required] Either Password or ClearTextPassword (AddItem)
    ///     - [Optional] ValidAuthenticationTypes (AddItem)
    /// </summary>
    public sealed class CredentialsItem : SettingItem
    {
        // The element name for credential items is the source name it's related to. But source names can have characters that are not allowed in XML element names.
        // For example, a source name "Package Source" will be encoded as "Package_x0020_Source" in the XML element name.
        // This property contains the decoded element name, and therefore needs to be encoded when it's written to the XML.
        public override string ElementName { get; }

        public string Username
        {
            get => _username.Value;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PropertyCannotBeNullOrEmpty, nameof(Username)));
                }

                _username.Value = value;
            }
        }

        public bool IsPasswordClearText { get; private set; }

        public string Password => _password.Value;

        public void UpdatePassword(string password, bool isPasswordClearText = true)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(password));
            }

            if (IsPasswordClearText && !isPasswordClearText)
            {
                _password.UpdateAttribute(ConfigurationConstants.KeyAttribute, ConfigurationConstants.PasswordToken);
            }
            else if (!IsPasswordClearText && isPasswordClearText)
            {
                _password.UpdateAttribute(ConfigurationConstants.KeyAttribute, ConfigurationConstants.ClearTextPasswordToken);
            }

            IsPasswordClearText = isPasswordClearText;

            if (!string.Equals(Password, password, StringComparison.Ordinal))
            {
                _password.Value = password;
            }
        }

        public string? ValidAuthenticationTypes
        {
            get => _validAuthenticationTypes?.Value;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _validAuthenticationTypes = null;
                }
                else
                {
                    if (_validAuthenticationTypes == null)
                    {
                        _validAuthenticationTypes = new AddItem(ConfigurationConstants.ValidAuthenticationTypesToken, value);

                        if (Origin != null)
                        {
                            _validAuthenticationTypes.SetOrigin(Origin);
                        }
                    }
                    else
                    {
                        _validAuthenticationTypes.Value = value!;
                    }
                }
            }
        }

        protected override bool CanHaveChildren => true;

        public override bool IsEmpty() => string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password);

        internal readonly AddItem _username;

        internal readonly AddItem _password;

        internal AddItem? _validAuthenticationTypes { get; set; }

        public CredentialsItem(string name, string username, string password, bool isPasswordClearText, string? validAuthenticationTypes)
           : base()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(password));
            }

            // ElementName is not being read from XML, so it's not decoded.
            ElementName = name;

            _username = new AddItem(ConfigurationConstants.UsernameToken, username);

            var passwordKey = isPasswordClearText ? ConfigurationConstants.ClearTextPasswordToken : ConfigurationConstants.PasswordToken;
            _password = new AddItem(passwordKey, password);

            IsPasswordClearText = isPasswordClearText;

            if (!string.IsNullOrEmpty(validAuthenticationTypes))
            {
                _validAuthenticationTypes = new AddItem(ConfigurationConstants.ValidAuthenticationTypesToken, validAuthenticationTypes);
            }
        }

        internal CredentialsItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            // ElementName is read from XML file, so it must be decoded.
            ElementName = XmlConvert.DecodeName(element.Name.LocalName);

            var elementDescendants = element.Elements();
            var countOfDescendants = elementDescendants.Count();

            var parsedItems = elementDescendants.Select(e => SettingFactory.Parse(e, origin)).OfType<AddItem>();

            foreach (var item in parsedItems)
            {
                if (string.Equals(item.Key, ConfigurationConstants.UsernameToken, StringComparison.OrdinalIgnoreCase))
                {
                    if (_username != null)
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.Error_MoreThanOneUsername, origin.ConfigFilePath));
                    }

                    _username = item;
                }
                else if (string.Equals(item.Key, ConfigurationConstants.PasswordToken, StringComparison.OrdinalIgnoreCase))
                {
                    if (_password != null)
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.Error_MoreThanOnePassword, origin.ConfigFilePath));
                    }

                    _password = item;
                    IsPasswordClearText = false;
                }
                else if (string.Equals(item.Key, ConfigurationConstants.ClearTextPasswordToken, StringComparison.OrdinalIgnoreCase))
                {
                    if (_password != null)
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.Error_MoreThanOnePassword, origin.ConfigFilePath));
                    }

                    _password = item;
                    IsPasswordClearText = true;
                }
                else if (string.Equals(item.Key, ConfigurationConstants.ValidAuthenticationTypesToken, StringComparison.OrdinalIgnoreCase))
                {
                    if (_validAuthenticationTypes != null)
                    {
                        throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.Error_MoreThanOneValidAuthenticationTypes, origin.ConfigFilePath));
                    }

                    _validAuthenticationTypes = item;
                }
            }

            if (_username == null || _password == null)
            {
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.CredentialsItemMustHaveUsernamePassword, origin.ConfigFilePath));
            }
        }

        public override SettingBase Clone()
        {
            var newSetting = new CredentialsItem(ElementName, Username, Password, IsPasswordClearText, ValidAuthenticationTypes);

            if (Origin != null)
            {
                newSetting.SetOrigin(Origin);
            }

            foreach (var attr in Attributes)
            {
                newSetting.AddAttribute(attr.Key, attr.Value);
            }

            return newSetting;
        }

        internal override XNode AsXNode()
        {
            if (Node is XElement)
            {
                return Node;
            }

            // Always encode the element name, since it might contain characters that are not allowed in XML element names.
            var element = new XElement(XmlUtility.GetEncodedXMLName(ElementName),
                _username.AsXNode(),
                _password.AsXNode());

            if (_validAuthenticationTypes != null)
            {
                element.Add(_validAuthenticationTypes.AsXNode());
            }

            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        public override bool Equals(object? other)
        {
            var item = other as CredentialsItem;

            if (item == null)
            {
                return false;
            }

            if (ReferenceEquals(this, item))
            {
                return true;
            }

            return string.Equals(ElementName, item.ElementName, StringComparison.Ordinal);
        }

        public override int GetHashCode() => ElementName.GetHashCode();

        /// <remarks>
        /// This method is internal because it updates directly the xElement behind this abstraction.
        /// It should only be called whenever the underlaying config file is intended to be changed.
        /// To persist changes to disk one must save the corresponding setting files
        /// </remarks>
        internal override void Update(SettingItem other)
        {
            base.Update(other);

            var credentials = other as CredentialsItem;

            if (!string.Equals(Username, credentials?.Username, StringComparison.Ordinal))
            {
                _username.Update(credentials!._username);
            }

            if (!string.Equals(Password, credentials?.Password, StringComparison.Ordinal) ||
                IsPasswordClearText != credentials!.IsPasswordClearText)
            {
                _password.Update(credentials!._password);
                IsPasswordClearText = credentials.IsPasswordClearText;
            }

            if (!string.Equals(ValidAuthenticationTypes, credentials.ValidAuthenticationTypes, StringComparison.Ordinal))
            {
                if (_validAuthenticationTypes == null)
                {
                    _validAuthenticationTypes = new AddItem(ConfigurationConstants.ValidAuthenticationTypesToken, credentials.ValidAuthenticationTypes);
                    _validAuthenticationTypes.SetNode(_validAuthenticationTypes.AsXNode());

                    if (Origin != null)
                    {
                        _validAuthenticationTypes.SetOrigin(Origin);
                    }

                    var element = Node as XElement;
                    if (element != null)
                    {
                        XElementUtility.AddIndented(element, _validAuthenticationTypes.Node);
                    }
                }
                else if (credentials._validAuthenticationTypes == null)
                {
                    XElementUtility.RemoveIndented(_validAuthenticationTypes.Node);
                    _validAuthenticationTypes = null;

                    if (Origin != null)
                    {
                        Origin.IsDirty = true;
                    }
                }
                else
                {
                    _validAuthenticationTypes.Update(credentials._validAuthenticationTypes);
                }
            }
        }

        internal override void SetOrigin(SettingsFile origin)
        {
            base.SetOrigin(origin);

            _username.SetOrigin(origin);
            _password.SetOrigin(origin);
            _validAuthenticationTypes?.SetOrigin(origin);
        }

        internal override void RemoveFromSettings()
        {
            base.RemoveFromSettings();

            _username.RemoveFromSettings();
            _password.RemoveFromSettings();
            _validAuthenticationTypes?.RemoveFromSettings();
        }
    }
}
