// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.AspNetCore.Razor.Language.RequiredAttributeDescriptor;
using SR = Microsoft.AspNetCore.Razor.Serialization.Json.Internal.Strings;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class ObjectReaders
{
    private static readonly StringCache s_stringCache = new();

    [return: NotNullIfNotNull(nameof(str))]
    private static string? Cached(string? str)
    {
        if (str is null)
        {
            return null;
        }

        // Some of the strings used in TagHelperDescriptors are interned by other processes,
        // so we should avoid duplicating those.
        var interned = string.IsInterned(str);
        if (interned != null)
        {
            return interned;
        }

        // We cache all our stings here to prevent them from ballooning memory in our Descriptors.
        return s_stringCache.GetOrAddValue(str);
    }

    public static RazorConfiguration ReadConfigurationFromProperties(JsonDataReader reader)
    {
        var configurationName = reader.ReadNonNullString(nameof(RazorConfiguration.ConfigurationName));
        var languageVersionText = reader.ReadNonNullString(nameof(RazorConfiguration.LanguageVersion));
        var extensions = reader.ReadImmutableArrayOrEmpty(nameof(RazorConfiguration.Extensions),
            static r =>
            {
                var extensionName = r.ReadNonNullString();
                return new RazorExtension(extensionName);
            });

        var languageVersion = RazorLanguageVersion.TryParse(languageVersionText, out var version)
            ? version
            : RazorLanguageVersion.Version_2_1;

        return new(languageVersion, configurationName, extensions);
    }

    public static RazorDiagnostic ReadDiagnostic(JsonDataReader reader)
        => reader.ReadNonNullObject(ReadDiagnosticFromProperties);

    public static RazorDiagnostic ReadDiagnosticFromProperties(JsonDataReader reader)
    {
        var id = reader.ReadNonNullString(nameof(RazorDiagnostic.Id));
        var severity = (RazorDiagnosticSeverity)reader.ReadInt32OrZero(nameof(RazorDiagnostic.Severity));
        var message = reader.ReadNonNullString(WellKnownPropertyNames.Message);

        var filePath = reader.ReadStringOrNull(nameof(SourceSpan.FilePath));
        var absoluteIndex = reader.ReadInt32OrZero(nameof(SourceSpan.AbsoluteIndex));
        var lineIndex = reader.ReadInt32OrZero(nameof(SourceSpan.LineIndex));
        var characterIndex = reader.ReadInt32OrZero(nameof(SourceSpan.CharacterIndex));
        var length = reader.ReadInt32OrZero(nameof(SourceSpan.Length));

        var descriptor = new RazorDiagnosticDescriptor(id, message, severity);
        var span = new SourceSpan(filePath, absoluteIndex, lineIndex, characterIndex, length);

        return RazorDiagnostic.Create(descriptor, span);
    }

    public static ProjectSnapshotHandle ReadProjectSnapshotHandleFromProperties(JsonDataReader reader)
    {
        var projectIdString = reader.ReadNonNullString(nameof(ProjectSnapshotHandle.ProjectId));
        var configuration = reader.ReadObjectOrNull(nameof(ProjectSnapshotHandle.Configuration), ReadConfigurationFromProperties) ?? RazorConfiguration.Default;
        var rootNamespace = reader.ReadStringOrNull(nameof(ProjectSnapshotHandle.RootNamespace));

        var projectId = ProjectId.CreateFromSerialized(Guid.Parse(projectIdString));

        return new(projectId, configuration, rootNamespace);
    }

    public static DocumentSnapshotHandle ReadDocumentSnapshotHandleFromProperties(JsonDataReader reader)
    {
        var filePath = reader.ReadNonNullString(nameof(DocumentSnapshotHandle.FilePath));
        var targetPath = reader.ReadNonNullString(nameof(DocumentSnapshotHandle.TargetPath));
        var fileKind = reader.ReadNonNullString(nameof(DocumentSnapshotHandle.FileKind));

        return new DocumentSnapshotHandle(filePath, targetPath, fileKind);
    }

    public static ProjectWorkspaceState ReadProjectWorkspaceStateFromProperties(JsonDataReader reader)
    {
        var tagHelpers = reader.ReadImmutableArrayOrEmpty(nameof(ProjectWorkspaceState.TagHelpers), static r => ReadTagHelper(r, useCache: true));
        var csharpLanguageVersion = (LanguageVersion)reader.ReadInt32OrZero(nameof(ProjectWorkspaceState.CSharpLanguageVersion));

        return ProjectWorkspaceState.Create(tagHelpers, csharpLanguageVersion);
    }

    public static TagHelperDescriptor ReadTagHelper(JsonDataReader reader, bool useCache)
        => reader.ReadNonNullObject(r => ReadTagHelperFromProperties(r, useCache));

    public static TagHelperDescriptor ReadTagHelperFromProperties(JsonDataReader reader, bool useCache)
    {
        TagHelperDescriptor? tagHelper;
        var checksumWasRead = false;

        // Try reading the optional checksum
        if (reader.TryReadPropertyName(WellKnownPropertyNames.Checksum))
        {
            var checksum = ReadChecksum(reader);
            checksumWasRead = true;

            if (useCache && TagHelperCache.Default.TryGet(checksum, out tagHelper))
            {
                reader.ReadToEndOfCurrentObject();
                return tagHelper;
            }
        }

        var kind = reader.ReadNonNullString(nameof(TagHelperDescriptor.Kind));
        var name = reader.ReadNonNullString(nameof(TagHelperDescriptor.Name));
        var assemblyName = reader.ReadNonNullString(nameof(TagHelperDescriptor.AssemblyName));

        var displayName = reader.ReadStringOrNull(nameof(TagHelperDescriptor.DisplayName));
        var documentationObject = ReadDocumentationObject(reader, nameof(TagHelperDescriptor.Documentation));
        var tagOutputHint = reader.ReadStringOrNull(nameof(TagHelperDescriptor.TagOutputHint));
        var caseSensitive = reader.ReadBooleanOrTrue(nameof(TagHelperDescriptor.CaseSensitive));

        var tagMatchingRules = reader.ReadImmutableArrayOrEmpty(nameof(TagHelperDescriptor.TagMatchingRules), ReadTagMatchingRule);
        var boundAttributes = reader.ReadImmutableArrayOrEmpty(nameof(TagHelperDescriptor.BoundAttributes), ReadBoundAttribute);
        var allowedChildTags = reader.ReadImmutableArrayOrEmpty(nameof(TagHelperDescriptor.AllowedChildTags), ReadAllowedChildTag);

        var metadata = ReadMetadata(reader, nameof(TagHelperDescriptor.Metadata));
        var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(TagHelperDescriptor.Diagnostics), ReadDiagnostic);

        tagHelper = new TagHelperDescriptor(
            Cached(kind), Cached(name), Cached(assemblyName),
            Cached(displayName)!, documentationObject,
            Cached(tagOutputHint), caseSensitive,
            tagMatchingRules, boundAttributes, allowedChildTags,
            metadata, diagnostics);

        if (useCache && checksumWasRead)
        {
            TagHelperCache.Default.TryAdd(tagHelper.Checksum, tagHelper);
        }

        return tagHelper;

        static TagMatchingRuleDescriptor ReadTagMatchingRule(JsonDataReader reader)
        {
            return reader.ReadNonNullObject(ReadFromProperties);

            static TagMatchingRuleDescriptor ReadFromProperties(JsonDataReader reader)
            {
                var tagName = reader.ReadNonNullString(nameof(TagMatchingRuleDescriptor.TagName));
                var parentTag = reader.ReadStringOrNull(nameof(TagMatchingRuleDescriptor.ParentTag));
                var tagStructure = (TagStructure)reader.ReadInt32OrZero(nameof(TagMatchingRuleDescriptor.TagStructure));
                var caseSensitive = reader.ReadBooleanOrTrue(nameof(TagMatchingRuleDescriptor.CaseSensitive));
                var attributes = reader.ReadImmutableArrayOrEmpty(nameof(TagMatchingRuleDescriptor.Attributes), ReadRequiredAttribute);

                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(TagMatchingRuleDescriptor.Diagnostics), ReadDiagnostic);

                return new TagMatchingRuleDescriptor(
                    Cached(tagName), Cached(parentTag),
                    tagStructure, caseSensitive,
                    attributes, diagnostics);
            }
        }

        static RequiredAttributeDescriptor ReadRequiredAttribute(JsonDataReader reader)
        {
            return reader.ReadNonNullObject(ReadFromProperties);

            static RequiredAttributeDescriptor ReadFromProperties(JsonDataReader reader)
            {
                var name = reader.ReadString(nameof(RequiredAttributeDescriptor.Name));
                var nameComparison = (NameComparisonMode)reader.ReadInt32OrZero(nameof(RequiredAttributeDescriptor.NameComparison));
                var caseSensitive = reader.ReadBooleanOrTrue(nameof(RequiredAttributeDescriptor.CaseSensitive));
                var value = reader.ReadStringOrNull(nameof(RequiredAttributeDescriptor.Value));
                var valueComparison = (ValueComparisonMode)reader.ReadInt32OrZero(nameof(RequiredAttributeDescriptor.ValueComparison));
                var displayName = reader.ReadNonNullString(nameof(RequiredAttributeDescriptor.DisplayName));

                var metadata = ReadMetadata(reader, nameof(RequiredAttributeDescriptor.Metadata));
                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(RequiredAttributeDescriptor.Diagnostics), ReadDiagnostic);

                return new RequiredAttributeDescriptor(
                    Cached(name)!, nameComparison,
                    caseSensitive,
                    Cached(value), valueComparison,
                    Cached(displayName), diagnostics, metadata);
            }
        }

        static BoundAttributeDescriptor ReadBoundAttribute(JsonDataReader reader)
        {
            return reader.ReadNonNullObject(ReadFromProperties);

            static BoundAttributeDescriptor ReadFromProperties(JsonDataReader reader)
            {
                var kind = reader.ReadNonNullString(nameof(BoundAttributeDescriptor.Kind));
                var name = reader.ReadString(nameof(BoundAttributeDescriptor.Name));
                var typeName = reader.ReadNonNullString(nameof(BoundAttributeDescriptor.TypeName));
                var isEnum = reader.ReadBooleanOrFalse(nameof(BoundAttributeDescriptor.IsEnum));
                var hasIndexer = reader.ReadBooleanOrFalse(nameof(BoundAttributeDescriptor.HasIndexer));
                var indexerNamePrefix = reader.ReadStringOrNull(nameof(BoundAttributeDescriptor.IndexerNamePrefix));
                var indexerTypeName = reader.ReadStringOrNull(nameof(BoundAttributeDescriptor.IndexerTypeName));
                var displayName = reader.ReadNonNullString(nameof(BoundAttributeDescriptor.DisplayName));
                var containingType = reader.ReadStringOrNull(nameof(BoundAttributeDescriptor.ContainingType));
                var documentationObject = ReadDocumentationObject(reader, nameof(BoundAttributeDescriptor.Documentation));
                var caseSensitive = reader.ReadBooleanOrTrue(nameof(BoundAttributeDescriptor.CaseSensitive));
                var isEditorRequired = reader.ReadBooleanOrFalse(nameof(BoundAttributeDescriptor.IsEditorRequired));
                var parameters = reader.ReadImmutableArrayOrEmpty("BoundAttributeParameters", ReadBoundAttributeParameter);

                var metadata = ReadMetadata(reader, nameof(BoundAttributeDescriptor.Metadata));
                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(BoundAttributeDescriptor.Diagnostics), ReadDiagnostic);

                return new BoundAttributeDescriptor(
                    Cached(kind), Cached(name)!, Cached(typeName), isEnum,
                    hasIndexer, Cached(indexerNamePrefix), Cached(indexerTypeName),
                    documentationObject, Cached(displayName), Cached(containingType),
                    caseSensitive, isEditorRequired, parameters, metadata, diagnostics);
            }
        }

        static BoundAttributeParameterDescriptor ReadBoundAttributeParameter(JsonDataReader reader)
        {
            return reader.ReadNonNullObject(ReadFromProperties);

            static BoundAttributeParameterDescriptor ReadFromProperties(JsonDataReader reader)
            {
                var kind = reader.ReadNonNullString(nameof(BoundAttributeParameterDescriptor.Kind));
                var name = reader.ReadString(nameof(BoundAttributeParameterDescriptor.Name));
                var typeName = reader.ReadNonNullString(nameof(BoundAttributeParameterDescriptor.TypeName));
                var isEnum = reader.ReadBooleanOrFalse(nameof(BoundAttributeParameterDescriptor.IsEnum));
                var displayName = reader.ReadNonNullString(nameof(BoundAttributeParameterDescriptor.DisplayName));
                var documentationObject = ReadDocumentationObject(reader, nameof(BoundAttributeParameterDescriptor.Documentation));
                var caseSensitive = reader.ReadBooleanOrTrue(nameof(BoundAttributeParameterDescriptor.CaseSensitive));

                var metadata = ReadMetadata(reader, nameof(RequiredAttributeDescriptor.Metadata));
                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(BoundAttributeParameterDescriptor.Diagnostics), ReadDiagnostic);

                return new BoundAttributeParameterDescriptor(
                    Cached(kind), Cached(name)!, Cached(typeName),
                    isEnum, documentationObject, Cached(displayName), caseSensitive,
                    metadata, diagnostics);
            }
        }

        static AllowedChildTagDescriptor ReadAllowedChildTag(JsonDataReader reader)
        {
            return reader.ReadNonNullObject(ReadFromProperties);

            static AllowedChildTagDescriptor ReadFromProperties(JsonDataReader reader)
            {
                var name = reader.ReadNonNullString(nameof(AllowedChildTagDescriptor.Name));
                var displayName = reader.ReadNonNullString(nameof(AllowedChildTagDescriptor.DisplayName));
                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(AllowedChildTagDescriptor.Diagnostics), ReadDiagnostic);

                return new AllowedChildTagDescriptor(Cached(name), Cached(displayName), diagnostics);
            }
        }

        static DocumentationObject ReadDocumentationObject(JsonDataReader reader, string propertyName)
        {
            return reader.TryReadPropertyName(propertyName)
                ? ReadCore(reader)
                : default;

            static DocumentationObject ReadCore(JsonDataReader reader)
            {
                if (reader.IsObjectStart)
                {
                    return reader.ReadNonNullObject(static reader =>
                    {
                        var id = (DocumentationId)reader.ReadInt32(nameof(DocumentationDescriptor.Id));
                        // Check to see if the Args property was actually written before trying to read it;
                        // otherwise, assume the args are null.
                        var args = reader.TryReadPropertyName(nameof(DocumentationDescriptor.Args))
                            ? reader.ReadArray(static r => r.ReadValue())
                            : null;

                        if (args is { Length: > 0 and var length })
                        {
                            for (var i = 0; i < length; i++)
                            {
                                if (args[i] is string s)
                                {
                                    args[i] = Cached(s);
                                }
                            }
                        }

                        return DocumentationDescriptor.From(id, args);
                    });
                }
                else
                {
                    return reader.ReadString() switch
                    {
                        string s => Cached(s),
                        null => default(DocumentationObject)
                    };
                }
            }
        }

        static MetadataCollection ReadMetadata(JsonDataReader reader, string propertyName)
        {
            return reader.TryReadPropertyName(propertyName)
                ? reader.ReadNonNullObject(ReadFromProperties)
                : MetadataCollection.Empty;

            static MetadataCollection ReadFromProperties(JsonDataReader reader)
            {
                using var builder = new MetadataBuilder();

                while (reader.TryReadNextPropertyName(out var key))
                {
                    var value = reader.ReadString();
                    builder.Add(Cached(key), Cached(value));
                }

                return builder.Build();
            }
        }
    }

    public static RazorProjectInfo ReadProjectInfoFromProperties(JsonDataReader reader)
    {
        if (!reader.TryReadInt32(WellKnownPropertyNames.Version, out var version) || version != SerializationFormat.Version)
        {
            throw new RazorProjectInfoSerializationException(SR.Unsupported_razor_project_info_version_encountered);
        }

        var projectKeyId = reader.ReadNonNullString(nameof(RazorProjectInfo.ProjectKey));
        var filePath = reader.ReadNonNullString(nameof(RazorProjectInfo.FilePath));
        var configuration = reader.ReadObject(nameof(RazorProjectInfo.Configuration), ReadConfigurationFromProperties) ?? RazorConfiguration.Default;
        var projectWorkspaceState = reader.ReadObject(nameof(RazorProjectInfo.ProjectWorkspaceState), ReadProjectWorkspaceStateFromProperties) ?? ProjectWorkspaceState.Default;
        var rootNamespace = reader.ReadString(nameof(RazorProjectInfo.RootNamespace));
        var documents = reader.ReadImmutableArray(nameof(RazorProjectInfo.Documents), static r => r.ReadNonNullObject(ReadDocumentSnapshotHandleFromProperties));

        var displayName = Path.GetFileNameWithoutExtension(filePath);

        return new RazorProjectInfo(new ProjectKey(projectKeyId), filePath, configuration, rootNamespace, displayName, projectWorkspaceState, documents);
    }

    public static Checksum ReadChecksum(JsonDataReader reader)
        => reader.ReadNonNullObject(ReadChecksumFromProperties);

    public static Checksum ReadChecksumFromProperties(JsonDataReader reader)
    {
        var data1 = reader.ReadInt64(nameof(Checksum.HashData.Data1));
        var data2 = reader.ReadInt64(nameof(Checksum.HashData.Data2));
        var data3 = reader.ReadInt64(nameof(Checksum.HashData.Data3));
        var data4 = reader.ReadInt64(nameof(Checksum.HashData.Data4));

        var hashData = new Checksum.HashData(data1, data2, data3, data4);

        return new Checksum(hashData);
    }
}
