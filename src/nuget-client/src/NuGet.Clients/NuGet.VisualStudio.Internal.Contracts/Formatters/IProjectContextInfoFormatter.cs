// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class IProjectContextInfoFormatter : NuGetMessagePackFormatter<IProjectContextInfo>
    {
        private const string ProjectIdPropertyName = "projectid";
        private const string ProjectKindPropertyName = "projectkind";
        private const string ProjectStylePropertyName = "projectstyle";

        internal static readonly IMessagePackFormatter<IProjectContextInfo?> Instance = new IProjectContextInfoFormatter();

        private IProjectContextInfoFormatter()
        {
        }

        protected override IProjectContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            NuGetProjectKind projectKind = NuGetProjectKind.Unknown;
            ProjectStyle projectStyle = ProjectStyle.Unknown;
            string? projectId = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case ProjectIdPropertyName:
                        projectId = reader.ReadString();
                        break;
                    case ProjectKindPropertyName:
                        projectKind = options.Resolver.GetFormatter<NuGetProjectKind>().Deserialize(ref reader, options);
                        break;
                    case ProjectStylePropertyName:
                        projectStyle = options.Resolver.GetFormatter<ProjectStyle>().Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(projectId);

            return new ProjectContextInfo(projectId, projectStyle, projectKind);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, IProjectContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 3);
            writer.Write(ProjectIdPropertyName);
            writer.Write(value.ProjectId);
            writer.Write(ProjectKindPropertyName);
            options.Resolver.GetFormatter<NuGetProjectKind>().Serialize(ref writer, value.ProjectKind, options);
            writer.Write(ProjectStylePropertyName);
            options.Resolver.GetFormatter<ProjectStyle>().Serialize(ref writer, value.ProjectStyle, options);
        }
    }
}
