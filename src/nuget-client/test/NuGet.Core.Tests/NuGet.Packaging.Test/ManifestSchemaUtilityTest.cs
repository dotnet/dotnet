// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class ManifestSchemaUtilityTest
    {
        [Theory]
        [InlineData(new object[] { 0 })]
        [InlineData(new object[] { -1 })]
        [InlineData(new object[] { -5 })]
        public void GetSchemaNamespaceThrowsIfVersionIsNotAPostiveInteger(int version)
        {
            // Act and Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => ManifestSchemaUtility.GetSchemaNamespace(version), "Unknown schema version '" + version + "'.");
        }

        [Theory]
        [InlineData(new object[] { 1, "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd" })]
        [InlineData(new object[] { 2, "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd" })]
        [InlineData(new object[] { 3, "http://schemas.microsoft.com/packaging/2011/10/nuspec.xsd" })]
        [InlineData(new object[] { 4, "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd" })]
        public void GetSchemaNamespaceReturnsRightSchemaVersion(int version, string expectedSchemaNamespace)
        {
            // Act
            string actualSchemaNamespace = ManifestSchemaUtility.GetSchemaNamespace(version);

            // Assert
            Assert.Equal(expectedSchemaNamespace, actualSchemaNamespace);
        }
    }
}
