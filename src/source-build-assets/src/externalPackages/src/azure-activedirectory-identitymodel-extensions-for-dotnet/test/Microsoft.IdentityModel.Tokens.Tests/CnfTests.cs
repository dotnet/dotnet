// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class CnfTests
    {
        [Fact]
        public void CtorWithJson_ExpectedClassName()
        {
            var json = "{\"jwk\":{\"kty\":\"oct\",\"k\":\"AQIDBAUGBwgJCgsMDQ4PEC==\"}}";
            var cnf = new Cnf(json);
            Assert.Equal("Microsoft.IdentityModel.Tokens.Cnf", cnf.ClassName);
        }

        [Fact]
        public void Ctor_ExpectedClassName()
        {
            var cnf = new Cnf();
            Assert.Equal("Microsoft.IdentityModel.Tokens.Cnf", cnf.ClassName);
        }
    }
}
