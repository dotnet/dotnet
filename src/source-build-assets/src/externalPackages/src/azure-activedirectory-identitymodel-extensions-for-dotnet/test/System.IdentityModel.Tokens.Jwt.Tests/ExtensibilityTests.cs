// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

// since we are in the System ns, we need to map to M.IM.Tokens
using Token = Microsoft.IdentityModel.Tokens.SecurityToken;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    /// <summary>
    /// Test some key extensibility scenarios
    /// </summary>
    public class ExtensibilityTests
    {
        [Fact]
        public void JwtSecurityTokenHandler_Extensibility()
        {
            DerivedJwtSecurityTokenHandler handler = new DerivedJwtSecurityTokenHandler()
            {
                DerivedTokenType = typeof(DerivedJwtSecurityToken)
            };

            JwtSecurityToken jwt =
                new JwtSecurityToken
                (
                    issuer: Default.Issuer,
                    audience: Default.Audience,
                    claims: ClaimSets.Simple(Default.Issuer, Default.Issuer),
                    signingCredentials: KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
                    expires: DateTime.UtcNow + TimeSpan.FromHours(10),
                    notBefore: DateTime.UtcNow
                );

            string encodedJwt = handler.WriteToken(jwt);
            TokenValidationParameters tvp = new TokenValidationParameters()
            {
                IssuerSigningKey = KeyingMaterial.DefaultX509Key_2048,
                ValidateAudience = false,
                ValidIssuer = Default.Issuer,
            };

            List<string> errors = new List<string>();
            ValidateDerived(encodedJwt, handler, tvp, ExpectedException.NoExceptionExpected, errors);
        }

        private void ValidateDerived(string jwt, DerivedJwtSecurityTokenHandler handler, TokenValidationParameters validationParameters, ExpectedException expectedException, List<string> errors)
        {
            try
            {
                Token validatedToken;
                handler.ValidateToken(jwt, validationParameters, out validatedToken);
                if ((handler.Jwt as DerivedJwtSecurityToken) == null)
                    errors.Add("(handler.Jwt as DerivedJwtSecurityToken) == null");

                if (!handler.ReadTokenCalled)
                    errors.Add("!handler.ReadTokenCalled");

                if (!handler.ValidateAudienceCalled)
                    errors.Add("!handler.ValidateAudienceCalled");

                if (!handler.ValidateIssuerCalled)
                    errors.Add("!handler.ValidateIssuerCalled");

                if (!handler.ValidateIssuerSigningKeyCalled)
                    errors.Add("!handler.ValidateIssuerSigningKeyCalled");

                if (!handler.ValidateLifetimeCalled)
                    errors.Add("!handler.ValidateLifetimeCalled");

                if (!handler.ValidateSignatureCalled)
                    errors.Add("!handler.ValidateSignatureCalled");

                expectedException.ProcessNoException(errors);
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex, errors);
            }
        }

        private void RunAlgorithmMappingTest(string jwt, TokenValidationParameters validationParameters, JwtSecurityTokenHandler handler, ExpectedException expectedException)
        {
            try
            {
                Token validatedToken;
                handler.ValidateToken(jwt, validationParameters, out validatedToken);
                expectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex);
            }
        }

        private string ReplaceAlgorithm(string algorithmKey, string newAlgorithmValue, IDictionary<string, string> algorithmMap)
        {
            string originalAlgorithmValue = null;
            if (algorithmMap.TryGetValue(algorithmKey, out originalAlgorithmValue))
            {
                algorithmMap.Remove(algorithmKey);
            }

            if (!string.IsNullOrWhiteSpace(newAlgorithmValue))
            {
                algorithmMap.Add(algorithmKey, newAlgorithmValue);
            }

            return originalAlgorithmValue;
        }
    }
}
