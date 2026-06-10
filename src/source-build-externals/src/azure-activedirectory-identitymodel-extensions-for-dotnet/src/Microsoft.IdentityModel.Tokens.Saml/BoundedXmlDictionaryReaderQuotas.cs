// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Provides <see cref="XmlDictionaryReaderQuotas"/> with a bounded <see cref="XmlDictionaryReaderQuotas.MaxDepth"/>
    /// suitable for parsing XML received from external sources.
    /// </summary>
    internal static class BoundedXmlDictionaryReaderQuotas
    {
        /// <summary>
        /// Bounded quotas with <see cref="XmlDictionaryReaderQuotas.MaxDepth"/> = 32. All other limits match <see cref="XmlDictionaryReaderQuotas.Max"/>.
        /// </summary>
        internal static XmlDictionaryReaderQuotas Quotas { get; } = new XmlDictionaryReaderQuotas
        {
            MaxDepth = 32,
            MaxStringContentLength = int.MaxValue,
            MaxArrayLength = int.MaxValue,
            MaxBytesPerRead = int.MaxValue,
            MaxNameTableCharCount = int.MaxValue
        };
    }
}