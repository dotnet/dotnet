// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages.TestServer;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Test.Utility
{
    public class SelfSignedCertificateMockServer : IDisposable
    {
        private readonly X509Certificate2 _certificate;
        private readonly string _packageDirectory;
        private readonly ITestOutputHelper _testOutputHelper;
        private TcpListener _tcpListener;
        private bool _runServer = true;
        public string URI;

        public SelfSignedCertificateMockServer(string packageDirectory, ITestOutputHelper testOutputHelper)
        {
            _packageDirectory = packageDirectory;
            _testOutputHelper = testOutputHelper;
            _certificate = GenerateSelfSignedCertificate();
        }

        public async Task StartServerAsync()
        {
            var portReserver = new PortReserver();
            var portNumber = await portReserver.ExecuteAsync((p, t) => Task.FromResult(p), CancellationToken.None);
            _tcpListener = new TcpListener(IPAddress.Loopback, portNumber);
            URI = $"https://{_tcpListener.LocalEndpoint}/";
            _tcpListener.Start();

            while (_runServer)
            {
                var client = await _tcpListener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }
        }

        public void StopServer()
        {
            _runServer = false;
            _tcpListener.Stop();
        }

        private async Task HandleClient(TcpClient client)
        {
            using (client)
            using (var sslStream = new SslStream(client.GetStream(), false))
            {
                await sslStream.AuthenticateAsServerAsync(_certificate, clientCertificateRequired: false, checkCertificateRevocation: true);
                using (var reader = new StreamReader(sslStream, Encoding.ASCII, false, 128))
                using (var writer = new StreamWriter(sslStream, Encoding.ASCII, 128, false))
                {
                    try
                    {
                        var requestLine = await reader.ReadLineAsync();
                        var requestParts = requestLine?.Split(' ');

                        if (requestParts == null || requestParts.Length < 2)
                        {
                            throw new InvalidOperationException("Invalid HTTP request line.");
                        }

                        string path = requestParts[1];
                        var parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if (path == "/v3/index.json")
                        {
                            SendIndexJsonResponse(writer);
                        }
                        else if (parts.Length > 1 && parts[0] == "v3")
                        {
                            if (parts[1] == "package")
                            {
                                if (parts.Length == 4)
                                {
                                    ProcessPackageRequest(parts[2], writer);
                                }
                                else
                                {
                                    SendPackageFile(parts[2], parts[3], parts[4], writer, sslStream);
                                }
                            }
                            else
                            {
                                await writer.WriteLineAsync("HTTP/1.1 404 Not Found");
                            }
                        }
                        else
                        {
                            await writer.WriteLineAsync("HTTP/1.1 404 Not Found");
                        }
                    }
                    catch (Exception ex)
                    {
                        _testOutputHelper.WriteLine($"[SelfSignedCertificateMockServer] Error processing request: {ex}");
                    }
                }
            }
        }

        private void ProcessPackageRequest(string id, StreamWriter writer)
        {
            try
            {
                var versions = GetVersionsFromDirectory(id);

                var json = JsonConvert.SerializeObject(new { versions });
                writer.WriteLine("HTTP/1.1 200 OK");
                writer.WriteLine("Content-Type: application/json");
                writer.WriteLine();
                writer.WriteLine(json);
                writer.Flush();
            }
            catch (Exception ex)
            {
                _testOutputHelper.WriteLine($"[SelfSignedCertificateMockServer] Error processing request: {ex}");
            }
        }

        private void SendPackageFile(string id, string version, string nupkg, StreamWriter writer, SslStream sslStream)
        {
            var filePath = Path.Combine(_packageDirectory, id, version, nupkg);

            if (!File.Exists(filePath))
            {
                writer.WriteLine("HTTP/1.1 404 Not Found");
                return;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                writer.WriteLine("HTTP/1.1 200 OK");
                writer.WriteLine("Content-Type: application/octet-stream");
                writer.WriteLine($"Content-Disposition: attachment; filename=\"{id}.{version}.nupkg\"");
                writer.WriteLine($"Content-Length: {new FileInfo(filePath).Length}");
                writer.WriteLine();
                writer.Flush();

                fileStream.CopyTo(sslStream);
            }
        }

        private string[] GetVersionsFromDirectory(string id)
        {
            var directoryPath = Path.Combine(_packageDirectory, id);

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
            }

            var dirInfo = new DirectoryInfo(directoryPath);
            return dirInfo.GetDirectories().Select(d => d.Name).ToArray();
        }

        private void SendIndexJsonResponse(StreamWriter writer)
        {
            var indexResponse = new
            {
                version = "3.0.0",
                resources = new object[]
                {
                new Resource { Type = "SearchQueryService", Id = $"{URI}v3/query" },
                new Resource { Type = "RegistrationsBaseUrl", Id = $"{URI}v3/registration" },
                new Resource { Type = "PackageBaseAddress/3.0.0", Id = $"{URI}v3/package" },
                new Resource { Type = "PackagePublish/2.0.0", Id = $"{URI}v3/packagepublish" }
                }
            };

            string jsonResponse = JsonConvert.SerializeObject(indexResponse);
            writer.WriteLine("HTTP/1.1 200 OK");
            writer.WriteLine("Content-Type: application/json");
            writer.WriteLine();
            writer.WriteLine(jsonResponse);
            writer.Flush();
        }

        private static X509Certificate2 GenerateSelfSignedCertificate()
        {
            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                var start = DateTime.UtcNow;
                var end = DateTime.UtcNow.AddYears(1);
                var cert = request.CreateSelfSigned(start, end);
                var certBytes = cert.Export(X509ContentType.Pfx, "password");
#if NET9_0_OR_GREATER
                return X509CertificateLoader.LoadPkcs12(certBytes, "password", X509KeyStorageFlags.Exportable);
#else
                return new X509Certificate2(certBytes, "password", X509KeyStorageFlags.Exportable);
#endif
            }
        }

        public void Dispose()
        {
            _tcpListener.Stop();

#if NET8_0_OR_GREATER
            _tcpListener.Dispose();
#endif

            _certificate.Dispose();
        }
    }

    internal class Resource
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@id")]
        public string Id { get; set; }
    }
}
