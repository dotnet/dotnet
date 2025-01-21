// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class ObjectReaders
{
    public static Checksum ReadChecksum(JsonDataReader reader)
        => reader.ReadNonNullObject(ReadChecksumFromProperties);

    public static Checksum ReadChecksumFromProperties(JsonDataReader reader)
    {
        var data1 = reader.ReadInt64(nameof(Checksum.HashData.Data1));
        var data2 = reader.ReadInt64(nameof(Checksum.HashData.Data2));
        var data3 = reader.ReadInt64(nameof(Checksum.HashData.Data3));
        var data4 = reader.ReadInt64(nameof(Checksum.HashData.Data4));

        var hashData = new Checksum.HashData(data1, data2, data3, data4);

        return new Checksum(hashData);
    }

    public static RazorConfiguration ReadConfigurationFromProperties(JsonDataReader reader)
    {
        var configurationName = reader.ReadNonNullString(nameof(RazorConfiguration.ConfigurationName));
        var languageVersionText = reader.ReadNonNullString(nameof(RazorConfiguration.LanguageVersion));
        var csharpLanguageVersion = (LanguageVersion)reader.ReadInt32OrZero(nameof(RazorConfiguration.CSharpLanguageVersion));
        var suppressAddComponentParameter = reader.ReadBooleanOrFalse(nameof(RazorConfiguration.SuppressAddComponentParameter));
        var useConsolidatedMvcViews = reader.ReadBooleanOrTrue(nameof(RazorConfiguration.UseConsolidatedMvcViews));
        var useRoslynTokenizer = reader.ReadBooleanOrFalse(nameof(RazorConfiguration.UseRoslynTokenizer));
        var preprocessorSymbols = reader.ReadImmutableArrayOrEmpty(nameof(RazorConfiguration.PreprocessorSymbols), r => r.ReadNonNullString());
        var extensions = reader.ReadImmutableArrayOrEmpty(nameof(RazorConfiguration.Extensions),
            static r =>
            {
                var extensionName = r.ReadNonNullString();
                return new RazorExtension(extensionName);
            });

        var languageVersion = RazorLanguageVersion.TryParse(languageVersionText, out var version)
            ? version
            : RazorLanguageVersion.Version_2_1;

        return new(
            languageVersion,
            configurationName,
            extensions,
            csharpLanguageVersion,
            UseConsolidatedMvcViews: useConsolidatedMvcViews,
            SuppressAddComponentParameter: suppressAddComponentParameter,
            UseRoslynTokenizer: useRoslynTokenizer,
            PreprocessorSymbols: preprocessorSymbols);
    }

    public static RazorDiagnostic ReadDiagnostic(JsonDataReader reader)
        => reader.ReadNonNullObject(ReadDiagnosticFromProperties);

    public static RazorDiagnostic ReadDiagnosticFromProperties(JsonDataReader reader)
    {
        var id = reader.ReadNonNullString(nameof(RazorDiagnostic.Id));
        var severity = (RazorDiagnosticSeverity)reader.ReadInt32OrZero(nameof(RazorDiagnostic.Severity));
        var message = reader.ReadNonNullString(WellKnownPropertyNames.Message);

        var filePath = reader.ReadStringOrNull(nameof(SourceSpan.FilePath));
        var absoluteIndex = reader.ReadInt32OrZero(nameof(SourceSpan.AbsoluteIndex));
        var lineIndex = reader.ReadInt32OrZero(nameof(SourceSpan.LineIndex));
        var characterIndex = reader.ReadInt32OrZero(nameof(SourceSpan.CharacterIndex));
        var length = reader.ReadInt32OrZero(nameof(SourceSpan.Length));

        var descriptor = new RazorDiagnosticDescriptor(id, message, severity);
        var span = new SourceSpan(filePath, absoluteIndex, lineIndex, characterIndex, length);

        return RazorDiagnostic.Create(descriptor, span);
    }
}
