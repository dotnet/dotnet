# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@60ae233](https://github.com/dotnet/arcade/tree/60ae233c3d77f11c5fdb53e570b64d503b13ba59)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@d11493f](https://github.com/dotnet/aspnetcore/tree/d11493ff125bffff4cb0db74dd62c8dc87c936d6)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@ff233bd](https://github.com/google/googletest/tree/ff233bdd4cac0a0bf6e5cd45bda3406814cb2796)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@c667bfe](https://github.com/dotnet/cecil/tree/c667bfea9cdbc5b5493e49e7ddc8dd635a217891)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@d18b30f](https://github.com/dotnet/efcore/tree/d18b30f63dd3473c0d2bc61683eb997a1e8ef6ca)*
- `src/emsdk`  
*[dotnet/emsdk@099439b](https://github.com/dotnet/emsdk/tree/099439b38815c2f6a7821d54dfdc4a3fa16537d1)*
- `src/fsharp`  
*[dotnet/fsharp@60fc17d](https://github.com/dotnet/fsharp/tree/60fc17ddcf7eac31e4e91a02324bee1e14a1c225)*
- `src/msbuild`  
*[dotnet/msbuild@314cf24](https://github.com/dotnet/msbuild/tree/314cf24eab4fbfbd1ec7318afae73588a7a5fe6f)*
- `src/nuget-client`  
*[nuget/nuget.client@1599103](https://github.com/nuget/nuget.client/tree/15991039d603b8da5d2603315fc9c5c2cfb91a07)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@fc8332f](https://github.com/dotnet/razor/tree/fc8332fdd7a535acc5dfe8254429c1e8e4a1487e)*
- `src/roslyn`  
*[dotnet/roslyn@08a167c](https://github.com/dotnet/roslyn/tree/08a167c19e5e04742b0922bdb1ea8046e9364f4b)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@fdb9965](https://github.com/dotnet/roslyn-analyzers/tree/fdb9965ce68c1f4e1c0ff301488adf9caa958615)*
- `src/runtime`  
*[dotnet/runtime@7384dc4](https://github.com/dotnet/runtime/tree/7384dc4b40071052d38fef092cefca9f66d3df65)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@4acdc96](https://github.com/dotnet/sdk/tree/4acdc967ec12e657c429164146efaab814d34761)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@457ff6e](https://github.com/dotnet/source-build-externals/tree/457ff6ef4705a0aa8de628a1f2a15474a05b7150)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.0`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.1`  
    *[mthalman/docker-creds-provider@6c73fa4](https://github.com/mthalman/docker-creds-provider/tree/6c73fa4784795ae07f49305a057abf5c473d2adb)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@0d066e6](https://github.com/dotnet/source-build-reference-packages/tree/0d066e61a30c2599d0ced871ea45acf0e10571af)*
- `src/sourcelink`  
*[dotnet/sourcelink@3f60d73](https://github.com/dotnet/sourcelink/tree/3f60d7363bb74fbd64cc98c18e566c929e1f5a22)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@500dcaf](https://github.com/dotnet/templating/tree/500dcaf2ad1b38ee681f4a6ddc869b4d4eeb6474)*
- `src/test-templates`  
*[dotnet/test-templates@3fb5ae4](https://github.com/dotnet/test-templates/tree/3fb5ae40a6e504c8163861ed2c7353e353263222)*
- `src/vstest`  
*[microsoft/vstest@54964cd](https://github.com/microsoft/vstest/tree/54964cdbcd254cbce066d3a2afa2b3908db51abd)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@e77880b](https://github.com/dotnet/windowsdesktop/tree/e77880b6c8a41cb5f04b2eb941cf272151b51c38)*
- `src/winforms`  
*[dotnet/winforms@1961ed8](https://github.com/dotnet/winforms/tree/1961ed892785d2216e53650d5c33c42de1289ae5)*
- `src/wpf`  
*[dotnet/wpf@a8c12d0](https://github.com/dotnet/wpf/tree/a8c12d0d17e71822d15f9de1bbdcc4f87f2b86dc)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
