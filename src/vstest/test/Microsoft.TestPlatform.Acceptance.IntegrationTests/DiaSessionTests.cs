// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DiaSessionTests : AcceptanceTestBase
{
    public static string? GetAndSetTargetFrameWork(IntegrationTestEnvironment testEnvironment)
    {
        var currentTargetFrameWork = testEnvironment.TargetFramework;
        testEnvironment.TargetFramework =
#if NETFRAMEWORK
            "net462";
#else
            "net8.0";
#endif
        return currentTargetFrameWork;
    }

    [TestMethod]
    public void GetNavigationDataShouldReturnCorrectFileNameAndLineNumber()
    {
        var currentTargetFrameWork = GetAndSetTargetFrameWork(_testEnvironment);
        var assemblyPath = GetAssetFullPath("SimpleClassLibrary.dll");

        var diaSession = new DiaSession(assemblyPath);
        DiaNavigationData? diaNavigationData = diaSession.GetNavigationData("SimpleClassLibrary.Class1", "PassingTest");

        Assert.IsNotNull(diaNavigationData, "Failed to get navigation data");
        StringAssert.EndsWith(diaNavigationData.FileName!.Replace("\\", "/"), @"\SimpleClassLibrary\Class1.cs".Replace("\\", "/"));

        ValidateMinLineNumber(11, diaNavigationData.MinLineNumber);
        Assert.AreEqual(13, diaNavigationData.MaxLineNumber, "Incorrect max line number");

        _testEnvironment.TargetFramework = currentTargetFrameWork;
    }

    [TestMethod]
    public void GetNavigationDataShouldReturnCorrectDataForAsyncMethod()
    {
        var currentTargetFrameWork = GetAndSetTargetFrameWork(_testEnvironment);
        var assemblyPath = GetAssetFullPath("SimpleClassLibrary.dll");

        var diaSession = new DiaSession(assemblyPath);
        DiaNavigationData? diaNavigationData = diaSession.GetNavigationData("SimpleClassLibrary.Class1+<AsyncTestMethod>d__1", "MoveNext");

        Assert.IsNotNull(diaNavigationData, "Failed to get navigation data");
        StringAssert.EndsWith(diaNavigationData.FileName!.Replace("\\", "/"), @"\SimpleClassLibrary\Class1.cs".Replace("\\", "/"));

        ValidateMinLineNumber(16, diaNavigationData.MinLineNumber);
        Assert.AreEqual(18, diaNavigationData.MaxLineNumber, "Incorrect max line number");

        _testEnvironment.TargetFramework = currentTargetFrameWork;
    }

    [TestMethod]
    public void GetNavigationDataShouldReturnCorrectDataForOverLoadedMethod()
    {
        var currentTargetFrameWork = GetAndSetTargetFrameWork(_testEnvironment);
        var assemblyPath = GetAssetFullPath("SimpleClassLibrary.dll");

        var diaSession = new DiaSession(assemblyPath);
        DiaNavigationData? diaNavigationData = diaSession.GetNavigationData("SimpleClassLibrary.Class1", "OverLoadedMethod");

        Assert.IsNotNull(diaNavigationData, "Failed to get navigation data");
        StringAssert.EndsWith(diaNavigationData.FileName!.Replace("\\", "/"), @"\SimpleClassLibrary\Class1.cs".Replace("\\", "/"));

        // Weird why DiaSession is now returning the first overloaded method
        // as compared to before when it used to return second method
        ValidateLineNumbers(diaNavigationData.MinLineNumber, diaNavigationData.MaxLineNumber);

        _testEnvironment.TargetFramework = currentTargetFrameWork;
    }

    [TestMethod]
    public void GetNavigationDataShouldReturnNullForNotExistMethodNameOrNotExistTypeName()
    {
        var currentTargetFrameWork = GetAndSetTargetFrameWork(_testEnvironment);
        var assemblyPath = GetAssetFullPath("SimpleClassLibrary.dll");

        var diaSession = new DiaSession(assemblyPath);

        // Not exist method name
        DiaNavigationData? diaNavigationData = diaSession.GetNavigationData("SimpleClassLibrary.Class1", "NotExistMethod");
        Assert.IsNull(diaNavigationData);

        // Not Exist Type name
        diaNavigationData = diaSession.GetNavigationData("SimpleClassLibrary.NotExistType", "PassingTest");
        Assert.IsNull(diaNavigationData);

        _testEnvironment.TargetFramework = currentTargetFrameWork;
    }

    [TestMethod]
    [Ignore] // TODO: This test tests against a time threshold, which makes it fail on server sometimes.
    public void DiaSessionPerfTest()
    {
        var currentTargetFrameWork = GetAndSetTargetFrameWork(_testEnvironment);
        var assemblyPath = GetAssetFullPath("SimpleClassLibrary.dll");

        var watch = Stopwatch.StartNew();
        var diaSession = new DiaSession(assemblyPath);
        DiaNavigationData? diaNavigationData = diaSession.GetNavigationData("SimpleClassLibrary.HugeMethodSet", "MSTest_D1_01");
        watch.Stop();

        Assert.IsNotNull(diaNavigationData, "Failed to get navigation data");
        StringAssert.EndsWith(diaNavigationData.FileName!.Replace("\\", "/"), @"\SimpleClassLibrary\HugeMethodSet.cs".Replace("\\", "/"));
        ValidateMinLineNumber(9, diaNavigationData.MinLineNumber);
        Assert.AreEqual(10, diaNavigationData.MaxLineNumber);
        var expectedTime = 150;
        Assert.IsTrue(watch.Elapsed.Milliseconds < expectedTime, $"DiaSession Perf test Actual time:{watch.Elapsed.Milliseconds} ms Expected time:{expectedTime} ms");

        _testEnvironment.TargetFramework = currentTargetFrameWork;
    }

    private static void ValidateLineNumbers(int min, int max)
    {
        // Release builds optimize code, hence min line numbers are different.
        if (IntegrationTestEnvironment.BuildConfiguration.StartsWith("release", StringComparison.OrdinalIgnoreCase))
        {
            Assert.AreEqual(min, max, "Incorrect min line number");
        }
        else
        {
            if (max == 22)
            {
                Assert.AreEqual(min + 1, max, "Incorrect min line number");
            }
            else if (max == 26)
            {
                Assert.AreEqual(min + 1, max, "Incorrect min line number");
            }
            else
            {
                Assert.Fail($"Incorrect min/max line number. Expected Max to be 22 or 26. And Min to be 21 or 25. But Min was {min}, and Max was {max}.");
            }
        }
    }

    private static void ValidateMinLineNumber(int expected, int actual)
    {
        // Release builds optimize code, hence min line numbers are different.
        if (IntegrationTestEnvironment.BuildConfiguration.StartsWith("release", StringComparison.OrdinalIgnoreCase))
        {
            Assert.AreEqual(expected + 1, actual, "Incorrect min line number");
        }
        else
        {
            Assert.AreEqual(expected, actual, "Incorrect min line number");
        }
    }
}
