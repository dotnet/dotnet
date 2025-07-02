using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.FormatSource,
	BuildTarget.RestoreTools
)]
public static partial class FormatSource
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Formatting source");

		var foundBOM = false;

		foreach (var (file, bytes) in context.FindFilesWithBOMs())
		{
			if (!foundBOM)
			{
				Console.WriteLine("  Removed UTF-8 byte order mark:");
				foundBOM = true;
			}

			Console.WriteLine("    - {0}", file[(context.BaseFolder.Length + 1)..]);
			File.WriteAllBytes(file, bytes.AsSpan().Slice(3).ToArray());
		}

		if (foundBOM)
			Console.WriteLine();

		await context.Exec("dotnet", $"format whitespace --folder {string.Join(" ", context.GetSkippedAnalysisFolders().Select(f => $"--exclude {f}"))}");
	}
}
