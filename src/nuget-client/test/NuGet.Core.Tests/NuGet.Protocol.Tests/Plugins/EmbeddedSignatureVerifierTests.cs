// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class EmbeddedSignatureVerifierTests
    {
        [PlatformFact(Platform.Windows)]
        public void Create_ReturnsWindowsEmbeddedSignatureVerifierOnWindows()
        {
            var verifier = EmbeddedSignatureVerifier.Create();

            Assert.IsType<WindowsEmbeddedSignatureVerifier>(verifier);
        }

        [PlatformFact(Platform.Darwin)]
        public void Create_ReturnsUnixPlatformsEmbeddedSignatureVerifierOnMacOS()
        {
            var verifier = EmbeddedSignatureVerifier.Create();

            Assert.IsType<UnixAndMonoPlatformsEmbeddedSignatureVerifier>(verifier);
        }

        [PlatformFact(Platform.Linux)]
        public void Create_ReturnsUnixPlatformsEmbeddedSignatureVerifierOnLinux()
        {
            var verifier = EmbeddedSignatureVerifier.Create();

            Assert.IsType<UnixAndMonoPlatformsEmbeddedSignatureVerifier>(verifier);
        }
    }
}
