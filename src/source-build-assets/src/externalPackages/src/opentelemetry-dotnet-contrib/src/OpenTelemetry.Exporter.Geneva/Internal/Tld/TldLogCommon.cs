// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.Geneva.Tld;

internal abstract class TldLogCommon : IDisposable
{
    protected const int MaxSanitizedEventNameLength = 50;

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
    protected readonly HashSet<string>? customFields;
    protected readonly Dictionary<string, string>? tableMappings;
    protected readonly ExceptionStackExportMode exceptionStackExportMode;

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

            this.tableMappings = tempTableMappings;
        }

        // TODO: Validate custom fields (reserved name? etc).
        if (options.CustomFields != null)
        {
            var customFields = new HashSet<string>(StringComparer.Ordinal);
            foreach (var name in options.CustomFields)
            {
                customFields.Add(name);
            }

            this.customFields = customFields;
        }

        if (options.PrepopulatedFields != null)
        {
            var prePopulatedFieldsCount = (byte)(options.PrepopulatedFields.Count - 1); // PrepopulatedFields option has the key ".ver" added to it which is not needed for TLD
            this.partAFieldsCount += prePopulatedFieldsCount;
        }
    }

    public void Dispose() => this.Dispose(true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static byte GetSeverityNumber(LogLevel logLevel)
    {
        // Maps the Ilogger LogLevel to OpenTelemetry logging level.
        // https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/logs/data-model.md#mapping-of-severitynumber
        // TODO: for improving perf simply do ((int)loglevel * 4) + 1
        // or ((int)logLevel << 2) + 1
        return logLevel switch
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
    }

    // This method would map the logger category to a table name which only contains alphanumeric values with the following additions:
    // Any character that is not allowed will be removed.
    // If the resulting string is longer than 50 characters, only the first 50 characters will be taken.
    // If the first character in the resulting string is a lower-case alphabet, it will be converted to the corresponding upper-case.
    // If the resulting string still does not comply with Rule, the category name will not be serialized.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static string GetSanitizedCategoryName(string categoryName)
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
            if (cur is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9')
            {
                result[validNameLength] = cur;
                ++validNameLength;
            }
        }

        return result.Slice(0, validNameLength).ToString();
    }

    protected static void OnProcessScopeForIndividualColumns(
        LogRecordScope scope,
        SerializationDataForScopes stateDataValue,
        HashSet<string>? customFields,
        KeyValuePair<string, object>[] partCFieldsValue,
        List<KeyValuePair<string, object?>>? envProperties)
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

    internal sealed class SerializationDataForScopes
    {
        public byte HasEnvProperties;
        public byte PartCFieldsCountFromState;
    }
}
