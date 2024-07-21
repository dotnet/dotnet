// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SR = Microsoft.AspNetCore.Razor.Serialization.Json.Internal.Strings;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static class JsonReaderExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckToken(this JsonReader reader, JsonToken expectedToken)
    {
        if (reader.TokenType != expectedToken)
        {
            ThrowUnexpectedTokenException(expectedToken, reader.TokenType);
        }

        [DoesNotReturn]
        static void ThrowUnexpectedTokenException(JsonToken expectedToken, JsonToken actualToken)
        {
            throw new InvalidOperationException(
                SR.FormatExpected_JSON_token_0_but_it_was_1(expectedToken, actualToken));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadToken(this JsonReader reader, JsonToken expectedToken)
    {
        reader.CheckToken(expectedToken);
        reader.Read();
    }
}
