using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(BuildTarget.RestoreTools)]
public static partial class RestoreTools
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Restoring .NET Core command-line tools");

		await context.Exec("dotnet", $"tool restore --verbosity {context.Verbosity}");
	}
}
