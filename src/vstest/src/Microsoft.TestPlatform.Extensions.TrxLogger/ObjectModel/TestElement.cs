// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Test element.
/// </summary>
internal abstract class TestElement : ITestElement, IXmlTestStore
{
    /// <summary>
    /// Default priority for a test method that does not specify a priority
    /// </summary>
    protected const int DefaultPriority = int.MaxValue;

    protected TestId _id;
    protected string _name;
    protected string _owner;
    protected string _storage;
    protected string _adapter;
    protected int _priority;
    protected bool _isRunnable;
    protected TestExecId _executionId;
    protected TestExecId _parentExecutionId;
    protected TestCategoryItemCollection _testCategories;
    protected WorkItemCollection _workItems;
    protected TestListCategoryId _catId;

    public TestElement(Guid id, string name, string adapter)
    {
        TPDebug.Assert(!name.IsNullOrEmpty(), "name is null");
        TPDebug.Assert(!adapter.IsNullOrEmpty(), "adapter is null");

        _owner = string.Empty;
        _priority = DefaultPriority;
        _storage = string.Empty;
        _executionId = TestExecId.Empty;
        _parentExecutionId = TestExecId.Empty;
        _testCategories = new TestCategoryItemCollection();
        _workItems = new WorkItemCollection();
        _isRunnable = true;
        _catId = TestListCategoryId.Uncategorized;

        _id = new TestId(id);
        _name = name;
        _adapter = adapter;
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public TestId Id
    {
        get { return _id; }
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name
    {
        get { return _name; }

        set
        {
            EqtAssert.ParameterNotNull(value, "Name");
            _name = value;
        }
    }

    /// <summary>
    /// Gets or sets the owner.
    /// </summary>
    public string Owner
    {
        get { return _owner; }

        set
        {
            EqtAssert.ParameterNotNull(value, "Owner");
            _owner = value;
        }
    }

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    public int Priority
    {
        get { return _priority; }
        set { _priority = value; }
    }

    /// <summary>
    /// Gets or sets the storage.
    /// </summary>
    public string Storage
    {
        get { return _storage; }

        set
        {
            EqtAssert.StringNotNullOrEmpty(value, "Storage");
            _storage = value.ToLowerInvariant();
        }
    }

    /// <summary>
    /// Gets or sets the execution id.
    /// </summary>
    public TestExecId ExecutionId
    {
        get { return _executionId; }
        set { _executionId = value; }
    }

    /// <summary>
    /// Gets or sets the parent execution id.
    /// </summary>
    public TestExecId ParentExecutionId
    {
        get { return _parentExecutionId; }
        set { _parentExecutionId = value; }
    }

    /// <summary>
    /// Gets the isRunnable value.
    /// </summary>
    public bool IsRunnable
    {
        get { return _isRunnable; }
    }

    /// <summary>
    /// Gets or sets the category id.
    /// </summary>
    /// <remarks>
    /// Instead of setting to null use TestListCategoryId.Uncategorized
    /// </remarks>
    public TestListCategoryId CategoryId
    {
        get { return _catId; }

        set
        {
            EqtAssert.ParameterNotNull(value, "CategoryId");
            _catId = value;
        }
    }

    /// <summary>
    /// Gets or sets the test categories.
    /// </summary>
    public TestCategoryItemCollection TestCategories
    {
        get { return _testCategories; }

        set
        {
            EqtAssert.ParameterNotNull(value, "value");
            _testCategories = value;
        }
    }

    /// <summary>
    /// Gets or sets the work items.
    /// </summary>
    public WorkItemCollection WorkItems
    {
        get { return _workItems; }

        set
        {
            EqtAssert.ParameterNotNull(value, "value");
            _workItems = value;
        }
    }

    /// <summary>
    /// Gets the adapter name.
    /// </summary>
    public string Adapter
    {
        get { return _adapter; }
    }

    /// <summary>
    /// Gets the test type.
    /// </summary>
    public abstract TestType TestType { get; }

    /// <summary>
    /// Override for ToString.
    /// </summary>
    /// <returns>String representation of test element.</returns>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "'{0}' {1}",
            _name ?? TrxLoggerResources.Common_NullInMessages,
            _id != null ? _id.ToString() : TrxLoggerResources.Common_NullInMessages);
    }

    /// <summary>
    /// Override for Equals.
    /// </summary>
    /// <param name="other">
    /// The object to compare.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override bool Equals(object? other)
    {
        return other is TestElement otherTest && _id.Equals(otherTest._id);
    }

    /// <summary>
    /// Override for GetHashCode
    /// </summary>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public virtual void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence h = new();

        h.SaveSimpleField(element, "@name", _name, null);
        h.SaveSimpleField(element, "@storage", _storage, string.Empty);
        h.SaveSimpleField(element, "@priority", _priority, DefaultPriority);
        h.SaveSimpleField(element, "Owners/Owner/@name", _owner, string.Empty);
        h.SaveObject(_testCategories, element, "TestCategory", parameters);

        if (_executionId != null)
            h.SaveGuid(element, "Execution/@id", _executionId.Id);
        if (_parentExecutionId != null)
            h.SaveGuid(element, "Execution/@parentId", _parentExecutionId.Id);

        h.SaveObject(_workItems, element, "Workitems", parameters);

        XmlTestStoreParameters testIdParameters = XmlTestStoreParameters.GetParameters();
        testIdParameters[TestId.IdLocationKey] = "@id";
        XmlPersistence.SaveObject(_id, element, testIdParameters);
    }
}
