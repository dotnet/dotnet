// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET

using System;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.JsonWebKeyComparison.*

    /// <summary>
    /// The purpose of this benchmark is to compare the performance of different methods to compare JsonWebKeys and RSASecurityKeys.
    /// JsonWebKeys have a base64url encoded exponent and modulus, while RSASecurityKeys have base64 encoded exponent and modulus.
    /// </summary>
    public class JsonWebKeyComparison
    {
        private RsaSecurityKey _rsaSecurityKey;
        private JsonWebKey _jsonWebKey;
        private string _base64EncodedE;
        private string _base64EncodedN;
        private string _base64UrlEncodedE;
        private string _base64UrlEncodedN;

        [GlobalSetup]
        public void Setup()
        {
            _rsaSecurityKey = BenchmarkUtils.RsaSecurityKey;
            _base64EncodedE = Convert.ToBase64String(_rsaSecurityKey.Parameters.Exponent);
            _base64EncodedN = Convert.ToBase64String(_rsaSecurityKey.Parameters.Modulus);
            _base64UrlEncodedE = Base64UrlEncoder.Encode(_rsaSecurityKey.Parameters.Exponent);
            _base64UrlEncodedN = Base64UrlEncoder.Encode(_rsaSecurityKey.Parameters.Modulus);
            _jsonWebKey = new JsonWebKey { E = _base64UrlEncodedE, N = _base64UrlEncodedN };
        }

        [Benchmark]
        public bool Base64UrlEncoderDecodeBytes()
        {
            return _base64EncodedE.Equals(Convert.ToBase64String(Base64UrlEncoder.DecodeBytes(_jsonWebKey.E)))
                && _base64EncodedN.Equals(Convert.ToBase64String(Base64UrlEncoder.DecodeBytes(_jsonWebKey.N)));
        }

        [Benchmark]
        public bool UtilityAreEqual()
        {
            return Utility.AreEqual(Convert.FromBase64String(_base64EncodedE), Base64UrlEncoder.DecodeBytes(_jsonWebKey.E))
                && Utility.AreEqual(Convert.FromBase64String(_base64EncodedN), Base64UrlEncoder.DecodeBytes(_jsonWebKey.N));
        }

        [Benchmark]
        public bool Base64EncodedEquals()
        {
            return _base64EncodedE.Equals(Convert.ToBase64String(Base64UrlEncoder.DecodeBytes(_jsonWebKey.E)))
                && _base64EncodedN.Equals(Convert.ToBase64String(Base64UrlEncoder.DecodeBytes(_jsonWebKey.N)));
        }

        [Benchmark]
        public bool Base64UrlEncodedEquals()
        {
            return _base64UrlEncodedE.Equals(Base64UrlEncoder.Encode(Convert.FromBase64String(_base64EncodedE)))
                && _base64UrlEncodedN.Equals(Base64UrlEncoder.Encode(Convert.FromBase64String(_base64EncodedN)));
        }
    }
}
#endif
