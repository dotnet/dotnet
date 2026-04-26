// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections;
using System.Globalization;

namespace OpenTelemetry.Exporter.Instana.Implementation;

internal static class InstanaSpanSerializer
{
#pragma warning disable SA1310 // Field names should not contain underscore
    private const string COMMA = ",";
    private const string OPEN_BRACE = "{";
    private const string CLOSE_BRACE = "}";
    private const string QUOTE = "\"";
    private const string COLON = ":";
    private const string QUOTE_COLON = QUOTE + COLON;
    private const string QUOTE_COLON_QUOTE = QUOTE + COLON + QUOTE;
    private const string QUOTE_COMMA_QUOTE = QUOTE + COMMA + QUOTE;
#pragma warning restore SA1310 // Field names should not contain underscore
    private static readonly long UnixZeroTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

    internal static IEnumerator? GetSpanTagsEnumerator(InstanaSpan instanaSpan)
    {
        return instanaSpan.Data.Tags.GetEnumerator();
    }

    internal static IEnumerator? GetSpanEventsEnumerator(InstanaSpan instanaSpan)
    {
        return instanaSpan.Data.Events.GetEnumerator();
    }

    internal static async Task SerializeToStreamWriterAsync(InstanaSpan instanaSpan, StreamWriter writer)
    {
        await writer.WriteAsync(OPEN_BRACE).ConfigureAwait(false);
        await AppendProperty(instanaSpan.T, "t", writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);
        await AppendProperty(instanaSpan.S, "s", writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(instanaSpan.P))
        {
            await AppendProperty(instanaSpan.P, "p", writer).ConfigureAwait(false);
            await writer.WriteAsync(COMMA).ConfigureAwait(false);
        }

        if (!string.IsNullOrEmpty(instanaSpan.Lt))
        {
            await AppendProperty(instanaSpan.Lt, "lt", writer).ConfigureAwait(false);
            await writer.WriteAsync(COMMA).ConfigureAwait(false);
        }

        if (instanaSpan.Tp)
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            await AppendProperty(true.ToString().ToLowerInvariant(), "tp", writer).ConfigureAwait(false);
#pragma warning restore CA1308 // Normalize strings to uppercase
            await writer.WriteAsync(COMMA).ConfigureAwait(false);
        }

        if (instanaSpan.K != SpanKind.NOT_SET)
        {
            await AppendProperty((((int)instanaSpan.K) + 1).ToString(CultureInfo.InvariantCulture), "k", writer).ConfigureAwait(false);
            await writer.WriteAsync(COMMA).ConfigureAwait(false);
        }

        await AppendProperty(instanaSpan.N, "n", writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);
        await AppendPropertyAsync(DateToUnixMillis(instanaSpan.Ts), "ts", writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);
        await AppendPropertyAsync(instanaSpan.D / 10_000L, "d", writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);
        await AppendObjectAsync(SerializeDataAsync, "data", instanaSpan, writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);
        await AppendObjectAsync(SerializeFromAsync, "f", instanaSpan, writer).ConfigureAwait(false);
        await writer.WriteAsync(COMMA).ConfigureAwait(false);
        await AppendPropertyAsync(instanaSpan.Ec, "ec", writer).ConfigureAwait(false);
        await writer.WriteAsync(CLOSE_BRACE).ConfigureAwait(false);
    }

    private static async Task SerializeFromAsync(InstanaSpan instanaSpan, StreamWriter writer)
    {
        await writer.WriteAsync("{\"e\":\"").ConfigureAwait(false);
        await writer.WriteAsync(instanaSpan.F.E).ConfigureAwait(false);
        await writer.WriteAsync("\"}").ConfigureAwait(false);
    }

    private static long DateToUnixMillis(long timeStamp)
    {
        return (timeStamp - UnixZeroTime) / 10000;
    }

    private static async Task SerializeTagsAsync(InstanaSpan instanaSpan, StreamWriter writer)
    {
        await SerializeTagsLogicAsync(instanaSpan.Data.Tags, writer).ConfigureAwait(false);
    }

    private static async Task SerializeTagsLogicAsync(Dictionary<string, string>? tags, StreamWriter writer)
    {
        await writer.WriteAsync(OPEN_BRACE).ConfigureAwait(false);
        if (tags == null)
        {
            return;
        }

        using (var enumerator = tags.GetEnumerator())
        {
            byte i = 0;
            try
            {
                while (enumerator.MoveNext())
                {
                    if (i > 0)
                    {
                        await writer.WriteAsync(COMMA).ConfigureAwait(false);
                    }
                    else
                    {
                        i = 1;
                    }

                    await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                    await writer.WriteAsync(enumerator.Current.Key).ConfigureAwait(false);
                    await writer.WriteAsync(QUOTE_COLON_QUOTE).ConfigureAwait(false);
                    await writer.WriteAsync(enumerator.Current.Value).ConfigureAwait(false);
                    await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException)
            {
                // if the collection gets modified while serializing, we might get a collision.
                // There is no good way of preventing this and continuing normally except locking
                // which needs investigation
            }
        }

        await writer.WriteAsync(CLOSE_BRACE).ConfigureAwait(false);
    }

    private static async Task AppendProperty(string? value, string? name, StreamWriter json)
    {
        await json.WriteAsync(QUOTE).ConfigureAwait(false);
        await json.WriteAsync(name).ConfigureAwait(false);
        await json.WriteAsync(QUOTE_COLON_QUOTE).ConfigureAwait(false);
        await json.WriteAsync(value).ConfigureAwait(false);
        await json.WriteAsync(QUOTE).ConfigureAwait(false);
    }

    private static async Task AppendPropertyAsync(long value, string name, StreamWriter json)
    {
        await json.WriteAsync(QUOTE).ConfigureAwait(false);
        await json.WriteAsync(name).ConfigureAwait(false);
        await json.WriteAsync(QUOTE_COLON).ConfigureAwait(false);
        await json.WriteAsync(value.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
    }

    private static async Task AppendObjectAsync(Func<InstanaSpan, StreamWriter, Task> valueFunction, string name, InstanaSpan instanaSpan, StreamWriter json)
    {
        await json.WriteAsync(QUOTE).ConfigureAwait(false);
        await json.WriteAsync(name).ConfigureAwait(false);
        await json.WriteAsync(QUOTE_COLON).ConfigureAwait(false);
        await valueFunction(instanaSpan, json).ConfigureAwait(false);
    }

    private static async Task SerializeDataAsync(InstanaSpan instanaSpan, StreamWriter writer)
    {
        await writer.WriteAsync(OPEN_BRACE).ConfigureAwait(false);
        if (instanaSpan.Data.data == null)
        {
            return;
        }

        using (var enumerator = instanaSpan.Data.data.GetEnumerator())
        {
            byte i = 0;
            try
            {
                while (enumerator.MoveNext())
                {
                    if (i > 0)
                    {
                        await writer.WriteAsync(COMMA).ConfigureAwait(false);
                    }
                    else
                    {
                        i = 1;
                    }

                    await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                    await writer.WriteAsync(enumerator.Current.Key).ConfigureAwait(false);
                    await writer.WriteAsync(QUOTE_COLON_QUOTE).ConfigureAwait(false);
                    await writer.WriteAsync(enumerator.Current.Value.ToString()).ConfigureAwait(false);
                    await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException)
            {
                // if the collection gets modified while serializing, we might get a collision.
                // There is no good way of preventing this and continuing normally except locking
                // which needs investigation
            }
        }

        if (instanaSpan.Data.Tags.Count > 0)
        {
            await writer.WriteAsync(COMMA).ConfigureAwait(false);

            // serialize tags
            await AppendObjectAsync(SerializeTagsAsync, InstanaExporterConstants.TAGS_FIELD, instanaSpan, writer).ConfigureAwait(false);
        }

        if (instanaSpan.Data.Events.Count > 0)
        {
            await writer.WriteAsync(COMMA).ConfigureAwait(false);

            // serialize tags
            await AppendObjectAsync(SerializeEventsAsync, InstanaExporterConstants.EVENTS_FIELD, instanaSpan, writer).ConfigureAwait(false);
        }

        await writer.WriteAsync(CLOSE_BRACE).ConfigureAwait(false);
    }

    private static async Task SerializeEventsAsync(InstanaSpan instanaSpan, StreamWriter writer)
    {
        if (instanaSpan.Data.Events == null)
        {
            return;
        }

        using var enumerator = instanaSpan.Data.Events.GetEnumerator();
        byte i = 0;
        try
        {
            await writer.WriteAsync("[").ConfigureAwait(false);
            while (enumerator.MoveNext())
            {
                if (i > 0)
                {
                    await writer.WriteAsync(COMMA).ConfigureAwait(false);
                }
                else
                {
                    i = 1;
                }

                await writer.WriteAsync(OPEN_BRACE).ConfigureAwait(false);
                await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                await writer.WriteAsync(InstanaExporterConstants.EVENT_NAME_FIELD).ConfigureAwait(false);
                await writer.WriteAsync(QUOTE_COLON_QUOTE).ConfigureAwait(false);
                await writer.WriteAsync(enumerator.Current.Name).ConfigureAwait(false);
                await writer.WriteAsync(QUOTE_COMMA_QUOTE).ConfigureAwait(false);
                await writer.WriteAsync(InstanaExporterConstants.EVENT_TIMESTAMP_FIELD).ConfigureAwait(false);
                await writer.WriteAsync(QUOTE_COLON_QUOTE).ConfigureAwait(false);
                await writer.WriteAsync(DateToUnixMillis(enumerator.Current.Ts).ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAsync(QUOTE).ConfigureAwait(false);

                if (enumerator.Current.Tags.Count > 0)
                {
                    await writer.WriteAsync(COMMA).ConfigureAwait(false);
                    await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                    await writer.WriteAsync(InstanaExporterConstants.TAGS_FIELD).ConfigureAwait(false);
                    await writer.WriteAsync(QUOTE).ConfigureAwait(false);
                    await writer.WriteAsync(COLON).ConfigureAwait(false);
                    await SerializeTagsLogicAsync(enumerator.Current.Tags, writer).ConfigureAwait(false);
                }

                await writer.WriteAsync(CLOSE_BRACE).ConfigureAwait(false);
            }

            await writer.WriteAsync("]").ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            // if the collection gets modified while serializing, we might get a collision.
            // There is no good way of preventing this and continuing normally except locking
            // which needs investigation
        }
    }
}
