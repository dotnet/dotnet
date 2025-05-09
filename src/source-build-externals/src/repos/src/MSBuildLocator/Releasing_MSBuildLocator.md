### Releasing MSBuildLocator

These instructions can only be followed by Microsoft-internal MSBuild maintainers.

1. Create a PR in https://github.com/microsoft/MSBuildLocator
2. Have it reviewed.
3. Once approved, merge it.
4. It will automatically start a pipeline build [here](https://dev.azure.com/devdiv/DevDiv/_build?definitionId=11881).
5. Once it succeeds, proceed to [our release page](https://dev.azure.com/devdiv/DevDiv/_release?_a=releases&view=mine&definitionId=408) and create a release (top-right). Specify the build that succeeded.
6. At some point, it will stop to request permission to continue. If you want to publish to NuGet, do so, clicking Approve. It will make a little more progress and push to NuGet! It will then give you the option to "resume" (or cancel) twice. Do so.

### Releasing a non-preview version of MSBuildLocator

The above steps will push a package to NuGet.org, but it is a preview version. To make a final release branded version, merge the latest changes into a release branch like `release/1.5`. Follow the steps as above, and it will publish a release package to NuGet.org.

### Changing the version
Nerdbank.GitVersioning automatically updates the build version with every commit. The major and minor versions (as well as the assembly version) are set manually in [version.json](https://github.com/microsoft/MSBuildLocator/blob/master/version.json).
