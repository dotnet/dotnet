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

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Prompt types for OpenIdConnect.
    /// </summary>
    public static class OpenIdConnectPrompt
    {
        /// <summary>
        /// Indicates 'none' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Indicates 'login' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string Login = "login";

        /// <summary>
        /// Indicates 'consent' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string Consent = "consent";

        /// <summary>
        /// Indicates 'select_account' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string SelectAccount = "select_account";
    }
}

