// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root flicense information.

using Xunit;

namespace NuGet.Packaging.Licenses.Test
{
    public class NuGetLicenseTests
    {
        [Theory]
        [InlineData("MIT++", true)] // standard license++
        [InlineData("MIT++", false)] // standard license++
        [InlineData("RandomLicense++", true)] // non standard license ++
        [InlineData("RandomLicense++", false)] // non standard license ++
        [InlineData("389-exception", true)] // exception
        [InlineData("389-exception", false)] // exception
        [InlineData("LicenseWith Bad Characters", true)]
        [InlineData("LicenseWith Bad Characters", false)]
        [InlineData("GFDL-1.1", true)] // deprecatated license id
        [InlineData("GFDL-1.1", false)] // deprecatated license id
        [InlineData("UNLICENSED", false)] // unlicensed is not allowed
        [InlineData("UNLICENSED+", true)] // unlicensed+ is bad in both cases
        [InlineData("UNLICENSED+", false)] // unlicensed+ is bad in both cases
        [InlineData("UNLICENSED++", true)] // unlicensed++ is bad in both bases.
        [InlineData("UNLICENSED++", false)] // unlicensed++ is bad in both bases.
        public void NuGetLicenseParser_ThrowsForInvalidLicenseIdentifiers(string expression, bool allowUnlicensed)
        {
            Assert.Throws<NuGetLicenseExpressionParsingException>(() => NuGetLicense.ParseIdentifier(expression, allowUnlicensed: allowUnlicensed));
        }

        [Theory]
        [InlineData("MIT", true, false, true)]
        [InlineData("MIT", true, false, false)]
        [InlineData("AFL-1.1+", true, true, true)]
        [InlineData("AFL-1.1+", true, true, false)]
        [InlineData("MyFancyLicense", false, false, true)]
        [InlineData("MyFancyLicense", false, false, false)]
        [InlineData("MyFancyLicense+", false, true, true)]
        [InlineData("MyFancyLicense+", false, true, false)]
        [InlineData("UNLICENSED", true, false, true)]
        public void NuGetLicenseParser_ParsesLicensesCorrectly(string expression, bool hasStandardIdentifiers, bool hasPlus, bool allowUnlicensed)
        {
            var license = NuGetLicense.ParseIdentifier(expression, allowUnlicensed: allowUnlicensed);
            Assert.Equal(expression, license.ToString());
            Assert.Equal(license.HasOnlyStandardIdentifiers(), hasStandardIdentifiers);
            Assert.Equal(hasPlus, license.Plus);
            Assert.Equal(expression.Equals(NuGetLicense.UNLICENSED), license.IsUnlicensed());
        }

        [Fact]
        public void NuGetLicenseParser_ParsesUnlicensedCorrectly()
        {
            var expression = NuGetLicense.UNLICENSED;
            var license = NuGetLicense.ParseIdentifier(expression, allowUnlicensed: true);
            Assert.Equal(expression, license.ToString());
            Assert.True(license.HasOnlyStandardIdentifiers());
            Assert.True(license.IsUnlicensed());
            Assert.False(license.Plus);
        }
    }
}
