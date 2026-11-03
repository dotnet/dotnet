// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace Microsoft.Security.AntiSSRF
{
    public static class URIValidator
    {
        /// <summary>
        /// Validates if the given URI belongs to the specified domain.
        /// Only supports HTTP, HTTPS, WS, and WSS protocols.
        /// </summary>
        /// <param name="untrustedUri">The URI to validate.</param>
        /// <param name="trustedDomain">The domain to check against.</param>
        /// <returns>True if the URI belongs to the domain; otherwise, false.</returns>
        public static bool InDomain(Uri? untrustedUri, string? trustedDomain)
        {
            return InOneDomain(untrustedUri, trustedDomain);
        }

        /// <summary>
        /// Validates if the given URI string belongs to the specified domain.
        /// Only supports HTTP, HTTPS, WS, and WSS protocols.
        /// </summary>
        /// <param name="uristring">The URI string to validate.</param>
        /// <param name="domain">The domain to check against.</param>
        /// <returns>True if the URI belongs to the domain; otherwise, false.</returns>
        public static bool InDomain(string? untrustedAddress, string? trustedDomain)
        {
            return InOneDomain(untrustedAddress, trustedDomain);
        }

        /// <summary>
        /// Validates if the given URI belongs to any of the specified domains.
        /// Only supports HTTP, HTTPS, WS, and WSS protocols.
        /// </summary>
        /// <param name="untrustedUri">The URI to validate.</param>
        /// <param name="trustedDomains">The domains to check against.</param>
        /// <returns>True if the URI belongs to any of the domains; otherwise, false.</returns>
        public static bool InDomain(Uri? untrustedUri, string[]? trustedDomains)
        {
            return InAnyDomain(untrustedUri, trustedDomains);
        }

        /// <summary>
        /// Validates if the given URI string belongs to any of the specified domains.
        /// Only supports HTTP, HTTPS, WS, and WSS protocols.
        /// </summary>
        /// <param name="untrustedAddress">The URI string to validate.</param>
        /// <param name="trustedDomains">The domains to check against.</param>
        /// <returns>True if the URI belongs to any of the domains; otherwise, false.</returns>
        public static bool InDomain(string? untrustedAddress, string[]? trustedDomains)
        {
            return InAnyDomain(untrustedAddress, trustedDomains);
        }

        /// <summary>
        /// Validates if the given URI belongs to an Azure Key Vault domain.
        /// Only supports HTTP and HTTPS protocols.
        /// </summary>
        /// <param name="untrustedUri">The URI to validate.</param>
        /// <returns>True if the URI belongs to an Azure Key Vault domain; otherwise, false.</returns>
        public static bool InAzureKeyVaultDomain(Uri? untrustedUri)
        {
            return InAzureServiceDomain(untrustedUri, Domains.AzureKeyVaultDomains);
        }

        /// <summary>
        /// Validates if the given URI string belongs to an Azure Key Vault domain.
        /// Only supports HTTP and HTTPS protocols.
        /// </summary>
        /// <param name="untrustedAddress">The URI string to validate.</param>
        /// <returns>True if the URI belongs to an Azure Key Vault domain; otherwise, false.</returns>
        public static bool InAzureKeyVaultDomain(string? untrustedAddress)
        {
            return InAzureServiceDomain(untrustedAddress, Domains.AzureKeyVaultDomains);
        }

        /// <summary>
        /// Validates if the given URI belongs to an Azure Storage domain.
        /// Only supports HTTP and HTTPS protocols.
        /// </summary>
        /// <param name="untrustedUri">The URI to validate.</param>
        /// <returns>True if the URI belongs to an Azure Storage domain; otherwise, false.</returns>
        public static bool InAzureStorageDomain(Uri? untrustedUri)
        {
            return InAzureServiceDomain(untrustedUri, Domains.AzureStorageDomains);
        }

        /// <summary>
        /// Validates if the given URI string belongs to an Azure Storage domain.
        /// Only supports HTTP and HTTPS protocols.
        /// </summary>
        /// <param name="untrustedAddress">The URI string to validate.</param>
        /// <returns>True if the URI belongs to an Azure Storage domain; otherwise, false.</returns>
        public static bool InAzureStorageDomain(string? untrustedAddress)
        {
            return InAzureServiceDomain(untrustedAddress, Domains.AzureStorageDomains);
        }

        private static readonly string[] _azureSDKProtocols = { "http", "https" };

        private static readonly string[] _inDomainProtocols = { "http", "https", "ws", "wss" };

        private static string GetValidHostname(object? input, bool isSdk)
        {
            Uri uri = input switch
            {
                Uri u => u,
                string address => new UriBuilder(address).Uri,
                null => throw new AntiSSRFException(),
                _ => throw new AntiSSRFException()
            };

            if (isSdk && !_azureSDKProtocols.Contains(uri.Scheme))
                throw new AntiSSRFException();

            if (!isSdk && !_inDomainProtocols.Contains(uri.Scheme))
                throw new AntiSSRFException();

            return uri.IdnHost;
        }

        private static string ParseDomain(string domain)
        {
            if (domain.StartsWith("."))
                domain = domain.Substring(1);
            return new UriBuilder("https://", domain).Uri.IdnHost;
        }

        private static string[] ParseDomains(string[] domains)
        {
            string[] parsedDomains = new string[domains.Length];
            for (int i = 0; i < domains.Length; i++)
            {
                parsedDomains[i] = ParseDomain(domains[i]);
            }
            return parsedDomains;
        }

        private static bool ValidatedInDomain(string idnHost, string parsedDomain)
        {
            int hostLen = idnHost.Length;
            int domainLen = parsedDomain.Length;

            return (hostLen >= domainLen)
                && (string.Compare(idnHost, hostLen - domainLen, parsedDomain, 0, domainLen, StringComparison.OrdinalIgnoreCase) == 0)
                && (hostLen == domainLen || idnHost[hostLen - domainLen - 1] == '.');
        }

        private static bool InOneDomain(object? untrustedInput, string? trustedDomain)
        {
            if (untrustedInput == null || trustedDomain == null || trustedDomain.Length == 0)
                return false;

            try
            {
                string idnHost = GetValidHostname(untrustedInput, false);
                string parsedDomain = ParseDomain(trustedDomain);
                return ValidatedInDomain(idnHost, parsedDomain);
            }
            catch
            {
                return false;
            }
        }

        private static bool InAnyDomain(object? untrustedInput, string[]? trustedDomains)
        {
            if (untrustedInput == null || trustedDomains == null || trustedDomains.Length == 0)
                return false;

            try
            {
                string idnHost = GetValidHostname(untrustedInput, false);
                string[] parsedDomains = ParseDomains(trustedDomains);
                foreach (var parsedDomain in parsedDomains)
                {
                    if (ValidatedInDomain(idnHost, parsedDomain))
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool InAzureServiceDomain(object? untrustedInput, string[] sdkDomains)
        {
            if (untrustedInput == null)
                return false;

            try
            {
                string idnHost = GetValidHostname(untrustedInput, true);

                if (idnHost.Contains("--"))
                    return false;

                foreach (var domain in sdkDomains)
                {
                    if (ValidatedInDomain(idnHost, domain))
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
