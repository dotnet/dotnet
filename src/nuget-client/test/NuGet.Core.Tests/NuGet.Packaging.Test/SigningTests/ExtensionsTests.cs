// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class ExtensionsTests
    {
        private readonly Oid _oid = new Oid("1.2.3");
        private readonly byte[] _value = new byte[] { 0, 1, 2 };

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => Extensions.Read(new byte[] { 0x30, 0x0b }));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Read_WithCritical_ReturnsInstance(bool isCritical)
        {
            Verify(new[] { new TestExtension(_oid, isCritical, _value) });
        }

        [Fact]
        public void Read_WithFalseDefaultForCritical_ReturnsInstance()
        {
            var bcExtensions = new DerSequence(
                new DerSequence(new DerObjectIdentifier(_oid.Value), new DerOctetString(_value)));
            var bytes = bcExtensions.GetDerEncoded();

            var extensions = Extensions.Read(bytes);

            Assert.Equal(1, extensions.ExtensionsList.Count);

            var extension = extensions.ExtensionsList[0];

            Assert.Equal(_oid.Value, extension.Id.Value);
            Assert.False(extension.Critical);
            Assert.Equal(_value, extension.Value);
        }

        [Fact]
        public void Read_WithMultipleExtensions_ReturnsInstance()
        {
            var testExtensions = new[]
            {
                new TestExtension(_oid, isCritical: true, value: _value),
                new TestExtension(new Oid(_oid.Value + ".4"),  isCritical: false, value: _value)
            };

            Verify(testExtensions);
        }

        private static void Verify(IReadOnlyList<TestExtension> testExtensions)
        {
            var bcExtensionsGenerator = new X509ExtensionsGenerator();

            foreach (var testExtension in testExtensions)
            {
                bcExtensionsGenerator.AddExtension(
                    new DerObjectIdentifier(testExtension.Id.Value), testExtension.IsCritical, testExtension.Value);
            }

            var bcExtensions = bcExtensionsGenerator.Generate();

            var extensions = Extensions.Read(bcExtensions.GetDerEncoded());

            Assert.Equal(testExtensions.Count, extensions.ExtensionsList.Count);

            var i = 0;

            foreach (var extensionOid in bcExtensions.GetExtensionOids())
            {
                var bcExtension = bcExtensions.GetExtension(extensionOid);
                var testExtension = testExtensions[i];
                var extension = extensions.ExtensionsList[i];

                Assert.Equal(testExtension.Id.Value, extension.Id.Value);
                Assert.Equal(testExtension.IsCritical, extension.Critical);
                Assert.Equal(testExtension.Value, extension.Value);

                Assert.Equal(extensionOid.Id, extension.Id.Value);
                Assert.Equal(bcExtension.IsCritical, extension.Critical);
                Assert.Equal(bcExtension.Value.GetOctets(), extension.Value);

                ++i;
            }
        }

        private sealed class TestExtension
        {
            internal Oid Id { get; }
            internal bool IsCritical { get; }
            internal byte[] Value { get; }

            internal TestExtension(Oid id, bool isCritical, byte[] value)
            {
                Id = id;
                IsCritical = isCritical;
                Value = value;
            }
        }
    }
}
