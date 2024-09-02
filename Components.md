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
*[dotnet/aspnetcore@42af9fe](https://github.com/dotnet/aspnetcore/tree/42af9fe6ddd7c3f9cde04ac003bf97509881873b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@ff233bd](https://github.com/google/googletest/tree/ff233bdd4cac0a0bf6e5cd45bda3406814cb2796)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@ed276e7](https://github.com/dotnet/cecil/tree/ed276e79e30bffc3e6405afa8a9323ec7e67c700)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@5838149](https://github.com/dotnet/diagnostics/tree/5838149521e80185e0dbe79720bd9ce6e81d2d35)*
- `src/efcore`  
*[dotnet/efcore@d94cc5a](https://github.com/dotnet/efcore/tree/d94cc5a79cb9d557485a95fba59ee3738c46aa81)*
- `src/emsdk`  
*[dotnet/emsdk@459c929](https://github.com/dotnet/emsdk/tree/459c92904b224d125a350a3f3e431fe90152a95e)*
- `src/fsharp`  
*[dotnet/fsharp@60fc17d](https://github.com/dotnet/fsharp/tree/60fc17ddcf7eac31e4e91a02324bee1e14a1c225)*
- `src/msbuild`  
*[dotnet/msbuild@d17ec72](https://github.com/dotnet/msbuild/tree/d17ec720df6ea81145fe462834ba3a68aa19d766)*
- `src/nuget-client`  
*[nuget/nuget.client@1599103](https://github.com/nuget/nuget.client/tree/15991039d603b8da5d2603315fc9c5c2cfb91a07)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@fc8332f](https://github.com/dotnet/razor/tree/fc8332fdd7a535acc5dfe8254429c1e8e4a1487e)*
- `src/roslyn`  
*[dotnet/roslyn@d30a570](https://github.com/dotnet/roslyn/tree/d30a570bb6c9b2f204db9e6ee65fc5316449fd4e)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@fdb9965](https://github.com/dotnet/roslyn-analyzers/tree/fdb9965ce68c1f4e1c0ff301488adf9caa958615)*
- `src/runtime`  
*[dotnet/runtime@c4d7f7c](https://github.com/dotnet/runtime/tree/c4d7f7c6f2e2f34f07e64c6caa3bf9b2ce915cc1)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@b3c7d9d](https://github.com/dotnet/sdk/tree/b3c7d9dcbe83abf9e37225a06ee8c5c014fea3ae)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@51b029e](https://github.com/dotnet/source-build-externals/tree/51b029e3272f35af0af337823cd122725f316c69)*
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
*[dotnet/source-build-reference-packages@b3570a1](https://github.com/dotnet/source-build-reference-packages/tree/b3570a108db2466aa25da3ac15849dfb79fc6474)*
- `src/sourcelink`  
*[dotnet/sourcelink@85fb5b9](https://github.com/dotnet/sourcelink/tree/85fb5b9ab7a4d308ccac2c376fb4661df5e752ce)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@9fcf5a8](https://github.com/dotnet/templating/tree/9fcf5a8ed78c2f28cae8230c51b3b07c28311f56)*
- `src/test-templates`  
*[dotnet/test-templates@0b1b52e](https://github.com/dotnet/test-templates/tree/0b1b52e10bf70c53ed9fc63da52281b7d10ddd48)*
- `src/vstest`  
*[microsoft/vstest@c5c8ee5](https://github.com/microsoft/vstest/tree/c5c8ee586c6bd58eee6e845aee32e88f789c127d)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@3b6a7eb](https://github.com/dotnet/windowsdesktop/tree/3b6a7ebeb759a4cb7848130d470fb30486ebddf5)*
- `src/winforms`  
*[dotnet/winforms@57b99ce](https://github.com/dotnet/winforms/tree/57b99cede4e438faed1b4a266583c344ecb7acc7)*
- `src/wpf`  
*[dotnet/wpf@51e3d84](https://github.com/dotnet/wpf/tree/51e3d8441f42707ea242c2a15de89efbd7f9cc5d)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
