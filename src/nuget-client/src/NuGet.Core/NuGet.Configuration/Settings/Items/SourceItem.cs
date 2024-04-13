// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;

namespace NuGet.Configuration
{
    public sealed class SourceItem : AddItem
    {
        public string? ProtocolVersion
        {
            get
            {
                if (Attributes.TryGetValue(ConfigurationConstants.ProtocolVersionAttribute, out var attribute))
                {
                    return Settings.ApplyEnvironmentTransform(attribute);
                }

                return null;
            }
            set => AddOrUpdateAttribute(ConfigurationConstants.ProtocolVersionAttribute, value);
        }

        public string? AllowInsecureConnections
        {
            get
            {
                if (Attributes.TryGetValue(ConfigurationConstants.AllowInsecureConnections, out var attribute))
                {
                    return Settings.ApplyEnvironmentTransform(attribute);
                }

                return null;
            }
            set => AddOrUpdateAttribute(ConfigurationConstants.AllowInsecureConnections, value);
        }

        public string? DisableTLSCertificateValidation
        {
            get
            {
                if (Attributes.TryGetValue(ConfigurationConstants.DisableTLSCertificateValidation, out var attribute))
                {
                    return Settings.ApplyEnvironmentTransform(attribute);
                }

                return null;
            }
            set => AddOrUpdateAttribute(ConfigurationConstants.DisableTLSCertificateValidation, value);
        }

        public SourceItem(string key, string value)
            : this(key, value, protocolVersion: "", allowInsecureConnections: "", disableTLSCertificateValidation: "")
        {
        }

        public SourceItem(string key, string value, string? protocolVersion)
            : this(key, value, protocolVersion, allowInsecureConnections: "", disableTLSCertificateValidation: "")
        {
        }

        public SourceItem(string key, string value, string? protocolVersion, string? allowInsecureConnections)
            : this(key, value, protocolVersion, allowInsecureConnections, disableTLSCertificateValidation: "")
        {
        }

        public SourceItem(string key, string value, string? protocolVersion, string? allowInsecureConnections, string? disableTLSCertificateValidation)
            : base(key, value)
        {
            if (!string.IsNullOrEmpty(protocolVersion))
            {
                ProtocolVersion = protocolVersion;
            }
            if (!string.IsNullOrEmpty(allowInsecureConnections))
            {
                AllowInsecureConnections = allowInsecureConnections;
            }
            if (!string.IsNullOrEmpty(disableTLSCertificateValidation))
            {
                DisableTLSCertificateValidation = disableTLSCertificateValidation;
            }
        }

        internal SourceItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        public override SettingBase Clone()
        {
            var newSetting = new SourceItem(Key, Value, ProtocolVersion, AllowInsecureConnections, DisableTLSCertificateValidation);

            if (Origin != null)
            {
                newSetting.SetOrigin(Origin);
            }

            return newSetting;
        }
    }
}
