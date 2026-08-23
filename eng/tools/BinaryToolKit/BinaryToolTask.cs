using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BinaryToolKit;

[MSBuildMultiThreadableTask]
public class BinaryToolTask : Microsoft.Build.Utilities.Task, IMultiThreadableTask
{
    /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
    public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

    [Required]
    public string Mode
    {
        set
        {
            if (Enum.TryParse<Modes>(value, out Modes parsedMode))
            {
                _mode = parsedMode;
            }
            else
            {
                throw new ArgumentException($"Invalid mode '{value}'. Valid modes are: {string.Join(", ", Enum.GetNames<Modes>())}");
            }
        }
    }

    [Required]
    public string TargetDirectory { get; set; } = string.Empty;

    [Required]
    public string AllowedBinariesFile { get; set; } = string.Empty;

    public string OutputReportDirectory { get; set; } = string.Empty;

    private Modes _mode;

    public override bool Execute()
    {
        try
        {
            ParseArgs();
            BinaryTool.Execute(Log, TaskEnvironment, TargetDirectory, OutputReportDirectory, AllowedBinariesFile, _mode);
        }
        catch (Exception ex)
        {
            Log.LogError(ex.Message);
            return false;
        }

        return !Log.HasLoggedErrors;
    }

    private void ParseArgs()
    {
        // TargetDirectory
        if (string.IsNullOrWhiteSpace(TargetDirectory) || !Directory.Exists(TaskEnvironment.GetAbsolutePath(TargetDirectory)))
        {
            throw new ArgumentException($"TargetDirectory '{TargetDirectory}' is required and must exist.");
        }

        // Need to remove trailing slash for consistency
        TargetDirectory = TargetDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // AllowedBinariesFile
        if (string.IsNullOrWhiteSpace(AllowedBinariesFile) || !File.Exists(TaskEnvironment.GetAbsolutePath(AllowedBinariesFile)))
        {
            throw new ArgumentException($"AllowedBinariesFile '{AllowedBinariesFile}' is required and must exist.");
        }

        // OutputReportDirectory
        if (string.IsNullOrWhiteSpace(OutputReportDirectory))
        {
            OutputReportDirectory = Path.Combine(TaskEnvironment.ProjectDirectory, "binary-report");
        }

        if (!Directory.Exists(TaskEnvironment.GetAbsolutePath(OutputReportDirectory)))
        {
            Directory.CreateDirectory(TaskEnvironment.GetAbsolutePath(OutputReportDirectory));
        }
    }
}
