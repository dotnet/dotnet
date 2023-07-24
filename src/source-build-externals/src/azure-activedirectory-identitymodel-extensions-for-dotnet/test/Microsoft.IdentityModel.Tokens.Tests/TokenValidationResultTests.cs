// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
            if (properties.Length != 10)
                Assert.True(false, "Number of public fields has changed from 10 to: " + properties.Length + ", adjust tests");

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
