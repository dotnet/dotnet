// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests.Http;

public class ServerCertificateValidationProviderTests
{
    private const string InvalidCertificateName = "invalidCert";

    [Fact]
    public void TestValidCertificate()
    {
        using var certificateUploader = new CertificateUploader();
        certificateUploader.Create();

        var serverCertificateValidationProvider =
            ServerCertificateValidationProvider.FromCertificateFile(certificateUploader.FilePath, NoopServerCertificateValidationEventSource.Instance);

        Assert.NotNull(serverCertificateValidationProvider);

#if NET9_0_OR_GREATER
        var certificate = X509CertificateLoader.LoadCertificateFromFile(certificateUploader.FilePath);
#else
        var certificate = new X509Certificate2(certificateUploader.FilePath);
#endif
        var chain = new X509Chain();
        chain.Build(certificate);

        // validates if certificate is valid
        Assert.NotNull(serverCertificateValidationProvider);
        Assert.NotNull(serverCertificateValidationProvider.ValidationCallback);
        Assert.True(serverCertificateValidationProvider.ValidationCallback(this, certificate, chain, System.Net.Security.SslPolicyErrors.None));
    }

    [Fact]
    public void TestInValidCertificate()
    {
        var serverCertificateValidationProvider =
            ServerCertificateValidationProvider.FromCertificateFile(InvalidCertificateName, NoopServerCertificateValidationEventSource.Instance);

        Assert.Null(serverCertificateValidationProvider);
    }

    [Fact]
    public void TestTestCallbackWithNullCertificate()
    {
        using var certificateUploader = new CertificateUploader();
        certificateUploader.Create();

        var serverCertificateValidationProvider =
            ServerCertificateValidationProvider.FromCertificateFile(certificateUploader.FilePath, NoopServerCertificateValidationEventSource.Instance);

        Assert.NotNull(serverCertificateValidationProvider);
        Assert.False(serverCertificateValidationProvider.ValidationCallback(this, null, new X509Chain(), default));
    }

    [Fact]
    public void TestCallbackWithNullChain()
    {
        using var certificateUploader = new CertificateUploader();
        certificateUploader.Create();

        var serverCertificateValidationProvider =
            ServerCertificateValidationProvider.FromCertificateFile(certificateUploader.FilePath, NoopServerCertificateValidationEventSource.Instance);

        Assert.NotNull(serverCertificateValidationProvider);
#if NET9_0_OR_GREATER
        var certificate = X509CertificateLoader.LoadCertificateFromFile(certificateUploader.FilePath);
#else
        var certificate = new X509Certificate2(certificateUploader.FilePath);
#endif
        Assert.False(serverCertificateValidationProvider.ValidationCallback(this, certificate, null, default));
    }
}

#endif
