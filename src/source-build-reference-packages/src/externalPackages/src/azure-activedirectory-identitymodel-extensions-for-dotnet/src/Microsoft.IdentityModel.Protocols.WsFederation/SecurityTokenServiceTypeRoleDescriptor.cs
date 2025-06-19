// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Class for SecurityTokenService type role descriptor
    /// </summary>
    public class SecurityTokenServiceTypeRoleDescriptor
    {
        /// <summary>
        /// KeyInfo
        /// </summary>
        public List<KeyInfo> KeyInfos
        {
            get;
            set;
        } = new List<KeyInfo>();

        /// <summary>
        /// Passive Requestor Token endpoint
        /// fed:PassiveRequestorEndpoint, https://docs.oasis-open.org/wsfed/federation/v1.2/os/ws-federation-1.2-spec-os.html#:~:text=fed%3ASecurityTokenServiceType/fed%3APassiveRequestorEndpoint
        /// </summary>
        public string? TokenEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Active Requestor Token Endpoint
        /// fed:SecurityTokenServiceType, http://docs.oasis-open.org/wsfed/federation/v1.2/os/ws-federation-1.2-spec-os.html#:~:text=fed%3ASecurityTokenSerivceEndpoint
        /// </summary>
        public string? ActiveTokenEndpoint
        {
            get;
            set;
        }
    }
}
