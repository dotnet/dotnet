// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NuGet.Frameworks.Test
{
    public class NuGetFrameworkTests
    {
        [Theory]
        [InlineData("net45", "net45")]
        [InlineData("net5.0", "net5.0")]
        [InlineData("net50", "net5.0")]
        [InlineData("netcoreapp5.0", "net5.0")]
        [InlineData("netcoreapp5.0-windows", "net5.0-windows")]
        [InlineData("netcoreapp5.0-windows10.0", "net5.0-windows10.0")]
        [InlineData("net5.0-windows7.0", "net5.0-windows7.0")]
        [InlineData("net5.0-android11.0", "net5.0-android11.0")]
        [InlineData("portable-net45+win8+monoandroid", "portable-net45+win8")]
        [InlineData("portable-net45+win8+xamarin.ios", "portable-net45+win8")]
        [InlineData("portable-net45+win8", "portable-net45+win8")]
        [InlineData("portable-win8+net45+monoandroid+monotouch", "portable-net45+win8")]
        [InlineData("portable-win8+net45+monoandroid+monotouch+xamarin.ios", "portable-net45+win8")]
        [InlineData("portable-win8+net45", "portable-net45+win8")]
        [InlineData("portable-monoandroid+monotouch+win8+net45", "portable-net45+win8")]
        [InlineData("portable-monoandroid+xamarin.ios+monotouch+win8+net45", "portable-net45+win8")]
        [InlineData("portable-monoandroid+win8+net45", "portable-net45+win8")]
        [InlineData("win10.0", "win10.0")]
        [InlineData("net45-client", "net45-client")]
        [InlineData("net45-unknown", "net45-unknown")]
        [InlineData("Any", "any")]
        [InlineData("Unsupported", "unsupported")]
        [InlineData("Agnostic", "agnostic")]
        [InlineData("portable-win8+net45+monoandroid+monotouch+xamarin.ios+xamarin.watchos", "portable-net45+win8")]
        [InlineData("portable-monoandroid+xamarin.ios+xamarin.watchos+monotouch+win8+net45", "portable-net45+win8")]
        [InlineData("portable-win8+net45+monoandroid+monotouch+xamarin.ios+xamarin.tvos", "portable-net45+win8")]
        [InlineData("portable-monoandroid+xamarin.ios+xamarin.tvos+monotouch+win8+net45", "portable-net45+win8")]
        public void NuGetFramework_ShortFolderName(string input, string expected)
        {
            var fw = NuGetFramework.Parse(input);

            string result = fw.GetShortFolderName();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("net5.0", ".NETCoreApp,Version=v5.0")]
        [InlineData("net452", ".NETFramework,Version=v4.5.2")]
        [InlineData("netcoreapp3.1", ".NETCoreApp,Version=v3.1")]
        public void NuGetFramework_GetDotNetFrameworkName(string input, string expected)
        {
            var fw = NuGetFramework.Parse(input);

            string result = fw.GetDotNetFrameworkName(DefaultFrameworkNameProvider.Instance);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("net48", ".NETFramework,Version=v4.8", "")]
        [InlineData("netstandard2.1", ".NETStandard,Version=v2.1", "")]
        [InlineData("net5.0", ".NETCoreApp,Version=v5.0", "")]
        [InlineData("net5.0-windows7.0", ".NETCoreApp,Version=v5.0", "windows,Version=7.0")]
        [InlineData("portable-net45+win8", ".NETPortable,Version=v0.0,Profile=Profile7", "")]
        public void NuGetFramework_TargetFrameworkMoniker_TargetPlatformMoniker(string input, string expectedTfm, string expectedTpm)
        {
            var framework = NuGetFramework.Parse(input);

            Assert.Equal(expectedTfm, framework.DotNetFrameworkName);
            Assert.Equal(expectedTpm, framework.DotNetPlatformName);
        }

        public static TheoryData<string, string> EqualsFrameworkData
        {
            get
            {
                return new TheoryData<string, string>
                {
                    { "net45", "net45" },
                    { "portable-net45+win8", "portable-net45+win8+monoandroid+monotouch" },
                    { "portable-net45+win8", "portable-net45+win8+monoandroid+monotouch+xamarin.ios" },
                    { "portable-net45+win8", "portable-net45+win8+xamarin.ios" },
                    { "portable-win8+net45", "portable-net45+win8+monoandroid+monotouch" },
                    { "portable-monoandroid+monotouch+win8+net45", "portable-net45+win8+monoandroid+monotouch" },
                    { "portable-monoandroid+win8+net45", "portable-net45+win8+monoandroid+monotouch" },
                    { "win10.0", "win10.0" },
                    { "net45-client", "net45-client" },
                    { "net45-unknown", "net45-unknown" },
                    { "Any", "any" },
                    { "Unsupported", "unsupported" },
                    { "Agnostic", "agnostic" },
                    { "portable-net45+win8", "portable-net45+win8+monoandroid+monotouch+xamarin.ios+xamarin.watchos" },
                    { "portable-net45+win8", "portable-net45+win8+xamarin.ios+xamarin.watchos" },
                    { "portable-net45+win8", "portable-net45+win8+monoandroid+monotouch+xamarin.ios+xamarin.tvos" },
                    { "portable-net45+win8", "portable-net45+win8+xamarin.ios+xamarin.tvos" },
                };
            }
        }

        [Theory]
        [MemberData(nameof(EqualsFrameworkData))]
        public void NuGetFramework_Equality(string a, string b)
        {
            var fw1 = NuGetFramework.Parse(a);
            var fw2 = NuGetFramework.Parse(b);
            HashSet<NuGetFramework> hashSet = new HashSet<NuGetFramework>() { fw1, fw2 };

            Assert.True(fw1.Equals(fw2));
            Assert.True(fw2.Equals(fw1));
            Assert.Equal(fw1.GetHashCode(), fw2.GetHashCode());
            Assert.Equal(1, hashSet.Count);
        }

        [Theory]
        [MemberData(nameof(EqualsFrameworkData))]
        public void EqualityOperator_ReturnTrueForEqualsFramework(string a, string b)
        {
            // Arrange
            var framework1 = NuGetFramework.Parse(a);
            var framework2 = NuGetFramework.Parse(b);

            // Act and Assert
            Assert.True(framework1 == framework2);
            Assert.True(framework2 == framework1);
        }

        [Fact]
        public void EqualityOperator_ReturnTrueIfBothFrameworksAreNull()
        {
            // Arrange
            NuGetFramework? framework1 = null;
            NuGetFramework? framework2 = null;

            // Act and Assert
            Assert.True(framework1 == framework2);
            Assert.True(framework2 == framework1);
        }

        [Theory]
        [MemberData(nameof(EqualsFrameworkData))]
        public void InequalityOperator_ReturnFalseForEqualsFramework(string a, string b)
        {
            // Arrange
            var framework1 = NuGetFramework.Parse(a);
            var framework2 = NuGetFramework.Parse(b);

            // Act and Assert
            Assert.False(framework1 != framework2);
            Assert.False(framework2 != framework1);
        }

        [Fact]
        public void InequalityOperator_ReturnFalseIfBothFrameworksAreNull()
        {
            // Arrange
            NuGetFramework? framework1 = null;
            NuGetFramework? framework2 = null;

            // Act and Assert
            Assert.False(framework1 != framework2);
            Assert.False(framework2 != framework1);
        }

        public static TheoryData<string, string> InequalsFrameworkData
        {
            get
            {
                return new TheoryData<string, string>
                {
                    { "net45", "net46" },
                    { "portable-net45+win8", "portable-win8+monoandroid+monotouch" },
                    { "win10.0", "win11.0" },
                    { "Unsupported", "netstandard1.0" },
                    { "netcoreapp1.0", "agnostic" },
                    { "netstandard1.1", "net451" },
                };
            }
        }

        [Theory]
        [MemberData(nameof(InequalsFrameworkData))]
        public void EqualityOperator_ReturnsFalseForInequalFrameworks(string a, string b)
        {
            // Arrange
            var framework1 = NuGetFramework.Parse(a);
            var framework2 = NuGetFramework.Parse(b);

            // Act and Assert
            Assert.False(framework1 == framework2);
            Assert.False(framework2 == framework1);
        }

        public static TheoryData<string> FrameworkEqualityWithNullData
        {
            get
            {
                return new TheoryData<string>
                {
                    "net451",
                    "netcoreapp1.0",
                    "portable-net45+win8",
                    "Unsupported",
                    "agnostic",
                };
            }
        }

        [Theory]
        [MemberData(nameof(FrameworkEqualityWithNullData))]
        public void EqualityOperator_ReturnsFalseIfOneFrameworkIsNull(string frameworkName)
        {
            // Arrange
            var framework1 = NuGetFramework.Parse(frameworkName);
            NuGetFramework? framework2 = null;

            // Act and Assert
            Assert.False(framework1 == framework2);
            Assert.False(framework2 == framework1);
        }

        [Theory]
        [MemberData(nameof(InequalsFrameworkData))]
        public void InequalityOperator_ReturnsTrueForInequalFrameworks(string a, string b)
        {
            // Arrange
            var framework1 = NuGetFramework.Parse(a);
            var framework2 = NuGetFramework.Parse(b);

            // Act and Assert
            Assert.True(framework1 != framework2);
            Assert.True(framework2 != framework1);
        }

        [Theory]
        [MemberData(nameof(FrameworkEqualityWithNullData))]
        public void InequalityOperator_ReturnsTrueIfOneFrameworkIsNull(string frameworkName)
        {
            // Arrange
            var framework1 = NuGetFramework.Parse(frameworkName);
            NuGetFramework? framework2 = null;

            // Act and Assert
            Assert.True(framework1 != framework2);
            Assert.True(framework2 != framework1);
        }

        [Theory]
        [InlineData("net45", "net450")]
        [InlineData("net45", "net4.5.0")]
        [InlineData("aspnetcore5", "aspnetcore500")]
        [InlineData(".NETFramework, Version=v4.5", "net45")]
        [InlineData("NETFramework, Version=v4.5", "net45")]
        [InlineData("NETFramework, Version=v4.5", "net450")]
        public void NuGetFramework_EqualityNormalization(string a, string b)
        {
            var fw1 = NuGetFramework.Parse(a);
            var fw2 = NuGetFramework.Parse(b);
            HashSet<NuGetFramework> hashSet = new HashSet<NuGetFramework>() { fw1, fw2 };

            Assert.True(fw1.Equals(fw2));
            Assert.True(fw2.Equals(fw1));
            Assert.Equal(fw1.GetHashCode(), fw2.GetHashCode());
            Assert.Equal(1, hashSet.Count);
        }

        [Theory]
        [InlineData("net45", false)]
        [InlineData("win8", false)]
        [InlineData("netstandardapp1.0", true)]
        [InlineData("netstandard1.0", true)]
        [InlineData("dotnet5.1", true)]
        [InlineData("dotnet", true)]
        [InlineData("netcoreapp1.0", true)]
        [InlineData("netcore50", true)]
        [InlineData("netcore51", true)]
        [InlineData("netcore61", true)]
        [InlineData("netcore45", false)]
        [InlineData("netcore4.9.9", false)]
        [InlineData("win81", false)]
        [InlineData("win10", false)] // this framework must use netcore
        [InlineData("aspnetcore1.0", false)] // deprecated
        [InlineData("aspnet451", false)]
        [InlineData("uap10.0", true)]
        [InlineData("uap11.0", true)]
        [InlineData("tizen3.0", true)]
        public void NuGetFramework_IsPackageBased(string framework, bool isPackageBased)
        {
            var fw = NuGetFramework.Parse(framework);

            Assert.Equal(isPackageBased, fw.IsPackageBased);
        }

        [Fact]
        public void NuGetFramework_EqualityMixed()
        {
            List<NuGetFramework> frameworks = new List<NuGetFramework>();
            frameworks.Add(NuGetFramework.Parse("net45"));
            frameworks.Add(NuGetFramework.Parse("net450"));
            frameworks.Add(NuGetFramework.Parse("net4.5"));
            frameworks.Add(NuGetFramework.Parse(".NETFramework, Version=v4.5"));
            frameworks.Add(NuGetFramework.Parse(".NETFramework, Version=4.5"));
            frameworks.Add(NuGetFramework.Parse("NETFramework, Version=v4.5"));
            frameworks.Add(NuGetFramework.Parse("NETFramework, Version=v4.5"));

            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5)));
            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5), string.Empty));

            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5, 0)));
            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5, 0), string.Empty));

            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5, 0, 0)));
            frameworks.Add(new NuGetFramework(".nETframework", new Version(4, 5, 0, 0), string.Empty));

            foreach (var fw1 in frameworks)
            {
                foreach (var fw2 in frameworks)
                {
                    Assert.True(fw1.Equals(fw2), fw1.ToString() + " " + fw2.ToString());
                    Assert.True(fw2.Equals(fw1), fw2.ToString() + " " + fw1.ToString());
                    Assert.True(Object.Equals(fw1, fw2));
                    Assert.True(Object.Equals(fw2, fw1));
                }
            }
        }

        [Fact]
        public void NuGetFramework_EqualityMixed2()
        {
            List<NuGetFramework> frameworks = new List<NuGetFramework>();

            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5), null));
            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5), string.Empty));

            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5, 0), null));
            frameworks.Add(new NuGetFramework(".NETFramework", new Version(4, 5, 0), string.Empty));

            foreach (var fw1 in frameworks)
            {
                foreach (var fw2 in frameworks)
                {
                    Assert.True(fw1.Equals(fw2), fw1.ToString() + " " + fw2.ToString());
                    Assert.True(fw2.Equals(fw1), fw2.ToString() + " " + fw1.ToString());
                    Assert.True(Object.Equals(fw1, fw2));
                    Assert.True(Object.Equals(fw2, fw1));
                }
            }
        }

        [Fact]
        public void NuGetFramework_GetPortableShortFolderNameWithNoProfile()
        {
            // Arrange
            var target = new NuGetFramework(".NETPortable", new Version(0, 0), string.Empty);

            // Act & Arrange
            var ex = Assert.Throws<FrameworkException>(() => target.GetShortFolderName());
            Assert.Equal(
                "Invalid portable frameworks for '.NETPortable,Version=v0.0'. " +
                "A portable framework must have at least one framework in the profile.",
                ex.Message);
        }

        [Fact]
        public void NuGetFramework_GetPortableShortFolderNameWithHyphenInProfile()
        {
            // Arrange
            var target = new NuGetFramework(".NETPortable", new Version(0, 0), "net45+net-cf+win8");

            // Act & Arrange
            var ex = Assert.Throws<ArgumentException>(() => target.GetShortFolderName());
            Assert.Equal(
                "Invalid portable frameworks 'net45+net-cf+win8'. " +
                "A hyphen may not be in any of the portable framework names.",
                ex.Message);
        }

        [Theory]
        [InlineData("netcoreapp5.0")]
        [InlineData("net45")]
        [InlineData("net5.0-windows10.0.16000.1")]
        public void NuGetFramework_WithCopyConstructor_CreatesEquivalentFrameworks(string frameworkName)
        {
            var originalFramework = NuGetFramework.Parse(frameworkName);

            var copiedFramework = new NuGetFramework(originalFramework);

            Assert.Equal(originalFramework, copiedFramework);
        }

        [Fact]
        public void NuGetFramework_Stuff()
        {
            var leftSide = NuGetFramework.ParseComponents(".NETCoreApp,Version=v5.0", "Windows,Version=7.0");
            var rightSide = NuGetFramework.ParseComponents(".NETCoreApp,Version=v5.0", "Windows,Version=7.0");

            leftSide.Should().Be(rightSide);
            leftSide.Should().Be(rightSide);
            leftSide.GetHashCode().Should().Be(rightSide.GetHashCode(), because: "Equivalent objects should have the same hash code.");
            leftSide.GetHashCode().Should().Be(rightSide.GetHashCode(), because: "Equivalent objects should have the same hash code.");

            var frameworks = new List<NuGetFramework> { leftSide, rightSide };

            var distinctFrameworksWithComparer = frameworks.Distinct(NuGetFrameworkFullComparer.Instance).ToArray();
            var distinctFrameworksWithoutComparer = frameworks.Distinct().ToArray();

            distinctFrameworksWithComparer.Should().HaveCount(1);
            distinctFrameworksWithoutComparer.Should().HaveCount(1);


        }
    }
}
