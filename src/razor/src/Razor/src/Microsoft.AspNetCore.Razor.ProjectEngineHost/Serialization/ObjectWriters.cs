// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Serialization;

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

        if (value.LanguageVersion == RazorLanguageVersion.Experimental)
        {
            writer.Write(nameof(value.LanguageVersion), "Experimental");
        }
        else
        {
            writer.Write(nameof(value.LanguageVersion), value.LanguageVersion.ToString());
        }

        writer.WriteArray(nameof(value.Extensions), value.Extensions, Write);
    }

    public static void Write(JsonDataWriter writer, RazorDiagnostic? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorDiagnostic value)
    {
        writer.Write(nameof(value.Id), value.Id);
        writer.Write(nameof(value.Severity), (int)value.Severity);
        writer.Write("Message", value.GetMessage(CultureInfo.CurrentCulture));
        writer.WriteObject(nameof(value.Span), value.Span, static (writer, value) =>
        {
            writer.Write(nameof(value.FilePath), value.FilePath);
            writer.Write(nameof(value.AbsoluteIndex), value.AbsoluteIndex);
            writer.Write(nameof(value.LineIndex), value.LineIndex);
            writer.Write(nameof(value.CharacterIndex), value.CharacterIndex);
            writer.Write(nameof(value.Length), value.Length);
        });
    }

    public static void Write(JsonDataWriter writer, ProjectSnapshotHandle? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandle value)
    {
        writer.Write(nameof(value.ProjectId), value.ProjectId.Id.ToString());
        writer.WriteObjectIfNotNull(nameof(value.Configuration), value.Configuration, WriteProperties);
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
        writer.WriteArray(nameof(value.TagHelpers), value.TagHelpers, Write);
        writer.Write(nameof(value.CSharpLanguageVersion), (int)value.CSharpLanguageVersion);
    }

    public static void Write(JsonDataWriter writer, TagHelperDescriptor? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, TagHelperDescriptor value)
    {
        writer.Write(RazorSerializationConstants.HashCodePropertyName, TagHelperDescriptorCache.GetTagHelperDescriptorCacheId(value));
        writer.Write(nameof(value.Kind), value.Kind);
        writer.Write(nameof(value.Name), value.Name);
        writer.Write(nameof(value.AssemblyName), value.AssemblyName);
        WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
        writer.WriteIfNotNull(nameof(value.TagOutputHint), value.TagOutputHint);
        writer.Write(nameof(value.CaseSensitive), value.CaseSensitive);
        writer.WriteArray(nameof(value.TagMatchingRules), value.TagMatchingRules, WriteTagMatchingRule);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.BoundAttributes), value.BoundAttributes, WriteBoundAttribute);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.AllowedChildTags), value.AllowedChildTags, WriteAllowedChildTag);
        writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
        writer.WriteObject(nameof(value.Metadata), value.Metadata, WriteMetadata);

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
                writer.WriteIfNotNull(nameof(value.Value), value.Value);
                writer.WriteIfNotZero(nameof(value.ValueComparison), (int)value.ValueComparison);
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);

                if (value.Metadata is { Count: > 0 })
                {
                    writer.WriteObject(nameof(value.Metadata), value.Metadata, WriteMetadata);
                }
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
                writer.WriteIfNotFalse(nameof(value.IsEditorRequired), value.IsEditorRequired);
                writer.WriteIfNotNull(nameof(value.IndexerNamePrefix), value.IndexerNamePrefix);
                writer.WriteIfNotNull(nameof(value.IndexerTypeName), value.IndexerTypeName);
                WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
                writer.WriteObject(nameof(value.Metadata), value.Metadata, WriteMetadata);
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.BoundAttributeParameters), value.BoundAttributeParameters, WriteBoundAttributeParameter);
            });
        }

        static void WriteBoundAttributeParameter(JsonDataWriter writer, BoundAttributeParameterDescriptor value)
        {
            writer.WriteObject(value, static (writer, value) =>
            {
                writer.Write(nameof(value.Name), value.Name);
                writer.Write(nameof(value.TypeName), value.TypeName);
                writer.WriteIfNotFalse(nameof(value.IsEnum), value.IsEnum);
                WriteDocumentationObject(writer, nameof(value.Documentation), value.DocumentationObject);
                writer.WriteArrayIfNotNullOrEmpty(nameof(value.Diagnostics), value.Diagnostics, Write);
                writer.WriteObject(nameof(value.Metadata), value.Metadata, WriteMetadata);
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

        static void WriteMetadata(JsonDataWriter writer, IReadOnlyDictionary<string, string> metadata)
        {
            foreach (var (key, value) in metadata)
            {
                writer.Write(key, value);
            }
        }
    }

    public static void Write(JsonDataWriter writer, ProjectRazorJson value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, ProjectRazorJson value)
    {
        writer.Write(nameof(value.SerializedFilePath), value.SerializedFilePath);
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.WriteObject(nameof(value.Configuration), value.Configuration, WriteProperties);
        writer.WriteObject(nameof(value.ProjectWorkspaceState), value.ProjectWorkspaceState, WriteProperties);
        writer.Write(nameof(value.RootNamespace), value.RootNamespace);
        writer.WriteArray(nameof(value.Documents), value.Documents, Write);
        writer.Write("SerializationFormat", ProjectSerializationFormat.Version);
    }
}
