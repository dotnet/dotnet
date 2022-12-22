// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System.Collections.Generic;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test.SigningTests
{
    public class Rfc3161TimestampProviderTests
    {
        public static IEnumerable<object[]> NonceData
        {
            get
            {
#if IS_DESKTOP
                yield return new object[] { new byte[] { 0xff, 0x00 }, new byte[] { 0xff, 0x01 } };
                yield return new object[] { new byte[] { 0xff, 0x01 }, new byte[] { 0xff, 0x01 } };
                yield return new object[] { new byte[] { 0xff, 0x7f }, new byte[] { 0xff, 0x7f } };
                yield return new object[] { new byte[] { 0xff, 0x80 }, new byte[] { 0xff, 0x01 } };
                yield return new object[] { new byte[] { 0xff, 0xff }, new byte[] { 0xff, 0x7f } };
#else
                yield return new object[] { new byte[] { 0x00, 0xff }, new byte[] { 0x00, 0xff } };
                yield return new object[] { new byte[] { 0x01, 0xff }, new byte[] { 0x01, 0xff } };
                yield return new object[] { new byte[] { 0x7f, 0xff }, new byte[] { 0x7f, 0xff } };
                yield return new object[] { new byte[] { 0x80, 0xff }, new byte[] { 0x00, 0xff } };
                yield return new object[] { new byte[] { 0xff, 0xff }, new byte[] { 0x7f, 0xff } };
#endif
            }
        }

        [Theory]
        [MemberData(nameof(NonceData))]
        public void EnsureValidNonce_Always_EnsuresValidNonce(byte[] actualNonce, byte[] expectedNonce)
        {
            Rfc3161TimestampProvider.EnsureValidNonce(actualNonce);

            Assert.Equal(expectedNonce, actualNonce);
        }
    }
}
#endif
