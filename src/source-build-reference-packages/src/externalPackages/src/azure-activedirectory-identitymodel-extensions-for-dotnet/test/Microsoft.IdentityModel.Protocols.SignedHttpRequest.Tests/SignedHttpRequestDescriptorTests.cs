// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Protocols.SignedHttpRequest;
using Microsoft.IdentityModel.Tokens;
using Xunit;


namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestDescriptorTests
    {
        [Fact]
        public void SignedHttpRequestDescriptor()
        {
            var httpRequestData = new HttpRequestData();
            var accessToken = SignedHttpRequestTestUtils.DefaultEncodedAccessToken;
            var signingCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials;
            var creationParameters = new SignedHttpRequestCreationParameters();
            var callContext = new CallContext();
            Assert.Throws<ArgumentNullException>("accessToken", () => new SignedHttpRequestDescriptor(null, httpRequestData, null));
            Assert.Throws<ArgumentNullException>("accessToken", () => new SignedHttpRequestDescriptor(null, httpRequestData, null, null));
            Assert.Throws<ArgumentNullException>("accessToken", () => new SignedHttpRequestDescriptor(string.Empty, httpRequestData, null));
            Assert.Throws<ArgumentNullException>("accessToken", () => new SignedHttpRequestDescriptor(string.Empty, httpRequestData, null, null));
            Assert.Throws<ArgumentNullException>("httpRequestData", () => new SignedHttpRequestDescriptor(accessToken, null, null));
            Assert.Throws<ArgumentNullException>("httpRequestData", () => new SignedHttpRequestDescriptor(accessToken, null, null, null));
            Assert.Throws<ArgumentNullException>("signingCredentials", () => new SignedHttpRequestDescriptor(accessToken, httpRequestData, null));
            Assert.Throws<ArgumentNullException>("signingCredentials", () => new SignedHttpRequestDescriptor(accessToken, httpRequestData, null, null));
            Assert.Throws<ArgumentNullException>("signedHttpRequestCreationParameters", () => new SignedHttpRequestDescriptor(accessToken, httpRequestData, signingCredentials, null));

            // no exceptions
            var signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(accessToken, httpRequestData, signingCredentials);
            Assert.Equal(httpRequestData, signedHttpRequestDescriptor.HttpRequestData);
            Assert.Equal(accessToken, signedHttpRequestDescriptor.AccessToken);
            Assert.Equal(signingCredentials, signedHttpRequestDescriptor.SigningCredentials);
            Assert.NotNull(signedHttpRequestDescriptor.SignedHttpRequestCreationParameters);

            signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(accessToken, httpRequestData, signingCredentials, creationParameters);
            Assert.Equal(httpRequestData, signedHttpRequestDescriptor.HttpRequestData);
            Assert.Equal(accessToken, signedHttpRequestDescriptor.AccessToken);
            Assert.Equal(signingCredentials, signedHttpRequestDescriptor.SigningCredentials);
            Assert.Equal(creationParameters, signedHttpRequestDescriptor.SignedHttpRequestCreationParameters);
        }
    }
}
