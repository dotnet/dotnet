// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class ArgumentProcessorTests : AcceptanceTestBase
{

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void PassingNoArgumentsToVsTestConsoleShouldPrintHelpMessage(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        InvokeVsTest(null);

        //Check for help usage, description and arguments text.
        StdOutputContains("Usage: vstest.console.exe");
        StdOutputContains("Description: Runs tests from the specified files.");
        StdOutputContains("Arguments:");

        //Check for help options text
        StdOutputContains("Options:");

        //Check for help examples text
        StdOutputContains("To run tests: >vstest.console.exe tests.dll");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void PassingInvalidArgumentsToVsTestConsoleShouldNotPrintHelpMessage(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /badArgument");

        InvokeVsTest(arguments);

        //Check for help usage, description and arguments text.
        StdOutputDoesNotContains("Usage: vstest.console.exe");
        StdOutputDoesNotContains("Description: Runs tests from the specified files.");
        StdOutputDoesNotContains("Arguments:");

        //Check for help options text
        StdOutputDoesNotContains("Options:");

        //Check for help examples text
        StdOutputDoesNotContains("To run tests: >vstest.console.exe tests.dll");

        //Check for message which guides using help option
        StdErrorContains("Please use the /help option to check the list of valid arguments");
    }
}
