// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// An enumeration describing the different support phases of a <see cref="Product"/>.
    /// </summary>
    [JsonConverter(typeof(SupportPhaseConverter))]
    public enum SupportPhase
    {
        /// <summary>
        /// The product is actively supported and will receive updates.
        /// </summary>
        Current = 0,
        /// <summary>
        /// The product is considered end-of-life and will not receive any updates.
        /// </summary>
        EOL,
        /// <summary>
        /// The product is in long term support and will continue to receive updates.
        /// </summary>
        LTS,
        /// <summary>
        /// The product is no longer in active support and will be declared end-of-life (see <see cref="Product.EndOfLifeDate"/>).
        /// Only security fixes are provided until the product reaches end-of-life status.
        /// </summary>
        Maintenance,
        /// <summary>
        /// The product is a preview release.
        /// </summary>
        Preview,
        /// <summary>
        /// The support phase designates a release candidate.
        /// </summary>
        RC,
        /// <summary>
        /// The support phase is unrecognized.
        /// </summary>
        Unknown = 99
    }
}
