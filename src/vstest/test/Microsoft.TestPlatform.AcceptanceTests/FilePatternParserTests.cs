// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class FilePatternParserTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void WildCardPatternShouldCorrectlyWorkOnFiles(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssembly = GetSampleTestAssembly();
        testAssembly = testAssembly.Replace("SimpleTestProject.dll", "*TestProj*.dll");

        var arguments = PrepareArguments(
           testAssembly,
           GetTestAdapterPath(),
           string.Empty, FrameworkArgValue,
           runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void WildCardPatternShouldCorrectlyWorkOnArbitraryDepthDirectories(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssembly = GetSampleTestAssembly();

        // Add one more directory to the temp path, so we can substitute it with **
        // and copy then whole directory there.
        TempDirectory.CopyDirectory(Path.GetDirectoryName(testAssembly)!, Path.Combine(TempDirectory.Path, "dir1"));

        // The path will end up looking like <random temp dir>\**\"*TestProj*.dll".
        var wildcardedPath = Path.Combine(TempDirectory.Path, "**", "*TestProj*.dll");

        var arguments = PrepareArguments(
           wildcardedPath,
           GetTestAdapterPath(),
           string.Empty, string.Empty,
           runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void WildCardPatternShouldCorrectlyWorkForRelativeAssemblyPath(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssembly = GetSampleTestAssembly();
        testAssembly = testAssembly.Replace("SimpleTestProject.dll", "*TestProj*.dll");

        var wildCardIndex = testAssembly.IndexOfAny(new char[] { '*' });
        var testAssemblyDirectory = testAssembly.Substring(0, wildCardIndex);
        testAssembly = testAssembly.Substring(wildCardIndex);

        Directory.SetCurrentDirectory(testAssemblyDirectory);

        var arguments = PrepareArguments(
           testAssembly,
           GetTestAdapterPath(),
           string.Empty, string.Empty,
           runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void WildCardPatternShouldCorrectlyWorkOnMultipleFiles(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssembly = BuildMultipleAssemblyPath("SimpleTestProject.dll", "SimpleTestProject2.dll");
        testAssembly = testAssembly.Replace("SimpleTestProject.dll", "*TestProj*.dll");
        testAssembly = testAssembly.Replace("SimpleTestProject2.dll", "*TestProj*.dll");

        var arguments = PrepareArguments(
           testAssembly,
           GetTestAdapterPath(),
           string.Empty, FrameworkArgValue,
           runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(2, 2, 2);
    }
}
