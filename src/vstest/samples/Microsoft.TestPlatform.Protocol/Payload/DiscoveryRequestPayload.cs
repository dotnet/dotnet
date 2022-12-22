// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Protocol
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Class used to define the DiscoveryRequestPayload sent by the Vstest.console translation layers into design mode
    /// </summary>
    public class DiscoveryRequestPayload
    {
        /// <summary>
        /// Settings used for the discovery request.
        /// </summary>
        [DataMember]
        public IEnumerable<string> Sources { get; set; }

        /// <summary>
        /// Settings used for the discovery request.
        /// </summary>
        [DataMember]
        public string RunSettings { get; set; }
    }
}
