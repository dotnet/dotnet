// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Test.Utility
{
    public class SelfSignedHttpsServerWithHttpResources : IDisposable
    {
        private TcpListener _tcpListener;
        private bool _runServer = true;
        private X509Certificate2 _certificate;
        public string URI;

        public SelfSignedHttpsServerWithHttpResources()
        {
            _certificate = GenerateSelfSignedCertificate();
        }

        public async Task StartServerAsync()
        {
            _tcpListener = new TcpListener(IPAddress.Loopback, 0);
            _tcpListener.Start();
            URI = $"https://localhost:{((IPEndPoint)_tcpListener.LocalEndpoint).Port}/";

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
                await sslStream.AuthenticateAsServerAsync(_certificate, clientCertificateRequired: false, checkCertificateRevocation: false);
                using (var reader = new StreamReader(sslStream, Encoding.ASCII, false, 128))
                using (var writer = new StreamWriter(sslStream, Encoding.ASCII, 128, false))
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
                        SendIndexJson(writer);
                    }
                    else
                    {
                        await writer.WriteLineAsync("HTTP/1.1 404 Not Found");
                    }
                }
            }
        }

        private void SendIndexJson(StreamWriter writer)
        {
            var resources = new List<Resource>
                {
                    new Resource { Id = "http://test/index.json", Type = "SearchQueryService"},
                    new Resource { Id = "http://test/index.json", Type = "SearchQueryService"},
                    new Resource { Id = "http://test/index.json", Type = "SearchAutocompleteService"},
                    new Resource { Id = "http://test/index.json", Type = "SearchAutocompleteService" },
                    new Resource { Id = "http://test/index.json", Type = "SearchGalleryQueryService/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "SearchGalleryQueryService/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "RegistrationsBaseUrl" },
                    new Resource { Id = "http://test/index.json", Type = "PackageBaseAddress/3.0.0" },
                    new Resource { Id = "http://test/index.json", Type = "LegacyGallery" },
                    new Resource { Id = "http://test/index.json", Type = "LegacyGallery/2.0.0" },
                    new Resource { Id = "http://test/index.json", Type = "PackagePublish/2.0.0" },
                    new Resource { Id = "http://test/index.json", Type = "SymbolPackagePublish/4.9.0" },
                    new Resource { Id = "http://test/index.json", Type = "SearchQueryService/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "SearchQueryService/3.5.0" },
                    new Resource { Id = "http://test/index.json", Type = "SearchAutocompleteService/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "SearchAutocompleteService/3.5.0" },
                    new Resource { Id = "http://test/index.json", Type = "RegistrationsBaseUrl" },
                    new Resource { Id = "http://test/index.json", Type = "ReportAbuseUriTemplate/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "PackageDisplayMetadataUriTemplate/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "PackageVersionDisplayMetadataUriTemplate/3.0.0-rc" },
                    new Resource { Id = "http://test/index.json", Type = "SearchQueryService/3.0.0-beta" },
                    new Resource { Id = "http://test/index.json", Type = "SearchAutocompleteService/3.0.0-beta" },
                    new Resource { Id = "http://test/index.json", Type = "RegistrationsBaseUrl/3.0.0-beta" },
                    new Resource { Id = "http://test/index.json", Type = "ReportAbuseUriTemplate/3.0.0-beta" },
                    new Resource { Id = "http://test/index.json", Type = "PackageDetailsUriTemplate/5.1.0" },
                    new Resource { Id = "http://test/index.json", Type = "OwnerDetailsUriTemplate/6.11.0" },
                    new Resource { Id = "http://test/index.json", Type = "RegistrationsBaseUrl/3.4.0" },
                    new Resource { Id = "http://test/index.json", Type = "RegistrationsBaseUrl/3.6.0" },
                    new Resource { Id = "http://test/index.json", Type = "RegistrationsBaseUrl/Versioned" },
                    new Resource { Id = "http://test/index.json", Type = "RepositorySignatures/4.7.0" },
                    new Resource { Id = "http://test/index.json", Type = "RepositorySignatures/5.0.0" },
                    new Resource { Id = "http://test/index.json", Type = "VulnerabilityInfo/6.7.0" },
                    new Resource { Id = "http://test/index.json", Type = "Catalog/3.0.0" },
                    new Resource { Id = "http://test/index.json", Type = "ReadmeUriTemplate/6.13.0" }
            };

            var indexResponse = new
            {
                version = "3.0.0",
                resources = resources
            };

            string jsonResponse = JsonConvert.SerializeObject(indexResponse, Formatting.Indented);
            int contentLength = Encoding.UTF8.GetByteCount(jsonResponse);

            writer.WriteLine("HTTP/1.1 200 OK");
            writer.WriteLine("Content-Type: application/json; charset=utf-8");
            writer.WriteLine($"Content-Length: {contentLength}");
            writer.WriteLine("Connection: close");
            writer.WriteLine();
            writer.Write(jsonResponse);
            writer.Flush();
        }


        public void Dispose()
        {
            StopServer();
            _tcpListener?.Stop();
        }

        private static X509Certificate2 GenerateSelfSignedCertificate()
        {
            // Create key for signing the certificate
            using RSA rsa = RSA.Create(2048);

            var req = new CertificateRequest(
                "CN=localhost",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            var cert = req.CreateSelfSigned(
                DateTimeOffset.UtcNow.AddDays(-1),
                DateTimeOffset.UtcNow.AddYears(1));

            var pfxBytes = cert.Export(X509ContentType.Pfx, "password");

#if NET9_0_OR_GREATER
            return X509CertificateLoader.LoadPkcs12(
                pfxBytes,
                "password",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
#else
            return new X509Certificate2(
                pfxBytes,
                "password",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
#endif
        }
    }
}
