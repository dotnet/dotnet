using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.BuildAll,
	BuildTarget.AnalyzeSource, BuildTarget.Test, BuildTarget.Packages
)]
public class BuildAll { }
