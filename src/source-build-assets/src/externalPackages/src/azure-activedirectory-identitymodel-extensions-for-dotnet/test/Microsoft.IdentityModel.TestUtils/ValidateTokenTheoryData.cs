// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    public class ValidateTokenTheoryData : TheoryDataBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ValidateTokenTheoryData(string testId) : base(testId) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public ValidateTokenTheoryData(TokenHandler tokenHandler, string testId) : base(testId)
        {
            TokenHandler = tokenHandler;
        }

        internal bool ExpectedIsValid { get; set; } = true;

        internal ExpectedException ExpectedExceptionValidationParameters { get; set; } = ExpectedException.NoExceptionExpected;

        /// <summary>
        /// The TokenValidation results for the exception vary between JWT and SAML tokens.
        /// This property is used to restrict the exception thrown for TVP validation errors.
        /// For the updated model using ValidationParameters, all tokens will provide the same exception for validation errors.
        /// </summary>
        public bool RelaxTVPException { get; set; } = false;

        public SecurityTokenDescriptor? SecurityTokenDescriptor { get; set; }

        public string Token { get; set; } = string.Empty;

        public TokenHandler TokenHandler { get; set; }

        internal ITestingTokenHandler? TestingTokenHandler { get; set; }

        internal TokenValidationParameters? TokenValidationParameters { get; set; }

        internal ValidationParameters? ValidationParameters { get; set; }
    }
}
