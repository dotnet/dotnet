// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class AssemblyMetadataProviderTests : AcceptanceTestBase
{
    private const int ExpectedTimeForFindingArchForDotNetAssembly = 15; // In milliseconds.
    private const string PerfAssertMessageFormat = "Expected Elapsed Time: {0} ms, Actual Elapsed Time: {1} ms";

    private readonly IAssemblyMetadataProvider _assemblyMetadataProvider;

    private readonly Mock<IFileHelper> _fileHelperMock;

    private readonly FileHelper _fileHelper;

    private bool _isManagedAssemblyArchitectureTest;

    public AssemblyMetadataProviderTests()
    {
        _fileHelper = new FileHelper();
        _fileHelperMock = new Mock<IFileHelper>();
        _isManagedAssemblyArchitectureTest = false;
        _assemblyMetadataProvider = new AssemblyMetadataProvider(_fileHelperMock.Object);

        _fileHelperMock.Setup(f =>
                f.GetStream(It.IsAny<string>(), FileMode.Open, FileAccess.Read))
            .Returns<string, FileMode, FileAccess>((filePath, mode, access) => _fileHelper.GetStream(filePath, mode, access));
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (!_isManagedAssemblyArchitectureTest)
        {
            _fileHelperMock.Verify(
                f => f.GetStream(It.IsAny<string>(), FileMode.Open, FileAccess.Read), Times.Once);
        }
    }

    [TestMethod]
    [DataRow("net462")]
    [DataRow("net8.0")]
    public void GetArchitectureShouldReturnCorrentArchForx64Assembly(string framework)
    {
        TestDotnetAssemblyArch("SimpleTestProject3", framework, Architecture.X64, expectedElapsedTime: ExpectedTimeForFindingArchForDotNetAssembly);
    }

    [TestMethod]
    [DataRow("net462")]
    [DataRow("net8.0")]
    public void GetArchitectureShouldReturnCorrentArchForx86Assembly(string framework)
    {
        TestDotnetAssemblyArch("SimpleTestProjectx86", framework, Architecture.X86, expectedElapsedTime: ExpectedTimeForFindingArchForDotNetAssembly);
    }

    [TestMethod]
    [DataRow("net462")]
    [DataRow("net8.0")]
    public void GetArchitectureShouldReturnCorrentArchForAnyCpuAssembly(string framework)
    {
        TestDotnetAssemblyArch("SimpleTestProject", framework, Architecture.AnyCPU, expectedElapsedTime: ExpectedTimeForFindingArchForDotNetAssembly);
    }

    [TestMethod]
    [DataRow("net462")]
    [DataRow("net8.0")]
    public void GetArchitectureShouldReturnCorrentArchForArm64Assembly(string framework)
    {
        TestDotnetAssemblyArch("SimpleTestProjectARM64", framework, Architecture.ARM64, expectedElapsedTime: ExpectedTimeForFindingArchForDotNetAssembly);
    }

    [TestMethod]
    [DataRow("x86")]
    [DataRow("x64")]
    public void GetArchitectureForNativeDll(string platform)
    {
        var expectedElapsedTime = 5;
        var platformPath = platform.Equals("x64") ? platform : string.Empty;
        var assemblyPath = $@"{_testEnvironment.PackageDirectory}/microsoft.testplatform.testasset.nativecpp/2.0.0/"
                           + $@"contentFiles/any/any/{platformPath}/Microsoft.TestPlatform.TestAsset.NativeCPP.dll";
        LoadAssemblyIntoMemory(assemblyPath);
        var stopWatch = Stopwatch.StartNew();
        var arch = _assemblyMetadataProvider.GetArchitecture(assemblyPath);
        stopWatch.Stop();

        Console.WriteLine("Platform:{0}, {1}", platform, string.Format(CultureInfo.CurrentCulture, PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));
        Assert.AreEqual(Enum.Parse(typeof(Architecture), platform, ignoreCase: true), arch);

        // We should not assert on time elapsed, it will vary depending on machine, & their state, commenting below assert
        // Assert.IsTrue(stopWatch.ElapsedMilliseconds < expectedElapsedTime, string.Format(PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));
    }

    [TestMethod]
    [DataRow("net462")]
    [DataRow("net8.0")]
    public void GetFrameWorkForDotNetAssembly(string framework)
    {
        var expectedElapsedTime = 5;
        var assemblyPath = _testEnvironment.GetTestAsset("SimpleTestProject3.dll", framework);
        LoadAssemblyIntoMemory(assemblyPath);
        var stopWatch = Stopwatch.StartNew();
        var actualFx = _assemblyMetadataProvider.GetFrameworkName(assemblyPath);
        stopWatch.Stop();

        if (framework.Equals("net462"))
        {
            // Reason is unknown for why full framework it is taking more time. Need to investigate.
            expectedElapsedTime = 100;
            Assert.AreEqual(".NETFramework,Version=v4.6.2", actualFx.FullName);
        }
        else
        {
            Assert.AreEqual(".NETCoreApp,Version=v8.0", actualFx.FullName);
        }

        Console.WriteLine("Framework:{0}, {1}", framework, string.Format(CultureInfo.CurrentCulture, PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));

        // We should not assert on time elapsed, it will vary depending on machine, & their state.
        // Assert.IsTrue(stopWatch.ElapsedMilliseconds < expectedElapsedTime, string.Format(PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));
    }

    [TestMethod]
    public void GetFrameworkForNativeDll()
    {
        var expectedElapsedTime = 5;
        var assemblyPath = $@"{_testEnvironment.PackageDirectory}/microsoft.testplatform.testasset.nativecpp/2.0.0/contentFiles/any/any/Microsoft.TestPlatform.TestAsset.NativeCPP.dll";
        LoadAssemblyIntoMemory(assemblyPath);
        var stopWatch = Stopwatch.StartNew();
        var fx = _assemblyMetadataProvider.GetFrameworkName(assemblyPath);
        stopWatch.Stop();

        Console.WriteLine(PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds);
        Assert.AreEqual(Framework.DefaultFramework.Name, fx.FullName);

        // We should not assert on time elapsed, it will vary depending on machine, & their state.
        // Assert.IsTrue(stopWatch.ElapsedMilliseconds < expectedElapsedTime, string.Format(PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));
    }

    private void TestDotnetAssemblyArch(string projectName, string framework, Architecture expectedArch, long expectedElapsedTime)
    {
        _isManagedAssemblyArchitectureTest = true;
        var assemblyPath = _testEnvironment.GetTestAsset(projectName + ".dll", framework);
        LoadAssemblyIntoMemory(assemblyPath);
        var stopWatch = Stopwatch.StartNew();
        var arch = _assemblyMetadataProvider.GetArchitecture(assemblyPath);
        stopWatch.Stop();

        Console.WriteLine("Framework:{0}, {1}", framework, string.Format(CultureInfo.CurrentCulture, PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));
        Assert.AreEqual(expectedArch, arch, $"Expected: {expectedArch} Actual: {arch}");

        // We should not assert on time elapsed, it will vary depending on machine, & their state.
        // Assert.IsTrue(stopWatch.ElapsedMilliseconds < expectedElapsedTime, string.Format(PerfAssertMessageFormat, expectedElapsedTime, stopWatch.ElapsedMilliseconds));
    }

    private static void LoadAssemblyIntoMemory(string assemblyPath)
    {
        // Load the file into RAM in ahead to avoid performance number(expectedElapsedTime) dependence on disk read time.
        File.ReadAllBytes(assemblyPath);
    }
}
