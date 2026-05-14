// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;

/// <summary>
/// Specifies the user specified RunSettings and framework provided context of the discovery.
/// </summary>
public class DiscoveryContext : IDiscoveryContext
{
    /// <summary>
    /// Gets the run settings specified for this request.
    /// </summary>
    public IRunSettings? RunSettings { get; internal set; }

    /// <summary>
    /// Returns TestCaseFilterExpression validated for supportedProperties.
    /// If there is a parsing error in filter expression, TestPlatformFormatException() is thrown.
    /// </summary>
    /// <param name="supportedProperties"> The supported Properties. </param>
    /// <param name="propertyProvider"> The property Provider. </param>
    /// <returns> The <see cref="ITestCaseFilterExpression"/>. </returns>
    public ITestCaseFilterExpression? GetTestCaseFilter(IEnumerable<string>? supportedProperties, Func<string, TestProperty?> propertyProvider)
    {
        TestCaseFilterExpression? adapterSpecificTestCaseFilter = null;
        if (FilterExpressionWrapper == null)
        {
            return adapterSpecificTestCaseFilter;
        }

        if (!StringUtils.IsNullOrEmpty(FilterExpressionWrapper.ParseError))
        {
            throw new TestPlatformFormatException(FilterExpressionWrapper.ParseError, FilterExpressionWrapper.FilterString);
        }

        adapterSpecificTestCaseFilter = new TestCaseFilterExpression(FilterExpressionWrapper);
        var invalidProperties = adapterSpecificTestCaseFilter.ValidForProperties(supportedProperties, propertyProvider);

        if (invalidProperties != null)
        {
            var invalidPropertiesString = string.Join(CrossPlatEngineResources.StringSeperator, invalidProperties);
            var validPropertiesSttring = supportedProperties == null ? string.Empty : string.Join(CrossPlatEngineResources.StringSeperator, supportedProperties.ToArray());
            var errorMessage = string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.UnsupportedPropertiesInTestCaseFilter, invalidPropertiesString, validPropertiesSttring);

            // For unsupported property don’t throw exception, just log the message. Later it is going to handle properly with TestCaseFilterExpression.MatchTestCase().
            EqtTrace.Info(errorMessage);
        }

        return adapterSpecificTestCaseFilter;
    }

    /// <summary>
    /// Gets or sets the FilterExpressionWrapper instance as created from filter string.
    /// </summary>
    internal FilterExpressionWrapper? FilterExpressionWrapper { get; set; }
}
