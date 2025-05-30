// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Microsoft.AspNetCore.StaticWebAssets.Tasks;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public struct StaticWebAssetEndpointResponseHeader : IEquatable<StaticWebAssetEndpointResponseHeader>, IComparable<StaticWebAssetEndpointResponseHeader>
{
    private static readonly JsonTypeInfo<StaticWebAssetEndpointResponseHeader[]> _jsonTypeInfo =
        StaticWebAssetsJsonSerializerContext.Default.StaticWebAssetEndpointResponseHeaderArray;

    public string Name { get; set; }

    public string Value { get; set; }

    internal static StaticWebAssetEndpointResponseHeader[] FromMetadataValue(string value) => string.IsNullOrEmpty(value) ? [] : JsonSerializer.Deserialize(value, _jsonTypeInfo);

    internal static string ToMetadataValue(StaticWebAssetEndpointResponseHeader[] responseHeaders) =>
        JsonSerializer.Serialize(
            responseHeaders ?? [],
            _jsonTypeInfo);

    private string GetDebuggerDisplay() => $"{Name}: {Value}";

    public override bool Equals(object obj) => obj is StaticWebAssetEndpointResponseHeader responseHeader &&
        Equals(responseHeader);

    public bool Equals(StaticWebAssetEndpointResponseHeader other) => string.Equals(Name, other.Name, StringComparison.Ordinal) &&
        string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override int GetHashCode()
    {
#if NET472_OR_GREATER
        var hashCode = -244751520;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
        return hashCode;
#else
        return HashCode.Combine(Name, Value);
#endif
    }

    public int CompareTo(StaticWebAssetEndpointResponseHeader other) => string.CompareOrdinal(Name, other.Name) switch
    {
        0 => string.CompareOrdinal(Value, other.Value),
        int result => result
    };
}
