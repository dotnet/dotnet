namespace Xunit.BuildTools.Models;

public partial class BuildTarget
{
	public const string AnalyzeSource = nameof(AnalyzeSource);
	public const string Build = nameof(Build);
	public const string BuildAll = nameof(BuildAll);
	public const string Clean = nameof(Clean);
	public const string FormatSource = nameof(FormatSource);
	public const string Packages = nameof(Packages);
	public const string PublishPackages = nameof(PublishPackages);
	public const string Restore = nameof(Restore);
	public const string RestoreTools = nameof(RestoreTools);
	public const string SignAssemblies = nameof(SignAssemblies);
	public const string SignPackages = nameof(SignPackages);
	public const string Test = nameof(Test);
}
