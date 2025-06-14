using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace BinaryToolKit;

public class BinaryToolTask : BuildTask
{
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

    public string OutputReportDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "binary-report");

    private Modes _mode;

    public override bool Execute() => ExecuteAsync().GetAwaiter().GetResult();

    private async Task<bool> ExecuteAsync()
    {
        try
        {
            ParseArgs();
            await BinaryTool.ExecuteAsync(Log, TargetDirectory, OutputReportDirectory, AllowedBinariesFile, _mode);
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
        if (string.IsNullOrWhiteSpace(TargetDirectory) || !Directory.Exists(TargetDirectory))
        {
            throw new ArgumentException($"TargetDirectory '{TargetDirectory}' is required and must exist.");
        }

        // Need to remove trailing slash for consistency
        TargetDirectory = TargetDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // AllowedBinariesFile
        if (string.IsNullOrWhiteSpace(AllowedBinariesFile) || !File.Exists(AllowedBinariesFile))
        {
            throw new ArgumentException($"AllowedBinariesFile '{AllowedBinariesFile}' is required and must exist.");
        }

        // OutputReportDirectory
        if (string.IsNullOrWhiteSpace(OutputReportDirectory))
        {
            throw new ArgumentException("OutputReportDirectory cannot be null or empty.");
        }

        if (!Directory.Exists(OutputReportDirectory))
        {
            Directory.CreateDirectory(OutputReportDirectory);
        }
    }
}
