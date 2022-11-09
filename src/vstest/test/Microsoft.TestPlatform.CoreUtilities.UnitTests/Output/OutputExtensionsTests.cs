// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests.Output;

[TestClass]
public class OutputExtensionsTests
{
    private readonly Mock<IOutput> _mockOutput;
    private ConsoleColor _color;
    private readonly ConsoleColor _previousColor;
    private readonly ConsoleColor _newColor;

    public OutputExtensionsTests()
    {
        // Setting Console.ForegroundColor to newColor which will be used to determine whether
        // test command output is redirecting to file or writing to console.
        // If command output is redirecting to file, then Console.ForegroundColor can't be modified.
        // So that tests which assert Console.ForegroundColor should not run.
        _previousColor = Console.ForegroundColor;
        _newColor = _previousColor == ConsoleColor.Gray
            ? ConsoleColor.Black
            : ConsoleColor.Blue;
        Console.ForegroundColor = _newColor;

        _mockOutput = new Mock<IOutput>();
        _color = Console.ForegroundColor;
        _mockOutput.Setup(o => o.WriteLine(It.IsAny<string>(), It.IsAny<OutputLevel>())).Callback(() => _color = Console.ForegroundColor);
    }

    [TestCleanup]
    public void CleanUp()
    {
        Console.ForegroundColor = _previousColor;
    }

    [TestMethod]
    public void OutputErrorForSimpleMessageShouldOutputTheMessageString()
    {
        _mockOutput.Object.Error(false, "HelloError", null);
        _mockOutput.Verify(o => o.WriteLine("HelloError", OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void OutputErrorForSimpleMessageShouldOutputTheMessageStringWithPrefixIfSet()
    {
        _mockOutput.Object.Error(true, "HelloError", null);
        _mockOutput.Verify(o => o.WriteLine("Error: HelloError", OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void OutputErrorForSimpleMessageShouldSetConsoleColorToRed()
    {
        if (CanNotSetConsoleForegroundColor())
        {
            return;
        }

        _mockOutput.Object.Error(false, "HelloError", null);
        Assert.IsTrue(_color == ConsoleColor.Red, "Console color not set.");
    }

    [TestMethod]
    public void OutputErrorForMessageWithParamsShouldOutputFormattedMessage()
    {
        _mockOutput.Object.Error(false, "HelloError {0} {1}", "Foo", "Bar");
        _mockOutput.Verify(o => o.WriteLine("HelloError Foo Bar", OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void OutputWarningForSimpleMessageShouldOutputTheMessageString()
    {
        _mockOutput.Object.Warning(false, "HelloWarning", null);
        _mockOutput.Verify(o => o.WriteLine("HelloWarning", OutputLevel.Warning), Times.Once());
    }

    [TestMethod]
    public void OutputWarningForSimpleMessageShouldSetConsoleColorToYellow()
    {
        if (CanNotSetConsoleForegroundColor())
        {
            return;
        }

        _mockOutput.Object.Warning(false, "HelloWarning", null);
        Assert.IsTrue(_color == ConsoleColor.Yellow);
    }

    [TestMethod]
    public void OutputWarningForMessageWithParamsShouldOutputFormattedMessage()
    {
        _mockOutput.Object.Warning(false, "HelloWarning {0} {1}", "Foo", "Bar");
        _mockOutput.Verify(o => o.WriteLine("HelloWarning Foo Bar", OutputLevel.Warning), Times.Once());
    }

    [TestMethod]
    public void OutputInformationForSimpleMessageShouldOutputTheMessageString()
    {
        _mockOutput.Object.Information(false, ConsoleColor.Green, "HelloInformation", null);
        _mockOutput.Verify(o => o.WriteLine("HelloInformation", OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void OutputInformationForSimpleMessageShouldSetConsoleColorToGivenColor()
    {
        if (CanNotSetConsoleForegroundColor())
        {
            return;
        }

        _mockOutput.Object.Information(false, ConsoleColor.Green, "HelloInformation", null);
        Assert.IsTrue(_color == ConsoleColor.Green);
    }

    [TestMethod]
    public void OutputInformationForMessageWithParamsShouldOutputFormattedMessage()
    {
        _mockOutput.Object.Information(false, "HelloInformation {0} {1}", "Foo", "Bar");
        _mockOutput.Verify(o => o.WriteLine("HelloInformation Foo Bar", OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void OutputInformationShouldNotChangeConsoleOutputColor()
    {
        if (CanNotSetConsoleForegroundColor())
        {
            return;
        }

        ConsoleColor color1 = Console.ForegroundColor, color2 = Console.ForegroundColor == ConsoleColor.Red ? ConsoleColor.Black : ConsoleColor.Red;
        _mockOutput.Setup(o => o.WriteLine(It.IsAny<string>(), It.IsAny<OutputLevel>())).Callback(() => color2 = Console.ForegroundColor);

        _mockOutput.Object.Information(false, "HelloInformation {0} {1}", "Foo", "Bar");
        _mockOutput.Verify(o => o.WriteLine("HelloInformation Foo Bar", OutputLevel.Information), Times.Once());
        Assert.IsTrue(color1 == color2);
    }

    private bool CanNotSetConsoleForegroundColor()
    {
        if (Console.ForegroundColor != _newColor)
        {
            Assert.Inconclusive("Can't set Console foreground color. Might be because process output redirect to file.");
            return true;
        }
        return false;
    }
}
