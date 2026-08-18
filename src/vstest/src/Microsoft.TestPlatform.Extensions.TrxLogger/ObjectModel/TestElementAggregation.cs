// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Test aggregation element.
/// </summary>
internal abstract class TestElementAggregation : TestElement, ITestAggregation
{
    /// <summary>
    /// Guards <see cref="_testLinks"/>, which is mutated from multiple threads when test results
    /// are reported concurrently during parallel execution. A lock is used instead of a
    /// <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}"/> so that the
    /// insertion order of the test links, which is meaningful for ordered tests, is preserved.
    /// </summary>
    private readonly object _testLinksLock = new();

    private readonly Dictionary<Guid, TestLink> _testLinks = new();

    public TestElementAggregation(Guid id, string name, string adapter) : base(id, name, adapter) { }

    /// <summary>
    /// Gets a snapshot of the test links.
    /// </summary>
    public IReadOnlyDictionary<Guid, TestLink> TestLinks
    {
        get
        {
            lock (_testLinksLock)
            {
                return new Dictionary<Guid, TestLink>(_testLinks);
            }
        }
    }

    /// <summary>
    /// Adds a test link, unless a link with the same id was already added.
    /// </summary>
    public void AddTestLink(TestLink testLink)
    {
        lock (_testLinksLock)
        {
            if (!_testLinks.ContainsKey(testLink.Id))
            {
                _testLinks.Add(testLink.Id, testLink);
            }
        }
    }

    public override void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        base.Save(element, parameters);

        List<TestLink> testLinks;
        lock (_testLinksLock)
        {
            testLinks = _testLinks.Values.ToList();
        }

        XmlPersistence h = new();
        if (testLinks.Count > 0)
        {
            h.SaveIEnumerable(testLinks, element, "TestLinks", ".", "TestLink", parameters);
        }
    }
}
