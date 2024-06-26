// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal static class ProtocolSerializer
{
    public static JsonSerializer Instance { get; } = CreateSerializer();

    private static JsonSerializer CreateSerializer()
    {
        var serializer = new JsonSerializer();
        serializer.AddVSInternalExtensionConverters();
        serializer.AddVSExtensionConverters();

        return serializer;
    }
}
