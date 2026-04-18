// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources;

internal interface IServerCertificateValidationEventSource
{
    public void FailedToValidateCertificate(string error);

    public void FailedToCreateHttpHandler(Exception exception);

    public void CertificateFileDoesNotExist(string filename);

    public void FailedToLoadCertificateInTrustedStorage(string filename);
}
