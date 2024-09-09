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
*[dotnet/fsharp@20cca61](https://github.com/dotnet/fsharp/tree/20cca61a546fe378948f0550a0026ec6077c1600)*
- `src/msbuild`  
*[dotnet/msbuild@2206a05](https://github.com/dotnet/msbuild/tree/2206a054f2c82e91918809ea27d1ef5b3f7cfc4b)*
- `src/nuget-client`  
*[nuget/nuget.client@38010e5](https://github.com/nuget/nuget.client/tree/38010e5968f1afc9c210866bd2089957906a2492)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@308833d](https://github.com/dotnet/razor/tree/308833d8b85f790695be44aad273433286ff68a4)*
- `src/roslyn`  
*[dotnet/roslyn@bd5c00e](https://github.com/dotnet/roslyn/tree/bd5c00e5e09de8564093f42d87fe49d4971f2e84)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@3211f48](https://github.com/dotnet/roslyn-analyzers/tree/3211f48253bc18560156d90dc5e710d35f7d03fa)*
- `src/runtime`  
*[dotnet/runtime@0fbd814](https://github.com/dotnet/runtime/tree/0fbd81404d1f211572387498474063bc6f407f0f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@f1715bf](https://github.com/dotnet/scenario-tests/tree/f1715bf842371d15ecb5fc52d7dc87ab2e47df30)*
- `src/sdk`  
*[dotnet/sdk@59b0ee6](https://github.com/dotnet/sdk/tree/59b0ee66abf2077c5543954b5f03c7295bfafa49)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@fb56fcf](https://github.com/dotnet/source-build-externals/tree/fb56fcf997fa59ede9c5fbafbb4d2d8a22d392ac)*
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
*[dotnet/source-build-reference-packages@ad3c9aa](https://github.com/dotnet/source-build-reference-packages/tree/ad3c9aa85596f42c6a483233c50fab8cee8c412a)*
- `src/sourcelink`  
*[dotnet/sourcelink@a3a6c55](https://github.com/dotnet/sourcelink/tree/a3a6c5560d7593ede6e78835f53bbaa08a48a2df)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@75cf65b](https://github.com/dotnet/templating/tree/75cf65b0c85a704e977ef56ff962d8f8045cfa4a)*
- `src/test-templates`  
*[dotnet/test-templates@e833d46](https://github.com/dotnet/test-templates/tree/e833d4684ffa6144968f530a0b3250c540fae026)*
- `src/vstest`  
*[microsoft/vstest@07acde2](https://github.com/microsoft/vstest/tree/07acde22b65497e72de145d57167b83609a7f7fb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@c3466bf](https://github.com/dotnet/windowsdesktop/tree/c3466bfa163ab8f93e92a364dbc2b10890e5611b)*
- `src/winforms`  
*[dotnet/winforms@3b37d24](https://github.com/dotnet/winforms/tree/3b37d24254fe96bc6ad3fd63c6adea21c04f7024)*
- `src/wpf`  
*[dotnet/wpf@cc2eee2](https://github.com/dotnet/wpf/tree/cc2eee2a404793ac377fb3f42d94c8efdf16ff3c)*
- `src/xdt`  
*[dotnet/xdt@c2a9df9](https://github.com/dotnet/xdt/tree/c2a9df9c1867454039a1223cef1c090359e33646)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
