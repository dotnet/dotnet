// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// Thin wrappers around <see cref="JsonSerializer"/> that suppress IL2026/IL3050 trimming
/// and AOT warnings. These are safe because every <see cref="JsonSerializerOptions"/> instance
/// in this assembly is configured with <see cref="TestPlatformJsonContext"/> (source-generated)
/// as the primary <see cref="IJsonTypeInfoResolver"/>.
/// </summary>
internal static class StjSafe
{
    private const string Justification = "Options are configured with TestPlatformJsonContext source-gen resolver.";

    [Conditional("DEBUG")]
    private static void AssertResolverConfigured(JsonSerializerOptions options)
    {
        Debug.Assert(
            options.TypeInfoResolver is not null,
            "StjSafe methods require options with a TypeInfoResolver (source-gen context). "
            + "Ensure options were created via JsonDataSerializer.CreateBaseOptions().");
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = Justification)]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = Justification)]
    internal static T? Deserialize<T>(string json, JsonSerializerOptions options)
    {
        AssertResolverConfigured(options);
        return JsonSerializer.Deserialize<T>(json, options);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = Justification)]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = Justification)]
    internal static T? Deserialize<T>(JsonElement element, JsonSerializerOptions options)
    {
        AssertResolverConfigured(options);
        return JsonSerializer.Deserialize<T>(element, options);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = Justification)]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = Justification)]
    internal static void Serialize<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        AssertResolverConfigured(options);
        JsonSerializer.Serialize(writer, value, options);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = Justification)]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = Justification)]
    internal static string Serialize<T>(T value, JsonSerializerOptions options)
    {
        AssertResolverConfigured(options);
        return JsonSerializer.Serialize(value, options);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = Justification)]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = Justification)]
    internal static JsonElement SerializeToElement(object value, Type inputType, JsonSerializerOptions options)
    {
        AssertResolverConfigured(options);
        return JsonSerializer.SerializeToElement(value, inputType, options);
    }
}

#endif
