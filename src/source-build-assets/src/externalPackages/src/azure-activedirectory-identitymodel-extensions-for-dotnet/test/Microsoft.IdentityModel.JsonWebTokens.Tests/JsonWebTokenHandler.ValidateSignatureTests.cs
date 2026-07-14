// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt.Tests;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerValidateSignatureTests
    {
        [Theory, MemberData(nameof(JsonWebTokenHandlerValidateSignatureTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateSignature(JsonWebTokenHandlerValidateSignatureTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidateSignature", theoryData);
            JsonWebToken jsonWebToken;
            if (theoryData.JWT == null && theoryData.SigningCredentials != null)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = theoryData.SigningCredentials,
                };
                var tokenHandler = new JsonWebTokenHandler();
                jsonWebToken = new JsonWebToken(tokenHandler.CreateToken(tokenDescriptor));
            }
            else
                jsonWebToken = theoryData.JWT;


            if (theoryData.Configuration is not null && theoryData.KeyToAddToConfiguration is not null)
                theoryData.Configuration.SigningKeys.Add(theoryData.KeyToAddToConfiguration);

            if (theoryData.ValidationParameters is not null && theoryData.KeyToAddToValidationParameters is not null)
                theoryData.ValidationParameters.SigningKeys.Add(theoryData.KeyToAddToValidationParameters);

            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            ValidationResult<SecurityKey, ValidationError> validationResult = jsonWebTokenHandler.ValidateSignature(
                jsonWebToken,
                theoryData.ValidationParameters,
                theoryData.Configuration,
                theoryData.CallContext);

            if (validationResult.Succeeded)
            {
                IdentityComparer.AreSecurityKeysEqual(
                    validationResult.Result,
                    theoryData.OperationResult.Result,
                    context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            else
            {
                ValidationError validationError = validationResult.Error;
                IdentityComparer.AreStringsEqual(
                    validationError.FailureType.Name,
                    theoryData.OperationResult.Error.FailureType.Name,
                    context);

                Exception exception = validationError.GetException();
                theoryData.ExpectedException.ProcessException(exception, context);

                if (jsonWebToken is not null)
                    Assert.Null(jsonWebToken.SigningKey);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebTokenHandlerValidateSignatureTheoryData> JsonWebTokenHandlerValidateSignatureTestCases
        {
            get
            {
                var unsignedToken = new JsonWebToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.");
                return new TheoryData<JsonWebTokenHandlerValidateSignatureTheoryData>
                {
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Invalid_Null_JWT")
                    {
                        JWT = null,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10000,
                                "jwtToken"),
                            ValidationFailureType.NullArgument,
                            null)
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Invalid_Null_ValidationParameters")
                    {
                        JWT = new JsonWebToken(EncodedJwts.LiveJwt),
                        ValidationParameters = null,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10000,
                                "validationParameters"),
                            ValidationFailureType.NullArgument,
                            null)
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Invalid_DelegateReturnsFailure")
                    {
                        JWT = new JsonWebToken(EncodedJwts.LiveJwt),
                        ValidationParameters = new ValidationParameters
                        {
                            SignatureValidator = (token, parameters, configuration, callContext) => new SignatureValidationError(
                                new MessageDetail("IDX10000: NullArgument", null),
                                ValidationFailureType.NullArgument,
                                ValidationError.GetCurrentStackFrame())
                        },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10000,
                                "NullArgument"),
                            ValidationFailureType.NullArgument,
                            null)
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Invalid_NoSignature")
                    {
                        JWT = unsignedToken,
                        ValidationParameters = new ValidationParameters(),
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10504:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10504,
                                LogHelper.MarkAsSecurityArtifact(unsignedToken, JwtTokenUtilities.SafeLogJwtToken)),
                            SignatureValidationFailure.TokenIsNotSigned,
                            null)
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Valid_DelegateReturnsSuccess")
                    {
                        JWT = new JsonWebToken(EncodedJwts.LiveJwt),
                        ValidationParameters = new ValidationParameters
                        {
                            SignatureValidator = (token, parameters, configuration, callContext) => KeyingMaterial.JsonWebKeyRsa256PublicSigningCredentials.Key
                        },
                        OperationResult = KeyingMaterial.JsonWebKeyRsa256PublicSigningCredentials.Key
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Valid_SignatureValidationResult_Success_KidMatches")
                    {
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        ValidationParameters = new ValidationParameters(),
                        KeyToAddToValidationParameters = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        OperationResult = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Valid_SignatureValidationResult_Success_X5tMatches")
                    {
                        SigningCredentials = KeyingMaterial.X509SigningCreds_1024_RsaSha2_Sha2,
                        ValidationParameters = new ValidationParameters(),
                        KeyToAddToValidationParameters = KeyingMaterial.X509SigningCreds_1024_RsaSha2_Sha2.Key,
                        OperationResult = KeyingMaterial.X509SigningCreds_1024_RsaSha2_Sha2.Key,
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Valid_IssuerSigningKeyResolverReturnsKeyThatMatches")
                    {
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        ValidationParameters = new ValidationParameters
                        {
                            SignatureKeyResolver = (token, securityToken, kid, validationParameters, configuration, callContext) => KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key
                        },
                        OperationResult = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Valid_ConfurationReturnsKeyThatMatches")
                    {
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        Configuration = new OpenIdConnectConfiguration(),
                        KeyToAddToConfiguration = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        ValidationParameters = new ValidationParameters(),
                        OperationResult = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Valid_NoKeyId_TryAllKeys")
                    {
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2_NoKeyId,
                        ValidationParameters = new ValidationParameters
                        {
                            TryAllSigningKeys = true
                        },
                        KeyToAddToValidationParameters = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2_NoKeyId.Key,
                        OperationResult = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2_NoKeyId.Key,
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Invalid_NoKeyId_DontTryAllKeys")
                    {
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2_NoKeyId,
                        ValidationParameters = new ValidationParameters(),
                        KeyToAddToValidationParameters = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2_NoKeyId.Key,
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10526:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(TokenLogMessages.IDX10526),
                            SignatureValidationFailure.SigningKeyNotFound,
                            null)
                    },
                    new JsonWebTokenHandlerValidateSignatureTheoryData("Invalid_NoKeys")
                    {
                        JWT = new JsonWebToken(EncodedJwts.LiveJwt),
                        ValidationParameters = new ValidationParameters(),
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10527:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(TokenLogMessages.IDX10500),
                            SignatureValidationFailure.SigningKeyNotFound,
                            null)
                    }
                };
            }
        }
    }

    public class JsonWebTokenHandlerValidateSignatureTheoryData : TheoryDataBase
    {
        public JsonWebTokenHandlerValidateSignatureTheoryData(string testId) : base(testId) { }
        public JsonWebToken JWT { get; set; }
        public BaseConfiguration Configuration { get; set; }
        public SigningCredentials SigningCredentials { get; internal set; }
        public SecurityKey KeyToAddToConfiguration { get; internal set; }
        public SecurityKey KeyToAddToValidationParameters { get; internal set; }
        internal ValidationResult<SecurityKey, ValidationError> OperationResult { get; set; }
        internal ValidationParameters ValidationParameters { get; set; }
    }
}
