// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlTestData
    {
        public static TheoryData<CreateTokenTheoryData> SecurityKeyNotFoundExceptionTestTheoryData()
        {
            return new TheoryData<CreateTokenTheoryData>()
            {
                new CreateTokenTheoryData
                {
                    First = true,
                    TestId = "TokenExpired",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                     },
                     ValidationParameters = new TokenValidationParameters
                     {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer,
                     },
                     ExpectedException = ExpectedException.SecurityTokenExpiredException("IDX10223:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "InvalidIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                    },
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "ExpiredAndInvalidIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    },
                    ExpectedException = ExpectedException.SecurityTokenExpiredException("IDX10223:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "KeysDontMatchValidLifetimeAndIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.X509SecurityKey_AAD_Public,
                        ValidIssuer = Default.Issuer,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireAudience = false,
                        ValidateLifetime = false
                    },
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512:")
                }
            };
        }
    }
}
