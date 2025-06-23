using System.IO;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(BuildTarget.Clean)]
public static partial class Clean
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Cleaning build artifacts");

		await context.Exec("dotnet", $"clean --verbosity {context.Verbosity} --nologo");

		if (Directory.Exists(context.ArtifactsFolder))
			Directory.Delete(context.ArtifactsFolder, recursive: true);
	}
}
