//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
