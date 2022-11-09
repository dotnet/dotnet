// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.Common.Filtering;

/// <summary>
/// Class holds information related to filtering criteria.
/// </summary>
public class FilterExpressionWrapper
{
    /// <summary>
    /// FilterExpression corresponding to filter criteria
    /// </summary>
    private readonly FilterExpression? _filterExpression;

    /// <remarks>
    /// Exposed for testing purpose.
    /// </remarks>
    internal readonly FastFilter? FastFilter;

    [MemberNotNullWhen(true, nameof(FastFilter))]
    private bool UseFastFilter => FastFilter != null;

    /// <summary>
    /// Initializes FilterExpressionWrapper with given filterString and options.
    /// </summary>
    public FilterExpressionWrapper(string filterString, FilterOptions? options)
    {
        ValidateArg.NotNullOrEmpty(filterString, nameof(filterString));

        FilterString = filterString;
        FilterOptions = options;

        try
        {
            // We prefer fast filter when it's available.
            _filterExpression = FilterExpression.Parse(filterString, out FastFilter);

            if (UseFastFilter)
            {
                _filterExpression = null;

                // Property value regex is only supported for fast filter,
                // so we ignore it if no fast filter is constructed.

                // TODO: surface an error message to suer.
                var regexString = options?.FilterRegEx;
                if (!regexString.IsNullOrEmpty())
                {
                    TPDebug.Assert(options!.FilterRegExReplacement == null || options.FilterRegEx != null);
                    FastFilter.PropertyValueRegex = new Regex(regexString, RegexOptions.Compiled);
                    FastFilter.PropertyValueRegexReplacement = options.FilterRegExReplacement;
                }
            }

        }
        catch (FormatException ex)
        {
            ParseError = ex.Message;
        }
        catch (ArgumentException ex)
        {
            FastFilter = null;
            ParseError = ex.Message;
        }
    }

    /// <summary>
    /// Initializes FilterExpressionWrapper with given filterString.
    /// </summary>
    public FilterExpressionWrapper(string filterString)
        : this(filterString, null)
    {
    }

    /// <summary>
    /// User specified filter criteria.
    /// </summary>
    public string FilterString
    {
        get;
        private set;
    }

    /// <summary>
    /// User specified additional filter options.
    /// </summary>
    public FilterOptions? FilterOptions
    {
        get;
        private set;
    }

    /// <summary>
    /// Parsing error (if any), when parsing 'FilterString' with built-in parser.
    /// </summary>
    public string? ParseError
    {
        get;
        private set;
    }

    /// <summary>
    /// Validate if underlying filter expression is valid for given set of supported properties.
    /// </summary>
    public string[]? ValidForProperties(IEnumerable<string>? supportedProperties, Func<string, TestProperty?>? propertyProvider)
        => UseFastFilter
            ? FastFilter.ValidForProperties(supportedProperties)
            : _filterExpression?.ValidForProperties(supportedProperties, propertyProvider);

    /// <summary>
    /// Evaluate filterExpression with given propertyValueProvider.
    /// </summary>
    public bool Evaluate(Func<string, object?> propertyValueProvider)
    {
        ValidateArg.NotNull(propertyValueProvider, nameof(propertyValueProvider));

        return UseFastFilter
            ? FastFilter.Evaluate(propertyValueProvider)
            : _filterExpression != null && _filterExpression.Evaluate(propertyValueProvider);
    }
}
