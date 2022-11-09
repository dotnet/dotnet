// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class ImplicitProjectActionFormatter : NuGetMessagePackFormatter<ImplicitProjectAction>
    {
        private const string IdPropertyName = "id";
        private const string PackageIdentityPropertyName = "packageidentity";
        private const string ProjectActionTypePropertyName = "projectactiontype";

        internal static readonly IMessagePackFormatter<ImplicitProjectAction?> Instance = new ImplicitProjectActionFormatter();

        private ImplicitProjectActionFormatter()
        {
        }

        protected override ImplicitProjectAction? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? id = null;
            PackageIdentity? packageIdentity = null;
            NuGetProjectActionType? projectActionType = null;

            int propertyCount = reader.ReadMapHeader();

            for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
            {
                switch (reader.ReadString())
                {
                    case IdPropertyName:
                        id = reader.ReadString();
                        break;

                    case PackageIdentityPropertyName:
                        packageIdentity = PackageIdentityFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case ProjectActionTypePropertyName:
                        projectActionType = options.Resolver.GetFormatter<NuGetProjectActionType>().Deserialize(ref reader, options);
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNullOrEmpty(id);
            Assumes.NotNull(packageIdentity);
            Assumes.True(projectActionType.HasValue);

            return new ImplicitProjectAction(id, packageIdentity, projectActionType!.Value);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, ImplicitProjectAction value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 3);
            writer.Write(IdPropertyName);
            writer.Write(value.Id);
            writer.Write(PackageIdentityPropertyName);
            PackageIdentityFormatter.Instance.Serialize(ref writer, value.PackageIdentity, options);
            writer.Write(ProjectActionTypePropertyName);
            options.Resolver.GetFormatter<NuGetProjectActionType>().Serialize(ref writer, value.ProjectActionType, options);
        }
    }
}
