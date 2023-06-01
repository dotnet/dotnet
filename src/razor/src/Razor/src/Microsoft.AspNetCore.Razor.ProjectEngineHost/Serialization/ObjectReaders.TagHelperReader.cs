// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal static partial class ObjectReaders
{
    private record struct TagHelperReader(TagHelperDescriptorBuilder Builder)
    {
        public static readonly PropertyMap<TagHelperReader> PropertyMap = new(
            (nameof(TagHelperDescriptor.Documentation), ReadDocumentation),
            (nameof(TagHelperDescriptor.TagOutputHint), ReadTagOutputHint),
            (nameof(TagHelperDescriptor.CaseSensitive), ReadCaseSensitive),
            (nameof(TagHelperDescriptor.TagMatchingRules), ReadTagMatchingRules),
            (nameof(TagHelperDescriptor.BoundAttributes), ReadBoundAttributes),
            (nameof(TagHelperDescriptor.AllowedChildTags), ReadAllowedChildTags),
            (nameof(TagHelperDescriptor.Diagnostics), ReadDiagnostics),
            (nameof(TagHelperDescriptor.Metadata), ReadMetadata));

        private static void ReadDocumentation(JsonDataReader reader, ref TagHelperReader arg)
        {
            var documentationObject = ReadDocumentationObject(reader);

            if (documentationObject is string text)
            {
                arg.Builder.SetDocumentation(Cached(text));
            }
            else
            {
                arg.Builder.SetDocumentation(documentationObject as DocumentationDescriptor);
            }
        }

        private static void ReadTagOutputHint(JsonDataReader reader, ref TagHelperReader arg)
            => arg.Builder.TagOutputHint = Cached(reader.ReadString());

        private static void ReadCaseSensitive(JsonDataReader reader, ref TagHelperReader arg)
            => arg.Builder.CaseSensitive = reader.ReadBoolean();

        private static void ReadTagMatchingRules(JsonDataReader reader, ref TagHelperReader arg)
        {
            reader.ProcessArray(arg.Builder, static (reader, builder) =>
            {
                builder.TagMatchingRule(ruleBuilder =>
                {
                    reader.ProcessObject(new TagMatchingRuleReader(ruleBuilder), TagMatchingRuleReader.PropertyMap);
                });
            });
        }

        private static void ReadBoundAttributes(JsonDataReader reader, ref TagHelperReader arg)
        {
            reader.ProcessArray(arg.Builder, static (reader, builder) =>
            {
                builder.BindAttribute(attributeBuilder =>
                {
                    reader.ProcessObject(new BoundAttributeReader(attributeBuilder), BoundAttributeReader.PropertyMap);
                });
            });
        }

        private static void ReadAllowedChildTags(JsonDataReader reader, ref TagHelperReader arg)
        {
            reader.ProcessArray(arg.Builder, static (reader, builder) =>
            {
                builder.AllowChildTag(childTagBuilder =>
                {
                    reader.ProcessObject(new AllowedChildTagReader(childTagBuilder), AllowedChildTagReader.PropertyMap);
                });
            });
        }

        private static void ReadDiagnostics(JsonDataReader reader, ref TagHelperReader arg)
            => reader.ProcessArray(arg.Builder.Diagnostics, ProcessDiagnostic);

        private static void ReadMetadata(JsonDataReader reader, ref TagHelperReader arg)
            => reader.ProcessObject(arg.Builder.Metadata, ProcessMetadata);
    }
}
