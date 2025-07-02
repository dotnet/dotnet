// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtSecurityTokenHandlerExtensibilityTests
    {
        [Theory, MemberData(nameof(DecryptTokenTheoryData))]
        public void DecryptToken(TheoryParams theoryParams)
        {
            try
            {
                theoryParams.TokenHandler.DecryptTokenPublic(theoryParams.Token, theoryParams.ValidationParameters);
            }
            catch (Exception ex)
            {
                theoryParams.EE.ProcessException(ex);
            }
        }

        public static TheoryData<TheoryParams> DecryptTokenTheoryData()
        {
            var theoryData = new TheoryData<TheoryParams>();

            var tokenHandler = new PublicJwtSecurityTokenHandler();

            // Parameter validation
            theoryData.Add(new TheoryParams("Test1", null, new TokenValidationParameters(), tokenHandler, ExpectedException.ArgumentNullException()));
            theoryData.Add(new TheoryParams("Test2", new JwtSecurityToken(), null, tokenHandler, ExpectedException.ArgumentNullException()));
            theoryData.Add(new TheoryParams("Test3", new JwtSecurityToken(), new TokenValidationParameters(), tokenHandler, ExpectedException.SecurityTokenException()));

            // Enc empty
            var header = new JwtHeader();
            header[JwtHeaderParameterNames.Enc] = "";
            theoryData.Add(new TheoryParams("Test4", new JwtSecurityToken(), new TokenValidationParameters(), tokenHandler, ExpectedException.SecurityTokenException()));

            // Alg empty
            header = new JwtHeader();
            header[JwtHeaderParameterNames.Enc] = SecurityAlgorithms.Aes128CbcHmacSha256;
            theoryData.Add(new TheoryParams("Test5", new JwtSecurityToken(), new TokenValidationParameters(), tokenHandler, ExpectedException.SecurityTokenException()));

            // Alg not supproted
            header = new JwtHeader();
            header[JwtHeaderParameterNames.Alg] = SecurityAlgorithms.Aes128KW;
            theoryData.Add(new TheoryParams("Test6", new JwtSecurityToken(), new TokenValidationParameters(), tokenHandler, ExpectedException.SecurityTokenException()));

            return theoryData;
        }

        public class TheoryParams
        {
            public TheoryParams(string testId, JwtSecurityToken token, TokenValidationParameters validationParamters, PublicJwtSecurityTokenHandler tokenHandler, ExpectedException ee)
            {
                TestId = testId;
                Token = token;
                TokenHandler = tokenHandler;
                ValidationParameters = validationParamters;
                EE = ee;
            }

            public string TestId { get; set; }
            public JwtSecurityToken Token { get; set; }
            public PublicJwtSecurityTokenHandler TokenHandler { get; set; }
            public TokenValidationParameters ValidationParameters { get; set; }
            public ExpectedException EE { get; set; }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
