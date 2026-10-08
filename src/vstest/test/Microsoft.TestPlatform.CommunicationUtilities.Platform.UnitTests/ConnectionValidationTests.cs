// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
public class ConnectionValidationTests
{
    [TestMethod]
    public async Task MatchingConnectionSecretsShouldAdmitConnection()
    {
        var connectionSecrets = ConnectionSecrets.GeneratePair();
        using var parent = new CommunicationManagerLifetime();
        using var child = new CommunicationManagerLifetime();
        var parentValidator = (IConnectionValidationConfigurable)parent.Manager;
        parentValidator.RequireConnectionValidation();
        parentValidator.UseConnectionSecrets(connectionSecrets);
        ((IConnectionAuthenticationResponder)child.Manager).UseConnectionSecretsAsChild(connectionSecrets);

        var port = parent.Manager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        var acceptTask = parent.Manager.AcceptClientAsync();
        await child.Manager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));

        Assert.IsTrue(parent.Manager.WaitForClientConnection(5000));
        Assert.IsTrue(child.Manager.WaitForServerConnection(5000));
        await acceptTask;
    }

    [TestMethod]
    public async Task MismatchedConnectionSecretsShouldRejectWithoutOwnershipFallback()
    {
        using var parent = new CommunicationManagerLifetime();
        using var child = new CommunicationManagerLifetime();
        var parentValidator = (IConnectionValidationConfigurable)parent.Manager;
        parentValidator.RequireConnectionValidation();
        parentValidator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());
        ((IConnectionAuthenticationResponder)child.Manager).UseConnectionSecretsAsChild(ConnectionSecrets.GeneratePair());

        var port = parent.Manager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        var acceptTask = parent.Manager.AcceptClientAsync();
        await child.Manager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));

        Assert.IsTrue(acceptTask.Wait(5000));
        await acceptTask;
        Assert.IsFalse(parent.Manager.WaitForClientConnection(0));
        Assert.IsFalse(child.Manager.WaitForServerConnection(0));
        Assert.IsNotNull(parentValidator.ConnectionValidationError);

        using var secondClient = new TcpClient();
        await Assert.ThrowsExceptionAsync<SocketException>(() => secondClient.ConnectAsync(IPAddress.Loopback, port));
    }

    [TestMethod]
    public async Task StopServerShouldInterruptAuthenticationOfAcceptedClient()
    {
        using var server = new CommunicationManagerLifetime();
        using var client = new TcpClient();
        ((IConnectionAuthenticationResponder)server.Manager).UseConnectionSecretsAsChild(ConnectionSecrets.GeneratePair());

        var endpoint = server.Manager.HostServer(new IPEndPoint(IPAddress.Loopback, 0));
        var acceptTask = server.Manager.AcceptClientAsync();
        await client.ConnectAsync(endpoint.Address, endpoint.Port);
        var stream = client.GetStream();
        stream.ReadTimeout = 5000;
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        _ = reader.ReadString();

        server.Manager.StopServer();

        Assert.IsTrue(acceptTask.Wait(5000));
        await acceptTask;
        Assert.IsFalse(server.Manager.WaitForClientConnection(0));
        Assert.IsTrue(client.Client.Poll(1000000, SelectMode.SelectRead));
        Assert.AreEqual(0, client.Client.Available);
    }

    /// <summary>
    /// There is no unauthenticated mode, so a peer that cannot be checked must end the wait promptly
    /// with the recorded reason rather than running to the connection timeout.
    /// </summary>
    [TestMethod]
    public async Task ConnectionShouldBeRefusedPromptlyWhenNoSecretsAreAvailable()
    {
        using var parent = new CommunicationManagerLifetime();
        using var child = new CommunicationManagerLifetime();
        var parentValidator = (IConnectionValidationConfigurable)parent.Manager;
        parentValidator.RequireConnectionValidation();
        parentValidator.CannotValidateConnection("testhost 1.2.3 is too old.");

        var port = parent.Manager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        _ = parent.Manager.AcceptClientAsync();
        await child.Manager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));

        Assert.IsFalse(parent.Manager.WaitForClientConnection(5000));
        StringAssert.Contains(parentValidator.ConnectionValidationError!, "too old");
    }

    /// <summary>
    /// The child checks the parent as well, so a listener that does not hold the pair cannot take
    /// over a released port and pose as the process that started us.
    /// </summary>
    [TestMethod]
    public async Task ChildShouldNotConnectToAServerThatDoesNotHoldThePair()
    {
        using var parent = new CommunicationManagerLifetime();
        using var child = new CommunicationManagerLifetime();
        var parentValidator = (IConnectionValidationConfigurable)parent.Manager;
        parentValidator.RequireConnectionValidation();
        parentValidator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());
        ((IConnectionAuthenticationResponder)child.Manager).UseConnectionSecretsAsChild(ConnectionSecrets.GeneratePair());

        var port = parent.Manager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        _ = parent.Manager.AcceptClientAsync();
        await child.Manager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));

        Assert.IsFalse(child.Manager.WaitForServerConnection(500));
    }

    private sealed class CommunicationManagerLifetime : IDisposable
    {
        internal SocketCommunicationManager Manager { get; } = new();

        public void Dispose()
        {
            Manager.StopClient();
            Manager.StopServer();
        }
    }
}
