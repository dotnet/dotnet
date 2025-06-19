// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
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

        private AadIssuerValidator CreateIssuerValidatorWithConfigurationProvider(string authority, Func<string, BaseConfigurationManager> configurationProvider)
        {
            return AadIssuerValidator.GetAadIssuerValidator(authority, _httpClient, configurationProvider);
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
            IdentityComparer.AreEqual(ProtocolVersion.V1, validator.AadAuthorityVersion, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetIssuerValidator_TwoTenants()
        {
            var context = new CompareContext();
            var validator = CreateIssuerValidator(ValidatorConstants.AuthorityV1);

            IdentityComparer.AreEqual(ValidatorConstants.AuthorityV1, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityCommonTenantWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreEqual(ProtocolVersion.V1, validator.AadAuthorityVersion, context);

            validator = CreateIssuerValidator(ValidatorConstants.AuthorityWithTenantSpecified);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityWithTenantSpecified, validator.AadAuthorityV1, context);
            IdentityComparer.AreEqual(ValidatorConstants.AuthorityWithTenantSpecifiedWithV2, validator.AadAuthorityV2, context);
            IdentityComparer.AreEqual(ProtocolVersion.V1, validator.AadAuthorityVersion, context);

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
            IdentityComparer.AreEqual(ProtocolVersion.V2, validator.AadAuthorityVersion, context);
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
            IdentityComparer.AreEqual(ProtocolVersion.V2, validator.AadAuthorityVersion, context);
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
            IdentityComparer.AreEqual(ProtocolVersion.V2, validator.AadAuthorityVersion, context);
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
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.AuthorityCommonTenant, ValidatorConstants.AadIssuer, false)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.AuthorityCommonTenant, ValidatorConstants.AadIssuer, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.UsGovTenantId, ValidatorConstants.UsGovIssuer, false)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.UsGovTenantId, ValidatorConstants.UsGovIssuer, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.AuthorityCommonTenant, ValidatorConstants.AadIssuer, true)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.AuthorityCommonTenant, ValidatorConstants.AadIssuer, true)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.UsGovTenantId, ValidatorConstants.UsGovIssuer, true)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.UsGovTenantId, ValidatorConstants.UsGovIssuer, true)]
        public void Validate_IssuerMatchedInValidIssuer_ReturnsIssuer(string tidClaimType, string tenantId, string issuer, bool useConfigurationManagerProvider)
        {
            var context = new CompareContext();
            AadIssuerValidator validator = null;
            if (!useConfigurationManagerProvider)
                validator = new AadIssuerValidator(_httpClient, issuer);
            else
                validator = new AadIssuerValidator(_httpClient, issuer, x => null);

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
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer, false)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer, true)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.TenantIdAsGuid, ValidatorConstants.V1Issuer, true)]
        public void Validate_IssuerMatchedInValidV1Issuer_ReturnsIssuer(string tidClaimType, string tenantId, string issuer, bool useConfigurationProvider)
        {
            var context = new CompareContext();

            AadIssuerValidator validator = null;
            if (useConfigurationProvider == false)
                validator = new AadIssuerValidator(_httpClient, issuer);
            else
                validator = new AadIssuerValidator(_httpClient, issuer, x => null);

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
        [InlineData(ValidatorConstants.ClaimNameTid, false)]
        [InlineData(ValidatorConstants.TenantId, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, true)]
        [InlineData(ValidatorConstants.TenantId, true)]
        public void Validate_IssuerMatchedInValidIssuers_ReturnsIssuer(string tidClaimType, bool useConfigurationProvider)
        {
            var context = new CompareContext();

            AadIssuerValidator validator = null;
            if (useConfigurationProvider == false)
                validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            else
                validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer, x => null);

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
        [InlineData(ValidatorConstants.ClaimNameTid, false)]
        [InlineData(ValidatorConstants.TenantId, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, true)]
        [InlineData(ValidatorConstants.TenantId, true)]
        public void Validate_IssuerNotInTokenValidationParameters_ReturnsIssuer(string tidClaimType, bool useConfigurationProvider)
        {
            var context = new CompareContext();
            AadIssuerValidator validator = null;
            if (useConfigurationProvider == false)
                validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer);
            else
                validator = new AadIssuerValidator(_httpClient, ValidatorConstants.AadIssuer, authority => new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = ValidatorConstants.AadIssuer }));

            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });

            var actualIssuer = validator.Validate(ValidatorConstants.AadIssuer, jwtSecurityToken, new TokenValidationParameters());

            IdentityComparer.AreEqual(ValidatorConstants.AadIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.AadIssuer, false)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.AadIssuer, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.V1Issuer, false)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.V1Issuer, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.AadIssuer, true)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.AadIssuer, true)]
        [InlineData(ValidatorConstants.ClaimNameTid, ValidatorConstants.V1Issuer, true)]
        [InlineData(ValidatorConstants.TenantId, ValidatorConstants.V1Issuer, true)]
        public void ValidateJsonWebToken_ReturnsIssuer(string tidClaimType, string issuer, bool useConfigurationProvider)
        {
            AadIssuerValidator validator = null;
            if (useConfigurationProvider == false)
                validator = new AadIssuerValidator(_httpClient, issuer);
            else
                validator = new AadIssuerValidator(_httpClient, issuer, authority => new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = issuer }));

            var context = new CompareContext();
            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            List<Claim> claims = new List<Claim>();
            claims.Add(tidClaim);
            claims.Add(issClaim);

            var jsonWebToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, claims)));
            var actualIssuer = validator.Validate(issuer, jsonWebToken, new TokenValidationParameters());

            IdentityComparer.AreEqual(issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ValidatorConstants.ClaimNameTid, false)]
        [InlineData(ValidatorConstants.TenantId, false)]
        [InlineData(ValidatorConstants.ClaimNameTid, true)]
        [InlineData(ValidatorConstants.TenantId, true)]
        public void Validate_V1IssuerNotInTokenValidationParameters_ReturnsV1Issuer(string tidClaimType, bool useConfigurationProvider)
        {
            AadIssuerValidator validator = null;
            if (useConfigurationProvider == false)
                validator = new AadIssuerValidator(_httpClient, ValidatorConstants.V1Issuer);
            else
                validator = new AadIssuerValidator(_httpClient, ValidatorConstants.V1Issuer, authority => new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration() { Issuer = ValidatorConstants.V1Issuer }));

            var context = new CompareContext();
            var tidClaim = new Claim(tidClaimType, ValidatorConstants.TenantIdAsGuid);

            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.V1Issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.V1Issuer, claims: new[] { issClaim, tidClaim });

            var actualIssuer = validator.Validate(ValidatorConstants.V1Issuer, jwtSecurityToken, new TokenValidationParameters());

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
            IdentityComparer.AreEqual(ProtocolVersion.V2, validator.AadAuthorityVersion, context);
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
            IdentityComparer.AreEqual(ProtocolVersion.V2, validator.AadAuthorityVersion, context);
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
            IdentityComparer.AreEqual(ProtocolVersion.V2, validator.AadAuthorityVersion, context);
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

        [Theory]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V2)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V2)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V2)]
        public void Validate_WithAuthorityUsingConfigurationProvider(ProtocolVersion authorityVersion, ProtocolVersion tokenVersion)
        {
            var configurationManagerProvider = (string authority) =>
            {
                var configManagerMap = new Dictionary<string, BaseConfigurationManager>
                {
                    {
                        ValidatorConstants.AuthorityV1,
                        new MockConfigurationManager<OpenIdConnectConfiguration>(
                           new OpenIdConnectConfiguration()
                           {
                               Issuer = ValidatorConstants.AadIssuerV1CommonAuthority
                           })
                    },
                    {
                        ValidatorConstants.AuthorityCommonTenantWithV2,
                        new MockConfigurationManager<OpenIdConnectConfiguration>(
                           new OpenIdConnectConfiguration()
                           {
                               Issuer = ValidatorConstants.AadIssuerV2CommonAuthority
                           })
                    },
                    {
                        ValidatorConstants.AuthorityCommonTenantWithV11,
                        new MockConfigurationManager<OpenIdConnectConfiguration>(
                            new OpenIdConnectConfiguration()
                            {
                                Issuer = ValidatorConstants.AadIssuerV11CommonAuthority
                            })
                    }
                };

                return configManagerMap[authority];
            };

            var tokenIssuerProvider = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AadIssuerV11;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AadIssuer;

                return ValidatorConstants.V1Issuer;
            };

            var authorityUrlProvider = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AuthorityCommonTenantWithV11;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AuthorityCommonTenantWithV2;

                return ValidatorConstants.AuthorityV1;
            };

            var context = new CompareContext();
            var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);

            var tokenIssuer = tokenIssuerProvider(tokenVersion);
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, tokenIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: tokenIssuer, claims: new[] { issClaim, tidClaim });

            var authority = authorityUrlProvider(authorityVersion);
            var aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, _httpClient, configurationManagerProvider);

            var actualIssuer = aadIssuerValidator.Validate(tokenIssuer, jwtSecurityToken, new TokenValidationParameters());

            IdentityComparer.AreEqual(tokenIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V2)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V2)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V2)]
        public void Validate_UsesLKGWithoutConfigurationProvider(ProtocolVersion authorityVersion, ProtocolVersion tokenVersion)
        {
            var tokenIssuerProvider = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AadIssuerV11;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AadIssuer;

                return ValidatorConstants.V1Issuer;
            };

            var authorityUrlProvider = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AuthorityCommonTenantWithV11;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AuthorityCommonTenantWithV2;

                return ValidatorConstants.AuthorityV1;
            };

            var goodAuthorityIssuer = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AadIssuerV11CommonAuthority;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AadIssuerV2CommonAuthority;

                return ValidatorConstants.AadIssuerV1CommonAuthority;
            };

            var configurationManagerSetter = (AadIssuerValidator validator, bool isRefresh = false) =>
            {
                if (!isRefresh)
                {
                    validator.ConfigurationManagerV1 = new MockConfigurationManager<OpenIdConnectConfiguration>(
                        new OpenIdConnectConfiguration
                        {
                            Issuer = goodAuthorityIssuer(ProtocolVersion.V1)
                        });
                    validator.ConfigurationManagerV11 = new MockConfigurationManager<OpenIdConnectConfiguration>(
                        new OpenIdConnectConfiguration
                        {
                            Issuer = goodAuthorityIssuer(ProtocolVersion.V11)
                        });
                    validator.ConfigurationManagerV2 = new MockConfigurationManager<OpenIdConnectConfiguration>(
                        new OpenIdConnectConfiguration
                        {
                            Issuer = goodAuthorityIssuer(ProtocolVersion.V2)
                        });
                }
                else
                {
                    var refreshedConfig = new OpenIdConnectConfiguration
                    {
                        Issuer = "hxxp://brokenissuer/{tenantid}"
                    };

                    ((MockConfigurationManager<OpenIdConnectConfiguration>)validator.ConfigurationManagerV11).RefreshedConfiguration = refreshedConfig;
                    ((MockConfigurationManager<OpenIdConnectConfiguration>)validator.ConfigurationManagerV2).RefreshedConfiguration = refreshedConfig;
                    ((MockConfigurationManager<OpenIdConnectConfiguration>)validator.ConfigurationManagerV1).RefreshedConfiguration = refreshedConfig;
                    validator.ConfigurationManagerV11.RequestRefresh();
                    validator.ConfigurationManagerV2.RequestRefresh();
                    validator.ConfigurationManagerV1.RequestRefresh();
                }
            };

            var context = new CompareContext();
            var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);

            var issuer = tokenIssuerProvider(tokenVersion);
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, claims: new[] { issClaim, tidClaim });

            var authority = authorityUrlProvider(authorityVersion);
            var aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, _httpClient);

            // set config to a mock and assert on LKG being null
            configurationManagerSetter(aadIssuerValidator);

            // set LKG
            var actualIssuer = aadIssuerValidator.Validate(issuer, jwtSecurityToken, new TokenValidationParameters());
            IdentityComparer.AreEqual(issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);

            // replace config with broken issuer and validate with LKG
            configurationManagerSetter(aadIssuerValidator, true);

            actualIssuer = aadIssuerValidator.Validate(issuer, jwtSecurityToken, new TokenValidationParameters { ValidateWithLKG = true });
            IdentityComparer.AreEqual(issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V1, ProtocolVersion.V2)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V11, ProtocolVersion.V2)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V1)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V11)]
        [InlineData(ProtocolVersion.V2, ProtocolVersion.V2)]
        public void Validate_CanFetchMetadataWithoutConfigurationProvider(ProtocolVersion authorityVersion, ProtocolVersion tokenVersion)
        {
            var tokenIssuerProvider = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AadIssuerV11PPE;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AadIssuerPPE;

                return ValidatorConstants.V1IssuerPPE;
            };

            var authorityUrlProvider = (ProtocolVersion version) =>
            {
                if (version == ProtocolVersion.V11)
                    return ValidatorConstants.AuthorityCommonTenantWithV11PPE;

                if (version == ProtocolVersion.V2)
                    return ValidatorConstants.AuthorityCommonTenantWithV2PPE;

                return ValidatorConstants.AuthorityV1PPE;
            };
           
            var context = new CompareContext();
            var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);

            var issuer = tokenIssuerProvider(tokenVersion);
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, issuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: issuer, claims: new[] { issClaim, tidClaim });

            var authority = authorityUrlProvider(authorityVersion);
            var aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(authority, _httpClient);
                        
            // set LKG
            var actualIssuer = aadIssuerValidator.Validate(issuer, jwtSecurityToken, new TokenValidationParameters());
            IdentityComparer.AreEqual(issuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void Validate_UsesLKGWithConfigurationProvider()
        {
            var v1Configuration = new OpenIdConnectConfiguration
            {
                Issuer = ValidatorConstants.AadIssuerV1CommonAuthority
            };

            var v1ConfigurationRefreshed = new OpenIdConnectConfiguration
            {
                Issuer = "hxxp://brokenissuer/{tenantid}"
            };

            var v2Configuration = new OpenIdConnectConfiguration
            {
                Issuer = ValidatorConstants.AadIssuerV2CommonAuthority
            };

            var v2ConfigurationRefreshed = new OpenIdConnectConfiguration
            {
                Issuer = "hxxp://brokenissuer/{tenantid}"
            };

            var v1ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(v1Configuration);
            var v2ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(v2Configuration);

            var configurationManagerProvider = (string authority) =>
            {
                var configManagerMap = new Dictionary<string, BaseConfigurationManager>
                {
                    {
                        ValidatorConstants.AuthorityV1,
                        v1ConfigurationManager
                    },
                    {
                        ValidatorConstants.AuthorityCommonTenantWithV2,
                        v2ConfigurationManager
                    }
                };

                return configManagerMap[authority];
            };

            var context = new CompareContext();
            var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);

            var v2TokenIssuer = ValidatorConstants.AadIssuer;
            var issClaim = new Claim(ValidatorConstants.ClaimNameIss, v2TokenIssuer);
            var jwtSecurityToken = new JwtSecurityToken(issuer: v2TokenIssuer, claims: new[] { issClaim, tidClaim });

            var v2Authority = ValidatorConstants.AuthorityCommonTenantWithV2;
            var v1Authority = ValidatorConstants.AuthorityCommonTenant;
            var aadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(v2Authority, _httpClient, configurationManagerProvider);
            var v1AadIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(v1Authority, _httpClient, configurationManagerProvider);

            // set LKG
            var actualIssuer = aadIssuerValidator.Validate(v2TokenIssuer, jwtSecurityToken, new TokenValidationParameters());

            IdentityComparer.AreEqual(v2TokenIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);
                        
            // refresh config to a one with a broken issuer and validate with LKG
            v2ConfigurationManager.RefreshedConfiguration = v2ConfigurationRefreshed;
            v2ConfigurationManager.RequestRefresh();

            actualIssuer = aadIssuerValidator.Validate(v2TokenIssuer, jwtSecurityToken, new TokenValidationParameters { ValidateWithLKG = true });
            IdentityComparer.AreEqual(v2TokenIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);

            var v1TokenIssuer = ValidatorConstants.V1Issuer;
            issClaim = new Claim(ValidatorConstants.ClaimNameIss, v1TokenIssuer);
            var v1JwtSecurityToken = new JwtSecurityToken(issuer: v1TokenIssuer, claims: new[] { issClaim, tidClaim });

            // before testing v1 LKG setup v1 LKG for v2 manager for cross version validation
            _ = aadIssuerValidator.Validate(v1TokenIssuer, v1JwtSecurityToken, new TokenValidationParameters()); 

            // V1 token and authority behaves like v2 token and authority
            actualIssuer = v1AadIssuerValidator.Validate(v1TokenIssuer, v1JwtSecurityToken, new TokenValidationParameters());
            IdentityComparer.AreEqual(v1TokenIssuer, actualIssuer, context);
            IdentityComparer.AreEqual(null, v1ConfigurationManager.LastKnownGoodConfiguration, context);
            TestUtilities.AssertFailIfErrors(context);

            //      refresh config to a broken one and validate with LKG
            v1ConfigurationManager.RefreshedConfiguration = v1ConfigurationRefreshed;
            v1ConfigurationManager.RequestRefresh();

            actualIssuer = v1AadIssuerValidator.Validate(v1TokenIssuer, v1JwtSecurityToken, new TokenValidationParameters { ValidateWithLKG = true });
            IdentityComparer.AreEqual(v1TokenIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);

            // validating cross versions also validates with LKG
            actualIssuer = aadIssuerValidator.Validate(v1TokenIssuer, v1JwtSecurityToken, new TokenValidationParameters { ValidateWithLKG = true });

            IdentityComparer.AreEqual(v1TokenIssuer, actualIssuer, context);
            TestUtilities.AssertFailIfErrors(context);

            // if LKG not valid validation fails
            //    set confgimanager lkg lifetime to 1ms
            //    validate successfully to set LKG
            //    wait 1ms, validate with expired LKG
            v1ConfigurationManager.RefreshedConfiguration = v1Configuration;
            v1ConfigurationManager.RequestRefresh();

            v1ConfigurationManager.LastKnownGoodLifetime = TimeSpan.FromMilliseconds(1);
            actualIssuer = aadIssuerValidator.Validate(v1TokenIssuer, v1JwtSecurityToken, new TokenValidationParameters());
            Thread.Sleep(TimeSpan.FromMilliseconds(1));

            var securityExceptionThrown = false;
            var exceptionMessage = string.Empty;
            try
            {
                _ = aadIssuerValidator.Validate(v1TokenIssuer, v1JwtSecurityToken, new TokenValidationParameters { ValidateWithLKG = true });
            }
            catch (SecurityTokenInvalidIssuerException securityException)
            {
                securityExceptionThrown = true;
                exceptionMessage = securityException.Message;
            }

            IdentityComparer.AreEqual(true, securityExceptionThrown, context);
            IdentityComparer.AreEqual("IDX40001: Issuer: 'https://sts.windows.net/f645ad92-e38d-4d1a-b510-d1b09a74a8ca/', does not match any of the valid issuers provided for this application. ", exceptionMessage, context);
            TestUtilities.AssertFailIfErrors(context);
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
