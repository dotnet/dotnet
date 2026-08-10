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
            while (xmlTokenStreamReader.Read()) ;
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

        public static string RemoveSignature(string xml)
        {
            // Remove the <ds:Signature>...</ds:Signature> element from the XML
            // This works for both indented and non-indented XML
            int signatureStart = xml.IndexOf("<ds:Signature");
            if (signatureStart != -1)
                return RemoveElement(xml, "ds:Signature");

            signatureStart = xml.IndexOf("<Signature");
            if (signatureStart == -1)
                throw new System.Xml.XmlException($"Start element not found to remove:'Signature', xml '{xml}'.");

            return RemoveElement(xml, "Signature");
        }

        public static string RemoveElement(string xml, string elementName)
        {
            // Remove the specified element from the XML
            // This works for both indented and non-indented XML
            int elementStart = xml.IndexOf($"<{elementName}");
            if (elementStart == -1)
                throw new System.Xml.XmlException($"Start element not found to remove:'{elementName}', xml '{xml}'.");

            int elementEnd = xml.IndexOf($"</{elementName}>", elementStart);
            if (elementEnd == -1)
                throw new System.Xml.XmlException($"End element not found to remove:'{elementName}', xml '{xml}'.");

            elementEnd += $"</{elementName}>".Length;

            // Remove the element
            string xmlWithoutElement = xml.Remove(elementStart, elementEnd - elementStart);
            return xmlWithoutElement;
        }

        /// <summary>
        /// Take the attribute statements from the source XML and swap them into the destination XML.
        /// </summary>
        /// <param name="xmlSource"></param>
        /// <param name="xmlDestination"></param>
        /// <returns></returns>
        /// <exception cref="System.Xml.XmlException"></exception>
        public static string SwapAttributeStatements(string xmlSource, string xmlDestination)
        {
            int destinationSigStart = xmlDestination.IndexOf("<saml:AttributeStatement>");
            if (destinationSigStart != -1)
                return SwapXmlElement(xmlSource, xmlDestination, "<saml:AttributeStatement>", "</saml:AttributeStatement>");
            else
            {
                destinationSigStart = xmlDestination.IndexOf("<AttributeStatement>");
                if (destinationSigStart == -1)
                    throw new System.Xml.XmlException($"AttributeStatement not found to swap: {xmlDestination}");

                return SwapXmlElement(xmlSource, xmlDestination, "<AttributeStatement>", "</AttributeStatement>");
            }
        }

        public static string SwapSignatureValueElements(string xmlSource, string xmlDestination)
        {
            // Find the <SignatureValue> element in destination
            int destinationSigStart = xmlDestination.IndexOf("<SignatureValue");
            if (destinationSigStart != -1)
                return SwapXmlElement(xmlSource, xmlDestination, "<SignatureValue", "</SignatureValue>");
            else
            {
                destinationSigStart = xmlDestination.IndexOf("<ds:SignatureValue");
                if (destinationSigStart == -1)
                    throw new System.Xml.XmlException($"No SignatureValue element found in {xmlDestination}");

                return SwapXmlElement(xmlSource, xmlDestination, "<ds:SignatureValue", "</ds:SignatureValue>");
            }
        }

        public static string SwapXmlElement(string xmlSource, string xmlDestination, string startElement, string endElement)
        {
            int destinationStart = xmlDestination.IndexOf(startElement);
            if (destinationStart == -1)
                return xmlDestination;

            int destinationEnd = xmlDestination.IndexOf(endElement);
            if (destinationEnd == -1)
                return xmlDestination;

            destinationEnd += endElement.Length;

            int sourceEnd = -1;
            int sourceStart = xmlSource.IndexOf(startElement);
            if (sourceStart == -1)
                return xmlSource;

            sourceEnd = xmlSource.IndexOf(endElement);
            if (sourceEnd == -1)
                return xmlSource;

            sourceEnd += endElement.Length;

            // Extract the element from xmlSource
            string newElement = xmlSource.Substring(sourceStart, sourceEnd - sourceStart);

            // Replace the element in xmlDestination
            string transformedElement = $"{xmlDestination.Substring(0, destinationStart)}{newElement}{xmlDestination
                .Substring(destinationEnd)}";

            return transformedElement;
        }

    }
}
