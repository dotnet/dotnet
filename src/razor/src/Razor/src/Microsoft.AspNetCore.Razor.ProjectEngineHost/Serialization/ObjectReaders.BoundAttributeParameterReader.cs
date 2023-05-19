// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal static partial class ObjectReaders
{
    private record struct BoundAttributeParameterReader(BoundAttributeParameterDescriptorBuilder Builder)
    {
        public static readonly PropertyMap<BoundAttributeParameterReader> PropertyMap = new(
            new(nameof(BoundAttributeParameterDescriptor.Kind), ReadKind),
            new(nameof(BoundAttributeParameterDescriptor.Name), ReadName),
            new(nameof(BoundAttributeParameterDescriptor.TypeName), ReadTypeName),
            new(nameof(BoundAttributeParameterDescriptor.IsEnum), ReadIsEnum),
            new(nameof(BoundAttributeParameterDescriptor.Documentation), ReadDocumentation),
            new(nameof(BoundAttributeParameterDescriptor.Metadata), ReadMetadata),
            new(nameof(BoundAttributeParameterDescriptor.Diagnostics), ReadDiagnostics));

        private static void ReadKind(JsonDataReader reader, ref BoundAttributeParameterReader arg)
        {
            // In old serialized files, Kind might appear, though it isn't meaningful.
            _ = reader.ReadString();
        }

        private static void ReadName(JsonDataReader reader, ref BoundAttributeParameterReader arg)
            => arg.Builder.Name = Cached(reader.ReadString());

        private static void ReadTypeName(JsonDataReader reader, ref BoundAttributeParameterReader arg)
            => arg.Builder.TypeName = Cached(reader.ReadString());

        private static void ReadIsEnum(JsonDataReader reader, ref BoundAttributeParameterReader arg)
            => arg.Builder.IsEnum = reader.ReadBoolean();

        private static void ReadDocumentation(JsonDataReader reader, ref BoundAttributeParameterReader arg)
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

        private static void ReadMetadata(JsonDataReader reader, ref BoundAttributeParameterReader arg)
            => reader.ProcessObject(arg.Builder.Metadata, ProcessMetadata);

        private static void ReadDiagnostics(JsonDataReader reader, ref BoundAttributeParameterReader arg)
            => reader.ProcessArray(arg.Builder.Diagnostics, ProcessDiagnostic);
    }
}
