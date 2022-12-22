// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Xunit;
using BcGeneralName = Org.BouncyCastle.Asn1.X509.GeneralName;
using GeneralName = NuGet.Packaging.Signing.GeneralName;

namespace NuGet.Packaging.Test
{
    public class GeneralNameTests
    {
        [Fact]
        public void Create_WhenDistinguishedNameNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => GeneralName.Create(distinguishedName: null));

            Assert.Equal("distinguishedName", exception.ParamName);
        }

        [Fact]
        public void Create_WithDistinguishedName_ReturnsGeneralName()
        {
            var distinguishedName = new X500DistinguishedName(
                "CN=leaf.test,OU=Test Organizational Unit Name,O=Test Organization Name,L=Redmond,S=WA,C=US",
                X500DistinguishedNameFlags.UseCommas);

            var generalName = GeneralName.Create(distinguishedName);

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
            var bytes = new BcGeneralName(BcGeneralName.RegisteredID, new DerObjectIdentifier("1.2.3")).GetDerEncoded();

            var exception = Assert.Throws<SignatureException>(
                () => GeneralName.Read(bytes));

            Assert.Equal("The ASN.1 data is unsupported.", exception.Message);
        }

        [Fact]
        public void Read_WithDistinguishedName_ReturnsGeneralName()
        {
            var bytes = new BcGeneralName(BcGeneralName.DirectoryName, new X509Name("CN=test")).GetDerEncoded();

            var generalName = GeneralName.Read(bytes);

            Assert.NotNull(generalName);
            Assert.Equal("CN=test", generalName.DirectoryName.Name);
        }
    }
}
