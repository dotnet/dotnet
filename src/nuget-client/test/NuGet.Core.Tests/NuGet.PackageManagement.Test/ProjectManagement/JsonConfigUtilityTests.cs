// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Versioning;
using Xunit;

namespace ProjectManagement.Test
{
    public class JsonConfigUtilityTests
    {
        [Fact]
        public void GetDependencies_ParsesIdAndVersion()
        {
            // Arrange
            var json = JObject.Parse(
                @"
                {
                    ""dependencies"": { 
                            ""PackageA"": ""1.0.0"", 
                            ""PackageC"": { ""type"": ""build"", version: ""2.0.0-beta2"" }
                    }
                }");

            // Act
            var dependencies = JsonConfigUtility.GetDependencies(json).ToList();

            // Assert
            Assert.Equal(2, dependencies.Count);
            Assert.Equal("PackageA", dependencies[0].Id);
            Assert.Equal(VersionRange.Parse("1.0.0"), dependencies[0].VersionRange);

            Assert.Equal("PackageC", dependencies[1].Id);
            Assert.Equal(VersionRange.Parse("2.0.0-beta2"), dependencies[1].VersionRange);
        }

        [Fact]
        public void GetDependencies_ThrowsIfValueIsNotAValidStringOrObject()
        {
            // Arrange
            var json = JObject.Parse(
                @"
                {
                    ""dependencies"": { 
                         ""PackageA"": 1
                    }
                }");

            // Act and Assert
            var ex = Assert.Throws<FormatException>(() => JsonConfigUtility.GetDependencies(json).ToList());

            // Assert
            Assert.Equal("Dependency '\"PackageA\": 1' has invalid version specification.", ex.Message);
        }

        [Fact]
        public void GetDependencies_ThrowsIfVersionIsAnEmptyString()
        {
            // Arrange
            var json = JObject.Parse(
                @"
                {
                    ""dependencies"": { 
                         ""PackageA"": """"
                    }
                }");

            // Act and Assert
            var ex = Assert.Throws<FormatException>(() => JsonConfigUtility.GetDependencies(json).ToList());

            // Assert
            Assert.Equal("Dependency '\"PackageA\": \"\"' has invalid version specification.", ex.Message);
        }

        [Fact]
        public void GetDependencies_ThrowsIfObjectDoesNotHaveVersionProperty()
        {
            // Arrange
            var json = JObject.Parse(
                @"
                {
                    ""dependencies"": { 
                         ""PackageA"": { ""type"": ""build"" }
                    }
                }");

            // Act and Assert
            var ex = Assert.Throws<FormatException>(() => JsonConfigUtility.GetDependencies(json).ToList());

            // Assert
            Assert.Equal($"Dependency '{json["dependencies"].First}' has invalid version specification.", ex.Message);
        }

        [Fact]
        public void JsonConfigUtility_GetTargetFramework()
        {
            // Arrange
            var json = BasicConfig;

            // Act
            var frameworks = JsonConfigUtility.GetFrameworks(json);

            // Assert
            Assert.Equal("netcore50", frameworks.Single().GetShortFolderName());
        }

        [Fact]
        public void JsonConfigUtility_AddFramework()
        {
            // Arrange
            var json = BasicConfig;

            var frameworks = JsonConfigUtility.GetFrameworks(json);
            Assert.Equal(1, frameworks.Count());

            // Act
            JsonConfigUtility.AddFramework(json, new NuGet.Frameworks.NuGetFramework("uap", new Version("10.0.0")));
            frameworks = JsonConfigUtility.GetFrameworks(json);

            //Assert
            Assert.Equal(2, frameworks.Count());
        }

        [Fact]
        public void JsonConfigUtility_AddFrameworkDoesNotAddDuplicate()
        {
            // Arrange
            var json = BasicConfig;

            var frameworks = JsonConfigUtility.GetFrameworks(json);
            Assert.Equal(1, frameworks.Count());

            JsonConfigUtility.AddFramework(json, new NuGet.Frameworks.NuGetFramework("uap", new Version("10.0.0")));
            frameworks = JsonConfigUtility.GetFrameworks(json);
            Assert.Equal(2, frameworks.Count());

            // Act
            JsonConfigUtility.AddFramework(json, new NuGet.Frameworks.NuGetFramework("uap", new Version("10.0.0")));
            frameworks = JsonConfigUtility.GetFrameworks(json);
            Assert.Equal(2, frameworks.Count());
        }

        [Fact]
        public void JsonConfigUtility_ClearFrameworks()
        {
            // Arrange
            var json = BasicConfig;

            var frameworks = JsonConfigUtility.GetFrameworks(json);
            Assert.Equal("netcore50", frameworks.Single().GetShortFolderName());

            // Act
            JsonConfigUtility.ClearFrameworks(json);
            frameworks = JsonConfigUtility.GetFrameworks(json);

            //Assert
            Assert.Equal(0, frameworks.Count());
        }

        [Fact]
        public void JsonConfigUtility_AddDependencyToNewFile()
        {
            // Arrange
            var json = BasicConfig;

            // Act
            JsonConfigUtility.AddDependency(json, new PackageDependency("testpackage", VersionRange.Parse("1.0.0")));

            // Assert
            Assert.Equal("1.0.0", json["dependencies"]["testpackage"].ToString());
        }

        [Fact]
        public void JsonConfigUtility_RemoveDependencyFromNewFile()
        {
            // Arrange
            var json = BasicConfig;

            // Act
            JsonConfigUtility.RemoveDependency(json, "testpackage");

            JToken val = null;
            json.TryGetValue("dependencies", out val);

            // Assert
            Assert.Null(val);
        }

        [Fact]
        public void JsonConfigUtility_VerifyPackagesAreSortedInProjectJson()
        {
            // Arrange
            var json = BasicConfig;

            // Act
            JsonConfigUtility.AddDependency(json, new PackageDependency("testpackageb", VersionRange.Parse("1.0.0")));
            JsonConfigUtility.AddDependency(json, new PackageDependency("testpackageE", VersionRange.Parse("2.0.0")));
            JsonConfigUtility.AddDependency(json, new PackageDependency("testpackageA", VersionRange.Parse("1.0.0")));
            JsonConfigUtility.AddDependency(json, new PackageDependency("testpackaged", VersionRange.Parse("4.0.0")));
            JsonConfigUtility.AddDependency(json, new PackageDependency("testpackageC", VersionRange.Parse("1.0.0")));

            JToken val = null;
            json.TryGetValue("dependencies", out val);

            // Assert
            Assert.Equal("testpackageA", ((JProperty)json["dependencies"].Children().First()).Name);
            Assert.Equal("testpackageb", ((JProperty)json["dependencies"].Children().Skip(1).First()).Name);
            Assert.Equal("testpackageC", ((JProperty)json["dependencies"].Children().Skip(2).First()).Name);
            Assert.Equal("testpackaged", ((JProperty)json["dependencies"].Children().Skip(3).First()).Name);
            Assert.Equal("testpackageE", ((JProperty)json["dependencies"].Children().Skip(4).First()).Name);
        }

        private static JObject BasicConfig
        {
            get
            {
                var json = new JObject();

                var frameworks = new JObject();
                frameworks["netcore50"] = new JObject();

                json["frameworks"] = frameworks;

                json.Add("runtimes", JObject.Parse("{ \"uap10-x86\": { }, \"uap10-x86-aot\": { } }"));

                return json;
            }
        }
    }
}
