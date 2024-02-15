# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@61ae141](https://github.com/dotnet/arcade/tree/61ae141d2bf3534619265c8f691fd55dc3e75147)*
- `src/aspire`  
*[_git/dotnet-aspire@48e42f5](https://dev.azure.com/dnceng/internal/_git/dotnet-aspire/?version=GC48e42f59d64d84b404e904996a9ed61f2a17a569)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@8e941eb](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC8e941eb42f819adb116b881195158b3887a70a1c)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@7e33b6a](https://github.com/google/googletest/tree/7e33b6a1c497ced1e98fc60175aeb4678419281c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@45dd3a7](https://github.com/dotnet/cecil/tree/45dd3a73dd5b64b010c4251303b3664bb30df029)*
- `src/command-line-api`  
*[dotnet/command-line-api@02fe27c](https://github.com/dotnet/command-line-api/tree/02fe27cd6a9b001c8feb7938e6ef4b3799745759)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@5957c5c](https://github.com/dotnet/deployment-tools/tree/5957c5c5f85f17c145e7fab4ece37ad6aafcded9)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/tree/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@201f4da](https://github.com/dotnet/emsdk/tree/201f4dae9d1a1e105d8ba86d7ece61eed1f665e0)*
- `src/format`  
*[dotnet/format@28925c0](https://github.com/dotnet/format/tree/28925c0e519d66c80328aacf973b74e40bb1d5bd)*
- `src/fsharp`  
*[dotnet/fsharp@424e4b7](https://github.com/dotnet/fsharp/tree/424e4b7cffb7656efd63f7a905a2498e39011104)*
- `src/installer`  
*[dotnet/installer@68e8abb](https://github.com/dotnet/installer/tree/68e8abb1d3e1a240a6e4c29dcd220aae91681676)*
- `src/msbuild`  
*[dotnet/msbuild@195e7f5](https://github.com/dotnet/msbuild/tree/195e7f5a3a8e51c37d83cd9e54cb99dc3fc69c22)*
- `src/nuget-client`  
*[nuget/nuget.client@0dd5a1e](https://github.com/nuget/nuget.client/tree/0dd5a1ea536201af94725353e4bc711d7560b246)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8c7fb27](https://github.com/dotnet/razor/tree/8c7fb27bf8e8d4f9ff8080b624b35bca5e812e97)*
- `src/roslyn`  
*[dotnet/roslyn@7b75981](https://github.com/dotnet/roslyn/tree/7b75981cf3bd520b86ec4ed00ec156c8bc48e4eb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b4d9a13](https://github.com/dotnet/roslyn-analyzers/tree/b4d9a1334d5189172977ba8fddd00bda70161e4a)*
- `src/runtime`  
*[_git/dotnet-runtime@bf5e279](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCbf5e279d9239bfef5bb1b8d6212f1b971c434606)*
- `src/sdk`  
*[_git/dotnet-sdk@0881444](https://dev.azure.com/dnceng/internal/_git/dotnet-sdk/?version=GC08814442c7c94f8b059c818902b6883a69844860)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@83274d9](https://github.com/dotnet/source-build-externals/tree/83274d94c7e2ff21081b0d75ecbec2da2241f831)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a607fa5](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/a607fa5e0005a6178cf1d2fed4fa0f8179cdb186)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.0`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.1`  
    *[mthalman/docker-creds-provider@b381eaf](https://github.com/mthalman/docker-creds-provider/tree/b381eafbeecb1039f5839fc98ef45e7b3e52dee9)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@453a37e](https://github.com/dotnet/source-build-reference-packages/tree/453a37ef7ae6c335cd49b3b9ab7713c87faeb265)*
- `src/sourcelink`  
*[dotnet/sourcelink@f6b97c9](https://github.com/dotnet/sourcelink/tree/f6b97c9ef635ad82b195a952ac0e0a969b9cf39c)*
- `src/symreader`  
*[dotnet/symreader@2c8079e](https://github.com/dotnet/symreader/tree/2c8079e2e8e78c0cd11ac75a32014756136ecdb9)*
- `src/templating`  
*[dotnet/templating@fbeda47](https://github.com/dotnet/templating/tree/fbeda4751af816d3555ea4dff95dbb3a5da35543)*
- `src/test-templates`  
*[dotnet/test-templates@1e5f360](https://github.com/dotnet/test-templates/tree/1e5f3603af2277910aad946736ee23283e7f3e16)*
- `src/vstest`  
*[microsoft/vstest@3259862](https://github.com/microsoft/vstest/tree/3259862fadd5e784564e7d920a2d61a75f415a79)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/tree/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
