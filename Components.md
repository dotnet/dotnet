# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@3c393bb](https://github.com/dotnet/arcade/tree/3c393bbd85ae16ddddba20d0b75035b0c6f1a52d)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@282167d](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC282167d92299fc4dc1972edfc69193397a4114ad)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@6dae7eb](https://github.com/google/googletest/tree/6dae7eb4a5c3a169f3e298392bff4680224aa94a)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@e51bd36](https://github.com/dotnet/cecil/tree/e51bd3677d5674fa34bf5676c5fc5562206bf94e)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@2247e40](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC2247e4090da6e2312ff02deb00b984ce859b3060)*
- `src/emsdk`  
*[dotnet/emsdk@cd2146c](https://github.com/dotnet/emsdk/tree/cd2146c90fc68d5ff2db715337e696229c74651e)*
- `src/fsharp`  
*[dotnet/fsharp@f07a914](https://github.com/dotnet/fsharp/tree/f07a91420bec3f657153e16c9f047cf151c1179f)*
- `src/msbuild`  
*[dotnet/msbuild@5b86656](https://github.com/dotnet/msbuild/tree/5b866566089bfdd756730948f6418e2759b8850f)*
- `src/nuget-client`  
*[nuget/nuget.client@1975634](https://github.com/nuget/nuget.client/tree/19756345139c45de23bd196e9b4be01d48e84fdd)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@ad2bd3e](https://github.com/dotnet/razor/tree/ad2bd3e7a405afd7787cf0c045897e90270013a3)*
- `src/roslyn`  
*[dotnet/roslyn@1d59d9f](https://github.com/dotnet/roslyn/tree/1d59d9f40a804971aa2a4ddee7097dd668d48902)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@3d61c57](https://github.com/dotnet/roslyn-analyzers/tree/3d61c57c73c3dd5f1f407ef9cd3414d94bf0eaf2)*
- `src/runtime`  
*[_git/dotnet-runtime@0456c7e](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC0456c7e91c34003f26acf8606ba9d20e29f518bd)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@1009e3b](https://github.com/dotnet/scenario-tests/tree/1009e3b6d23e049de56b91de82fe975fe84444f8)*
- `src/sdk`  
*[dotnet/sdk@7b4a9df](https://github.com/dotnet/sdk/tree/7b4a9dfe00403fb304024bd3cdca864c77024254)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@4df883d](https://github.com/dotnet/source-build-externals/tree/4df883d781a4290873b3b968afc0ff0df7132507)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
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
*[dotnet/source-build-reference-packages@a2ad736](https://github.com/dotnet/source-build-reference-packages/tree/a2ad736b86ce484df4d2dfc967bd29cd9d1b5de9)*
- `src/sourcelink`  
*[dotnet/sourcelink@1f252d7](https://github.com/dotnet/sourcelink/tree/1f252d795c563515fe55a228f499e399b915363f)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@e07a90b](https://github.com/dotnet/templating/tree/e07a90b4df2f1b41a83064cfa3164611756c3746)*
- `src/test-templates`  
*[dotnet/test-templates@6be1945](https://github.com/dotnet/test-templates/tree/6be1945e663153cd1db4ccd31f6ca38e7f35330d)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@2e5178f](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GC2e5178f9d725c4becd3f04f9627f9e45d7469f03)*
- `src/winforms`  
*[_git/dotnet-winforms@bcf70f4](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GCbcf70f48f3ad07c7747c64601348e1df81f1f19e)*
- `src/wpf`  
*[_git/dotnet-wpf@c23c8e8](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GCc23c8e8b764b527c6bf96b8f38379faae3c4dc1d)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
