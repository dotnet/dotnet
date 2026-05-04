// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    [Collection(nameof(JsonWebTokenHandlerClaimsIdentityTests))]
    public class JsonWebTokenHandlerClaimsIdentityTests
    {
        [Fact]
        public void CreateClaimsIdentity_ReturnsCaseSensitveClaimsIdentity_ByDefault()
        {
            var handler = new DerivedJsonWebTokenHandler();
            var jsonWebToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor()));
            var tokenValidationParameters = new TokenValidationParameters();

            var actualClaimsIdentity = handler.CreateClaimsIdentity(jsonWebToken, tokenValidationParameters);
            Assert.IsType<CaseSensitiveClaimsIdentity>(actualClaimsIdentity);
            Assert.NotNull(((CaseSensitiveClaimsIdentity)actualClaimsIdentity).SecurityToken);

            actualClaimsIdentity = handler.CreateClaimsIdentity(jsonWebToken, tokenValidationParameters, Default.Issuer);
            Assert.IsType<CaseSensitiveClaimsIdentity>(actualClaimsIdentity);
            Assert.NotNull(((CaseSensitiveClaimsIdentity)actualClaimsIdentity).SecurityToken);

            actualClaimsIdentity = handler.CreateClaimsIdentityInternal(jsonWebToken, tokenValidationParameters, Default.Issuer);
            Assert.IsType<CaseSensitiveClaimsIdentity>(actualClaimsIdentity);
            Assert.NotNull(((CaseSensitiveClaimsIdentity)actualClaimsIdentity).SecurityToken);

            // This will also test mapped claims flow.
            handler.MapInboundClaims = true;
            actualClaimsIdentity = handler.CreateClaimsIdentityInternal(jsonWebToken, tokenValidationParameters, Default.Issuer);
            Assert.IsType<CaseSensitiveClaimsIdentity>(actualClaimsIdentity);
            Assert.NotNull(((CaseSensitiveClaimsIdentity)actualClaimsIdentity).SecurityToken);

            AppContextSwitches.ResetAllSwitches();
        }

        [Fact]
        public void CreateClaimsIdentity_ReturnsClaimsIdentity_WithAppContextSwitch()
        {
            AppContext.SetSwitch(AppContextSwitches.UseClaimsIdentityTypeSwitch, true);

            var handler = new DerivedJsonWebTokenHandler();
            var jsonWebToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor()));
            var tokenValidationParameters = new TokenValidationParameters();

            Assert.IsType<ClaimsIdentity>(handler.CreateClaimsIdentity(jsonWebToken, tokenValidationParameters));
            Assert.IsType<ClaimsIdentity>(handler.CreateClaimsIdentity(jsonWebToken, tokenValidationParameters, Default.Issuer));
            Assert.IsType<ClaimsIdentity>(handler.CreateClaimsIdentityInternal(jsonWebToken, tokenValidationParameters, Default.Issuer));
            // This will also test mapped claims flow.
            handler.MapInboundClaims = true;
            Assert.IsType<ClaimsIdentity>(handler.CreateClaimsIdentityInternal(jsonWebToken, tokenValidationParameters, Default.Issuer));

            AppContextSwitches.ResetAllSwitches();
        }

        private class DerivedJsonWebTokenHandler : JsonWebTokenHandler
        {
            public new ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters) => base.CreateClaimsIdentity(jwtToken, validationParameters);
            public new ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters, string issuer) => base.CreateClaimsIdentity(jwtToken, validationParameters, issuer);
            public new ClaimsIdentity CreateClaimsIdentityInternal(SecurityToken securityToken, TokenValidationParameters tokenValidationParameters, string issuer) => base.CreateClaimsIdentityInternal(securityToken, tokenValidationParameters, issuer);
        }
    }
}
