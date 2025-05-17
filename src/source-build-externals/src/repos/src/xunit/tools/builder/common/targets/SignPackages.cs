using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.SignPackages,
	BuildTarget.RestoreTools, BuildTarget.Packages
)]
public static partial class SignPackages
{
	public static Task OnExecute(BuildContext context)
	{
		context.BuildStep("Signing NuGet packages");

		return context.SignFiles(context.PackageOutputFolder, "**/*.nupkg");
	}
}
