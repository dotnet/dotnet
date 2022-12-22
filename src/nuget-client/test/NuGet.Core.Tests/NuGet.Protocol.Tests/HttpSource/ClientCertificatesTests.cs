// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class ClientCertificatesTests
    {
        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, Platform.Linux)]
        public void EnsurePackageSourceClientCertificatesForwardedToV3HttpClientHandler()
        {
            // Arrange
            var certificate = GetCertificate();
            var packageSource = new PackageSource("https://contoso.com/v3/index.json") { ClientCertificates = new List<X509Certificate> { certificate } };
            var sourceRepository = new SourceRepository(packageSource, new[] { new HttpHandlerResourceV3Provider() });

            // Act
            var httpHandlerResourceV3 = sourceRepository.GetResource<HttpHandlerResource>();

            // Assert
            Assert.NotNull(httpHandlerResourceV3);
            Assert.True(httpHandlerResourceV3.ClientHandler.ClientCertificates.Contains(certificate));
        }

        private X509Certificate2 GetCertificate()
        {
            var rsa = RSA.Create(2048);
            var request = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            var start = DateTime.UtcNow.AddDays(-1);
            var end = start.AddYears(1);
            var cert = request.CreateSelfSigned(start, end);
            var data = cert.Export(X509ContentType.Pfx);
            return new X509Certificate2(data);
        }
    }
}
