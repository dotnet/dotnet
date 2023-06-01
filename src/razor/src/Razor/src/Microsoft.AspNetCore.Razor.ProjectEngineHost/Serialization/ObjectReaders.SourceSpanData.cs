// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Serialization;

internal static partial class ObjectReaders
{
    private record struct SourceSpanData(string? FilePath, int AbsoluteIndex, int LineIndex, int CharacterIndex, int Length)
    {
        public static readonly PropertyMap<SourceSpanData> PropertyMap = new(
            (nameof(FilePath), ReadFilePath),
            (nameof(AbsoluteIndex), ReadAbsoluteIndex),
            (nameof(LineIndex), ReadLineIndex),
            (nameof(CharacterIndex), ReadCharacterIndex),
            (nameof(Length), ReadLength));

        private static void ReadFilePath(JsonDataReader reader, ref SourceSpanData data)
            => data.FilePath = reader.ReadString();

        private static void ReadAbsoluteIndex(JsonDataReader reader, ref SourceSpanData data)
            => data.AbsoluteIndex = reader.ReadInt32();

        private static void ReadLineIndex(JsonDataReader reader, ref SourceSpanData data)
            => data.LineIndex = reader.ReadInt32();

        private static void ReadCharacterIndex(JsonDataReader reader, ref SourceSpanData data)
            => data.CharacterIndex = reader.ReadInt32();

        private static void ReadLength(JsonDataReader reader, ref SourceSpanData data)
            => data.Length = reader.ReadInt32();
    }
}
