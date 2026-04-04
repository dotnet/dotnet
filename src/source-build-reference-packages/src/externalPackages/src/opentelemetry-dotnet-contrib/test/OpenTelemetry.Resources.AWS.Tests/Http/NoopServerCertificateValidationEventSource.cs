// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources.AWS.Tests.Http;

internal sealed class NoopServerCertificateValidationEventSource : IServerCertificateValidationEventSource
{
    public static NoopServerCertificateValidationEventSource Instance { get; } = new NoopServerCertificateValidationEventSource();

    public void FailedToValidateCertificate(string error)
    {
    }

    public void FailedToCreateHttpHandler(Exception exception)
    {
    }

    public void CertificateFileDoesNotExist(string filename)
    {
    }

    public void FailedToLoadCertificateInTrustedStorage(string filename)
    {
    }
}
