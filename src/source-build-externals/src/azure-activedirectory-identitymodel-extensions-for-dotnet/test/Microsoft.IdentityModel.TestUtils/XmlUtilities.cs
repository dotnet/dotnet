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

using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Xml;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.IdentityModel.TestUtils
{
    public static class XmlUtilities
    {

        /// <summary>
        /// This XmlReader when wrapped as an XmlDictionaryReader will not be able to Canonicalize.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlReader CreateXmlReader(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XmlReader.Create(new StringReader(xml), new XmlReaderSettings() { XmlResolver = null });
        }

        /// <summary>
        /// This XmlReader will be able to Canonicalize.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlDictionaryReader CreateDictionaryReader(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xml), XmlDictionaryReaderQuotas.Max);
        }

        public static XmlReader CreateXDocumentReader(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XDocument.Parse(xml).CreateReader();
        }

        public static EnvelopedSignatureReader CreateEnvelopedSignatureReader(string xml)
        {
            return new EnvelopedSignatureReader(CreateDictionaryReader(xml));
        }

        public static XmlTokenStream CreateXmlTokenStream(string xml)
        {
            var xmlTokenStreamReader = new XmlTokenStreamReader(CreateDictionaryReader(xml));
            while (xmlTokenStreamReader.Read());
            return xmlTokenStreamReader.TokenStream;
        }

        public static byte[] CreateDigestBytes(string xml, bool includeComments)
        {
            using (var stream = new MemoryStream())
            {
                var transform = new ExclusiveCanonicalizationTransform(includeComments);
                return transform.ProcessAndDigest(CreateXmlTokenStream(xml), Default.HashAlgorithm);
            }
        }

        public static byte[] GenerateSignatureBytes(SignedInfo signedInfo, SecurityKey key)
        {
            using (var stream = new MemoryStream())
            {
                var serailizer = new DSigSerializer();
                var writer = XmlDictionaryWriter.CreateTextWriter(Stream.Null);
                var includeComments = signedInfo.CanonicalizationMethod == SecurityAlgorithms.ExclusiveC14nWithComments;
                writer.StartCanonicalization(stream, includeComments, null);
                serailizer.WriteSignedInfo(writer, signedInfo);
                writer.EndCanonicalization();
                writer.Flush();
                stream.Position = 0;
                var provider = key.CryptoProviderFactory.CreateForSigning(key, signedInfo.SignatureMethod);
                return provider.Sign(stream.ToArray());
            }
        }
    }
}
