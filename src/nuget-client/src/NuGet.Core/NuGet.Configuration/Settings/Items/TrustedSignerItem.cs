// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public abstract class TrustedSignerItem : SettingItem
    {
        protected override bool CanHaveChildren => true;

        public IList<CertificateItem> Certificates { get; }

        public virtual string Name => Attributes[ConfigurationConstants.NameAttribute];

        protected void SetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PropertyCannotBeNullOrEmpty, nameof(Name)));
            }

            UpdateAttribute(ConfigurationConstants.NameAttribute, value);
        }

        internal readonly IEnumerable<SettingBase> _parsedDescendants;

        protected TrustedSignerItem(string name, IEnumerable<CertificateItem> certificates)
            : base()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            if (certificates == null || !certificates.Any())
            {
                throw new ArgumentException(Resources.TrustedSignerMustHaveCertificates);
            }

            AddAttribute(ConfigurationConstants.NameAttribute, name);

            Certificates = new List<CertificateItem>();

            foreach (var certificate in certificates)
            {
                Certificates.Add(certificate);
            }
        }

        internal TrustedSignerItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            _parsedDescendants = element.Nodes().Where(n => n is XElement || n is XText text && !string.IsNullOrWhiteSpace(text.Value))
                .Select(e => SettingFactory.Parse(e, origin));

            var parsedCertificates = _parsedDescendants.OfType<CertificateItem>().ToList();

            if (parsedCertificates.Count == 0)
            {
                throw new NuGetConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.TrustedSignerMustHaveCertificates, origin.ConfigFilePath));
            }

            Certificates = parsedCertificates;
        }

        internal override void SetOrigin(SettingsFile origin)
        {
            base.SetOrigin(origin);

            foreach (var certificate in Certificates)
            {
                certificate.SetOrigin(origin);
            }
        }

        internal override void RemoveFromSettings()
        {
            base.RemoveFromSettings();

            foreach (var certificate in Certificates)
            {
                certificate.RemoveFromSettings();
            }
        }

        internal override void Update(SettingItem other)
        {
            var trustedSigner = other as TrustedSignerItem;

            if (!trustedSigner.Certificates.Any())
            {
                throw new InvalidOperationException(Resources.TrustedSignerMustHaveCertificates);
            }

            base.Update(other);

            var otherCerts = trustedSigner.Certificates.ToDictionary(c => c, c => c);
            var immutableCerts = new List<CertificateItem>(Certificates);
            foreach (var cert in immutableCerts)
            {
                if (otherCerts.TryGetValue(cert, out var otherChild))
                {
                    otherCerts.Remove(cert);
                }

                if (otherChild == null)
                {
                    Certificates.Remove(cert);
                    cert.RemoveFromSettings();
                }
                else if (cert is SettingItem item)
                {
                    item.Update(otherChild as SettingItem);
                }
            }

            foreach (var newCert in otherCerts)
            {
                var certToAdd = newCert.Value;
                Certificates.Add(certToAdd);

                if (Origin != null)
                {
                    certToAdd.SetOrigin(Origin);

                    if (Node != null)
                    {
                        certToAdd.SetNode(certToAdd.AsXNode());

                        XElementUtility.AddIndented(Node as XElement, certToAdd.Node);
                        Origin.IsDirty = true;
                    }
                }
            }
        }
    }
}
