// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// An enumeration describing the different support phases of a <see cref="Product"/>.
    /// See the <see href="https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core">support lifecycle</see>
    /// documentation for further details.
    /// </summary>
    public enum SupportPhase
    {
        /// <summary>
        /// The product is in active support and will continue to receive servicing and security updates.
        /// </summary>
        Active,

        /// <summary>
        /// The product is considered end-of-life and will not receive any updates.
        /// </summary>
        EOL,

        /// <summary>
        /// A preview or release candidate that is supported in production.
        /// </summary>
        GoLive,

        /// <summary>
        /// The product is no longer in active support and will be declared <see cref="Product.EndOfLifeDate">end-of-life</see>.
        /// Only security fixes are provided until the product reaches end-of-life status.
        /// </summary>
        Maintenance,

        /// <summary>
        /// The product is a preview release and is unsupported.
        /// </summary>
        Preview,

        /// <summary>
        /// The support phase is unknown and could not be parsed.
        /// </summary>
        Unknown = 99
    }
}
