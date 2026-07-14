// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Extensibility.Tests
{
    public class ExtensibilityTheoryData : TheoryDataBase
    {
        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            AlgorithmValidationDelegate algorithmDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.AlgorithmValidator = algorithmDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomAlgorithmValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(AlgorithmValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            AudienceValidationDelegate audienceValidationDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.AudienceValidator = audienceValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomAudienceValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(AudienceValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            IssuerValidationDelegateAsync issuerValidationDelegate)
    : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.IssuerValidatorAsync = issuerValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomIssuerValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(IssuerValidationError), new List<string> { "StackFramesCount" });
        }

        // TODO is there a IssuerSigningKeyResolver Error?
        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            SignatureKeyResolverDelegate signtureKeyResolverDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.SignatureKeyResolver = signtureKeyResolverDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomIssuerSigningKeyValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(SignatureKeyValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            SignatureKeyValidationDelegate issuerSigningKeyValidationDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.SignatureKeyValidator = issuerSigningKeyValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomIssuerSigningKeyValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(SignatureKeyValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            LifetimeValidationDelegate lifetimeValidationDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.LifetimeValidator = lifetimeValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomLifetimeValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(LifetimeValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            SignatureValidationDelegate signatureValidationDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.SignatureValidator = signatureValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomSignatureValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(SignatureValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            TokenReplayValidationDelegate tokenReplayValidationDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.TokenReplayValidator = tokenReplayValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomTokenReplayValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(TokenReplayValidationError), new List<string> { "StackFramesCount" });
        }

        internal ExtensibilityTheoryData(
            string testId,
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            TokenTypeValidationDelegate tokenTypeValidationDelegate)
            : base(testId)
        {
            TokenHandler = tokenHandler;
            SecurityToken = securityToken;
            ValidationParameters.TokenTypeValidator = tokenTypeValidationDelegate;
            ValidationParameters.SigningKeys.Add(Default.SigningCredentials.Key);
            PropertiesToIgnoreWhenComparing.Add(typeof(CustomTokenTypeValidationError), new List<string> { "StackFramesCount" });
            PropertiesToIgnoreWhenComparing.Add(typeof(TokenTypeValidationError), new List<string> { "StackFramesCount" });
        }

        public SecurityToken SecurityToken
        {
            get;
            set;
        }

        internal TokenHandler TokenHandler { get; }

        internal ValidationParameters ValidationParameters { get; } = new ValidationParameters
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

        internal ValidationError? ValidationError { get; set; }

        internal ExpectedException? ExpectedInnerException { get; set; }
    }
}
#nullable restore
