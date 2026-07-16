// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    [CollectionDefinition("JwtSecurityTokenHandlerTestsWithContextSwitches", DisableParallelization = true)]
    public class JwtSecurityTokenHandlerTestsWithContextSwitches
    {
        [Theory]
        [InlineData(SecurityAlgorithms.RsaOAEP, true)]
        [InlineData(SecurityAlgorithms.RsaOaepKeyWrap, false)]
        public void JwtSecurityTokenHandler_CreateToken_AddShortFormMappingForRsaOAEP(string algorithm, bool useShortNameForRsaOaepKey)
        {
            AppContext.SetSwitch(AppContextSwitches.UseShortNameForRsaOaepKeySwitch, useShortNameForRsaOaepKey);
            var encryptingCredentials = new X509EncryptingCredentials(Default.Certificate);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken token = CreateJwtSecurityToken(tokenHandler, encryptingCredentials);

            Assert.Equal(token.Header.Alg, algorithm);

            AppContextSwitches.ResetAllSwitches();
        }

        [Theory]
        [InlineData(SecurityAlgorithms.RsaOAEP, true)]
        [InlineData(SecurityAlgorithms.RsaOaepKeyWrap, false)]
        public void JsonWebTokenHandler_CreateToken_AddShortFormMappingForRsaOAEP(string algorithm, bool useShortNameForRsaOaepKey)
        {
            AppContext.SetSwitch(AppContextSwitches.UseShortNameForRsaOaepKeySwitch, useShortNameForRsaOaepKey);
            var encryptingCredentials = new X509EncryptingCredentials(Default.Certificate);
            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

            JsonWebToken jsonToken = new JsonWebToken(CreateJwtSecurityTokenAsString(tokenHandler, encryptingCredentials));

            Assert.Equal(jsonToken.Alg, algorithm);

            AppContextSwitches.ResetAllSwitches();
        }

        private JwtSecurityToken CreateJwtSecurityToken(JwtSecurityTokenHandler tokenHandler, X509EncryptingCredentials encryptingCredentials)
        {
            return tokenHandler.CreateJwtSecurityToken(CreateTokenDescriptor(encryptingCredentials));
        }

        private string CreateJwtSecurityTokenAsString(JsonWebTokenHandler tokenHandler, X509EncryptingCredentials encryptingCredentials)
        {
            return tokenHandler.CreateToken(CreateTokenDescriptor(encryptingCredentials));
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(X509EncryptingCredentials encryptingCredentials)
        {
            return new SecurityTokenDescriptor
            {
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                EncryptingCredentials = encryptingCredentials,
            };
        }
    }
}
