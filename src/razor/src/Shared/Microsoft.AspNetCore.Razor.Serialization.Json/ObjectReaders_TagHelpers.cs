// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
#if JSONSERIALIZATION_ENABLETAGHELPERCACHE
using Microsoft.CodeAnalysis.Razor.Utilities;
#endif

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

#if JSONSERIALIZATION_ENABLETAGHELPERCACHE
    public static TagHelperDescriptor ReadTagHelper(JsonDataReader reader, bool useCache)
        => reader.ReadNonNullObject(r => ReadTagHelperFromProperties(r, useCache));

    public static TagHelperDescriptor ReadTagHelper(JsonDataReader reader)
        => reader.ReadNonNullObject(r => ReadTagHelperFromProperties(r, useCache: false));
#else
    public static TagHelperDescriptor ReadTagHelper(JsonDataReader reader)
        => reader.ReadNonNullObject(ReadTagHelperFromProperties);
#endif

#if JSONSERIALIZATION_ENABLETAGHELPERCACHE
    public static TagHelperDescriptor ReadTagHelperFromProperties(JsonDataReader reader)
        => ReadTagHelperFromProperties(reader, useCache: false);

    public static TagHelperDescriptor ReadTagHelperFromProperties(JsonDataReader reader, bool useCache)
#else
    public static TagHelperDescriptor ReadTagHelperFromProperties(JsonDataReader reader)
#endif
    {
        TagHelperDescriptor? tagHelper;

        var checksum = reader.ReadNonNullObject(WellKnownPropertyNames.Checksum, ReadChecksumFromProperties);

#if JSONSERIALIZATION_ENABLETAGHELPERCACHE
        // Try reading the optional checksum
        if (reader.TryReadPropertyName(WellKnownPropertyNames.Checksum))
        {
            if (useCache && TagHelperCache.Default.TryGet(checksum, out tagHelper))
            {
                reader.ReadToEndOfCurrentObject();
                return tagHelper;
            }
        }
#endif

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

#if JSONSERIALIZATION_ENABLETAGHELPERCACHE
        if (useCache)
        {
            TagHelperCache.Default.TryAdd(tagHelper.Checksum, tagHelper);
        }
#endif

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
                var flags = (RequiredAttributeDescriptorFlags)reader.ReadByte(nameof(RequiredAttributeDescriptor.Flags));
                var name = reader.ReadString(nameof(RequiredAttributeDescriptor.Name));
                var nameComparison = (RequiredAttributeNameComparison)reader.ReadByteOrZero(nameof(RequiredAttributeDescriptor.NameComparison));
                var value = reader.ReadStringOrNull(nameof(RequiredAttributeDescriptor.Value));
                var valueComparison = (RequiredAttributeValueComparison)reader.ReadByteOrZero(nameof(RequiredAttributeDescriptor.ValueComparison));

                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(RequiredAttributeDescriptor.Diagnostics), ReadDiagnostic);

                return new RequiredAttributeDescriptor(
                    flags, Cached(name)!, nameComparison, Cached(value), valueComparison, diagnostics);
            }
        }

        static BoundAttributeDescriptor ReadBoundAttribute(JsonDataReader reader)
        {
            return reader.ReadNonNullObject(ReadFromProperties);

            static BoundAttributeDescriptor ReadFromProperties(JsonDataReader reader)
            {
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
                    Cached(name)!, Cached(typeName), isEnum,
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
                var flags = (BoundAttributeParameterFlags)reader.ReadInt32(nameof(BoundAttributeParameterDescriptor.Flags));
                var name = reader.ReadString(nameof(BoundAttributeParameterDescriptor.Name));
                var propertyName = reader.ReadNonNullString(nameof(BoundAttributeParameterDescriptor.PropertyName));
                var typeNameObject = ReadTypeNameObject(reader, nameof(BoundAttributeParameterDescriptor.TypeName));
                var documentationObject = ReadDocumentationObject(reader, nameof(BoundAttributeParameterDescriptor.Documentation));
                var diagnostics = reader.ReadImmutableArrayOrEmpty(nameof(BoundAttributeParameterDescriptor.Diagnostics), ReadDiagnostic);

                return new BoundAttributeParameterDescriptor(
                    flags, Cached(name)!, Cached(propertyName), typeNameObject, documentationObject, diagnostics);
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

        static TypeNameObject ReadTypeNameObject(JsonDataReader reader, string propertyName)
        {
            if (!reader.TryReadPropertyName(propertyName))
            {
                return default;
            }

            if (reader.IsInteger)
            {
                var index = reader.ReadByte();
                return new(index);
            }

            Debug.Assert(reader.IsString);

            var fullName = reader.ReadNonNullString();
            return new(Cached(fullName));
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
}
