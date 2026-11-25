// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Security.AntiSSRF
{

    /// <summary>
    /// Defines the available configuration options for AntiSSRF policies.
    /// </summary>
    public enum PolicyConfigOptions
    {
        /// <summary>
        /// Block all IP addresses by default. Users must explicitly add allowed address ranges.
        /// </summary>
        InternalOnly,

        /// <summary>
        /// Block IP addresses from the recommendedV1 list. Adds X-Forwarded-For header.
        /// </summary>
        ExternalOnlyV1,

        /// <summary>
        /// Block IP addresses from the latest recommended list. Adds X-Forwarded-For header.
        /// </summary>
        ExternalOnlyLatest,

        /// <summary>
        /// No IP address restrictions by default.
        /// </summary>
        None
    }

    public class AntiSSRFPolicy
    {
        private readonly List<string> _deniedHeaders;
        private readonly List<string> _requiredHeaders;
        private readonly List<CIDRBlock> _allowedAddresses;
        private readonly List<CIDRBlock> _deniedAddresses;
        private volatile bool _editLock = false;

        /// <summary>
        /// Gets or sets whether plain text HTTP requests are allowed. If false, any request with the "http" scheme will be blocked by the handler.
        /// </summary>
        /// <exception cref="AntiSSRFException">Thrown when attempting to change the property after the policy has been used to create a handler.</exception>
        public bool AllowPlainTextHttp
        {
            get => _allowPlainTextHttp;
            set
            {
                if (_editLock)
                    throw new AntiSSRFException("Can't change AllowPlainTextHttp after policy has been used to create a handler");
                _allowPlainTextHttp = value;
            }
        }
        private bool _allowPlainTextHttp = false;

        /// <summary>
        /// Gets or sets whether the handler should add an "X-Forwarded-For: true" header to outgoing HTTP requests.
        /// </summary>
        /// <exception cref="AntiSSRFException">Thrown when attempting to change the property after the policy has been used to create a handler.</exception>
        public bool AddXFFHeader
        {
            get => _addXFFHeader;
            set
            {
                if (_editLock)
                    throw new AntiSSRFException("Can't change AddXFFHeader after policy has been used to create a handler");
                _addXFFHeader = value;
            }
        }
        private bool _addXFFHeader = false;

        /// <summary>
        /// Gets or sets whether the handler should deny all unspecified IP addresses. If true, any request to an IP address not explicitly allowed will be blocked.
        /// </summary>
        /// <exception cref="AntiSSRFException">Thrown when attempting to change the property after the policy has been used to create a handler.</exception>
        public bool DenyAllUnspecifiedIPs
        {
            get => _denyAllUnspecifiedIPs;
            set
            {
                if (_editLock)
                    throw new AntiSSRFException("Can't change DenyAllUnspecifiedIPs after policy has been used to create a handler");
                _denyAllUnspecifiedIPs = value;
            }
        }
        private bool _denyAllUnspecifiedIPs = false;

        /// <summary>
        /// Gets a read-only view of the currently denied headers.
        /// </summary>
        public IReadOnlyList<string> DeniedHeaders => _deniedHeaders.AsReadOnly();

        /// <summary>
        /// Gets a read-only view of the currently required headers.
        /// </summary>
        public IReadOnlyList<string> RequiredHeaders => _requiredHeaders.AsReadOnly();

        /// <summary>
        /// Gets a read-only view of the currently allowed IP address ranges as CIDR notation strings.
        /// </summary>
        public IReadOnlyList<string> AllowedAddresses => _allowedAddresses.ConvertAll(cidr => cidr.ToCIDR()).AsReadOnly();

        /// <summary>
        /// Gets a read-only view of the currently denied IP address ranges as CIDR notation strings.
        /// </summary>
        public IReadOnlyList<string> DeniedAddresses => _deniedAddresses.ConvertAll(cidr => cidr.ToCIDR()).AsReadOnly();

        /// <summary>
        /// Creates a new AntiSSRFPolicy instance with the specified configuration.
        /// </summary>
        /// <param name="config">The policy configuration option to use.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid policy option is provided.</exception>
        public AntiSSRFPolicy(PolicyConfigOptions config)
        {
            _deniedHeaders = new List<string>();
            _requiredHeaders = new List<string>();
            _allowedAddresses = new List<CIDRBlock>();
            _deniedAddresses = new List<CIDRBlock>();

            switch (config)
            {
                // Block all IPs by default. Users must add their intended internal ranges.
                case PolicyConfigOptions.InternalOnly:
                    DenyAllUnspecifiedIPs = true;
                    AddXFFHeader = false;
                    break;
                // Block recommendedV1 IPs. Blocks IMDS, so add XFF.
                case PolicyConfigOptions.ExternalOnlyV1:
                    AddDeniedAddresses(IPAddressRanges.recommendedV1);
                    AddXFFHeader = true;
                    break;
                // Block recommendedLatest IPs. Blocks IMDS, so add XFF.
                case PolicyConfigOptions.ExternalOnlyLatest:
                    AddDeniedAddresses(IPAddressRanges.recommendedLatest);
                    AddXFFHeader = true;
                    break;
                // No restrictions by default.
                case PolicyConfigOptions.None:
                    AddXFFHeader = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(config), config, "Argument must be a valid PolicyConfigOptions value");
            }
        }

        /// <summary>
        /// Adds IP networks to the allow list. Requests to any IP address that matches any of the specified networks will be allowed by the handler.
        /// Allowed addresses take precedence over denied addresses, so if an IP address matches a network on both the allow and deny list, it will be allowed.
        /// </summary>
        /// <param name="networks">The IP networks to allow.</param>
        /// <exception cref="ArgumentNullException">Thrown when the networks parameter is null or contains null values.</exception>
        /// <exception cref="FormatException">Thrown when a network string is not in valid CIDR format.</exception>
        /// <exception cref="AntiSSRFException">Thrown on attempts to edit the policy after it has been used to create a handler.</exception>
        public void AddAllowedAddresses(string[]? networks)
        {
            if (networks is null)
                throw new ArgumentNullException(nameof(networks));

            if (_editLock)
                throw new AntiSSRFException("Can't AddAllowedAddresses after policy is used to create a handler");

            List<CIDRBlock> parsedNetworks = new List<CIDRBlock>(networks.Length);

            foreach (string network in networks)
            {
                parsedNetworks.Add(CIDRBlock.Parse(network));
            }

            _allowedAddresses.AddRange(parsedNetworks);
        }

        /// <summary>
        /// Adds IP networks to the deny list. Requests to any IP address that matches any of the specified networks will be blocked by the handler.
        /// Allowed addresses take precedence over denied addresses, so if an IP address matches a network on both the allow and deny list, it will be allowed.
        /// </summary>
        /// <param name="networks">The IP networks to deny.</param>
        /// <exception cref="ArgumentNullException">Thrown when the networks parameter is null or contains null values.</exception>
        /// <exception cref="FormatException">Thrown when a network string is not in valid CIDR format.</exception>
        /// <exception cref="AntiSSRFException">Thrown on invalid operations or attempts to edit the policy after it has been used to create a handler.</exception>
        public void AddDeniedAddresses(string[]? networks)
        {
            if (networks is null)
                throw new ArgumentNullException(nameof(networks));

            if (_editLock)
                throw new AntiSSRFException("Can't AddDeniedAddresses after policy is used to create a handler");

            if (DenyAllUnspecifiedIPs)
                throw new AntiSSRFException("Can't add denied networks when DenyAllUnspecifiedIPs is true");

            List<CIDRBlock> parsedNetworks = new List<CIDRBlock>(networks.Length);

            foreach (string network in networks)
            {
                parsedNetworks.Add(CIDRBlock.Parse(network));
            }

            _deniedAddresses.AddRange(parsedNetworks);
        }

        /// <summary>
        /// Adds headers to the list of denied headers.
        /// HTTP requests containing any of these headers will be blocked by the handler.
        /// </summary>
        /// <param name="deniedHeaders">An array of denied header names.</param>
        /// <exception cref="ArgumentNullException">Thrown when the deniedHeaders parameter is null or contains null values.</exception>
        /// <exception cref="ArgumentException">Thrown when a header name is empty or whitespace.</exception>
        /// <exception cref="AntiSSRFException">Thrown on attempts to edit the policy after it has been used to create a handler.</exception>
        public void AddDeniedHeaders(string[]? deniedHeaders)
        {
            if (deniedHeaders is null)
                throw new ArgumentNullException(nameof(deniedHeaders));

            if (_editLock)
                throw new AntiSSRFException("Can't AddDeniedHeaders after policy is used to create a handler");

            foreach (string headerName in deniedHeaders)
            {
                if (headerName is null)
                    throw new ArgumentNullException(nameof(deniedHeaders), "Headers cannot be null");
                
                if (string.IsNullOrWhiteSpace(headerName))
                    throw new ArgumentException($"Headers cannot be empty or whitespace", nameof(deniedHeaders));
            }

            _deniedHeaders.AddRange(deniedHeaders);
        }

        /// <summary>
        /// Adds headers to the list of required headers.
        /// HTTP requests missing any of these headers will be blocked by the handler.
        /// </summary>
        /// <param name="requiredHeaders">An array of required header names.</param>
        /// <exception cref="ArgumentNullException">Thrown when the requiredHeaders parameter is null or contains null values.</exception>
        /// <exception cref="ArgumentException">Thrown when a header name is empty or whitespace.</exception>
        /// <exception cref="AntiSSRFException">Thrown on attempts to edit the policy after it has been used to create a handler.</exception>
        public void AddRequiredHeaders(string[]? requiredHeaders)
        {
            if (requiredHeaders is null)
                throw new ArgumentNullException(nameof(requiredHeaders));

            if (_editLock)
                throw new AntiSSRFException("Can't AddRequiredHeaders after policy is used to create a handler");

            foreach (string headerName in requiredHeaders)
            {
                if (headerName is null)
                    throw new ArgumentNullException(nameof(requiredHeaders), "Headers cannot be null");
                
                if (string.IsNullOrWhiteSpace(headerName))
                    throw new ArgumentException($"Headers cannot be empty or whitespace", nameof(requiredHeaders));
            }

            _requiredHeaders.AddRange(requiredHeaders);
        }

        /// <summary>
        /// Creates and returns a new <see cref="AntiSSRFHandler"/> instance based on the current policy configuration.
        /// </summary>
        /// <returns>A new <see cref="AntiSSRFHandler"/> instance based on this policy.</returns>
        public AntiSSRFHandler GetHandler()
        {
            _editLock = true;
            return new AntiSSRFHandler(this);
        }

        // --- Private and internal methods --- //

        internal bool IsNetworkConnectionAllowed(IPAddress[]? dnsResolvedIPAddresses)
        {
            if (dnsResolvedIPAddresses is null)
            {
                return false;
            }

            foreach (IPAddress ipAddress in dnsResolvedIPAddresses)
            {
                IPAddress ipv6Address = ipAddress.MapToIPv6();

                if (DenyAllUnspecifiedIPs)
                {
                    // If the address is not in an allow list, it's not allowed.
                    if (!NetworksContainAddress(_allowedAddresses, ipv6Address))
                    {
                        return false;
                    }
                }
                else if (_deniedAddresses != null && _deniedAddresses.Count > 0)
                {
                    // If the address is in a deny list and not in an allow list, it's not allowed.
                    if (NetworksContainAddress(_deniedAddresses, ipv6Address) &&
                        !NetworksContainAddress(_allowedAddresses, ipv6Address))
                    {
                        return false;
                    }
                }
            }

            // No ip addresses returned by DNS resolution was denied
            return true;
        }

        private static bool NetworksContainAddress(List<CIDRBlock> networks, IPAddress address)
        {
            foreach (var network in networks)
            {
                if (network.Contains(address))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsHttpRequestAllowed(string? scheme, HttpRequestHeaders headers)
        {
            if (!AllowPlainTextHttp && !"https".Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (AddXFFHeader && !headers.Contains("X-Forwarded-For"))
            {
                headers.Add("X-Forwarded-For", "true");
            }

            foreach (string header in _deniedHeaders)
            {
                if (headers.Contains(header))
                {
                    return false;
                }
            }

            foreach (string header in _requiredHeaders)
            {
                if (!headers.Contains(header))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
