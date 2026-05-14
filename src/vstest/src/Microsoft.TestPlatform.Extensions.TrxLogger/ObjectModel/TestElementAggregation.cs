// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Test aggregation element.
/// </summary>
internal abstract class TestElementAggregation : TestElement, ITestAggregation
{
    protected Dictionary<Guid, TestLink> _testLinks = new();

    public TestElementAggregation(Guid id, string name, string adapter) : base(id, name, adapter) { }

    /// <summary>
    /// Test links.
    /// </summary>
    public Dictionary<Guid, TestLink> TestLinks
    {
        get { return _testLinks; }
    }

    public override void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        base.Save(element, parameters);

        XmlPersistence h = new();
        if (_testLinks.Count > 0)
        {
            h.SaveIEnumerable(_testLinks.Values, element, "TestLinks", ".", "TestLink", parameters);
        }
    }
}
