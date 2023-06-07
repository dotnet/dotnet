// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// The datacollection launcher.
/// This works for Desktop local scenarios
/// </summary>
internal class DefaultDataCollectionLauncher : DataCollectionLauncher
{
    private const string DataCollectorProcessName = "datacollector.exe";
    private const string DataCollectorProcessNameArm64 = "datacollector.arm64.exe";

    /// <summary>
    /// The constructor.
    /// </summary>
    public DefaultDataCollectionLauncher()
        : this(new ProcessHelper(), TestSessionMessageLogger.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultDataCollectionLauncher"/> class.
    /// </summary>
    /// <param name="processHelper">
    /// The process helper.
    /// </param>
    /// <param name="messageLogger">
    /// The message Logger.
    /// </param>
    internal DefaultDataCollectionLauncher(IProcessHelper processHelper, IMessageLogger messageLogger) : base(processHelper, messageLogger)
    {
        _processHelper = processHelper;
        DataCollectorProcessId = -1;
    }

    /// <summary>
    /// Launches the test host for discovery/execution.
    /// </summary>
    /// <param name="environmentVariables">Environment variables for the process.</param>
    /// <param name="commandLineArguments">The command line arguments to pass to the process.</param>
    /// <returns>ProcessId of launched Process. 0 means not launched.</returns>
    public override int LaunchDataCollector(IDictionary<string, string?>? environmentVariables, IList<string> commandLineArguments)
    {
        var dataCollectorDirectory = Path.GetDirectoryName(typeof(DefaultDataCollectionLauncher).Assembly.GetAssemblyLocation());
        TPDebug.Assert(dataCollectorDirectory is not null, "dataCollectorDirectory is not null");

        var currentProcessPath = _processHelper.GetCurrentProcessFileName();
        TPDebug.Assert(currentProcessPath is not null, "currentProcessPath is not null");

        // If current process is dotnet/dotnet.exe and you are here, datacollector.exe/datacollector.arm64.exe is present in TestHost folder.
        string dataCollectorProcessName = _processHelper.GetCurrentProcessArchitecture() == PlatformArchitecture.ARM64
            ? DataCollectorProcessNameArm64
            : DataCollectorProcessName;
        string dataCollectorProcessPath = currentProcessPath.EndsWith("dotnet", StringComparison.OrdinalIgnoreCase)
                                          || currentProcessPath.EndsWith("dotnet.exe", StringComparison.OrdinalIgnoreCase)
            ? Path.Combine(dataCollectorDirectory, "TestHostNetFramework", dataCollectorProcessName)
            : Path.Combine(dataCollectorDirectory, dataCollectorProcessName);

        var argumentsString = string.Join(" ", commandLineArguments);
        var dataCollectorProcess = _processHelper.LaunchProcess(dataCollectorProcessPath, argumentsString, Directory.GetCurrentDirectory(), environmentVariables, ErrorReceivedCallback, ExitCallBack, null);
        DataCollectorProcessId = _processHelper.GetProcessId(dataCollectorProcess);
        return DataCollectorProcessId;
    }
}
