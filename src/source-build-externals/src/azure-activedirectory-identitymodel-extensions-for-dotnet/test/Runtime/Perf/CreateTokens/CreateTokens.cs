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
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using RuntimeCommon;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace RuntimeTests
{
    public class CreateTokens
    {
        public static void Run(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;
            var testRuns = TestConfig.SetupTestRuns(
                new List<TestExecutor>
                {
                    TokenTestExecutors.JsonWebTokenHandler_CreateToken,
                    TokenTestExecutors.JwtSecurityTokenHandler_CreateToken,
                    TokenTestExecutors.Saml2SecurityTokenHandler_CreateToken,
                    TokenTestExecutors.SamlSecurityTokenHandler_CreateToken,
                });

            var testConfig = TestConfig.ParseArgs(args);
            var tokenTestData = new TokenTestRunData
            {
                JwtSecurityTokenHandler = new JwtSecurityTokenHandler(),
                JsonWebTokenHandler = new JsonWebTokenHandler(),
                NumIterations = testConfig.NumIterations,
                SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
                SecurityTokenDescriptor = TestData.SecurityTokenDescriptor(TestData.RsaSigningCredentials_2048Sha256)
            };

            // run each test to set any static data
            foreach(var testRun in testRuns)
                testRun.TestExecutor(tokenTestData);

            var assemblyVersion = typeof(JwtSecurityTokenHandler).Assembly.GetName().Version.ToString();
#if DEBUG
            var prefix = "DEBUG";
#else
            var prefix = "RELEASE";
#endif
            testConfig.Version = $"{prefix}-{assemblyVersion}";
            var logName = $"CreateTokens-{testConfig.Version}_{DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss")}.txt";
            var directory = testConfig.LogDirectory;
            var logFile = Path.Combine(directory, logName);
            Directory.CreateDirectory(directory);

            TestRunner.Run(testConfig, testRuns, tokenTestData);
            File.WriteAllText(logFile, testConfig.Logger.Logs);
        }
    }
}
