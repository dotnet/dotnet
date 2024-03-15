// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NETCOREAPP
using System.Collections.Generic;
#endif
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static class ObjectWriters
{
    public static void Write(JsonDataWriter writer, RazorExtension? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorExtension value)
    {
        writer.Write(nameof(value.ExtensionName), value.ExtensionName);
    }

    public static void Write(JsonDataWriter writer, RazorConfiguration? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorConfiguration value)
    {
        writer.Write(nameof(value.ConfigurationName), value.ConfigurationName);

        var languageVersionText = value.LanguageVersion == RazorLanguageVersion.Experimental
            ? nameof(RazorLanguageVersion.Experimental)
            : value.LanguageVersion.ToString();

        writer.Write(nameof(value.LanguageVersion), languageVersionText);
        writer.Write(nameof(value.ForceRuntimeCodeGeneration), value.ForceRuntimeCodeGeneration);

        writer.WriteArrayIfNotNullOrEmpty(nameof(value.Extensions), value.Extensions, static (w, v) => w.Write(v.ExtensionName));
    }

    public static void Write(JsonDataWriter writer, RazorDiagnostic? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorDiagnostic value)
    {
        writer.Write(nameof(value.Id), value.Id);
        writer.Write(nameof(value.Severity), (int)value.Severity);
        writer.Write(WellKnownPropertyNames.Message, value.GetMessage(CultureInfo.CurrentCulture));

        var span = value.Span;
        writer.WriteIfNotNull(nameof(span.FilePath), span.FilePath);
        writer.WriteIfNotZero(nameof(span.AbsoluteIndex), span.AbsoluteIndex);
        writer.WriteIfNotZero(nameof(span.LineIndex), span.LineIndex);
        writer.WriteIfNotZero(nameof(span.CharacterIndex), span.CharacterIndex);
        writer.WriteIfNotZero(nameof(span.Length), span.Length);
    }

    public static void Write(JsonDataWriter writer, ProjectSnapshotHandle? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandle value)
    {
        writer.Write(nameof(value.ProjectId), value.ProjectId.Id.ToString());
        writer.WriteObject(nameof(value.Configuration), value.Configuration, WriteProperties);
        writer.WriteIfNotNull(nameof(value.RootNamespace), value.RootNamespace);
    }

    public static void Write(JsonDataWriter writer, DocumentSnapshotHandle? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, DocumentSnapshotHandle value)
    {
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.Write(nameof(value.TargetPath), value.TargetPath);
        writer.Write(nameof(value.FileKind), value.FileKind);
    }

    public static void Write(JsonDataWriter writer, ProjectWorkspaceState? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectWorkspaceState value)
    {
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.TagHelpers), value.TagHelpers, Write);
        writer.WriteIfNotZero(nameof(value.CSharpLanguageVersion), (int)value.CSharpLanguageVersion);
    }

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
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.TagMatchingRules), value.TagMatchingRules, WriteTagMatchingRule);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.BoundAttributes), value.BoundAttributes, WriteBoundAttribute);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.AllowedChildTags), value.AllowedChildTags, WriteAllowedChildTag);
        WriteMetadata(writer, nameof(value.Metadata), value.Metadata);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);

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
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Attributes), value.Attributes, WriteRequiredAttribute);
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
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
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
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
                WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
                writer.WriteIfNotTrue(nameof(value.CaseSensitive), value.CaseSensitive);
                writer.WriteIfNotFalse(nameof(value.IsEditorRequired), value.IsEditorRequired);
                writer.WriteArrayIfNotNullOrEmpty("BoundAttributeParameters", value.Parameters, WriteBoundAttributeParameter);

                WriteMetadata(writer, nameof(value.Metadata), value.Metadata);
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
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
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
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

    public static void Write(JsonDataWriter writer, RazorProjectInfo value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorProjectInfo value)
    {
        writer.Write(WellKnownPropertyNames.Version, SerializationFormat.Version);
        writer.Write(nameof(value.SerializedFilePath), value.SerializedFilePath);
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.WriteObject(nameof(value.Configuration), value.Configuration, WriteProperties);
        writer.WriteObject(nameof(value.ProjectWorkspaceState), value.ProjectWorkspaceState, WriteProperties);
        writer.Write(nameof(value.RootNamespace), value.RootNamespace);
        writer.WriteArray(nameof(value.Documents), value.Documents, Write);
    }

    public static void Write(JsonDataWriter writer, Checksum value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, Checksum value)
    {
        var data = value.Data;

        writer.Write(nameof(data.Data1), data.Data1);
        writer.Write(nameof(data.Data2), data.Data2);
        writer.Write(nameof(data.Data3), data.Data3);
        writer.Write(nameof(data.Data4), data.Data4);
    }
}
