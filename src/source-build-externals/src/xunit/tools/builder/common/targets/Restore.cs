using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(BuildTarget.Restore)]
public static partial class Restore
{
	public static Task OnExecute(BuildContext context)
	{
		context.BuildStep("Restoring NuGet packages");

		return context.Exec("dotnet", $"restore --verbosity {context.Verbosity}");
	}
}
