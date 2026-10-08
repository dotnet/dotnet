// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class ConnectionValidatorTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task BothSidesShouldSucceedWhenTheyHoldTheSamePair()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        validator.RequireValidation();
        validator.UseConnectionSecrets(connectionSecrets);

        var childAuthentication = Task.Run(
            () => ConnectionHandshake.AuthenticateAsChild(connections.Child, connectionSecrets, 5000, CancellationToken.None));
        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        var childResult = await childAuthentication;

        Assert.IsTrue(validation.Succeeded);
        Assert.IsTrue(childResult.Succeeded);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task HandshakeShouldAllowPeerToRespondAfterTwoSeconds(bool authenticateAsParent)
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        var delayedPeer = Task.Run(async () =>
        {
            await Task.Delay(2500);
            return authenticateAsParent
                ? ConnectionHandshake.AuthenticateAsChild(connections.Child, connectionSecrets, 10000, CancellationToken.None)
                : ConnectionHandshake.AuthenticateAsParent(connections.Parent, connectionSecrets, 10000, CancellationToken.None);
        });

        var result = authenticateAsParent
            ? ConnectionHandshake.AuthenticateAsParent(connections.Parent, connectionSecrets, 10000, CancellationToken.None)
            : ConnectionHandshake.AuthenticateAsChild(connections.Child, connectionSecrets, 10000, CancellationToken.None);
        var peerResult = await delayedPeer;

        Assert.IsTrue(result.Succeeded, result.Error);
        Assert.IsTrue(peerResult.Succeeded, peerResult.Error);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task CancellationShouldInterruptHandshakeWithoutWaitingForConnectionTimeout(bool authenticateAsParent)
    {
        const int timeoutMilliseconds = 90000;
        using var connections = await ConnectedTcpClients.CreateAsync();
        using var cancellation = new CancellationTokenSource();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        var client = authenticateAsParent ? connections.Parent : connections.Child;
        var stream = client.GetStream();
        var stopwatch = Stopwatch.StartNew();
        var authentication = Task.Run(() => authenticateAsParent
            ? ConnectionHandshake.AuthenticateAsParent(client, connectionSecrets, timeoutMilliseconds, cancellation.Token)
            : ConnectionHandshake.AuthenticateAsChild(client, connectionSecrets, timeoutMilliseconds, cancellation.Token));

        try
        {
            Assert.IsTrue(SpinWait.SpinUntil(() => stream.ReadTimeout != Timeout.Infinite, 5000));
            var readTimeout = stream.ReadTimeout;
            Assert.IsTrue(readTimeout >= timeoutMilliseconds - (int)stopwatch.ElapsedMilliseconds);
            Assert.IsTrue(readTimeout <= timeoutMilliseconds);
            Assert.IsFalse(authentication.IsCompleted);

            cancellation.Cancel();

            Assert.IsTrue(authentication.Wait(5000));
            Assert.IsFalse((await authentication).Succeeded);
        }
        finally
        {
            cancellation.Cancel();
            client.Close();
        }
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task HandshakeShouldAllowAValidPeerDelayedBeyondTwoSeconds(bool authenticateAsParent)
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        Assert.IsTrue(ConnectionSecrets.TrySplit(connectionSecrets, out var childSecret, out var parentSecret));
        var authenticatingClient = authenticateAsParent ? connections.Parent : connections.Child;
        var peer = authenticateAsParent ? connections.Child : connections.Parent;
        var stream = authenticatingClient.GetStream();

        var peerResponse = Task.Run(() =>
        {
            // Start the delay only after the authentication deadline has been installed.
            Assert.IsTrue(SpinWait.SpinUntil(() => stream.ReadTimeout > 0, 5000));
            Thread.Sleep(2500);
            WriteFrame(peer.GetStream(), CreateHandshakeMessage(authenticateAsParent ? childSecret : parentSecret));
        });

        var result = authenticateAsParent
            ? ConnectionHandshake.AuthenticateAsParent(authenticatingClient, connectionSecrets, 90000, CancellationToken.None)
            : ConnectionHandshake.AuthenticateAsChild(authenticatingClient, connectionSecrets, 90000, CancellationToken.None);
        await peerResponse;

        Assert.IsTrue(result.Succeeded, result.Error);
    }

    [TestMethod]
    [DataRow(true, 90000)]
    [DataRow(false, 90000)]
    [DataRow(true, 120000)]
    [DataRow(false, 120000)]
    [DataRow(true, 10000)]
    [DataRow(false, 10000)]
    public async Task HandshakeShouldUseTheCallerTimeoutAndRestoreSocketTimeouts(
        bool authenticateAsParent,
        int callerTimeoutMilliseconds)
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        Assert.IsTrue(ConnectionSecrets.TrySplit(connectionSecrets, out var childSecret, out var parentSecret));
        var authenticatingClient = authenticateAsParent ? connections.Parent : connections.Child;
        var peer = authenticateAsParent ? connections.Child : connections.Parent;
        var stream = authenticatingClient.GetStream();
        var stopwatch = Stopwatch.StartNew();

        var peerResponse = Task.Run(() =>
        {
            Assert.IsTrue(SpinWait.SpinUntil(() => stream.ReadTimeout > 0 && stream.WriteTimeout > 0, 5000));
            var readTimeout = stream.ReadTimeout;
            var writeTimeout = stream.WriteTimeout;
            var elapsed = stopwatch.ElapsedMilliseconds;
            WriteFrame(peer.GetStream(), CreateHandshakeMessage(authenticateAsParent ? childSecret : parentSecret));
            return (readTimeout, writeTimeout, elapsed);
        });

        var result = authenticateAsParent
            ? ConnectionHandshake.AuthenticateAsParent(authenticatingClient, connectionSecrets, callerTimeoutMilliseconds, CancellationToken.None)
            : ConnectionHandshake.AuthenticateAsChild(authenticatingClient, connectionSecrets, callerTimeoutMilliseconds, CancellationToken.None);
        var observed = await peerResponse;

        Assert.IsTrue(result.Succeeded, result.Error);
        Assert.IsTrue(observed.readTimeout <= callerTimeoutMilliseconds);
        Assert.IsTrue(observed.writeTimeout <= callerTimeoutMilliseconds);
        Assert.IsTrue(observed.readTimeout + observed.elapsed >= callerTimeoutMilliseconds);
        Assert.IsTrue(observed.writeTimeout + observed.elapsed >= callerTimeoutMilliseconds);
        Assert.AreEqual(Timeout.Infinite, stream.ReadTimeout);
        Assert.AreEqual(Timeout.Infinite, stream.WriteTimeout);
    }

    [TestMethod]
    public async Task ParentShouldRejectAChildHoldingADifferentPair()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        validator.RequireValidation();
        validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());

        var childAuthentication = Task.Run(
            () => ConnectionHandshake.AuthenticateAsChild(
                connections.Child,
                ConnectionSecrets.GeneratePair(),
                5000,
                CancellationToken.None));
        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        var childResult = await childAuthentication;

        Assert.IsFalse(validation.Succeeded);
        Assert.AreEqual(PeerProcessValidationFailureKind.Rejected, validation.FailureKind);
        Assert.IsFalse(childResult.Succeeded);
    }

    /// <summary>
    /// The property that makes connecting useless to an attacker: the parent answers only after the
    /// child's secret has matched, so a process that connects without the secrets never receives the
    /// value the parent would present.
    /// </summary>
    [TestMethod]
    public async Task ParentShouldNotPresentItsSecretToAChildThatFailed()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        validator.RequireValidation();
        validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());

        var parentReply = Task.Run(
            () =>
            {
                var stream = connections.Child.GetStream();
                WriteFrame(stream, CreateHandshakeMessage("not-the-expected-secret"));
                stream.ReadTimeout = 5000;
                try
                {
                    // Anything other than "the peer closed without writing" means the parent
                    // presented something to a caller that did not prove itself.
                    return stream.ReadByte();
                }
                catch (IOException)
                {
                    return -1;
                }
            },
            CancellationToken.None);

        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        connections.Parent.Close();
        var bytesReceivedByFailedPeer = await parentReply;

        Assert.IsFalse(validation.Succeeded);
        Assert.AreEqual(PeerProcessValidationFailureKind.Rejected, validation.FailureKind);
        Assert.AreEqual(-1, bytesReceivedByFailedPeer, "The parent answered a peer that failed to present the expected secret.");
    }

    /// <summary>
    /// The child checks the parent too, so a process that took a released port cannot pose as the
    /// process that started us.
    /// </summary>
    [TestMethod]
    public async Task ChildShouldRejectAParentThatPresentsTheWrongSecret()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var connectionSecrets = ConnectionSecrets.GeneratePair();

        var imposter = Task.Run(
            () =>
            {
                var stream = connections.Parent.GetStream();
                _ = ReadFrame(stream);
                WriteFrame(stream, CreateHandshakeMessage("not-the-parent-secret"));
            },
            CancellationToken.None);

        var childResult = ConnectionHandshake.AuthenticateAsChild(connections.Child, connectionSecrets, 5000, CancellationToken.None);
        await imposter;

        Assert.IsFalse(childResult.Succeeded);
        Assert.AreEqual(PeerProcessValidationFailureKind.Rejected, childResult.FailureKind);
    }

    [TestMethod]
    public async Task MalformedResponseShouldBeRejected()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        validator.RequireValidation();
        validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());

        var childResponse = Task.Run(
            () => WriteFrame(connections.Child.GetStream(), "{"),
            CancellationToken.None);
        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        await childResponse;

        Assert.IsFalse(validation.Succeeded);
        Assert.AreEqual(PeerProcessValidationFailureKind.Rejected, validation.FailureKind);
    }

    [TestMethod]
    [DataRow("[]")]
    [DataRow("null")]
    [DataRow("{}")]
    [DataRow("{\"Version\":999999999999999999999,\"MessageType\":\"TestPlatform.Handshake\",\"Payload\":{}}")]
    public async Task InvalidResponseShapeShouldBeRejected(string response)
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        validator.RequireValidation();
        validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());

        var childResponse = Task.Run(
            () => WriteFrame(connections.Child.GetStream(), response),
            CancellationToken.None);
        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        await childResponse;

        Assert.IsFalse(validation.Succeeded);
        Assert.AreEqual(PeerProcessValidationFailureKind.Rejected, validation.FailureKind);
    }

    /// <summary>
    /// The handshake carries NO protocol version, and any version a peer puts on the envelope is
    /// ignored. That is what lets a serviced old branch take part: a serviced 15.9 performs this
    /// handshake and still speaks its own old protocol version, so rejecting an envelope because of
    /// its version would mean no serviced peer could ever connect. Identity is decided by the secret,
    /// and whether a peer performs the check at all is decided by the marker compiled into its
    /// binary, never by a version number.
    /// </summary>
    [TestMethod]
    [DataRow(0)]
    [DataRow(2)]
    [DataRow(999)]
    public async Task EnvelopeVersionShouldBeIgnored(int envelopeVersion)
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        Assert.IsTrue(ConnectionSecrets.TrySplit(connectionSecrets, out var childSecret, out _));
        validator.RequireValidation();
        validator.UseConnectionSecrets(connectionSecrets);

        var childResponse = Task.Run(
            () =>
            {
                var correct = CreateHandshakeMessage(childSecret);
                var tampered = SetEnvelopeVersion(correct, envelopeVersion);
                Assert.AreNotEqual(correct, tampered);
                WriteFrame(connections.Child.GetStream(), tampered);
            },
            CancellationToken.None);
        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        await childResponse;

        Assert.IsTrue(validation.Succeeded);
    }

    [TestMethod]
    public async Task HandshakeShouldEnforceOneOverallDeadline()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        using var cancellation = new CancellationTokenSource();
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        Assert.IsTrue(ConnectionSecrets.TrySplit(connectionSecrets, out var childSecret, out _));

        var childResponse = Task.Run(
            () =>
            {
                try
                {
                    WriteFrameSlowly(connections.Child.GetStream(), CreateHandshakeMessage(childSecret), cancellation.Token);
                }
                catch (IOException) when (cancellation.IsCancellationRequested)
                {
                }
                catch (SocketException) when (cancellation.IsCancellationRequested)
                {
                }
                catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
                {
                }
            },
            CancellationToken.None);

        var stopwatch = Stopwatch.StartNew();
        var validation = ConnectionHandshake.AuthenticateAsParent(
            connections.Parent,
            connectionSecrets,
            timeoutMilliseconds: 100,
            cancellationToken: CancellationToken.None);
        stopwatch.Stop();
        cancellation.Cancel();
        connections.Parent.Close();
        await childResponse;

        Assert.IsFalse(validation.Succeeded);
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000);
    }

    /// <summary>
    /// A missing secret must end the wait immediately with the recorded reason, rather than running
    /// to the connection timeout and reporting something the user cannot act on.
    /// </summary>
    [TestMethod]
    public async Task MissingSecretsShouldFailImmediatelyWithTheRecordedReason()
    {
        using var connections = await ConnectedTcpClients.CreateAsync();
        var validator = new ConnectionValidator();
        validator.RequireValidation();
        validator.CannotValidate("Testhost 1.2.3 is too old.");

        var stopwatch = Stopwatch.StartNew();
        var validation = validator.ValidateAsParent(connections.Parent, CancellationToken.None);
        stopwatch.Stop();

        Assert.IsFalse(validation.Succeeded);
        Assert.AreEqual(PeerProcessValidationFailureKind.Fatal, validation.FailureKind);
        StringAssert.Contains(validation.Error!, "too old");
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 5000);
    }

    [TestMethod]
    public void SecretsMustBeSuppliedAfterValidationIsRequired()
    {
        var validator = new ConnectionValidator();

        Assert.ThrowsException<InvalidOperationException>(
            () => validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair()));
    }

    [TestMethod]
    public void SecretsMustNotBeSuppliedTwice()
    {
        var validator = new ConnectionValidator();
        validator.RequireValidation();
        validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());

        Assert.ThrowsException<InvalidOperationException>(
            () => validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair()));
    }

    [TestMethod]
    public void GeneratedPairShouldHaveTwoDistinctHalvesOfFullLength()
    {
        var pair = ConnectionSecrets.GeneratePair();

        Assert.IsTrue(ConnectionSecrets.TrySplit(pair, out var childSecret, out var parentSecret));
        // 256 bits rendered as hex is 64 characters.
        Assert.AreEqual(64, childSecret.Length);
        Assert.AreEqual(64, parentSecret.Length);
        Assert.AreNotEqual(childSecret, parentSecret);
    }

    [TestMethod]
    public void EveryGeneratedPairShouldDiffer()
    {
        Assert.AreNotEqual(ConnectionSecrets.GeneratePair(), ConnectionSecrets.GeneratePair());
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(":")]
    [DataRow("abc")]
    [DataRow(":abc")]
    [DataRow("abc:")]
    [DataRow("a:b:c")]
    public void MalformedPairShouldNotSplit(string pair)
    {
        Assert.IsFalse(ConnectionSecrets.TrySplit(pair, out _, out _));
    }

    [TestMethod]
    public void NullPairShouldNotSplit()
    {
        Assert.IsFalse(ConnectionSecrets.TrySplit(null, out _, out _));
    }

    [TestMethod]
    public void SecretComparisonShouldRejectNullAndCaseDifferences()
    {
        Assert.IsTrue(ConnectionSecrets.AreEqual("ABC", "ABC"));
        Assert.IsFalse(ConnectionSecrets.AreEqual("ABC", "abc"));
        Assert.IsFalse(ConnectionSecrets.AreEqual("ABC", null));
        Assert.IsFalse(ConnectionSecrets.AreEqual(null, "ABC"));
        Assert.IsFalse(ConnectionSecrets.AreEqual(null, null));
    }

    [TestMethod]
    public void HandshakeMessageShouldRedactTheSecretFromLogs()
    {
        const string secret = "DO-NOT-LOG-THIS";
        var message = JsonDataSerializer.Instance.DeserializeMessage(
            JsonDataSerializer.Instance.SerializePayload(
                MessageType.Handshake,
                new HandshakePayload { Secret = secret }));

        Assert.IsNotNull(message);
        Assert.IsFalse((message.ToString()).Contains(secret));
        StringAssert.Contains(message.ToString(), "<redacted>");
    }

    /// <summary>
    /// Redaction is by property name, not by message type, so a message that was never on a list
    /// still has its secrets kept out of the log. This is what protects the environment blocks that
    /// travel to a child process.
    /// </summary>
    [TestMethod]
    [DataRow("VSTEST_CONNECTION_SECRETS")]
    [DataRow("VSTEST_DATACOLLECTION_CONNECTION_SECRETS")]
    [DataRow("VSTEST_DESIGNMODE_CONNECTION_SECRETS")]
    [DataRow("SomeFuturePassword")]
    [DataRow("A_TOKEN_WE_HAVE_NOT_INVENTED_YET")]
    public void AnyMessageCarryingASecretNamedValueShouldRedactItFromLogs(string variableName)
    {
        const string secret = "DO-NOT-LOG-THIS";
        var message = new Message
        {
            // Deliberately a message type nobody would think to add to a list.
            MessageType = MessageType.TestMessage,
            Payload = JObject.Parse("{\"EnvironmentVariables\":{\"" + variableName + "\":\"" + secret + "\"}}"),
        };

        Assert.IsFalse(message.ToString().Contains(secret));
        StringAssert.Contains(message.ToString(), "<redacted>");

        // The name itself stays, so a log still shows which variable was set.
        StringAssert.Contains(message.ToString(), variableName);
    }

    /// <summary>
    /// Redaction must not cost the diagnostic value of everything else in the message.
    /// </summary>
    [TestMethod]
    public void RedactionShouldLeaveValuesThatDoNotLookLikeSecrets()
    {
        var message = new Message
        {
            MessageType = MessageType.TestMessage,
            Payload = JObject.Parse("{\"EnvironmentVariables\":{\"DOTNET_ROOT\":\"C:\\\\dotnet\",\"MY_SECRET\":\"hide-me\"}}"),
        };

        var logged = message.ToString();

        StringAssert.Contains(logged, "dotnet");
        Assert.IsFalse(logged.Contains("hide-me"));
    }

    [TestMethod]
    public void MessageWithNoSecretShouldBeLoggedUnchanged()
    {
        var message = new Message
        {
            MessageType = MessageType.TestMessage,
            Payload = JObject.Parse("{\"Source\":\"test.dll\"}"),
        };

        // Nothing here looks like a secret, so the value survives and nothing is redacted.
        var logged = message.ToString();
        StringAssert.Contains(logged, "test.dll");
        Assert.IsFalse(logged.Contains("<redacted>"));
    }

    private static string CreateHandshakeMessage(string secret)
    {
        var message = JsonDataSerializer.Instance.SerializePayload(
            MessageType.Handshake,
            new HandshakePayload { Secret = secret },
            ProtocolVersioning.Version1);
        // Matches ConnectionHandshake.SerializeHandshakePayload: no version is added. Serializing at
        // Version1 yields an envelope with no Version field, which is the point.
        return message;
    }

    private static string ReadFrame(NetworkStream stream)
    {
        var length = 0;
        for (var shift = 0; shift < 35; shift += 7)
        {
            var value = stream.ReadByte();
            Assert.IsTrue(value >= 0);
            length |= (value & 0x7f) << shift;
            if ((value & 0x80) == 0)
            {
                break;
            }
        }

        var bytes = new byte[length];
        var offset = 0;
        while (offset < bytes.Length)
        {
            var read = stream.Read(bytes, offset, bytes.Length - offset);
            Assert.IsTrue(read > 0);
            offset += read;
        }

        return Encoding.UTF8.GetString(bytes);
    }

    private static void WriteFrame(NetworkStream stream, string message)
    {
        var frame = CreateFrame(message);
        stream.Write(frame, 0, frame.Length);
        stream.Flush();
    }

    private static void WriteFrameSlowly(
        NetworkStream stream,
        string message,
        CancellationToken cancellationToken)
    {
        foreach (var value in CreateFrame(message))
        {
            cancellationToken.ThrowIfCancellationRequested();
            stream.WriteByte(value);
            stream.Flush();
            Thread.Sleep(10);
        }
    }

    private static byte[] CreateFrame(string message)
    {
        var payload = Encoding.UTF8.GetBytes(message);
        var frame = new System.Collections.Generic.List<byte>();
        var length = (uint)payload.Length;
        while (length >= 0x80)
        {
            frame.Add((byte)(length | 0x80));
            length >>= 7;
        }

        frame.Add((byte)length);
        frame.AddRange(payload);
        return frame.ToArray();
    }

    private static string SetEnvelopeVersion(string message, int version)
    {
        const string versionProperty = "\"Version\":";
        var messageTypeIndex = message.IndexOf("\"MessageType\"", StringComparison.Ordinal);
        Assert.IsTrue(messageTypeIndex > 0);
        var versionIndex = message.IndexOf(versionProperty, 0, messageTypeIndex, StringComparison.Ordinal);
        if (versionIndex < 0)
        {
            return message.Insert(1, $"{versionProperty}{version},");
        }

        var valueStart = versionIndex + versionProperty.Length;
        var valueEnd = valueStart;
        while (valueEnd < messageTypeIndex && char.IsDigit(message[valueEnd]))
        {
            valueEnd++;
        }

        return message.Substring(0, valueStart)
            + version
            + message.Substring(valueEnd);
    }

    private sealed class ConnectedTcpClients : IDisposable
    {
        private ConnectedTcpClients(TcpClient parent, TcpClient child)
        {
            Parent = parent;
            Child = child;
        }

        internal TcpClient Parent { get; }

        internal TcpClient Child { get; }

        internal static async Task<ConnectedTcpClients> CreateAsync()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            try
            {
                var endpoint = (IPEndPoint)listener.LocalEndpoint;
                var child = new TcpClient();
                var acceptTask = listener.AcceptTcpClientAsync();
                await child.ConnectAsync(endpoint.Address, endpoint.Port);
                var parent = await acceptTask;
                return new ConnectedTcpClients(parent, child);
            }
            finally
            {
                listener.Stop();
            }
        }

        public void Dispose()
        {
            Parent.Dispose();
            Child.Dispose();
        }
    }
}
