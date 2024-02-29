// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public sealed class AuthorItem : TrustedSignerItem
    {
        public override string ElementName => ConfigurationConstants.Author;

        protected override IReadOnlyCollection<string> RequiredAttributes { get; }
            = new HashSet<string>(new[] { ConfigurationConstants.NameAttribute });

        public AuthorItem(string name, params CertificateItem[] certificates)
            : base(name, certificates)
        {
        }

        internal AuthorItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        public override SettingBase Clone()
        {
            var newItem = new AuthorItem(Name, Certificates.Select(c => (CertificateItem)c.Clone()).ToArray());

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

            foreach (var cert in Certificates)
            {
                element.Add(cert.AsXNode());
            }

            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        public override bool Equals(object? other)
        {
            if (other is AuthorItem author)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return string.Equals(Name, author.Name, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
