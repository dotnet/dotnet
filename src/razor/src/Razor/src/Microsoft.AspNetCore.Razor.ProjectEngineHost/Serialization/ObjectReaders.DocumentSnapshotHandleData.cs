// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Serialization;

internal static partial class ObjectReaders
{
    private record struct DocumentSnapshotHandleData(string FilePath, string TargetPath, string FileKind)
    {
        public static readonly PropertyMap<DocumentSnapshotHandleData> PropertyMap = new(
            (nameof(FilePath), ReadFilePath),
            (nameof(TargetPath), ReadTargetPath),
            (nameof(FileKind), ReadFileKind));

        private static void ReadFilePath(JsonDataReader reader, ref DocumentSnapshotHandleData data)
            => data.FilePath = reader.ReadNonNullString();

        private static void ReadTargetPath(JsonDataReader reader, ref DocumentSnapshotHandleData data)
            => data.TargetPath = reader.ReadNonNullString();

        private static void ReadFileKind(JsonDataReader reader, ref DocumentSnapshotHandleData data)
            => data.FileKind = reader.ReadNonNullString();
    }
}
