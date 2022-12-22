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

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Constants for XML Signature
    /// Definitions for namespace, attributes and elements as defined in http://www.w3.org/TR/xmldsig-core/
    /// </summary>
    public static class XmlSignatureConstants
    {
#pragma warning disable 1591
        public const string ExclusiveC14nInclusiveNamespaces = "InclusiveNamespaces";
        public const string ExclusiveC14nNamespace = "http://www.w3.org/2001/10/xml-exc-c14n#";
        public const string ExclusiveC14nPrefix = "ec";
        public const string Namespace = "http://www.w3.org/2000/09/xmldsig#";
        public const string PreferredPrefix = "ds";
        public const string SecurityJan2004Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        public const string SecurityJan2004Prefix = "o";
        public const string TransformationParameters = "TransformationParameters";
        public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
        public const string XmlNamespaceNamespace = "http://www.w3.org/2000/xmlns/";
        public const string XmlNamepspacePrefix = "xmlns";
        public const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        public static class Attributes
        {
            public const string Algorithm = "Algorithm";
            public const string AnyUri = "anyURI";
            public const string Id = "Id";
            public const string NcName = "NCName";
            public const string Nil = "nil";
            public const string PrefixList = "PrefixList";
            public const string Type = "Type";
            public const string URI = "URI";
        }

        public static class Elements
        {
            public const string CanonicalizationMethod = "CanonicalizationMethod";
            public const string DigestMethod = "DigestMethod";
            public const string DigestValue = "DigestValue";
            public const string Exponent = "Exponent";
            public const string KeyInfo = "KeyInfo";
            public const string KeyName = "KeyName";
            public const string KeyValue = "KeyValue";
            public const string Modulus = "Modulus";
            public const string Object = "Object";
            public const string InclusiveNamespaces = "InclusiveNamespaces";
            public const string Reference = "Reference";
            public const string RetrievalMethod = "RetrievalMethod";
            public const string RSAKeyValue = "RSAKeyValue";
            public const string Signature = "Signature";
            public const string SignatureMethod = "SignatureMethod";
            public const string SignatureValue = "SignatureValue";
            public const string SignedInfo = "SignedInfo";
            public const string Transform = "Transform";
            public const string Transforms = "Transforms";
            public const string TransformationParameters = "TransformationParameters";
            public const string X509Certificate = "X509Certificate";
            public const string X509CRL = "X509CRL";
            public const string X509Data = "X509Data";
            public const string X509IssuerName = "X509IssuerName";
            public const string X509IssuerSerial = "X509IssuerSerial";
            public const string X509SerialNumber = "X509SerialNumber";
            public const string X509SKI = "X509SKI";
            public const string X509SubjectName = "X509SubjectName";
        }
#pragma warning restore 1591
    }
}
