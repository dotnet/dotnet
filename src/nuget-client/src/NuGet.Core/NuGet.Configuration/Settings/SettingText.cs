// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public sealed class SettingText : SettingBase
    {
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(value));
                }

                _value = value;
            }
        }

        public SettingText(string value)
            : base()
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(value));
            }

            _value = value;
        }

        public override bool Equals(object? other)
        {
            var text = other as SettingText;

            if (text == null)
            {
                return false;
            }

            if (ReferenceEquals(this, text))
            {
                return true;
            }

            return string.Equals(Value, text.Value, StringComparison.Ordinal);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override bool IsEmpty() => string.IsNullOrEmpty(Value);

        public override SettingBase Clone()
        {
            var newSetting = new SettingText(Value);

            if (Origin != null)
            {
                newSetting.SetOrigin(Origin);
            }

            return newSetting;
        }

        internal SettingText(XText text, SettingsFile origin)
            : base(text, origin)
        {
            var value = text.Value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.TextShouldNotBeEmpty, origin.ConfigFilePath));
            }

            _value = value;
        }

        internal override XNode AsXNode()
        {
            if (Node is XText xText)
            {
                return xText;
            }

            return new XText(Value);
        }
    }
}
