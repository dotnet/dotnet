// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Defines a class for validating the WsFederationConfiguration.
    /// </summary>
    public class WsFederationConfigurationValidator : IConfigurationValidator<WsFederationConfiguration>
    {
        /// <summary>
        /// Validates a WsFederationConfiguration.
        /// </summary>
        /// <param name="configuration">WsFederationConfiguration to validate</param>
        /// <returns>A <see cref="ConfigurationValidationResult"/> containing the validation result.</returns>
        /// <exception cref="ArgumentNullException">If the provided configuration is null</exception>
        public ConfigurationValidationResult Validate(WsFederationConfiguration configuration)
        {
            if (configuration == null)
                throw LogArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(configuration.Issuer))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22700,
                    Succeeded = false
                };
            }

            if (configuration.Signature == null)
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22701,
                    Succeeded = false
                };
            }

            if (configuration.Signature.KeyInfo == null)
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22702,
                    Succeeded = false
                };
            }

            if (string.IsNullOrWhiteSpace(configuration.Signature.SignatureValue))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22703,
                    Succeeded = false
                };
            }

            if (configuration.Signature.SignedInfo == null)
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22704,
                    Succeeded = false
                };
            }

            if (string.IsNullOrWhiteSpace(configuration.Signature.SignedInfo.SignatureMethod))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22705,
                    Succeeded = false
                };
            }

            if (configuration.Signature.SignedInfo.References == null || configuration.Signature.SignedInfo.References.Count == 0)
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22706,
                    Succeeded = false
                };
            }

            if (string.IsNullOrWhiteSpace(configuration.ActiveTokenEndpoint))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22707,
                    Succeeded = false
                };
            }

            if (!Uri.IsWellFormedUriString(configuration.ActiveTokenEndpoint, UriKind.Absolute))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22708,
                    Succeeded = false
                };
            }

            if (string.IsNullOrWhiteSpace(configuration.TokenEndpoint))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22709,
                    Succeeded = false
                };
            }

            if (!Uri.IsWellFormedUriString(configuration.TokenEndpoint, UriKind.Absolute))
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22710,
                    Succeeded = false
                };
            }

            if (configuration.SigningKeys == null || configuration.SigningKeys.Count == 0)
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX22711,
                    Succeeded = false
                };
            }

            // Get the thumbprint of the cert used to sign the metadata
            string signingKeyId = string.Empty;
            var signatureX509Data = configuration.Signature.KeyInfo.X509Data.GetEnumerator();

            if (signatureX509Data.MoveNext())
            {
                var signatureCertData = signatureX509Data.Current.Certificates.GetEnumerator();
                if (signatureCertData.MoveNext() && !string.IsNullOrWhiteSpace(signatureCertData.Current))
                {
                    X509Certificate2? cert = null;

                    try
                    {
                        cert = CertificateHelper.LoadX509Certificate(signatureCertData.Current);
                        signingKeyId = cert.Thumbprint;
                    }
                    catch (CryptographicException)
                    {
                        return new ConfigurationValidationResult
                        {
                            ErrorMessage = LogMessages.IDX22712,
                            Succeeded = false
                        };
                    }
                    finally
                    {
                        if (cert != null)
                        {
                            ((IDisposable)cert).Dispose();
                        }
                    }
                }
            }

            // We know the key used to sign the doc is part of the token signing keys as per the spec.
            // http://docs.oasis-open.org/wsfed/federation/v1.2/os/ws-federation-1.2-spec-os.html#_Toc223174958:~:text=%3C/fed%3ATargetScopes%20%3E-,3.1.15%20%5BSignature%5D%20Property,-The%20OPTIONAL%20%5BSignature
            // If the metadata is for a token issuer then the key used to sign issued tokens SHOULD
            // be used to sign this document.  This means that if a <fed:TokenSigningKey> is specified,
            // it SHOULD be used to sign this document.

            foreach (SecurityKey key in configuration.SigningKeys)
            {
                if (key == null || key.CryptoProviderFactory == null || signingKeyId != key.KeyId)
                    continue;

                try
                {
                    configuration.Signature.Verify(key, key.CryptoProviderFactory);

                    return new ConfigurationValidationResult
                    {
                        Succeeded = true
                    };
                }
                catch (XmlValidationException)
                {
                    // We know the signature is invalid at this point
                    break;
                }
            }

            return new ConfigurationValidationResult
            {
                ErrorMessage = LogMessages.IDX22713,
                Succeeded = false
            };
        }
    }
}
