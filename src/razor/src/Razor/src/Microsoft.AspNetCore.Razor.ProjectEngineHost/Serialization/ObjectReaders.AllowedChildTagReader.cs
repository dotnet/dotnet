// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal static partial class ObjectReaders
{
    private record struct AllowedChildTagReader(AllowedChildTagDescriptorBuilder Builder)
    {
        public static readonly PropertyMap<AllowedChildTagReader> PropertyMap = new(
            new(nameof(AllowedChildTagDescriptor.Name), ReadName),
            new(nameof(AllowedChildTagDescriptor.DisplayName), ReadDisplayName),
            new(nameof(AllowedChildTagDescriptor.Diagnostics), ReadDiagnostics));

        private static void ReadName(JsonDataReader reader, ref AllowedChildTagReader arg)
            => arg.Builder.Name = Cached(reader.ReadString());

        private static void ReadDisplayName(JsonDataReader reader, ref AllowedChildTagReader arg)
            => arg.Builder.DisplayName = Cached(reader.ReadString());

        private static void ReadDiagnostics(JsonDataReader reader, ref AllowedChildTagReader arg)
            => reader.ProcessArray(arg.Builder.Diagnostics, ProcessDiagnostic);
    }
}
