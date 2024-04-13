// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Security;

namespace NuGet.Test.Server
{
    public class TcpListenerServer : ITestServer
    {
        private X509Certificate2 _tlsCertificate;
        public async Task<T> ExecuteAsync<T>(Func<string, Task<T>> action)
        {
            Func<TcpListener, CancellationToken, Task> startServer;
            switch (Mode)
            {
                case TestServerMode.ServerProtocolViolation:
                    startServer = StartServerProtocolViolationAsync;
                    break;

                case TestServerMode.SlowResponseBody:
                    startServer = StartSlowResponseBody;
                    break;
                case TestServerMode.InvalidTLSCertificate:
                    startServer = StartInvalidTlsCertificateServer;
                    _tlsCertificate = GenerateSelfSignedCertificate();
                    break;

                default:
                    throw new InvalidOperationException($"The mode {Mode} is not supported by this server.");
            }

            var portReserver = new PortReserver();
            return await portReserver.ExecuteAsync(
                async (port, token) =>
                {
                    // start the server
                    var serverCts = new CancellationTokenSource();
                    var tcpListener = new TcpListener(IPAddress.Loopback, port);
                    tcpListener.Start();
                    var serverTask = startServer(tcpListener, serverCts.Token);
                    string address;

                    if (Mode == TestServerMode.InvalidTLSCertificate)
                    {
                        address = $"https://localhost:{port}/";
                    }
                    else
                    {
                        address = $"http://localhost:{port}/";
                    }

                    // execute the caller's action
                    var result = await action(address);

                    // stop the server
                    serverCts.Cancel();
                    tcpListener.Stop();

                    return result;
                },
                CancellationToken.None);
        }

        public TcpListenerServer()
        { }

        private static X509Certificate2 GenerateSelfSignedCertificate()
        {
            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                var start = DateTime.UtcNow;
                var end = DateTime.UtcNow.AddYears(1);
                var cert = request.CreateSelfSigned(start, end);
                var certBytes = cert.Export(X509ContentType.Pfx, "password");

                return new X509Certificate2(certBytes, "password", X509KeyStorageFlags.Exportable);
            }
        }

        private async Task StartInvalidTlsCertificateServer(TcpListener tcpListener, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var client = await Task.Run(tcpListener.AcceptTcpClientAsync, token))
                using (var sslStream = new SslStream(client.GetStream(), false))
                {
                    sslStream.AuthenticateAsServer(_tlsCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);
                    using (var reader = new StreamReader(sslStream, Encoding.ASCII, false, 128))
                    using (var writer = new StreamWriter(sslStream, Encoding.ASCII, 128, false))
                    {
                        while (!string.IsNullOrEmpty(reader.ReadLine()))
                        {
                        }

                        string content = "{}";
                        writer.WriteLine("HTTP/1.1 200 OK");
                        writer.WriteLine($"Date: {DateTimeOffset.UtcNow:R}");
                        writer.WriteLine($"Content-Length: {content.Length}");
                        writer.WriteLine("Content-Type: application/json");
                        writer.WriteLine();
                        writer.WriteLine(content);
                    }
                }
            }
        }

        public TestServerMode Mode { get; set; } = TestServerMode.ServerProtocolViolation;
        public TimeSpan SleepDuration { get; set; } = TimeSpan.FromSeconds(110);

        private async Task StartSlowResponseBody(TcpListener tcpListener, CancellationToken token)
        {
            // This server does not process any request body.
            while (!token.IsCancellationRequested)
            {
                using (var client = await Task.Run(tcpListener.AcceptTcpClientAsync, token))
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.ASCII, false, 1))
                using (var writer = new StreamWriter(stream, Encoding.ASCII, 1, false))
                {
                    while (!string.IsNullOrEmpty(reader.ReadLine()))
                    {
                    }

                    string contentBefore = @"{""a"": 1, ";
                    string contentAfter = @"""b"": 2}";

                    writer.WriteLine("HTTP/1.1 200 OK");
                    writer.WriteLine($"Date: {DateTimeOffset.UtcNow:R}");
                    writer.WriteLine($"Content-Length: {contentBefore.Length + contentAfter.Length}");
                    writer.WriteLine("Content-Type: application/json");
                    writer.WriteLine();
                    writer.Write(contentBefore);
                    writer.Flush();
                    await Task.Delay(SleepDuration, token);
                    writer.Write(contentAfter);
                }
            }
        }

        private async Task StartServerProtocolViolationAsync(TcpListener tcpListener, CancellationToken token)
        {
            // This server does not process any request body.
            while (!token.IsCancellationRequested)
            {
                using (var client = await Task.Run(tcpListener.AcceptTcpClientAsync, token))
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.ASCII, false, 1))
                using (var writer = new StreamWriter(stream, Encoding.ASCII, 1, false))
                {
                    while (!string.IsNullOrEmpty(reader.ReadLine()))
                    {
                    }

                    writer.WriteLine("HTTP/1.1 BAD SERVER");
                    writer.WriteLine($"Date: {DateTimeOffset.UtcNow:R}");
                    writer.WriteLine();
                }
            }
        }
    }
}
