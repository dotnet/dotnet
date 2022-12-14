// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security;
using Xunit;

namespace System.Security.Cryptography.X509Certificates.Tests
{
    [SkipOnPlatform(TestPlatforms.Browser, "Browser doesn't support X.509 certificates")]
    public static class ImportTests
    {
        [Fact]
        public static void TestImportNotSupported_X509Certificate()
        {
            using (var c = new X509Certificate())
            {
                VerifyImportNotSupported(c);
            }
        }

        [Fact]
        public static void TestImportNotSupported_X509Certificate2()
        {
            using (var c = new X509Certificate2())
            {
                VerifyImportNotSupported(c);
            }
        }

        private static void VerifyImportNotSupported(X509Certificate c)
        {
            Assert.Throws<PlatformNotSupportedException>(() => c.Import(Array.Empty<byte>()));
            Assert.Throws<PlatformNotSupportedException>(() => c.Import(string.Empty));
            Assert.Throws<PlatformNotSupportedException>(() => c.Import(Array.Empty<byte>(), string.Empty, X509KeyStorageFlags.DefaultKeySet));
            Assert.Throws<PlatformNotSupportedException>(() => c.Import(Array.Empty<byte>(), new SecureString(), X509KeyStorageFlags.DefaultKeySet));
            Assert.Throws<PlatformNotSupportedException>(() => c.Import(string.Empty, string.Empty, X509KeyStorageFlags.DefaultKeySet));
            Assert.Throws<PlatformNotSupportedException>(() => c.Import(string.Empty, new SecureString(), X509KeyStorageFlags.DefaultKeySet));
        }
    }
}
