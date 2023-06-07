// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// The datacollection launcher.
/// This works for Desktop local scenarios
/// </summary>
internal class DotnetDataCollectionLauncher : DataCollectionLauncher
{
    private const string DataCollectorProcessName = "datacollector.dll";

    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetDataCollectionLauncher"/> class.
    /// </summary>
    public DotnetDataCollectionLauncher()
        : this(new ProcessHelper(), new FileHelper(), TestSessionMessageLogger.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetDataCollectionLauncher"/> class.
    /// </summary>
    /// <param name="processHelper">
    /// The process helper.
    /// </param>
    /// <param name="fileHelper">
    /// The file Helper.
    /// </param>
    /// <param name="messageLogger">
    /// The message Logger.
    /// </param>
    internal DotnetDataCollectionLauncher(IProcessHelper processHelper, IFileHelper fileHelper, IMessageLogger messageLogger) : base(processHelper, messageLogger)
    {
        _processHelper = processHelper;
        _fileHelper = fileHelper;
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
        TPDebug.Assert(dataCollectorDirectory is not null, "dataCollectorDirectory is null");

        var dataCollectorAssemblyPath = Path.Combine(dataCollectorDirectory, DataCollectorProcessName);
        string dataCollectorFileName = Path.GetFileNameWithoutExtension(dataCollectorAssemblyPath);

        var args = "exec";

        // Probe for runtime config and deps file for the test source
        var runtimeConfigPath = Path.Combine(dataCollectorDirectory, string.Concat(dataCollectorFileName, ".runtimeconfig.json"));

        if (_fileHelper.Exists(runtimeConfigPath))
        {
            var argsToAdd = " --runtimeconfig " + runtimeConfigPath.AddDoubleQuote();
            args += argsToAdd;
            EqtTrace.Verbose("DotnetDataCollectionLauncher: Adding {0} in args", argsToAdd);
        }
        else
        {
            EqtTrace.Verbose("DotnetDataCollectionLauncher: File {0}, does not exist", runtimeConfigPath);
        }

        // Use the deps.json for test source
        var depsFilePath = Path.Combine(dataCollectorDirectory, string.Concat(dataCollectorFileName, ".deps.json"));
        if (_fileHelper.Exists(depsFilePath))
        {
            var argsToAdd = " --depsfile " + depsFilePath.AddDoubleQuote();
            args += argsToAdd;
            EqtTrace.Verbose("DotnetDataCollectionLauncher: Adding {0} in args", argsToAdd);
        }
        else
        {
            EqtTrace.Verbose("DotnetDataCollectionLauncher: File {0}, does not exist", depsFilePath);
        }

        var currentProcessFileName = _processHelper.GetCurrentProcessFileName();
        TPDebug.Assert(currentProcessFileName is not null, "currentProcessFileName is null");
        EqtTrace.Verbose("DotnetDataCollectionLauncher: Full path of dotnet.exe is {0}", currentProcessFileName);

        var cliArgs = string.Join(" ", commandLineArguments);
        var argumentsString = string.Format(CultureInfo.InvariantCulture, "{0} \"{1}\" {2} ", args, dataCollectorAssemblyPath, cliArgs);
        var dataCollectorProcess = _processHelper.LaunchProcess(currentProcessFileName, argumentsString, Directory.GetCurrentDirectory(), environmentVariables, ErrorReceivedCallback, ExitCallBack, null);

        DataCollectorProcessId = _processHelper.GetProcessId(dataCollectorProcess);

        return DataCollectorProcessId;
    }
}
