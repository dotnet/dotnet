// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class PackageLevelToGroupNameConverterTests
    {
        public static IEnumerable<object[]> GetConvertData()
        {
            yield return new object[] { new object[] { PackageLevel.TopLevel, 1, 2 }, string.Format(Resources.PackageLevel_TopLevelPackageHeaderText, 1) };
            yield return new object[] { new object[] { PackageLevel.Transitive, 1, 2 }, string.Format(Resources.PackageLevel_TransitivePackageHeaderText, 2) };
            yield return new object[] { new object[] { PackageLevel.TopLevel, 0, 1 }, string.Format(Resources.PackageLevel_TopLevelPackageHeaderText, 0) };
            yield return new object[] { new object[] { PackageLevel.Transitive, 1, 0 }, string.Format(Resources.PackageLevel_TransitivePackageHeaderText, 0) };
            yield return new object[] { new object[] { Resources.PackageLevel_TopLevelPackageHeaderText, 1, 2 }, null };
            yield return new object[] { new object[] { "some string", 1, 2 }, null };
            yield return new object[] { new object[] { PackageLevel.TopLevel, 1 }, null };
            yield return new object[] { new object[] { PackageLevel.Transitive, 1 }, null };
            yield return new object[] { new object[] { new object() }, null };
            yield return new object[] { new object[] { 12345 }, null };
            yield return new object[] { null, null };
        }

        [Theory]
        [MemberData(nameof(GetConvertData))]
        public void Convert_MultipleInputs_Succeeds(object[] input, object expected)
        {
            var converterToTest = new PackageLevelToGroupNameConverter();

            object value = converterToTest.Convert(input, targetType: null, parameter: null, culture: null);

            Assert.Equal(expected, value);
        }
    }
}
