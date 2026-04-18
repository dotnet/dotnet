// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using OpenTelemetry.Exporter.Geneva.MsgPack;

namespace OpenTelemetry.Exporter.Geneva;

internal sealed class TableNameSerializer
{
    public const int MaxSanitizedCategoryNameLength = 50;
    public const int MaxSanitizedCategoryNameBytes = MaxSanitizedCategoryNameLength + 2;
    public const int MaxCachedSanitizedTableNames = 1024;
    private const StringComparison DictionaryKeyComparison = StringComparison.Ordinal;

#pragma warning disable CA1825 // Avoid zero-length array allocations
    /* Note: We don't use Array.Empty<byte> here because that is used to
    indicate an invalid name. We need a different instance to trigger the
    pass-through case. */
    private static readonly byte[] PassthroughTableName = [];
#pragma warning restore CA1825 // Avoid zero-length array allocations
    private static readonly StringComparer DictionaryKeyComparer = StringComparer.Ordinal;

    private readonly byte[] defaultTableName;
    private readonly Dictionary<string, byte[]>? tableMappings;
    private readonly bool shouldPassThruTableMappings;
    private readonly Lock lockObject = new();
    private TableNameCacheDictionary tableNameCache = new();

    public TableNameSerializer(GenevaExporterOptions options, string defaultTableName)
    {
        Debug.Assert(options != null, "options were null");
        Debug.Assert(!string.IsNullOrWhiteSpace(defaultTableName), "defaultEventName was null or whitespace");

        this.defaultTableName = BuildStr8BufferForAsciiString(defaultTableName);

        if (options!.TableNameMappings != null)
        {
            var tempTableMappings = new Dictionary<string, byte[]>(options.TableNameMappings.Count, DictionaryKeyComparer);
            foreach (var kv in options.TableNameMappings)
            {
                if (kv.Key == "*")
                {
                    if (kv.Value == "*")
                    {
                        this.shouldPassThruTableMappings = true;
                    }
                    else
                    {
                        this.defaultTableName = BuildStr8BufferForAsciiString(kv.Value);
                    }
                }
                else
                {
                    tempTableMappings[kv.Key] =
                        kv.Value == "*" ? PassthroughTableName : BuildStr8BufferForAsciiString(kv.Value);
                }
            }

            this.tableMappings = tempTableMappings;
        }
    }

    // Note: This is used for tests.
    public interface ITableNameCacheDictionary : IReadOnlyDictionary<string, byte[]>
    {
        int CachedSanitizedTableNameCount { get; }
    }

    public ITableNameCacheDictionary TableNameCache => this.tableNameCache;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ResolveAndSerializeTableNameForCategoryName(byte[] destination, int offset, string categoryName, out ReadOnlySpan<byte> tableName)
    {
        var mappedTableName = this.ResolveTableMappingForCategoryName(categoryName);

        if (mappedTableName == PassthroughTableName)
        {
            // Pass-through mode with a full cache.

            var bytesWritten = WriteSanitizedCategoryNameToSpan(new Span<byte>(destination, offset, MaxSanitizedCategoryNameBytes), categoryName);

            tableName = new ReadOnlySpan<byte>(destination, offset, bytesWritten);

            return offset + bytesWritten;
        }

        tableName = mappedTableName;

        return MessagePackSerializer.SerializeSpan(destination, offset, tableName);
    }

    private static byte[] BuildStr8BufferForAsciiString(string value)
    {
        var length = value.Length;

        var buffer = new byte[length + 2];

        Encoding.ASCII.GetBytes(value, 0, length, buffer, 2);

        MessagePackSerializer.WriteStr8Header(buffer, 0, length);

        return buffer;
    }

    // This method would map the logger category to a table name which only contains alphanumeric values with the following additions:
    // Any character that is not allowed will be removed.
    // If the resulting string is longer than 50 characters, only the first 50 characters will be taken.
    // If the first character in the resulting string is a lower-case alphabet, it will be converted to the corresponding upper-case.
    // If the resulting string still does not comply with Rule, the category name will not be serialized.
    private static int WriteSanitizedCategoryNameToSpan(Span<byte> buffer, string categoryName)
    {
        // Reserve 2 bytes for storing LIMIT_MAX_STR8_LENGTH_IN_BYTES and (byte)validNameLength -
        // these 2 bytes will be back filled after iterating through categoryName.
        var cursor = 2;
        var validNameLength = 0;

        // Special treatment for the first character.
        var firstChar = categoryName[0];
        if (firstChar is >= 'A' and <= 'Z')
        {
            buffer[cursor++] = (byte)firstChar;
            ++validNameLength;
        }
        else if (firstChar is >= 'a' and <= 'z')
        {
            // If the first character in the resulting string is a lower-case alphabet,
            // it will be converted to the corresponding upper-case.
            buffer[cursor++] = (byte)(firstChar - 32);
            ++validNameLength;
        }
        else
        {
            // Not a valid name.
            return 0;
        }

        for (var i = 1; i < categoryName.Length; ++i)
        {
            if (validNameLength == MaxSanitizedCategoryNameLength)
            {
                break;
            }

            var cur = categoryName[i];
            if (cur is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9'))
            {
                buffer[cursor++] = (byte)cur;
                ++validNameLength;
            }
        }

        // Backfilling MessagePack serialization protocol and valid category length to the startIdx of the categoryName byte array.
        MessagePackSerializer.WriteStr8Header(buffer, 0, validNameLength);

        return cursor;
    }

    private byte[] ResolveTableMappingForCategoryName(string categoryName)
    {
        var tableNameCache = this.tableNameCache;

        return tableNameCache.TryGetValue(categoryName, out var tableName) ? tableName : this.ResolveTableMappingForCategoryNameRare(categoryName);
    }

    private byte[] ResolveTableMappingForCategoryNameRare(string categoryName)
    {
        byte[]? mappedTableName = null;

        // If user configured table name mappings run resolution logic.
        if (this.tableMappings != null
            && !this.tableMappings.TryGetValue(categoryName, out mappedTableName))
        {
            // Find best match if an exact match was not found.

            string? currentKey = null;

            foreach (var mapping in this.tableMappings)
            {
                if (!categoryName.StartsWith(mapping.Key, DictionaryKeyComparison))
                {
                    continue;
                }

                if (currentKey == null || mapping.Key.Length >= currentKey.Length)
                {
                    currentKey = mapping.Key;
                    mappedTableName = mapping.Value;
                }
            }
        }

        mappedTableName ??= !this.shouldPassThruTableMappings
            ? this.defaultTableName
            : PassthroughTableName;

        var sanitizedTableNameStorage = mappedTableName == PassthroughTableName
            ? stackalloc byte[MaxSanitizedCategoryNameBytes]
            : [];

        if (sanitizedTableNameStorage.Length > 0)
        {
            // We resolved to a wildcard which is pass-through mode.

            var bytesWritten = WriteSanitizedCategoryNameToSpan(sanitizedTableNameStorage, categoryName);
            if (bytesWritten > 0)
            {
                sanitizedTableNameStorage = sanitizedTableNameStorage.Slice(0, bytesWritten);
            }
            else
            {
                // Note: When the table name could not be sanitized we cache
                // the empty array NOT s_passthroughTableName.
                mappedTableName = [];
            }
        }

        lock (this.lockObject)
        {
            var tableNameCache = this.tableNameCache;

            // Check if another thread added the mapping while we waited on the
            // lock.
            if (tableNameCache.TryGetValue(categoryName, out var tableName))
            {
                return tableName;
            }

            if (mappedTableName == PassthroughTableName
                && tableNameCache.CachedSanitizedTableNameCount < MaxCachedSanitizedTableNames)
            {
                mappedTableName = sanitizedTableNameStorage.ToArray();
                tableNameCache.CachedSanitizedTableNameCount++;
            }

            // Note: This is using copy-on-write pattern to keep the happy
            // path lockless once everything has spun up.
            TableNameCacheDictionary newTableNameCache = new(tableNameCache)
            {
                [categoryName] = mappedTableName,
            };

            this.tableNameCache = newTableNameCache;

            return mappedTableName;
        }
    }

    private sealed class TableNameCacheDictionary : Dictionary<string, byte[]>, ITableNameCacheDictionary
    {
        public TableNameCacheDictionary()
            : base(0, DictionaryKeyComparer)
        {
        }

        public TableNameCacheDictionary(TableNameCacheDictionary sourceCache)
            : base(sourceCache, DictionaryKeyComparer)
        {
            this.CachedSanitizedTableNameCount = sourceCache.CachedSanitizedTableNameCount;
        }

        public int CachedSanitizedTableNameCount { get; set; }
    }
}
