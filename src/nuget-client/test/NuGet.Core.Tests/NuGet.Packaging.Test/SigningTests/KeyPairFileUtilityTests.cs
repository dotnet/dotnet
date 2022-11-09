// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class KeyPairFileUtilityTests
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>() { { "a", "b" } };

        [Fact]
        public void GetValueOrThrow_WhenKeyDoesNotExist_Throws()
        {
            var exception = Assert.Throws<SignatureException>(
                () => KeyPairFileUtility.GetValueOrThrow(_dictionary, key: "c"));

            Assert.Equal(NuGetLogCode.NU3000, exception.Code);
            Assert.Equal("Missing expected key: c", exception.Message);
        }

        [Fact]
        public void GetValueOrThrow_WhenKeyExists_ReturnsValue()
        {
            var value = KeyPairFileUtility.GetValueOrThrow(_dictionary, key: "a");

            Assert.Equal("b", value);
        }
    }
}
