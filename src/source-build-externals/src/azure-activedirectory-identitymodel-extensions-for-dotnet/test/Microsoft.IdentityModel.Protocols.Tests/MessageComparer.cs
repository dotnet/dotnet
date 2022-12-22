//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
