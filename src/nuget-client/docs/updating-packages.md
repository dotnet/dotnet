# Updating Packages in the NuGet.Client repo

The short summary is that the NuGet.Client repo can't always upgrade packages to their latest versions, even if they have a known vulnerability (in which case we need to validate that we're not using the package in a way that exposes us to the vulnerability).
To be safe, we have to wait until both the .NET SDK and Visual Studio update to include the higher version assemblies before we can take on the new version, but of course there's more subtlety if you dig into the details.
If unsure, ask.

Generally, the .NET SDK requirements will be more constraining than Visual Studio.
This is because the .NET SDK team makes promises to customers that newer versions of the .NET SDK will work on older versions of Visual Studio, as documented at https://aka.ms/dotnet/matrixofpain.
For example, the .NET 8.0.100 SDK came out with 17.8, but has a minimum VS version of 17.7 (as long as the customer targets `net7.0` or earlier).
Then, 8.0.200, 8.0.300, and 8.0.400 all had minimum VS versions of 17.8.
This means that NuGet could not update any Microsoft.Build.* package above version 17.8 until the 9.0.100 SDK.
Similarly, 9.0.200, 9.0.300 and 9.0.400 are expected to have a min VS version of 17.12, so the corresponding versions of NuGet.Client must not update the MSBuild packages above 17.12 until .NET 10.

However, this is not just limited to MSBuild packages, but since MSBuild needs to load assemblies like System.Text.Json, System.Memory, System.Buffers, etc, NuGet needs to limit the package version of all those packages to the version that MSBuild ships in the oldest version of VS that the .NET SDK supports.

Additionally, since NuGet flows into https://github.com/dotnet/sdk, we need their CI to pass in order for them to take our new versions.
dotnet/sdk tests include Visual Studio tests, running on the .NET (Core) Engineering (dnceng) team's [Helix infrastructure](https://github.com/dotnet/arcade/blob/main/Documentation/Helix.md).
Helix uses the public preview versions of Visual Studio, but may lag behind the public release of VS previews by a few days.
This means for packages like System.Text.Json, we need to wait until MSBuild & VS updates to a newer version, then for that preview of VS to be available on Helix (a few days after it ships publicly), before we should upgrade.

## Technical details

Generally any package used by a project under `src/NuGet.Core/` needs to follow the process described by this document.
Technically, it's only packages that dotnet/sdk's MSBuild tasks use that have the VS public preview constraint.
But it's easier to say it's dependencies of all projects under `NuGet.Core`.

If you have the appropriate version of Visual Studio installed and want to check what assembly version it contains, make sure to look at `MSBuild\Current\bin`.
While VS's `PublicAssemblies` directory will usually contain the same version of assemblies as MSBuild, the build tasks are going to load MSBuild's copy of the assemblies, so just in case they're different versions, the MSBuild assemblies are the source of truth.

For packages used only by `src/NuGet.Core/NuGet.CommandLine.XPlat`, it's safe to update any time.
This project is only used by the `dotnet` CLI, and even though that's included in the .NET SDK, it doesn't have a .NET Framework version that Visual Studio needs to load.

The payload that NuGet ships in VS is in the VSIX file, created by the `src/NuGet.Clients/NuGet.VisualStudio.Client` project.
Any assembly listed in the `.vsixignore` file certainly follows the above advice, we cannot update NuGet's dependency version until VS includes it first (and those assemblies will be loaded from the `PublicAssemblies` or `PrivateAssemblies` directories, not the MSBuild directory, as the MSBuild tasks do).
For packages that are only referenced by `src/NuGet.Clients/` projects, it's safe to update as soon as VS's main branch has the updated package/assembly version.
In that case, it's not necessary to wait until the dependency is updated in a public preview, since it won't affect the .NET SDK.
For packages/assemblies that we include in the vsix (see the `.vsixinclude` file), it's probably safe to update at any time.
