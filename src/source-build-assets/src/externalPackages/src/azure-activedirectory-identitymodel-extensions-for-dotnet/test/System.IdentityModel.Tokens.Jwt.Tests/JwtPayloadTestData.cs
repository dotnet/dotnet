// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtPayloadTestData
    {
        static JwtPayloadTestData()
        {
            // an object for each first class property
            ObjectForEachProperty = new Dictionary<string, object>
            {
                {JwtRegisteredClaimNames.Acr, Default.Acr},
                {JwtRegisteredClaimNames.Actort, Default.AsymmetricJwt},
                {JwtRegisteredClaimNames.Amr, Default.Amr},
                {JwtRegisteredClaimNames.AuthTime, EpochTime.GetIntDate(DateTime.UtcNow)},
                {JwtRegisteredClaimNames.Aud, Default.Audience},
                {JwtRegisteredClaimNames.Azp, Default.AuthorizedParty},
                {JwtRegisteredClaimNames.CHash, Guid.NewGuid().ToString()},
                {JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.UtcNow + TimeSpan.FromHours(1))},
                {JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()},
                {JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow + TimeSpan.FromSeconds(1))},
                {JwtRegisteredClaimNames.Iss, Default.Issuer},
                {JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.UtcNow - TimeSpan.FromHours(1))},
                {JwtRegisteredClaimNames.Nonce, Guid.NewGuid().ToString()},
                {JwtRegisteredClaimNames.Sub, Default.Subject},
            };

            // a claim for each first class property
            ClaimForEachProperty = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Actort, ObjectForEachProperty[JwtRegisteredClaimNames.Actort] as string),
                new Claim(JwtRegisteredClaimNames.Acr, ObjectForEachProperty[JwtRegisteredClaimNames.Acr] as string),
                new Claim(JwtRegisteredClaimNames.Amr, ObjectForEachProperty[JwtRegisteredClaimNames.Amr] as string),
                new Claim(JwtRegisteredClaimNames.AuthTime, ObjectForEachProperty[JwtRegisteredClaimNames.AuthTime].ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Aud, ObjectForEachProperty[JwtRegisteredClaimNames.Aud] as string),
                new Claim(JwtRegisteredClaimNames.Azp, ObjectForEachProperty[JwtRegisteredClaimNames.Azp] as string),
                new Claim(JwtRegisteredClaimNames.CHash, ObjectForEachProperty[JwtRegisteredClaimNames.CHash] as string),
                new Claim(JwtRegisteredClaimNames.Exp, ObjectForEachProperty[JwtRegisteredClaimNames.Exp].ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Jti, ObjectForEachProperty[JwtRegisteredClaimNames.Jti] as string),
                new Claim(JwtRegisteredClaimNames.Iat, ObjectForEachProperty[JwtRegisteredClaimNames.Iat].ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, ObjectForEachProperty[JwtRegisteredClaimNames.Iss] as string),
                new Claim(JwtRegisteredClaimNames.Nbf, ObjectForEachProperty[JwtRegisteredClaimNames.Nbf].ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Nonce, ObjectForEachProperty[JwtRegisteredClaimNames.Nonce] as string),
                new Claim(JwtRegisteredClaimNames.Sub, ObjectForEachProperty[JwtRegisteredClaimNames.Sub] as string),
            };

            Multiples = new KeyValuePair<List<Claim>, Dictionary<string, object>>(new List<Claim>(), new Dictionary<string, object>());
            Multiples.Value[JwtRegisteredClaimNames.Aud] = new List<object>();
            foreach (var aud in Default.Audiences)
            {
                Multiples.Key.Add(new Claim(JwtRegisteredClaimNames.Aud, aud));
                (Multiples.Value[JwtRegisteredClaimNames.Aud] as List<object>).Add(aud);
            }

            Multiples.Value[JwtRegisteredClaimNames.Amr] = new List<object>();
            foreach (var amr in Default.Amrs)
            {
                Multiples.Key.Add(new Claim(JwtRegisteredClaimNames.Amr, amr));
                (Multiples.Value[JwtRegisteredClaimNames.Amr] as List<object>).Add(amr);
            }
        }

        public static List<Claim> ClaimForEachProperty
        {
            get;
            private set;
        }

        public static KeyValuePair<List<Claim>, Dictionary<string, object>> Multiples
        {
            get;
            private set;
        }

        public static Dictionary<string, object> ObjectForEachProperty
        {
            get;
            private set;
        }
    }
}
