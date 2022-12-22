// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Implementing this interface indicates a custom persistence logic is provided.
/// The attribute based persistence is ignored in such a case.
/// </summary>
internal interface IXmlTestStore
{
    /// <summary>
    /// Saves the class under the XmlElement.
    /// </summary>
    /// <param name="element"> XmlElement element </param>
    /// <param name="parameters"> XmlTestStoreParameters parameters</param>
    void Save(XmlElement element, XmlTestStoreParameters? parameters);
}
