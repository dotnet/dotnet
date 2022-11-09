//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace RuntimeCommon
{
    public static class TokenTestExecutors
    {
        private static Action[] _jsonCreateTokenActions;
        private static Action[] _jwtCreateTokenActions;
        private static Action[] _samlCreateTokenActions;
        private static Action[] _saml2CreateTokenActions;
        private static Action[] _jsonValidateTokenActions;
        private static Action[] _jwtValidateTokenActions;
        private static Action[] _samlValidateTokenActions;
        private static Action[] _saml2ValidateTokenActions;


        /// <summary>
        /// Calls: <see cref="JsonWebTokenHandler.CreateToken(SecurityTokenDescriptor)"/>.
        /// Expects: <see cref="TokenTestData.JsonWebTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string JsonWebTokenHandler_CreateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_jsonCreateTokenActions == null)
            {
                _jsonCreateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    tokenTestData.JsonWebTokenHandler.CreateToken(tokenTestData.SecurityTokenDescriptor);
                }

                for (int i = 0; i < _jsonCreateTokenActions.Length; i++)
                    _jsonCreateTokenActions[i] = action;
            }

            Parallel.Invoke(_jsonCreateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JwtSecurityTokenHandler.ValidateToken"/>.
        /// Expects: <see cref="TokenTestData.JwtSecurityTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string JwtSecurityTokenHandler_CreateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_jwtCreateTokenActions == null)
            {
                _jwtCreateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    tokenTestData.JwtSecurityTokenHandler.CreateEncodedJwt(tokenTestData.SecurityTokenDescriptor);
                }

                for (int i = 0; i < _jsonCreateTokenActions.Length; i++)
                    _jwtCreateTokenActions[i] = action;
            }

            Parallel.Invoke(_jwtCreateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="SamlSecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.SamlSecurityTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string SamlSecurityTokenHandler_CreateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_samlCreateTokenActions == null)
            {
                _samlCreateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    var samlToken = tokenTestData.SamlSecurityTokenHandler.CreateToken(tokenTestData.SecurityTokenDescriptor);
                    tokenTestData.SamlSecurityTokenHandler.WriteToken(samlToken);
                }

                for (int i = 0; i < _samlCreateTokenActions.Length; i++)
                    _samlCreateTokenActions[i] = action;
            }

            Parallel.Invoke(_samlCreateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="Saml2SecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.Saml2SecurityTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string Saml2SecurityTokenHandler_CreateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_saml2CreateTokenActions == null)
            {
                _saml2CreateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    var samlToken = tokenTestData.Saml2SecurityTokenHandler.CreateToken(tokenTestData.SecurityTokenDescriptor);
                    tokenTestData.Saml2SecurityTokenHandler.WriteToken(samlToken);
                }

                for (int i = 0; i < _saml2CreateTokenActions.Length; i++)
                    _saml2CreateTokenActions[i] = action;
            }

            Parallel.Invoke(_saml2CreateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JsonWebTokenHandler.ValidateToken(string, TokenValidationParameters)"/>.
        /// Expects: <see cref="TokenTestData.JsonWebTokenHandler"/>, <see cref="TokenTestData.JwtToken"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string JsonWebTokenHandler_ValidateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;
            if (_jsonValidateTokenActions == null)
            {
                _jsonValidateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    tokenTestData.JsonWebTokenHandler.ValidateToken(tokenTestData.JwtToken, tokenTestData.TokenValidationParameters);
                }

                for (int i = 0; i < _jsonValidateTokenActions.Length; i++)
                    _jsonValidateTokenActions[i] = action;
            }

            Parallel.Invoke(_jsonValidateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JwtSecurityTokenHandler.ValidateToken"/>.
        /// Expects: <see cref="TokenTestData.JwtSecurityTokenHandler"/>, <see cref="TokenTestData.JwtToken"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string JwtSecurityTokenHandler_ValidateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_jwtValidateTokenActions == null)
            {
                _jwtValidateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    tokenTestData.JwtSecurityTokenHandler.ValidateToken(tokenTestData.JwtToken, tokenTestData.TokenValidationParameters, out SecurityToken _);
                }

                for (int i = 0; i < _jwtValidateTokenActions.Length; i++)
                    _jwtValidateTokenActions[i] = action;
            }

            Parallel.Invoke(_jwtValidateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="SamlSecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.SamlSecurityTokenHandler"/>, <see cref="TokenTestData.SamlToken"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string SamlSecurityTokenHandler_ValidateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_samlValidateTokenActions == null)
            {
                _samlValidateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    tokenTestData.SamlSecurityTokenHandler.ValidateToken(tokenTestData.SamlToken, tokenTestData.TokenValidationParameters, out SecurityToken _);
                }

                for (int i = 0; i < _samlValidateTokenActions.Length; i++)
                    _samlValidateTokenActions[i] = action;
            }

            Parallel.Invoke(_samlValidateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="Saml2SecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.Saml2SecurityTokenHandler"/>, <see cref="TokenTestData.Saml2Token"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string Saml2SecurityTokenHandler_ValidateToken_InParallel(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            if (_saml2ValidateTokenActions == null)
            {
                _saml2ValidateTokenActions = new Action[tokenTestData.NumIterations];
                void action()
                {
                    tokenTestData.Saml2SecurityTokenHandler.ValidateToken(tokenTestData.Saml2Token, tokenTestData.TokenValidationParameters, out SecurityToken _);
                }

                for (int i = 0; i < _saml2ValidateTokenActions.Length; i++)
                    _saml2ValidateTokenActions[i] = action;
            }

            Parallel.Invoke(_saml2ValidateTokenActions);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JsonWebTokenHandler.CreateToken(SecurityTokenDescriptor)"/>.
        /// Expects: <see cref="TokenTestData.JsonWebTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string JsonWebTokenHandler_CreateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
                tokenTestData.JsonWebTokenHandler.CreateToken(tokenTestData.SecurityTokenDescriptor);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JwtSecurityTokenHandler.ValidateToken"/>.
        /// Expects: <see cref="TokenTestData.JwtSecurityTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string JwtSecurityTokenHandler_CreateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
                tokenTestData.JwtSecurityTokenHandler.CreateEncodedJwt(tokenTestData.SecurityTokenDescriptor);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="SamlSecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.SamlSecurityTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string SamlSecurityTokenHandler_CreateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
            {
                var samlToken = tokenTestData.SamlSecurityTokenHandler.CreateToken(tokenTestData.SecurityTokenDescriptor);
                tokenTestData.SamlSecurityTokenHandler.WriteToken(samlToken);
            }

            return "";
        }

        /// <summary>
        /// Calls: <see cref="Saml2SecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.Saml2SecurityTokenHandler"/>, <see cref="TokenTestData.SecurityTokenDescriptor"/>.
        /// </summary>
        public static string Saml2SecurityTokenHandler_CreateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
            {
                var samlToken = tokenTestData.Saml2SecurityTokenHandler.CreateToken(tokenTestData.SecurityTokenDescriptor);
                tokenTestData.Saml2SecurityTokenHandler.WriteToken(samlToken);
            }

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JsonWebTokenHandler.ValidateToken(string, TokenValidationParameters)"/>.
        /// Expects: <see cref="TokenTestData.JsonWebTokenHandler"/>, <see cref="TokenTestData.JwtToken"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string JsonWebTokenHandler_ValidateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
                tokenTestData.JsonWebTokenHandler.ValidateToken(tokenTestData.JwtToken, tokenTestData.TokenValidationParameters);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="JwtSecurityTokenHandler.ValidateToken"/>.
        /// Expects: <see cref="TokenTestData.JwtSecurityTokenHandler"/>, <see cref="TokenTestData.JwtToken"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string JwtSecurityTokenHandler_ValidateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
                tokenTestData.JwtSecurityTokenHandler.ValidateToken(tokenTestData.JwtToken, tokenTestData.TokenValidationParameters, out SecurityToken _);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="SamlSecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.SamlSecurityTokenHandler"/>, <see cref="TokenTestData.SamlToken"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string SamlSecurityTokenHandler_ValidateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
                tokenTestData.SamlSecurityTokenHandler.ValidateToken(tokenTestData.SamlToken, tokenTestData.TokenValidationParameters, out SecurityToken _);

            return "";
        }

        /// <summary>
        /// Calls: <see cref="Saml2SecurityTokenHandler.ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>.
        /// Expects: <see cref="TokenTestData.Saml2SecurityTokenHandler"/>, <see cref="TokenTestData.Saml2Token"/>, <see cref="TokenTestData.TokenValidationParameters"/>.
        /// </summary>
        public static string Saml2SecurityTokenHandler_ValidateToken(TestRunData testData)
        {
            var tokenTestData = testData as TokenTestRunData;

            for (int i = 0; i < tokenTestData.NumIterations; i++)
                tokenTestData.Saml2SecurityTokenHandler.ValidateToken(tokenTestData.Saml2Token, tokenTestData.TokenValidationParameters, out SecurityToken _);

            return "";
        }
    }
}
