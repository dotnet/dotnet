// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Frameworks;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class IProjectMetadataContextInfoFormatter : NuGetMessagePackFormatter<IProjectMetadataContextInfo>
    {
        private const string FullPathPropertyName = "fullpath";
        private const string NamePropertyName = "name";
        private const string ProjectIdPropertyName = "projectid";
        private const string SupportedFrameworksPropertyName = "supportedframeworks";
        private const string TargetFrameworkPropertyName = "targetframework";
        private const string UniqueNamePropertyName = "uniquename";

        internal static readonly IMessagePackFormatter<IProjectMetadataContextInfo?> Instance = new IProjectMetadataContextInfoFormatter();

        private IProjectMetadataContextInfoFormatter()
        {
        }

        protected override IProjectMetadataContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? fullPath = null;
            string? name = null;
            string? projectId = null;
            NuGetFramework[]? supportedFrameworks = null;
            NuGetFramework? targetFramework = null;
            string? uniqueName = null;

            int propertyCount = reader.ReadMapHeader();

            for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
            {
                switch (reader.ReadString())
                {
                    case FullPathPropertyName:
                        fullPath = reader.ReadString();
                        break;

                    case NamePropertyName:
                        name = reader.ReadString();
                        break;

                    case ProjectIdPropertyName:
                        projectId = reader.ReadString();
                        break;

                    case SupportedFrameworksPropertyName:
                        if (!reader.TryReadNil())
                        {
                            int elementCount = reader.ReadArrayHeader();
                            supportedFrameworks = new NuGetFramework[elementCount];

                            for (var i = 0; i < elementCount; ++i)
                            {
                                NuGetFramework? framework = NuGetFrameworkFormatter.Instance.Deserialize(ref reader, options);

                                Assumes.NotNull(framework);

                                supportedFrameworks[i] = framework;
                            }
                        }
                        break;

                    case TargetFrameworkPropertyName:
                        targetFramework = NuGetFrameworkFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case UniqueNamePropertyName:
                        uniqueName = reader.ReadString();
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            return new ProjectMetadataContextInfo(fullPath, name, projectId, supportedFrameworks, targetFramework, uniqueName);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, IProjectMetadataContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 6);
            writer.Write(FullPathPropertyName);
            writer.Write(value.FullPath);
            writer.Write(NamePropertyName);
            writer.Write(value.Name);
            writer.Write(ProjectIdPropertyName);
            writer.Write(value.ProjectId);
            writer.Write(SupportedFrameworksPropertyName);

            if (value.SupportedFrameworks is null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.WriteArrayHeader(value.SupportedFrameworks.Count);

                foreach (NuGetFramework framework in value.SupportedFrameworks)
                {
                    NuGetFrameworkFormatter.Instance.Serialize(ref writer, framework, options);
                }
            }

            writer.Write(TargetFrameworkPropertyName);

            if (value.TargetFramework is null)
            {
                writer.WriteNil();
            }
            else
            {
                NuGetFrameworkFormatter.Instance.Serialize(ref writer, value.TargetFramework, options);
            }

            writer.Write(UniqueNamePropertyName);
            writer.Write(value.UniqueName);
        }
    }
}
