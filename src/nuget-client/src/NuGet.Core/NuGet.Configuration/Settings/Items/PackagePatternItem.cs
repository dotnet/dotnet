// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    /// <summary>
    /// A PackagePatternItem has only a key and no children.
    ///     - [Required] Id
    /// </summary>
    public sealed class PackagePatternItem : SettingItem
    {
        public override string ElementName => ConfigurationConstants.Package;

        public string Pattern => Attributes[ConfigurationConstants.PatternAttribute];

        protected override IReadOnlyCollection<string> RequiredAttributes { get; }
                = new HashSet<string>(new[] { ConfigurationConstants.PatternAttribute });

        public PackagePatternItem(string pattern)
            : base()
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(pattern));
            }

            AddAttribute(ConfigurationConstants.PatternAttribute, pattern);
        }

        internal PackagePatternItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        public override SettingBase Clone()
        {
            var newItem = new PackagePatternItem(Pattern);

            if (Origin != null)
            {
                newItem.SetOrigin(Origin);
            }

            return newItem;
        }

        public override bool Equals(object? other)
        {
            if (other is PackagePatternItem item)
            {
                if (ReferenceEquals(this, item))
                {
                    return true;
                }

                return string.Equals(Pattern, item.Pattern, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Pattern);
    }
}
