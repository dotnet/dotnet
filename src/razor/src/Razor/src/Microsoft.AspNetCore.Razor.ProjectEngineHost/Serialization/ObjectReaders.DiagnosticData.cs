// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal static partial class ObjectReaders
{
    private record struct DiagnosticData(string Id, RazorDiagnosticSeverity Severity, string Message, SourceSpan Span)
    {
        public static readonly PropertyMap<DiagnosticData> PropertyMap = new(
            (nameof(Id), ReadId),
            (nameof(Severity), ReadSeverity),
            (nameof(Message), ReadMessage),
            (nameof(Span), ReadSpan));

        private static void ReadId(JsonDataReader reader, ref DiagnosticData data)
            => data.Id = reader.ReadNonNullString();

        private static void ReadSeverity(JsonDataReader reader, ref DiagnosticData data)
            => data.Severity = (RazorDiagnosticSeverity)reader.ReadInt32();

        private static void ReadMessage(JsonDataReader reader, ref DiagnosticData data)
            => data.Message = reader.ReadNonNullString();

        private static void ReadSpan(JsonDataReader reader, ref DiagnosticData data)
        {
            SourceSpanData span = default;
            reader.ReadObjectData(ref span, SourceSpanData.PropertyMap);
            data.Span = new SourceSpan(span.FilePath, span.AbsoluteIndex, span.LineIndex, span.CharacterIndex, span.Length);
        }
    }
}
