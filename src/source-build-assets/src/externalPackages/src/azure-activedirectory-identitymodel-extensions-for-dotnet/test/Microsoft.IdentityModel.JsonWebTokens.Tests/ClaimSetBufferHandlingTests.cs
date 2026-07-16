// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    /// <summary>
    /// Exercises the buffer handling path used by JsonWebToken claim-set decoding,
    /// covering both the stack-allocated and pooled buffer code paths.
    /// </summary>
    public class ClaimSetBufferHandlingTests
    {
        /// <summary>
        /// A payload larger than the stackalloc threshold uses the pooled buffer path.
        /// All decoded claim values must round-trip exactly.
        /// </summary>
        [Fact]
        public void LargePayload_PooledBufferPath_ClaimsRoundTrip()
        {
            var claims = new Dictionary<string, object>
            {
                ["sub"] = "user@example.com",
                ["name"] = "A deliberately long claim value used to exercise the pooled buffer path",
                ["role"] = "administrator",
                ["department"] = "Engineering - additional padding to grow the encoded payload",
                ["description"] = new string('X', 200)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('k', 128)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = creds,
                Claims = claims,
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
            };

            var handler = new JsonWebTokenHandler();
            string tokenString = handler.CreateToken(descriptor);

            var jwt = new JsonWebToken(tokenString);

            Assert.Equal("user@example.com", jwt.Claims.First(c => c.Type == "sub").Value);
            Assert.Equal("A deliberately long claim value used to exercise the pooled buffer path",
                jwt.Claims.First(c => c.Type == "name").Value);
            Assert.Equal("administrator", jwt.Claims.First(c => c.Type == "role").Value);
            Assert.Equal("Engineering - additional padding to grow the encoded payload",
                jwt.Claims.First(c => c.Type == "department").Value);
            Assert.Equal(new string('X', 200),
                jwt.Claims.First(c => c.Type == "description").Value);
        }

        /// <summary>
        /// A small payload takes the stack-allocated path.
        /// Regression check: parsing must still produce the expected claim values.
        /// </summary>
        [Fact]
        public void SmallPayload_StackallocPath_ClaimsRoundTrip()
        {
            var claims = new Dictionary<string, object>
            {
                ["sub"] = "alice",
                ["role"] = "user"
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('k', 128)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = creds,
                Claims = claims,
                Issuer = "https://issuer.example.com",
                Audience = "https://audience.example.com",
            };

            var handler = new JsonWebTokenHandler();
            string tokenString = handler.CreateToken(descriptor);

            var jwt = new JsonWebToken(tokenString);

            Assert.Equal("alice", jwt.Claims.First(c => c.Type == "sub").Value);
            Assert.Equal("user", jwt.Claims.First(c => c.Type == "role").Value);
            Assert.Equal("https://issuer.example.com", jwt.Issuer);
            Assert.Equal("https://audience.example.com",
                jwt.Claims.First(c => c.Type == "aud").Value);
        }

        /// <summary>
        /// Repeatedly parses tokens that take the pooled buffer path to confirm
        /// that consecutive uses of pooled buffers do not affect parsing results.
        /// </summary>
        [Fact]
        public void RepeatedParsing_PooledBufferPath_ProducesConsistentResults()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('k', 128)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var handler = new JsonWebTokenHandler();

            for (int i = 0; i < 20; i++)
            {
                string uniqueValue = $"iteration-{i}-" + new string((char)('A' + (i % 26)), 200);

                var descriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = creds,
                    Claims = new Dictionary<string, object>
                    {
                        ["seq"] = i.ToString(),
                        ["payload"] = uniqueValue
                    },
                    Issuer = "https://issuer.example.com",
                };

                string tokenString = handler.CreateToken(descriptor);
                var jwt = new JsonWebToken(tokenString);

                Assert.Equal(i.ToString(), jwt.Claims.First(c => c.Type == "seq").Value);
                Assert.Equal(uniqueValue, jwt.Claims.First(c => c.Type == "payload").Value);
            }
        }
    }
}
