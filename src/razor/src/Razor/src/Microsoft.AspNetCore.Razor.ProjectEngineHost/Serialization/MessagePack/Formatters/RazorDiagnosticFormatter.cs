// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Globalization;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class RazorDiagnosticFormatter : MessagePackFormatter<RazorDiagnostic>
{
    public static readonly MessagePackFormatter<RazorDiagnostic> Instance = new RazorDiagnosticFormatter();

    private RazorDiagnosticFormatter()
    {
    }

    public override RazorDiagnostic Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var id = DeserializeString(ref reader, options);
        var severity = (RazorDiagnosticSeverity)reader.ReadInt32();
        var message = DeserializeString(ref reader, options);

        var filePath = AllowNull.DeserializeString(ref reader, options);
        var absoluteIndex = reader.ReadInt32();
        var lineIndex = reader.ReadInt32();
        var characterIndex = reader.ReadInt32();
        var length = reader.ReadInt32();

        var descriptor = new RazorDiagnosticDescriptor(id, MessageFormat(message), severity);
        var span = new SourceSpan(filePath, absoluteIndex, lineIndex, characterIndex, length);

        return RazorDiagnostic.Create(descriptor, span);

        static Func<string> MessageFormat(string message)
        {
            return () => message;
        }
    }

    public override void Serialize(ref MessagePackWriter writer, RazorDiagnostic value, MessagePackSerializerOptions options)
    {
        writer.Write(value.Id);
        writer.Write((int)value.Severity);
        writer.Write(value.GetMessage(CultureInfo.CurrentCulture));

        var span = value.Span;
        writer.Write(span.FilePath);
        writer.Write(span.AbsoluteIndex);
        writer.Write(span.LineIndex);
        writer.Write(span.CharacterIndex);
        writer.Write(span.Length);
    }
}
