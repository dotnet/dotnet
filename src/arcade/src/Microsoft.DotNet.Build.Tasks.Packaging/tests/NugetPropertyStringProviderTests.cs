// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class NugetPropertyStringProviderTests
    {

        [Fact]
        public void ShouldParseSingleValidKeyValue()
        {
            var expectedDictionary = new Dictionary<string, string>{ {"a", "b"} };
            AssertPropertyStringsParseToDictionary(new[] { "a=b" }, expectedDictionary);
            AssertPropertyStringsParseToDictionary(new[] { " a=b " }, expectedDictionary);
        }

        [Fact]
        public void ShouldParseSingleValidKeyValueWithEqualsInValue()
        {
            var expectedDictionary = new Dictionary<string, string> { { "a", "=b=" } };
            AssertPropertyStringsParseToDictionary(new[] { "a==b=" }, expectedDictionary);
        }

        [Fact]
        public void ShouldParseSingleValidKeyValueWithMultineContents()
        {
            var multiLineString = @"b
                                    c";
            var expectedDictionary = new Dictionary<string, string> { { "a", multiLineString } };
            AssertPropertyStringsParseToDictionary(new[] { $"a={multiLineString}" }, expectedDictionary);

            expectedDictionary = new Dictionary<string, string> { { multiLineString, "b" } };
            AssertPropertyStringsParseToDictionary(new[] { $"{multiLineString}=b" }, expectedDictionary);

            expectedDictionary = new Dictionary<string, string> { { multiLineString, multiLineString } };
            AssertPropertyStringsParseToDictionary(new[] { $"{multiLineString}={multiLineString}" }, expectedDictionary);
        }

        [Fact]
        public void ShouldReturnNullOnNullInput()
        {
            NuspecPropertyStringProvider.GetNuspecPropertyDictionary(null).Should().BeNull();
        }

        [Fact]
        public void ShouldReturnEmptyDictionaryOnEmptyInput()
        {
            var expectedDictionary = new Dictionary<string, string>();
            AssertPropertyStringsParseToDictionary(new string[0], expectedDictionary);
        }

        [Fact]
        public void ShouldFailWithNoEquals()
        {
            Action act = () => NuspecPropertyStringProvider.GetNuspecPropertyDictionary(new[] { "abc" });
            act.Should().Throw<InvalidDataException>();
        }

        [Fact]
        public void ShouldFailWithNoValue()
        {
            Action act = () => NuspecPropertyStringProvider.GetNuspecPropertyDictionary(new[] { "a= " });
            act.Should().Throw<InvalidDataException>();
        }

        [Fact]
        public void ShouldFailWithNoKey()
        {
            Action act = () => NuspecPropertyStringProvider.GetNuspecPropertyDictionary(new[] { " = b" });
            act.Should().Throw<InvalidDataException>();
        }

        [Fact]
        public void ShouldFailWithNoKeyAndValue()
        {
            Action act = () => NuspecPropertyStringProvider.GetNuspecPropertyDictionary(new[] { " = " });
            act.Should().Throw<InvalidDataException>();
        }

        private static void AssertPropertyStringsParseToDictionary(string[] propertyStrings, Dictionary<string, string> expectedDictionary)
        {
            NuspecPropertyStringProvider.GetNuspecPropertyDictionary(propertyStrings).Should().BeEquivalentTo(expectedDictionary);
        }
    }
}
