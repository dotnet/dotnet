using System;
using System.Linq;
using System.Threading.Tasks;
using SimpleExec;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.AnalyzeSource,
	BuildTarget.RestoreTools
)]
public static partial class AnalyzeSource
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Analyzing source (if this fails, run './build FormatSource' to fix)");

		var foundBOM = false;

		foreach (var (file, _) in context.FindFilesWithBOMs())
		{
			if (!foundBOM)
			{
				Console.WriteLine("  One of more files were found with UTF-8 byte order marks:");
				foundBOM = true;
			}

			Console.WriteLine("    - {0}", file[(context.BaseFolder.Length + 1)..]);
		}

		if (foundBOM)
			throw new ExitCodeException(-1);

		await context.Exec("dotnet", $"format whitespace --verify-no-changes --folder --verbosity {context.Verbosity} {string.Join(" ", context.GetSkippedAnalysisFolders().Select(f => $"--exclude {f}"))}");
	}
}
