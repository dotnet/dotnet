// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NET
using System.Collections.Generic;
#endif
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class ObjectWriters
{
    public static void Write(JsonDataWriter writer, TagHelperDescriptor? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, TagHelperDescriptor value)
    {
        writer.WriteObject(WellKnownPropertyNames.Checksum, value.Checksum, WriteProperties);
        writer.Write(nameof(value.Kind), value.Kind);
        writer.Write(nameof(value.Name), value.Name);
        writer.Write(nameof(value.AssemblyName), value.AssemblyName);
        writer.WriteIfNotNull(nameof(value.DisplayName), value.DisplayName);
        WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
        writer.WriteIfNotNull(nameof(value.TagOutputHint), value.TagOutputHint);
        writer.Write(nameof(value.CaseSensitive), value.CaseSensitive);
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.TagMatchingRules), value.TagMatchingRules, WriteTagMatchingRule);
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.BoundAttributes), value.BoundAttributes, WriteBoundAttribute);
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.AllowedChildTags), value.AllowedChildTags, WriteAllowedChildTag);
        WriteMetadata(writer, nameof(value.Metadata), value.Metadata);
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);

        static void WriteDocumentationObject(JsonDataWriter writer, string propertyName, DocumentationObject documentationObject)
        {
            switch (documentationObject.Object)
            {
                case DocumentationDescriptor descriptor:
                    writer.WriteObject(propertyName, descriptor, static (writer, value) =>
                    {
                        writer.Write(nameof(value.Id), (int)value.Id);
                        if (value.Args is { Length: > 0 })
                        {
                            writer.WriteArray(nameof(value.Args), value.Args, static (w, v) => w.WriteValue(v));
                        }
                    });

                    break;

                case string text:
                    writer.Write(propertyName, text);
                    break;

                case null:
                    // Don't write a property if there isn't any documentation.
                    break;

                default:
                    Debug.Fail($"Documentation objects should only be of type {nameof(DocumentationDescriptor)}, string, or null.");
                    break;
            }
        }

        static void WriteTagMatchingRule(JsonDataWriter writer, TagMatchingRuleDescriptor value)
        {
            writer.WriteObject(value, static (writer, value) =>
            {
                writer.Write(nameof(value.TagName), value.TagName);
                writer.WriteIfNotNull(nameof(value.ParentTag), value.ParentTag);
                writer.WriteIfNotZero(nameof(value.TagStructure), (int)value.TagStructure);
                writer.WriteIfNotTrue(nameof(value.CaseSensitive), value.CaseSensitive);
                writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Attributes), value.Attributes, WriteRequiredAttribute);
                writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
            });
        }

        static void WriteRequiredAttribute(JsonDataWriter writer, RequiredAttributeDescriptor value)
        {
            writer.WriteObject(value, static (writer, value) =>
            {
                writer.Write(nameof(value.Name), value.Name);
                writer.WriteIfNotZero(nameof(value.NameComparison), (int)value.NameComparison);
                writer.WriteIfNotTrue(nameof(value.CaseSensitive), value.CaseSensitive);
                writer.WriteIfNotNull(nameof(value.Value), value.Value);
                writer.WriteIfNotZero(nameof(value.ValueComparison), (int)value.ValueComparison);
                writer.WriteIfNotNull(nameof(value.DisplayName), value.DisplayName);

                WriteMetadata(writer, nameof(value.Metadata), value.Metadata);
                writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
            });
        }

        static void WriteBoundAttribute(JsonDataWriter writer, BoundAttributeDescriptor value)
        {
            writer.WriteObject(value, static (writer, value) =>
            {
                writer.Write(nameof(value.Kind), value.Kind);
                writer.Write(nameof(value.Name), value.Name);
                writer.Write(nameof(value.TypeName), value.TypeName);
                writer.WriteIfNotFalse(nameof(value.IsEnum), value.IsEnum);
                writer.WriteIfNotFalse(nameof(value.HasIndexer), value.HasIndexer);
                writer.WriteIfNotNull(nameof(value.IndexerNamePrefix), value.IndexerNamePrefix);
                writer.WriteIfNotNull(nameof(value.IndexerTypeName), value.IndexerTypeName);
                writer.WriteIfNotNull(nameof(value.DisplayName), value.DisplayName);
                writer.WriteIfNotNull(nameof(value.ContainingType), value.ContainingType);
                WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
                writer.WriteIfNotTrue(nameof(value.CaseSensitive), value.CaseSensitive);
                writer.WriteIfNotFalse(nameof(value.IsEditorRequired), value.IsEditorRequired);
                writer.WriteArrayIfNotDefaultOrEmpty("BoundAttributeParameters", value.Parameters, WriteBoundAttributeParameter);

                WriteMetadata(writer, nameof(value.Metadata), value.Metadata);
                writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
            });
        }

        static void WriteBoundAttributeParameter(JsonDataWriter writer, BoundAttributeParameterDescriptor value)
        {
            writer.WriteObject(value, static (writer, value) =>
            {
                writer.Write(nameof(value.Kind), value.Kind);
                writer.Write(nameof(value.Name), value.Name);
                writer.Write(nameof(value.TypeName), value.TypeName);
                writer.WriteIfNotFalse(nameof(value.IsEnum), value.IsEnum);
                writer.WriteIfNotNull(nameof(value.DisplayName), value.DisplayName);
                WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
                writer.WriteIfNotTrue(nameof(value.CaseSensitive), value.CaseSensitive);

                WriteMetadata(writer, nameof(value.Metadata), value.Metadata);
                writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
            });
        }

        static void WriteAllowedChildTag(JsonDataWriter writer, AllowedChildTagDescriptor value)
        {
            writer.WriteObject(value, static (writer, value) =>
            {
                writer.Write(nameof(value.Name), value.Name);
                writer.Write(nameof(value.DisplayName), value.DisplayName);
                writer.WriteArray(nameof(value.Diagnostics), value.Diagnostics, Write);
            });
        }

        static void WriteMetadata(JsonDataWriter writer, string propertyName, MetadataCollection metadata)
        {
            // If there isn't any metadata, don't write the property.
            if (metadata.Count == 0)
            {
                return;
            }

            writer.WriteObject(propertyName, metadata, static (writer, metadata) =>
            {
                foreach (var (key, value) in metadata)
                {
                    writer.Write(key, value);
                }
            });
        }
    }
}
