// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenIdConnectProtocolValidationContextTests
    {
        [Fact]
        public void GetSets()
        {
            OpenIdConnectProtocolValidationContext validationContext = new OpenIdConnectProtocolValidationContext();
            Type type = typeof(OpenIdConnectProtocolValidationContext);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 6)
                Assert.True(true, "Number of properties has changed from 6 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>> 
                    { 
                        new KeyValuePair<string, List<object>>("State", new List<object>{(string)null, "AuthorizationCode", "AuthorizationCode_AuthorizationCode"}),
                        new KeyValuePair<string, List<object>>("Nonce", new List<object>{(string)null, "Nonce", "Nonce_Nonce"}),
                    },
                    Object = validationContext,
                };
            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("OpenIdConnectProtocolValidationContext_GetSets", context.Errors);
        }
    }
}
