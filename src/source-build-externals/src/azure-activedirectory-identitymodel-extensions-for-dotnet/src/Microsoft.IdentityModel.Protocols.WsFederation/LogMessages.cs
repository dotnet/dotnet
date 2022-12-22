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

// Microsoft.IdentityModel.Protocols.WsFederation
// Range: 22000 - 22999

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // general
        internal const string IDX22000 = "IDX22000: The parameter '{0}' cannot be a 'null' or an empty object.";

        // wsfederation messages
        internal const string IDX22900 = "IDX22900: Building wsfederation message from query string: '{0}'.";
        internal const string IDX22901 = "IDX22901: Building wsfederation message from uri: '{0}'.";
        internal const string IDX22902 = "IDX22902: Token is not found in Wresult";
        internal const string IDX22903 = "IDX22903: Multiple tokens were found in the RequestSecurityTokenCollection. Only a single token is supported.";
        internal const string IDX22904 = "IDX22904: Wresult does not contain a 'RequestedSecurityToken' element.";

        // xml metadata messages
        internal const string IDX22800 = "IDX22800: Exception thrown while reading WsFedereationMetadata. Element '{0}'. Caught exception: '{1}'.";
        internal const string IDX22801 = "IDX22801: entityID attribute is not found in EntityDescriptor element in metadata file.";
        internal const string IDX22802 = "IDX22802: Current name '{0} and namespace '{1}' do not match the expected name '{2}' and namespace '{3}'.";
        internal const string IDX22803 = "IDX22803: Token reference address is missing in SecurityTokenServiceEndpoint in metadata file.";
        internal const string IDX22804 = "IDX22804: Security token type role descriptor is expected.";
        internal const string IDX22806 = "IDX22806: Key descriptor for signing is missing in security token service type RoleDescriptor.";
        internal const string IDX22807 = "IDX22807: Token endpoint is missing in security token service type RoleDescriptor.";
        internal const string IDX22808 = "IDX22808: 'Use' attribute is missing in KeyDescriptor.";
        internal const string IDX22810 = "IDX22810: 'Issuer' value is missing in wsfederationconfiguration.";
        internal const string IDX22811 = "IDX22811: 'TokenEndpoint' value is missing in wsfederationconfiguration.";
        internal const string IDX22812 = "IDX22812: Element: '{0}' was an empty element. 'TokenEndpoint' value is missing in wsfederationconfiguration.";

#pragma warning restore 1591
    }
}
