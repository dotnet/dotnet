// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace Microsoft.AspNetCore.Razor.Language;
internal class TypeNameHelper
{
    private static readonly string[] PredefinedTypeNames = new[] {
        "bool",
        "int",
        "uint",
        "nint",
        "byte",
        "char",
        "long",
        "ulong",
        "short",
        "nuint",
        "sbyte",
        "float",
        "object",
        "string",
        "ushort",
        "double",
        "decimal"
    };

    public static void WriteGloballyQualifiedName(CodeWriter codeWriter, string typeName)
    {
        if (typeName == null)
        {
            throw new ArgumentNullException(nameof(typeName));
        }

        WriteGloballyQualifiedName(codeWriter, new StringSegment(typeName));
    }

    internal static string GetGloballyQualifiedNameIfNeeded(string typeName)
    {
        if (typeName.StartsWith("global::", StringComparison.Ordinal))
        {
            return typeName;
        }

        // Fast path, if the length doesn't fall within that of the
        // builtin c# types, then we can add global without further checks.
        if (typeName.Length < 3 || typeName.Length > 7)
        {
            return $"global::{typeName}";
        }

        for (var i = 0; i < PredefinedTypeNames.Length; i++)
        {
            if (typeName.Equals(PredefinedTypeNames[i], StringComparison.Ordinal))
            {
                return typeName;
            }
        }

        return $"global::{typeName}";
    }

    internal static void WriteGloballyQualifiedName(CodeWriter codeWriter, StringSegment typeName)
    {
        if (typeName.StartsWith("global::", StringComparison.Ordinal))
        {
            codeWriter.Write(typeName);
            return;
        }

        // Fast path, if the length doesn't fall within that of the
        // builtin c# types, then we can add global without further checks.
        if (typeName.Length < 3 || typeName.Length > 7)
        {
            codeWriter.Write("global::");
            codeWriter.Write(typeName);
            return;
        }

        for (var i = 0; i < PredefinedTypeNames.Length; i++)
        {
            if (typeName.Equals(PredefinedTypeNames[i], StringComparison.Ordinal))
            {
                codeWriter.Write(typeName);
                return;
            }
        }

        codeWriter.Write("global::");
        codeWriter.Write(typeName);
    }
}
