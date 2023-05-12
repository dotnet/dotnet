// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal partial class ProjectSnapshotHandleJsonConverter
{
    private record struct Data(string FilePath, RazorConfiguration? Configuration, string? RootNamespace)
    {
        public static readonly PropertyMap<Data> PropertyMap = new(
            (nameof(FilePath), ReadFilePath),
            (nameof(Configuration), ReadConfiguration),
            (nameof(RootNamespace), ReadRootNamespace));

        private static void ReadFilePath(JsonDataReader reader, ref Data data)
            => data.FilePath = reader.ReadNonNullString();

        private static void ReadConfiguration(JsonDataReader reader, ref Data data)
            => data.Configuration = reader.ReadObject(ObjectReaders.ReadConfigurationFromProperties);

        private static void ReadRootNamespace(JsonDataReader reader, ref Data data)
            => data.RootNamespace = reader.ReadString();
    }
}
