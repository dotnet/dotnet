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
using System.Reflection;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class TokenValidationResultTests
    {
        [Fact]
        public void GetSets()
        {
            TestUtilities.WriteHeader("TokenValidationResultTests.GetSets()");

            TokenValidationResult tokenValidationResult = new TokenValidationResult();
            Type type = typeof(TokenValidationResult);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 9)
                Assert.True(false, "Number of public fields has changed from 9 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("ClaimsIdentity", new List<object>{(ClaimsIdentity)null, new ClaimsIdentity(), new ClaimsIdentity()}),
                        new KeyValuePair<string, List<object>>("Exception", new List<object>{(Exception)null, new Exception(), new Exception()}),
                        new KeyValuePair<string, List<object>>("Issuer",  new List<object>{(string)null, "issuer", "issuer2"}),
                        new KeyValuePair<string, List<object>>("IsValid", new List<object>{false, false, true}),
                        new KeyValuePair<string, List<object>>("SecurityToken", new List<object>{(SecurityToken)null, new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor())), new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor()))}),
                        new KeyValuePair<string, List<object>>("TokenContext", new List<object>{(CallContext)null, new CallContext(), new CallContext()}),
                        new KeyValuePair<string, List<object>>("TokenType", new List<object>{(string)null, "JWTToken", "JwtToken2"}),
                        new KeyValuePair<string, List<object>>("PropertyBag", new List<object>{ tokenValidationResult.PropertyBag })
                    },
                    Object = tokenValidationResult,
                };

            TestUtilities.GetSet(context);

            TestUtilities.AssertFailIfErrors("TokenValidationResultTests.GetSets", context.Errors);
        }
    }
}
