// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Newtonsoft.Json;
using System.Text.Json;
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

        public static TheoryData<JwtTheoryData> InvalidNumberOfSegmentsData(IList<string> errorStrings, TheoryData<JwtTheoryData> theoryData)
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
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[0]),
                TestId = "a",
                Token = "a"
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[0]),
                TestId = "a.",
                Token = "a."
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[1]),
                TestId = "a.b",
                Token = "a.b"
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[2]),
                TestId = "a.b.c.",
                Token = "a.b.c."
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[3]),
                TestId = "a.b.c.d",
                Token = "a.b.c.d"
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[4]),
                TestId = "a.b.c.d.",
                Token = "a.b.c.d."
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorStrings[5]),
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
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: second position'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir)'",
                Token = validRegEx + ".." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12740:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir, Cipher text missing)'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir, Cipher text missing)'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (four parts)'",
                Token = validRegEx + "." + invalidRegEx + ".",
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir, Cipher text missing)'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12739:")
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'Encoding == SignedEncodedJwts.Asymmetric_LocalSts'",
                Token = "SignedEncodedJwts.Asymmetric_LocalSts",
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException("IDX12741:")
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
                TestId = "'invalidRegEx: first position'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: second position'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position'",
                Token = validRegEx + "." + validRegEx + "." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir)'",
                Token = validRegEx + ".." + invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fourth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir)'",
                Token = invalidRegEx + ".." + validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: first position (dir, Cipher text missing)'",
                Token = invalidRegEx + "." + validRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (dir, Cipher text missing)'",
                Token = validRegEx + "." + invalidRegEx + "." + validRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: third position (four parts)'",
                Token = validRegEx + "." + invalidRegEx + ".",
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'invalidRegEx: fifth position (dir, Cipher text missing)'",
                Token = validRegEx + "." + validRegEx + "." + invalidRegEx,
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
            });
            theoryData.Add(new JwtTheoryData
            {
                CanRead = false,
                TestId = "'Encoding == SignedEncodedJwts.Asymmetric_LocalSts'",
                Token = "SignedEncodedJwts.Asymmetric_LocalSts",
                ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(errorString)
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
                ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX12723:", null, true)
            });

            return theoryData;
        }
    }
}
