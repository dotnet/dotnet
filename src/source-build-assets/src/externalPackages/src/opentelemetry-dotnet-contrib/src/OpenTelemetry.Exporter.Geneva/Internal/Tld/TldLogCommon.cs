// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Buffers;
using System.Collections.Frozen;
#endif
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.Geneva.Tld;

internal abstract class TldLogCommon : IDisposable
{
    protected const int MaxSanitizedEventNameLength = 50;
    protected const int MaxCachedSanitizedCategoryNames = 10000;

    protected static readonly ThreadLocal<List<KeyValuePair<string, object?>>> EnvProperties = new();
    protected static readonly ThreadLocal<KeyValuePair<string, object>[]> PartCFields = new(); // This is used to temporarily store the PartC fields from tags

    protected static readonly string[] LogLevels =
    [
        "Trace", "Debug", "Information", "Warning", "Error", "Critical", "None"
    ];

    protected readonly ThreadLocal<SerializationDataForScopes> serializationData = new(); // This is used for Scopes
    protected readonly byte partAFieldsCount = 1; // At least one field: time
    protected readonly bool shouldPassThruTableMappings;
    protected readonly string defaultEventName = "Log";
#if NET
    protected readonly FrozenSet<string>? customFields;
    protected readonly FrozenDictionary<string, string>? tableMappings;
#else
    protected readonly HashSet<string>? customFields;
    protected readonly Dictionary<string, string>? tableMappings;
#endif
    protected readonly ExceptionStackExportMode exceptionStackExportMode;

#if NET
    private static readonly SearchValues<char> AlphanumericChars = SearchValues.Create(
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

#endif
#if NET
    private FrozenDictionary<string, string> sanitizedCategoryNameCache = FrozenDictionary<string, string>.Empty;
#else
    private Dictionary<string, string> sanitizedCategoryNameCache = new(StringComparer.Ordinal);
#endif
    private bool isDisposed;

    protected TldLogCommon(GenevaExporterOptions options)
    {
        this.exceptionStackExportMode = options.ExceptionStackExportMode;

        // TODO: Validate mappings for reserved tablenames etc.
        if (options.TableNameMappings != null)
        {
            var tempTableMappings = new Dictionary<string, string>(options.TableNameMappings.Count, StringComparer.Ordinal);
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
                        this.defaultEventName = kv.Value;
                    }
                }
                else
                {
                    tempTableMappings[kv.Key] = kv.Value;
                }
            }

            this.tableMappings = tempTableMappings
#if NET
                .ToFrozenDictionary(StringComparer.Ordinal);
#else
                ;
#endif
        }

        // TODO: Validate custom fields (reserved name? etc).
        if (options.CustomFields != null)
        {
            var customFields = new HashSet<string>(StringComparer.Ordinal);
            foreach (var name in options.CustomFields)
            {
                customFields.Add(name);
            }

#if NET
            this.customFields = customFields.ToFrozenSet(StringComparer.Ordinal);
#else
            this.customFields = customFields;
#endif
        }

        if (options.PrepopulatedFields != null)
        {
            var prePopulatedFieldsCount = (byte)(options.PrepopulatedFields.Count - 1); // PrepopulatedFields option has the key ".ver" added to it which is not needed for TLD
            this.partAFieldsCount += prePopulatedFieldsCount;
        }
    }

    public void Dispose() => this.Dispose(true);

    // Maps the Ilogger LogLevel to OpenTelemetry logging level.
    // https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/logs/data-model.md#mapping-of-severitynumber
    // TODO: for improving perf simply do ((int)loglevel * 4) + 1
    // or ((int)logLevel << 2) + 1

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static byte GetSeverityNumber(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => 1,
        LogLevel.Debug => 5,
        LogLevel.Information => 9,
        LogLevel.Warning => 13,
        LogLevel.Error => 17,
        LogLevel.Critical => 21,

        // we reach default only for LogLevel.None
        // but that is filtered out anyway.
        // should we throw here then?
        LogLevel.None => 1,
        _ => 1,
    };

    protected static void OnProcessScopeForIndividualColumns(
        LogRecordScope scope,
        SerializationDataForScopes stateDataValue,
#if NET
        FrozenSet<string>? customFields)
#else
        HashSet<string>? customFields)
#endif
    {
        Debug.Assert(stateDataValue != null, "state.serializationData was null");
        Debug.Assert(PartCFields.Value != null, "PartCFields.Value was null");

        var stateData = stateDataValue!;
        var kvpArrayForPartCFields = PartCFields.Value!;

        List<KeyValuePair<string, object?>>? envPropertiesList = null;

        foreach (var scopeItem in scope)
        {
            if (string.IsNullOrEmpty(scopeItem.Key) || scopeItem.Key == "{OriginalFormat}")
            {
                continue;
            }

            if (customFields == null || customFields.Contains(scopeItem.Key))
            {
                if (scopeItem.Value != null)
                {
                    kvpArrayForPartCFields[stateData.PartCFieldsCountFromState] = new(scopeItem.Key, scopeItem.Value);
                    stateData.PartCFieldsCountFromState++;
                }
            }
            else
            {
                if (stateData.HasEnvProperties == 0)
                {
                    stateData.HasEnvProperties = 1;

                    envPropertiesList = EnvProperties.Value ??= [];

                    envPropertiesList.Clear();
                }

                // TODO: This could lead to unbounded memory usage.
                envPropertiesList!.Add(new(scopeItem.Key, scopeItem.Value));
            }
        }
    }

    // Maps the logger category to a table/event name. The set of logger
    // category names in a process is small and stable, so the sanitized result
    // is cached (copy-on-write, keeping the happy path lockless) instead of
    // being recomputed and reallocated on every log record. This mirrors the
    // caching the MsgPack TableNameSerializer already performs.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected string GetSanitizedCategoryName(string categoryName)
    {
        // Fast path: when the category name is already a valid event name we can
        // return the original string reference as-is, avoiding both the cache
        // lookup and any allocation. The check is an accelerated character scan.
        if (IsAlreadySanitized(categoryName))
        {
            return categoryName;
        }

        var cache = Volatile.Read(ref this.sanitizedCategoryNameCache);

        return cache.TryGetValue(categoryName, out var sanitized)
            ? sanitized
            : this.GetSanitizedCategoryNameRare(categoryName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            try
            {
                // DO NOT Dispose eventBuilder, keyValuePairs, and partCFields as they are static
                this.serializationData?.Dispose();
            }
            catch (Exception ex)
            {
                ExporterEventSource.Log.ExporterException("TldLogCommon Dispose failed.", ex);
            }
        }

        this.isDisposed = true;
    }

    // Returns true when SanitizeCategoryName(categoryName) would produce a string
    // identical to categoryName, i.e. the name is non-empty, starts with an
    // upper-case ASCII letter, is no longer than MaxSanitizedEventNameLength, and
    // contains only ASCII alphanumeric characters. In that case the original
    // string can be reused verbatim with no allocation.
    private static bool IsAlreadySanitized(string categoryName)
    {
        var length = categoryName.Length;
        if (length == 0 || length > MaxSanitizedEventNameLength)
        {
            return false;
        }

        // The first character must already be an upper-case letter (a lower-case
        // first character would be up-cased by sanitization, changing the string).
        if (categoryName[0] is not (>= 'A' and <= 'Z'))
        {
            return false;
        }

#if NET
        return !categoryName.AsSpan(1).ContainsAnyExcept(AlphanumericChars);
#else
        for (var i = 1; i < length; i++)
        {
            var cur = categoryName[i];
            if (cur is not ((>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9')))
            {
                return false;
            }
        }

        return true;
#endif
    }

    private static string SanitizeCategoryName(string categoryName)
    {
        var validNameLength = 0;
        Span<char> result = stackalloc char[MaxSanitizedEventNameLength];

        // Special treatment for the first character.
        var firstChar = categoryName[0];
        if (firstChar is >= 'A' and <= 'Z')
        {
            result[0] = firstChar;
            ++validNameLength;
        }
        else if (firstChar is >= 'a' and <= 'z')
        {
            // If the first character in the resulting string is a lower-case alphabet,
            // it will be converted to the corresponding upper-case.
            result[0] = (char)(firstChar - 32);
            ++validNameLength;
        }
        else
        {
            // Not a valid name.
            return string.Empty;
        }

        for (var i = 1; i < categoryName.Length; i++)
        {
            if (validNameLength == MaxSanitizedEventNameLength)
            {
                break;
            }

            var cur = categoryName[i];
            if (cur is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9'))
            {
                result[validNameLength] = cur;
                ++validNameLength;
            }
        }

        return result.Slice(0, validNameLength).ToString();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private string GetSanitizedCategoryNameRare(string categoryName)
    {
        var sanitized = SanitizeCategoryName(categoryName);

        // Lock-free copy-on-write update. The cache is a pure memoization of a
        // deterministic, side-effect-free function, so a lost race is harmless:
        // the worst case is that the value is recomputed and re-inserted later.
        // We use compare-and-swap (rather than a plain write) so the update is
        // guaranteed to make progress and never overwrite a concurrent insert
        // with a stale snapshot.
        //
        // The CAS loop only ever retries when it loses a race to a concurrent
        // writer (i.e. another thread made progress), so it is provably finite.
        // We still cap the attempts as a safety net: rather than risk spinning
        // forever if that reasoning is ever wrong, we simply stop caching and
        // return the freshly computed value uncached. Failing to memoize is a
        // benign performance degradation, so we degrade gracefully instead of
        // throwing on a telemetry hot path.
        const int maxAttempts = 64;
        for (var attempt = 0; attempt < maxAttempts; attempt++)
        {
            var cache = Volatile.Read(ref this.sanitizedCategoryNameCache);

            // Another thread may have inserted the entry already.
            if (cache.TryGetValue(categoryName, out var existing))
            {
                return existing;
            }

            // Bound the cache to guard against unbounded growth from
            // pathological/adversarial category-name churn.
            if (cache.Count >= MaxCachedSanitizedCategoryNames)
            {
                return sanitized;
            }

            // On modern runtimes the published snapshot is a FrozenDictionary,
            // which trades a higher one-time build cost for faster lookups - a
            // good fit for this read-mostly, rarely-mutated cache.
#if NET
            var newCache = new Dictionary<string, string>(cache, StringComparer.Ordinal)
            {
                [categoryName] = sanitized,
            }.ToFrozenDictionary(StringComparer.Ordinal);
#else
            var newCache = new Dictionary<string, string>(cache, StringComparer.Ordinal)
            {
                [categoryName] = sanitized,
            };
#endif

            if (Interlocked.CompareExchange(ref this.sanitizedCategoryNameCache, newCache, cache) == cache)
            {
                return sanitized;
            }

            // Lost the race against a concurrent update; re-read and retry.
        }

        // Exceeded the retry budget under pathological contention: return the
        // computed value without caching it.
        return sanitized;
    }

    internal sealed class SerializationDataForScopes
    {
        public byte HasEnvProperties;
        public byte PartCFieldsCountFromState;
    }
}
