// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Contains WsFederation metadata that can be populated from a XML string.
    /// </summary>
    public class WsFederationConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Initializes an new instance of <see cref="WsFederationConfiguration"/>.
        /// </summary>
        public WsFederationConfiguration()
        {
        }

        /// <summary>
        /// The <see cref="Xml.Signature"/> element that was found when reading metadata.
        /// </summary>
        public Signature? Signature
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Tokens.SigningCredentials"/> that was used to sign the metadata.
        /// </summary>
        public SigningCredentials? SigningCredentials
        {
            get;
            set;
        }

        /// <summary>
        /// Get the <see cref="IList{KeyInfo}"/> that the IdentityProvider indicates are to be used signing keys.
        /// </summary>
        public ICollection<KeyInfo> KeyInfos
        {
            get;
        } = new List<KeyInfo>();
    }
}
