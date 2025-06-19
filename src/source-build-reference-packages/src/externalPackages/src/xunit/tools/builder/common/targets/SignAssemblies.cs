using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.SignAssemblies,
	BuildTarget.Build
)]
public static partial class SignAssemblies
{ }
