// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System;

namespace Microsoft.Security.AntiSSRF
{
    // Based on [System.Net IPNetwork Struct](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipnetwork?view=net-10.0)
    internal readonly struct CIDRBlock
    {
        private readonly byte[] _networkBytes;
        private readonly byte[] _maskBytes;
        private readonly int _prefixLength;

        private CIDRBlock(IPAddress ipv6, int prefixLength)
        {
            Debug.Assert(ipv6.AddressFamily == AddressFamily.InterNetworkV6);
            
            _prefixLength = prefixLength;
            
#if NETCOREAPP2_1_OR_GREATER
            Span<byte> ipBytes = stackalloc byte[16];
            ipv6.TryWriteBytes(ipBytes, out _);
#else
            var ipBytes = ipv6.GetAddressBytes();
#endif

            // Precompute the mask
            _maskBytes = new byte[16];
            var fullBytes = Math.DivRem(prefixLength, 8, out int remainingBits);
            
            // Set full mask bytes to 0xFF
            for (int i = 0; i < fullBytes; i++)
            {
                _maskBytes[i] = 0xFF;
            }
            
            // Set partial mask byte
            if (remainingBits > 0 && fullBytes < 16)
            {
                _maskBytes[fullBytes] = (byte)(0xFF << (8 - remainingBits));
            }
            
            // Precompute the network address (ip & mask)
            _networkBytes = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                _networkBytes[i] = (byte)(ipBytes[i] & _maskBytes[i]);
            }
        }

        /// <exception cref="ArgumentNullException">Thrown when cidrString is null</exception>
        /// <exception cref="FormatException">Thrown when cidrString is not valid CIDR notation</exception>
        internal static CIDRBlock Parse(string cidrString)
        {
            if (cidrString is null)
            {
                throw new ArgumentNullException(nameof(cidrString));
            }

            string[] _parts = cidrString.Split('/');
            if (IPAddress.TryParse(_parts[0], out IPAddress? ipAddress))
            {
                switch (_parts.Length)
                {
                    case 1:
                        return new CIDRBlock(ipAddress.MapToIPv6(), 128);

                    case 2:
                        // Validate that the prefix part contains only digits (no signs, whitespace, or leading zeros except "0")
                        string prefixPart = _parts[1];
                        if (!string.IsNullOrEmpty(prefixPart) && 
                            (prefixPart == "0" || (prefixPart[0] != '0' && prefixPart.All(char.IsDigit))) &&
                            int.TryParse(prefixPart, out int prefixLength))
                        {
                            switch (ipAddress.AddressFamily)
                            {
                                case AddressFamily.InterNetwork when (uint)prefixLength <= 32:
                                    return new CIDRBlock(ipAddress.MapToIPv6(), prefixLength + 96);

                                case AddressFamily.InterNetworkV6 when (uint)prefixLength <= 128:
                                    return new CIDRBlock(ipAddress, prefixLength);
                            }
                        }
                        break;
                }
            }

            throw new FormatException("Invalid CIDR notation");
        }

        /// <exception cref="ArgumentNullException">Thrown when ip is null</exception>
        internal bool Contains(IPAddress ip)
        {
            if (ip is null)
            {
                throw new ArgumentNullException(nameof(ip));
            }

            // Convert to IPv6 and get bytes
            var ipv6 = ip.MapToIPv6();

#if NETCOREAPP2_1_OR_GREATER
            Span<byte> ipBytes = stackalloc byte[16];
            ipv6.TryWriteBytes(ipBytes, out _);
#else
            var ipBytes = ipv6.GetAddressBytes();
#endif

            // Apply mask and compare with precomputed network address
            for (int i = 0; i < 16; i++)
            {
                if ((ipBytes[i] & _maskBytes[i]) != _networkBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        internal bool Contains(CIDRBlock other)
        {
            // For one CIDR to contain another, this CIDR must have broader or equal scope (shorter or equal prefix)
            if (_prefixLength > other._prefixLength)
            {
                return false;
            }

            // Apply this CIDR's mask to the other CIDR's network address and compare
            for (int i = 0; i < 16; i++)
            {
                if ((other._networkBytes[i] & _maskBytes[i]) != _networkBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        internal string ToCIDR()
        {
            var networkAddress = new IPAddress(_networkBytes);
            
            // Check if this is an IPv4 address mapped to IPv6 (prefix length > 96)
            if (_prefixLength >= 96 && networkAddress.IsIPv4MappedToIPv6)
            {
                // Extract the IPv4 part and adjust prefix length
                var ipv4Address = networkAddress.MapToIPv4();
                int ipv4PrefixLength = _prefixLength - 96;
                return $"{ipv4Address}/{ipv4PrefixLength}";
            }
            else
            {
                // Pure IPv6 address
                return $"{networkAddress}/{_prefixLength}";
            }
        }
    }
}