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

using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Defines a XML transform that removes the XML nodes associated with the Signature.
    /// </summary>
    public class EnvelopedSignatureTransform : Transform
    {
        /// <summary>
        /// Creates an EnvelopedSignatureTransform
        /// </summary>
        public EnvelopedSignatureTransform()
        {
        }

        /// <summary>
        /// Gets the Algorithm associated with this transform
        /// </summary>
        public override string Algorithm { get => SecurityAlgorithms.EnvelopedSignature; }

        /// <summary>
        /// Sets the reader to exclude the &lt;Signature> element
        /// </summary>
        /// <param name="tokenStream"><see cref="XmlTokenStream"/>to process.</param>
        /// <returns><see cref="XmlTokenStream"/>with exclusion set.</returns>
        public override XmlTokenStream Process(XmlTokenStream tokenStream)
        {
            if (tokenStream == null)
                throw LogArgumentNullException(nameof(tokenStream));

            tokenStream.SetElementExclusion(XmlSignatureConstants.Elements.Signature, "http://www.w3.org/2000/09/xmldsig#");
            return tokenStream;
        }
    }
}
