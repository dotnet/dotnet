// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Collections.Frozen;
#endif

namespace OpenTelemetry.Exporter.Geneva.MsgPack;

internal abstract class MsgPackExporter
{
    internal static readonly IReadOnlyDictionary<string, string> PART_A_MAPPING_DICTIONARY = new Dictionary<string, string>
    {
        // Part A
        [Schema.V40.PartA.IKey] = "env_iKey",
        [Schema.V40.PartA.Name] = "env_name",
        [Schema.V40.PartA.Ver] = "env_ver",
        [Schema.V40.PartA.Time] = "env_time",

        // Part A Application Extension
        [Schema.V40.PartA.Extensions.App.Id] = "env_app_id",
        [Schema.V40.PartA.Extensions.App.Ver] = "env_app_ver",

        // Part A Cloud Extension
        [Schema.V40.PartA.Extensions.Cloud.Role] = "env_cloud_role",
        [Schema.V40.PartA.Extensions.Cloud.RoleInstance] = "env_cloud_roleInstance",
        [Schema.V40.PartA.Extensions.Cloud.RoleVer] = "env_cloud_roleVer",

        // Part A Os extension
        [Schema.V40.PartA.Extensions.Os.Name] = "env_os_name",
        [Schema.V40.PartA.Extensions.Os.Ver] = "env_os_ver",

        // Part A Dt extension
        [Schema.V40.PartA.Extensions.Dt.TraceId] = "env_dt_traceId",
        [Schema.V40.PartA.Extensions.Dt.SpanId] = "env_dt_spanId",
    };

#if NET
    internal static readonly IReadOnlyDictionary<string, string> V40_PART_A_MAPPING = PART_A_MAPPING_DICTIONARY.ToFrozenDictionary();
#else
    internal static readonly IReadOnlyDictionary<string, string> V40_PART_A_MAPPING = PART_A_MAPPING_DICTIONARY;
#endif

    protected static int AddPartAField(byte[] buffer, int cursor, string name, object? value)
    {
        cursor = V40_PART_A_MAPPING.TryGetValue(name, out var replacementKey) ? MessagePackSerializer.SerializeAsciiString(buffer, cursor, replacementKey) : MessagePackSerializer.SerializeUnicodeString(buffer, cursor, name);

        cursor = MessagePackSerializer.Serialize(buffer, cursor, value);
        return cursor;
    }

    protected static int AddPartAField(byte[] buffer, int cursor, string name, ReadOnlySpan<byte> value)
    {
        cursor = V40_PART_A_MAPPING.TryGetValue(name, out var replacementKey) ? MessagePackSerializer.SerializeAsciiString(buffer, cursor, replacementKey) : MessagePackSerializer.SerializeUnicodeString(buffer, cursor, name);

        cursor = MessagePackSerializer.SerializeSpan(buffer, cursor, value);
        return cursor;
    }
}
