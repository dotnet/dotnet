// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// An enumeration describing different releases types based on their support duration.
    /// See the <see href="https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core">support lifecycle</see>
    /// documentation for further details.
    /// </summary>
    public enum ReleaseType
    {
        /// <summary>
        /// Indicates a release is supported for the Long Term Support (LTS) timeframe (3 years).
        /// </summary>
        LTS,

        /// <summary>
        /// Indicates a release is supported for the Standard Term Support (STS) timeframe (18 months).
        /// </summary>
        STS,

        /// <summary>
        /// The release type is unknown and could not be parsed.
        /// </summary>
        Unknown = 99
    }
}
