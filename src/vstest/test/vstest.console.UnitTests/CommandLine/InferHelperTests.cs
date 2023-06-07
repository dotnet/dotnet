// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Versioning;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLineUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.CommandLine;

[TestClass]
public class InferHelperTests
{
    private readonly Mock<IAssemblyMetadataProvider> _mockAssemblyHelper;
    private readonly InferHelper _inferHelper;
    private readonly Framework _defaultFramework = Framework.DefaultFramework;
    private readonly Architecture _defaultArchitecture = Architecture.X64;
    private readonly Framework _frameworkNet45 = Framework.FromString(".NETFramework,Version=4.5")!;
    private readonly Framework _frameworkNet46 = Framework.FromString(".NETFramework,Version=4.6")!;
    private readonly Framework _frameworkNet47 = Framework.FromString(".NETFramework,Version=4.7")!;
    private readonly Framework _frameworkCore10 = Framework.FromString(".NETCoreApp,Version=1.0")!;
    private readonly Framework _frameworkCore11 = Framework.FromString(".NETCoreApp,Version=1.1")!;

    public InferHelperTests()
    {
        _mockAssemblyHelper = new Mock<IAssemblyMetadataProvider>();
        _inferHelper = new InferHelper(_mockAssemblyHelper.Object);
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchitectureOnNullSources()
    {
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(null, _defaultArchitecture, out _));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchitectureOnEmptySources()
    {
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>(0), _defaultArchitecture, out _));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchitectureOnNullItemInSources()
    {
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { null! }, _defaultArchitecture, out _));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchitectureOnWhiteSpaceItemInSources()
    {
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { " " }, _defaultArchitecture, out _));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnCorrectArchForOneSource()
    {
        _mockAssemblyHelper.Setup(ah => ah.GetArchitecture(It.IsAny<string>())).Returns(Architecture.X86);
        Assert.AreEqual(Architecture.X86, _inferHelper.AutoDetectArchitecture(new List<string>() { "1.dll" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnCorrectDefaultArchForNotDotNetAssembly()
    {
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { "NotDotNetAssebly.appx" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldSetDefaultArchForNotDotNetAssembly()
    {
        _inferHelper.AutoDetectArchitecture(new List<string>() { "NotDotNetAssebly.appx" }, _defaultArchitecture, out var sourceArchitectures);
        Assert.AreEqual(_defaultArchitecture, sourceArchitectures["NotDotNetAssebly.appx"]);
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchForAllAnyCpuAssemblies()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.AnyCPU).Returns(Architecture.AnyCPU).Returns(Architecture.AnyCPU);
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { "AnyCPU1.dll", "AnyCPU2.exe", "AnyCPU3.dll" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnX86ArchIfOneX86AssemblyAndRestAnyCPU()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.AnyCPU).Returns(Architecture.AnyCPU).Returns(Architecture.X86);
        Assert.AreEqual(Architecture.X86, _inferHelper.AutoDetectArchitecture(new List<string>() { "AnyCPU1.dll", "AnyCPU2.exe", "x86.dll" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnARMArchIfOneARMAssemblyAndRestAnyCPU()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM).Returns(Architecture.ARM).Returns(Architecture.ARM);
        Assert.AreEqual(Architecture.ARM, _inferHelper.AutoDetectArchitecture(new List<string>() { "ARM1.dll", "ARM2.dll", "ARM3.dll" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnX64ArchIfOneX64AssemblyAndRestAnyCPU()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.AnyCPU).Returns(Architecture.AnyCPU).Returns(Architecture.X64);
        Assert.AreEqual(Architecture.X64, _inferHelper.AutoDetectArchitecture(new List<string>() { "x64.dll", "AnyCPU2.exe", "x64-2.dll" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchOnConflictArches()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.AnyCPU).Returns(Architecture.X64).Returns(Architecture.X86);
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { "AnyCPU1.dll", "x64.exe", "x86.dll" }, _defaultArchitecture, out _));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldPopulateSourceArchitectureDictionary()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.AnyCPU).Returns(Architecture.X64).Returns(Architecture.X86);

        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { "AnyCPU1.dll", "x64.exe", "x86.dll" }, _defaultArchitecture, out var sourceArchitectures));
        Assert.AreEqual(3, sourceArchitectures.Count);
        Assert.AreEqual(_defaultArchitecture, sourceArchitectures["AnyCPU1.dll"]);
        Assert.AreEqual(Architecture.X64, sourceArchitectures["x64.exe"]);
        Assert.AreEqual(Architecture.X86, sourceArchitectures["x86.dll"]);

        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectArchitectureShouldReturnDefaultArchIfthereIsNotDotNetAssemblyInSources()
    {
        _mockAssemblyHelper.SetupSequence(ah => ah.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.AnyCPU);
        Assert.AreEqual(_defaultArchitecture, _inferHelper.AutoDetectArchitecture(new List<string>() { "AnyCPU1.dll", "NotDotNetAssebly.appx" }, _defaultArchitecture, out var sourceArchitectures));
        _mockAssemblyHelper.Verify(ah => ah.GetArchitecture(It.IsAny<string>()), Times.Exactly(1));
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnDefaultFrameworkOnNullSources()
    {
        Assert.AreEqual(_defaultFramework, _inferHelper.AutoDetectFramework(null, out _));
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnDefaultFrameworkOnEmptySources()
    {
        Assert.AreEqual(_defaultFramework, _inferHelper.AutoDetectFramework(new List<string?>(0), out _));
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnDefaultFrameworkOnNullItemInSources()
    {
        Assert.AreEqual(_defaultFramework, _inferHelper.AutoDetectFramework(new List<string?>() { null! }, out _));
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnDefaultFrameworkOnEmptyItemInSources()
    {
        Assert.AreEqual(_defaultFramework.Name, _inferHelper.AutoDetectFramework(new List<string?>() { string.Empty }, out _).Name);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnFrameworkCore10OnCore10Sources()
    {
        var fx = _frameworkCore10;
        var assemblyName = "netcoreapp.dll";
        SetupAndValidateForSingleAssembly(assemblyName, fx, true);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnFramework46On46Sources()
    {
        var fx = _frameworkNet46;
        var assemblyName = "net46.dll";
        SetupAndValidateForSingleAssembly(assemblyName, fx, true);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnFrameworkUap10ForAppxFiles()
    {
        var fx = Framework.FromString(Constants.DotNetFrameworkUap10)!;
        var assemblyName = "uwp10.appx";
        SetupAndValidateForSingleAssembly(assemblyName, fx, false);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnFrameworkUap10ForMsixFiles()
    {
        var fx = Framework.FromString(Constants.DotNetFrameworkUap10)!;
        var assemblyName = "uwp10.msix";
        SetupAndValidateForSingleAssembly(assemblyName, fx, false);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnFrameworkUap10ForAppxrecipeFiles()
    {
        var fx = Framework.FromString(Constants.DotNetFrameworkUap10)!;
        var assemblyName = "uwp10.appxrecipe";
        SetupAndValidateForSingleAssembly(assemblyName, fx, false);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnDefaultFullFrameworkForJsFiles()
    {
        var fx = Framework.FromString(Constants.DotNetFramework40)!;
        var assemblyName = "vstests.js";
        SetupAndValidateForSingleAssembly(assemblyName, fx, false);
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnHighestVersionFxOnSameFxName()
    {
        _mockAssemblyHelper.SetupSequence(sh => sh.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(_frameworkNet46.Name))
            .Returns(new FrameworkName(_frameworkNet47.Name))
            .Returns(new FrameworkName(_frameworkNet45.Name));
        Assert.AreEqual(_frameworkNet47.Name, _inferHelper.AutoDetectFramework(new List<string?>() { "net46.dll", "net47.exe", "net45.dll" }, out _).Name);
        _mockAssemblyHelper.Verify(ah => ah.GetFrameworkName(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldPopulatetheDictionaryForAllTheSources()
    {
        _mockAssemblyHelper.SetupSequence(sh => sh.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(_frameworkNet46.Name))
            .Returns(new FrameworkName(_frameworkNet47.Name))
            .Returns(new FrameworkName(_frameworkNet45.Name));

        Assert.AreEqual(_frameworkNet47.Name, _inferHelper.AutoDetectFramework(new List<string?>() { "net46.dll", "net47.exe", "net45.dll" }, out var sourceFrameworks).Name);

        Assert.AreEqual(3, sourceFrameworks.Count);
        Assert.AreEqual(_frameworkNet46.Name, sourceFrameworks["net46.dll"].Name);
        Assert.AreEqual(_frameworkNet47.Name, sourceFrameworks["net47.exe"].Name);
        Assert.AreEqual(_frameworkNet45.Name, sourceFrameworks["net45.dll"].Name);
        _mockAssemblyHelper.Verify(ah => ah.GetFrameworkName(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void AutoDetectFrameworkShouldReturnHighestVersionFxOnEvenManyLowerVersionFxNameExists()
    {
        _mockAssemblyHelper.SetupSequence(sh => sh.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(_frameworkCore10.Name))
            .Returns(new FrameworkName(_frameworkCore11.Name))
            .Returns(new FrameworkName(_frameworkCore10.Name));
        Assert.AreEqual(_frameworkCore11.Name, _inferHelper.AutoDetectFramework(new List<string?>() { "netcore10_1.dll", "netcore11.dll", "netcore10_2.dll" }, out _).Name);
        _mockAssemblyHelper.Verify(ah => ah.GetFrameworkName(It.IsAny<string>()), Times.Exactly(3));
    }

    private void SetupAndValidateForSingleAssembly(string assemblyName, Framework fx, bool verify)
    {
        _mockAssemblyHelper.Setup(sh => sh.GetFrameworkName(assemblyName))
            .Returns(new FrameworkName(fx.Name));
        Assert.AreEqual(fx.Name, _inferHelper.AutoDetectFramework(new List<string?>() { assemblyName }, out _).Name);
        if (verify)
        {
            _mockAssemblyHelper.Verify(ah => ah.GetFrameworkName(assemblyName));
        }
    }
}
