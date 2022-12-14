// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Versioning;

namespace System.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about a network interface address.
    /// </summary>
    public abstract class IPAddressInformation
    {
        /// <summary>
        /// Gets the Internet Protocol (IP) address.
        /// </summary>
        public abstract IPAddress Address { get; }

        /// <summary>
        /// Gets a bool value that indicates whether the Internet Protocol (IP) address is legal to appear in a Domain Name System (DNS) server database.
        /// </summary>
        [SupportedOSPlatform("windows")]
        public abstract bool IsDnsEligible { get; }

        /// <summary>
        /// Gets a bool value that indicates whether the Internet Protocol (IP) address is transient.
        /// </summary>
        [SupportedOSPlatform("windows")]
        public abstract bool IsTransient { get; }
    }
}
