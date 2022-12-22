// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Internal;

[TestClass]
public class ProgressIndicatorTests
{
    private readonly ProgressIndicator _indicator;
    private readonly Mock<IOutput> _consoleOutput;
    private readonly Mock<IConsoleHelper> _consoleHelper;

    public ProgressIndicatorTests()
    {
        _consoleOutput = new Mock<IOutput>();
        _consoleHelper = new Mock<IConsoleHelper>();
        _consoleHelper.Setup(c => c.WindowWidth).Returns(100);
        _consoleHelper.Setup(c => c.CursorTop).Returns(20);
        _indicator = new ProgressIndicator(_consoleOutput.Object, _consoleHelper.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _indicator.Stop();
    }

    [TestMethod]
    public void StartShouldStartPrintingProgressMessage()
    {
        _indicator.Start();
        _consoleOutput.Verify(m => m.Write("Test run in progress.", OutputLevel.Information), Times.Once);
        Assert.IsTrue(_indicator.IsRunning);
    }

    [TestMethod]
    public void StartShouldShowProgressMessage()
    {
        _indicator.Start();

        _consoleHelper.Setup(c => c.CursorLeft).Returns(30);
        System.Threading.Thread.Sleep(1500);

        Assert.IsTrue(_indicator.IsRunning);
        _consoleOutput.Verify(m => m.Write("Test run in progress.", OutputLevel.Information), Times.Once);
        _consoleOutput.Verify(m => m.Write(".", OutputLevel.Information), Times.Once);
    }

    [TestMethod]
    public void PauseShouldClearTheStdOutMessage()
    {
        _indicator.Start();
        _indicator.Pause();

        Assert.IsFalse(_indicator.IsRunning);
        string clearMessage = new(' ', _consoleHelper.Object.WindowWidth);
        _consoleOutput.Verify(m => m.Write("Test run in progress.", OutputLevel.Information), Times.Once);
        _consoleOutput.Verify(m => m.Write(clearMessage, OutputLevel.Information), Times.Once);

        _consoleHelper.Verify(ch => ch.SetCursorPosition(0, 20), Times.Exactly(2));
    }

    [TestMethod]
    public void PauseStartAndStopShouldClearPrintProgressAndThenClearTheStdOutMessage()
    {
        _indicator.Start();
        _indicator.Pause();
        _indicator.Start();
        _indicator.Stop();

        Assert.IsFalse(_indicator.IsRunning);
        string clearMessage = new(' ', _consoleHelper.Object.WindowWidth);
        _consoleOutput.Verify(m => m.Write("Test run in progress.", OutputLevel.Information), Times.Exactly(2));
        _consoleOutput.Verify(m => m.Write(clearMessage, OutputLevel.Information), Times.Exactly(2));
        _consoleHelper.Verify(ch => ch.SetCursorPosition(0, 20), Times.Exactly(4));
    }

    [TestMethod]
    public void StopShouldClearTheStdOutMessage()
    {
        _indicator.Start();
        _indicator.Stop();

        Assert.IsFalse(_indicator.IsRunning);
        string clearMessage = new(' ', _consoleHelper.Object.WindowWidth);
        _consoleOutput.Verify(m => m.Write("Test run in progress.", OutputLevel.Information), Times.Once);
        _consoleOutput.Verify(m => m.Write(clearMessage, OutputLevel.Information), Times.Once);
        _consoleHelper.Verify(ch => ch.SetCursorPosition(0, 20), Times.Exactly(2));
    }
}
