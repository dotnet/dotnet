// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Represents the Cnf Claim
    /// </summary>
    internal class Cnf : Tokens.Cnf
    {
        public Cnf() : base()
        {
        }

        public Cnf(string json) : base(json)
        {
        }
    }
}
