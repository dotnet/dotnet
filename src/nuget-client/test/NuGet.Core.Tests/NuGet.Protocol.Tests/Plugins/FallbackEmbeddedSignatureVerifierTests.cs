// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class FallbackEmbeddedSignatureVerifierTests
    {
        [Fact]
        public void IsValid_Throws()
        {
            var verifier = new FallbackEmbeddedSignatureVerifier();

            Assert.Throws<PlatformNotSupportedException>(() => verifier.IsValid(filePath: "a"));
        }
    }
}
