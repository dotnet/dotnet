// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Moq;
using NuGet.PackageManagement.Telemetry;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class ParametersToHyperlinkTupleConverterTests
    {
        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new object[] { }, null };
            yield return new object[] { null, null };
            yield return new object[] { new object[] { "query", HyperlinkType.DeprecationMoreInfo }, Tuple.Create("query", HyperlinkType.DeprecationMoreInfo) };
            yield return new object[] { new object[] { "query", HyperlinkType.ProjectUri, "blah" }, null };
            yield return new object[] { new object[] { 1, 2, 3, 4 }, null };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void ParametersToHyperlinkTupleConverter_HappyPath_Succeeds(object[] input, object expected)
        {
            var converter = new ParametersToHyperlinkTupleConverter();

            object val = converter.Convert(input, typeof(It.IsAnyType), It.IsAny<object>(), It.IsAny<CultureInfo>());

            Assert.Equal(expected, val);
        }
    }
}
