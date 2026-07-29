// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    // This interface is used to test the extensibility of the ValidateTokenAsync method
    // in the JsonWebTokenHandler, SamlSecurityTokenHandler, and Saml2SecurityTokenHandler classes,
    // since the ValidateTokenAsync method with ValidationParameters is not part of any shared interface.
    internal interface ITestingTokenHandler
    {
        Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken);

        Task<TokenValidationResult> ValidateTokenAsync(
            string token,
            TokenValidationParameters validationParameters);

        Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken);

        SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor);

        string CreateStringToken(SecurityTokenDescriptor tokenDescriptor);

        string CreateStringTokenNoKid(SecurityTokenDescriptor tokenDescriptor);

        bool SetDefaultTimesOnTokenCreation { get; set; }

        string CreateTamperedSignature(SecurityTokenDescriptor securityTokenDescriptor);

        string CreateWithTamperedHeader(SecurityTokenDescriptor securityTokenDescriptor);

        string CreateWithTamperedPayload(SecurityTokenDescriptor securityTokenDescriptor);

        string CreateWithoutSignature(SecurityTokenDescriptor securityTokenDescriptor);
    }

    // Because the ValidateTokenAsync method in the destinationToken handler subclasses is internal, we need
    // to create classes that implement the ValidateTokenAsyncResult interface to use in tests.
    internal class JsonWebTestingTokenHandler : ITestingTokenHandler
    {
        private readonly JsonWebTokenHandler _handler = new JsonWebTokenHandler();

        public JsonWebTestingTokenHandler()
        {
        }

        public async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters, callContext, cancellationToken);
        }

        public async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters, callContext, cancellationToken);
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(
            string token,
            TokenValidationParameters validationParameters)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters);
        }

        public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;
            string token = _handler.CreateToken(tokenDescriptor);

            return _handler.ReadToken(token);
        }

        public string CreateStringToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;

            return _handler.CreateToken(tokenDescriptor);
        }

        public string CreateStringTokenNoKid(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;

            return _handler.CreateToken(tokenDescriptor);
        }

        public bool SetDefaultTimesOnTokenCreation { get; set; } = true;

        public string CreateTamperedSignature(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string token = CreateStringToken(securityTokenDescriptor);
            string newToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Issuer = Guid.NewGuid().ToString(),
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            });

            var parts = token.Split('.');
            var parts2 = newToken.Split('.');

            return parts[0] + "." + parts[1] + "." + parts2[2];
        }

        public string CreateWithTamperedPayload(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string token = CreateStringToken(securityTokenDescriptor);
            string newToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(2),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            });

            var parts = token.Split('.');
            var parts2 = newToken.Split('.');

            return parts[0] + "." + parts2[1] + "." + parts[2];
        }

        public string CreateWithTamperedHeader(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string token = CreateStringToken(securityTokenDescriptor);
            string newToken = CreateStringToken(new SecurityTokenDescriptor
            {
                AdditionalHeaderClaims = new Dictionary<string, object>
                {
                    { "Claim", "Value" }
                },
                Audience = Default.Audience,
                Expires = DateTime.UtcNow + TimeSpan.FromDays(2),
                Issuer = Default.Issuer,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = Default.ClaimsIdentityLongNames
            });

            var parts = token.Split('.');
            var parts2 = newToken.Split('.');

            return parts2[0] + "." + parts[1] + "." + parts[2];
        }

        public string CreateWithoutSignature(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string token = CreateStringToken(securityTokenDescriptor);
            var parts = token.Split('.');

            return parts[0] + "." + parts[1] + ".";
        }
    }

    internal class SamlSecurityTestingTokenHandler : ITestingTokenHandler
    {
        private readonly SamlSecurityTokenHandler _handler = new SamlSecurityTokenHandler();

        public async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters, callContext, cancellationToken);
        }

        public async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters, callContext, cancellationToken);
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(
            string token,
            TokenValidationParameters validationParameters)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters);
        }

        public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;
            SamlSecurityToken token = (SamlSecurityToken)_handler.CreateToken(tokenDescriptor);
            // SamlSecurityTokenHandler.CreateToken does not set correctly the signature on the destinationToken.
            // Reading the destinationToken from the CanonicalString will set the signature correctly.
            return _handler.ReadToken(token.Assertion.CanonicalString);
        }

        public string CreateStringToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;

            return ((SamlSecurityToken)_handler.CreateToken(tokenDescriptor)).Assertion.CanonicalString;
        }

        public string CreateStringTokenNoKid(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;
            string token = ((SamlSecurityToken)_handler.CreateToken(tokenDescriptor)).Assertion.CanonicalString;

            return XmlUtilities.RemoveElement(token, "KeyInfo");
        }

        public bool SetDefaultTimesOnTokenCreation { get; set; } = true;

        public string CreateWithTamperedHeader(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string destinationToken = CreateStringToken(securityTokenDescriptor);

            ClaimsIdentity claimsIdentity = Default.ClaimsIdentityLongNames;
            claimsIdentity.AddClaim(new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Default.Issuer));

            string sourceToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = securityTokenDescriptor.Audience,
                Expires = securityTokenDescriptor.Expires,
                Issuer = securityTokenDescriptor.Issuer,
                SigningCredentials = securityTokenDescriptor.SigningCredentials,
                Subject = claimsIdentity
            });

            return XmlUtilities.SwapAttributeStatements(sourceToken, destinationToken);
        }

        public string CreateWithTamperedPayload(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string destinationToken = CreateStringToken(securityTokenDescriptor);

            ClaimsIdentity claimsIdentity = Default.ClaimsIdentityLongNames;
            claimsIdentity.AddClaim(new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Default.Issuer));

            string sourceToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = securityTokenDescriptor.Audience,
                Expires = securityTokenDescriptor.Expires,
                Issuer = securityTokenDescriptor.Issuer,
                SigningCredentials = securityTokenDescriptor.SigningCredentials,
                Subject = claimsIdentity
            });

            return XmlUtilities.SwapAttributeStatements(sourceToken, destinationToken);
        }

        public string CreateTamperedSignature(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string destinationToken = CreateStringToken(securityTokenDescriptor);

            string sourceToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = securityTokenDescriptor.Audience,
                Expires = securityTokenDescriptor.Expires,
                Issuer = securityTokenDescriptor.Issuer,
                SigningCredentials = securityTokenDescriptor.SigningCredentials,
                Subject = securityTokenDescriptor.Subject
            });

            return XmlUtilities.SwapSignatureValueElements(sourceToken, destinationToken);
        }

        public string CreateWithoutSignature(SecurityTokenDescriptor securityTokenDescriptor)
        {
            // Create the SAML destinationToken as a string
            string token = CreateStringToken(securityTokenDescriptor);

            return XmlUtilities.RemoveSignature(token);
        }
    }

    internal class Saml2SecurityTestingTokenHandler : ITestingTokenHandler
    {
        private readonly Saml2SecurityTokenHandler _handler = new Saml2SecurityTokenHandler();

        public async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters, callContext, cancellationToken);
        }

        public async Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters, callContext, cancellationToken);
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(
            string token,
            TokenValidationParameters validationParameters)
        {
            return await _handler.ValidateTokenAsync(token, validationParameters);
        }

        public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;
            Saml2SecurityToken token = (Saml2SecurityToken)_handler.CreateToken(tokenDescriptor);

            return _handler.ReadToken(token.Assertion.CanonicalString);
        }

        public string CreateStringToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;

            return ((Saml2SecurityToken)_handler.CreateToken(tokenDescriptor)).Assertion.CanonicalString;
        }

        public string CreateStringTokenNoKid(SecurityTokenDescriptor tokenDescriptor)
        {
            _handler.SetDefaultTimesOnTokenCreation = SetDefaultTimesOnTokenCreation;
            string token = ((Saml2SecurityToken)_handler.CreateToken(tokenDescriptor)).Assertion.CanonicalString;

            return XmlUtilities.RemoveElement(token, "KeyInfo");
        }

        public bool SetDefaultTimesOnTokenCreation { get; set; } = true;

        public string CreateWithTamperedHeader(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string destinationToken = CreateStringToken(securityTokenDescriptor);

            ClaimsIdentity claimsIdentity = Default.ClaimsIdentityLongNames;
            claimsIdentity.AddClaim(new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Default.Issuer));

            string sourceToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = securityTokenDescriptor.Audience,
                Expires = securityTokenDescriptor.Expires,
                Issuer = securityTokenDescriptor.Issuer,
                SigningCredentials = securityTokenDescriptor.SigningCredentials,
                Subject = claimsIdentity
            });

            return XmlUtilities.SwapAttributeStatements(sourceToken, destinationToken);
        }

        public string CreateWithTamperedPayload(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string destinationToken = CreateStringToken(securityTokenDescriptor);

            ClaimsIdentity claimsIdentity = Default.ClaimsIdentityLongNames;
            claimsIdentity.AddClaim(new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Default.Issuer));

            string sourceToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = securityTokenDescriptor.Audience,
                Expires = securityTokenDescriptor.Expires,
                Issuer = securityTokenDescriptor.Issuer,
                SigningCredentials = securityTokenDescriptor.SigningCredentials,
                Subject = claimsIdentity
            });

            return XmlUtilities.SwapAttributeStatements(sourceToken, destinationToken);
        }

        public string CreateTamperedSignature(SecurityTokenDescriptor securityTokenDescriptor)
        {
            string destinationToken = CreateStringToken(securityTokenDescriptor);

            string sourceToken = CreateStringToken(new SecurityTokenDescriptor
            {
                Audience = securityTokenDescriptor.Audience,
                Expires = securityTokenDescriptor.Expires,
                Issuer = securityTokenDescriptor.Issuer,
                SigningCredentials = securityTokenDescriptor.SigningCredentials,
                Subject = securityTokenDescriptor.Subject
            });

            return XmlUtilities.SwapSignatureValueElements(sourceToken, destinationToken);
        }

        public string CreateWithoutSignature(SecurityTokenDescriptor securityTokenDescriptor)
        {
            // Create the SAML2 destinationToken as a string
            string token = CreateStringToken(securityTokenDescriptor);

            return XmlUtilities.RemoveSignature(token);
        }
    }
}
#nullable restore
