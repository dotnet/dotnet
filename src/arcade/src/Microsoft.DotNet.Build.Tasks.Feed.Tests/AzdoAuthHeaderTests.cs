// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class AzdoAuthHeaderTests
    {
        private static string Basic(string token) =>
            Convert.ToBase64String(Encoding.ASCII.GetBytes($":{token}"));

        [Theory]
        // JWTs (three non-empty dot-separated segments) => Bearer, token passed through verbatim.
        [InlineData("aaa.bbb.ccc")]
        [InlineData("header.payload.signature")]
        public void JwtTokensUseBearer(string token)
        {
            var header = GeneralUtils.CreateAzdoAuthHeader(token);

            header.Scheme.Should().Be("Bearer");
            header.Parameter.Should().Be(token);
        }

        [Theory]
        // Opaque PATs and malformed values => Basic auth with an empty username.
        [InlineData("opaquepattokenvalue")]        // no dots => PAT
        [InlineData("a.b")]                         // two segments => PAT
        [InlineData("a.b.c.d")]                     // four segments => PAT
        [InlineData("..")]                          // empty segments must not be misclassified as a JWT
        [InlineData("a..b")]                        // only two non-empty segments
        public void OpaqueOrMalformedTokensUseBasic(string token)
        {
            var header = GeneralUtils.CreateAzdoAuthHeader(token);

            header.Scheme.Should().Be("Basic");
            header.Parameter.Should().Be(Basic(token));
        }

        [Theory]
        // Null/empty/whitespace tokens are never valid for Azure DevOps and must fail loudly
        // instead of producing a broken empty Basic header.
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NullOrEmptyTokenThrows(string token)
        {
            Action act = () => GeneralUtils.CreateAzdoAuthHeader(token);

            act.Should().Throw<ArgumentException>().WithParameterName("token");
        }
    }
}
