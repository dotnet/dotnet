// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class DotnetDataCollectionLauncherTests
{
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IMessageLogger> _mockMessageLogger;
    private readonly DotnetDataCollectionLauncher _dataCollectionLauncher;

    public DotnetDataCollectionLauncherTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockProcessHelper.Setup(x => x.GetCurrentProcessFileName()).Returns("dotnet");
        _mockMessageLogger = new Mock<IMessageLogger>();

        _dataCollectionLauncher = new DotnetDataCollectionLauncher(_mockProcessHelper.Object, _mockFileHelper.Object, _mockMessageLogger.Object);
    }

    [TestMethod]
    public void LaunchDataCollectorShouldLaunchDataCollectorProcess()
    {
        List<string> arguments = new();
        _dataCollectionLauncher.LaunchDataCollector(null, arguments);

        _mockProcessHelper.Verify(x => x.LaunchProcess(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string?>>(), It.IsAny<Action<object?, string?>>(), It.IsAny<Action<object?>>(), It.IsAny<Action<object?, string?>>()), Times.Once());
    }

    [TestMethod]
    public void LaunchDataCollectorShouldAppendDoubleQuoteForDataCollectorDllPath()
    {
        var currentWorkingDirectory = Path.GetDirectoryName(typeof(DefaultDataCollectionLauncher).Assembly.GetAssemblyLocation())!;
        var dataCollectorAssemblyPath = Path.Combine(currentWorkingDirectory, "datacollector.dll");

        List<string> arguments = new();
        _dataCollectionLauncher.LaunchDataCollector(null, arguments);

        _mockProcessHelper.Verify(x => x.LaunchProcess(It.IsAny<string>(), string.Format(CultureInfo.InvariantCulture, "{0} \"{1}\" {2} ", "exec", dataCollectorAssemblyPath, string.Join(" ", arguments)), It.IsAny<string>(), It.IsAny<IDictionary<string, string?>>(), It.IsAny<Action<object?, string?>>(), It.IsAny<Action<object?>>(), It.IsAny<Action<object?, string?>>()), Times.Once());
    }

    [TestMethod]
    public void LaunchDataCollectorShouldLaunchDataCollectorProcessWithCurrecntWorkingDirectory()
    {
        List<string> arguments = new();
        _dataCollectionLauncher.LaunchDataCollector(null, arguments);

        string currentWorkingDirectory = Directory.GetCurrentDirectory();

        _mockProcessHelper.Verify(x => x.LaunchProcess(It.IsAny<string>(), It.IsAny<string>(), currentWorkingDirectory, It.IsAny<IDictionary<string, string?>>(), It.IsAny<Action<object?, string?>>(), It.IsAny<Action<object?>>(), It.IsAny<Action<object?, string?>>()), Times.Once());
    }
}
