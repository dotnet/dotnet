// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class ExtensionsTests
    {
        private readonly Oid _oid = new("1.2.3");
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
            Verify(new[] { new TestExtension(_oid, _value, isCritical) });
        }

        [Fact]
        public void Read_WithFalseDefaultForCritical_ReturnsInstance()
        {
            Verify(new[] { new TestExtension(_oid, _value) });
        }

        [Fact]
        public void Read_WithMultipleExtensions_ReturnsInstance()
        {
            var testExtensions = new[]
            {
                new TestExtension(_oid, _value, critical: true),
                new TestExtension(new Oid(_oid.Value + ".4"), _value, critical: false)
            };

            Verify(testExtensions);
        }

        private static void Verify(IReadOnlyList<TestExtension> expectedExtensions)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                foreach (TestExtension expectedExtension in expectedExtensions)
                {
                    expectedExtension.Encode(writer);
                }
            }

            byte[] bytes = writer.Encode();
            Extensions actualExtensions = Extensions.Read(bytes);

            Assert.Equal(expectedExtensions.Count, actualExtensions.ExtensionsList.Count);

            for (var i = 0; i < expectedExtensions.Count; ++i)
            {
                TestExtension expectedExtension = expectedExtensions[i];
                Extension actualExtension = actualExtensions.ExtensionsList[i];

                Assert.Equal(expectedExtension.Oid.Value, actualExtension.Id.Value);
                Assert.Equal(expectedExtension.Critical, actualExtension.Critical);
                Assert.Equal(expectedExtension.Value.Span.ToArray(), actualExtension.Value);
            }
        }

        private sealed class TestExtension
        {
            internal Oid Oid { get; }
            internal bool? Critical { get; }
            internal ReadOnlyMemory<byte> Value { get; }

            internal TestExtension(Oid oid, ReadOnlyMemory<byte> value, bool? critical = false)
            {
                Oid = oid;
                Critical = critical;
                Value = value;
            }

            internal void Encode(AsnWriter writer)
            {
                using (writer.PushSequence())
                {
                    writer.WriteObjectIdentifier(Oid.Value);

                    if (Critical.HasValue)
                    {
                        writer.WriteBoolean(Critical.Value);
                    }

                    writer.WriteOctetString(Value.Span);
                }
            }
        }
    }
}
