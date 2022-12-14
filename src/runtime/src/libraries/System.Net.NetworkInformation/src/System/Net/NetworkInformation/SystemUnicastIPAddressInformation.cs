// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
    // Specifies the unicast addresses for an interface.
    internal sealed class SystemUnicastIPAddressInformation : UnicastIPAddressInformation
    {
        private readonly long _dhcpLeaseLifetime;
        private readonly SystemIPAddressInformation _innerInfo;
        private readonly IPAddress? _ipv4Mask;
        private readonly PrefixOrigin _prefixOrigin;
        private readonly SuffixOrigin _suffixOrigin;
        private readonly DuplicateAddressDetectionState _dadState;
        private readonly uint _validLifetime;
        private readonly uint _preferredLifetime;
        private readonly byte _prefixLength;

        internal SystemUnicastIPAddressInformation(in Interop.IpHlpApi.IpAdapterUnicastAddress adapterAddress)
        {
            IPAddress ipAddress = adapterAddress.address.MarshalIPAddress();
            _innerInfo = new SystemIPAddressInformation(ipAddress, adapterAddress.flags);
            _prefixOrigin = adapterAddress.prefixOrigin;
            _suffixOrigin = adapterAddress.suffixOrigin;
            _dadState = adapterAddress.dadState;
            _validLifetime = adapterAddress.validLifetime;
            _preferredLifetime = adapterAddress.preferredLifetime;
            _dhcpLeaseLifetime = adapterAddress.leaseLifetime;

            _prefixLength = adapterAddress.prefixLength;

            // IPv6 returns 0.0.0.0 for consistency with down-level platforms.
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                _ipv4Mask = PrefixLengthToSubnetMask(_prefixLength, ipAddress.AddressFamily);
            }
        }

        public override IPAddress Address { get { return _innerInfo.Address; } }

        public override IPAddress IPv4Mask
        {
            get
            {
                // The IPv6 equivalent was never available on down-level platforms.
                // We've kept this behavior for legacy reasons. For IPv6 use PrefixLength instead.
                if (Address.AddressFamily != AddressFamily.InterNetwork)
                {
                    return IPAddress.Any;
                }

                return _ipv4Mask!;
            }
        }

        public override int PrefixLength
        {
            get
            {
                return _prefixLength;
            }
        }

        // The address is a cluster address and shouldn't be used by most applications.
        public override bool IsTransient
        {
            get
            {
                return _innerInfo.IsTransient;
            }
        }

        // This address can be used for DNS.
        public override bool IsDnsEligible
        {
            get
            {
                return _innerInfo.IsDnsEligible;
            }
        }

        public override PrefixOrigin PrefixOrigin
        {
            get
            {
                return _prefixOrigin;
            }
        }

        public override SuffixOrigin SuffixOrigin
        {
            get
            {
                return _suffixOrigin;
            }
        }

        public override DuplicateAddressDetectionState DuplicateAddressDetectionState
        {
            get
            {
                return _dadState;
            }
        }

        // Specifies the valid lifetime of the address in seconds.
        public override long AddressValidLifetime
        {
            get
            {
                return _validLifetime;
            }
        }

        // Specifies the preferred lifetime of the address in seconds.
        public override long AddressPreferredLifetime
        {
            get
            {
                return _preferredLifetime;
            }
        }

        // Specifies the preferred lifetime of the address in seconds.
        public override long DhcpLeaseLifetime
        {
            get
            {
                return _dhcpLeaseLifetime;
            }
        }

        // Helper method that marshals the address information into the classes.
        internal static unsafe UnicastIPAddressInformationCollection MarshalUnicastIpAddressInformationCollection(IntPtr ptr)
        {
            UnicastIPAddressInformationCollection addressList = new UnicastIPAddressInformationCollection();

            Interop.IpHlpApi.IpAdapterUnicastAddress* pIpAdapterAddress = (Interop.IpHlpApi.IpAdapterUnicastAddress*)ptr;
            while (pIpAdapterAddress != null)
            {
                addressList.InternalAdd(new SystemUnicastIPAddressInformation(in *pIpAdapterAddress));
                pIpAdapterAddress = pIpAdapterAddress->next;
            }

            return addressList;
        }
    }
}
