// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

[TestClass]
public class MtpServerConnectionTests
{
    private const int Port = 12345;

    private string _tempDir = null!;

    [TestInitialize]
    public void SetUp()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "mtp-buildlaunch-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDir);
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, recursive: true);
            }
        }
        catch
        {
            // best-effort cleanup
        }
    }

    [TestMethod]
    public void BuildLaunchWhenSourceIsExeLaunchesItDirectly()
    {
        string exe = Path.Combine(_tempDir, "Foo.exe");
        File.WriteAllText(exe, string.Empty);

        var (fileName, arguments, workingDirectory) = MtpServerConnection.BuildLaunch(exe, Port);

        Assert.AreEqual(exe, fileName);
        Assert.DoesNotContain("\"", arguments);
        Assert.AreEqual(_tempDir, workingDirectory);
    }

    [TestMethod]
    public void BuildLaunchWhenDllHasNoApphostFallsBackToDotnet()
    {
        string dll = Path.Combine(_tempDir, "Foo.dll");
        File.WriteAllText(dll, string.Empty);

        var (fileName, arguments, _) = MtpServerConnection.BuildLaunch(dll, Port);

        Assert.AreEqual("dotnet", fileName);
        Assert.Contains($"\"{dll}\"", arguments);
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux | OperatingSystems.OSX)]
    public void BuildLaunchOnUnixIgnoresSiblingWindowsExeAndFallsBackToDotnet()
    {
        string dll = Path.Combine(_tempDir, "Foo.dll");
        File.WriteAllText(dll, string.Empty);

        // Stand in for a Windows PE apphost dragged along in a Windows-built payload that is then
        // unzipped on Linux: the file exists but is not a native Unix executable.
        File.WriteAllText(Path.Combine(_tempDir, "Foo.exe"), string.Empty);

        var (fileName, arguments, _) = MtpServerConnection.BuildLaunch(dll, Port);

        Assert.AreEqual("dotnet", fileName);
        Assert.Contains($"\"{dll}\"", arguments);
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Windows)]
    public void BuildLaunchOnWindowsSelectsSiblingExeApphost()
    {
        string dll = Path.Combine(_tempDir, "Foo.dll");
        File.WriteAllText(dll, string.Empty);

        // On Windows the apphost is <name>.exe and its mere presence is sufficient.
        string exe = Path.Combine(_tempDir, "Foo.exe");
        File.WriteAllText(exe, string.Empty);

        var (fileName, arguments, _) = MtpServerConnection.BuildLaunch(dll, Port);

        Assert.AreEqual(exe, fileName);
        Assert.DoesNotContain("\"", arguments);
    }

    [TestMethod]
    public void IsUsableApphostReturnsFalseWhenFileMissing()
    {
        Assert.IsFalse(MtpServerConnection.IsUsableApphost(Path.Combine(_tempDir, "does-not-exist")));
    }

#if NET
    [TestMethod]
    [OSCondition(OperatingSystems.Linux | OperatingSystems.OSX)]
    public void IsUsableApphostOnUnixReturnsFalseForNonExecutableFile()
    {
        string apphost = Path.Combine(_tempDir, "Foo");
        File.WriteAllText(apphost, string.Empty);
        File.SetUnixFileMode(apphost, UnixFileMode.UserRead | UnixFileMode.UserWrite);

        Assert.IsFalse(MtpServerConnection.IsUsableApphost(apphost));
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux | OperatingSystems.OSX)]
    public void IsUsableApphostOnUnixReturnsTrueForExecutableFile()
    {
        string apphost = Path.Combine(_tempDir, "Foo");
        File.WriteAllText(apphost, string.Empty);
        File.SetUnixFileMode(apphost, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);

        Assert.IsTrue(MtpServerConnection.IsUsableApphost(apphost));
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux | OperatingSystems.OSX)]
    public void BuildLaunchOnUnixSelectsExecutableExtensionlessApphost()
    {
        string dll = Path.Combine(_tempDir, "Foo.dll");
        File.WriteAllText(dll, string.Empty);

        string apphost = Path.Combine(_tempDir, "Foo");
        File.WriteAllText(apphost, string.Empty);
        File.SetUnixFileMode(apphost, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);

        var (fileName, _, _) = MtpServerConnection.BuildLaunch(dll, Port);

        Assert.AreEqual(apphost, fileName);
    }
#endif
}
