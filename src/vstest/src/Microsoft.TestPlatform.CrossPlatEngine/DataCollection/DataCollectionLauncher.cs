// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// Abstract DataCollection Launcher provides functionality to handle process launch and exit events.
/// </summary>
internal abstract class DataCollectionLauncher : IDataCollectionLauncher
{
    protected IProcessHelper _processHelper;

    protected IMessageLogger _messageLogger;

    protected StringBuilder _processStdError;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionLauncher"/> class.
    /// </summary>
    /// <param name="processHelper">
    /// The process helper.
    /// </param>
    /// <param name="messageLogger">
    /// The message logger.
    /// </param>
    public DataCollectionLauncher(IProcessHelper processHelper, IMessageLogger messageLogger)
    {
        _processHelper = processHelper;
        _messageLogger = messageLogger;
        _processStdError = new StringBuilder(0, CoreUtilities.Constants.StandardErrorMaxLength);
    }

    /// <inheritdoc />
    public int DataCollectorProcessId { get; protected set; }

    /// <summary>
    /// Gets callback on process exit
    /// </summary>
    protected Action<object?> ExitCallBack => process =>
    {
        var processStdErrorStr = _processStdError.ToString();

        _processHelper.TryGetExitCode(process, out int exitCode);

        if (exitCode != 0)
        {
            EqtTrace.Error("DataCollectionLauncher.ExitCallBack: Data collector exited with exitcode:{0} error: '{1}'", exitCode, processStdErrorStr);

            if (!StringUtils.IsNullOrWhiteSpace(processStdErrorStr))
            {
                _messageLogger.SendMessage(TestMessageLevel.Error, processStdErrorStr);
            }
        }
        else
        {
            EqtTrace.Info("DataCollectionLauncher.ExitCallBack: Data collector exited with exitcode: 0 error: '{0}'", processStdErrorStr);
        }
    };

    /// <summary>
    /// Gets callback to read from process error stream
    /// </summary>
    protected Action<object?, string?> ErrorReceivedCallback => (process, data) =>
    {
        // Log all standard error message because on too much data we ignore starting part.
        // This is helpful in abnormal failure of datacollector.
        EqtTrace.Warning("DataCollectionLauncher.ErrorReceivedCallback datacollector standard error line: {0}", data);

        _processStdError.AppendSafeWithNewLine(data);
    };

    /// <summary>
    /// The launch data collector.
    /// </summary>
    /// <param name="environmentVariables">
    /// The environment variables.
    /// </param>
    /// <param name="commandLineArguments">
    /// The command line arguments.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public abstract int LaunchDataCollector(
        IDictionary<string, string?>? environmentVariables,
        IList<string> commandLineArguments);
}
