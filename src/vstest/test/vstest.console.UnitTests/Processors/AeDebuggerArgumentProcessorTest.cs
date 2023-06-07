// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace vstest.console.UnitTests.Processors;

[TestClass]
[TestCategory("Windows-Review")]
public class AeDebuggerArgumentProcessorTest
{
    private readonly Mock<IEnvironment> _environment = new();
    private readonly Mock<IFileHelper> _fileHelper = new();
    private readonly Mock<IProcessHelper> _processHelper = new();
    private readonly Mock<IOutput> _output = new();
    private readonly Mock<IEnvironmentVariableHelper> _environmentVariableHelper = new();
    private readonly AeDebuggerArgumentExecutor _executor;

    public AeDebuggerArgumentProcessorTest()
    {
        _executor = new AeDebuggerArgumentExecutor(_environment.Object, _fileHelper.Object, _processHelper.Object, _output.Object, _environmentVariableHelper.Object);
    }

    [TestMethod]
    public void AeDebuggerArgumentProcessorCommandName()
    {
        Assert.AreEqual("/AeDebugger", AeDebuggerArgumentProcessor.CommandName);
    }

    [TestMethod]
    public void AeDebuggerArgumentProcessorCapabilities()
    {
        AeDebuggerArgumentProcessorCapabilities aeDebuggerArgumentProcessor = new();
        Assert.IsNull(aeDebuggerArgumentProcessor.HelpContentResourceName);
        Assert.IsTrue(aeDebuggerArgumentProcessor.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, aeDebuggerArgumentProcessor.Priority);
    }

    [TestMethod]
    public void AeDebuggerArgumentProcessorReturnsCorrectTypes()
    {
        AeDebuggerArgumentProcessor aeDebuggerArgumentProcessor = new();
        Assert.IsInstanceOfType(aeDebuggerArgumentProcessor.Executor!.Value, typeof(AeDebuggerArgumentExecutor));
        Assert.IsInstanceOfType(aeDebuggerArgumentProcessor.Metadata!.Value, typeof(AeDebuggerArgumentProcessorCapabilities));
    }

    [TestMethod]
    public void AeDebuggerArgumentExecutor_InvalidCtor()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new AeDebuggerArgumentExecutor(_environment.Object, _fileHelper.Object, _processHelper.Object, _output.Object, null!));
        Assert.ThrowsException<ArgumentNullException>(() => new AeDebuggerArgumentExecutor(_environment.Object, _fileHelper.Object, _processHelper.Object, null!, _environmentVariableHelper.Object));
        Assert.ThrowsException<ArgumentNullException>(() => new AeDebuggerArgumentExecutor(_environment.Object, _fileHelper.Object, null!, _output.Object, _environmentVariableHelper.Object));
        Assert.ThrowsException<ArgumentNullException>(() => new AeDebuggerArgumentExecutor(_environment.Object, null!, _processHelper.Object, _output.Object, _environmentVariableHelper.Object));
        Assert.ThrowsException<ArgumentNullException>(() => new AeDebuggerArgumentExecutor(null!, _fileHelper.Object, _processHelper.Object, _output.Object, _environmentVariableHelper.Object));
    }

    [TestMethod]
    public void AeDebuggerArgumentExecutor_NullArgument()
    {
        _executor.Initialize(null);
        Assert.AreEqual(ArgumentProcessorResult.Fail, _executor.Execute());
        _output.Verify(x => x.WriteLine(It.IsAny<string>(), OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    [DataRow("Instal;ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath;DumpDirectoryPath=c:\\DumpDirectoryPath")]
    [DataRow("Uninstal;ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath;DumpDirectoryPath=c:\\DumpDirectoryPath")]
    public void AeDebuggerArgumentExecutor_WrongInstallUnistallCommand(string wrongCommand)
    {
        _executor.Initialize(wrongCommand);
        Assert.ThrowsException<CommandLineException>(() => _executor.Execute());
    }

    [TestMethod]
    public void AeDebuggerArgumentExecutor_NonWindowsOS()
    {
        _environment.Setup(x => x.OperatingSystem).Returns(PlatformOperatingSystem.Unix);
        _executor.Initialize("Install;ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath;DumpDirectoryPath=c:\\DumpDirectoryPath");
        Assert.AreEqual(ArgumentProcessorResult.Fail, _executor.Execute());
    }

    [TestMethod]
    [DataRow("Install;{0};DumpDirectoryPath=c:\\DumpDirectoryPath", null)]
    [DataRow("Install;{0};DumpDirectoryPath=c:\\DumpDirectoryPath", "ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPat")]
    [DataRow("Install;{0};ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath", null)]
    [DataRow("Install;{0};ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath", "DumpDirectoryPath=c:\\DumpDirectoryPat")]

    public void AeDebuggerArgumentExecutor_WrongDirectoryPaths(string command, string? directoryPath)
    {
        _fileHelper.Setup(x => x.DirectoryExists(It.IsAny<string>()))
            .Returns((string path) => directoryPath is null || !directoryPath.EndsWith(path));
        _fileHelper.Setup(x => x.Exists(It.IsAny<string>()))
            .Returns((string path) => path.EndsWith("procdump.exe") && path != "procdump.exe");
        _executor.Initialize(string.Format(CultureInfo.InvariantCulture, command, directoryPath));
        Assert.AreEqual(ArgumentProcessorResult.Fail, _executor.Execute());
    }

    [TestMethod]
    [DataRow("Install;DumpDirectoryPath=c:\\DumpDirectoryPath", "PROCDUMP_PATH", "c:\\procDump")]
    [DataRow("Install;DumpDirectoryPath=c:\\DumpDirectoryPath", "PATH", "c:\\procDump;")]

    public void AeDebuggerArgumentExecutor_ShouldUseEnvironmentVariables(string command, string environmentVariablesKey, string environmentVariableValue)
    {
        _environmentVariableHelper.Setup(x => x.GetEnvironmentVariable(environmentVariablesKey)).Returns(environmentVariableValue);
        _fileHelper.Setup(x => x.DirectoryExists("c:\\procDump")).Returns(true);
        _fileHelper.Setup(x => x.DirectoryExists("c:\\DumpDirectoryPath")).Returns(true);
        _fileHelper.Setup(x => x.Exists(It.IsAny<string>())).Returns((string fileName) => fileName == "c:\\procDump\\procdump.exe");
        _executor.Initialize(command);
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }

    [TestMethod]
    [DataRow("Install;ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath;DumpDirectoryPath=c:\\DumpDirectoryPath", true)]
    [DataRow("Uninstall;ProcDumpToolDirectoryPath=c:\\ProcDumpToolDirectoryPath;DumpDirectoryPath=c:\\DumpDirectoryPath", false)]
    public void AeDebuggerArgumentExecutor_ProcdumpArgument(string command, bool install)
    {
        _fileHelper.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
        _processHelper.Setup(x => x.LaunchProcess(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            null,
            It.IsAny<Action<object?, string?>>(),
            It.IsAny<Action<object?>>(),
            It.IsAny<Action<object?, string?>>()))
         .Returns((string processPath, string? arguments, string? workingDirectory, IDictionary<string, string?>? envVariables, Action<object?, string?>? errorCallback, Action<object?>? exitCallBack, Action<object?, string?>? outputCallBack) =>
         {
             Assert.IsTrue(install ? arguments == "-ma -i" : arguments == "-u");
             return new object();
         });
        _executor.Initialize(command);
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }
}
