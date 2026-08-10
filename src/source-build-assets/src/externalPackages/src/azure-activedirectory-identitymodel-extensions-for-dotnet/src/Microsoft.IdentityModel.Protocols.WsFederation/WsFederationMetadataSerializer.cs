// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;
using static Microsoft.IdentityModel.Protocols.WsFederation.WsFederationConstants;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Metadata serializer class for WsFed. 
    /// </summary>
    public class WsFederationMetadataSerializer
    {
        private DSigSerializer _dsigSerializer = DSigSerializer.Default;
        private string _preferredPrefix = WsFederationConstants.PreferredPrefix;

        /// <summary>
        /// Metadata serializer for WsFed.
        /// </summary>
        public WsFederationMetadataSerializer() { }

        /// <summary>
        /// Gets or sets the prefix to use when writing xml.
        /// </summary>
        public string PreferredPrefix
        {
            get => _preferredPrefix;
            set => _preferredPrefix = string.IsNullOrEmpty(value) ? throw LogExceptionMessage(new ArgumentNullException(nameof(value))) : value;
        }

        #region Read Metadata

        /// <summary>
        /// Read metadata and create the corresponding <see cref="WsFederationConfiguration"/>.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> used to read metadata</param>
        /// <returns><see cref="WsFederationConfiguration"/></returns>
        /// <exception cref="XmlReadException">if error occurs when reading metadata</exception>
        public WsFederationConfiguration ReadMetadata(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, Elements.EntityDescriptor, MetadataNamespace);

            var envelopeReader = new EnvelopedSignatureReader(reader);

            try
            {
                var configuration = ReadEntityDescriptor(envelopeReader);
                configuration.Signature = envelopeReader.Signature;
                return configuration;
            }
            catch (Exception ex)
            {
                if (ex is XmlReadException)
                    throw;

                throw XmlUtil.LogReadException(LogMessages.IDX22800, ex, Elements.EntityDescriptor, ex);
            }
        }

        /// <summary>
        /// Read EntityDescriptor element in xml.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> used to read entity descriptor</param>
        /// <returns><see cref="WsFederationConfiguration"/></returns>
        /// <exception cref="XmlReadException">if error occurs when reading entity descriptor</exception>
        protected virtual WsFederationConfiguration ReadEntityDescriptor(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, Elements.EntityDescriptor, MetadataNamespace);

            var configuration = new WsFederationConfiguration();

            // get entityID for issuer
            var issuer = reader.GetAttribute(Attributes.EntityId);
            if (string.IsNullOrEmpty(issuer))
                throw XmlUtil.LogReadException(LogMessages.IDX22801);
            configuration.Issuer = issuer;

            bool isEmptyElement = reader.IsEmptyElement;

            // <EntityDescriptor>
            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                if (IsSecurityTokenServiceTypeRoleDescriptor(reader))
                {
                    var roleDescriptor = ReadSecurityTokenServiceTypeRoleDescriptor(reader);
                    foreach (var keyInfo in roleDescriptor.KeyInfos)
                    {
                        configuration.KeyInfos.Add(keyInfo);
                        if (keyInfo.X509Data != null)
                        {
                            foreach (var data in keyInfo.X509Data)
                            {
                                foreach (var certificate in data.Certificates)
                                {
                                    X509Certificate2 cert = CertificateHelper.LoadX509Certificate(certificate);
                                    configuration.SigningKeys.Add(new X509SecurityKey(cert));
                                }
                            }
                        }
                    }

                    configuration.TokenEndpoint = roleDescriptor.TokenEndpoint;
                    configuration.ActiveTokenEndpoint = roleDescriptor.ActiveTokenEndpoint;
                }
                else
                {
                    reader.ReadOuterXml();
                }
            }

            // </EntityDescriptor>
            if (!isEmptyElement)
                reader.ReadEndElement();

            return configuration;
        }

        /// <summary>
        /// Read KeyDescriptor element in xml.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> used to read key descriptor</param>
        /// <returns><see cref="KeyInfo"/></returns>
        /// <exception cref="XmlReadException">if error occurs when reading key descriptor</exception>
        protected virtual KeyInfo ReadKeyDescriptorForSigning(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, Elements.KeyDescriptor, MetadataNamespace);

            var use = reader.GetAttribute(Attributes.Use);
            if (string.IsNullOrEmpty(use))
                LogHelper.LogWarning(LogMessages.IDX22808);

            // <KeyDescriptor>
            reader.ReadStartElement();

            if (reader.IsStartElement(XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace))
            {
                var keyInfo = _dsigSerializer.ReadKeyInfo(reader);
                // </KeyDescriptor>
                reader.ReadEndElement();
                return keyInfo;
            }
            else
            {
                throw XmlUtil.LogReadException(LogMessages.IDX22802, reader.LocalName, reader.NamespaceURI, XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace);
            }
        }

        /// <summary>
        /// Read RoleDescriptor element in xml.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> used to read security token service type role descriptor</param>
        /// <returns><see cref="SecurityTokenServiceTypeRoleDescriptor"/></returns>
        /// <exception cref="XmlReadException">if error occurs when reading role descriptor</exception>
        protected virtual SecurityTokenServiceTypeRoleDescriptor ReadSecurityTokenServiceTypeRoleDescriptor(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, Elements.RoleDescriptor, MetadataNamespace);

            if (!IsSecurityTokenServiceTypeRoleDescriptor(reader))
                throw XmlUtil.LogReadException(LogMessages.IDX22804);

            var roleDescriptor = new SecurityTokenServiceTypeRoleDescriptor();

            // <RoleDescriptor>
            bool isEmptyElement = reader.IsEmptyElement;

            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                if (reader.IsStartElement(Elements.KeyDescriptor, MetadataNamespace))
                    roleDescriptor.KeyInfos.Add(ReadKeyDescriptorForSigning(reader));
                else if (reader.IsStartElement(Elements.PassiveRequestorEndpoint, Namespace))
                    roleDescriptor.TokenEndpoint = ReadPassiveRequestorEndpoint(reader);
                else if (reader.IsStartElement(Elements.SecurityTokenServiceEndpoint, Namespace))
                    roleDescriptor.ActiveTokenEndpoint = ReadSecurityTokenServiceEndpoint(reader);
                else
                    reader.ReadOuterXml();
            }

            // </RoleDescriptor>
            if (!isEmptyElement)
                reader.ReadEndElement();

            if (roleDescriptor.KeyInfos.Count == 0)
                LogHelper.LogWarning(LogMessages.IDX22806);

            if (string.IsNullOrEmpty(roleDescriptor.TokenEndpoint))
                LogHelper.LogWarning(LogMessages.IDX22807);

            if (string.IsNullOrEmpty(roleDescriptor.ActiveTokenEndpoint))
                LogHelper.LogWarning(LogMessages.IDX22813);

            return roleDescriptor;
        }

        /// <summary>
        /// Read fed:PassiveRequestorEndpoint element in xml.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> used to read PassiveRequestorEndpoint</param>
        /// <returns>token endpoint string</returns>
        /// <exception cref="XmlReadException">if error occurs when reading PassiveRequestorEndpoint</exception>
        protected virtual string ReadPassiveRequestorEndpoint(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, Elements.PassiveRequestorEndpoint, Namespace);

            // <PassiveRequestorEndpoint>
            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22812, Elements.PassiveRequestorEndpoint);

            reader.ReadStartElement();
            reader.MoveToContent();

            // <EndpointReference>
            XmlUtil.CheckReaderOnEntry(reader, WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);
            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22812, WsAddressing.Elements.EndpointReference);

            reader.ReadStartElement(WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);
            reader.MoveToContent();

            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22803);

            // <Address>
            XmlUtil.CheckReaderOnEntry(reader, WsAddressing.Elements.Address, WsAddressing.Namespace);

            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22812, WsAddressing.Elements.Address);

            reader.ReadStartElement(WsAddressing.Elements.Address, WsAddressing.Namespace);
            reader.MoveToContent();

            var tokenEndpoint = Trim(reader.ReadContentAsString());

            if (string.IsNullOrEmpty(tokenEndpoint))
                throw XmlUtil.LogReadException(LogMessages.IDX22803);

            // </Address>
            reader.MoveToContent();
            reader.ReadEndElement();

            // </EndpointReference>
            reader.MoveToContent();
            reader.ReadEndElement();

            // </PassiveRequestorEndpoint>
            reader.MoveToContent();
            reader.ReadEndElement();

            return tokenEndpoint;
        }

        /// <summary>
        /// Read fed:SecurityTokenServiceEndpoint element from metadata XML.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/> used to read SecurityTokenServiceEndpoint.</param>
        /// <returns>Active token endpoint string</returns>
        /// <exception cref="XmlReadException">If an error occurs while reading the SecurityTokenServiceEndpoint</exception>
        protected virtual string? ReadSecurityTokenServiceEndpoint(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, Elements.SecurityTokenServiceEndpoint, Namespace);

            // <SecurityTokenServiceEndpoint>
            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22812, Elements.SecurityTokenServiceEndpoint);

            reader.ReadStartElement();
            reader.MoveToContent();

            // <EndpointReference>
            XmlUtil.CheckReaderOnEntry(reader, WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);
            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22812, WsAddressing.Elements.EndpointReference);

            reader.ReadStartElement(WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);
            reader.MoveToContent();

            if (reader.IsEmptyElement)
                throw XmlUtil.LogReadException(LogMessages.IDX22814);

            string? tokenEndpoint = null;

            while (reader.IsStartElement())
            {
                if (reader.IsStartElement(WsAddressing.Elements.Address, WsAddressing.Namespace))
                {
                    // <Address>
                    XmlUtil.CheckReaderOnEntry(reader, WsAddressing.Elements.Address, WsAddressing.Namespace);

                    if (reader.IsEmptyElement)
                        throw XmlUtil.LogReadException(LogMessages.IDX22812, WsAddressing.Elements.Address);

                    reader.ReadStartElement(WsAddressing.Elements.Address, WsAddressing.Namespace);
                    reader.MoveToContent();

                    tokenEndpoint = Trim(reader.ReadContentAsString());

                    if (string.IsNullOrEmpty(tokenEndpoint))
                        throw XmlUtil.LogReadException(LogMessages.IDX22814);

                    // </Address>
                    reader.MoveToContent();
                    reader.ReadEndElement();
                }
                else
                {
                    reader.ReadOuterXml();
                }
            }

            // </EndpointReference>
            reader.MoveToContent();
            reader.ReadEndElement();

            // </SecurityTokenServiceEndpoint>
            reader.MoveToContent();
            reader.ReadEndElement();

            return tokenEndpoint;
        }

        private static bool IsSecurityTokenServiceTypeRoleDescriptor(XmlReader reader)
        {
            if (reader == null || !reader.IsStartElement(Elements.RoleDescriptor, MetadataNamespace))
                return false;

            var type = reader.GetAttribute(Attributes.Type, XmlSignatureConstants.XmlSchemaNamespace);
            var typeQualifiedName = new XmlQualifiedName();

            if (!string.IsNullOrEmpty(type))
                typeQualifiedName = XmlUtil.ResolveQName(reader, type);

            if (!XmlUtil.EqualsQName(typeQualifiedName, Types.SecurityTokenServiceType, Namespace))
                return false;

            return true;
        }

        internal static string Trim(string stringToTrim)
        {
            if (string.IsNullOrEmpty(stringToTrim))
                return stringToTrim;

            char[] charsToTrim = { ' ', '\n' };
            return stringToTrim.Trim(charsToTrim);
        }

        #endregion

        #region Write Metadata

        /// <summary>
        /// Write the content in configuration into writer.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> used to write the configuration content.</param>
        /// <param name="configuration">The <see cref="WsFederationConfiguration"/> provided.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="configuration"/> is null.</exception>
        /// <exception cref="XmlWriteException">if <paramref name="configuration"/>.Issuer is null or empty.</exception>
        /// <exception cref="XmlWriteException">if <paramref name="configuration"/>.TokenEndpoint is null or empty.</exception>
        /// <exception cref="XmlWriteException">if error occurs when writing metadata.</exception>
        public void WriteMetadata(XmlWriter writer, WsFederationConfiguration configuration)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (configuration == null)
                throw LogArgumentNullException(nameof(configuration));

            if (string.IsNullOrEmpty(configuration.Issuer))
                throw XmlUtil.LogWriteException(LogMessages.IDX22810);

            if (string.IsNullOrEmpty(configuration.TokenEndpoint))
                throw XmlUtil.LogWriteException(LogMessages.IDX22811);

            if (configuration.SigningCredentials != null)
                writer = new EnvelopedSignatureWriter(writer, configuration.SigningCredentials, "id");

            writer.WriteStartDocument();

            // <EntityDescriptor>
            writer.WriteStartElement(Prefixes.Md, Elements.EntityDescriptor, MetadataNamespace);

            // @entityID
            writer.WriteAttributeString(Attributes.EntityId, configuration.Issuer);

            // <RoleDescriptor>
            writer.WriteStartElement(Prefixes.Md, Elements.RoleDescriptor, MetadataNamespace);
            writer.WriteAttributeString(Xmlns, Prefixes.Xsi, null, XmlSignatureConstants.XmlSchemaNamespace);
            writer.WriteAttributeString(Xmlns, PreferredPrefix, null, Namespace);
            writer.WriteAttributeString(Prefixes.Xsi, Attributes.Type, null, PreferredPrefix + ":" + Types.SecurityTokenServiceType);
            writer.WriteAttributeString(Attributes.ProtocolSupportEnumeration, Namespace);

            // write the key infos
            if (configuration.KeyInfos != null)
            {
                foreach (var keyInfo in configuration.KeyInfos)
                {
                    // <KeyDescriptor>
                    writer.WriteStartElement(Prefixes.Md, Elements.KeyDescriptor, MetadataNamespace);
                    writer.WriteAttributeString(Attributes.Use, KeyUse.Signing);
                    _dsigSerializer.WriteKeyInfo(writer, keyInfo);
                    // </KeyDescriptor>
                    writer.WriteEndElement();
                }
            }

            if (!string.IsNullOrEmpty(configuration.ActiveTokenEndpoint))
            {
                // <fed:SecurityTokenServiceEndpoint>
                writer.WriteStartElement(PreferredPrefix, Elements.SecurityTokenServiceEndpoint, Namespace);

                // <wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing"">
                writer.WriteStartElement(WsAddressing.PreferredPrefix, WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);

                // <wsa:Address>
                writer.WriteStartElement(WsAddressing.PreferredPrefix, WsAddressing.Elements.Address, WsAddressing.Namespace);

                // write TokenEndpoint
                writer.WriteString(configuration.ActiveTokenEndpoint);

                // </wsa:Address>
                writer.WriteEndElement();

                // </wsa:EndpointReference>
                writer.WriteEndElement();

                // </fed:SecurityTokenServiceEndpoint>
                writer.WriteEndElement();
            }

            // <fed:PassiveRequestorEndpoint>
            writer.WriteStartElement(PreferredPrefix, Elements.PassiveRequestorEndpoint, Namespace);

            // <wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing"">
            writer.WriteStartElement(WsAddressing.PreferredPrefix, WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);

            // <wsa:Address>
            writer.WriteStartElement(WsAddressing.PreferredPrefix, WsAddressing.Elements.Address, WsAddressing.Namespace);

            // write TokenEndpoint
            writer.WriteString(configuration.TokenEndpoint);

            // </wsa:Address>
            writer.WriteEndElement();

            // </wsa:EndpointReference>
            writer.WriteEndElement();

            // </fed:PassiveRequestorEndpoint>
            writer.WriteEndElement();

            // </RoleDescriptor>
            writer.WriteEndElement();

            // </EntityDescriptor>
            writer.WriteEndElement();

            writer.WriteEndDocument();
        }

        #endregion
    }
}
