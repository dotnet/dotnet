// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;

[TestClass]
public class MainTests
{
    [TestMethod]
    public void RunWhenCliUiLanguageIsSetChangesCultureAndFlowsOverride()
    {
        // Arrange
        var culture = new CultureInfo("fr-fr");
        var envVarMock = new Mock<IEnvironmentVariableHelper>();
        envVarMock.Setup(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE")).Returns(culture.Name);

        bool threadCultureWasSet = false;

        // Act - We have an exception because we are not passing the right args but that's ok for our test
        TestPlatform.CommandLine.Program.Run(null, new(envVarMock.Object, lang => threadCultureWasSet = lang.Equals(culture)));

        // Assert
        Assert.IsTrue(threadCultureWasSet, "DefaultThreadCurrentUICulture was not set");
        envVarMock.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Exactly(2));
        envVarMock.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("VSLANG", culture.LCID.ToString(CultureInfo.InvariantCulture)), Times.Once);
        envVarMock.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("PreferredUILang", culture.Name), Times.Once);
    }

    [TestMethod]
    public void RunWhenVsLangIsSetChangesCultureAndFlowsOverride()
    {
        // Arrange
        var culture = new CultureInfo("fr-fr");
        var envVarMock = new Mock<IEnvironmentVariableHelper>();
        envVarMock.Setup(x => x.GetEnvironmentVariable("VSLANG")).Returns(culture.LCID.ToString(CultureInfo.InvariantCulture));

        bool threadCultureWasSet = false;

        // Act - We have an exception because we are not passing the right args but that's ok for our test
        TestPlatform.CommandLine.Program.Run(null, new(envVarMock.Object, lang => threadCultureWasSet = lang.Equals(culture)));

        // Assert
        Assert.IsTrue(threadCultureWasSet, "DefaultThreadCurrentUICulture was not set");
        envVarMock.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Exactly(2));
        envVarMock.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Exactly(2));
        envVarMock.Verify(x => x.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", culture.Name), Times.Once);
        envVarMock.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("PreferredUILang", culture.Name), Times.Once);
    }

    [TestMethod]
    public void RunWhenNoCultureEnvVarSetDoesNotChangeCultureNorFlowsOverride()
    {
        // Arrange
        var envVarMock = new Mock<IEnvironmentVariableHelper>();
        envVarMock.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>())).Returns(default(string));

        bool threadCultureWasSet = false;

        // Act - We have an exception because we are not passing the right args but that's ok for our test
        TestPlatform.CommandLine.Program.Run(null, new(envVarMock.Object, lang => threadCultureWasSet = true));

        // Assert
        Assert.IsFalse(threadCultureWasSet, "DefaultThreadCurrentUICulture was set");
        envVarMock.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("VSLANG", It.IsAny<string>()), Times.Never);
        envVarMock.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", It.IsAny<string>()), Times.Never);
        envVarMock.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Never);
        envVarMock.Verify(x => x.SetEnvironmentVariable("PreferredUILang", It.IsAny<string>()), Times.Never);
    }
}
