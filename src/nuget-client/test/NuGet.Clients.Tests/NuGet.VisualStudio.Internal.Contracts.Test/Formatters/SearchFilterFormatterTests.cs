// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class SearchFilterFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(SearchFilter expectedResult)
        {
            SearchFilter? actualResult = SerializeThenDeserialize(SearchFilterFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Filter, actualResult!.Filter);
            Assert.Equal(expectedResult.IncludeDelisted, actualResult.IncludeDelisted);
            Assert.Equal(expectedResult.IncludePrerelease, actualResult.IncludePrerelease);
            Assert.Equal(expectedResult.OrderBy, actualResult.OrderBy);
            Assert.Equal(expectedResult.PackageType, actualResult.PackageType);
            Assert.Equal(expectedResult.SupportedFrameworks, actualResult.SupportedFrameworks);
        }

        public static TheoryData<SearchFilter> TestData => new()
            {
                {
                    new SearchFilter(includePrerelease: true, SearchFilterType.IsAbsoluteLatestVersion)
                    {
                        IncludeDelisted = true,
                        OrderBy = SearchOrderBy.Id,
                        PackageType = "Dependency",
                        SupportedFrameworks = new List<string>() { ".Net451", ".Net452" }
                    }
                },
            };
    }
}
