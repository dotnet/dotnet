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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents Canonicalization algorithms found in &lt;SignedInfo> and in &lt;Reference>.
    /// </summary>
    public class ExclusiveCanonicalizationTransform : CanonicalizingTransfrom
    {
        /// <summary>
        /// Initializes an instance of <see cref="ExclusiveCanonicalizationTransform"/>.
        /// </summary>
        public ExclusiveCanonicalizationTransform() :
            this(false)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="ExclusiveCanonicalizationTransform"/>.
        /// </summary>
        /// <param name="includeComments">controls if the transform will include comments.</param>
        public ExclusiveCanonicalizationTransform(bool includeComments)
        {
            IncludeComments = includeComments;
        }

        /// <summary>
        /// Gets the Algorithm associated with this transform
        /// </summary>
        public override string Algorithm { get => IncludeComments ? SecurityAlgorithms.ExclusiveC14nWithComments : SecurityAlgorithms.ExclusiveC14n; }

        /// <summary>
        /// Applies a canonicalization transform over a set of XML nodes and computes the hash value.
        /// </summary>
        /// <param name="tokenStream">the set of XML nodes to transform.</param>
        /// <param name="hash">the hash algorithm to apply.</param>
        /// <returns>the hash of the transformed octets.</returns>
        public override byte[] ProcessAndDigest(XmlTokenStream tokenStream, HashAlgorithm hash)
        {
            if (tokenStream == null)
                throw LogArgumentNullException(nameof(tokenStream));

            if (hash == null)
                throw LogArgumentNullException(nameof(hash));

            using (var stream = new MemoryStream())
            using (var writer = XmlDictionaryWriter.CreateTextWriter(Stream.Null))
            {
                writer.StartCanonicalization(stream, IncludeComments, XmlUtil.TokenizeInclusiveNamespacesPrefixList(InclusiveNamespacesPrefixList));
                tokenStream.WriteTo(writer);
                writer.EndCanonicalization();
                writer.Flush();
                return hash.ComputeHash(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
    }
}
