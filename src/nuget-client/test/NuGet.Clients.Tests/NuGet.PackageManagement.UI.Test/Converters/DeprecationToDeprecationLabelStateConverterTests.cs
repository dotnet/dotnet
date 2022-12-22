// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class DeprecationToDeprecationLabelStateConverterTests
    {
        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { null, PackageItemDeprecationLabelState.Invisible };

            var deprecation = new PackageDeprecationMetadataContextInfo("deprecated", new List<string> { "old APIs" }, alternatePackageContextInfo: null);
            yield return new object[] { deprecation, PackageItemDeprecationLabelState.Deprecation };

            var alternative = new PackageDeprecationMetadataContextInfo("deprecated", new List<string> { "old APIs" }, alternatePackageContextInfo: new AlternatePackageMetadataContextInfo("alternatePackage", VersionRange.Parse("[1.0, 2.0)")));
            yield return new object[] { alternative, PackageItemDeprecationLabelState.AlternativeAvailable };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void DeprecationToDeprecationLabelStateConverter_MultipleCases_Succeeds(PackageDeprecationMetadataContextInfo input, PackageItemDeprecationLabelState expected)
        {
            var converter = new DeprecationToDeprecationLabelStateConverter();

            object value = converter.Convert(input, targetType: null, parameter: null, culture: null);

            Assert.IsType(typeof(PackageItemDeprecationLabelState), value);
            Assert.Equal(expected, value);
        }
    }
}
