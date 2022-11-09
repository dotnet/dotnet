// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Microsoft.VisualStudio.TestPlatform.Common.Filtering;

/// <summary>
/// Implements ITestCaseFilterExpression, providing test case filtering functionality.
/// </summary>
public class TestCaseFilterExpression : ITestCaseFilterExpression
{
    private readonly FilterExpressionWrapper _filterWrapper;

    /// <summary>
    /// If filter Expression is valid for performing TestCase matching
    /// (has only supported properties, syntax etc)
    /// </summary>
    private readonly bool _validForMatch;


    /// <summary>
    /// Adapter specific filter expression.
    /// </summary>
    public TestCaseFilterExpression(FilterExpressionWrapper filterWrapper)
    {
        _filterWrapper = filterWrapper ?? throw new ArgumentNullException(nameof(filterWrapper));
        _validForMatch = filterWrapper.ParseError.IsNullOrEmpty();
    }

    /// <summary>
    /// User specified filter criteria.
    /// </summary>
    public string TestCaseFilterValue
    {
        get
        {
            return _filterWrapper.FilterString;
        }
    }

    /// <summary>
    /// Validate if underlying filter expression is valid for given set of supported properties.
    /// </summary>
    public string[]? ValidForProperties(IEnumerable<string>? supportedProperties, Func<string, TestProperty?> propertyProvider)
    {
        string[]? invalidProperties = null;
        if (null != _filterWrapper && _validForMatch)
        {
            invalidProperties = _filterWrapper.ValidForProperties(supportedProperties, propertyProvider);
        }
        return invalidProperties;
    }

    /// <summary>
    /// Match test case with filter criteria.
    /// </summary>
    public bool MatchTestCase(TestCase testCase, Func<string, object?> propertyValueProvider)
    {
        ValidateArg.NotNull(testCase, nameof(testCase));
        ValidateArg.NotNull(propertyValueProvider, nameof(propertyValueProvider));

        if (!_validForMatch)
        {
            return false;
        }

        if (null == _filterWrapper)
        {
            // can be null when parsing error occurs. Invalid filter results in no match.
            return false;
        }

        return _filterWrapper.Evaluate(propertyValueProvider);
    }

}
