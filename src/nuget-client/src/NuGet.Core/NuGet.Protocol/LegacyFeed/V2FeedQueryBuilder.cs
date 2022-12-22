// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Build the path part of a V2 feed URL. These values are appended to the V2 base URL.
    /// </summary>
    public class V2FeedQueryBuilder
    {
        // shared constants
        private const string IsLatestVersionFilterFlag = "IsLatestVersion";
        private const string IsAbsoluteLatestVersionFilterFlag = "IsAbsoluteLatestVersion";
        private const string IdProperty = "Id";
        private const string SemVerLevel = "semVerLevel=2.0.0";

        // constants for /Packages(ID,VERSION) endpoint
        private const string GetSpecificPackageFormat = "/Packages(Id='{0}',Version='{1}')";

        // constants for /Search() endpoint
        private const string SearchEndpointFormat = "/Search()?{0}{1}searchTerm='{2}'&targetFramework='{3}'&includePrerelease={4}&$skip={5}&$top={6}&" + SemVerLevel;
        private const string QueryDelimiter = "&";

        // constants for /FindPackagesById() endpoint
        private const string FindPackagesByIdFormat = "/FindPackagesById()?id='{0}'&" + SemVerLevel;

        // constants for /Packages() endpoint
        private const string GetPackagesFormat = "/Packages{0}";
        private const string EndpointParenthesis = "()";
        private const string SearchClauseFormat = "({0}%20ne%20null)%20and%20substringof('{1}',tolower({0}))";
        private const string OrFormat = "({0})%20or%20({1})";
        private const string AndFormat = "({0})%20and%20{1}";
        private const string FilterFormat = "$filter={0}";
        private const string OrderByFormat = "$orderby={0}";
        private const string SkipFormat = "$skip={0}";
        private const string TopFormat = "$top={0}";
        private const string TagTermFormat = " {0} ";
        private const string FirstParameterFormat = "?{0}";
        private const string ParameterFormat = "&{0}";
        private const string TagsProperty = "Tags";
        private static readonly string[] _propertiesToSearch = new[]
        {
            IdProperty,
            "Description",
            TagsProperty
        };

        public string BuildSearchUri(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take)
        {
            var shortFormTargetFramework = string.Join(
                "|",
                filters
                    .SupportedFrameworks
                    .Select(targetFramework => NuGetFramework.Parse(targetFramework).GetShortFolderName()));

            var orderBy = BuildOrderBy(filters.OrderBy);

            var filter = BuildPropertyFilter(filters.Filter);
            if (filter != null)
            {
                filter = string.Format(
                    CultureInfo.InvariantCulture,
                    FilterFormat,
                    filter);
            }

            var uri = string.Format(
                CultureInfo.InvariantCulture,
                SearchEndpointFormat,
                filter != null ? filter + QueryDelimiter : string.Empty,
                orderBy != null ? orderBy + QueryDelimiter : string.Empty,
                UriUtility.UrlEncodeOdataParameter(searchTerm),
                UriUtility.UrlEncodeOdataParameter(shortFormTargetFramework),
                filters.IncludePrerelease.ToString(CultureInfo.CurrentCulture).ToLowerInvariant(),
                skip,
                take);

            return uri;
        }

        public string BuildFindPackagesByIdUri(string id)
        {
            var uri = string.Format(
                CultureInfo.InvariantCulture,
                FindPackagesByIdFormat,
                UriUtility.UrlEncodeOdataParameter(id));

            return uri;
        }

        public string BuildGetPackageUri(PackageIdentity package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (!package.HasVersion)
            {
                throw new ArgumentException(nameof(package.Version));
            }

            var uri = string.Format(
                CultureInfo.InvariantCulture,
                GetSpecificPackageFormat,
                UriUtility.UrlEncodeOdataParameter(package.Id),
                UriUtility.UrlEncodeOdataParameter(package.Version.ToNormalizedString()));

            return uri;
        }

        public string BuildGetPackagesUri(
            string searchTerm,
            SearchFilter filters,
            int? skip,
            int? take)
        {
            var filterParameter = BuildFilter(searchTerm, filters.Filter);
            var orderByParameter = BuildOrderBy(filters.OrderBy);
            var skipParameter = BuildSkip(skip);
            var topParameter = BuildTop(take);

            // The parenthesis right after the "/Packages" path in the URL are excluded if the filter, orderby, and
            // top parameters are not used. This is a quirk of the NuGet 2.x implementation.
            var useParenthesis = filterParameter != null || orderByParameter != null || topParameter != null;

            // Start building the URI.
            var builder = new StringBuilder();

            builder.AppendFormat(CultureInfo.InvariantCulture, GetPackagesFormat, useParenthesis ? EndpointParenthesis : string.Empty);

            var hasParameters = false;

            // Append each query parameter.
            if (filterParameter != null)
            {
                builder.AppendFormat(
                    CultureInfo.CurrentCulture,
                    hasParameters ? ParameterFormat : FirstParameterFormat,
                    filterParameter);
                hasParameters = true;
            }

            if (orderByParameter != null)
            {
                builder.AppendFormat(
                    CultureInfo.CurrentCulture,
                    hasParameters ? ParameterFormat : FirstParameterFormat,
                    orderByParameter);
                hasParameters = true;
            }

            if (skipParameter != null)
            {
                builder.AppendFormat(
                    CultureInfo.CurrentCulture,
                    hasParameters ? ParameterFormat : FirstParameterFormat,
                    skipParameter);
                hasParameters = true;
            }

            if (topParameter != null)
            {
                builder.AppendFormat(
                    CultureInfo.CurrentCulture,
                    hasParameters ? ParameterFormat : FirstParameterFormat,
                    topParameter);
                hasParameters = true;
            }

            builder.AppendFormat(
                CultureInfo.CurrentCulture,
                hasParameters ? ParameterFormat : FirstParameterFormat,
                SemVerLevel);
            hasParameters = true;

            return builder.ToString();
        }

        private string BuildTop(int? top)
        {
            if (!top.HasValue)
            {
                return null;
            }

            return string.Format(CultureInfo.InvariantCulture, TopFormat, top);
        }

        private string BuildSkip(int? skip)
        {
            if (!skip.HasValue)
            {
                return null;
            }

            return string.Format(CultureInfo.InvariantCulture, SkipFormat, skip);
        }

        private string BuildFilter(string searchTerm, SearchFilterType? searchFilterType)
        {
            var pieces = new List<string>
            {
                BuildFieldSearchFilter(searchTerm),
                BuildPropertyFilter(searchFilterType)
            }.AsEnumerable();

            pieces = pieces.Where(p => p != null);

            if (!pieces.Any())
            {
                return null;
            }

            var filter = pieces
                .Aggregate((a, b) => string.Format(CultureInfo.InvariantCulture, AndFormat, a, b));

            return string.Format(CultureInfo.InvariantCulture, FilterFormat, filter);
        }

        private string BuildOrderBy(SearchOrderBy? searchOrderBy)
        {
            string orderBy;
            switch (searchOrderBy)
            {
                case SearchOrderBy.Id:
                    orderBy = IdProperty;
                    break;
                case null:
                    orderBy = null;
                    break;
                default:
                    Debug.Fail("Unhandled value of SearchFilterType");
                    orderBy = null;
                    break;
            }

            if (orderBy != null)
            {
                orderBy = string.Format(CultureInfo.InvariantCulture, OrderByFormat, orderBy);
            }

            return orderBy;
        }

        private string BuildPropertyFilter(SearchFilterType? searchFilterType)
        {
            string filter;
            switch (searchFilterType)
            {
                case SearchFilterType.IsLatestVersion:
                    filter = IsLatestVersionFilterFlag;
                    break;
                case SearchFilterType.IsAbsoluteLatestVersion:
                    filter = IsAbsoluteLatestVersionFilterFlag;
                    break;
                case null:
                    filter = null;
                    break;
                default:
                    Debug.Fail("Unhandled value of SearchFilterType");
                    filter = null;
                    break;
            }

            return filter;
        }

        private string BuildFieldSearchFilter(string searchTerm)
        {
            if (searchTerm == null)
            {
                return null;
            }

            var searchTerms = searchTerm.Split();

            var clauses =
                from term in searchTerms
                from property in _propertiesToSearch
                select BuildFieldSearchClause(term, property);

            var fieldSearch = clauses
                .Aggregate((a, b) => string.Format(CultureInfo.InvariantCulture, OrFormat, a, b));

            return fieldSearch;
        }

        private string BuildFieldSearchClause(string term, string property)
        {
            if (property == TagsProperty)
            {
                term = string.Format(CultureInfo.InvariantCulture, TagTermFormat, term);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                SearchClauseFormat,
                property,
                UriUtility.UrlEncodeOdataParameter(term));
        }
    }
}
