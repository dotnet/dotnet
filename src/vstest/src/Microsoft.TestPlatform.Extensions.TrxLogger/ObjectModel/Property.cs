// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Stores a string which categorizes the Test
/// </summary>
internal sealed class Property : IXmlTestStore
{
    [StoreXmlSimpleField(Location = "@Key", DefaultValue = "")]
    private readonly string _key = string.Empty;
    [StoreXmlSimpleField(Location = "@Value", DefaultValue = "")]
    private readonly string _value = string.Empty;

    /// <summary>
    /// Create a new item with the property set
    /// </summary>
    public Property(string key, string value)
    {
        // Treat null as empty.

        _key = StripIllegalChars(key);
        _value = StripIllegalChars(value);
    }

    /// <summary>
    /// Gets the property for this Trait
    /// </summary>
    public string Trait
    {
        get
        {
            return _key;
        }
    }

    private static string StripIllegalChars(string property)
    {
        string ret = property.Trim();
        ret = ret.Replace("&", string.Empty);
        ret = ret.Replace("|", string.Empty);
        ret = ret.Replace("!", string.Empty);
        ret = ret.Replace(",", string.Empty);
        return ret;
    }

    /// <summary>
    /// Compare the values of the items
    /// </summary>
    /// <param name="other">Value being compared to.</param>
    /// <returns>True if the values are the same and false otherwise.</returns>
    public override bool Equals(object? other)
    {
        if (other is not Property otherItem)
        {
            return false;
        }

        TPDebug.Assert(_key != null, "property is null");
        return string.Equals(_key, otherItem._key, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Convert the property name to a hashcode
    /// </summary>
    /// <returns>Hashcode of the property.</returns>
    public override int GetHashCode()
    {
        TPDebug.Assert(_key != null, "property is null");
        return _key.ToUpperInvariant().GetHashCode();
    }

    /// <summary>
    /// Convert the property name to a string
    /// </summary>
    /// <returns>The property.</returns>
    public override string ToString()
    {
        TPDebug.Assert(_key != null, "property is null");
        return _key;
    }

    /// <summary>
    /// Saves the class under the XmlElement.
    /// </summary>
    /// <param name="element"> XmlElement element </param>
    /// <param name="parameters"> XmlTestStoreParameters parameters</param>
    public void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        new XmlPersistence().SaveSingleFields(element, this, parameters);
    }
}
