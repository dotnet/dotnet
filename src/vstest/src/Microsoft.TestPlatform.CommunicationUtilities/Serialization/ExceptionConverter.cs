// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="Exception"/> that handles the read-only properties
/// (<c>Message</c>, <c>StackTrace</c>) which STJ's source-generated metadata cannot populate
/// via the parameterless constructor.
/// <para>
/// On deserialization, produces a <see cref="RemoteException"/> that preserves the original
/// type name, message, stack trace, and inner exception. The original exception type is erased
/// (all exceptions materialize as <see cref="RemoteException"/>) but <c>ToString()</c> renders
/// the full original information including type name and stack trace.
/// </para>
/// </summary>
internal class ExceptionConverter : JsonConverter<Exception>
{
    /// <inheritdoc/>
    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string? className = root.TryGetProperty("ClassName", out var clsProp) && clsProp.ValueKind == JsonValueKind.String
            ? clsProp.GetString()
            : null;

        string? message = root.TryGetProperty("Message", out var msgProp) && msgProp.ValueKind != JsonValueKind.Null
            ? msgProp.GetString()
            : null;

        string? stackTrace = root.TryGetProperty("StackTraceString", out var stProp) && stProp.ValueKind == JsonValueKind.String
            ? stProp.GetString()
            : null;

        Exception? innerException = null;
        if (root.TryGetProperty("InnerException", out var innerProp) && innerProp.ValueKind != JsonValueKind.Null)
        {
            innerException = StjSafe.Deserialize<Exception>(innerProp, options);
        }

        var exception = new RemoteException(className, message, stackTrace, innerException);

        if (root.TryGetProperty("HResult", out var hresultProp) && hresultProp.ValueKind == JsonValueKind.Number)
        {
            exception.HResult = hresultProp.GetInt32();
        }

        if (root.TryGetProperty("Source", out var sourceProp) && sourceProp.ValueKind == JsonValueKind.String)
        {
            exception.Source = sourceProp.GetString();
        }

        return exception;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // For RemoteException (already deserialized once), preserve the original ClassName.
        writer.WriteString("ClassName", value is RemoteException remote
            ? remote.ClassName
            : value.GetType().FullName);
        writer.WriteString("Message", value.Message);
        writer.WriteString("StackTraceString", value is RemoteException re
            ? re.RemoteStackTrace
            : value.StackTrace);
        writer.WriteString("Source", value.Source);
        writer.WriteNumber("HResult", value.HResult);

        writer.WritePropertyName("InnerException");
        if (value.InnerException is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            StjSafe.Serialize(writer, value.InnerException, options);
        }

        writer.WriteEndObject();
    }
}

/// <summary>
/// Exception that preserves diagnostic information from a remotely-serialized exception,
/// including the original type name and stack trace. Since the original exception type may
/// not be available (or may be trimmed under NativeAOT), this type acts as a carrier that
/// faithfully reproduces the original <c>ToString()</c> output.
/// </summary>
internal sealed class RemoteException : Exception
{
    /// <summary>
    /// Gets the fully qualified name of the original exception type (e.g.
    /// <c>"System.InvalidOperationException"</c>).
    /// </summary>
    public string? ClassName { get; }

    /// <summary>
    /// Gets the original stack trace string as captured from the remote process.
    /// </summary>
    public string? RemoteStackTrace { get; }

    public RemoteException(string? className, string? message, string? stackTrace, Exception? innerException)
        : base(message, innerException)
    {
        ClassName = className;
        RemoteStackTrace = stackTrace;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(ClassName ?? nameof(RemoteException));
        if (!string.IsNullOrEmpty(Message))
        {
            sb.Append(": ").Append(Message);
        }

        if (InnerException is not null)
        {
            sb.Append(" ---> ").Append(InnerException).AppendLine()
              .Append("   --- End of inner exception stack trace ---");
        }

        if (!string.IsNullOrEmpty(RemoteStackTrace))
        {
            sb.AppendLine().Append(RemoteStackTrace);
        }

        return sb.ToString();
    }
}

#endif
