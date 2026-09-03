// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json;

namespace NuGet.Protocol.Plugins.Tests
{
    internal static class TestUtilities
    {
        internal static string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, value.GetType(), PluginJsonContext.Default.Options);
        }
    }
}
