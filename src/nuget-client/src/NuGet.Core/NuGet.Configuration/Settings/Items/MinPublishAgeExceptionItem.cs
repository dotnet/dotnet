// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    /// <summary>
    /// Defines a package ID pattern that is exempt from minimum publish age enforcement.
    /// </summary>
    public sealed class MinPublishAgeExceptionItem : SettingItem
    {
        private string _pattern = string.Empty;

        public override string ElementName => "package";

        /// <summary>
        /// Gets the package ID pattern.
        /// </summary>
        public required string Pattern
        {
            get => _pattern;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Empty_Or_WhiteSpaceOnly, nameof(value));
                }

                if (value.Length > PackageSourceMapping.PackageIdMaxLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                AddOrUpdateAttribute(ConfigurationConstants.PatternAttribute, value);
                _pattern = value;
            }
        }

        protected override IReadOnlyCollection<string> RequiredAttributes { get; } =
            new HashSet<string>(new[] { ConfigurationConstants.PatternAttribute });

        /// <summary>
        /// Initializes a new instance of the <see cref="MinPublishAgeExceptionItem"/> class.
        /// </summary>
        public MinPublishAgeExceptionItem()
            : base()
        {
        }

        [SetsRequiredMembers]
        internal MinPublishAgeExceptionItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            string pattern = Attributes[ConfigurationConstants.PatternAttribute];
            try
            {
                Pattern = pattern;
            }
            catch (ArgumentException exception)
            {
                string reason = string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.Error_InvalidMinPublishAgeExceptionPattern,
                    pattern);
                throw new NuGetConfigurationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.UserSettings_UnableToParseConfigFile,
                        reason,
                        origin.ConfigFilePath),
                    exception);
            }
        }

        public override SettingBase Clone()
        {
            var newItem = new MinPublishAgeExceptionItem
            {
                Pattern = Pattern
            };

            if (Origin != null)
            {
                newItem.SetOrigin(Origin);
            }

            return newItem;
        }

        public override bool Equals(object? other)
        {
            if (other is MinPublishAgeExceptionItem item)
            {
                return string.Equals(Pattern, item.Pattern, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Pattern);
    }
}
