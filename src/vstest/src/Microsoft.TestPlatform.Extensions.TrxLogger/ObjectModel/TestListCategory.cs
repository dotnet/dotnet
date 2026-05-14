// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// The test list category.
/// </summary>
internal class TestListCategory : IXmlTestStore
{
    private static TestListCategory? s_uncategorizedResults;
    private static TestListCategory? s_allResults;

    private static readonly object ReservedCategoryLock = new();
    [StoreXmlSimpleField("@name", DefaultValue = "")]
    private readonly string _name = string.Empty;

    private TestListCategoryId _parentCategoryId;

    /// <summary>
    /// Constructor for TestListCategory .
    /// </summary>
    /// <param name="name">The name of new category.</param>
    /// <param name="parentCategoryId">Id of parent category. Use TestListCategoryId.Root for top level categories.</param>
    public TestListCategory(string name, TestListCategoryId parentCategoryId)
    {
        EqtAssert.StringNotNullOrEmpty(name, nameof(name));
        EqtAssert.ParameterNotNull(parentCategoryId, nameof(parentCategoryId));

        _name = name;
        _parentCategoryId = parentCategoryId;
    }

    /// <summary>
    /// Used internally for fake uncategorized category.
    /// </summary>
    /// <param name="name">
    /// Category name.
    /// </param>
    /// <param name="id">
    /// Category id.
    /// </param>
    /// <param name="parentId">
    /// The parent Id.
    /// </param>
    private TestListCategory(string name, TestListCategoryId id, TestListCategoryId parentId) : this(name, parentId)
    {
        EqtAssert.ParameterNotNull(id, nameof(id));
        Id = id;
    }

    /// <summary>
    /// Gets the uncategorized results.
    /// </summary>
    public static TestListCategory UncategorizedResults
    {
        get
        {
            if (s_uncategorizedResults == null)
            {
                lock (ReservedCategoryLock)
                {
                    s_uncategorizedResults ??= new TestListCategory(
                            TrxLoggerResources.TS_UncategorizedResults, TestListCategoryId.Uncategorized, TestListCategoryId.Root);
                }
            }

            return s_uncategorizedResults;
        }
    }

    /// <summary>
    /// Gets the all results.
    /// </summary>
    public static TestListCategory AllResults
    {
        get
        {
            if (s_allResults == null)
            {
                lock (ReservedCategoryLock)
                {
                    s_allResults ??= new TestListCategory(
                            TrxLoggerResources.TS_AllResults, TestListCategoryId.AllItems, TestListCategoryId.Root);
                }
            }

            return s_allResults;
        }
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public TestListCategoryId Id { get; } = new TestListCategoryId();

    /// <summary>
    /// Gets or sets id of parent category. Use TestCategoryId.Root for top level categories.
    /// We do not keep category children in Object Model, only parent.
    /// </summary>
    public TestListCategoryId ParentCategoryId
    {
        get
        {
            return _parentCategoryId;
        }

        set
        {
            EqtAssert.ParameterNotNull(value, "ParentCategoryId.value");
            _parentCategoryId = value;
        }
    }


    #region Overrides

    /// <summary>
    /// Override function for Equals.
    /// </summary>
    /// <param name="obj">
    /// The object to compare.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is not TestListCategory cat)
        {
            return false;
        }

        TPDebug.Assert(Id != null, "id is null");
        return Id.Equals(cat.Id);
    }

    /// <summary>
    /// Override function for GetHashCode.
    /// </summary>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    #endregion

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
        XmlPersistence h = new();

        h.SaveSingleFields(element, this, parameters);
        h.SaveGuid(element, "@id", Id.Id);
        h.SaveGuid(element, "@parentListId", ParentCategoryId.Id);
    }

    #endregion
}
