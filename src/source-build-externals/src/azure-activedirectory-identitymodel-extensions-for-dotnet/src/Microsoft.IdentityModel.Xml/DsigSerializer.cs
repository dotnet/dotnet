// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Reads and writes XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820
    /// </summary>
    public class DSigSerializer
    {
        private static DSigSerializer _default;
        private int _maximumReferenceTransforms = 5;
        private TransformFactory _transformFactory = TransformFactory.Default;

        /// <summary>
        /// Returns the default <see cref="DSigSerializer"/> instance.
        /// </summary>
        public static DSigSerializer Default
        {
            get => _default;
            set => _default = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Static constructor that initializes the default <see cref="DSigSerializer"/>.
        /// </summary>
        static DSigSerializer()
        {
            Default = new DSigSerializer();
        }

        /// <summary>
        /// Initializes an instance of <see cref="DSigSerializer"/>
        /// </summary>
        public DSigSerializer()
        {
        }

        /// <summary>
        /// Gets or sets the maximum number of Transforms that are allowed on a Reference
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if value is less than 1.</exception>
        /// <remarks>Default value is: 10.</remarks>
        [DefaultValue(10)]
        public int MaximumReferenceTransforms
        {
            get => _maximumReferenceTransforms;
            set => _maximumReferenceTransforms = value < 0 ? throw LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), FormatInvariant(LogMessages.IDX30600, LogHelper.MarkAsNonPII(value)))) : value;
        }

        /// <summary>
        /// Gets or sets the prefix to use when writing the Signature element.
        /// </summary>
        public string Prefix
        {
            get;
            set;
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-KeyInfo
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> pointing positioned on a &lt;KeyInfo> element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        /// <remarks>Only handles IssuerSerial, Ski, SubjectName, Certificate. Unsupported types are skipped. Only a X509 data element is supported.</remarks>
        public virtual KeyInfo ReadKeyInfo(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace);

            var keyInfo = new KeyInfo
            {
                Prefix = reader.Prefix
            };

            try
            {
                bool isEmptyElement = reader.IsEmptyElement;

                // <KeyInfo>
                reader.ReadStartElement();
                while (reader.IsStartElement())
                {
                    // <X509Data>
                    if (reader.IsStartElement(XmlSignatureConstants.Elements.X509Data, XmlSignatureConstants.Namespace))
                    {
                        keyInfo.X509Data.Add(ReadX509Data(reader));
                    }
                    // <RetrievalMethod>
                    else if (reader.IsStartElement(XmlSignatureConstants.Elements.RetrievalMethod, XmlSignatureConstants.Namespace))
                    {
                        keyInfo.RetrievalMethodUri = reader.GetAttribute(XmlSignatureConstants.Attributes.URI);
                        reader.ReadOuterXml();
                    }
                    // <KeyName>
                    else if (reader.IsStartElement(XmlSignatureConstants.Elements.KeyName, XmlSignatureConstants.Namespace))
                    {
                        keyInfo.KeyName = reader.ReadElementContentAsString(XmlSignatureConstants.Elements.KeyName, XmlSignatureConstants.Namespace);
                    }
                    // <KeyValue>
                    else if (reader.IsStartElement(XmlSignatureConstants.Elements.KeyValue, XmlSignatureConstants.Namespace))
                    {
                        reader.ReadStartElement(XmlSignatureConstants.Elements.KeyValue, XmlSignatureConstants.Namespace);
                        if (reader.IsStartElement(XmlSignatureConstants.Elements.RSAKeyValue, XmlSignatureConstants.Namespace))
                        {
                            // Multiple RSAKeyValues were found
                            if (keyInfo.RSAKeyValue != null)
                                throw XmlUtil.LogReadException(LogMessages.IDX30015, XmlSignatureConstants.Elements.RSAKeyValue);

                            keyInfo.RSAKeyValue = ReadRSAKeyValue(reader);
                        }
                        else
                        {
                            // Skip the element since it is not an <RSAKeyValue>
                            if (LogHelper.IsEnabled(EventLogLevel.Warning))
                            {
                                LogHelper.LogWarning(LogMessages.IDX30300, reader.ReadOuterXml());
                            }
                            else
                            {
                                reader.Skip();
                            }
                        }

                        // </KeyValue>
                        reader.ReadEndElement();
                    }
                    else
                    {
                        // Skip the element since it is not one of  <RetrievalMethod>, <X509Data>, <KeyValue>
                        if (LogHelper.IsEnabled(EventLogLevel.Warning))
                        {
                            LogHelper.LogWarning(LogMessages.IDX30300, reader.ReadOuterXml());
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                }

                // </KeyInfo>
                if (!isEmptyElement)
                    reader.ReadEndElement();

            }
            catch(Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30017, ex, XmlSignatureConstants.Elements.KeyInfo, ex);
            }

            return keyInfo;
        }

        /// <summary>
        /// Reads the "X509DataElement" element conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-X509Data.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned on a <see cref="XmlSignatureConstants.Elements.X509Data"/> element.</param>
        private static X509Data ReadX509Data(XmlReader reader)
        {
            var data = new X509Data();

            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX30108);

            reader.ReadStartElement(XmlSignatureConstants.Elements.X509Data, XmlSignatureConstants.Namespace);
            while (reader.IsStartElement())
            {
                if (reader.IsStartElement(XmlSignatureConstants.Elements.X509Certificate, XmlSignatureConstants.Namespace))
                {
                    data.Certificates.Add(reader.ReadElementContentAsString());
                }
                else if (reader.IsStartElement(XmlSignatureConstants.Elements.X509IssuerSerial, XmlSignatureConstants.Namespace))
                {
                    if (data.IssuerSerial != null)
                        throw XmlUtil.LogReadException(LogMessages.IDX30015, XmlSignatureConstants.Elements.X509IssuerSerial);
                    data.IssuerSerial = ReadIssuerSerial(reader);
                }
                else if (reader.IsStartElement(XmlSignatureConstants.Elements.X509SKI, XmlSignatureConstants.Namespace))
                {
                    if (data.SKI != null)
                        throw XmlUtil.LogReadException(LogMessages.IDX30015, XmlSignatureConstants.Elements.X509SKI);
                    data.SKI = reader.ReadElementContentAsString();
                }
                else if (reader.IsStartElement(XmlSignatureConstants.Elements.X509SubjectName, XmlSignatureConstants.Namespace))
                {
                    if (data.SubjectName != null)
                        throw XmlUtil.LogReadException(LogMessages.IDX30015, XmlSignatureConstants.Elements.X509SubjectName);
                    data.SubjectName = reader.ReadElementContentAsString();
                }
                else if (reader.IsStartElement(XmlSignatureConstants.Elements.X509CRL, XmlSignatureConstants.Namespace))
                {
                    if (data.CRL != null)
                        throw XmlUtil.LogReadException(LogMessages.IDX30015, XmlSignatureConstants.Elements.X509CRL);
                    data.CRL = reader.ReadElementContentAsString();
                }
                else
                {
                    // Skip the element since it is not one of  <X509Certificate>, <X509IssuerSerial>, <X509SKI>, <X509SubjectName>, <X509CRL>
                    if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    {
                        LogHelper.LogWarning(LogMessages.IDX30300, reader.ReadOuterXml());
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
            }

            // </X509Data>
            reader.ReadEndElement();

            return data;
        }

        /// <summary>
        /// Reads the "X509IssuerSerial" element conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-X509Data.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned on a <see cref="XmlSignatureConstants.Elements.X509IssuerSerial"/> element.</param>
        private static IssuerSerial ReadIssuerSerial(XmlReader reader)
        {
            reader.ReadStartElement(XmlSignatureConstants.Elements.X509IssuerSerial, XmlSignatureConstants.Namespace);

            if (!reader.IsStartElement(XmlSignatureConstants.Elements.X509IssuerName, XmlSignatureConstants.Namespace))
                throw XmlUtil.LogReadException(LogMessages.IDX30011, XmlSignatureConstants.Namespace, XmlSignatureConstants.Elements.X509IssuerName, reader.NamespaceURI, reader.LocalName);

            var issuerName = reader.ReadElementContentAsString(XmlSignatureConstants.Elements.X509IssuerName, XmlSignatureConstants.Namespace);

            if (!reader.IsStartElement(XmlSignatureConstants.Elements.X509SerialNumber, XmlSignatureConstants.Namespace))
                throw XmlUtil.LogReadException(LogMessages.IDX30011, XmlSignatureConstants.Namespace, XmlSignatureConstants.Elements.X509SerialNumber, reader.NamespaceURI, reader.LocalName);

            var serialNumber = reader.ReadElementContentAsString(XmlSignatureConstants.Elements.X509SerialNumber, XmlSignatureConstants.Namespace);

            reader.ReadEndElement();

            return new IssuerSerial(issuerName, serialNumber);
         }

        /// <summary>
        /// Reads the "RSAKeyValue" element conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-RSAKeyValue.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned on a <see cref="XmlSignatureConstants.Elements.RSAKeyValue"/> element.</param>
        private static RSAKeyValue ReadRSAKeyValue(XmlReader reader)
        {
            reader.ReadStartElement(XmlSignatureConstants.Elements.RSAKeyValue, XmlSignatureConstants.Namespace);

            if (!reader.IsStartElement(XmlSignatureConstants.Elements.Modulus, XmlSignatureConstants.Namespace))
                throw XmlUtil.LogReadException(LogMessages.IDX30011, XmlSignatureConstants.Namespace, XmlSignatureConstants.Elements.Modulus, reader.NamespaceURI, reader.LocalName);

            string modulus = reader.ReadElementContentAsString(XmlSignatureConstants.Elements.Modulus, XmlSignatureConstants.Namespace);

            if (!reader.IsStartElement(XmlSignatureConstants.Elements.Exponent, XmlSignatureConstants.Namespace))
                throw XmlUtil.LogReadException(LogMessages.IDX30011, XmlSignatureConstants.Namespace, XmlSignatureConstants.Elements.Exponent, reader.NamespaceURI, reader.LocalName);

            string exponent = reader.ReadElementContentAsString(XmlSignatureConstants.Elements.Exponent, XmlSignatureConstants.Namespace);

            reader.ReadEndElement();

            return new RSAKeyValue(modulus, exponent);    
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Signature
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/>positioned on a &lt;Signature> element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        /// <returns><see cref="Signature"/></returns>
        public virtual Signature ReadSignature(XmlReader reader)
        {
            try
            {
                // <Signature>
                XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.Signature, XmlSignatureConstants.Namespace);

                var prefix = reader.Prefix;
                var id = reader.GetAttribute(XmlSignatureConstants.Attributes.Id, null);
                reader.Read();

                var signedInfo = ReadSignedInfo(reader);
                reader.MoveToContent();
                var signatureValue = reader.ReadElementContentAsString().Trim();
                KeyInfo keyInfo = null;
                if (reader.IsStartElement(XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace))
                    keyInfo = ReadKeyInfo(reader);

                // </Signature>
                reader.MoveToContent();

                // throw if we are not on EndElement, something unexpected
                if (reader.NodeType != XmlNodeType.EndElement)
                    throw XmlUtil.LogReadException(LogMessages.IDX30025, XmlSignatureConstants.Elements.Signature, reader.NodeType, reader.LocalName);

                reader.ReadEndElement();
                return new Signature
                {
                    Id = id,
                    KeyInfo = keyInfo,
                    Prefix = prefix,
                    SignedInfo = signedInfo,
                    SignatureValue = signatureValue
                };
            }
            catch(Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30016, ex, XmlSignatureConstants.Elements.Signature);
            }
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-SignedInfo
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/>positioned on a &lt;SignedInfo> element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        /// <returns><see cref="SignedInfo"/></returns>
        public virtual SignedInfo ReadSignedInfo(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.SignedInfo, XmlSignatureConstants.Namespace);

            try
            {
                XmlDictionaryReader canonicalizingReader = null;
                XmlDictionaryReader actualReader = null;
                if (reader is XmlDictionaryReader dictionaryReader)
                {
                    if (dictionaryReader.CanCanonicalize)
                    {
                        canonicalizingReader = dictionaryReader;
                        actualReader = reader as XmlDictionaryReader;
                        if (actualReader == null)
                            actualReader = XmlDictionaryReader.CreateDictionaryReader(reader);
                    }
                }

                if (canonicalizingReader == null && reader is XmlTokenStreamReader tokenStreamReader)
                {
                    if (tokenStreamReader.XmlDictionaryReader.CanCanonicalize)
                    {
                        canonicalizingReader = tokenStreamReader.XmlDictionaryReader;
                        actualReader = reader as XmlDictionaryReader;
                        if (actualReader == null)
                            actualReader = XmlDictionaryReader.CreateDictionaryReader(reader);
                    }
                }

                if (canonicalizingReader == null)
                {
                    var bufferedStream = new MemoryStream();
                    var settings = new XmlWriterSettings
                    {
                        Encoding = Encoding.UTF8,
                        NewLineHandling = NewLineHandling.None
                    };

                    // need to read into buffer since the canonicalization reader needs a stream.
                    var bufferwriter = XmlDictionaryWriter.Create(bufferedStream, settings);
                    bufferwriter.WriteNode(reader, true);
                    bufferwriter.Flush();
                    bufferedStream.Position = 0;
                    canonicalizingReader = XmlDictionaryReader.CreateTextReader(bufferedStream, XmlDictionaryReaderQuotas.Max);
                    actualReader = canonicalizingReader;
                }

                var signedInfo = new SignedInfo
                {
                    CanonicalStream = new MemoryStream()
                };

                // TODO - should not always use 'false'
                canonicalizingReader.StartCanonicalization(signedInfo.CanonicalStream, false, null);
                actualReader.MoveToStartElement(XmlSignatureConstants.Elements.SignedInfo, XmlSignatureConstants.Namespace);
                signedInfo.Prefix = actualReader.Prefix;
                signedInfo.Id = actualReader.GetAttribute(XmlSignatureConstants.Attributes.Id, null);
                // read <SignedInfo ...> start element
                actualReader.Read();
                // TODO - if comments are not false, then we need to reset.
                // this should be very rare.
                signedInfo.CanonicalizationMethod = ReadCanonicalizationMethod(actualReader);
                signedInfo.SignatureMethod = ReadSignatureMethod(actualReader);
                signedInfo.References.Add(ReadReference(actualReader));

                if (actualReader.IsStartElement(XmlSignatureConstants.Elements.Reference, XmlSignatureConstants.Namespace))
                    throw XmlUtil.LogReadException(LogMessages.IDX30020);

                actualReader.ReadEndElement();
                canonicalizingReader.EndCanonicalization();
                signedInfo.CanonicalStream.Flush();

                return signedInfo;
            }
            catch(Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30016, ex, XmlSignatureConstants.Elements.SignedInfo);
            }
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Reference
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/>positioned on a &lt;Reference> element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        /// <returns><see cref="Reference"/></returns>
        public virtual Reference ReadReference(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.Reference, XmlSignatureConstants.Namespace);

            try
            {
                var reference = new Reference
                {
                    Prefix = reader.Prefix,
                    Id = reader.GetAttribute(XmlSignatureConstants.Attributes.Id, null),
                    Type = reader.GetAttribute(XmlSignatureConstants.Attributes.Type, null),
                    Uri = reader.GetAttribute(XmlSignatureConstants.Attributes.URI, null)
                };

                reader.Read();
                ReadTransforms(reader, reference);

                // <DigestMethod> - required
                XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.DigestMethod, XmlSignatureConstants.Namespace);
                bool isEmptyElement = reader.IsEmptyElement;
                var digestMethod = reader.GetAttribute(XmlSignatureConstants.Attributes.Algorithm, null);
                if (string.IsNullOrEmpty(digestMethod))
                    throw XmlUtil.OnRequiredAttributeMissing(XmlSignatureConstants.Elements.DigestMethod, XmlSignatureConstants.Attributes.Algorithm);

                reference.DigestMethod = digestMethod;

                reader.Read();
                reader.MoveToContent();
                if (!isEmptyElement)
                    reader.ReadEndElement();

                // <DigestValue>
                XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.DigestValue, XmlSignatureConstants.Namespace);
                var digestValue = reader.ReadElementContentAsString().Trim();
                if (string.IsNullOrEmpty(digestValue))
                    throw XmlUtil.LogReadException(LogMessages.IDX30206, reference.Uri ?? reference.Id);

                reference.DigestValue = digestValue;

                // </Reference>
                reader.MoveToContent();
                reader.ReadEndElement();

                return reference;
            }
            catch(Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30016, ex, XmlSignatureConstants.Elements.Reference);
            }
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Transforms
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/>positioned on a &lt;Transforms> element.</param>
        /// <param name="reference">a <see cref="Reference"/> to attach transforms.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="reference"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        public virtual void ReadTransforms(XmlReader reader, Reference reference)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (reference == null)
                throw LogArgumentNullException(nameof(reference));

            try
            {
                // <Transforms> - optional
                if (!reader.IsStartElement(XmlSignatureConstants.Elements.Transforms, XmlSignatureConstants.Namespace))
                    return;

                if (reader.IsEmptyElement)
                {
                    reader.Read();
                    return;
                }

                reader.Read();
                // <Transform> - unbounded
                int numberOfTransforms = 0;
                while (reader.IsStartElement(XmlSignatureConstants.Elements.Transform, XmlSignatureConstants.Namespace))
                {
                    var isEmptyElement = reader.IsEmptyElement;
                    var algorithm = reader.GetAttribute(XmlSignatureConstants.Attributes.Algorithm);
                    if (string.IsNullOrEmpty(algorithm))
                        throw XmlUtil.LogReadException(LogMessages.IDX30105);

                    if (TransformFactory.IsSupportedTransform(algorithm))
                    {
                        reference.Transforms.Add(TransformFactory.GetTransform(algorithm));
                        reader.Read();
                    }
                    else if (TransformFactory.IsSupportedCanonicalizingTransfrom(algorithm))
                    {
                        reference.CanonicalizingTransfrom = TransformFactory.GetCanonicalizingTransform(algorithm);
                        reader.Read();
                        // release 5.2.1 did not require 'ec' ns. So, we need to accept names with and without a prefix.
                        if (reader.IsStartElement(XmlSignatureConstants.Elements.InclusiveNamespaces, XmlSignatureConstants.ExclusiveC14nNamespace) || reader.IsStartElement(XmlSignatureConstants.Elements.InclusiveNamespaces))
                        {
                            bool isOnEmptyElement = reader.IsEmptyElement;
                            reference.CanonicalizingTransfrom.InclusiveNamespacesPrefixList = reader.GetAttribute(XmlSignatureConstants.Attributes.PrefixList);
                            reader.ReadStartElement();
                            if (!isOnEmptyElement)
                                reader.ReadEndElement();
                        }
                    }
                    else
                        throw XmlUtil.LogReadException(LogMessages.IDX30210, algorithm);

                    if (++numberOfTransforms > MaximumReferenceTransforms)
                        throw LogHelper.LogExceptionMessage(new XmlReadException(FormatInvariant(LogMessages.IDX30029, !string.IsNullOrEmpty(reference.Id) ? reference.Id : (!string.IsNullOrEmpty(reference.Uri) ? reference.Uri : reference.GetType().ToString()), MaximumReferenceTransforms)));

                    reader.MoveToContent();
                    if (!isEmptyElement)
                        reader.ReadEndElement();
                }

                // </ Transforms>
                reader.MoveToContent();
                reader.ReadEndElement();
            }
            catch (Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30016, ex, XmlSignatureConstants.Elements.Transforms);
            }
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-SignatureMethod
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/>positioned on a &lt;SignatureMethod> element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        /// <returns>A string with the signature method.</returns>
        public virtual string ReadSignatureMethod(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.SignatureMethod, XmlSignatureConstants.Namespace);

            try
            {
                bool isEmptyElement = reader.IsEmptyElement;
                var signatureMethod = reader.GetAttribute(XmlSignatureConstants.Attributes.Algorithm, null);
                if (signatureMethod == null)
                    throw XmlUtil.OnRequiredAttributeMissing(XmlSignatureConstants.Elements.SignatureMethod, XmlSignatureConstants.Attributes.Algorithm);

                reader.Read();
                reader.MoveToContent();
                if (!isEmptyElement)
                {
                    reader.MoveToContent();
                    reader.ReadEndElement();
                }

                return signatureMethod;
            }
            catch(Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30016, ex, XmlSignatureConstants.Elements.Transform);
            }
        }

        /// <summary>
        /// Reads XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-CanonicalizationMethod
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/>positioned on a &lt;CanonicalizationMethod> element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="XmlReadException">if there is a problem reading the XML.</exception>
        /// <returns>A string with the canonicalization method.</returns>
        public virtual string ReadCanonicalizationMethod(XmlReader reader)
        {
            // <CanonicalizationMethod>
            XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.CanonicalizationMethod, XmlSignatureConstants.Namespace);

            try
            {
                bool isEmptyElement = reader.IsEmptyElement;
                var algorithm = reader.GetAttribute(XmlSignatureConstants.Attributes.Algorithm, null);
                if (string.IsNullOrEmpty(algorithm))
                    throw XmlUtil.LogReadException(LogMessages.IDX30013, XmlSignatureConstants.Elements.Signature, XmlSignatureConstants.Attributes.Algorithm);

                if (algorithm != SecurityAlgorithms.ExclusiveC14nWithComments && algorithm != SecurityAlgorithms.ExclusiveC14n)
                    throw XmlUtil.LogReadException(LogMessages.IDX30100, XmlSignatureConstants.Elements.Transform, algorithm, SecurityAlgorithms.ExclusiveC14n, SecurityAlgorithms.ExclusiveC14nWithComments);

                reader.Read();
                reader.MoveToContent();
                if (!isEmptyElement)
                {
                    if (reader.IsStartElement(XmlSignatureConstants.ExclusiveC14nInclusiveNamespaces))
                        throw XmlUtil.LogReadException(LogMessages.IDX30107);

                    reader.MoveToContent();
                    reader.ReadEndElement();
                }

                return algorithm;
            }
            catch(Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX30016, ex, XmlSignatureConstants.Elements.Transform);
            }
        }

        /// <summary>
        /// Writes the contents of a <see cref="KeyInfo"/> as XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-KeyInfo.
        /// </summary>
        /// <param name="writer">the <see cref="XmlWriter"/> to use.</param>
        /// <param name="keyInfo">the <see cref="KeyInfo"/>to write.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyInfo"/> is null.</exception>
        /// <exception cref="XmlWriteException">if there is a problem writing the XML.</exception>
        public virtual void WriteKeyInfo(XmlWriter writer, KeyInfo keyInfo)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (keyInfo == null)
                throw LogArgumentNullException(nameof(keyInfo));

            // <KeyInfo>
            writer.WriteStartElement(XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace);

            if (keyInfo.KeyName != null)
            {
                writer.WriteElementString(Prefix, XmlSignatureConstants.Elements.KeyName, XmlSignatureConstants.Namespace, keyInfo.KeyName);
            }

            if (keyInfo.RSAKeyValue != null)
            {
                // <KeyValue>
                writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.KeyValue, XmlSignatureConstants.Namespace);

                // <RSAKeyValue>
                writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.RSAKeyValue, XmlSignatureConstants.Namespace);

                // <Modulus>...</Modulus>
                writer.WriteElementString(Prefix, XmlSignatureConstants.Elements.Modulus, XmlSignatureConstants.Namespace, keyInfo.RSAKeyValue.Modulus);

                // <Exponent>...</Exponent>
                writer.WriteElementString(Prefix, XmlSignatureConstants.Elements.Exponent, XmlSignatureConstants.Namespace, keyInfo.RSAKeyValue.Exponent);

                // </RSAKeyValue>
                writer.WriteEndElement();

                // </KeyValue>
                writer.WriteEndElement();
            }

            if (keyInfo.RetrievalMethodUri != null)
            {
                writer.WriteStartElement(keyInfo.Prefix, XmlSignatureConstants.Elements.RetrievalMethod, XmlSignatureConstants.Namespace);
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.URI, null, keyInfo.RetrievalMethodUri);
                writer.WriteEndElement();
            }

            foreach (var data in keyInfo.X509Data)
            {
                // <X509Data>
                writer.WriteStartElement(keyInfo.Prefix, XmlSignatureConstants.Elements.X509Data, XmlSignatureConstants.Namespace);

                if (data.IssuerSerial != null)
                {
                    writer.WriteStartElement(keyInfo.Prefix, XmlSignatureConstants.Elements.X509IssuerSerial, XmlSignatureConstants.Namespace);

                    writer.WriteElementString(keyInfo.Prefix, XmlSignatureConstants.Elements.X509IssuerName, XmlSignatureConstants.Namespace, data.IssuerSerial.IssuerName);

                    writer.WriteElementString(keyInfo.Prefix, XmlSignatureConstants.Elements.X509SerialNumber, XmlSignatureConstants.Namespace, data.IssuerSerial.SerialNumber);

                    writer.WriteEndElement();
                }

                if (data.SKI != null)
                {
                    writer.WriteElementString(keyInfo.Prefix, XmlSignatureConstants.Elements.X509SKI, XmlSignatureConstants.Namespace, data.SKI);

                }

                if (data.SubjectName != null)
                {
                    writer.WriteElementString(keyInfo.Prefix, XmlSignatureConstants.Elements.X509SubjectName, XmlSignatureConstants.Namespace, data.SubjectName);
                }

                foreach (var certificate in data.Certificates)
                {
                    // <X509Certificate>...</X509Certificate>
                    writer.WriteElementString(keyInfo.Prefix, XmlSignatureConstants.Elements.X509Certificate, XmlSignatureConstants.Namespace, certificate);
                }

                if (data.CRL != null)
                {
                    writer.WriteElementString(keyInfo.Prefix, XmlSignatureConstants.Elements.X509CRL, XmlSignatureConstants.Namespace, data.CRL);
                }

                // </X509Data>
                writer.WriteEndElement();
            }

            // </KeyInfo>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the contents of a <see cref="Reference"/> as XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Reference.
        /// </summary>
        /// <param name="writer">the <see cref="XmlWriter"/> to use.</param>
        /// <param name="reference">the <see cref="Reference"/>to write.</param>
        /// <remarks>Assumes the &lt;DigestValue> has been calculated, no canonicalization or digest calculation is performed.</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="reference"/> is null.</exception>
        /// <exception cref="XmlWriteException">if <see cref="Reference.DigestMethod"/> is null or empty.</exception>
        /// <exception cref="XmlWriteException">if <see cref="Reference.DigestValue"/> is null or empty.</exception>
        /// <exception cref="XmlWriteException">if one of the values in <see cref="Reference.Transforms"/> is null or empty.</exception>
        public virtual void WriteReference(XmlWriter writer, Reference reference)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (reference == null)
                throw LogArgumentNullException(nameof(reference));

            if (string.IsNullOrEmpty(reference.DigestMethod))
                throw XmlUtil.LogWriteException(LogMessages.IDX30401, XmlSignatureConstants.Elements.Reference, XmlSignatureConstants.Elements.DigestMethod);

            if (string.IsNullOrEmpty(reference.DigestValue))
                throw XmlUtil.LogWriteException(LogMessages.IDX30401, XmlSignatureConstants.Elements.Reference, XmlSignatureConstants.Elements.DigestValue);

            // <Reference>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.Reference, XmlSignatureConstants.Namespace);

            // @Id
            if (reference.Id != null)
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Id, null, reference.Id);

            // @Uri
            if (reference.Uri != null)
            {
                if (reference.Uri.StartsWith("#", StringComparison.Ordinal))
                    writer.WriteAttributeString(XmlSignatureConstants.Attributes.URI, null, reference.Uri);
                else
                    writer.WriteAttributeString(XmlSignatureConstants.Attributes.URI, null, "#" + reference.Uri);
            }

            // @Type
            if (reference.Type != null)
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Type, null, reference.Type);

            // <Transforms>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.Transforms, XmlSignatureConstants.Namespace);

            // <Transform>
            foreach (var transform in reference.Transforms)
            {
                if (transform == null)
                    throw XmlUtil.LogWriteException(LogMessages.IDX30403);

                // <Transform>
                writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.Transform, XmlSignatureConstants.Namespace);

                // @Algorithm
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Algorithm, transform.Algorithm);

                // </Transform>
                writer.WriteEndElement();
            }
            
            // Write Canonicalizing transform
            if (reference.CanonicalizingTransfrom != null)
            {
                // <Transform>
                writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.Transform, XmlSignatureConstants.Namespace);

                // @Algorithm
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Algorithm, reference.CanonicalizingTransfrom.Algorithm);

                // <InclusiveNamespaces>
                if (!string.IsNullOrEmpty(reference.CanonicalizingTransfrom.InclusiveNamespacesPrefixList))
                {
                    // @PrefixList
                    writer.WriteStartElement(XmlSignatureConstants.ExclusiveC14nPrefix, XmlSignatureConstants.Elements.InclusiveNamespaces, XmlSignatureConstants.ExclusiveC14nNamespace);
                    writer.WriteAttributeString(XmlSignatureConstants.Attributes.PrefixList, reference.CanonicalizingTransfrom.InclusiveNamespacesPrefixList);
                    writer.WriteEndElement();
                }

                // </Transform>
                writer.WriteEndElement();
            }

            // </Transforms>
            writer.WriteEndElement();

            // <DigestMethod>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.DigestMethod, XmlSignatureConstants.Namespace);

            // @Algorithm
            writer.WriteAttributeString(XmlSignatureConstants.Attributes.Algorithm, null, reference.DigestMethod);

            // </DigestMethod>
            writer.WriteEndElement();

            // <DigestValue />
            writer.WriteElementString(Prefix, XmlSignatureConstants.Elements.DigestValue, XmlSignatureConstants.Namespace, reference.DigestValue);

            // </Reference>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the contents of a <see cref="Signature"/> as XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Signature.
        /// </summary>
        /// <param name="writer">the <see cref="XmlWriter"/> to use.</param>
        /// <param name="signature">the <see cref="Signature"/>to write.</param>
        /// <remarks>Assumes the &lt;SignatureValue> has been calculated, no canonicalization or signature calculation is performed.</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signature"/> is null.</exception>
        /// <exception cref="XmlWriteException">if <see cref="Signature.SignatureValue"/> is null or empty.</exception>
        /// <exception cref="XmlWriteException">if <see cref="Signature.SignedInfo"/> is null.</exception>
        /// <exception cref="XmlWriteException">if one of the values in <see cref="Reference.Transforms"/> is null or empty.</exception>
        public virtual void WriteSignature(XmlWriter writer, Signature signature)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (signature == null)
                throw LogArgumentNullException(nameof(signature));

            if (string.IsNullOrEmpty(signature.SignatureValue))
                throw XmlUtil.LogWriteException(LogMessages.IDX30401, XmlSignatureConstants.Elements.Signature, XmlSignatureConstants.Elements.SignatureValue);

            if (signature.SignedInfo == null)
                throw XmlUtil.LogWriteException(LogMessages.IDX30404);

            // <Signature>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.Signature, XmlSignatureConstants.Namespace);
            if (signature.Id != null)
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Id, null, signature.Id);

            // <SignedInfo>
            WriteSignedInfo(writer, signature.SignedInfo);

            // <SignatureValue>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.SignatureValue, XmlSignatureConstants.Namespace);

            // TODO - need signature value id
            // @Id
            if (!string.IsNullOrEmpty(signature.Id))
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Id, null, signature.Id);

            writer.WriteString(signature.SignatureValue);

            // </ SignatureValue>
            writer.WriteEndElement();

            // <KeyInfo>
            if (signature.KeyInfo != null)
                WriteKeyInfo(writer, signature.KeyInfo);

            // </ Signature>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the contents of a <see cref="SignedInfo"/> as XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-SignedInfo.
        /// </summary>
        /// <param name="writer">the <see cref="XmlWriter"/> to use.</param>
        /// <param name="signedInfo">the <see cref="SignedInfo"/>to write.</param>
        /// <remarks>Assumes the &lt;Reference> digest has been calculated, no canonicalization or digest calculation is performed.</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signedInfo"/> is null.</exception>
        /// <exception cref="XmlWriteException">if <see cref="SignedInfo.CanonicalizationMethod"/> is null or empty.</exception>
        /// <exception cref="XmlWriteException">if <see cref="SignedInfo.References"/> is null.</exception>
        /// <exception cref="NotSupportedException">if <see cref="SignedInfo.References" />.Count > 1.</exception>
        /// <exception cref="XmlWriteException">if <see cref="SignedInfo.SignatureMethod"/> is null or empty.</exception>
        public virtual void WriteSignedInfo(XmlWriter writer, SignedInfo signedInfo)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (signedInfo == null)
                throw LogArgumentNullException(nameof(signedInfo));

            if (string.IsNullOrEmpty(signedInfo.CanonicalizationMethod))
                throw XmlUtil.LogWriteException(LogMessages.IDX30401, XmlSignatureConstants.Elements.SignedInfo, XmlSignatureConstants.Elements.CanonicalizationMethod);

            if (string.IsNullOrEmpty(signedInfo.SignatureMethod))
                throw XmlUtil.LogWriteException(LogMessages.IDX30401, XmlSignatureConstants.Elements.SignedInfo, XmlSignatureConstants.Elements.SignatureMethod);

            if (signedInfo.References == null)
                throw XmlUtil.LogWriteException(LogMessages.IDX30405);

            // <SignedInfo>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.SignedInfo, XmlSignatureConstants.Namespace);

            // @Id
            if (signedInfo.Id != null)
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Id, null, signedInfo.Id);

            // <CanonicalizationMethod>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.CanonicalizationMethod, XmlSignatureConstants.Namespace);
            
            //@Algorithm
            writer.WriteAttributeString(XmlSignatureConstants.Attributes.Algorithm, null, signedInfo.CanonicalizationMethod);
            writer.WriteEndElement();

            // <SignatureMethod>
            writer.WriteStartElement(Prefix, XmlSignatureConstants.Elements.SignatureMethod, XmlSignatureConstants.Namespace);
            
            // @Algorithm
            writer.WriteAttributeString(XmlSignatureConstants.Attributes.Algorithm, null, signedInfo.SignatureMethod);

            // </SignatureMethod>
            writer.WriteEndElement();

            // <Reference>
            foreach(var reference in signedInfo.References)
                WriteReference(writer, reference);

            // </SignedInfo>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the <see cref="TransformFactory"/> to use when processing transforms in References
        /// </summary>
        public TransformFactory TransformFactory
        {
            get => _transformFactory;
            set => _transformFactory = value ?? throw LogArgumentNullException(nameof(value));
        }
    }
}
