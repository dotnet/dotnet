// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

//using System.Security.Claims;
//using System;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils.TokenValidationExtensibility.Tests
{
    public class ExtensibilityTheoryData : TheoryDataBase
    {
        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken) : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters = new ValidationParameters
            {
                AlgorithmValidator = SkipValidationDelegates.SkipAlgorithmValidation,
                AudienceValidator = SkipValidationDelegates.SkipAudienceValidation,
                SignatureKeyValidator = SkipValidationDelegates.SkipIssuerSigningKeyValidation,
                IssuerValidatorAsync = SkipValidationDelegates.SkipIssuerValidation,
                LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation,
                //SignatureValidator = SkipValidationDelegates.SkipSignatureValidation,
                TokenReplayValidator = SkipValidationDelegates.SkipTokenReplayValidation,
                TokenTypeValidator = SkipValidationDelegates.SkipTokenTypeValidation
            };

            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
        }

        internal static ITestingTokenHandler CreateSecurityTokenHandlerForType(string tokenHandlerType)
        {
            return tokenHandlerType switch
            {
                "JWT" => new JsonWebTestingTokenHandler(),
                "SAML" => new SamlSecurityTestingTokenHandler(),
                "SAML2" => new Saml2SecurityTestingTokenHandler(),
                _ => throw new NotImplementedException(tokenHandlerType)
            };
        }

        public SecurityToken SecurityToken
        {
            get;
            set;
        }

        internal TokenHandler TokenHandler { get; }

        internal ValidationParameters ValidationParameters { get; }

        internal ValidationError? ValidationError { get; set; }

        internal ExpectedException? ExpectedInnerException { get; set; }
    }
}
#nullable restore
