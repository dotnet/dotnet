// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Serialization;

internal static class JsonSerializerExtensions
{
    public static void RegisterRazorConverters(this JsonSerializer serializer)
    {
        if (serializer is null)
        {
            throw new ArgumentNullException(nameof(serializer));
        }

        serializer.Converters.RegisterRazorConverters();

        AddConverter(serializer, PlatformAgnosticClientCapabilities.JsonConverter);
    }

    public static void RegisterVSInternalExtensionConverters(this JsonSerializer serializer)
    {
        if (serializer is null)
        {
            throw new ArgumentNullException(nameof(serializer));
        }

        serializer.AddVSInternalExtensionConverters();
    }

    private static void AddConverter(JsonSerializer serializer, JsonConverter converter)
    {
        serializer.Converters.Add(converter);
    }
}
