// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TrxLoggerConstants = Microsoft.TestPlatform.Extensions.TrxLogger.Utility.Constants;
using TrxLoggerObjectModel = Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

using DefaultLoggerParameterNames = Microsoft.VisualStudio.TestPlatform.ObjectModel.DefaultLoggerParameterNames;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.UnitTests;

/// <summary>
/// Regression tests for:
/// - GH-2319: Setting ErrorStackTrace before ErrorMessage must not crash.
/// - GH-5132: WarnOnFileOverwrite=false must suppress overwrite warning.
/// - GH-4243: TestOutcome.Error must have value 0, must serialize as "Error" (not "Min").
/// </summary>
[TestClass]
public class RegressionBugFixTests
{
    private static readonly string DefaultTestRunDirectory = Path.GetTempPath();

    #region GH-2319: ErrorStackTrace without ErrorMessage doesn't crash

    [TestMethod]
    public void ErrorStackTrace_SetBeforeErrorMessage_MustNotThrow()
    {
        // GH-2319: Previously, setting ErrorStackTrace first would crash with Debug.Assert
        // because _errorInfo was null. The fix uses ??= to lazily initialize.
        var testResult = CreateTestResult();

        // Act: set ErrorStackTrace FIRST, before any ErrorMessage
        testResult.ErrorStackTrace = "at SomeTest.Method() in file.cs:line 42";

        // Assert: should not throw and ErrorMessage should return empty string (not null or crash)
        Assert.AreEqual("at SomeTest.Method() in file.cs:line 42", testResult.ErrorStackTrace);
        Assert.AreEqual(string.Empty, testResult.ErrorMessage,
            "GH-2319: ErrorMessage must return empty string when only ErrorStackTrace is set.");
    }

    [TestMethod]
    public void ErrorMessage_ThenErrorStackTrace_BothAccessible()
    {
        // Setting ErrorMessage first, then ErrorStackTrace: both must be readable.
        var testResult = CreateTestResult();

        testResult.ErrorMessage = "Assert.Fail hit";
        testResult.ErrorStackTrace = "at MyTest.Run()";

        Assert.AreEqual("Assert.Fail hit", testResult.ErrorMessage);
        Assert.AreEqual("at MyTest.Run()", testResult.ErrorStackTrace);
    }

    [TestMethod]
    public void ErrorMessage_NeverSet_ReturnsEmptyString()
    {
        // Before the fix, accessing getters on a fresh TestResult without _errorInfo
        // would behave unpredictably. The fix returns string.Empty via null-coalescing.
        var testResult = CreateTestResult();

        Assert.AreEqual(string.Empty, testResult.ErrorMessage);
        Assert.AreEqual(string.Empty, testResult.ErrorStackTrace);
    }

    #endregion

    #region GH-5132: WarnOnFileOverwrite parameter

    [TestMethod]
    public void Initialize_WarnOnFileOverwriteFalse_FieldMustBeFalse()
    {
        // GH-5132: When WarnOnFileOverwrite=false, the _warnOnFileOverwrite field must be false.
        // If the fix were reverted (field removed), this would fail.
        var logger = new TestableTrxLogger();
        var events = new Mock<TestLoggerEvents>();
        var parameters = CreateDefaultParameters();
        parameters[TrxLoggerConstants.WarnOnFileOverwrite] = "false";

        logger.Initialize(events.Object, parameters);

        var warnField = typeof(VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger)
            .GetField("_warnOnFileOverwrite", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(warnField, "_warnOnFileOverwrite field must exist.");
        Assert.IsFalse((bool)warnField.GetValue(logger)!,
            "GH-5132: _warnOnFileOverwrite must be false when parameter is 'false'.");
    }

    [TestMethod]
    public void Initialize_WarnOnFileOverwriteNotSet_DefaultsToTrue()
    {
        // GH-5132: When WarnOnFileOverwrite parameter is not provided, default must be true
        // (preserving existing behavior for users who did not opt out).
        var logger = new TestableTrxLogger();
        var events = new Mock<TestLoggerEvents>();
        var parameters = CreateDefaultParameters();

        logger.Initialize(events.Object, parameters);

        var warnField = typeof(VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger)
            .GetField("_warnOnFileOverwrite", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(warnField, "_warnOnFileOverwrite field must exist.");
        Assert.IsTrue((bool)warnField.GetValue(logger)!,
            "GH-5132: _warnOnFileOverwrite must default to true when parameter is not provided.");
    }

    #endregion

    #region GH-4243: TestOutcome has no Min/Max aliases

    [TestMethod]
    public void TestOutcome_Error_MustHaveIntValue0()
    {
        // GH-4243: Error must be the first enum member (value 0).
        // Previously Min=Error alias made Error.ToString() return "Min".
        // Box through object to avoid compile-time constant folding (MSTEST0032).
        object errorEnum = TrxLoggerObjectModel.TestOutcome.Error;
        Assert.AreEqual(0, (int)(TrxLoggerObjectModel.TestOutcome)errorEnum,
            "GH-4243: TestOutcome.Error must have integer value 0.");
    }

    [TestMethod]
    public void TestOutcome_Error_MustSerializeAsError_NotMin()
    {
        // GH-4243: The fix removed Min=Error alias. Error.ToString() must return "Error".
        // If the fix were reverted and Min=Error added back, ToString() would return "Min".
        Assert.AreEqual("Error", TrxLoggerObjectModel.TestOutcome.Error.ToString(),
            "GH-4243: TestOutcome.Error.ToString() must be 'Error', not 'Min'.");
    }

    [TestMethod]
    public void TestOutcome_NoMinOrMaxMember()
    {
        // GH-4243: The enum must NOT have a member named "Min" or "Max".
        // Verify via reflection.
        var names = Enum.GetNames(typeof(TrxLoggerObjectModel.TestOutcome));
        CollectionAssert.DoesNotContain(names, "Min",
            "GH-4243: TestOutcome enum must not have a 'Min' member.");
        CollectionAssert.DoesNotContain(names, "Max",
            "GH-4243: TestOutcome enum must not have a 'Max' member.");
    }

    #endregion

    #region Helpers

    private static TrxLoggerObjectModel.TestResult CreateTestResult()
    {
        return new TrxLoggerObjectModel.TestResult(
            runId: Guid.NewGuid(),
            testId: Guid.NewGuid(),
            executionId: Guid.NewGuid(),
            parentExecutionId: Guid.Empty,
            resultName: "TestResult1",
            computerName: Environment.MachineName,
            outcome: TrxLoggerObjectModel.TestOutcome.Failed,
            testType: new TrxLoggerObjectModel.TestType(Guid.NewGuid()),
            testCategoryId: TrxLoggerObjectModel.TestListCategoryId.Uncategorized,
            trxFileHelper: new TrxFileHelper());
    }

    private static Dictionary<string, string?> CreateDefaultParameters()
    {
        return new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [TrxLoggerConstants.LogFileNameKey] = "test.trx"
        };
    }

    #endregion
}
