// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

using Moq;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests.Helpers;

[TestClass]
public sealed class DotnetHostHelperTest : IDisposable
{
    private readonly Mock<IFileHelper> _fileHelper = new();
    private readonly Mock<IProcessHelper> _processHelper = new();
    private readonly Mock<IEnvironment> _environmentHelper = new();
    private readonly Mock<IWindowsRegistryHelper> _windowsRegistrytHelper = new();
    private readonly Mock<IEnvironmentVariableHelper> _environmentVariableHelper = new();
    private readonly MockMuxerHelper _muxerHelper = new();

    [TestMethod]
    public void GetDotnetPathByArchitecture_SameArchitecture()
    {
        // Arrange
        string finalMuxerPath = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, PlatformArchitecture.X64);
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        _environmentHelper.SetupGet(x => x.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        _environmentHelper.SetupGet(x => x.Architecture).Returns(PlatformArchitecture.X64);
        _processHelper.Setup(x => x.GetCurrentProcessFileName()).Returns(finalMuxerPath);
        _processHelper.Setup(x => x.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.X64);

        // Act & Assert
        Assert.IsTrue(dotnetHostHelper.TryGetDotnetPathByArchitecture(PlatformArchitecture.X64, DotnetMuxerResolutionStrategy.Default, out string? muxerPath));
        Assert.AreEqual(finalMuxerPath, muxerPath);
    }

    [DataTestMethod]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT(x86)")]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT")]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT(x86)", true, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT", true, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]

    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT(x86)", false)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT_ARM64")]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT")]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT(x86)", false, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT_ARM64", true, DotnetMuxerResolutionStrategy.DotnetRootArchitecture)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT", true, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]

    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT(x86)", false)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.Windows, "DOTNET_ROOT_X64")]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.Windows, "DOTNET_ROOT")]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, PlatformOperatingSystem.Windows, "DOTNET_ROOT(x86)", false, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.Windows, "DOTNET_ROOT_X64", true, DotnetMuxerResolutionStrategy.DotnetRootArchitecture)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.Windows, "DOTNET_ROOT", true, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]

    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.OSX, "DOTNET_ROOT_ARM64")]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.OSX, "DOTNET_ROOT")]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.OSX, "DOTNET_ROOT_ARM64", true, DotnetMuxerResolutionStrategy.DotnetRootArchitecture)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, PlatformOperatingSystem.OSX, "DOTNET_ROOT", true, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]

    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.OSX, "DOTNET_ROOT_X64")]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.OSX, "DOTNET_ROOT")]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.OSX, "DOTNET_ROOT_X64", true, DotnetMuxerResolutionStrategy.DotnetRootArchitecture)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, PlatformOperatingSystem.OSX, "DOTNET_ROOT", true, DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]
    public void GetDotnetPathByArchitecture_EnvVars(PlatformArchitecture targetArchitecture,
        PlatformArchitecture platformArchitecture,
        PlatformOperatingSystem platformSystem,
        string envVar,
        bool found = true,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        string dotnetRootX64 = _muxerHelper.RenameMuxerAndReturnPath(platformSystem, PlatformArchitecture.X64);
        string dotnetRootArm64 = _muxerHelper.RenameMuxerAndReturnPath(platformSystem, PlatformArchitecture.ARM64);
        string? dotnetRootX86 = platformSystem == PlatformOperatingSystem.Windows
            ? _muxerHelper.RenameMuxerAndReturnPath(platformSystem, PlatformArchitecture.X86)
            : null;
        string dotnetRoot = _muxerHelper.RenameMuxerAndReturnPath(platformSystem, targetArchitecture);
        Dictionary<string, string?> envVars = new()
        {
            { "DOTNET_ROOT_X64", dotnetRootX64 },
            { "DOTNET_ROOT_ARM64", dotnetRootArm64 },
            { "DOTNET_ROOT(x86)", dotnetRootX86 },
            { "DOTNET_ROOT", dotnetRoot },
        };

        _environmentHelper.SetupGet(x => x.Architecture).Returns(platformArchitecture);
        _environmentHelper.SetupGet(x => x.OperatingSystem).Returns(platformSystem);
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable(envVar)).Returns(Path.GetDirectoryName(envVars[envVar])!);
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable("ProgramFiles")).Returns("notfound");
        _fileHelper.Setup(x => x.DirectoryExists(Path.GetDirectoryName(envVars[envVar])!)).Returns(true);
        _fileHelper.Setup(x => x.Exists(envVars[envVar])).Returns(true);
        if (found)
        {
            _fileHelper.Setup(x => x.GetStream(envVars[envVar]!, FileMode.Open, FileAccess.Read)).Returns(File.OpenRead(envVars[envVar]!));
        }

        // Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.AreEqual(found, dotnetHostHelper.TryGetDotnetPathByArchitecture(targetArchitecture, strategy, out string? muxerPath));
        Assert.AreEqual(found ? envVars[envVar] : null, muxerPath);
    }

    [DataTestMethod]
    [DataRow("DOTNET_ROOT_ARM64", "DOTNET_ROOT", PlatformArchitecture.ARM64, PlatformArchitecture.X64)]
    [DataRow("DOTNET_ROOT(x86)", "DOTNET_ROOT", PlatformArchitecture.X86, PlatformArchitecture.X64)]
    [DataRow("DOTNET_ROOT_ARM64", "DOTNET_ROOT", PlatformArchitecture.ARM64, PlatformArchitecture.X64, DotnetMuxerResolutionStrategy.DotnetRootArchitecture | DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]
    [DataRow("DOTNET_ROOT(x86)", "DOTNET_ROOT", PlatformArchitecture.X86, PlatformArchitecture.X64, DotnetMuxerResolutionStrategy.DotnetRootArchitecture | DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess)]
    public void GetDotnetPathByArchitecture_EnvVars_DirectoryNotExists_TryNext(
        string notExists,
        string nextEnv,
        PlatformArchitecture targetAchitecture,
        PlatformArchitecture platformArchitecture,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        string dotnetRootX64 = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, PlatformArchitecture.X64);
        string dotnetRootArm64 = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, PlatformArchitecture.ARM64);
        string dotnetRootX86 = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, PlatformArchitecture.X86);
        string dotnetRoot = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, targetAchitecture);
        Dictionary<string, string> envVars = new()
        {
            { "DOTNET_ROOT_X64", dotnetRootX64 },
            { "DOTNET_ROOT_ARM64", dotnetRootArm64 },
            { "DOTNET_ROOT(x86)", dotnetRootX86 },
            { "DOTNET_ROOT", dotnetRoot },
        };

        _environmentHelper.SetupGet(x => x.Architecture).Returns(platformArchitecture);
        _environmentHelper.SetupGet(x => x.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable(notExists)).Returns(Path.GetDirectoryName(envVars[notExists])!);
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable(nextEnv)).Returns(Path.GetDirectoryName(envVars[nextEnv])!);
        _fileHelper.Setup(x => x.DirectoryExists(Path.GetDirectoryName(envVars[nextEnv])!)).Returns(true);
        _fileHelper.Setup(x => x.Exists(envVars[nextEnv])).Returns(true);
        _fileHelper.Setup(x => x.GetStream(envVars[nextEnv], FileMode.Open, FileAccess.Read)).Returns(File.OpenRead(envVars[nextEnv]));

        //Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.IsTrue(dotnetHostHelper.TryGetDotnetPathByArchitecture(targetAchitecture, strategy, out string? muxerPath));
        Assert.AreEqual(envVars[nextEnv], muxerPath);
    }

    [DataTestMethod]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, true)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X86, false)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, true, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X86, false, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    public void GetDotnetPathByArchitecture_GlobalInstallation_Windows(
        PlatformArchitecture muxerArchitecture,
        PlatformArchitecture targetArchitecture,
        bool found,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        string dotnetMuxer = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, muxerArchitecture);
        Mock<IRegistryKey> installedVersionKey = new();
        Mock<IRegistryKey> architectureSubKey = new();
        Mock<IRegistryKey> nativeArchSubKey = new();
        installedVersionKey.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(architectureSubKey.Object);
        architectureSubKey.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(nativeArchSubKey.Object);
        nativeArchSubKey.Setup(x => x.GetValue(It.IsAny<string>())).Returns(Path.GetDirectoryName(dotnetMuxer)!);
        _windowsRegistrytHelper.Setup(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)).Returns(installedVersionKey.Object);
        _fileHelper.Setup(x => x.Exists(dotnetMuxer)).Returns(true);
        _fileHelper.Setup(x => x.GetStream(dotnetMuxer, FileMode.Open, FileAccess.Read)).Returns(File.OpenRead(dotnetMuxer));

        //Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.AreEqual(found, dotnetHostHelper.TryGetDotnetPathByArchitecture(targetArchitecture, strategy, out string? muxerPath));
        Assert.AreEqual(found ? dotnetMuxer : null, muxerPath);
    }

    [DataTestMethod]
    [DataRow(true, false, false, false)]
    [DataRow(false, true, false, false)]
    [DataRow(false, false, true, false)]
    [DataRow(false, false, false, true)]
    [DataRow(true, false, false, false, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(false, true, false, false, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(false, false, true, false, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(false, false, false, true, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    public void GetDotnetPathByArchitecture_GlobalInstallation_NullSubkeys(
        bool nullInstalledVersion,
        bool nullArchitecture,
        bool nullNative,
        bool nullInstallLocation,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        Mock<IRegistryKey> installedVersionKey = new();
        Mock<IRegistryKey> architectureSubKey = new();
        Mock<IRegistryKey> nativeArchSubKey = new();
        installedVersionKey.Setup(x => x.OpenSubKey(It.IsAny<string>()))
            .Returns(nullArchitecture ? null! : architectureSubKey.Object);
        architectureSubKey.Setup(x => x.OpenSubKey(It.IsAny<string>()))
            .Returns(nullNative ? null! : nativeArchSubKey.Object);
        nativeArchSubKey.Setup(x => x.GetValue(It.IsAny<string>()))
            .Returns(nullInstallLocation ? null! : "");
        _windowsRegistrytHelper.Setup(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            .Returns(nullInstalledVersion ? null! : installedVersionKey.Object);
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable("ProgramFiles")).Returns("notfound");

        // Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.IsFalse(dotnetHostHelper.TryGetDotnetPathByArchitecture(PlatformArchitecture.X64, strategy, out string? muxerPath));
    }

    [DataTestMethod]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_arm64", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location_x64", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_x64", false, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_arm64", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location_x64", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_x64", false, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]

    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_arm64", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location_x64", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_x64", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_arm64", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location_x64", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, "/etc/dotnet/install_location", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    [DataRow(PlatformArchitecture.ARM64, "/etc/dotnet/install_location_x64", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.GlobalInstallationLocation)]
    public void GetDotnetPathByArchitecture_GlobalInstallation_Unix(
        PlatformArchitecture targetArchitecture,
        string installLocation,
        bool found,
        PlatformOperatingSystem os,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        string dotnetMuxer = _muxerHelper.RenameMuxerAndReturnPath(os, targetArchitecture);
        _environmentHelper.SetupGet(x => x.OperatingSystem).Returns(os);
        _fileHelper.Setup(x => x.Exists(installLocation)).Returns(true);
        _fileHelper.Setup(x => x.Exists(dotnetMuxer)).Returns(true);
        _fileHelper.Setup(x => x.GetStream(installLocation, FileMode.Open, FileAccess.Read)).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Path.GetDirectoryName(dotnetMuxer)!)));
        if (found)
        {
            _fileHelper.Setup(x => x.GetStream(dotnetMuxer, FileMode.Open, FileAccess.Read)).Returns(File.OpenRead(dotnetMuxer));
        }

        //Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.AreEqual(found, dotnetHostHelper.TryGetDotnetPathByArchitecture(targetArchitecture, strategy, out string? muxerPath));
        Assert.AreEqual(found ? dotnetMuxer : null, muxerPath);
    }

    [DataTestMethod]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X64, "ProgramFiles(x86)", "dotnet", true)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, "ProgramFiles", @"dotnet\x64", true)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "ProgramFiles", "dotnet", true)]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X86, "ProgramFiles", "dotnet", true)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "ProgramFiles", "dotnet", false)]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X64, "ProgramFiles(x86)", "dotnet", true, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, "ProgramFiles", @"dotnet\x64", true, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "ProgramFiles", "dotnet", true, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X86, PlatformArchitecture.X86, "ProgramFiles", "dotnet", true, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "ProgramFiles", "dotnet", false, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [TestCategory("Windows")]
    public void GetDotnetPathByArchitecture_DefaultInstallation_Win(
        PlatformArchitecture targetArchitecture,
        PlatformArchitecture platformArchitecture,
        string envVar,
        string subfolder,
        bool found,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        string dotnetMuxer = _muxerHelper.RenameMuxerAndReturnPath(PlatformOperatingSystem.Windows, targetArchitecture, subfolder);
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable(envVar)).Returns(dotnetMuxer.Replace(Path.Combine(subfolder, "dotnet.exe"), string.Empty));
        _environmentHelper.Setup(x => x.Architecture).Returns(platformArchitecture);
        if (found)
        {
            _fileHelper.Setup(x => x.Exists(dotnetMuxer)).Returns(true);
            _fileHelper.Setup(x => x.GetStream(dotnetMuxer, FileMode.Open, FileAccess.Read)).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Path.GetDirectoryName(dotnetMuxer)!)));
            _fileHelper.Setup(x => x.GetStream(dotnetMuxer, FileMode.Open, FileAccess.Read)).Returns(File.OpenRead(dotnetMuxer));
        }

        //Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.AreEqual(found, dotnetHostHelper.TryGetDotnetPathByArchitecture(targetArchitecture, strategy, out string? muxerPath));
        Assert.AreEqual(found ? dotnetMuxer : null, muxerPath);
    }

#pragma warning disable MSTEST0042 // duplicate data row - TODO: Look more into it
    [DataTestMethod]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/local/share/dotnet", "", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, "/usr/local/share/dotnet/x64", "", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, "/usr/local/share/dotnet", "", true, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/local/share/dotnet", "", false, PlatformOperatingSystem.OSX)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/local/share/dotnet", "", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, "/usr/local/share/dotnet/x64", "", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, "/usr/local/share/dotnet", "", true, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/local/share/dotnet", "", false, PlatformOperatingSystem.OSX, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]

    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/share/dotnet", "", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, "/usr/share/dotnet/x64", "", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, "/usr/share/dotnet", "", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/share/dotnet", "", false, PlatformOperatingSystem.Unix)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/share/dotnet", "", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.ARM64, "/usr/share/dotnet/x64", "", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.ARM64, PlatformArchitecture.X64, "/usr/share/dotnet", "", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
    [DataRow(PlatformArchitecture.X64, PlatformArchitecture.X64, "/usr/share/dotnet", "", false, PlatformOperatingSystem.Unix, DotnetMuxerResolutionStrategy.DefaultInstallationLocation)]
#pragma warning restore MSTEST0042
    public void GetDotnetPathByArchitecture_DefaultInstallation_Unix(
        PlatformArchitecture targetArchitecture,
        PlatformArchitecture platformArchitecture,
        string expectedFolder,
        string subfolder,
        bool found,
        PlatformOperatingSystem os,
        DotnetMuxerResolutionStrategy strategy = DotnetMuxerResolutionStrategy.Default)
    {
        // Arrange
        string dotnetMuxer = _muxerHelper.RenameMuxerAndReturnPath(os, targetArchitecture, subfolder);
        _environmentHelper.SetupGet(x => x.OperatingSystem).Returns(os);
        _environmentHelper.Setup(x => x.Architecture).Returns(platformArchitecture);
        string expectedMuxerPath = Path.Combine(expectedFolder, "dotnet");
        _fileHelper.Setup(x => x.Exists(expectedMuxerPath)).Returns(true);
        _fileHelper.Setup(x => x.GetStream(expectedMuxerPath, FileMode.Open, FileAccess.Read)).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Path.GetDirectoryName(dotnetMuxer)!)));
        if (found)
        {
            _fileHelper.Setup(x => x.GetStream(expectedMuxerPath, FileMode.Open, FileAccess.Read)).Returns(File.OpenRead(dotnetMuxer));
        }

        //Act & Assert
        var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
        Assert.AreEqual(found, dotnetHostHelper.TryGetDotnetPathByArchitecture(targetArchitecture, strategy, out string? muxerPath));
        Assert.AreEqual(found ? expectedMuxerPath : null, muxerPath);
    }

    [TestMethod]
    public void GetDotnetPathByArchitecture_Strategies()
    {
        foreach (DotnetMuxerResolutionStrategy strategy in Enum.GetValues(typeof(DotnetMuxerResolutionStrategy)))
        {
            // Arrange
            _environmentHelper.Reset();
            _fileHelper.Reset();
            _windowsRegistrytHelper.Reset();
            _environmentVariableHelper.Reset();
            _processHelper.Reset();

            _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable("ProgramFiles")).Returns("notfound");
            var dotnetHostHelper = new DotnetHostHelper(_fileHelper.Object, _environmentHelper.Object, _windowsRegistrytHelper.Object, _environmentVariableHelper.Object, _processHelper.Object);
            dotnetHostHelper.TryGetDotnetPathByArchitecture(PlatformArchitecture.X64, strategy, out string? _);

            // Assert
            switch (strategy)
            {
                case DotnetMuxerResolutionStrategy.DotnetRootArchitecture:
                    // Assert env vars
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT_X64"), Times.Once);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT"), Times.Never);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Once);

                    // Assert local installation
                    _windowsRegistrytHelper.Verify(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32), Times.Never);

                    // Assert default installation folder
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("ProgramFiles"), Times.Never);

                    break;
                case DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess:
                    // Assert env vars
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT_X64"), Times.Never);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT"), Times.Once);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Once);

                    // Assert local installation
                    _windowsRegistrytHelper.Verify(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32), Times.Never);

                    // Assert default installation folder
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("ProgramFiles"), Times.Never);

                    break;
                case DotnetMuxerResolutionStrategy.GlobalInstallationLocation:
                    // Assert env vars
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT_X64"), Times.Never);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT"), Times.Never);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);

                    // Assert local installation
                    _windowsRegistrytHelper.Verify(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32), Times.Once);

                    // Assert default installation folder
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("ProgramFiles"), Times.Never);

                    break;
                case DotnetMuxerResolutionStrategy.DefaultInstallationLocation:
                    // Assert env vars
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT_X64"), Times.Never);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT"), Times.Never);

                    // Assert local installation
                    _windowsRegistrytHelper.Verify(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32), Times.Never);

                    // Assert default installation folder
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("ProgramFiles"), Times.Once);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Once);

                    break;
                case DotnetMuxerResolutionStrategy.Default:

                    // Assert env vars
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT_X64"), Times.Once);
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_ROOT"), Times.Once);

                    // Assert local installation
                    _windowsRegistrytHelper.Verify(x => x.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32), Times.Once);

                    // Assert default installation folder
                    _environmentVariableHelper.Verify(x => x.GetEnvironmentVariable("ProgramFiles"), Times.Once);

                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public void Dispose() => _muxerHelper.Dispose();

    private class MockMuxerHelper : IDisposable
    {
        private static readonly string DotnetMuxerWinX86 = "TestAssets/dotnetWinX86.exe";
        private static readonly string DotnetMuxerWinX64 = "TestAssets/dotnetWinX64.exe";
        private static readonly string DotnetMuxerWinArm64 = "TestAssets/dotnetWinArm64.exe";
        private static readonly string DotnetMuxerMacArm64 = "TestAssets/dotnetMacArm64";
        private static readonly string DotnetMuxerMacX64 = "TestAssets/dotnetMacX64";
        private readonly List<string> _muxers = new();

        public MockMuxerHelper()
        {
            Assert.IsTrue(File.Exists(DotnetMuxerWinX86));
            Assert.IsTrue(File.Exists(DotnetMuxerWinX64));
            Assert.IsTrue(File.Exists(DotnetMuxerWinArm64));
            Assert.IsTrue(File.Exists(DotnetMuxerMacArm64));
            Assert.IsTrue(File.Exists(DotnetMuxerMacX64));
        }

        public string RenameMuxerAndReturnPath(PlatformOperatingSystem platform, PlatformArchitecture architecture, string subfolder = "")
        {
            string tmpDirectory = Path.GetTempPath();
            string muxerPath;
            switch (platform)
            {
                case PlatformOperatingSystem.Windows:
                    {
                        muxerPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString("N"), subfolder, "dotnet.exe");
                        Directory.CreateDirectory(Path.GetDirectoryName(muxerPath)!);
                        if (architecture == PlatformArchitecture.ARM64)
                        {
                            File.Copy(DotnetMuxerWinArm64, muxerPath);
                            break;
                        }
                        else if (architecture == PlatformArchitecture.X64)
                        {
                            File.Copy(DotnetMuxerWinX64, muxerPath);
                            break;
                        }
                        else if (architecture == PlatformArchitecture.X86)
                        {
                            File.Copy(DotnetMuxerWinX86, muxerPath);
                            break;
                        }

                        throw new NotSupportedException($"Unsupported architecture '{architecture}'");
                    }
                case PlatformOperatingSystem.OSX:
                    {
                        muxerPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString("N"), subfolder, "dotnet");
                        Directory.CreateDirectory(Path.GetDirectoryName(muxerPath)!);
                        if (architecture == PlatformArchitecture.ARM64)
                        {
                            File.Copy(DotnetMuxerMacArm64, muxerPath);
                            break;
                        }
                        else if (architecture == PlatformArchitecture.X64)
                        {
                            File.Copy(DotnetMuxerMacX64, muxerPath);
                            break;
                        }

                        throw new NotSupportedException($"Unsupported architecture '{architecture}'");
                    }
                case PlatformOperatingSystem.Unix:
                    {
                        muxerPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString("N"), subfolder, "dotnet");
                        Directory.CreateDirectory(Path.GetDirectoryName(muxerPath)!);
                        File.WriteAllText(muxerPath, "not supported");
                        break;
                    }
                default:
                    throw new NotSupportedException($"Unsupported OS '{platform}'");
            }

            _muxers.Add(muxerPath);
            return muxerPath;
        }

        public void Dispose()
        {
            foreach (var muxer in _muxers)
            {
                Directory.Delete(Path.GetDirectoryName(muxer)!, true);
            }
        }
    }
}
