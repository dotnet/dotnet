// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
