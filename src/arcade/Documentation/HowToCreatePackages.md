# How to Create an Arcade Package

One of the goals of Arcade is to be a vehicle to provide code sharing. One of the ways to accomplish this is to pack each tool into its own NuGet package. This document assumes that you are adding a package called `ProjectOne` to Arcade and provides a list of things you should check while doing that.

## Things to Check

- Package sources must go into the `src\` folder on the root of the repository.

- Inside the `src\` folder create a folder representing the project namespace: `src\Microsoft.DotNet.ProjectOne`.

- The main sources for the package must go into `src\` folder. That is:

  `src\Microsoft.DotNet.ProjectOne\src`

- The sources for unit tests should go side-by-side with the main sources.

  `src\Microsoft.DotNet.ProjectOne\tests`

- You should include a `README.md` file on the root of the package.

- Make sure the namespace prefix of the package follows the name of the project. For instance:

  `namespace Microsoft.DotNet.ProjectOne.Model`

- Include the package on the `Arcade.slnx` file. This way it will be compiled automatically when the solution is built.

- If your package produces binaries that need to be signed, make sure to mark it as <IsPackable>true</IsPackable> or check the SignTool [documentation](../src/Microsoft.DotNet.SignTool/README.md) to see how to sign specific files.

- If the package needs to target a version of .NET Framework we recommend you to use the Arcade defined version, which is stored in the `$(NetMinimum)` or `$(NetFrameworkMinimum)` properties. For instance:

  `<TargetFrameworks>$(NetMinimum);netcoreapp2.0</TargetFrameworks>`

## Further References

- [`Overview.md`](Overview.md)

- [`..\eng\Versions.props`](../eng/Versions.props)
