// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using AwesomeAssertions;

namespace Microsoft.DotNet.SetupNugetSources.Tests
{
    public static class NuGetConfigAssertions
    {
        /// <summary>
        /// Compares two NuGet.config files for semantic equality, ignoring whitespace differences
        /// </summary>
        public static void ShouldBeSemanticallySame(this string actualContent, string expectedContent, string because = "")
        {
            var actualNormalized = NormalizeXml(actualContent);
            var expectedNormalized = NormalizeXml(expectedContent);
            
            actualNormalized.Should().Be(expectedNormalized, because);
        }

        /// <summary>
        /// Asserts that the config contains a package source with the specified key
        /// </summary>
        public static void ShouldContainPackageSource(this string configContent, string key, string value = null, string because = "")
        {
            var doc = XDocument.Parse(configContent);
            var packageSources = doc.Root?.Element("packageSources");
            packageSources.Should().NotBeNull($"packageSources section should exist {because}");
            
            var source = packageSources.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == key);
            source.Should().NotBeNull($"package source '{key}' should exist {because}");
            
            if (value != null)
            {
                source.Attribute("value")?.Value.Should().Be(value, $"package source '{key}' should have the correct value {because}");
            }
        }

        /// <summary>
        /// Asserts that the config does not contain a package source with the specified key
        /// </summary>
        public static void ShouldNotContainPackageSource(this string configContent, string key, string because = "")
        {
            var doc = XDocument.Parse(configContent);
            var packageSources = doc.Root?.Element("packageSources");
            
            if (packageSources != null)
            {
                var source = packageSources.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == key);
                source.Should().BeNull($"package source '{key}' should not exist {because}");
            }
        }

        /// <summary>
        /// Asserts that the config contains credentials for the specified source
        /// </summary>
        public static void ShouldContainCredentials(this string configContent, string sourceName, string username = null, string because = "")
        {
            var doc = XDocument.Parse(configContent);
            var credentials = doc.Root?.Element("packageSourceCredentials");
            credentials.Should().NotBeNull($"packageSourceCredentials section should exist {because}");
            
            var sourceCredentials = credentials.Element(sourceName);
            sourceCredentials.Should().NotBeNull($"credentials for '{sourceName}' should exist {because}");
            
            if (username != null)
            {
                var usernameElement = sourceCredentials.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "Username");
                usernameElement.Should().NotBeNull($"username credential should exist for '{sourceName}' {because}");
                usernameElement.Attribute("value")?.Value.Should().Be(username, $"username should match for '{sourceName}' {because}");
            }
            
            var passwordElement = sourceCredentials.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "ClearTextPassword");
            passwordElement.Should().NotBeNull($"password credential should exist for '{sourceName}' {because}");
        }

        /// <summary>
        /// Asserts that the config does not contain credentials for the specified source
        /// </summary>
        public static void ShouldNotContainCredentials(this string configContent, string sourceName, string because = "")
        {
            var doc = XDocument.Parse(configContent);
            var credentials = doc.Root?.Element("packageSourceCredentials");
            
            if (credentials != null)
            {
                var sourceCredentials = credentials.Element(sourceName);
                sourceCredentials.Should().BeNull($"credentials for '{sourceName}' should not exist {because}");
            }
        }

        /// <summary>
        /// Asserts that a source is not in the disabled sources list
        /// </summary>
        public static void ShouldNotBeDisabled(this string configContent, string sourceName, string because = "")
        {
            var doc = XDocument.Parse(configContent);
            var disabledSources = doc.Root?.Element("disabledPackageSources");
            
            if (disabledSources != null)
            {
                var disabledSource = disabledSources.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == sourceName);
                disabledSource.Should().BeNull($"source '{sourceName}' should not be disabled {because}");
            }
        }

        /// <summary>
        /// Asserts that a source is in the disabled sources list
        /// </summary>
        public static void ShouldBeDisabled(this string configContent, string sourceName, string because = "")
        {
            var doc = XDocument.Parse(configContent);
            var disabledSources = doc.Root?.Element("disabledPackageSources");
            disabledSources.Should().NotBeNull($"disabledPackageSources section should exist {because}");
            
            var disabledSource = disabledSources.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == sourceName);
            disabledSource.Should().NotBeNull($"source '{sourceName}' should be disabled {because}");
        }

        /// <summary>
        /// Counts the number of package sources in the config
        /// </summary>
        public static int GetPackageSourceCount(this string configContent)
        {
            var doc = XDocument.Parse(configContent);
            var packageSources = doc.Root?.Element("packageSources");
            return packageSources?.Elements("add").Count() ?? 0;
        }

        /// <summary>
        /// Normalizes XML content for comparison by removing whitespace differences and sorting elements consistently
        /// </summary>
        private static string NormalizeXml(string xmlContent)
        {
            var doc = XDocument.Parse(xmlContent);
            
            // Sort package sources by key for consistent comparison
            var packageSources = doc.Root?.Element("packageSources");
            if (packageSources != null)
            {
                var sortedSources = packageSources.Elements("add")
                    .OrderBy(e => e.Attribute("key")?.Value)
                    .ToList();
                packageSources.RemoveAll();
                foreach (var source in sortedSources)
                {
                    packageSources.Add(source);
                }
            }

            // Sort disabled sources by key
            var disabledSources = doc.Root?.Element("disabledPackageSources");
            if (disabledSources != null)
            {
                var sortedDisabled = disabledSources.Elements("add")
                    .OrderBy(e => e.Attribute("key")?.Value)
                    .ToList();
                disabledSources.RemoveAll();
                foreach (var source in sortedDisabled)
                {
                    disabledSources.Add(source);
                }
            }

            // Sort credentials by source name
            var credentials = doc.Root?.Element("packageSourceCredentials");
            if (credentials != null)
            {
                var sortedCredentials = credentials.Elements()
                    .OrderBy(e => e.Name.LocalName)
                    .ToList();
                credentials.RemoveAll();
                foreach (var cred in sortedCredentials)
                {
                    credentials.Add(cred);
                }
            }

            return doc.ToString(SaveOptions.DisableFormatting);
        }
    }
}


