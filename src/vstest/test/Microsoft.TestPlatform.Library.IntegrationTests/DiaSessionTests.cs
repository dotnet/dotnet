// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Library.IntegrationTests;

[TestClass]
// TODO: these tests potentially not test anything useful that we would not test in other tests? Replace them with integration tests that have portable and full symbols test file, and that collect source info, unless we have such tests already. Right now we test just portable symbol reader it seems.
public class DiaSessionTests : AcceptanceTestBase
{
    public static string? GetAndSetTargetFrameWork(IntegrationTestEnvironment testEnvironment)
    {
        var currentTargetFrameWork = testEnvironment.TargetFramework;
        testEnvironment.TargetFramework =
#if NETFRAMEWORK
            "net481";
#else
            "net11.0";
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
        Assert.EndsWith(@"\SimpleClassLibrary\Class1.cs".Replace("\\", "/"), diaNavigationData.FileName!.Replace("\\", "/"));

        SourceAssert.LineIsAtMethodBodyStart(diaNavigationData.FileName!, "PassingTest", diaNavigationData.MinLineNumber, "Incorrect min line number");

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
        Assert.EndsWith(@"\SimpleClassLibrary\Class1.cs".Replace("\\", "/"), diaNavigationData.FileName!.Replace("\\", "/"));

        // The async state machine's MoveNext maps back to the original async method source lines.
        SourceAssert.LineIsAtMethodBodyStart(diaNavigationData.FileName!, "AsyncTestMethod", diaNavigationData.MinLineNumber, "Incorrect min line number");

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
        Assert.EndsWith(@"\SimpleClassLibrary\Class1.cs".Replace("\\", "/"), diaNavigationData.FileName!.Replace("\\", "/"));

        // DiaSession may return any overload;verify min line falls within one of them.
        SourceAssert.LineIsAtMethodBodyStart(diaNavigationData.FileName!, "OverLoadedMethod", diaNavigationData.MinLineNumber,
            $"Min line number ({diaNavigationData.MinLineNumber}) is not at the body start of any OverLoadedMethod overload.");

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
        Assert.EndsWith(@"\SimpleClassLibrary\HugeMethodSet.cs".Replace("\\", "/"), diaNavigationData.FileName!.Replace("\\", "/"));

        SourceAssert.LineIsAtMethodBodyStart(diaNavigationData.FileName!, "MSTest_D1_01", diaNavigationData.MinLineNumber, "Incorrect min line number");

        var expectedTime = 150;
        Assert.IsLessThan(expectedTime, watch.Elapsed.Milliseconds, $"DiaSession Perf test Actual time:{watch.Elapsed.Milliseconds} ms Expected time:{expectedTime} ms");

        _testEnvironment.TargetFramework = currentTargetFrameWork;
    }
}
