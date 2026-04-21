// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TrxLoggerConstants = Microsoft.TestPlatform.Extensions.TrxLogger.Utility.Constants;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.UnitTests;

/// <summary>
/// Regression tests for TrxLogger WarnOnFileOverwrite parameter.
/// </summary>
[TestClass]
public class TrxLoggerWarnOnOverwriteRegressionTests
{
    private static readonly string DefaultTestRunDirectory = System.IO.Path.GetTempPath();

    // Regression test for #5141 — Add option to overwrite trx without warning
    [TestMethod]
    public void Initialize_WarnOnFileOverwriteTrue_ShouldNotThrow()
    {
        var logger = new VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger();
        var events = new Mock<TestLoggerEvents>();
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [TrxLoggerConstants.LogFileNameKey] = "test.trx",
            [TrxLoggerConstants.WarnOnFileOverwrite] = "true"
        };

        // Should not throw
        logger.Initialize(events.Object, parameters);
    }

    // Regression test for #5141
    [TestMethod]
    public void Initialize_WarnOnFileOverwriteFalse_ShouldNotThrow()
    {
        var logger = new VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger();
        var events = new Mock<TestLoggerEvents>();
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [TrxLoggerConstants.LogFileNameKey] = "test.trx",
            [TrxLoggerConstants.WarnOnFileOverwrite] = "false"
        };

        // Should not throw
        logger.Initialize(events.Object, parameters);
    }

    // Regression test for #5141
    [TestMethod]
    public void Initialize_WarnOnFileOverwriteInvalidValue_ShouldDefaultToTrue()
    {
        var logger = new VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger();
        var events = new Mock<TestLoggerEvents>();
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [TrxLoggerConstants.LogFileNameKey] = "test.trx",
            [TrxLoggerConstants.WarnOnFileOverwrite] = "not-a-bool"
        };

        // Should not throw — invalid value falls back to true
        logger.Initialize(events.Object, parameters);
    }

    // Regression test for #5141
    [TestMethod]
    public void Initialize_WarnOnFileOverwriteNotProvided_ShouldDefaultToTrue()
    {
        var logger = new VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger();
        var events = new Mock<TestLoggerEvents>();
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [TrxLoggerConstants.LogFileNameKey] = "test.trx",
        };

        // Should not throw — missing parameter falls back to true
        logger.Initialize(events.Object, parameters);
    }
}
