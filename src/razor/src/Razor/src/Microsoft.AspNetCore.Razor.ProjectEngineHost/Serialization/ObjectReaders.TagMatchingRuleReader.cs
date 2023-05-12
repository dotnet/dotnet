// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal static partial class ObjectReaders
{
    private record struct TagMatchingRuleReader(TagMatchingRuleDescriptorBuilder Builder)
    {
        public static readonly PropertyMap<TagMatchingRuleReader> PropertyMap = new(
            (nameof(TagMatchingRuleDescriptor.TagName), ReadTagName),
            (nameof(TagMatchingRuleDescriptor.ParentTag), ReadParentTag),
            (nameof(TagMatchingRuleDescriptor.TagStructure), ReadTagStructure),
            (nameof(TagMatchingRuleDescriptor.Attributes), ReadAttributes),
            (nameof(TagMatchingRuleDescriptor.Diagnostics), ReadDiagnostics));

        private static void ReadTagName(JsonDataReader reader, ref TagMatchingRuleReader arg)
            => arg.Builder.TagName = Cached(reader.ReadString());

        private static void ReadParentTag(JsonDataReader reader, ref TagMatchingRuleReader arg)
            => arg.Builder.ParentTag = Cached(reader.ReadString());

        private static void ReadTagStructure(JsonDataReader reader, ref TagMatchingRuleReader arg)
            => arg.Builder.TagStructure = (TagStructure)reader.ReadInt32();

        private static void ReadAttributes(JsonDataReader reader, ref TagMatchingRuleReader arg)
        {
            reader.ProcessArray(arg.Builder, static (reader, builder) =>
            {
                builder.Attribute(attributeBuilder =>
                {
                    reader.ProcessObject(new RequiredAttributeReader(attributeBuilder), RequiredAttributeReader.PropertyMap);
                });
            });
        }

        private static void ReadDiagnostics(JsonDataReader reader, ref TagMatchingRuleReader arg)
            => reader.ProcessArray(arg.Builder.Diagnostics, ProcessDiagnostic);
    }
}
