// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class EnvelopedSignatureTheoryData : TheoryDataBase
    {
        public Action<EnvelopedSignatureWriter> Action { get; set; }

        public CryptoProviderFactory CryptoProviderFactory { get; set; } = CryptoProviderFactory.Default;

        public bool ExpectSignature { get; set; } = true;

        public string ReferenceId { get; set; }

        public string InclusiveNamespacesPrefixList { get; set; }

        public SecurityKey SecurityKey { get; set; }

        public SecurityKey TokenSecurityKey { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public Signature Signature { get; set; }

        public string Xml { get; set; }

        public XmlReader XmlReader { get; set; }

        public XmlWriter XmlWriter { get; set; }

        public IXmlElementReader XmlElementReader { get; set; }

        public override string ToString()
        {
            return $"{TestId}, ExpectedException: {ExpectedException}.";
        }
    }
}
