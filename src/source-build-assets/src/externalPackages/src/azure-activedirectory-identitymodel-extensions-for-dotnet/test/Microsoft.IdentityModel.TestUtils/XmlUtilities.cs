// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

        public static byte[] CreateNonTransformedDigestBytes(string xml)
        {
            using (var stream = new MemoryStream())
            using (var writer = XmlWriter.Create(stream))
            using (var dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer))
            {
                CreateXmlTokenStream(xml).WriteTo(dictionaryWriter);
                dictionaryWriter.Flush();
                return Default.HashAlgorithm.ComputeHash(stream.ToArray());
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
