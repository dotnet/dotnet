// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents a XmlDsig SignedInfo element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-SignedInfo
    /// </summary>
    public class SignedInfo : DSigElement
    {
        private DSigSerializer _dsigSerializer = DSigSerializer.Default;
        private string _canonicalizationMethod = SecurityAlgorithms.ExclusiveC14n;
        private string _signatureMethod = SecurityAlgorithms.RsaSha256Signature;

        /// <summary>
        /// Initializes a <see cref="SignedInfo"/> instance.
        /// </summary>
        public SignedInfo()
        {
            References = new List<Reference>();
        }

        /// <summary>
        /// Initializes a <see cref="SignedInfo"/> instance.
        /// </summary>
        /// <param name="reference">a <see cref="Reference"/> to include.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reference"/> is null.</exception>
        public SignedInfo(Reference reference)
        {
            if (reference == null)
                throw LogArgumentNullException(nameof(reference));

            References = new List<Reference> { reference };
        }

        internal MemoryStream CanonicalStream
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CanonicalizationMethod
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        /// <exception cref="NotSupportedException">if 'value' is not one of:
        /// "http://www.w3.org/2001/10/xml-exc-c14n#"
        /// "http://www.w3.org/2001/10/xml-exc-c14n#WithComments"
        /// </exception>
        public string CanonicalizationMethod
        {
            get
            {
                return _canonicalizationMethod;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                if (!string.Equals(value,SecurityAlgorithms.ExclusiveC14n) && !string.Equals(value, SecurityAlgorithms.ExclusiveC14nWithComments))
                    throw LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX30204, LogHelper.MarkAsNonPII(CanonicalizationMethod), LogHelper.MarkAsNonPII(SecurityAlgorithms.ExclusiveC14n), LogHelper.MarkAsNonPII(SecurityAlgorithms.ExclusiveC14nWithComments))));

                _canonicalizationMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the Reference.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public IList<Reference> References
        {
            get;
        }

        /// <summary>
        /// Gets or sets the SignatureMethod.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public string SignatureMethod
        {
            get
            {
                return _signatureMethod;
            }
            set
            {
                _signatureMethod = string.IsNullOrEmpty(value) ? throw LogArgumentNullException(value) : value;
            }
        }

        /// <summary>
        /// Verifies the digest of all <see cref="References"/>.
        /// </summary>
        /// <param name="cryptoProviderFactory">supplies any required cryptographic operators.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="cryptoProviderFactory"/> is null.</exception>
        public void Verify(CryptoProviderFactory cryptoProviderFactory)
        {
            if (cryptoProviderFactory == null)
                throw LogArgumentNullException(nameof(cryptoProviderFactory));

            foreach (var reference in References)
                reference.Verify(cryptoProviderFactory);
        }

        /// <summary>
        /// Writes the Canonicalized bytes into a stream.
        /// </summary>
        /// <param name="stream">the <see cref="Stream"/> to receive the bytes.</param>
        public void GetCanonicalBytes(Stream stream)
        {
            if (stream == null)
                throw LogArgumentNullException(nameof(stream));

            // CanonicalStream is set by reading with the DSigSerializer
            if (CanonicalStream != null)
            {
                CanonicalStream.WriteTo(stream);
            }
            else
            {
                using (var signedInfoWriter = XmlDictionaryWriter.CreateTextWriter(Stream.Null))
                {
                    signedInfoWriter.StartCanonicalization(stream, _canonicalizationMethod.Equals(SecurityAlgorithms.ExclusiveC14nWithComments), null);
                    _dsigSerializer.WriteSignedInfo(signedInfoWriter, this);
                    signedInfoWriter.Flush();
                    signedInfoWriter.EndCanonicalization();
                }
            }
        }
    }
}
