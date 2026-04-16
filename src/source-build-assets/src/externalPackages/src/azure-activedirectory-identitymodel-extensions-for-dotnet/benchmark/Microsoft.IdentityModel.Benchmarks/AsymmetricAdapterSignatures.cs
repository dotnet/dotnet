// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET6_0_OR_GREATER

using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.AsymmetricAdapterSignatures.*

    public class AsymmetricAdapterSignatures
    {
        private byte[] _bytesToSign;
        private byte[] _signatureBuffer;
        private AsymmetricAdapter _rsaAsymmetricAdapter;

        [GlobalSetup]
        public void Setup()
        {
            SecurityTokenDescriptor securityTokenDescriptor = new ()
            {
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
                Claims = BenchmarkUtils.Claims,
                TokenType = JwtHeaderParameterNames.Jwk
            };

            _bytesToSign = Encoding.UTF8.GetBytes((new JsonWebTokenHandler()).CreateToken(securityTokenDescriptor));
           _rsaAsymmetricAdapter = new AsymmetricAdapter(
               BenchmarkUtils.SigningCredentialsRsaSha256.Key,
               SecurityAlgorithms.RsaSha256,
               SHA256.Create(),
               SupportedAlgorithms.GetHashAlgorithmName(SecurityAlgorithms.RsaSha256),
               true );

            _signatureBuffer = new byte[256];
        }

        /// <summary>
        /// In this case, .NET creates a buffer to hold the signature.
        /// ArrayPool is not used, because the buffer is created by the framework and not the user.
        /// The buffer is not returned to the pool, and must be garbage collected.
        /// </summary>
        [Benchmark]
        public void SignDotnetCreatingBufferRSA()
        {
            _rsaAsymmetricAdapter.Sign(_bytesToSign);
        }

        /// <summary>
        /// In this case, the user obtains a buffer to hold the signature from the array pool.
        /// A new api available in .NET 5.0+ is used to provide the buffer to place the signature.
        /// The size of the bytes written is returned in the out parameter, size.
        /// </summary>
        [Benchmark]
        public void SignSpanWithArrayPoolRSA()
        {
            byte[] signature = ArrayPool<byte>.Shared.Rent(256);
            _rsaAsymmetricAdapter.SignUsingSpan(_bytesToSign, signature.AsSpan(), out int size);
            ArrayPool<byte>.Shared.Return(signature);
        }

        /// <summary>
        /// In this case, the user has created a SINGLE global buffer to hold the signature.
        /// This is not a recommended approach, because the buffer will be reused and signatures will get mixed up.
        /// Is used to illustrate that using the array pool is cheap.
        /// Uses a new api available in .NET 5.0 + to provide the buffer to place the signature.
        /// The size of the bytes written is returned in the out parameter, size.
        /// </summary>
        [Benchmark]
        public void SignSpanWithFixedBufferRSA()
        {
            _rsaAsymmetricAdapter.SignUsingSpan(_bytesToSign, _signatureBuffer, out int size);
        }
    }
}
#endif
