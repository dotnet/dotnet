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

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// This set of tests if for ensuring the net45 targets are not broken.
/// </summary>
namespace Microsoft.IdentityModel.Net45.Tests
{
    [TestClass]
    public class Net45Tests
    {
        [TestMethod]
        public void TestJwtTokenCreationAndValidation()
        {
            IdentityModelEventSource.ShowPII = true;
            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();
            var jwt = handler.CreateEncodedJwt(Default.AsymmetricSignSecurityTokenDescriptor(null));
            var jwtToken = new JwtSecurityToken(jwt) { SigningKey = Default.AsymmetricSigningKey };
            SecurityToken token = null;
            handler.ValidateToken(jwt, Default.AsymmetricSignTokenValidationParameters, out token);
            var context = new CompareContext
            {
                PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    { typeof(JwtHeader), new List<string> { "Item" } },
                    { typeof(JwtPayload), new List<string> { "Item" } }
                }
            };

            if (!IdentityComparer.AreJwtSecurityTokensEqual(jwtToken, token as JwtSecurityToken, context))
                TestUtilities.AssertFailIfErrors("TestJwtTokenCreationAndValidation", context.Diffs);
        }
    }
}
