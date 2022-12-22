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

using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    /// <summary>
    /// Contains theory data shared between different tests
    /// </summary>
    public static class JwtTestData
    {
        public static TheoryData<JwtTheoryData> ValidEncodedSegmentsData(TheoryData<JwtTheoryData> theoryData)
        {
            string[] tokenParts = EncodedJwts.Asymmetric_LocalSts.Split('.');
            theoryData.Add(new JwtTheoryData
            {
                TestId = nameof(EncodedJwts.OverClaims),
                Token = EncodedJwts.OverClaims
            });
            theoryData.Add(new JwtTheoryData
            {
                TestId = "'EncodedJwts.Asymmetric_LocalSts.Split, two parts'",
                Token = string.Format("{0}.{1}.", tokenParts[0], tokenParts[1]),
            });
            theoryData.Add(new JwtTheoryData
            {
                TestId = nameof(EncodedJwts.Asymmetric_LocalSts),
                Token = EncodedJwts.Asymmetric_LocalSts,
            });

            return theoryData;
        }

        public static TheoryData<JwtTheoryData> InvalidNumberOfSegmentsData(string errorString, TheoryData<JwtTheoryData> theoryData)
        {
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.ArgumentNullException(),
                TestId = "null"
            });

            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.ArgumentNullException(),
                TestId = "emptystring",
                Token = ""
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.ArgumentException(errorString),
                TestId = "a",
                Token = "a"
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.ArgumentException(errorString),
                TestId = "a.b",
                Token = "a.b"
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.ArgumentException(errorString),
                TestId = "a.b.c.d",
                Token = "a.b.c.d"
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.ArgumentException(errorString),
                TestId = "a.b.c.d.e.f",
                Token = "a.b.c.d.e.f"
            });

            return theoryData;
        }

        public static TheoryData<JwtTheoryData> InvalidRegExSegmentsData(TheoryData<JwtTheoryData> theoryData)
        {
            var validRegEx = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9";
            var invalidRegEx = "eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1Z CI6Imh0";
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: second position'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir)'",
                Token = validRegEx + ".." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir, Cipher text missing)'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir, Cipher text missing)'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (four parts)'",
                Token = validRegEx + "." + invalidRegEx + ".",
                ExpectedException = ExpectedException.ArgumentException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir, Cipher text missing)'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.ArgumentException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'Encoding == SignedEncodedJwts.Asymmetric_LocalSts'",
                Token = "SignedEncodedJwts.Asymmetric_LocalSts",
                ExpectedException = ExpectedException.ArgumentException("IDX12741:")
            });

            return theoryData;
        }

        public static TheoryData<JwtTheoryData> InvalidRegExSegmentsDataForReadToken(string errorString, TheoryData<JwtTheoryData> theoryData)
        {
            var validRegEx = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9";
            var invalidRegEx = "eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1Z CI6Imh0";
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId =  "'invalidRegEx: first position'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: second position'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir)'",
                Token = validRegEx + ".." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir, Cipher text missing)'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir, Cipher text missing)'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (four parts)'",
                Token = validRegEx + "." + invalidRegEx + ".",
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir, Cipher text missing)'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'Encoding == SignedEncodedJwts.Asymmetric_LocalSts'",
                Token = "SignedEncodedJwts.Asymmetric_LocalSts",
                ExpectedException = ExpectedException.ArgumentException(errorString)
            });

            return theoryData;
        }

        public static TheoryData<JwtTheoryData> InvalidEncodedSegmentsData(string errorString, TheoryData<JwtTheoryData> theoryData)
        {
            theoryData.Add(new JwtTheoryData
            {
                CanRead = true,
                TestId = nameof(EncodedJwts.InvalidPayload),
                Token = EncodedJwts.InvalidPayload,
                ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX12723:", inner: typeof(JsonReaderException))
            });

            return theoryData;
        }
    }
}
