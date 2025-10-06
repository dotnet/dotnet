// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.ContentModel;
using Xunit;

namespace NuGet.Packaging.Test.ContentModelTests
{
    // These tests violates closed box testing.
    // They are testing the implementation in addition tot he contract,
    // This is needed because the actual internals of the implementation are what reduces allocations and improves the performance.
    public class ContentItemTests
    {
        [Fact]
        public void Add_WithPropertiesNotInitialized__WithAssembly_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.ManagedAssembly, value);
            // Assert
            contentItem._assembly.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithTfm_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.TargetFrameworkMoniker, value);
            // Assert
            contentItem._tfm.Should().Be(value);
            contentItem._properties.Should().BeNull();
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithRuntimeIdentifier_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.RuntimeIdentifier, value);
            // Assert
            contentItem._rid.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithAny_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.AnyValue, value);
            // Assert
            contentItem._any.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithLocale_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.Locale, value);
            // Assert
            contentItem._locale.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithMsbuild_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.MSBuild, value);
            // Assert
            contentItem._msbuild.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithSatelliteAssembly_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.SatelliteAssembly, value);
            // Assert
            contentItem._satelliteAssembly.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithCodeLanguage_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.CodeLanguage, value);
            // Assert
            contentItem._codeLanguage.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithRelated_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.Related, value);
            // Assert
            contentItem._related.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void Add_WithPropertiesNotInitialized__WithTfmRaw_SetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add(ContentItem.TfmRaw, value);
            // Assert
            contentItem._tfmRaw.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }


        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithAssembly_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._assembly = value;
            // Assert
            contentItem.TryGetValue(ContentItem.ManagedAssembly, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithTfm_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._tfm = value;
            // Assert
            contentItem.TryGetValue(ContentItem.TargetFrameworkMoniker, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithRuntimeIdentifier_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._rid = value;
            // Assert
            contentItem.TryGetValue(ContentItem.RuntimeIdentifier, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithAny_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._any = value;
            // Assert
            contentItem.TryGetValue(ContentItem.AnyValue, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithLocale_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._locale = value;
            // Assert
            contentItem.TryGetValue(ContentItem.Locale, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithMsbuild_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._msbuild = value;
            // Assert
            contentItem.TryGetValue(ContentItem.MSBuild, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithSatelliteAssembly_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._satelliteAssembly = value;
            // Assert
            contentItem.TryGetValue(ContentItem.SatelliteAssembly, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithCodeLanguage_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._codeLanguage = value;
            // Assert
            contentItem.TryGetValue(ContentItem.CodeLanguage, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithRelated_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._related = value;
            // Assert
            contentItem.TryGetValue(ContentItem.Related, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void TryGetValue_WithPropertiesNotInitialized__WithTfmRaw_GetsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem._tfmRaw = value;
            // Assert
            contentItem.TryGetValue(ContentItem.TfmRaw, out object result);
            result.Should().Be(value);
            contentItem._properties.Should().BeNull();
        }

        [Fact]
        public void AddAndTryGetValues_WithUnknownProperty_InitiailizesDictionary_AndReturnsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Act
            contentItem.Add("assembly_raw", value);
            // Assert
            contentItem._properties.Should().NotBeNull();
            contentItem._properties.Should().HaveCount(1);
            contentItem._properties.Should().Contain(new KeyValuePair<string, object>("assembly_raw", value));

            // Act again
            contentItem.TryGetValue("assembly_raw", out object result);

            // Assert
            result.Should().Be(value);
        }


        [Fact]
        public void AddAndTryGetValues_WithUnknownPropertyFollowingAPackedOne_InitiailizesDictionary_AndReturnsCorrectValue()
        {
            var contentItem = new ContentItem() { Path = "lib/net5.0/a.dll" };
            var value = "value";
            // Pre-conditions
            contentItem.Add(ContentItem.ManagedAssembly, value);
            contentItem._properties.Should().BeNull();
            contentItem._assembly.Should().Be(value);

            // Add unknown value
            contentItem.Add("assembly_raw", value);

            contentItem._properties.Should().HaveCount(2);
            contentItem._properties.Should().Contain(new KeyValuePair<string, object>("assembly_raw", value));
            contentItem._properties.Should().Contain(new KeyValuePair<string, object>(ContentItem.ManagedAssembly, value));

            // Act #1
            contentItem.TryGetValue("assembly_raw", out object rawResult);
            contentItem.TryGetValue(ContentItem.ManagedAssembly, out object result);

            // Assert
            result.Should().Be(value);
            rawResult.Should().Be(value);

            // Act #2
            var tfm = "net5.0";
            contentItem.Add(ContentItem.TargetFrameworkMoniker, tfm);
            contentItem.TryGetValue(ContentItem.TargetFrameworkMoniker, out object resultTfm);

            // Assert
            resultTfm.Should().Be(tfm);
            contentItem.Properties.Should().HaveCount(3);
            contentItem._tfm.Should().Be(null);
        }
    }
}
