# Microsoft.Windows.Compatibility package

This Windows Compatibility Pack provides access to APIs that were previously available only for .NET Framework. It can be used from both .NET as well as .NET Standard.

### How to add a new dependency
1. Add the package version to the repository's Versions.props file.
2. Add the package dependency to the repository's Version.Details.xml file. If the dependency doesn't come from a repository with a direct subscription to (i.e. winforms and runtime), add the `CoherentParentDependency` attribute to the dependency.
3. Add the package reference into the Microsoft.Windows.Compatibility.csproj file and use the version just defined in the Versions.props file.
4. If a `CoherentParentDependency` attribute was required, a dependency must be added to the repository dependency chain as well. I.e. to add a new dependency from runtime, a dependency must also be added in winforms and wpf's Version.Details.xml file.