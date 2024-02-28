// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    public abstract class ClientCertItem : SettingItem
    {
        protected ClientCertItem(string packageSource)
        {
            if (string.IsNullOrEmpty(packageSource))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageSource));
            }

            AddAttribute(ConfigurationConstants.PackageSourceAttribute, packageSource);
        }

        internal ClientCertItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        public string PackageSource => Attributes[ConfigurationConstants.PackageSourceAttribute];

        protected override bool CanHaveChildren => false;

        public override bool Equals(object? other)
        {
            var item = other as ClientCertItem;
            if (item == null)
            {
                return false;
            }

            if (ReferenceEquals(this, item))
            {
                return true;
            }

            if (!string.Equals(ElementName, item.ElementName, StringComparison.Ordinal))
            {
                return false;
            }

            if (!string.Equals(PackageSource, item.PackageSource, StringComparison.Ordinal))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(ElementName);
            combiner.AddObject(PackageSource);

            return combiner.CombinedHash;
        }

        public abstract IEnumerable<X509Certificate> Search();

        protected void SetPackageSource(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PropertyCannotBeNullOrEmpty, nameof(PackageSource)));
            }

            UpdateAttribute(ConfigurationConstants.PackageSourceAttribute, value);
        }
    }
}
