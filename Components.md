# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@0ed1d3d](https://github.com/dotnet/arcade/tree/0ed1d3dc15420cd41c252b0c5d9474f197a29e06)*
- `src/aspire`  
*[dotnet/aspire@b88ce9e](https://github.com/dotnet/aspire/tree/b88ce9e7cb0430fb0b4e2d018f13694a4c733289)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@42af9fe](https://github.com/dotnet/aspnetcore/tree/42af9fe6ddd7c3f9cde04ac003bf97509881873b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@ff233bd](https://github.com/google/googletest/tree/ff233bdd4cac0a0bf6e5cd45bda3406814cb2796)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b9d928a](https://github.com/dotnet/cecil/tree/b9d928a9d65ed39b9257846e1b8e853cea609c00)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@57d7bae](https://github.com/dotnet/deployment-tools/tree/57d7baec5f331a145174d0e8f00d7bbfdf2b77d4)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@d94cc5a](https://github.com/dotnet/efcore/tree/d94cc5a79cb9d557485a95fba59ee3738c46aa81)*
- `src/emsdk`  
*[dotnet/emsdk@edf3e90](https://github.com/dotnet/emsdk/tree/edf3e90fa25b1fc4f7f63ceb45ef70f49c6b121a)*
- `src/fsharp`  
*[dotnet/fsharp@6b652cd](https://github.com/dotnet/fsharp/tree/6b652cd1d5c02d6f331818ef16479499161b5b6d)*
- `src/msbuild`  
*[dotnet/msbuild@ab7c289](https://github.com/dotnet/msbuild/tree/ab7c28995f198f717aa5fb823e6fc36cadf42254)*
- `src/nuget-client`  
*[nuget/nuget.client@675e8a8](https://github.com/nuget/nuget.client/tree/675e8a81acb53958e40dc21be5f562d00239a449)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@4b1b541](https://github.com/dotnet/razor/tree/4b1b541ed34e34b4b91202871f661879dabb4f55)*
- `src/roslyn`  
*[dotnet/roslyn@ba2f19a](https://github.com/dotnet/roslyn/tree/ba2f19abe63a267dc9b2d082d0e1904cd91c1947)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@db5ed35](https://github.com/dotnet/roslyn-analyzers/tree/db5ed35361c522fa21097337e406124faa1d59b5)*
- `src/runtime`  
*[dotnet/runtime@0fbd814](https://github.com/dotnet/runtime/tree/0fbd81404d1f211572387498474063bc6f407f0f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@b7ce675](https://github.com/dotnet/scenario-tests/tree/b7ce67571aff209313584ed0ee34f714270099dd)*
- `src/sdk`  
*[dotnet/sdk@3be343c](https://github.com/dotnet/sdk/tree/3be343cb41720c9e5b4e24f711be470a4bf4c312)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@bc43c32](https://github.com/dotnet/source-build-externals/tree/bc43c328b4788e3cb68e9b4dc764443f2aa29a24)*
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
*[dotnet/source-build-reference-packages@dde12f4](https://github.com/dotnet/source-build-reference-packages/tree/dde12f4d0a8f1662209e3f8dfd00719f4c61af09)*
- `src/sourcelink`  
*[dotnet/sourcelink@9700a2b](https://github.com/dotnet/sourcelink/tree/9700a2bfe76f993dbeb34439d4fe320c31182b2a)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@bf40ec0](https://github.com/dotnet/templating/tree/bf40ec00f3761436f9e503691191ed722575f1bb)*
- `src/test-templates`  
*[dotnet/test-templates@950cc4a](https://github.com/dotnet/test-templates/tree/950cc4a13d0c3f7d84558aec0552e15a5e8559d4)*
- `src/vstest`  
*[microsoft/vstest@07acde2](https://github.com/microsoft/vstest/tree/07acde22b65497e72de145d57167b83609a7f7fb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@c3466bf](https://github.com/dotnet/windowsdesktop/tree/c3466bfa163ab8f93e92a364dbc2b10890e5611b)*
- `src/winforms`  
*[dotnet/winforms@3b37d24](https://github.com/dotnet/winforms/tree/3b37d24254fe96bc6ad3fd63c6adea21c04f7024)*
- `src/wpf`  
*[dotnet/wpf@cc2eee2](https://github.com/dotnet/wpf/tree/cc2eee2a404793ac377fb3f42d94c8efdf16ff3c)*
- `src/xdt`  
*[dotnet/xdt@4ddd811](https://github.com/dotnet/xdt/tree/4ddd8113a29852380b7b929117bfe67f401ac320)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
