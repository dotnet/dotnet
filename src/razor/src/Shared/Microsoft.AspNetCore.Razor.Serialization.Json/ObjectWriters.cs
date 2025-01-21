// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class ObjectWriters
{
    public static void Write(JsonDataWriter writer, Checksum value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, Checksum value)
    {
        var data = value.Data;

        writer.Write(nameof(data.Data1), data.Data1);
        writer.Write(nameof(data.Data2), data.Data2);
        writer.Write(nameof(data.Data3), data.Data3);
        writer.Write(nameof(data.Data4), data.Data4);
    }

    public static void Write(JsonDataWriter writer, RazorConfiguration? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorConfiguration value)
    {
        writer.Write(nameof(value.ConfigurationName), value.ConfigurationName);

        var languageVersionText = value.LanguageVersion == RazorLanguageVersion.Experimental
            ? nameof(RazorLanguageVersion.Experimental)
            : value.LanguageVersion.ToString();

        writer.Write(nameof(value.LanguageVersion), languageVersionText);

        writer.WriteIfNotZero(nameof(value.CSharpLanguageVersion), (int)value.CSharpLanguageVersion);
        writer.WriteIfNotFalse(nameof(value.SuppressAddComponentParameter), value.SuppressAddComponentParameter);
        writer.WriteIfNotTrue(nameof(value.UseConsolidatedMvcViews), value.UseConsolidatedMvcViews);
        writer.WriteIfNotFalse(nameof(value.UseRoslynTokenizer), value.UseRoslynTokenizer);
        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.PreprocessorSymbols), value.PreprocessorSymbols, static (w, v) => w.Write(v));

        writer.WriteArrayIfNotDefaultOrEmpty(nameof(value.Extensions), value.Extensions, static (w, v) => w.Write(v.ExtensionName));
    }

    public static void Write(JsonDataWriter writer, RazorDiagnostic? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorDiagnostic value)
    {
        writer.Write(nameof(value.Id), value.Id);
        writer.Write(nameof(value.Severity), (int)value.Severity);
        writer.Write(WellKnownPropertyNames.Message, value.GetMessage(CultureInfo.CurrentCulture));

        var span = value.Span;
        writer.WriteIfNotNull(nameof(span.FilePath), span.FilePath);
        writer.WriteIfNotZero(nameof(span.AbsoluteIndex), span.AbsoluteIndex);
        writer.WriteIfNotZero(nameof(span.LineIndex), span.LineIndex);
        writer.WriteIfNotZero(nameof(span.CharacterIndex), span.CharacterIndex);
        writer.WriteIfNotZero(nameof(span.Length), span.Length);
    }

    public static void Write(JsonDataWriter writer, RazorExtension? value)
        => writer.WriteObject(value, WriteProperties);

    public static void WriteProperties(JsonDataWriter writer, RazorExtension value)
    {
        writer.Write(nameof(value.ExtensionName), value.ExtensionName);
    }
}
