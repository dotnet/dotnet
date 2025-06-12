# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@2ce3f8c](https://github.com/dotnet/arcade/tree/2ce3f8c7b2614c2b19985b669ffcd934bc39ffd1)*
- `src/aspire`  
*[_git/dotnet-aspire@48e42f5](https://dev.azure.com/dnceng/internal/_git/dotnet-aspire/?version=GC48e42f59d64d84b404e904996a9ed61f2a17a569)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@1988c37](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC1988c37e68bc83b1a0d3d230276a1ae8f89da38b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@e9092b1](https://github.com/google/googletest/tree/e9092b12dc3cf617d47578f13a1f64285cfa5b2f)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@45dd3a7](https://github.com/dotnet/cecil/tree/45dd3a73dd5b64b010c4251303b3664bb30df029)*
- `src/command-line-api`  
*[dotnet/command-line-api@02fe27c](https://github.com/dotnet/command-line-api/tree/02fe27cd6a9b001c8feb7938e6ef4b3799745759)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@5255d40](https://github.com/dotnet/deployment-tools/tree/5255d40e228ea1d4b624781b5b97ec16484a3b4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/tree/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@2b0cca8](https://github.com/dotnet/emsdk/tree/2b0cca8ad3a88e02fe0878139009ffffde071f1f)*
- `src/format`  
*[dotnet/format@664013f](https://github.com/dotnet/format/tree/664013fbe5e68941be81f9a710058db028da788f)*
- `src/fsharp`  
*[dotnet/fsharp@fc5e9ed](https://github.com/dotnet/fsharp/tree/fc5e9eda234e2b69aa479f4f83faddc31fdd4da7)*
- `src/installer`  
*[dotnet/installer@89f721d](https://github.com/dotnet/installer/tree/89f721d3f6fafacaad9ed5a0ad377001db9be8d9)*
- `src/msbuild`  
*[_git/DotNet-msbuild-Trusted@3ab07f0](https://dev.azure.com/devdiv/DevDiv/_git/DotNet-msbuild-Trusted/?version=GC3ab07f0cf43d15c3d05da60fb0ff44b49799ed5e)*
- `src/nuget-client`  
*[_git/NuGet-NuGet.Client-Trusted@550277e](https://dev.azure.com/devdiv/DevDiv/_git/NuGet-NuGet.Client-Trusted/?version=GC550277e0616e549446f03fda35d3e23dff75dc01)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@4c8bb4f](https://github.com/dotnet/razor/tree/4c8bb4ff79523da1aa8ffbf2834ede5118f73d60)*
- `src/roslyn`  
*[dotnet/roslyn@a77b6e4](https://github.com/dotnet/roslyn/tree/a77b6e41d030878096fa496e00e4c0dffe5af9f0)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@de4aff5](https://github.com/dotnet/roslyn-analyzers/tree/de4aff542dacdc8027c1b4c4580a385dc097341f)*
- `src/runtime`  
*[_git/dotnet-runtime@ef853a7](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCef853a71052646a42abf17e888ec6d9a69614ad9)*
- `src/sdk`  
*[_git/dotnet-sdk@ebec7e8](https://dev.azure.com/dnceng/internal/_git/dotnet-sdk/?version=GCebec7e89791189b708284a372160e80906724174)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@16bcad1](https://github.com/dotnet/source-build-externals/tree/16bcad1c13be082bd52ce178896d1119a73081a9)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a607fa5](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/a607fa5e0005a6178cf1d2fed4fa0f8179cdb186)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
    *[mthalman/docker-creds-provider@6e1ecd0](https://github.com/mthalman/docker-creds-provider/tree/6e1ecd0a80755f9f0e88dc23b98b52f51a77c65e)*
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
*[dotnet/source-build-reference-packages@cee2d05](https://github.com/dotnet/source-build-reference-packages/tree/cee2d057118e3ad3c58a72a23271ad88b87d4d4d)*
- `src/sourcelink`  
*[dotnet/sourcelink@94eaac3](https://github.com/dotnet/sourcelink/tree/94eaac3385cafff41094454966e1af1d1cf60f00)*
- `src/symreader`  
*[dotnet/symreader@2c8079e](https://github.com/dotnet/symreader/tree/2c8079e2e8e78c0cd11ac75a32014756136ecdb9)*
- `src/templating`  
*[dotnet/templating@a1aab40](https://github.com/dotnet/templating/tree/a1aab400bac5186c2c21df38edb272f0d3123b4e)*
- `src/test-templates`  
*[dotnet/test-templates@1e5f360](https://github.com/dotnet/test-templates/tree/1e5f3603af2277910aad946736ee23283e7f3e16)*
- `src/vstest`  
*[microsoft/vstest@aa59400](https://github.com/microsoft/vstest/tree/aa59400b11e1aeee2e8af48928dbd48748a8bef9)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/tree/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
