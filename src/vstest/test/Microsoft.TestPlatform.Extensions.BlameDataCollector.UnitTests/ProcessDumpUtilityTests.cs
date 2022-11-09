// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector.UnitTests;

/// <summary>
/// The blame collector tests.
/// </summary>
[TestClass]
public class ProcessDumpUtilityTests
{
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IHangDumperFactory> _mockHangDumperFactory;
    private readonly Mock<ICrashDumperFactory> _mockCrashDumperFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessDumpUtilityTests"/> class.
    /// </summary>
    public ProcessDumpUtilityTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockHangDumperFactory = new Mock<IHangDumperFactory>();
        _mockCrashDumperFactory = new Mock<ICrashDumperFactory>();
    }

    /// <summary>
    /// GetDumpFile will return empty list of strings if no dump files found
    /// </summary>
    [TestMethod]
    public void GetDumpFileWillThrowExceptionIfNoDumpfile()
    {
        var process = "process";
        var processId = 12345;
        var testResultsDirectory = "D:\\TestResults";

        _mockFileHelper.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns(System.Array.Empty<string>());
        _mockProcessHelper.Setup(x => x.GetProcessName(processId))
            .Returns(process);

        _mockHangDumperFactory.Setup(x => x.Create(It.IsAny<string>()))
            .Returns(new Mock<IHangDumper>().Object);

        _mockCrashDumperFactory.Setup(x => x.Create(It.IsAny<string>()))
            .Returns(new Mock<ICrashDumper>().Object);

        var processDumpUtility = new ProcessDumpUtility(
            _mockProcessHelper.Object,
            _mockFileHelper.Object,
            _mockHangDumperFactory.Object,
            _mockCrashDumperFactory.Object);

        processDumpUtility.StartTriggerBasedProcessDump(processId, testResultsDirectory, false, ".NETCoreApp,Version=v5.0", false, _ => { });

        var ex = Assert.ThrowsException<FileNotFoundException>(() => processDumpUtility.GetDumpFiles(true, false));
        Assert.AreEqual(ex.Message, Resources.Resources.DumpFileNotGeneratedErrorMessage);
    }
}
