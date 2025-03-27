// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Globalization;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class RazorDiagnosticFormatter : ValueFormatter<RazorDiagnostic>
{
    public static readonly ValueFormatter<RazorDiagnostic> Instance = new RazorDiagnosticFormatter();

    private RazorDiagnosticFormatter()
    {
    }

    public override RazorDiagnostic Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(8);

        var id = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var severity = (RazorDiagnosticSeverity)reader.ReadInt32();
        var message = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();

        var filePath = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var absoluteIndex = reader.ReadInt32();
        var lineIndex = reader.ReadInt32();
        var characterIndex = reader.ReadInt32();
        var length = reader.ReadInt32();

        var descriptor = new RazorDiagnosticDescriptor(id, message, severity);
        var span = new SourceSpan(filePath, absoluteIndex, lineIndex, characterIndex, length);

        return RazorDiagnostic.Create(descriptor, span);
    }

    public override void Serialize(ref MessagePackWriter writer, RazorDiagnostic value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(8);

        CachedStringFormatter.Instance.Serialize(ref writer, value.Id, options);
        writer.Write((int)value.Severity);
        CachedStringFormatter.Instance.Serialize(ref writer, value.GetMessage(CultureInfo.CurrentCulture), options);

        var span = value.Span;
        CachedStringFormatter.Instance.Serialize(ref writer, span.FilePath, options);
        writer.Write(span.AbsoluteIndex);
        writer.Write(span.LineIndex);
        writer.Write(span.CharacterIndex);
        writer.Write(span.Length);
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(8);

        CachedStringFormatter.Instance.Skim(ref reader, options); // Id
        reader.Skip(); // Severity
        CachedStringFormatter.Instance.Skim(ref reader, options); // Message

        CachedStringFormatter.Instance.Skim(ref reader, options); // FilePath
        reader.Skip(); // AbsoluteIndex
        reader.Skip(); // LineIndex
        reader.Skip(); // CharacterIndex
        reader.Skip(); // Length
    }
}
