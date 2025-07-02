using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.Packages,
	BuildTarget.Build, BuildTarget.SignAssemblies
)]
public static partial class Packages
{ }
