// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Unit test element.
/// </summary>
internal class UnitTestElement : TestElement, IXmlTestStoreCustom
{
    private string? _codeBase;

    public UnitTestElement(
        Guid id,
        string name,
        string adapter,
        TestMethod testMethod) : base(id, name, adapter)
    {
        TPDebug.Assert(!string.IsNullOrEmpty(adapter), "adapter is null");
        TPDebug.Assert(testMethod != null, "testMethod is null");
        TPDebug.Assert(testMethod != null && testMethod.ClassName != null, "className is null");

        TestMethod = testMethod;
    }

    string IXmlTestStoreCustom.ElementName
    {
        get { return Constants.UnitTestElementName; }
    }

    string? IXmlTestStoreCustom.NamespaceUri
    {
        get { return null; }
    }

    /// <summary>
    /// Gets the test type.
    /// </summary>
    public override TestType TestType
    {
        get { return Constants.UnitTestType; }
    }

    /// <summary>
    /// Gets the test method.
    /// </summary>
    public TestMethod TestMethod { get; }

    /// <summary>
    /// Gets or sets the storage.
    /// </summary>
    public string? CodeBase
    {
        get { return _codeBase; }

        set
        {
            EqtAssert.StringNotNullOrEmpty(value, "CodeBase");
            _codeBase = value;
        }
    }

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameter
    /// </param>
    public override void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        base.Save(element, parameters);
        XmlPersistence h = new();

        h.SaveSimpleField(element, "TestMethod/@codeBase", _codeBase, string.Empty);
        h.SaveSimpleField(element, "TestMethod/@adapterTypeName", _adapter, string.Empty);
        h.SaveObject(TestMethod, element, "TestMethod", parameters);
    }
}
