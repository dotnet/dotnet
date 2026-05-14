// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// Class which defines additional specifiable parameters for vstest.console.exe
/// </summary>
public class ConsoleParameters
{
    internal static readonly ConsoleParameters Default = new();

    private readonly IFileHelper _fileHelper;

    private string? _logFilePath;

    /// <summary>
    /// Create instance of <see cref="ConsoleParameters"/>
    /// </summary>
    public ConsoleParameters() : this(new FileHelper())
    { }

    /// <summary>
    /// Create instance of <see cref="ConsoleParameters"/>
    /// </summary>
    /// <param name="fileHelper"> Object of type <see cref="IFileHelper"/></param>
    public ConsoleParameters(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    /// <summary>
    /// Environment variables to be set for the process. This will merge the specified entries to the environment variables
    /// inherited from the current process. If you wish to provide a full set of environment variables yourself set <see cref="InheritEnvironmentVariables"/> to false.
    /// </summary>
    public Dictionary<string, string?> EnvironmentVariables { get; set; } = new();

    /// <summary>
    /// When set to true (default), all environment variables are inherited from the current process and the entries provided in <see cref="EnvironmentVariables"/> are merged with that set.
    /// When set to false, only the values you provide in <see cref="EnvironmentVariables"/> are used. Giving you full control of the environment vstest.console is started with.
    /// This is only rarely useful and can lead to vstest.console not being able to start at all.
    /// You most likely want to use <see cref="System.Environment.GetEnvironmentVariables(System.EnvironmentVariableTarget)"/> and combine
    /// <see cref="System.EnvironmentVariableTarget.Machine"/> and <see cref="System.EnvironmentVariableTarget.User"/> responses.
    /// </summary>
    public bool InheritEnvironmentVariables { get; set; } = true;

    /// <summary>
    /// Trace level for logs.
    /// </summary>
    public TraceLevel TraceLevel { get; set; } = TraceLevel.Verbose;

    /// <summary>
    /// Full path for the log file
    /// </summary>
    public string? LogFilePath
    {
        get
        {
            return _logFilePath;
        }

        set
        {
            ValidateArg.NotNullOrEmpty(value!, "LogFilePath");
            var directoryPath = Path.GetDirectoryName(value);
            if (!directoryPath.IsNullOrEmpty() && !_fileHelper.DirectoryExists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Ensure path is double quoted. if path has white space then it can create problem.
            _logFilePath = value!.AddDoubleQuote();
        }
    }

    /// <summary>
    /// Port Number for communication
    /// vstest.console will need this port number to communicate with this component - translation layer
    /// Currently Internal as we are not intentionally exposing this to consumers of translation layer
    /// </summary>
    internal int PortNumber { get; set; }

    /// <summary>
    /// Parent Process ID of the process whose lifetime should dictate the life time of vstest.console.exe
    /// vstest.console will need this process ID to know when the process exits.
    /// If parent process dies/crashes without invoking EndSession, vstest.console should exit immediately
    /// Currently Internal as we are not intentionally exposing this to consumers of translation layer
    /// </summary>
    internal int ParentProcessId { get; set; }
}
