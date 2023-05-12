// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal partial class TagHelperResolutionResultJsonConverter
{
    private record struct Data(TagHelperDescriptor[] Descriptors, RazorDiagnostic[] Diagnostics)
    {
        public static readonly PropertyMap<Data> PropertyMap = new(
            (nameof(Data.Descriptors), ReadDescriptors),
            (nameof(Data.Diagnostics), ReadDiagnostics));

        public static void ReadDescriptors(JsonDataReader reader, ref Data data)
            => data.Descriptors = reader.ReadArrayOrEmpty(
                static r => r.ReadNonNullObject(
                    static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: true)));

        public static void ReadDiagnostics(JsonDataReader reader, ref Data data)
            => data.Diagnostics = reader.ReadArrayOrEmpty(static r => r.ReadNonNullObject(ObjectReaders.ReadDiagnosticFromProperties));
    }
}
