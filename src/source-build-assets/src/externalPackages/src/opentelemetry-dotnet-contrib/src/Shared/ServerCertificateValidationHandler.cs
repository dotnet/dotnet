// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

namespace OpenTelemetry.Resources;

internal static class ServerCertificateValidationHandler
{
    public static HttpClientHandler? Create(string certificateFile, IServerCertificateValidationEventSource log)
    {
        try
        {
            var serverCertificateValidationProvider = ServerCertificateValidationProvider.FromCertificateFile(certificateFile, log);

            if (serverCertificateValidationProvider == null)
            {
                return new HttpClientHandler();
            }

            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                (sender, x509Certificate2, x509Chain, sslPolicyErrors) =>
                    serverCertificateValidationProvider.ValidationCallback(sender, x509Certificate2, x509Chain, sslPolicyErrors),
            };
            return clientHandler;
        }
        catch (Exception ex)
        {
            log.FailedToCreateHttpHandler(ex);
        }

        return null;
    }
}

#endif
