// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    public sealed class SourceItem : AddItem
    {
        public string ProtocolVersion
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

        public SourceItem(string key, string value, string protocolVersion = "")
            : base(key, value)
        {
            if (!string.IsNullOrEmpty(protocolVersion))
            {
                ProtocolVersion = protocolVersion;
            }
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Key);

            if (ProtocolVersion != null)
            {
                combiner.AddObject(ProtocolVersion);
            }

            return combiner.CombinedHash;
        }

        internal SourceItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        public override SettingBase Clone()
        {
            var newSetting = new SourceItem(Key, Value, ProtocolVersion);

            if (Origin != null)
            {
                newSetting.SetOrigin(Origin);
            }

            return newSetting;
        }

        public override bool Equals(object other)
        {
            var source = other as SourceItem;

            if (source == null)
            {
                return false;
            }

            if (ReferenceEquals(this, source))
            {
                return true;
            }

            return string.Equals(Key, source.Key, StringComparison.Ordinal) &&
                string.Equals(ProtocolVersion, source.ProtocolVersion, StringComparison.Ordinal);
        }
    }
}
