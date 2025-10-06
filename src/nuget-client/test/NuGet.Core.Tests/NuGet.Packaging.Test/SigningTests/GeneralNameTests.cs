// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing;
using Xunit;
using GeneralName = NuGet.Packaging.Signing.GeneralName;
using TestGeneralName = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.GeneralName;

namespace NuGet.Packaging.Test
{
    public class GeneralNameTests
    {
        [Fact]
        public void Create_WhenDistinguishedNameNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => GeneralName.Create(distinguishedName: null));

            Assert.Equal("distinguishedName", exception.ParamName);
        }

        [Fact]
        public void Create_WithDistinguishedName_ReturnsGeneralName()
        {
            var distinguishedName = new X500DistinguishedName(
                "CN=leaf.test,OU=Test Organizational Unit Name,O=Test Organization Name,L=Redmond,S=WA,C=US",
                X500DistinguishedNameFlags.UseCommas);

            GeneralName generalName = GeneralName.Create(distinguishedName);

            Assert.Same(distinguishedName, generalName.DirectoryName);
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => GeneralName.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithUnsupportedChoice_Throws()
        {
            TestGeneralName testGeneralName = new(registeredId: "1.2.3");
            byte[] bytes = Encode(testGeneralName);

            SignatureException exception = Assert.Throws<SignatureException>(
                () => GeneralName.Read(bytes));

            Assert.Equal("The ASN.1 data is unsupported.", exception.Message);
        }

        [Fact]
        public void Read_WithDistinguishedName_ReturnsGeneralName()
        {
            X500DistinguishedName directoryName = new("CN=test");
            TestGeneralName testGeneralName = new(directoryName: directoryName.RawData);
            byte[] bytes = Encode(testGeneralName);

            GeneralName generalName = GeneralName.Read(bytes);

            Assert.NotNull(generalName);
            Assert.Equal(directoryName.Name, generalName.DirectoryName.Name);
        }

        private static byte[] Encode(TestGeneralName testGeneralName)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            testGeneralName.Encode(writer);

            return writer.Encode();
        }
    }
}
