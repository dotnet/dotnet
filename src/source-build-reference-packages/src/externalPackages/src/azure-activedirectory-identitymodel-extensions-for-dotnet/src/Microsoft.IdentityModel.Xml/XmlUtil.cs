// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Utilities for working with XML
    /// </summary>
    public static class XmlUtil
    {
        private static readonly Dictionary<byte, string> _hexDictionary = new Dictionary<byte, string>
        {
            { 0, "0" },
            { 1, "1" },
            { 2, "2" },
            { 3, "3" },
            { 4, "4" },
            { 5, "5" },
            { 6, "6" },
            { 7, "7" },
            { 8, "8" },
            { 9, "9" },
            { 10, "A" },
            { 11, "B" },
            { 12, "C" },
            { 13, "D" },
            { 14, "E" },
            { 15, "F" }
        };

        /// <summary>
        /// Checks if the <see cref="XmlReader"/> is pointing to an expected element.
        /// </summary>
        /// <param name="reader">the <see cref="XmlReader"/>to check.</param>
        /// <param name="element">the expected element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="element"/> is null or empty.</exception>
        /// <exception cref="XmlReadException">if <paramref name="reader"/> if not at a StartElement.</exception>
        /// <exception cref="XmlReadException">if <paramref name="reader"/> if not at at expected element.</exception>
        public static void CheckReaderOnEntry(XmlReader reader, string element)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (string.IsNullOrEmpty(element))
                throw LogArgumentNullException(nameof(element));

            // IsStartElement calls reader.MoveToContent().
            if (!reader.IsStartElement())
                throw LogReadException(LogMessages.IDX30022, reader.NodeType);

            if (!string.Equals(reader.LocalName, element, StringComparison.OrdinalIgnoreCase))
                throw LogReadException(LogMessages.IDX30024, element, reader.LocalName);
        }

        /// <summary>
        /// Checks if the <see cref="XmlReader"/> is pointing to an expected element.
        /// </summary>
        /// <param name="reader">the <see cref="XmlReader"/>to check.</param>
        /// <param name="element">the expected element.</param>
        /// <param name="namespace">the expected namespace.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="element"/> is null or empty.</exception>
        /// <exception cref="XmlReadException">if <paramref name="reader"/> if not at a StartElement.</exception>
        /// <exception cref="XmlReadException">if <paramref name="reader"/> if not at expected element.</exception>
        public static void CheckReaderOnEntry(XmlReader reader, string element, string @namespace)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            // IsStartElement calls reader.MoveToContent().
            if (!reader.IsStartElement())
                throw LogReadException(LogMessages.IDX30022, reader.NodeType);

            if (string.IsNullOrEmpty(@namespace))
            {
                if (!reader.IsStartElement(element))
                    throw LogReadException(LogMessages.IDX30024, element, reader.LocalName);
            }
            else
            {
                if (!reader.IsStartElement(element, @namespace))
                    throw LogReadException(LogMessages.IDX30011, @namespace, element, reader.NamespaceURI, reader.LocalName);
            }
        }

        /// <summary>
        /// Determine if reader is at expected element in one of the listed namespace in namespaceList. 
        /// </summary>
        /// <param name="reader">the <see cref="XmlReader"/>to check.</param>
        /// <param name="element">the expected element.</param>
        /// <param name="namespaceList">the expected namespace list.</param>
        /// <returns>if <paramref name="reader"/> is at expected element.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="element"/> is null or empty.</exception>
        public static bool IsStartElement(XmlReader reader, string element, ICollection<string> namespaceList)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (string.IsNullOrEmpty(element))
                throw LogArgumentNullException(nameof(element));

            if (namespaceList == null)
                return reader.IsStartElement(element);

            foreach (var @namespace in namespaceList)
            {
                if (!string.IsNullOrEmpty(@namespace) && reader.IsStartElement(element, @namespace))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if a Qualified names equals a name / namespace pair.
        /// </summary>
        /// <param name="qualifiedName">the <see cref="XmlQualifiedName"/> to compare.</param>
        /// <param name="name">the name to compare.</param>
        /// <param name="namespace">the namepace to compare.</param>
        /// <returns></returns>
        public static bool EqualsQName(XmlQualifiedName qualifiedName, string name, string @namespace)
        {
            return null != qualifiedName
                && StringComparer.Ordinal.Equals(name, qualifiedName.Name)
                && StringComparer.Ordinal.Equals(@namespace, qualifiedName.Namespace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>Hex representation of bytes</returns>
        internal static string GenerateHexString(byte[] bytes)
        {
            var stringBuilder = new StringBuilder();

            foreach (var b in bytes)
            {
                stringBuilder.Append(_hexDictionary[(byte)(b >> 4)]);
                stringBuilder.Append(_hexDictionary[(byte)(b & (byte)0x0F)]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the xsi:type as a <see cref="XmlQualifiedName"/> for the current element.
        /// </summary>
        /// <param name="reader">an <see cref="XmlReader"/>pointing at an Element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <returns>a <see cref="XmlQualifiedName"/>if the current element has an XSI type.
        /// If <paramref name="reader"/> is not on an element OR xsi type is not found, null.</returns>
        public static XmlQualifiedName GetXsiTypeAsQualifiedName(XmlReader reader)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (reader.NodeType != XmlNodeType.Element)
                return null;

            string xsiType = reader.GetAttribute(XmlSignatureConstants.Attributes.Type, XmlSignatureConstants.XmlSchemaNamespace);
            if (string.IsNullOrEmpty(xsiType))
                return null;

            return ResolveQName(reader, xsiType);
        }

        /// <summary>
        /// Determines if the <paramref name="reader"/> has an attribute that is 'nil'
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/> positioned on an element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <returns>true is the attribute value is 'nil'</returns>
        public static bool IsNil(XmlReader reader)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            string xsiNil = reader.GetAttribute(XmlSignatureConstants.Attributes.Nil, XmlSignatureConstants.XmlSchemaNamespace);
            return !string.IsNullOrEmpty(xsiNil) && XmlConvert.ToBoolean(xsiNil);
        }

        /// <summary>
        /// Normalizes an empty string to 'null'.
        /// </summary>
        /// <param name="string"></param>
        /// <returns>null if string is null or empty.</returns>
        public static string NormalizeEmptyString(string @string)
        {
            return string.IsNullOrEmpty(@string) ? null : @string;
        }

        /// <summary>
        /// Returns a new <see cref="XmlReadException"/> with message including the element and attribute.
        /// </summary>
        /// <param name="element">the missing element.</param>
        /// <param name="attribute">the missing attribute.</param>
        /// <returns>a <see cref="XmlReadException"/>.</returns>
        internal static Exception OnRequiredAttributeMissing(string element, string attribute)
        {
            return LogExceptionMessage(new XmlReadException(FormatInvariant(LogMessages.IDX30013, MarkAsNonPII(element), MarkAsNonPII(attribute))));
        }

        /// <summary>
        /// Determines if the prefix on a name maps to a namespace that is in scope the reader.
        /// </summary>
        /// <param name="reader">the <see cref="XmlReader"/> in scope.</param>
        /// <param name="qualifiedString">the qualifiedName to check.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="qualifiedString"/> is null.</exception>
        /// <returns>a <see cref="XmlQualifiedName"/> with the namespace that was in scope. If the prefix was not in scope, the namespace will be null.</returns>
        public static XmlQualifiedName ResolveQName(XmlReader reader, string qualifiedString)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (qualifiedString == null)
                throw LogArgumentNullException(nameof(qualifiedString));

            string name = qualifiedString;
            string prefix = String.Empty;

            int colon = qualifiedString.IndexOf(':');
            if (colon > -1)
            {
                prefix = qualifiedString.Substring(0, colon);
                name = qualifiedString.Substring(colon + 1, qualifiedString.Length - (colon + 1));
            }

            return new XmlQualifiedName(name, reader.LookupNamespace(prefix));
        }

        /// <summary>
        /// Validates that element the <paramref name="reader"/> is positioned on has an xsi:type attribute
        /// with a specific name and type.
        /// </summary>
        /// <param name="reader">an <see cref="XmlReader"/> positioned on an element.</param>
        /// <param name="expectedTypeName">the expected name of the xsi:type.</param>
        /// <param name="expectedTypeNamespace">the expected namespace of the xsi:type.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="expectedTypeName"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="expectedTypeNamespace"/> is null.</exception>
        /// <remarks>if the <paramref name="reader"/> does require an xsi:type attribute to be present. If the xsi:type is present, it will be validated.</remarks>
        public static void ValidateXsiType(XmlReader reader, string expectedTypeName, string expectedTypeNamespace)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (expectedTypeName == null)
                throw LogArgumentNullException(nameof(expectedTypeName));

            if (expectedTypeNamespace == null)
                throw LogArgumentNullException(nameof(expectedTypeNamespace));

            ValidateXsiType(reader, expectedTypeName, expectedTypeNamespace, false);
        }

        /// <summary>
        /// Validates that element the <paramref name="reader"/> is positioned on has an xsi:type attribute
        /// with a specific name and type.
        /// </summary>
        /// <param name="reader">an <see cref="XmlReader"/> positioned on an element.</param>
        /// <param name="expectedTypeName">the expected name of the xsi:type.</param>
        /// <param name="expectedTypeNamespace">the expected namespace of the xsi:type.</param>
        /// <param name="requireDeclaration">controls if the xsi:type must be present.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="expectedTypeName"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="expectedTypeNamespace"/> is null.</exception>
        /// <exception cref="XmlException">if xsi:type is not found and required.</exception>
        /// <exception cref="XmlException">if xsi:type is found and did not match expected.</exception>
        public static void ValidateXsiType(XmlReader reader, string expectedTypeName, string expectedTypeNamespace, bool requireDeclaration)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (expectedTypeName == null)
                throw LogArgumentNullException(nameof(expectedTypeName));

            if (expectedTypeNamespace == null)
                throw LogArgumentNullException(nameof(expectedTypeNamespace));

            var declaredType = GetXsiTypeAsQualifiedName(reader);
            if (declaredType == null)
            {
                if (requireDeclaration)
                    throw LogExceptionMessage(new XmlException(FormatInvariant(LogMessages.IDX30500, MarkAsNonPII(expectedTypeName), MarkAsNonPII(expectedTypeNamespace))));
                else
                    return;
            }

            if (!(StringComparer.Ordinal.Equals(expectedTypeNamespace, declaredType.Namespace)
               && StringComparer.Ordinal.Equals(expectedTypeName, declaredType.Name)))
            {
                throw LogExceptionMessage(new XmlException(FormatInvariant(LogMessages.IDX30501, MarkAsNonPII(expectedTypeName), MarkAsNonPII(expectedTypeNamespace), MarkAsNonPII(declaredType.Name), MarkAsNonPII(declaredType.Namespace))));
            }
        }

        /// <summary>
        /// Sends formatted <see cref="XmlReadException"/> to the Logger.
        /// </summary>
        /// <param name="format">the format string.</param>
        /// <param name="args">the arguments to use for formating.</param>
        /// <returns>a <see cref="XmlReadException"/>.</returns>
        public static Exception LogReadException(string format, params object[] args)
        {
            return LogExceptionMessage(new XmlReadException(FormatInvariant(format, args)));
        }

        /// <summary>
        /// Sends formatted <see cref="XmlReadException"/> to the Logger.
        /// </summary>
        /// <param name="format">the format string.</param>
        /// <param name="args">the arguments to use for formating.</param>
        /// <param name="inner">the inner exception.</param>
        /// <returns>a <see cref="XmlReadException"/>.</returns>
        public static Exception LogReadException(string format, Exception inner, params object[] args)
        {
            return LogExceptionMessage(new XmlReadException(FormatInvariant(format, args), inner));
        }

        /// <summary>
        /// Sends formatted <see cref="XmlValidationException"/> to the Logger.
        /// </summary>
        /// <param name="format">the format string.</param>
        /// <param name="args">the arguments to use for formating.</param>
        /// <returns>a <see cref="XmlValidationException"/>.</returns>
        public static Exception LogValidationException(string format, params object[] args)
        {
            return LogExceptionMessage(new XmlValidationException(FormatInvariant(format, args)));
        }

        /// <summary>
        /// Sends formatted <see cref="XmlValidationException"/> to the Logger.
        /// </summary>
        /// <param name="format">the format string.</param>
        /// <param name="args">the arguments to use for formating.</param>
        /// <param name="inner">the inner exception.</param>
        /// <returns>a <see cref="XmlValidationException"/>.</returns>
        public static Exception LogValidationException(string format, Exception inner, params object[] args)
        {
            return LogExceptionMessage(new XmlValidationException(FormatInvariant(format, args), inner));
        }

        /// <summary>
        /// Sends formatted <see cref="XmlWriteException"/> to the Logger.
        /// </summary>
        /// <param name="format">the format string.</param>
        /// <param name="args">the arguments to use for formating.</param>
        /// <returns>a <see cref="XmlWriteException"/>.</returns>
        public static Exception LogWriteException(string format, params object[] args)
        {
            return LogExceptionMessage(new XmlWriteException(FormatInvariant(format, args)));
        }

        /// <summary>
        /// Sends formatted <see cref="XmlWriteException"/> to the Logger.
        /// </summary>
        /// <param name="format">the format string.</param>
        /// <param name="args">the arguments to use for formating.</param>
        /// <param name="inner">the inner exception.</param>
        /// <returns>a <see cref="XmlWriteException"/>.</returns>
        public static Exception LogWriteException(string format, Exception inner, params object[] args)
        {
            return LogExceptionMessage(new XmlWriteException(FormatInvariant(format, args), inner));
        }

        internal static string[] TokenizeInclusiveNamespacesPrefixList(string inclusiveNamespacesPrefixList)
        {
            if (inclusiveNamespacesPrefixList == null)
                return null;

            string[] prefixes = inclusiveNamespacesPrefixList.Split(null);
            int count = 0;
            for (int i = 0; i < prefixes.Length; i++)
            {
                string prefix = prefixes[i];
                if (prefix == "#default")
                {
                    prefixes[count++] = string.Empty;
                }
                else if (prefix.Length > 0)
                {
                    prefixes[count++] = prefix;
                }
            }
            if (count == 0)
            {
                return null;
            }
            else if (count == prefixes.Length)
            {
                return prefixes;
            }
            else
            {
                string[] result = new string[count];
                Array.Copy(prefixes, result, count);
                return result;
            }
        }
    }
}
