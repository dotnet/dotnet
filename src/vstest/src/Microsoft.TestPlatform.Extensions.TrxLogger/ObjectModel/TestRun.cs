// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Class having information about a test run.
/// </summary>
internal sealed class TestRun
{
    // These fields will be valid when the test run summary is loaded from a results file.
    // The summary fields need to be first in the class so they get serialized first. When we
    // read the summary we don't want to parse the XML tags for other fields because they can
    // be quite large.
    //
    // When reading the results file, the summary is considered complete when all summary fields
    // are non-null. Any new summary fields that are initialized in the constructor should be
    // placed before the last non-initialized field.
    //
    // The summary parsing code is in XmlTestReader.ReadTestRunSummary.
    [StoreXmlSimpleField("@id")]
    private Guid _id;

    [StoreXmlSimpleField("@name")]
    private string _name;

    [StoreXmlSimpleField("@runUser", "")]
    private readonly string _runUser;

    private TestRunConfiguration? _runConfig;

    [StoreXmlSimpleField("Times/@creation")]
    private readonly DateTime _created;

    [StoreXmlSimpleField("Times/@queuing")]
    private readonly DateTime _queued;

    [StoreXmlSimpleField("Times/@start")]
    private DateTime _started;

    [StoreXmlSimpleField("Times/@finish")]
    private DateTime _finished;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRun"/> class.
    /// </summary>
    /// <param name="runId">
    /// The run id.
    /// </param>
    internal TestRun(Guid runId)
    {
        _id = Guid.NewGuid();
        _name = string.Format(CultureInfo.CurrentCulture, TrxLoggerResources.Common_TestRunName, Environment.GetEnvironmentVariable("UserName"), Environment.MachineName, FormatDateTimeForRunName(DateTime.Now));

        // Fix for issue (https://github.com/Microsoft/vstest/issues/213). Since there is no way to find current user in linux machine.
        // We are catching PlatformNotSupportedException for non windows machine.
        try
        {
            _runUser = WindowsIdentity.GetCurrent().Name;
        }
        catch (PlatformNotSupportedException)
        {
            _runUser = string.Empty;
        }
        _created = DateTime.UtcNow;
        _queued = DateTime.UtcNow;
        _started = DateTime.UtcNow;
        _finished = DateTime.UtcNow;

        EqtAssert.IsTrue(!Guid.Empty.Equals(runId), "Can't use Guid.Empty for run ID.");
        _id = runId;
    }

    /// <summary>
    /// Gets or sets the run configuration.
    /// </summary>
    internal TestRunConfiguration? RunConfiguration
    {
        get
        {
            return _runConfig;
        }

        set
        {
            EqtAssert.ParameterNotNull(value, "RunConfiguration");
            _runConfig = value;
        }
    }

    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    internal DateTime Started
    {
        get
        {
            return _started;
        }

        set
        {
            _started = value;
        }
    }

    /// <summary>
    /// Gets or sets the finished time of Test run.
    /// </summary>
    internal DateTime Finished
    {
        get { return _finished; }
        set { _finished = value; }
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    internal string Name
    {
        get
        {
            return _name;
        }

        set
        {
            EqtAssert.StringNotNullOrEmpty(value, "Name");
            _name = value;
        }
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    internal Guid Id
    {
        get { return _id; }
    }

    /// <summary>
    /// WARNING: do not use from inside Test Adapters, use from only on HA by UI etc.
    /// Returns directory on HA for dependent files for TestResult. XmlPersistence method for UI.
    /// Throws on error (e.g. if deployment directory was not set for test run).
    /// </summary>
    /// <param name="result">
    /// Test Result to get dependent files directory for.
    /// </param>
    /// <returns>
    /// Result directory.
    /// </returns>
    internal string GetResultFilesDirectory(TestResult result)
    {
        EqtAssert.ParameterNotNull(result, nameof(result));
        return Path.Combine(GetResultsDirectory(), result.RelativeTestResultsDirectory);
    }

    /// <summary>
    /// Gets the results directory, which is the run deployment In directory
    /// </summary>
    /// <returns>The results directory</returns>
    /// <remarks>This method is called by public properties/methods, so it needs to throw on error</remarks>
    internal string GetResultsDirectory()
    {
        if (RunConfiguration == null)
        {
            Debug.Fail("'RunConfiguration' is null");
            throw new Exception(TrxLoggerResources.Common_MissingRunConfigInRun);
        }

        if (string.IsNullOrEmpty(RunConfiguration.RunDeploymentRootDirectory))
        {
            Debug.Fail("'RunConfiguration.RunDeploymentRootDirectory' is null or empty");
            throw new Exception(TrxLoggerResources.Common_MissingRunDeploymentRootInRunConfig);
        }

        return RunConfiguration.RunDeploymentInDirectory;
    }

    private static string FormatDateTimeForRunName(DateTime timeStamp)
    {
        // We use custom format string to make sure that runs are sorted in the same way on all intl machines.
        // This is both for directory names and for Data Warehouse.
        return timeStamp.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
    }
}
