// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Validators.Tests
{
    public class MicrosoftIdentityIssuerValidatorTest
    {
        private readonly HttpClient _httpClient;

        public MicrosoftIdentityIssuerValidatorTest()
        {
            Logging.IdentityModelEventSource.ShowPII = true;
            _httpClient = new HttpClient();
            AadIssuerValidator.s_issuerValidators.Clear();
        }

        private AadIssuerValidator CreateIssuerValidator(string authority)
        {
            return AadIssuerValidator.GetAadIssuerValidator(authority, _httpClient);
        }

        [Fact]
        public void GetIssuerValidator_NullOrEmptyAuthority_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(ValidatorConstants.AadAuthority, () => CreateIssuerValidator(string.Empty));

            Assert.Throws<ArgumentNullException>(ValidatorConstants.AadAuthority, () => CreateIssuerValidator(null));
        }

        [Fact]
        public void GetIssuerValidator_InvalidAuthority_ReturnsValidatorBasedOnFallbackAuthority()
        {
            Assert.NotNull(CreateIssuerValidator(ValidatorConstants.InvalidAuthorityFormat));
        }

        [Fact]
        public void GetIssuerValidator_V1Authority()
        {
            var context = new CompareContext();
            var authorityInAliases = ValidatorConstants.AuthorityV1;

            var validator = CreateIssuerValidator(authorityInAliases);

            IdentityComparer.AreEqual(ValidatorConstants.AuthorityV1, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityCommonTenantWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(false, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetIssuerValidator_TwoTenants()
        {
            var context = new CompareContext();
            var validator = CreateIssuerValidator(ValidatorConstants.AuthorityV1);

            IdentityComparer.AreEqual(ValidatorConstants.AuthorityV1, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityCommonTenantWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(false, validator.IsV2Authority, context);

            validator = CreateIssuerValidator(ValidatorConstants.AuthorityWithTenantSpecified);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityWithTenantSpecified, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityWithTenantSpecifiedWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(false, validator.IsV2Authority, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetIssuerValidator_CommonAuthorityInAliases()
        {
            var context = new CompareContext();
            var authorityInAliases = ValidatorConstants.AuthorityCommonTenantWithV2;

            var validator = CreateIssuerValidator(authorityInAliases);

            IdentityComparer.AreEqual(ValidatorConstants.AuthorityV1, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityCommonTenantWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(true, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetIssuerValidator_OrganizationsAuthorityInAliases()
        {
            var context = new CompareContext();
            var authorityInAliases = ValidatorConstants.AuthorityOrganizationsWithV2;

            var validator = CreateIssuerValidator(authorityInAliases);

            IdentityComparer.AreEqual(ValidatorConstants.AuthorityV1, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityOrganizationsWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(true, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetIssuerValidator_B2cAuthorityNotInAliases()
        {
            var context = new CompareContext();
            var authorityNotInAliases = ValidatorConstants.B2CAuthorityWithV2;

            var validator = CreateIssuerValidator(authorityNotInAliases);
            IdentityComparer.AreEqual(ValidatorConstants.B2CAuthority, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.B2CAuthorityWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(true, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetIssuerValidator_CachedAuthority_ReturnsCachedValidator()
        {
            var context = new CompareContext();
            var authorityNotInAliases = ValidatorConstants.AuthorityWithTenantSpecifiedWithV2;

            var validator1 = CreateIssuerValidator(authorityNotInAliases);
            var validator2 = CreateIssuerValidator(authorityNotInAliases);

            IdentityComparer.AreEqual(validator1, validator2, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_NullOrEmptyParameters_ThrowsException()
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken();
            var validationParams = new TokenValidationParameters();

            Assert.Throws<ArgumentNullException>(ValidatorConstants.Issuer, () => validator.Validate(null, jwtSecurityToken, validationParams));

            var exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() => validator.Validate(string.Empty, jwtSecurityToken, validationParams));

            IdentityComparer.AreEqual(LogMessages.IDX40003, exception.Message);

            Assert.Throws<ArgumentNullException>(ValidatorConstants.SecurityToken, () => validator.Validate(ValidatorConstants.AadIssuer, null, validationParams));

            Assert.Throws<ArgumentNullException>(ValidatorConstants.ValidationParameters, () => validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, null));
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_NullOrEmptyTenantId_ThrowsException()
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken();
            var jsonWebToken = new JsonWebToken($"{{}}", $"{{}}");
            var securityToken = Substitute.For<SecurityToken>();
            var validationParameters = new TokenValidationParameters();

            var exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() => validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, validationParameters));
            IdentityComparer.AreEqual(LogMessages.IDX40003, exception.Message, context);

            exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() => validator.Validate(ValidatorConstants.AadIssuer, jsonWebToken, validationParameters));
            IdentityComparer.AreEqual(LogMessages.IDX40003, exception.Message, context);

            exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() => validator.Validate(ValidatorConstants.AadIssuer, securityToken, validationParameters));
            IdentityComparer.AreEqual(LogMessages.IDX40003, exception.Message, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.AuthorityCommonTenant, ValidatorConstants.AadIssuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.AuthorityCommonTenant, ValidatorConstants.AadIssuer)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.UsGovTenantId, ValidatorConstants.UsGovIssuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.UsGovTenantId, ValidatorConstants.UsGovIssuer)]
        public void Validate_IssuerMatchedInValidIssuer_ReturnsIssuer(string tidClaimType, string tenantId, string issuer)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, issuer);
            var tidClaim = new Claim(tidClaimType, tenantId);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, claims: new[] { issClaim, tidClaim });

            validator.AadIssuerV2 = issuer;

            var actualIssuer = validator.Validate(issuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuer = issuer });

            IdentityComparer.AreEqual(issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.AadIssuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.AadIssuer)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer)]
        public void Validate_NoHttpclientFactory_ReturnsIssuer(string tidClaimType, string tenantId, string issuer)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(null, issuer);
            var tidClaim = new Claim(tidClaimType, tenantId);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, claims: new[] { issClaim, tidClaim });

            var tokenValidationParams = new TokenValidationParameters() { ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = issuer }) };

            IdentityComparer.AreEqual(issuer, validator.Validate(issuer, jwtSecurityToken, tokenValidationParams), context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer)]
        public void Validate_IssuerMatchedInValidV1Issuer_ReturnsIssuer(string tidClaimType, string tenantId, string issuer)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, issuer);
            var tidClaim = new Claim(tidClaimType, tenantId);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, claims: new[] { issClaim, tidClaim });

            validator.AadIssuerV1 = issuer;

            var actualIssuer = validator.Validate(issuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuer = issuer });

            IdentityComparer.AreEqual(issuer, actualIssuer, context);

            var actualIssuers = validator.Validate(issuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuers = new[] { issuer } });

            IdentityComparer.AreEqual(issuer, actualIssuers, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid)]
        [InlineData(ValidatorConstants.TenantId)]
        public void Validate_IssuerMatchedInValidIssuers_ReturnsIssuer(string tidClaimType)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });

            var actualIssuers = validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuers = new[] { ValidatorConstants.AadIssuer } });

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuers, context);

            var actualIssuer = validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuer = ValidatorConstants.AadIssuer });

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid)]
        [InlineData(ValidatorConstants.TenantId)]
        public void Validate_IssuerNotInTokenValidationParameters_ReturnsIssuer(string tidClaimType)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });
            var tokenValidationParams = new TokenValidationParameters() { ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = ValidatorConstants.AadIssuer }) };

            var actualIssuer = validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, tokenValidationParams);

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.AadIssuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.AadIssuer)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.V1Issuer)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.V1Issuer)]
        public void ValidateJsonWebToken_ReturnsIssuer(string tidClaimType, string issuer)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, issuer);
            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            List<Claim> claims = new List<Claim>();
            claims.Add(tidClaim);
            claims.Add(issClaim);

            var jsonWebToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, claims)));
            var tokenValidationParams = new TokenValidationParameters() { ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = issuer }) };

            var actualIssuer = validator.Validate(issuer, jsonWebToken, tokenValidationParams);

            IdentityComparer.AreEqual(issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid)]
        [InlineData(ValidatorConstants.TenantId)]
        public void Validate_V1IssuerNotInTokenValidationParameters_ReturnsV1Issuer(string tidClaimType)
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.V1Issuer);
            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.V1Issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.V1Issuer, claims: new[] { issClaim, tidClaim });

            var tokenValidationParams = new TokenValidationParameters() { ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = ValidatorConstants.V1Issuer }) };

            var actualIssuer = validator.Validate(ValidatorConstants.V1Issuer, jwtSecurityToken, tokenValidationParams);

            IdentityComparer.AreEqual(ValidatorConstants.V1Issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_TenantIdInIssuerNotInToken_ReturnsIssuer()
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim });

            var actualIssuer = validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuer = ValidatorConstants.AadIssuer });

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_TidClaimInToken_ReturnsIssuer()
        {
            var context = new CompareContext();
            var validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });
            var jsonWebToken = new JsonWebToken($"{{}}", $"{{\"{ValidatorConstants.ClaimNameIss}\":\"{ValidatorConstants.AadIssuer}\",\"{ValidatorConstants.ClaimNameTid}\":\"{ValidatorConstants.TenantIdAsGuid}\"}}");

            var actualIssuer = validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, new TokenValidationParameters() { ValidIssuer = ValidatorConstants.AadIssuer });

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuer, context);

            actualIssuer = validator.Validate(ValidatorConstants.AadIssuer, jsonWebToken, new TokenValidationParameters() { ValidIssuer = ValidatorConstants.AadIssuer });

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        // Regression test for https://github.com/Azure-Samples/active-directory-dotnet-native-aspnetcore-v2/issues/68
        // Similar to Validate_NotMatchedToMultipleIssuers_ThrowsException but uses B2C values
        [Fact]
        public void Validate_InvalidIssuerToValidate_ThrowsException()
        {
            var context = new CompareContext();
            string invalidIssuerToValidate = $"https://badissuer/{ValidatorConstants.TenantIdAsGuid}/v2.0";
            AadIssuerValidator validator = new AadIssuerValidator(_httpClient, invalidIssuerToValidate);
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
            Claim tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });
            var expectedErrorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    LogMessages.IDX40001,
                    invalidIssuerToValidate);

            var exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() =>
                validator.Validate(invalidIssuerToValidate, jwtSecurityToken, new TokenValidationParameters() { ValidIssuers = new[] { ValidatorConstants.AadIssuer } }));
            IdentityComparer.AreEqual(expectedErrorMessage, exception.Message, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromB2CAuthority_WithNoTidClaim_ValidateSuccessfully()
        {
            var context = new CompareContext();
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.B2CIssuer);
            Claim tfpClaim = new Claim(ValidatorConstants.ClaimNameTfp, ValidatorConstants.B2CSignUpSignInUserFlow);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.B2CIssuer, claims: new[] { issClaim, tfpClaim });

            AadIssuerValidator validator = CreateIssuerValidator(ValidatorConstants.B2CAuthorityWithV2);

            validator.Validate(
                ValidatorConstants.B2CIssuer,
                jwtSecurityToken,
                new TokenValidationParameters()
                {
                    ValidIssuers = new[] { ValidatorConstants.B2CIssuer },
                });
            IdentityComparer.AreEqual(ValidatorConstants.B2CAuthority, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.B2CAuthorityWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(true, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromB2CAuthority_WithTokenValidateParametersValidIssuersUnspecified_ValidateSuccessfully()
        {
            var context = new CompareContext();
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.B2CIssuer);
            var tfpClaim = new Claim(ValidatorConstants.ClaimNameTfp, ValidatorConstants.B2CSignUpSignInUserFlow);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.B2CIssuer, claims: new[] { issClaim, tfpClaim });

            var validator = new AadIssuerValidator(null, ValidatorConstants.B2CAuthority);

            var tokenValidationParams = new TokenValidationParameters()
            {
                ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration()
                {
                    Issuer = ValidatorConstants.B2CIssuer
                })
            };

            IdentityComparer.AreEqual(ValidatorConstants.B2CIssuer, validator.Validate(ValidatorConstants.B2CIssuer, jwtSecurityToken, tokenValidationParams), context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromB2CAuthority_WithTidClaim_ValidateSuccessfully()
        {
            var context = new CompareContext();
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.B2CIssuer);
            Claim tfpClaim = new Claim(ValidatorConstants.ClaimNameTfp, ValidatorConstants.B2CSignUpSignInUserFlow);
            Claim tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.B2CTenantAsGuid);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.B2CIssuer, claims: new[] { issClaim, tfpClaim, tidClaim });

            AadIssuerValidator validator = CreateIssuerValidator(ValidatorConstants.B2CAuthorityWithV2);

            validator.Validate(
                ValidatorConstants.B2CIssuer,
                jwtSecurityToken,
                new TokenValidationParameters()
                {
                    ValidIssuers = new[] { ValidatorConstants.B2CIssuer },
                });
            IdentityComparer.AreEqual(ValidatorConstants.B2CAuthority, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.B2CAuthorityWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(true, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromB2CAuthority_InvalidIssuer_Fails()
        {
            var context = new CompareContext();
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.B2CIssuer2);
            Claim tfpClaim = new Claim(ValidatorConstants.ClaimNameTfp, ValidatorConstants.B2CSignUpSignInUserFlow);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.B2CIssuer2, claims: new[] { issClaim, tfpClaim });

            AadIssuerValidator validator = CreateIssuerValidator(ValidatorConstants.B2CAuthorityWithV2);

            var exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() =>
                validator.Validate(
                    ValidatorConstants.B2CIssuer2,
                    jwtSecurityToken,
                    new TokenValidationParameters()
                    {
                        ValidIssuers = new[] { ValidatorConstants.B2CIssuer },
                    }));
            IdentityComparer.AreEqual(string.Format(LogMessages.IDX40001, ValidatorConstants.B2CIssuer2), exception.Message, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromB2CAuthority_InvalidIssuerTid_Fails()
        {
            var context = new CompareContext();
            string issuerWithInvalidTid = ValidatorConstants.B2CInstance + "/" + ValidatorConstants.TenantIdAsGuid + "/v2.0";
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuerWithInvalidTid);
            Claim tfpClaim = new Claim(ValidatorConstants.ClaimNameTfp, ValidatorConstants.B2CSignUpSignInUserFlow);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: issuerWithInvalidTid, claims: new[] { issClaim, tfpClaim });

            AadIssuerValidator validator = CreateIssuerValidator(ValidatorConstants.B2CAuthorityWithV2);

            var exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() =>
                validator.Validate(
                    issuerWithInvalidTid,
                    jwtSecurityToken,
                    new TokenValidationParameters()
                    {
                        ValidIssuers = new[] { ValidatorConstants.B2CIssuer },
                    }));

            IdentityComparer.AreEqual(string.Format(LogMessages.IDX40001, issuerWithInvalidTid), exception.Message, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromCustomB2CAuthority_ValidateSuccessfully()
        {
            var context = new CompareContext();
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.B2CCustomDomainIssuer);
            Claim tfpClaim = new Claim(ValidatorConstants.ClaimNameTfp, ValidatorConstants.B2CCustomDomainUserFlow);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.B2CCustomDomainIssuer, claims: new[] { issClaim, tfpClaim });

            AadIssuerValidator validator = CreateIssuerValidator(ValidatorConstants.B2CCustomDomainAuthorityWithV2);

            validator.Validate(
                ValidatorConstants.B2CCustomDomainIssuer,
                jwtSecurityToken,
                new TokenValidationParameters()
                {
                    ValidIssuers = new[] { ValidatorConstants.B2CCustomDomainIssuer },
                });

            IdentityComparer.AreEqual(ValidatorConstants.B2CCustomDomainAuthority, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.B2CCustomDomainAuthorityWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreBoolsEqual(true, validator.IsV2Authority, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_FromB2CAuthority_WithTfpIssuer_ThrowsException()
        {
            var context = new CompareContext();
            Claim issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.B2CIssuerTfp);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.B2CIssuerTfp, claims: new[] { issClaim });

            AadIssuerValidator validator = CreateIssuerValidator(ValidatorConstants.B2CAuthorityWithV2);

            var exception = Assert.Throws<SecurityTokenInvalidIssuerException>(() =>
                validator.Validate(
                    ValidatorConstants.B2CIssuerTfp,
                    jwtSecurityToken,
                    new TokenValidationParameters()
                    {
                        ValidIssuers = new[] { ValidatorConstants.B2CIssuerTfp },
                    }));

            IdentityComparer.AreEqual(LogMessages.IDX40002, exception.Message, context);
            TestUtilities.AssertFailIfErrors(context);
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
