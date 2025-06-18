using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimpleExec;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.PublishPackages,
	BuildTarget.SignPackages
)]
public static partial class PublishPackages
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Publishing NuGet packages");

		var pushApiKey = Environment.GetEnvironmentVariable("PUSH_APIKEY");
		var pushUri = Environment.GetEnvironmentVariable("PUSH_URI");

		if (string.IsNullOrWhiteSpace(pushApiKey) || string.IsNullOrWhiteSpace(pushUri))
		{
			context.WriteLineColor(ConsoleColor.Red, "One or more package publishing environment variables are missing: PUSH_APIKEY, PUSH_URI");
			throw new ExitCodeException(-1);
		}

		var packageFiles =
			Directory
				.GetFiles(context.PackageOutputFolder, "*.nupkg", SearchOption.AllDirectories)
				.Select(x => x.Substring(context.BaseFolder.Length + 1));

		foreach (var packageFile in packageFiles.OrderBy(x => x))
		{
			var args = $"nuget push --source {pushUri} --api-key {pushApiKey} {packageFile}";
			var redactedArgs = args.Replace(pushApiKey, "[redacted]");
			await context.Exec("dotnet", args, redactedArgs);
		}
	}
}
