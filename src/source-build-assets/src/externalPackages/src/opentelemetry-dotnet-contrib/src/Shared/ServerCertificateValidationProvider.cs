// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace OpenTelemetry.Resources;

internal class ServerCertificateValidationProvider
{
    private readonly X509Certificate2Collection trustedCertificates;
    private readonly IServerCertificateValidationEventSource log;

    private ServerCertificateValidationProvider(X509Certificate2Collection trustedCertificates, IServerCertificateValidationEventSource log)
    {
        this.trustedCertificates = trustedCertificates;
        this.ValidationCallback = (_, cert, chain, errors) =>
            this.ValidateCertificate(cert != null ? new X509Certificate2(cert) : null, chain, errors);
        this.log = log;
    }

    public RemoteCertificateValidationCallback ValidationCallback { get; }

    public static ServerCertificateValidationProvider? FromCertificateFile(string certificateFile, IServerCertificateValidationEventSource log)
    {
        if (!File.Exists(certificateFile))
        {
            log.CertificateFileDoesNotExist(certificateFile);
            return null;
        }

        var trustedCertificates = new X509Certificate2Collection();
        if (!LoadCertificateToTrustedCollection(trustedCertificates, certificateFile))
        {
            log.FailedToLoadCertificateInTrustedStorage(certificateFile);
            return null;
        }

        return new ServerCertificateValidationProvider(trustedCertificates, log);
    }

    private static bool LoadCertificateToTrustedCollection(X509Certificate2Collection collection, string certFileName)
    {
        try
        {
#if NET10_0_OR_GREATER
            collection.Add(X509CertificateLoader.LoadCertificateFromFile(certFileName));
#else
            collection.Import(certFileName);
#endif

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static bool HasCommonCertificate(X509Chain chain, X509Certificate2Collection? collection)
    {
        if (collection == null)
        {
            return false;
        }

        foreach (var chainElement in chain.ChainElements)
        {
            foreach (var certificate in collection)
            {
                if (chainElement.Certificate.GetPublicKey().SequenceEqual(certificate.GetPublicKey()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool ValidateCertificate(X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
    {
        var isSslPolicyPassed = errors is SslPolicyErrors.None or SslPolicyErrors.RemoteCertificateChainErrors;
        if (!isSslPolicyPassed)
        {
            if ((errors | SslPolicyErrors.RemoteCertificateNotAvailable) == errors)
            {
                this.log.FailedToValidateCertificate("SslPolicyError RemoteCertificateNotAvailable occurred");
            }

            if ((errors | SslPolicyErrors.RemoteCertificateNameMismatch) == errors)
            {
                this.log.FailedToValidateCertificate("SslPolicyError RemoteCertificateNameMismatch occurred");
            }
        }

        if (chain == null)
        {
            this.log.FailedToValidateCertificate("Certificate chain is null.");
            return false;
        }

        if (cert == null)
        {
            this.log.FailedToValidateCertificate("Certificate is null.");
            return false;
        }

        chain.ChainPolicy.ExtraStore.AddRange(this.trustedCertificates);
        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

        // building the chain to process basic validations e.g. signature, use, expiration, revocation
        var isValidChain = chain.Build(cert);

        if (!isValidChain)
        {
            var chainErrors = string.Empty;
            foreach (var element in chain.ChainElements)
            {
                foreach (var status in element.ChainElementStatus)
                {
                    chainErrors +=
                        $"\nCertificate [{element.Certificate.Subject}] Status [{status.Status}]: {status.StatusInformation}";
                }
            }

            this.log.FailedToValidateCertificate(chainErrors);
        }

        // check if at least one certificate in the chain is in our trust list
        var isTrusted = HasCommonCertificate(chain, this.trustedCertificates);
        if (!isTrusted)
        {
            var serverCertificates = string.Empty;
            foreach (var element in chain.ChainElements)
            {
                serverCertificates += " " + element.Certificate.Subject;
            }

            var trustCertificates = string.Empty;
            foreach (var trustCertificate in this.trustedCertificates)
            {
                trustCertificates += " " + trustCertificate.Subject;
            }

            this.log.FailedToValidateCertificate($"Server Certificates Chain cannot be trusted. The chain doesn't match with the Trusted Certificates provided. Server Certificates:{serverCertificates}. Trusted Certificates:{trustCertificates}");
        }

        return isSslPolicyPassed && isValidChain && isTrusted;
    }
}

#endif
