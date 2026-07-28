// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Defines a XML transform that applies C14n canonicalization and produces a hash over the transformed XML nodes.
    /// </summary>
    public abstract class CanonicalizingTransfrom
    {
        /// <summary>
        /// Gets the algorithm
        /// </summary>
        public abstract string Algorithm { get; }

        /// <summary>
        /// Gets or sets a value indicating if this transform should include comments.
        /// </summary>
        public bool IncludeComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the a PrefixList to use when there is a need to include InclusiveNamespaces writing token.
        /// </summary>
        public string InclusiveNamespacesPrefixList
        {
            get;
            set;
        }

        /// <summary>
        /// Processes a set of XML nodes and returns the hash of the octets.
        /// </summary>
        /// <param name="tokenStream">the <see cref="XmlTokenStream"/> that has the XML nodes to process.</param>
        /// <param name="hashAlg">the <see cref="HashAlgorithm"/>to use</param>
        /// <returns>the hash of the processed XML nodes.</returns>
        public abstract byte[] ProcessAndDigest(XmlTokenStream tokenStream, HashAlgorithm hashAlg);

        /// <summary>
        /// Applies a canonicalization transform over a set of XML nodes.
        /// </summary>
        /// <param name="tokenStream">the set of XML nodes to transform.</param>
        /// <param name="includeComments">include comments in canonical bytes.</param>
        /// <param name="inclusiveNamespacesPrefixList">list of namespace prefixes to include</param>
        /// <returns>the bytes of the transformed octets.</returns>
        internal static string GetString(XmlTokenStream tokenStream, bool includeComments, string[] inclusiveNamespacesPrefixList)
        {
            var streamWriter = new XmlTokenStreamWriter(tokenStream);
            using (var stream = new MemoryStream())
            using (var writer = XmlDictionaryWriter.CreateTextWriter(Stream.Null))
            {
                writer.StartCanonicalization(stream, includeComments, inclusiveNamespacesPrefixList);
                streamWriter.WriteTo(writer);
                writer.EndCanonicalization();
                writer.Flush();
                return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
    }
}
