// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using NuGet.Common;

namespace NuGet.Configuration
{
    public sealed class CertificateItem : SettingItem
    {
        public override string ElementName => ConfigurationConstants.Certificate;

        public string Fingerprint
        {
            get => Attributes[ConfigurationConstants.Fingerprint];
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PropertyCannotBeNullOrEmpty, nameof(Fingerprint)));
                }

                UpdateAttribute(ConfigurationConstants.Fingerprint, value);
            }
        }

        public HashAlgorithmName HashAlgorithm
        {
            get => CryptoHashUtility.GetHashAlgorithmName(Attributes[ConfigurationConstants.HashAlgorithm]);
            set
            {
                if (value == HashAlgorithmName.Unknown)
                {
                    throw new ArgumentException(Resources.UnknownHashAlgorithmNotSupported);
                }

                UpdateAttribute(ConfigurationConstants.HashAlgorithm, value.ToString().ToUpper(CultureInfo.InvariantCulture));
            }
        }

        public bool AllowUntrustedRoot
        {
            get
            {
                if (bool.TryParse(Attributes[ConfigurationConstants.AllowUntrustedRoot], out var parsedAttribute))
                {
                    return parsedAttribute;
                }

                return false;
            }
            set => UpdateAttribute(ConfigurationConstants.AllowUntrustedRoot, value.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.InvariantCulture));
        }

        protected override IReadOnlyCollection<string> RequiredAttributes { get; }
                = new HashSet<string>(new[] { ConfigurationConstants.Fingerprint, ConfigurationConstants.HashAlgorithm, ConfigurationConstants.AllowUntrustedRoot });

        public CertificateItem(string fingerprint, HashAlgorithmName hashAlgorithm, bool allowUntrustedRoot = false)
            : base()
        {
            if (string.IsNullOrEmpty(fingerprint))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(fingerprint));
            }

            if (hashAlgorithm == HashAlgorithmName.Unknown)
            {
                throw new ArgumentException(Resources.UnknownHashAlgorithmNotSupported);
            }

            AddAttribute(ConfigurationConstants.Fingerprint, fingerprint);
            AddAttribute(ConfigurationConstants.HashAlgorithm, hashAlgorithm.ToString().ToUpper(CultureInfo.InvariantCulture));
            AddAttribute(ConfigurationConstants.AllowUntrustedRoot, allowUntrustedRoot.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.InvariantCulture));
        }

        internal CertificateItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            if (HashAlgorithm == HashAlgorithmName.Unknown)
            {
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                    string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedHashAlgorithm, Attributes[ConfigurationConstants.HashAlgorithm]),
                    origin.ConfigFilePath));
            }

            // Update attributes with propert casing
            UpdateAttribute(ConfigurationConstants.HashAlgorithm, HashAlgorithm.ToString().ToUpper(CultureInfo.InvariantCulture));
            UpdateAttribute(ConfigurationConstants.AllowUntrustedRoot, AllowUntrustedRoot.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.InvariantCulture));
        }

        public override SettingBase Clone()
        {
            var newItem = new CertificateItem(Fingerprint, HashAlgorithm, AllowUntrustedRoot);

            if (Origin != null)
            {
                newItem.SetOrigin(Origin);
            }

            return newItem;
        }

        public override bool Equals(object other)
        {
            if (other is CertificateItem cert)
            {
                if (ReferenceEquals(this, cert))
                {
                    return true;
                }

                return string.Equals(Fingerprint, cert.Fingerprint, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode() => Fingerprint.GetHashCode();
    }
}
