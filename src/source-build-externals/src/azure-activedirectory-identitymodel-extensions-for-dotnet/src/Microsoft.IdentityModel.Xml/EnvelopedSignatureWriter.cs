// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Wraps a <see cref="XmlWriter"/> and generates a signature automatically when the envelope
    /// is written completely. By default the generated signature is inserted as
    /// the last element in the envelope. This can be modified by explicitly
    /// calling WriteSignature to indicate the location inside the envelope where
    /// the signature should be inserted.
    /// </summary>
    public class EnvelopedSignatureWriter : DelegatingXmlDictionaryWriter
    {
        /// <summary>
        /// Default name of the SignaturePlaceholder element.
        /// </summary>
        /// <remarks>
        /// Signature placeholder element will be written, and later replaced with a correct signature
        /// when the envelope is completed. Placeholder element will be written only if <see cref="WriteSignature"/>
        /// method was explicitly called.
        /// </remarks>
        public static readonly string SignaturePlaceholder = "_SignaturePlaceholder";

        private MemoryStream _canonicalStream;
        private bool _disposed;
        private DSigSerializer _dsigSerializer = DSigSerializer.Default;
        private int _elementCount;
        private string _inclusiveNamespacesPrefixList;
        private XmlWriter _originalWriter;
        private string _referenceUri;
        private bool _signaturePlaceholderWritten;
        private SigningCredentials _signingCredentials;
        private MemoryStream _internalStream;
        private object _signatureLock = new object();

        /// <summary>
        /// Initializes an instance of <see cref="EnvelopedSignatureWriter"/>. The returned writer can be directly used
        /// to write the envelope. The signature will be automatically generated when
        /// the envelope is completed.
        /// </summary>
        /// <param name="writer">Writer to wrap/</param>
        /// <param name="signingCredentials">SigningCredentials to be used to generate the signature.</param>
        /// <param name="referenceId">The reference Id of the envelope.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="referenceId"/> is null or Empty.</exception>
        public EnvelopedSignatureWriter(XmlWriter writer, SigningCredentials signingCredentials, string referenceId)
            : this(writer, signingCredentials, referenceId, null)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="EnvelopedSignatureWriter"/>. The returned writer can be directly used
        /// to write the envelope. The signature will be automatically generated when
        /// the envelope is completed.
        /// </summary>
        /// <param name="writer">Writer to wrap/</param>
        /// <param name="signingCredentials">SigningCredentials to be used to generate the signature.</param>
        /// <param name="referenceId">The reference Id of the envelope.</param>
        /// <param name="inclusivePrefixList">inclusive prefix list to use for exclusive canonicalization.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="referenceId"/> is null or Empty.</exception>
        public EnvelopedSignatureWriter(XmlWriter writer, SigningCredentials signingCredentials, string referenceId, string inclusivePrefixList)
        {
            _originalWriter = writer ?? throw LogArgumentNullException(nameof(writer));
            _signingCredentials = signingCredentials ?? throw LogArgumentNullException(nameof(signingCredentials));
            if (string.IsNullOrEmpty(referenceId))
                throw LogArgumentNullException(nameof(referenceId));

            _inclusiveNamespacesPrefixList = inclusivePrefixList;
            _referenceUri = referenceId;
            _internalStream = new MemoryStream();
            _canonicalStream = new MemoryStream();
            InnerWriter = CreateTextWriter(Stream.Null);
            InnerWriter.StartCanonicalization(_canonicalStream, false, XmlUtil.TokenizeInclusiveNamespacesPrefixList(_inclusiveNamespacesPrefixList));
            InternalWriter = CreateTextWriter(_internalStream, Encoding.UTF8, false);
            _signaturePlaceholderWritten = false;
        }

        /// <summary>
        /// Gets or sets the <see cref="DSigSerializer"/> to use.
        /// </summary>
        /// <exception cref="ArgumentNullException">if value is null.</exception>
        public DSigSerializer DSigSerializer
        {
            get => _dsigSerializer;
            set => _dsigSerializer = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Calculates and inserts/replaces the Signature.
        /// </summary>
        private void OnEndRootElement()
        {
            // wrap-up canonicalization
            InnerWriter.WriteEndElement();
            InnerWriter.Flush();
            InnerWriter.EndCanonicalization();

            var signature = CreateSignature();

            // in case when signature placeholder element is written, it needs to be replaced with a correct (real) signature.
            if (_signaturePlaceholderWritten)
            {
                InternalWriter.WriteEndElement();
                InternalWriter.Flush();

                // create XmlTokenStream out of the internalStream, and write that XmlTokenStream into original writer.
                // while writing the XmlTokenStream, replace signature (placeholder) element with the real signature (using DSigSerializer).
                _internalStream.Position = 0;
                var xmlTokenStreamReader = new XmlTokenStreamReader(XmlDictionaryReader.CreateTextReader(_internalStream, XmlDictionaryReaderQuotas.Max));

                while (xmlTokenStreamReader.Read() != false) ;

                var xmlTokenStreamWriter = new XmlTokenStreamWriter(xmlTokenStreamReader.TokenStream);
                xmlTokenStreamWriter.WriteAndReplaceSignature(_originalWriter, signature, DSigSerializer);
            }
            // write the signature into the internalStream and write the complete internalStream, as a node, into the originalWriter.
            else
            {
                DSigSerializer.WriteSignature(InternalWriter, signature);
                InternalWriter.WriteEndElement();
                InternalWriter.Flush();

                _internalStream.Position = 0;
                XmlReader reader = XmlDictionaryReader.CreateTextReader(_internalStream, XmlDictionaryReaderQuotas.Max);
                reader.MoveToContent();
                _originalWriter.WriteNode(reader, false);

                // wrap-up the TracingWriter, if initialized.
                if (TracingWriter != null)
                {
                    DSigSerializer.WriteSignature(TracingWriter, signature);
                    TracingWriter.WriteEndElement();
                    TracingWriter.Flush();
                }
            }

            _originalWriter.Flush();
        }

        private Signature CreateSignature()
        {
            CryptoProviderFactory cryptoProviderFactory = _signingCredentials.CryptoProviderFactory ?? _signingCredentials.Key.CryptoProviderFactory;
            string digestAlgorithm = !string.IsNullOrEmpty(_signingCredentials.Digest) ? _signingCredentials.Digest : SupportedAlgorithms.GetDigestFromSignatureAlgorithm(_signingCredentials.Algorithm);
            var hashAlgorithm = cryptoProviderFactory.CreateHashAlgorithm(digestAlgorithm);

            if (hashAlgorithm == null)
                throw LogExceptionMessage(new XmlValidationException(FormatInvariant(LogMessages.IDX30213, MarkAsNonPII(cryptoProviderFactory.GetType().Name), _signingCredentials.Digest)));

            Reference reference = null;
            try
            {
                lock (_signatureLock)
                {
                    reference = new Reference(new EnvelopedSignatureTransform(), new ExclusiveCanonicalizationTransform { InclusiveNamespacesPrefixList = _inclusiveNamespacesPrefixList })
                    {
                        Uri = _referenceUri,
                        DigestValue = Convert.ToBase64String(hashAlgorithm.ComputeHash(_canonicalStream.GetBuffer(), 0, (int)_canonicalStream.Length)),
                        DigestMethod = digestAlgorithm
                    };
                }
            }
            finally
            {
                if (hashAlgorithm != null)
                    cryptoProviderFactory.ReleaseHashAlgorithm(hashAlgorithm);
            }

            var signedInfo = new SignedInfo(reference)
            {
                CanonicalizationMethod = SecurityAlgorithms.ExclusiveC14n,
                SignatureMethod = _signingCredentials.Algorithm
            };

            using (var canonicalSignedInfoStream = new MemoryStream())
            {
                using (var signedInfoWriter = CreateTextWriter(Stream.Null))
                {
                    signedInfoWriter.StartCanonicalization(canonicalSignedInfoStream, false, null);
                    DSigSerializer.WriteSignedInfo(signedInfoWriter, signedInfo);
                    signedInfoWriter.EndCanonicalization();
                    signedInfoWriter.Flush();

                    var provider = cryptoProviderFactory.CreateForSigning(_signingCredentials.Key, _signingCredentials.Algorithm);
                    if (provider == null)
                        throw LogExceptionMessage(new XmlValidationException(FormatInvariant(LogMessages.IDX30213, MarkAsNonPII(cryptoProviderFactory.GetType().Name), _signingCredentials.Key.ToString(), MarkAsNonPII(_signingCredentials.Algorithm))));

                    try
                    {
                        return new Signature
                        {
                            KeyInfo = new KeyInfo(_signingCredentials.Key),
                            SignatureValue = Convert.ToBase64String(provider.Sign(canonicalSignedInfoStream.ToArray())),
                            SignedInfo = signedInfo,
                        };
                    }
                    finally
                    {
                        if (provider != null)
                            cryptoProviderFactory.ReleaseSignatureProvider(provider);
                    }
                }
            }
        }

        /// <summary>
        /// Call this method while writing the envelope to indicate at which point the
        /// signature should be inserted.
        /// </summary>
        /// <remarks>
        /// Signature placeholder element will be written, and later replaced with a correct signature
        /// when the envelope is completed.
        /// </remarks>
        public void WriteSignature()
        {
            InternalWriter.WriteStartElement(SignaturePlaceholder);
            InternalWriter.WriteEndElement();
            InternalWriter.Flush();
            _signaturePlaceholderWritten = true;
        }

        /// <summary>
        /// Overrides the base class implementation. When the last element of the envelope is written
        /// the signature is automatically computed over the envelope and the signature is inserted at
        /// the appropriate position, if WriteSignature was explicitly called or is inserted at the
        /// end of the envelope.
        /// </summary>
        public override void WriteEndElement()
        {
            _elementCount--;
            if (_elementCount == 0)
            {
                base.Flush();
                OnEndRootElement();
            }
            else
            {
                base.WriteEndElement();
            }
        }

        /// <summary>
        /// Overrides the base class implementation. When the last element of the envelope is written
        /// the signature is automatically computed over the envelope and the signature is inserted at
        /// the appropriate position, if WriteSignature was explicitly called or is inserted at the
        /// end of the envelope.
        /// </summary>
        public override void WriteFullEndElement()
        {
            _elementCount--;
            if (_elementCount == 0)
            {
                base.Flush();
                OnEndRootElement();
            }
            else
            {
                base.WriteFullEndElement();
            }
        }

        /// <summary>
        /// Overrides the base class. Writes the specified start tag and associates
        /// it with the given namespace.
        /// </summary>
        /// <param name="prefix">The namespace prefix of the element.</param>
        /// <param name="localName">The local name of the element.</param>
        /// <param name="namespace">The namespace URI to associate with the element.</param>
        public override void WriteStartElement(string prefix, string localName, string @namespace)
        {
            _elementCount++;
            base.WriteStartElement(prefix, localName, @namespace);
        }

#region IDisposable Members
        /// <summary>
        /// Releases the unmanaged resources used by the System.IdentityModel.Protocols.XmlSignature.EnvelopedSignatureWriter and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                if (_internalStream != null)
                {
                    _internalStream.Dispose();
                    _internalStream = null;
                }

                if (_canonicalStream != null)
                {
                    _canonicalStream.Dispose();
                    _canonicalStream = null;
                }
            }
        }
#endregion
    }
}
