# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@beb827d](https://github.com/dotnet/arcade/tree/beb827ded6acdff8c7333dfc6583cc984a8f2620)*
- `src/aspire`  
*[dotnet/aspire@137e8dc](https://github.com/dotnet/aspire/tree/137e8dcae0a7b22c05f48c4e7a5d36fe3f00a8d7)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@99135af](https://github.com/dotnet/aspnetcore/tree/99135af51fa200682ecfc585011eaba907dea4ba)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@0953a17](https://github.com/google/googletest/tree/0953a17a4281fc26831da647ad3fcd5e21e6473b)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@9c94433](https://github.com/dotnet/cecil/tree/9c9443396f8deacceb8edb169890e52aac25f311)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@c7035b3](https://github.com/dotnet/efcore/tree/c7035b3ccc485a63bd03a64da4d9f0ec18f27948)*
- `src/emsdk`  
*[dotnet/emsdk@8e660ff](https://github.com/dotnet/emsdk/tree/8e660ff41e91879977e3a9d837e068bd72234c26)*
- `src/fsharp`  
*[dotnet/fsharp@19610c0](https://github.com/dotnet/fsharp/tree/19610c0b654766eec49d044cb97ca6eaa2a63d16)*
- `src/msbuild`  
*[dotnet/msbuild@4ae11fa](https://github.com/dotnet/msbuild/tree/4ae11fa8e4a86aef804cc79a42102641ad528106)*
- `src/nuget-client`  
*[nuget/nuget.client@1975634](https://github.com/nuget/nuget.client/tree/19756345139c45de23bd196e9b4be01d48e84fdd)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@a7648d0](https://github.com/dotnet/razor/tree/a7648d0ddc50f60c651bc2a25f4f36bdc2b496d7)*
- `src/roslyn`  
*[dotnet/roslyn@6a9d2b0](https://github.com/dotnet/roslyn/tree/6a9d2b077973e31b213517579f674b461053ac1c)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@2595aeb](https://github.com/dotnet/roslyn-analyzers/tree/2595aeb5e9a506f3f845c01be18d70ded045e33a)*
- `src/runtime`  
*[dotnet/runtime@3429fee](https://github.com/dotnet/runtime/tree/3429fee9ed58213a8916e1e2aa921fda6ba24aa2)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@76edac9](https://github.com/dotnet/scenario-tests/tree/76edac963b2c6296018c7bf1e04a03c8488076d3)*
- `src/sdk`  
*[dotnet/sdk@39a9e05](https://github.com/dotnet/sdk/tree/39a9e0574eebbee77fda3aa9e52f3d45701c11eb)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@fbe8f0b](https://github.com/dotnet/source-build-externals/tree/fbe8f0b52ae0e083461d89db7229f6d70e874644)*
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
*[dotnet/source-build-reference-packages@9ac2fa7](https://github.com/dotnet/source-build-reference-packages/tree/9ac2fa767b57b99e84560ca78ef48bd1c76ab0dd)*
- `src/sourcelink`  
*[dotnet/sourcelink@a8b2777](https://github.com/dotnet/sourcelink/tree/a8b2777e6aaf9d2f87c5adeec5b65aa19fa22f0c)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@a23da1c](https://github.com/dotnet/templating/tree/a23da1c15c737b5e121650cfa5a86805e74e34fc)*
- `src/test-templates`  
*[dotnet/test-templates@b144c27](https://github.com/dotnet/test-templates/tree/b144c275128d0407667fcd94f9fd1b6477cb571a)*
- `src/vstest`  
*[microsoft/vstest@c8f1628](https://github.com/microsoft/vstest/tree/c8f1628bcfce589e3165e171ecf8a8ce5b3c688f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@9a901a2](https://github.com/dotnet/windowsdesktop/tree/9a901a200d9605f5c73be47e3eb4879dd63b0537)*
- `src/winforms`  
*[dotnet/winforms@856508c](https://github.com/dotnet/winforms/tree/856508c70a15183eaac3328ee9cc2d40b7806435)*
- `src/wpf`  
*[dotnet/wpf@63eac6a](https://github.com/dotnet/wpf/tree/63eac6ac4758df3f2755018846d60bba4ba0916d)*
- `src/xdt`  
*[dotnet/xdt@4ddd811](https://github.com/dotnet/xdt/tree/4ddd8113a29852380b7b929117bfe67f401ac320)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
