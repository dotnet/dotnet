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
            Assert.Equal(expectedResult.PackageTypes, actualResult.PackageTypes);
            Assert.Equal(expectedResult.SupportedFrameworks, actualResult.SupportedFrameworks);
        }

        public static TheoryData TestData => new TheoryData<SearchFilter>
            {
                {
                    new SearchFilter(includePrerelease: true, SearchFilterType.IsAbsoluteLatestVersion)
                    {
                        IncludeDelisted = true,
                        OrderBy = SearchOrderBy.Id,
                        PackageTypes = new List<string>() { "packageType1", "packageType2" },
                        SupportedFrameworks = new List<string>() { ".Net451", ".Net452" }
                    }
                },
            };
    }
}
