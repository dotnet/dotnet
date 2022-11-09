// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    /// <summary>
    ///     A StoreClientCertItem have 4 Attributes:
    ///     - [Required] packageSource
    ///     - [Optional] storeLocation. StoreLocation.CurrentUser by default.
    ///     - [Optional] storeName. StoreName.My by default.
    ///     - [Optional] findBy. X509FindType.FindByThumbprint by default.
    ///     - [Required] findValue
    /// </summary>
    public sealed class StoreClientCertItem : ClientCertItem
    {
        private const X509FindType DefaultFindBy = X509FindType.FindByThumbprint;
        private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
        private const StoreName DefaultStoreName = StoreName.My;

        public static string GetString(X509FindType type)
        {
            return type.ToString().Replace("FindBy", string.Empty);
        }

        public static string GetString(StoreName storeName)
        {
            return storeName.ToString();
        }

        public static string GetString(StoreLocation storeLocation)
        {
            return storeLocation.ToString();
        }

        public StoreClientCertItem(string packageSource,
                                   string findValue,
                                   StoreLocation? storeLocation = null,
                                   StoreName? storeName = null,
                                   X509FindType? findBy = null)
            : base(packageSource)
        {
            if (!storeLocation.HasValue)
            {
                storeLocation = DefaultStoreLocation;
            }

            if (!storeName.HasValue)
            {
                storeName = DefaultStoreName;
            }

            if (!findBy.HasValue)
            {
                findBy = DefaultFindBy;
            }

            if (string.IsNullOrWhiteSpace(findValue))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(findValue));
            }

            Update(findValue, storeLocation, storeName, findBy);
        }

        internal StoreClientCertItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            var storeLocation = element.Attribute(XName.Get(ConfigurationConstants.StoreLocationAttribute))?.Value;
            if (string.IsNullOrWhiteSpace(storeLocation))
            {
                storeLocation = DefaultStoreLocation.ToString();
            }

            AddAttribute(ConfigurationConstants.StoreLocationAttribute, storeLocation);

            var storeName = element.Attribute(XName.Get(ConfigurationConstants.StoreNameAttribute))?.Value;
            if (string.IsNullOrWhiteSpace(storeName))
            {
                storeName = DefaultStoreName.ToString();
            }

            AddAttribute(ConfigurationConstants.StoreNameAttribute, storeName);

            var findBy = element.Attribute(XName.Get(ConfigurationConstants.FindByAttribute))?.Value;
            if (string.IsNullOrWhiteSpace(findBy))
            {
                findBy = GetString(DefaultFindBy);
            }

            AddAttribute(ConfigurationConstants.FindByAttribute, findBy);

            var findValue = element.Attribute(XName.Get(ConfigurationConstants.FindValueAttribute))?.Value;
            if (string.IsNullOrWhiteSpace(findValue))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty);
            }

            AddAttribute(ConfigurationConstants.FindValueAttribute, findValue);
        }

        public override string ElementName => ConfigurationConstants.StoreCertificate;

        public X509FindType FindType
        {
            get
            {
                if (Enum.TryParse("FindBy" + Attributes[ConfigurationConstants.FindByAttribute], ignoreCase: true, result: out X509FindType result))
                {
                    return result;
                }

                return DefaultFindBy;
            }
        }

        public string FindValue => Attributes[ConfigurationConstants.FindValueAttribute];

        public StoreLocation StoreLocation
        {
            get
            {
                if (Enum.TryParse(Attributes[ConfigurationConstants.StoreLocationAttribute], ignoreCase: true, result: out StoreLocation result))
                {
                    return result;
                }

                return DefaultStoreLocation;
            }
        }

        public StoreName StoreName
        {
            get
            {
                if (Enum.TryParse(Attributes[ConfigurationConstants.StoreNameAttribute], ignoreCase: true, result: out StoreName result))
                {
                    return result;
                }

                return DefaultStoreName;
            }
        }

        protected override IReadOnlyCollection<string> AllowedAttributes { get; }
            = new HashSet<string>(new[]
                {
                    ConfigurationConstants.PackageSourceAttribute,
                    ConfigurationConstants.StoreLocationAttribute,
                    ConfigurationConstants.StoreNameAttribute,
                    ConfigurationConstants.FindByAttribute,
                    ConfigurationConstants.FindValueAttribute
                });

        protected override IReadOnlyDictionary<string, IReadOnlyCollection<string>> AllowedValues { get; } = new Dictionary<string, IReadOnlyCollection<string>>
        {
            {
                ConfigurationConstants.StoreLocationAttribute,
                new HashSet<string>(new[]
                {
                    GetString(StoreLocation.CurrentUser),
                    GetString(StoreLocation.LocalMachine)
                },
                    StringComparer.OrdinalIgnoreCase)
            },
            {
                ConfigurationConstants.StoreNameAttribute,
                new HashSet<string>(new []
                {
                    GetString(StoreName.AddressBook),
                    GetString(StoreName.AuthRoot),
                    GetString(StoreName.CertificateAuthority),
                    GetString(StoreName.Disallowed),
                    GetString(StoreName.My),
                    GetString(StoreName.Root),
                    GetString(StoreName.TrustedPeople),
                    GetString(StoreName.TrustedPublisher)
                },
                    StringComparer.OrdinalIgnoreCase)
            },
            {
                ConfigurationConstants.FindByAttribute,
                new HashSet<string>(new[]
                {
                    GetString(X509FindType.FindByThumbprint),
                    GetString(X509FindType.FindBySubjectName),
                    GetString(X509FindType.FindBySubjectDistinguishedName),
                    GetString(X509FindType.FindByIssuerName),
                    GetString(X509FindType.FindByIssuerDistinguishedName),
                    GetString(X509FindType.FindBySerialNumber),
                    GetString(X509FindType.FindByTimeValid),
                    GetString(X509FindType.FindByTimeNotYetValid),
                    GetString(X509FindType.FindByTimeExpired),
                    GetString(X509FindType.FindByTemplateName),
                    GetString(X509FindType.FindByApplicationPolicy),
                    GetString(X509FindType.FindByCertificatePolicy),
                    GetString(X509FindType.FindByExtension),
                    GetString(X509FindType.FindByKeyUsage),
                    GetString(X509FindType.FindBySubjectKeyIdentifier)
                },
                    StringComparer.OrdinalIgnoreCase)
            }
        };

        protected override IReadOnlyCollection<string> RequiredAttributes
        { get; }
        = new HashSet<string>(new[] { ConfigurationConstants.PackageSourceAttribute, ConfigurationConstants.FindValueAttribute });

        internal override XNode AsXNode()
        {
            if (Node is XElement)
            {
                return Node;
            }

            var element = new XElement(ElementName);

            foreach (KeyValuePair<string, string> attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        public override SettingBase Clone()
        {
            return new StoreClientCertItem(PackageSource, FindValue, StoreLocation, StoreName, FindType);
        }

        public override IEnumerable<X509Certificate> Search()
        {
            var store = new X509Store(StoreName, StoreLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection foundCertificates = store.Certificates.Find(FindType, FindValue, false);
                if (foundCertificates.Count == 0)
                {
                    throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture,
                                                                        Resources.Error_StoreCertCertificateNotFound,
                                                                        PackageSource,
                                                                        GetString(StoreLocation),
                                                                        GetString(StoreName),
                                                                        GetString(FindType),
                                                                        FindValue));
                }

                return foundCertificates.OfType<X509Certificate>();
            }
            finally
            {
                store.Close();
            }
        }

        public void Update(string findValue,
                           StoreLocation? storeLocation = null,
                           StoreName? storeName = null,
                           X509FindType? findBy = null)
        {
            if (storeLocation.HasValue)
            {
                AddOrUpdateAttribute(ConfigurationConstants.StoreLocationAttribute, storeLocation.Value.ToString());
            }

            if (storeName.HasValue)
            {
                AddOrUpdateAttribute(ConfigurationConstants.StoreNameAttribute, storeName.Value.ToString());
            }

            if (findBy.HasValue)
            {
                AddOrUpdateAttribute(ConfigurationConstants.FindByAttribute, GetString(findBy.Value));
            }

            if (!string.IsNullOrWhiteSpace(findValue))
            {
                AddOrUpdateAttribute(ConfigurationConstants.FindValueAttribute, findValue);
            }
        }
    }
}
