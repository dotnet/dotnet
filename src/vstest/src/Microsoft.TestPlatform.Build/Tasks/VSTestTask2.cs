// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.TestPlatform.Build.Tasks;

public class VSTestTask2 : ToolTask, ITestTask
{
    [Required]
    public ITaskItem? TestFileFullPath { get; set; }
    public string? VSTestSetting { get; set; }
    public ITaskItem[]? VSTestTestAdapterPath { get; set; }
    public string? VSTestFramework { get; set; }
    public string? VSTestPlatform { get; set; }
    public string? VSTestTestCaseFilter { get; set; }
    public string[]? VSTestLogger { get; set; }
    public bool VSTestListTests { get; set; }
    public string? VSTestDiag { get; set; }
    public string[]? VSTestCLIRunSettings { get; set; }
    [Required]
    public ITaskItem? VSTestConsolePath { get; set; }
    public ITaskItem? VSTestResultsDirectory { get; set; }
    public string? VSTestVerbosity { get; set; }
    public string[]? VSTestCollect { get; set; }
    public bool VSTestBlame { get; set; }
    public bool VSTestBlameCrash { get; set; }
    public string? VSTestBlameCrashDumpType { get; set; }
    public bool VSTestBlameCrashCollectAlways { get; set; }
    public bool VSTestBlameHang { get; set; }
    public string? VSTestBlameHangDumpType { get; set; }
    public string? VSTestBlameHangTimeout { get; set; }
    public ITaskItem? VSTestTraceDataCollectorDirectoryPath { get; set; }
    public bool VSTestNoLogo { get; set; }
    public string? VSTestArtifactsProcessingMode { get; set; }
    public string? VSTestSessionCorrelationId { get; set; }


    private readonly string _errorSplitter = "||||";
    private readonly string[] _errorSplitterArray = new[] { "||||" };

    private readonly string _fullErrorSplitter = "~~~~";
    private readonly string[] _fullErrorSplitterArray = new[] { "~~~~" };

    private readonly string _fullErrorNewlineSplitter = "!!!!";

    protected override string? ToolName
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "dotnet.exe";
            else
                return "dotnet";
        }
    }

    public VSTestTask2()
    {
        LogStandardErrorAsError = false;
        StandardOutputImportance = "Normal";
    }

    protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
    {
        if (singleLine.StartsWith(_errorSplitter))
        {
            var parts = singleLine.Split(_errorSplitterArray, StringSplitOptions.None);
            if (parts.Length == 5)
            {
                var line = 0;
                var file = parts[1];
                var _ = !StringUtils.IsNullOrWhiteSpace(parts[3]) && int.TryParse(parts[2], out line);
                var code = parts[3];
                var message = parts[4];

                // Join them with space if both are not null,
                // otherwise use the one that is not null.
                string? error = code != null && message != null
                    ? code + " " + message
                    : code ?? message;

                file ??= string.Empty;

                Log.LogError(null, "VSTEST1", null, file, line, 0, 0, 0, error, null);
                return;
            }
        }

        if (singleLine.StartsWith(_fullErrorSplitter))
        {
            var parts = singleLine.Split(_fullErrorSplitterArray, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var message = parts[1];
                if (message != null)
                {
                    message = message.Replace(_fullErrorNewlineSplitter, Environment.NewLine);
                }

                string? stackTrace = null;
                if (parts.Length > 2)
                {
                    stackTrace = parts[2];
                    if (stackTrace != null)
                    {
                        stackTrace = stackTrace.Replace(_fullErrorNewlineSplitter, Environment.NewLine);
                    }
                }

                var logMessage = $"{message}{Environment.NewLine}StackTrace:{Environment.NewLine}{stackTrace}";

                Log.LogMessage(MessageImportance.Low, logMessage);
                return;
            }
        }

        base.LogEventsFromTextOutput(singleLine, messageImportance);
    }

    protected override string? GenerateCommandLineCommands()
    {
        return TestTaskUtils.CreateCommandLineArguments(this);
    }

    protected override string? GenerateFullPathToTool()
    {
        if (!ToolPath.IsNullOrEmpty())
        {
            return Path.Combine(Path.GetDirectoryName(Path.GetFullPath(ToolPath))!, ToolExe);
        }

        //TODO: https://github.com/dotnet/sdk/issues/20 Need to get the dotnet path from MSBuild?

        var dhp = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!dhp.IsNullOrEmpty())
        {
            var path = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dhp))!, ToolExe);
            if (File.Exists(path))
            {
                return path;
            }
        }

        if (File.Exists(ToolExe))
        {
            return Path.GetFullPath(ToolExe);
        }

        var values = Environment.GetEnvironmentVariable("PATH");
        foreach (var p in values!.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(p, ToolExe);
            if (File.Exists(fullPath))
                return fullPath;
        }

        return null;
    }
}
