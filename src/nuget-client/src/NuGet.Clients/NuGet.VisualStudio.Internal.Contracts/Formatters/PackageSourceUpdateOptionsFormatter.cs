// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using NuGet.Configuration;

namespace NuGet.VisualStudio.Internal.Contracts
{
#pragma warning disable CS0618 // Type or member is obsolete
    internal sealed class PackageSourceUpdateOptionsFormatter : NuGetMessagePackFormatter<PackageSourceUpdateOptions>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private const string UpdateCredentialsPropertyName = "updatecredentials";
        private const string UpdateEnabledPropertyName = "updateenabled";

#pragma warning disable CS0618 // Type or member is obsolete
        internal static readonly IMessagePackFormatter<PackageSourceUpdateOptions?> Instance = new PackageSourceUpdateOptionsFormatter();
#pragma warning restore CS0618 // Type or member is obsolete

        private PackageSourceUpdateOptionsFormatter()
        {
        }

#pragma warning disable CS0618 // Type or member is obsolete
        protected override PackageSourceUpdateOptions? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            bool updateCredentials = true;
            bool updateEnabled = true;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case UpdateCredentialsPropertyName:
                        updateCredentials = reader.ReadBoolean();
                        break;
                    case UpdateEnabledPropertyName:
                        updateEnabled = reader.ReadBoolean();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

#pragma warning disable CS0618 // Type or member is obsolete
            return new PackageSourceUpdateOptions(updateCredentials, updateEnabled);
#pragma warning restore CS0618 // Type or member is obsolete
        }

#pragma warning disable CS0618 // Type or member is obsolete
        protected override void SerializeCore(ref MessagePackWriter writer, PackageSourceUpdateOptions value, MessagePackSerializerOptions options)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            writer.WriteMapHeader(count: 2);
            writer.Write(UpdateCredentialsPropertyName);
            writer.Write(value.UpdateCredentials);
            writer.Write(UpdateEnabledPropertyName);
            writer.Write(value.UpdateEnabled);
        }
    }
}
