// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class V2FeedQueryBuilderTests
    {
        /// <summary>
        /// First query made by "nuget.exe list -allversions" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_NoParameters()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: null,
                filters: new SearchFilter(includePrerelease: false, filter: null),
                skip: null,
                take: null);

            // Assert
            Assert.Equal("/Packages?semVerLevel=2.0.0", actual);
        }

        /// <summary>
        /// Second query made by "nuget.exe list -allversions" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_JustSkip()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: null,
                filters: new SearchFilter(includePrerelease: false, filter: null),
                skip: 100,
                take: null);

            // Assert
            Assert.Equal("/Packages?$skip=100&semVerLevel=2.0.0", actual);
        }

        /// <summary>
        /// Query made by "nuget.exe list -allversions -prerelease" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_OrderBySkipAndTop()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: null,
                filters: new SearchFilter(includePrerelease: false, filter: null)
                {
                    OrderBy = SearchOrderBy.Id
                },
                skip: 0,
                take: 30);

            // Assert
            Assert.Equal("/Packages()?$orderby=Id&$skip=0&$top=30&semVerLevel=2.0.0", actual);
        }

        /// <summary>
        /// Query made by "nuget.exe list -prerelease" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_FilterOrderBySkipAndTop_IsAbsoluteLatestVersion()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: null,
                filters: new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id
                },
                skip: 0,
                take: 30);

            // Assert
            Assert.Equal("/Packages()?$filter=IsAbsoluteLatestVersion&$orderby=Id&$skip=0&$top=30&semVerLevel=2.0.0", actual);
        }

        /// <summary>
        /// Query made by "nuget.exe list -prerelease" against an endpoint with no support for Search() or 
        /// IsAbsoluteLatestVersion.
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_FilterOrderBySkipAndTop_IsLatestVersion()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: null,
                filters: new SearchFilter(includePrerelease: false, filter: SearchFilterType.IsLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id
                },
                skip: 0,
                take: 30);

            // Assert
            Assert.Equal("/Packages()?$filter=IsLatestVersion&$orderby=Id&$skip=0&$top=30&semVerLevel=2.0.0", actual);
        }

        /// <summary>
        /// Query made by "nuget.exe list nuget" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_FilterSearchTerms()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: "nuget",
                filters: new SearchFilter(includePrerelease: false, filter: null),
                skip: null,
                take: null);

            // Assert
            Assert.Equal(
                "/Packages()?$filter=(((Id%20ne%20null)%20and%20substringof('nuget',tolower(Id)))%20or%20" +
                "((Description%20ne%20null)%20and%20substringof('nuget',tolower(Description))))%20or%20" +
                "((Tags%20ne%20null)%20and%20substringof('%20nuget%20',tolower(Tags)))&semVerLevel=2.0.0",
                actual);
        }

        /// <summary>
        /// Query made by "nuget.exe list nuget -prerelease" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_FilterSearchTermsAndFilter()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: "nuget",
                filters: new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id
                },
                skip: 0,
                take: 30);

            // Assert
            Assert.Equal(
                "/Packages()?$filter=((((Id%20ne%20null)%20and%20substringof('nuget',tolower(Id)))%20or%20" +
                "((Description%20ne%20null)%20and%20substringof('nuget',tolower(Description))))%20or%20" +
                "((Tags%20ne%20null)%20and%20substringof('%20nuget%20',tolower(Tags))))%20and%20" +
                "IsAbsoluteLatestVersion&$orderby=Id&$skip=0&$top=30&semVerLevel=2.0.0",
                actual);
        }

        /// <summary>
        /// Query made by "nuget.exe list "foo bar baz" -prerelease" against an endpoint with no support for Search().
        /// </summary>
        [Fact]
        public void BuildGetPackagesUri_MultipleSearchTerms()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildGetPackagesUri(
                searchTerm: "foo bar baz",
                filters: new SearchFilter(includePrerelease: false, filter: null),
                skip: null,
                take: null);

            // Assert
            Assert.Equal(
                "/Packages()?$filter=(((((((((Id%20ne%20null)%20and%20substringof('foo',tolower(Id)))%20or%20" +
                "((Description%20ne%20null)%20and%20substringof('foo',tolower(Description))))%20or%20" +
                "((Tags%20ne%20null)%20and%20substringof('%20foo%20',tolower(Tags))))%20or%20((Id%20ne%20null)" +
                "%20and%20substringof('bar',tolower(Id))))%20or%20((Description%20ne%20null)%20and%20" +
                "substringof('bar',tolower(Description))))%20or%20((Tags%20ne%20null)%20and%20" +
                "substringof('%20bar%20',tolower(Tags))))%20or%20((Id%20ne%20null)%20and%20" +
                "substringof('baz',tolower(Id))))%20or%20((Description%20ne%20null)%20and%20" +
                "substringof('baz',tolower(Description))))%20or%20((Tags%20ne%20null)%20and%20" +
                "substringof('%20baz%20',tolower(Tags)))&semVerLevel=2.0.0",
                actual);
        }

        [Fact]
        public void BuildFindPackagesByIdUri_KeepsOriginalCase()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildFindPackagesByIdUri("Newtonsoft.Json");

            // Assert
            Assert.Equal("/FindPackagesById()?id='Newtonsoft.Json'&semVerLevel=2.0.0", actual);
        }

        [Fact]
        public void BuildFindPackagesByIdUri_EscapesId()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildFindPackagesByIdUri("foo! bar/ baz?");

            // Assert
            Assert.Equal("/FindPackagesById()?id='foo%21%20bar%2F%20baz%3F'&semVerLevel=2.0.0", actual);
        }

        [Fact]
        public void BuildGetPackageUri_RejectsNullPackageIdentity()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act & Assert
            var actual = Assert.Throws<ArgumentNullException>(() => target.BuildGetPackageUri(package: null));
            Assert.Equal("package", actual.ParamName);
        }

        [Fact]
        public void BuildGetPackageUri_RejectsNullPackageVersion()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();
            var package = new PackageIdentity("Newtonsoft.Json", version: null);

            // Act & Assert
            var actual = Assert.Throws<ArgumentException>(() => target.BuildGetPackageUri(package));
            Assert.Equal("Version", actual.Message);
        }

        [Fact]
        public void BuildGetPackageUri()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();
            var package = new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("9.0.01-BETA2"));

            // Act
            var actual = target.BuildGetPackageUri(package);

            // Assert
            Assert.Equal("/Packages(Id='Newtonsoft.Json',Version='9.0.1-BETA2')", actual);
        }

        [Fact]
        public void BuildSearchUri_AllParameters()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildSearchUri(
                searchTerm: "foo! bar/ baz?",
                filters: new SearchFilter(includePrerelease: true)
                {
                    OrderBy = SearchOrderBy.Id,
                    SupportedFrameworks = new[]
                    {
                        "net45",
                        "netcoreapp1.0"
                    }
                },
                skip: 0,
                take: 30);

            // Assert
            Assert.Equal(
                "/Search()?$filter=IsAbsoluteLatestVersion&$orderby=Id&searchTerm='foo%21%20bar%2F%20baz%3F'&" +
                "targetFramework='net45%7Cnetcoreapp1.0'&includePrerelease=true&$skip=0&$top=30&semVerLevel=2.0.0",
                actual);
        }

        [Fact]
        public void BuildSearchUri_NoOrderByFilterOrTargetFrameworks()
        {
            // Arrange
            var target = new V2FeedQueryBuilder();

            // Act
            var actual = target.BuildSearchUri(
                searchTerm: "foo! bar/ baz?",
                filters: new SearchFilter(includePrerelease: true, filter: null),
                skip: 0,
                take: 30);

            // Assert
            Assert.Equal(
                "/Search()?searchTerm='foo%21%20bar%2F%20baz%3F'&targetFramework=''" +
                "&includePrerelease=true&$skip=0&$top=30&semVerLevel=2.0.0",
                actual);
        }
    }
}
