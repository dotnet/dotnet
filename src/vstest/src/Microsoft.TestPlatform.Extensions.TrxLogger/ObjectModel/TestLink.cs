// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Test link.
/// </summary>
internal sealed class TestLink : IXmlTestStore
{
    public TestLink(Guid id, string name, string storage)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("ID cant be empty");
        }

        EqtAssert.StringNotNullOrEmpty(name, nameof(name));
        EqtAssert.ParameterNotNull(storage, nameof(storage));

        Id = id;
        Name = name;
        Storage = storage;
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Gets the storage.
    /// </summary>
    public string Storage { get; } = string.Empty;

    /// <summary>
    /// Whether this Link is equal to other Link. Compares by Id.
    /// </summary>
    public override bool Equals(object? other)
    {
        return other is TestLink link && Id.Equals(link.Id);
    }

    /// <summary>
    /// Whether this Link is exactly the same as other Link. Compares all fields.
    /// </summary>
    public bool IsSame(TestLink other)
    {
        return other != null
               && Id.Equals(other.Id) &&
               Name.Equals(other.Name) &&
               Storage.Equals(other.Storage);
    }

    /// <summary>
    /// Override for GetHashCode.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Override for ToString.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "Link to '{0}' {1} '{2}'.",
            Name ?? "(null)",
            Id.ToString("B"),
            Storage ?? "(null)");
    }

    public void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence h = new();
        h.SaveGuid(element, "@id", Id);
        h.SaveSimpleField(element, "@name", Name, null);
        h.SaveSimpleField(element, "@storage", Storage, string.Empty);
    }
}
