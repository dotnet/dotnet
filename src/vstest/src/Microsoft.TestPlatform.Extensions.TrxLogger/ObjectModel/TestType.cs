// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Class identifying test type.
/// </summary>
internal sealed class TestType : IXmlTestStore
{
    [StoreXmlSimpleField(".")]
    private readonly Guid _typeId;

    public TestType(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        _typeId = id;
    }

    public Guid Id
    {
        get { return _typeId; }
    }

    public override bool Equals(object? obj)
    {
        return obj is TestType tt && _typeId.Equals(tt._typeId);
    }


    public override int GetHashCode()
    {
        return _typeId.GetHashCode();
    }

    #region IXmlTestStore Members

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameter
    /// </param>
    public void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence.SaveUsingReflection(element, this, null, parameters);
    }

    #endregion
}
