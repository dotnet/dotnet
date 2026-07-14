// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests;

[TestClass]
public class RunSettingsManagerTests
{
    [TestMethod]
    public void ActiveRunSettingsShouldBeNonNullByDefault()
    {
        var instance = new RunSettingsManager();

        Assert.IsNotNull(instance.ActiveRunSettings);
    }

    [TestMethod]
    public void SetActiveRunSettingsShouldThrowIfRunSettingsPassedIsNull()
    {
        var instance = new RunSettingsManager();

        Assert.ThrowsExactly<ArgumentNullException>(() => instance.SetActiveRunSettings(null!));
    }

    [TestMethod]
    public void SetActiveRunSettingsShouldSetTheActiveRunSettingsProperty()
    {
        var instance = new RunSettingsManager();

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml("<RunSettings></RunSettings>");

        instance.SetActiveRunSettings(runSettings);

        Assert.AreEqual(runSettings, instance.ActiveRunSettings);
    }
}
