// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NuGet.Common;
using NuGet.Packaging.Rules;
using Xunit;
using static NuGet.Packaging.Rules.ReferencesInNuspecMatchRefAssetsRule;

namespace NuGet.Packaging.Test
{
    public class ReferencesInNuspecMatchRefAssetsRuleTests
    {
        [Fact]
        public void Compare_PackageWithReferencesAndRefFolderMatching_ShouldBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "net462", new List<string>() { "MyLib.dll", "MyHelpers.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net462/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Empty(missingReferences);
        }

        [Fact]
        public void Compare_EmptyNuspecReferences_ShouldBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "any", new List<string>() }
            };
            var files = new string[] { };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Empty(missingReferences);
        }

        [Fact]
        public void Compare_PackageWithNoReferences_ShouldBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>();
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net462/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Empty(missingReferences);
        }

        [Fact]
        public void Compare_PackageWithReferencesThatTheRefFolderDoesNotHave_RefFilesMissingShouldNotBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "net462", new List<string>() { "MyLib.dll", "MyHelpers.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleIssue = missingReferences.Single(t => t.Tfm.Equals("net462") && t.MissingItems.Contains("MyHelpers.dll"));
            Assert.Equal("net462", missingReferences.First().Tfm);
            Assert.Equal(1, missingReferences.First().MissingItems.Length);
            Assert.Equal("MyHelpers.dll", missingReferences.First().MissingItems[0]);
        }

        [Fact]
        public void Compare_PackageWithRefAssetsThatTheNuspecDoesNotHave_ShouldHaveOneMissingItem()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "net462", new List<string>() { "MyLib.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net462/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleIssue = missingReferences.Single(t => t.Tfm.Equals("net462") && t.MissingItems.Contains("MyHelpers.dll"));
        }

        [Fact]
        public void Compare_PackageWithRefAssetsUnderRefFolder_ShouldBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>()
            {
                { "net462", new List<string>() { "MyLib.dll", "MyHelpers.dll" } }
            };
            var files = new string[]
            {
                "ref/MyLib.dll",
                "ref/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleIssue = missingReferences.Single(t => t.MissingFrom.Equals("ref"));
            Assert.Equal("net462", missingReferences.First().Tfm);
            Assert.Equal(2, missingReferences.First().MissingItems.Length);
            Assert.True(missingReferences.First().MissingItems.Contains("MyLib.dll"));
            Assert.True(missingReferences.First().MissingItems.Contains("MyHelpers.dll"));
        }

        [Fact]
        public void Compare_PackageWithAssetsUnderMiscellaneousFolders_ShouldBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>()
            {
                { "MyCompany", new List<string>() { "MyLib.dll" } }
            };
            var files = new string[]
            {
                "ref/MyCompany/MyLib.dll",
                "ref/MyCompany/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Empty(missingReferences);
        }

        [Fact]
        public void Compare_PackageWithMismatchedReferencesInMultipleSubFolders_ShouldHaveCorrectItems()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "net462", new List<string>() { "MyLib.dll", "MyHelpers.dll" } },
                { "net472", new List<string>() { "MyLib.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net472/MyLib.dll",
                "ref/net472/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 2);
            var singleRefFilesMissing = missingReferences.Single(t => t.MissingFrom.Equals("ref"));
            var singleReferencesMissing = missingReferences.Single(t => t.MissingFrom.Equals("nuspec"));
            Assert.Equal("net462", singleRefFilesMissing.Tfm);
            Assert.Equal(1, singleRefFilesMissing.MissingItems.Length);
            Assert.Equal("net472", singleReferencesMissing.Tfm);
            Assert.Equal(1, singleReferencesMissing.MissingItems.Length);
            Assert.Equal("MyHelpers.dll", singleReferencesMissing.MissingItems[0]);
        }

        [Fact]
        public void Compare_PackageHasReferencesWithNoRefFiles_ShouldContainCorrectFile()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "net462", new List<string>() { "MyLib.dll" } }
            };
            var files = Array.Empty<string>();

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleIssue = missingReferences.Single(t => t.Tfm.Equals("net462"));
            Assert.Equal("net462", missingReferences.First().Tfm);
            Assert.Equal(1, missingReferences.First().MissingItems.Length);
            Assert.Equal("MyLib.dll", missingReferences.First().MissingItems[0]);
            Assert.Equal("ref", missingReferences.First().MissingFrom);
        }

        [Fact]
        public void Compare_NuspecHasFilesWithNoSpecificTfmWithMatchingRefFiles_ShouldBeEmpty()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "any", new List<string>() { "MyLib.dll", "MyHelpers.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net462/MyHelpers.dll",
                "ref/net472/MyLib.dll",
                "ref/net472/MyHelpers.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Empty(missingReferences);
        }

        [Fact]
        public void Compare_MissingSubFolderInNuspec_ShouldHaveSubFolder()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "net462", new List<string>() { "MyLib.dll", "MyHelpers.dll" } },
                { "net472", new List<string>() { "MyLib.dll", "MyHelpers.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net462/MyHelpers.dll",
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleMissingReference = missingReferences.Single(t => t.MissingFrom.Equals("ref"));
            Assert.Equal("net472", singleMissingReference.Tfm);
            Assert.Equal(2, singleMissingReference.MissingItems.Length);
            Assert.True(singleMissingReference.MissingItems.Contains("MyLib.dll"));
            Assert.True(singleMissingReference.MissingItems.Contains("MyHelpers.dll"));
        }
        [Fact]
        public void Compare_NuspecHasFilesWithNoSpecificTfmWithMissingRefFiles_ShouldWarnOnce()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "any", new List<string>() { "MyLib.dll", "MyHelpers.dll" } }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll"
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleMissingReference = missingReferences.Single(t => t.MissingFrom.Equals("ref"));
            Assert.Equal("net462", singleMissingReference.Tfm);
            Assert.Equal(1, singleMissingReference.MissingItems.Length);
            Assert.True(singleMissingReference.MissingItems.Contains("MyHelpers.dll"));
        }

        [Fact]
        public void Compare_NuspecHasFilesWithNoSpecificTfmWithMissingReferences_ShouldWarn()
        {
            //Arrange
            var references = new Dictionary<string, IEnumerable<string>>
            {
                { "any", new List<string>() { "MyLib.dll",} }
            };
            var files = new string[]
            {
                "ref/net462/MyLib.dll",
                "ref/net462/MyHelpers.dll",
            };

            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = rule.Compare(references, files);

            //Assert
            Assert.Equal(missingReferences.Count(), 1);
            var singleMissingReference = missingReferences.Single(t => t.MissingFrom.Equals("nuspec"));
            Assert.Equal("net462", singleMissingReference.Tfm);
            Assert.Equal(1, singleMissingReference.MissingItems.Length);
            Assert.True(singleMissingReference.MissingItems.Contains("MyHelpers.dll"));
        }
        [Fact]
        public void GenerateWarnings_PackageWithReferencesMissingFromTheNuspec_ShouldWarn()
        {
            //Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = new List<MissingReference>
            {
                new MissingReference("nuspec", "net462", new string[] { "MyHelpers.dll" })
            };
            var issues = rule.GenerateWarnings(missingReferences);

            //Assert
            Assert.Equal(issues.Count(), 1);
            var singleIssue = issues.Single(t => t.Code == NuGetLogCode.NU5131);
            var expectedMessage = "References were found in the nuspec, but some reference assemblies were not found in both the nuspec and ref folder. Add the following reference assemblies:" + Environment.NewLine +
                "- Add MyHelpers.dll to the net462 reference group in the nuspec" + Environment.NewLine;
            Assert.Equal(singleIssue.Message, expectedMessage);
        }

        [Fact]
        public void GenerateWarnings_PackageWithAssetsMissingFromTheRefFolder_ShouldWarn()
        {
            //Arrange & Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = new List<MissingReference>
            {
                new MissingReference("ref", "net462", new string[] { "MyHelpers.dll" })
            };
            var issues = rule.GenerateWarnings(missingReferences);

            //Assert
            Assert.Equal(issues.Count(), 1);
            var singleIssue = issues.Single(t => t.Code == NuGetLogCode.NU5131);
            var expectedMessage = "References were found in the nuspec, but some reference assemblies were not found in both the nuspec and ref folder. Add the following reference assemblies:" + Environment.NewLine +
                "- Add MyHelpers.dll to the ref/net462/ directory" + Environment.NewLine;
            Assert.Equal(singleIssue.Message, expectedMessage);
        }

        [Fact]
        public void GenerateWarnings_PackageWithSubfoldersThatAreMismatched_ShouldWarn()
        {
            //Arrange & Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = new List<MissingReference>
            {
                new MissingReference("ref", "net472", new string[] { "MyLib.dll" }),
                new MissingReference("nuspec", "net462", new string[] { "MyHelpers.dll" })
            };
            var issues = rule.GenerateWarnings(missingReferences);

            //Assert
            Assert.Equal(1, issues.Count());
            var singleIssue = issues.Single(t => t.Code == NuGetLogCode.NU5131);
            var expectedMessage = "References were found in the nuspec, but some reference assemblies were not found in both the nuspec and ref folder. Add the following reference assemblies:" + Environment.NewLine +
                "- Add MyLib.dll to the ref/net472/ directory" + Environment.NewLine +
                "- Add MyHelpers.dll to the net462 reference group in the nuspec" + Environment.NewLine;
            Assert.Equal(singleIssue.Message, expectedMessage);
        }

        [Fact]
        public void GenerateWarnings_PackageWithSubFoldersThatAreCorrect_ShouldNotWarn()
        {
            //Arrange & Act
            var rule = new ReferencesInNuspecMatchRefAssetsRule();
            var missingReferences = Array.Empty<MissingReference>();
            var issues = rule.GenerateWarnings(missingReferences);

            //Assert
            Assert.Empty(issues);
        }
    }
}
