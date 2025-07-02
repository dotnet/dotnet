using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.Build,
	BuildTarget.Restore
)]
public static partial class Build
{
	public static partial Task PerformBuild(BuildContext context);

	public static Task OnExecute(BuildContext context) =>
		PerformBuild(context);
}
