// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.Tests
{
    public class MessageComparer
    {
        public static bool AreEqual(AuthenticationProtocolMessage authenticationProtocolMessage1, AuthenticationProtocolMessage authenticationProtocolMessage2)
        {
            if (!DoObjectsHaveSameNullState(authenticationProtocolMessage1, authenticationProtocolMessage2))
            {
                return false;
            }

            if (authenticationProtocolMessage1 == null)
            {
                return true;
            }

            if (!DoObjectsHaveSameNullState(authenticationProtocolMessage1.IssuerAddress, authenticationProtocolMessage2.IssuerAddress))
            {
                return false;
            }

            if (authenticationProtocolMessage1.IssuerAddress != null && authenticationProtocolMessage1.IssuerAddress != authenticationProtocolMessage2.IssuerAddress)
            {
                return false;
            }

            if (authenticationProtocolMessage1.Parameters.Count != authenticationProtocolMessage2.Parameters.Count)
            {
                return false;
            }

            return true;
        }

#if SAML
        public static bool AreEqual(WsFederationMessage wsFederationMessage1, WsFederationMessage wsFederationMessage2)
        {
            if (!MessageComparer.AreEqual(wsFederationMessage1 as AuthenticationProtocolMessage, wsFederationMessage2 as AuthenticationProtocolMessage))
            {
                return false;
            }

            return true;
        }
#endif
        static bool DoObjectsHaveSameNullState(object object1, object object2)
        {
            if ((object1 == null && object2 != null) || (object1 != null && object2 == null))
            {
                return false;
            }

            return true;
        }
    }
}
