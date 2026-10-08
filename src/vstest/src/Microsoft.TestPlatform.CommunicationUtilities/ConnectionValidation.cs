// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;

// This line serializes with Newtonsoft rather than the System.Text.Json / Jsonite pair that the
// reviewed branch was written against, so the exception a malformed handshake produces is
// Newtonsoft's. JsonReaderException and JsonSerializationException both derive from it, so the base
// type covers a truncated frame and a well-formed frame with the wrong shape alike.
using HandshakeJsonException = Newtonsoft.Json.JsonException;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The validator has the same lifetime as its communication transport.")]
internal sealed class ConnectionValidator
{
    private readonly object _syncObject = new();
    private readonly ManualResetEventSlim _configurationReady = new(false);

    private bool _validationRequired;
    private string? _connectionSecrets;

    internal string? Error { get; private set; }

    internal bool IsValidationRequired => _validationRequired;

    internal void RequireValidation()
    {
        lock (_syncObject)
        {
            _validationRequired = true;
            _connectionSecrets = null;
            Error = null;
            _configurationReady.Reset();
        }
    }

    /// <summary>
    /// Supplies the secret pair generated for this child process. Called after the child has been
    /// launched, because the pair is created as part of building its environment.
    /// </summary>
    internal void UseConnectionSecrets(string connectionSecrets)
    {
        if (string.IsNullOrWhiteSpace(connectionSecrets))
        {
            throw new ArgumentException("The connection secrets cannot be empty.", nameof(connectionSecrets));
        }

        lock (_syncObject)
        {
            if (!_validationRequired)
            {
                throw new InvalidOperationException(
                    "Connection identity validation must be required before the secrets are supplied.");
            }

            if (_connectionSecrets is not null)
            {
                throw new InvalidOperationException("The connection secrets have already been supplied.");
            }

            _connectionSecrets = connectionSecrets;
            Error = null;
            _configurationReady.Set();
        }
    }

    /// <summary>
    /// Records that no secrets are available for this connection, so the wait below ends promptly
    /// with an explanation rather than running to the connection timeout.
    /// </summary>
    internal void CannotValidate(string error)
    {
        lock (_syncObject)
        {
            Error = error;
            _configurationReady.Set();
        }
    }

    internal PeerProcessValidationResult ValidateAsParent(TcpClient client, CancellationToken cancellationToken)
    {
        if (!_validationRequired)
        {
            return PeerProcessValidationResult.Success();
        }

        try
        {
            _configurationReady.Wait(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return Fail(PeerProcessValidationResult.Fatal(
                string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsInternalError, ConnectionSecretsCapabilities.NegotiationErrorLink)));
        }

        if (_connectionSecrets is null)
        {
            return Fail(PeerProcessValidationResult.Fatal(
                Error ?? string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsInternalError, ConnectionSecretsCapabilities.NegotiationErrorLink)));
        }

        var result = ConnectionHandshake.AuthenticateAsParent(
            client,
            _connectionSecrets,
            EnvironmentHelper.GetConnectionTimeout() * 1000,
            cancellationToken);

        if (result.Succeeded)
        {
            EqtTrace.Info("ConnectionValidator.ValidateAsParent: The peer presented the expected connection secret.");
            return result;
        }

        return Fail(result);
    }

    private PeerProcessValidationResult Fail(PeerProcessValidationResult result)
    {
        Error = result.Error;
        return result;
    }
}
internal static class ConnectionHandshake
{
    private const int MaxHandshakeBytes = 4096;

    private static readonly Encoding StrictUtf8 = new UTF8Encoding(false, true);

    /// <summary>
    /// Direction of a handshake message. The side that was started presents first, whichever side
    /// listens, so a peer that takes a released port cannot pose as either end.
    /// </summary>
    internal static PeerProcessValidationResult AuthenticateAsParent(
        TcpClient client,
        string connectionSecrets,
        int timeoutMilliseconds,
        CancellationToken cancellationToken)
    {
        if (!ConnectionSecrets.TrySplit(connectionSecrets, out var childSecret, out var parentSecret))
        {
            return PeerProcessValidationResult.Fatal(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsInternalError, ConnectionSecretsCapabilities.NegotiationErrorLink));
        }

        try
        {
            // Closing the socket interrupts synchronous reads when the transport is stopped.
            using var cancellationRegistration = cancellationToken.Register(client.Close);
            var deadline = new HandshakeDeadline(timeoutMilliseconds);
            var stream = client.GetStream();
            SetTimeouts(stream, deadline, out var previousReadTimeout, out var previousWriteTimeout);
            try
            {
                var presented = DeserializeHandshakePayload(ReadHandshake(stream, deadline))?.Secret;
                deadline.ThrowIfExpired();
                if (!ConnectionSecrets.AreEqual(childSecret, presented))
                {
                    // Do not answer a peer that failed, or connecting would be enough to collect
                    // the value this side presents.
                    return PeerProcessValidationResult.Rejected(
                        string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsPeerSecretMismatch, ConnectionSecretsCapabilities.NegotiationErrorLink));
                }

                WriteFrame(stream, SerializeHandshakePayload(new HandshakePayload { Secret = parentSecret }), deadline);
                deadline.ThrowIfExpired();
                return PeerProcessValidationResult.Success();
            }
            finally
            {
                RestoreTimeouts(stream, previousReadTimeout, previousWriteTimeout);
            }
        }
        catch (Exception ex) when (ex is IOException
            or SocketException
            or FormatException
            or DecoderFallbackException
            or InvalidOperationException
            or TimeoutException)
        {
            return PeerProcessValidationResult.Rejected(
                ConnectionSecretsCapabilities.WithDetails(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsPeerNegotiationIncomplete, ConnectionSecretsCapabilities.NegotiationErrorLink), new[] { ConnectionSecretsCapabilities.Detail(Resources.Resources.ConnectionSecretsDetailReason, ex.Message) }));
        }
    }

    internal static PeerProcessValidationResult AuthenticateAsChild(
        TcpClient client,
        string connectionSecrets,
        int timeoutMilliseconds,
        CancellationToken cancellationToken)
    {
        if (!ConnectionSecrets.TrySplit(connectionSecrets, out var childSecret, out var parentSecret))
        {
            return PeerProcessValidationResult.Fatal(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsInternalError, ConnectionSecretsCapabilities.NegotiationErrorLink));
        }

        try
        {
            using var cancellationRegistration = cancellationToken.Register(client.Close);
            var deadline = new HandshakeDeadline(timeoutMilliseconds);
            var stream = client.GetStream();
            SetTimeouts(stream, deadline, out var previousReadTimeout, out var previousWriteTimeout);
            try
            {
                WriteFrame(stream, SerializeHandshakePayload(new HandshakePayload { Secret = childSecret }), deadline);

                var presented = DeserializeHandshakePayload(ReadHandshake(stream, deadline))?.Secret;
                deadline.ThrowIfExpired();

                return ConnectionSecrets.AreEqual(parentSecret, presented)
                    ? PeerProcessValidationResult.Success()
                    : PeerProcessValidationResult.Rejected(
                        string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsParentSecretMismatch, ConnectionSecretsCapabilities.NegotiationErrorLink));
            }
            finally
            {
                RestoreTimeouts(stream, previousReadTimeout, previousWriteTimeout);
            }
        }
        catch (Exception ex) when (ex is IOException
            or SocketException
            or FormatException
            or DecoderFallbackException
            or InvalidOperationException
            or TimeoutException)
        {
            return PeerProcessValidationResult.Rejected(
                ConnectionSecretsCapabilities.WithDetails(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConnectionSecretsParentNegotiationIncomplete, ConnectionSecretsCapabilities.NegotiationErrorLink), new[] { ConnectionSecretsCapabilities.Detail(Resources.Resources.ConnectionSecretsDetailReason, ex.Message) }));
        }
    }
    private static string SerializeHandshakePayload(HandshakePayload payload)
    {
        // Deliberately UNVERSIONED, and serializing at version 1 is how that is achieved: the payload
        // serializer writes a "Version" field only above version 1, so this produces an envelope with
        // no version in it.
        //
        // DO NOT add a version to this message. The handshake is exchanged before the protocol version
        // is negotiated, and it ships in serviced builds whose protocol version stays where it was, so
        // a serviced 15.9 performs this handshake and still speaks its own old version. Gating the
        // handshake on a version would mean no serviced peer ever performs it. Whether a peer performs
        // the check is answered by the marker compiled into its binary, never by its protocol version.
        var unversionedMessage = JsonDataSerializer.Instance.SerializePayload(
            MessageType.Handshake,
            payload,
            ProtocolVersioning.Version1);
        if (unversionedMessage.Length == 0 || unversionedMessage[0] != '{')
        {
            throw new InvalidOperationException("The connection-authentication message was not a JSON object.");
        }

        return unversionedMessage;
    }

    private static Message ReadHandshake(NetworkStream stream, HandshakeDeadline deadline)
    {
        var rawMessage = ReadFrame(stream, deadline);
        Message? message;
        try
        {
            message = JsonDataSerializer.Instance.DeserializeMessage(rawMessage);
        }
        catch (Exception ex) when (IsHandshakeDeserializationException(ex))
        {
            throw new FormatException("The connection-authentication message was invalid.", ex);
        }

        // Only the message type is checked. The handshake carries no protocol version - see
        // SerializeHandshakePayload - so there is nothing to compare. A message with no version
        // deserializes as a plain Message, and the payload serializer falls back to its default, which
        // is the same one the sender used.
        if (message?.MessageType != MessageType.Handshake)
        {
            throw new FormatException("The first connection-authentication message was not a supported nonce challenge.");
        }

        // Normalise whatever version the peer put on the envelope. DeserializePayload picks its
        // serializer from that version and throws NotSupportedException on one it does not know, so a
        // peer stamping an unfamiliar number would be refused for that instead of being judged on its
        // secret. The payload is a single string either way, so the serializer choice cannot change
        // what is read, only whether reading throws.
        if (message is VersionedMessage versionedMessage)
        {
            versionedMessage.Version = ProtocolVersioning.Version1;
        }

        return message;
    }

    private static HandshakePayload? DeserializeHandshakePayload(Message message)
    {
        try
        {
            return JsonDataSerializer.Instance.DeserializePayload<HandshakePayload>(message);
        }
        catch (Exception ex) when (IsHandshakeDeserializationException(ex))
        {
            throw new FormatException("The connection-authentication payload was invalid.", ex);
        }
    }

    private static bool IsHandshakeDeserializationException(Exception exception)
        => exception is HandshakeJsonException
            or ArgumentException
            or InvalidCastException
            or KeyNotFoundException
            or NullReferenceException
            or NotSupportedException
            or OverflowException;

    private static string ReadFrame(NetworkStream stream, HandshakeDeadline deadline)
    {
        var byteCount = Read7BitEncodedInt(stream, deadline);
        if (byteCount is < 0 or > MaxHandshakeBytes)
        {
            throw new FormatException("The connection-authentication message exceeded the allowed size.");
        }

        var bytes = new byte[byteCount];
        var offset = 0;
        while (offset < bytes.Length)
        {
            ApplyDeadline(stream, deadline);
            var read = stream.Read(bytes, offset, bytes.Length - offset);
            if (read == 0)
            {
                throw new EndOfStreamException(
                    "The peer disconnected while sending its connection-authentication message.");
            }

            offset += read;
        }

        return StrictUtf8.GetString(bytes);
    }

    private static int Read7BitEncodedInt(NetworkStream stream, HandshakeDeadline deadline)
    {
        var result = 0;
        for (var shift = 0; shift < 35; shift += 7)
        {
            ApplyDeadline(stream, deadline);
            var value = stream.ReadByte();
            if (value < 0)
            {
                throw new EndOfStreamException(
                    "The peer disconnected before sending its connection-authentication message.");
            }

            result |= (value & 0x7f) << shift;
            if ((value & 0x80) == 0)
            {
                return result;
            }
        }

        throw new FormatException("The connection-authentication message length was invalid.");
    }

    private static void WriteFrame(NetworkStream stream, string message, HandshakeDeadline deadline)
    {
        var bytes = StrictUtf8.GetBytes(message);
        if (bytes.Length > MaxHandshakeBytes)
        {
            throw new InvalidOperationException(
                "The connection-authentication message exceeded the allowed size.");
        }

        var value = (uint)bytes.Length;
        while (value >= 0x80)
        {
            ApplyDeadline(stream, deadline);
            stream.WriteByte((byte)(value | 0x80));
            value >>= 7;
        }

        ApplyDeadline(stream, deadline);
        stream.WriteByte((byte)value);
        ApplyDeadline(stream, deadline);
        stream.Write(bytes, 0, bytes.Length);
        ApplyDeadline(stream, deadline);
        stream.Flush();
    }

    private static void SetTimeouts(
        NetworkStream stream,
        HandshakeDeadline deadline,
        out int previousReadTimeout,
        out int previousWriteTimeout)
    {
        previousReadTimeout = stream.ReadTimeout;
        previousWriteTimeout = stream.WriteTimeout;
        ApplyDeadline(stream, deadline);
    }

    private static void ApplyDeadline(NetworkStream stream, HandshakeDeadline deadline)
    {
        var remainingMilliseconds = deadline.GetRemainingMilliseconds();
        stream.ReadTimeout = remainingMilliseconds;
        stream.WriteTimeout = remainingMilliseconds;
    }

    private static void RestoreTimeouts(
        NetworkStream stream,
        int previousReadTimeout,
        int previousWriteTimeout)
    {
        if (stream.CanTimeout)
        {
            stream.ReadTimeout = previousReadTimeout;
            stream.WriteTimeout = previousWriteTimeout;
        }
    }

    private sealed class HandshakeDeadline
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly int _timeoutMilliseconds;

        internal HandshakeDeadline(int timeoutMilliseconds)
        {
            // Authentication uses the configured connection timeout, without a shorter phase limit.
            _timeoutMilliseconds = Math.Max(1, timeoutMilliseconds);
        }

        internal int GetRemainingMilliseconds()
        {
            var remainingMilliseconds = _timeoutMilliseconds - _stopwatch.ElapsedMilliseconds;
            if (remainingMilliseconds <= 0)
            {
                throw new TimeoutException("The connection-authentication handshake timed out.");
            }

            return checked((int)remainingMilliseconds);
        }

        internal void ThrowIfExpired()
            => _ = GetRemainingMilliseconds();
    }
}
