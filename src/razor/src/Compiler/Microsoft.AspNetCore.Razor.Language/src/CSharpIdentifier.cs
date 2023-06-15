// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class CSharpIdentifier
{
    public static string GetClassNameFromPath(string path)
    {
        var span = path.AsSpanOrDefault();

        if (span.Length == 0)
        {
            return path;
        }

        const string cshtmlExtension = ".cshtml";

        if (span.EndsWith(cshtmlExtension.AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            span = span[..^cshtmlExtension.Length];
        }

        return SanitizeIdentifier(span);
    }

    // CSharp Spec §2.4.2
    private static bool IsIdentifierStart(char character)
    {
        return char.IsLetter(character) ||
            character == '_' ||
            CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.LetterNumber;
    }

    public static bool IsIdentifierPart(char character)
    {
        return char.IsDigit(character) ||
               IsIdentifierStart(character) ||
               IsIdentifierPartByUnicodeCategory(character);
    }

    private static bool IsIdentifierPartByUnicodeCategory(char character)
    {
        var category = CharUnicodeInfo.GetUnicodeCategory(character);

        return category is UnicodeCategory.NonSpacingMark or // Mn
                           UnicodeCategory.SpacingCombiningMark or // Mc
                           UnicodeCategory.ConnectorPunctuation or // Pc
                           UnicodeCategory.Format; // Cf
    }

    public static string SanitizeIdentifier(ReadOnlySpan<char> inputName)
    {
        if (inputName.Length == 0)
        {
            return string.Empty;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        var firstChar = inputName[0];
        if (!IsIdentifierStart(firstChar) && IsIdentifierPart(firstChar))
        {
            builder.SetCapacityIfLarger(inputName.Length + 1);
            builder.Append('_');
        }
        else
        {
            builder.SetCapacityIfLarger(inputName.Length);
        }

        foreach (var ch in inputName)
        {
            builder.Append(IsIdentifierPart(ch) ? ch : '_');
        }

        return builder.ToString();
    }

    public static void AppendSanitized(StringBuilder builder, ReadOnlySpan<char> inputName)
    {
        if (inputName.Length == 0)
        {
            return;
        }

        var firstChar = inputName[0];
        if (!IsIdentifierStart(firstChar) && IsIdentifierPart(firstChar))
        {
            builder.Append('_');
        }

        foreach (var ch in inputName)
        {
            builder.Append(IsIdentifierPart(ch) ? ch : '_');
        }
    }
}
