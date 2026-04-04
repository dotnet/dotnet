// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests.Http;

public class ServerCertificateValidationHandlerTests
{
    private const string INVALIDCRTNAME = "invalidcert";

    [Fact]
    public void TestValidHandler()
    {
        using (var certificateUploader = new CertificateUploader())
        {
            certificateUploader.Create();

            // Validates if the handler created.
            Assert.NotNull(ServerCertificateValidationHandler.Create(certificateUploader.FilePath, NoopServerCertificateValidationEventSource.Instance));
        }
    }

    [Fact]
    public void TestInValidHandler()
    {
        // Validates if the handler created if no certificate is loaded into the trusted collection
        Assert.NotNull(ServerCertificateValidationHandler.Create(INVALIDCRTNAME, NoopServerCertificateValidationEventSource.Instance));
    }
}

#endif
