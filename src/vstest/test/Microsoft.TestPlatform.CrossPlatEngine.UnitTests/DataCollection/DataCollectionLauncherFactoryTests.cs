// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class DataCollectionLauncherFactoryTests
{
    private readonly Mock<IProcessHelper> _mockProcessHelper;

    private readonly string _dummyRunSettings =
        "<RunSettings><DataCollectionRunSettings><DataCollectors><DataCollector friendlyName=\"dummy\"></DataCollector></DataCollectors></DataCollectionRunSettings></RunSettings>";

    public DataCollectionLauncherFactoryTests()
    {
        _mockProcessHelper = new Mock<IProcessHelper>();
    }

    [TestMethod]
    public void GetDataCollectorLauncherShouldReturnDefaultDataCollectionLauncherWithFullClrRunner()
    {
        _mockProcessHelper.Setup(x => x.GetCurrentProcessFileName()).Returns("vstest.console.exe");
        var dataCollectorLauncher = DataCollectionLauncherFactory.GetDataCollectorLauncher(_mockProcessHelper.Object, _dummyRunSettings);
        Assert.IsInstanceOfType(dataCollectorLauncher, typeof(DefaultDataCollectionLauncher));
    }

    [TestMethod]
    public void GetDataCollectorLauncherShouldReturnDotnetDataCollectionLauncherWithDotnetCore()
    {
        _mockProcessHelper.Setup(x => x.GetCurrentProcessFileName()).Returns("dotnet");
        var dataCollectorLauncher = DataCollectionLauncherFactory.GetDataCollectorLauncher(_mockProcessHelper.Object, _dummyRunSettings);
        Assert.IsInstanceOfType(dataCollectorLauncher, typeof(DotnetDataCollectionLauncher));
    }

    [TestMethod]
    public void GetDataCollectorLauncherShouldBeInsensitiveToCaseOfCurrentProcess()
    {
        _mockProcessHelper.Setup(x => x.GetCurrentProcessFileName()).Returns("DOTNET");
        var dataCollectorLauncher = DataCollectionLauncherFactory.GetDataCollectorLauncher(_mockProcessHelper.Object, _dummyRunSettings);
        Assert.IsInstanceOfType(dataCollectorLauncher, typeof(DotnetDataCollectionLauncher));
    }
}
