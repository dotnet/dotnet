// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.ApiCompat.Tests
{
    public class AttributeDifferenceTests
    {
        private readonly string _implementationPath = Path.Combine(AppContext.BaseDirectory, "Implementation", "AttributeDifference.dll");
        private readonly string _contractPath = Path.Combine(AppContext.BaseDirectory, "Contract");

        [Theory]
        [InlineData(ApiCompatFrontend.Console)]
        [InlineData(ApiCompatFrontend.MSBuildTask)]
        public void AttributeDifferenceIsFound(ApiCompatFrontend frontend)
        {
            string runOutput = Helpers.RunApiCompat(_implementationPath, _contractPath, "implementation", "contract", frontend);

            Assert.Contains("CannotRemoveAttribute : Attribute 'System.ComponentModel.DesignerAttribute' exists on 'AttributeDifference.AttributeDifferenceClass1' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'AttributeDifference.FooAttribute' exists on 'AttributeDifference.AttributeDifferenceClass1' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'AttributeDifference.FooAttribute' exists on 'AttributeDifference.AttributeDifferenceClass1.PropertyWithAttribute' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'AttributeDifference.FooAttribute' exists on 'AttributeDifference.AttributeDifferenceClass1.EventWithAttribute' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'System.ComponentModel.DefaultValueAttribute' exists on generic param 'T' on member 'AttributeDifference.AttributeDifferenceClass1.GenericMethodWithAttribute<T>()' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'System.ComponentModel.DefaultValueAttribute' exists on generic param 'T' on member 'AttributeDifference.AttributeDifferenceClass1.GenericMethodWithAttribute<T>()' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'AttributeDifference.FooAttribute' exists on 'AttributeDifference.AttributeDifferenceClass1.MethodWithAttribute()' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'AttributeDifference.FooAttribute' exists on parameter 'myParameter' on member 'AttributeDifference.AttributeDifferenceClass1.MethodWithAttribute(System.String, System.Object)' in the implementation but not the contract.", runOutput);
            Assert.Contains("CannotRemoveAttribute : Attribute 'System.ComponentModel.DefaultValueAttribute' exists on generic param 'TOne' on member 'AttributeDifference.AttributeDifferenceGenericCLass<TOne, TTwo>' in the implementation but not the contract.", runOutput);
            Assert.Contains("Total Issues: 8", runOutput);
        }

        [Theory]
        [InlineData(ApiCompatFrontend.Console)]
        [InlineData(ApiCompatFrontend.MSBuildTask)]
        public void AttributeDifferenceIsFoundWithExcludeAttributesFile(ApiCompatFrontend frontend)
        {
            using TempFile excludeAttributesFile = TempFile.Create();

            File.WriteAllLines(excludeAttributesFile.Path, new string[] { "T:System.ComponentModel.DisplayNameAttribute", "T:AttributeDifference.FooAttribute" });

            string runOutput = Helpers.RunApiCompat(_implementationPath, new string[] { _contractPath }, new string[] { excludeAttributesFile.Path }, "implementation", "contract", frontend);
            Assert.Contains("CannotRemoveAttribute : Attribute 'System.ComponentModel.DesignerAttribute' exists on 'AttributeDifference.AttributeDifferenceClass1' in the implementation but not the contract.", runOutput);
            Assert.Contains("Total Issues: 3", runOutput);
        }

        [Theory]
        [InlineData(ApiCompatFrontend.Console)]
        [InlineData(ApiCompatFrontend.MSBuildTask)]
        public void NoIssuesWithExcludeAttributesFile(ApiCompatFrontend frontend)
        {
            using TempFile excludeAttributesFile = TempFile.Create();

            File.WriteAllLines(excludeAttributesFile.Path, new string[] { "T:System.ComponentModel.DesignerAttribute", "T:AttributeDifference.FooAttribute", "T:System.ComponentModel.DefaultValueAttribute" });

            string runOutput = Helpers.RunApiCompat(_implementationPath, new string[] { _contractPath }, new string[] { excludeAttributesFile.Path }, null, null, frontend);

            Assert.Contains("Total Issues: 0", runOutput);
        }
    }
}
