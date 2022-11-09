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

using System.Collections.Generic;
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
