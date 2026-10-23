# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@659e244](https://github.com/dotnet/arcade/tree/659e244c0464986411ccd6ccf51084c96ae66091)*
- `src/aspire`  
*[_git/dotnet-aspire@48e42f5](https://dev.azure.com/dnceng/internal/_git/dotnet-aspire/?version=GC48e42f59d64d84b404e904996a9ed61f2a17a569)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@22cdf6c](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC22cdf6c9e048a2949dd0d4c6b43f8d914905d9cb)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@36ba75f](https://github.com/google/googletest/tree/36ba75f0ad5383a9759f17f3f72fd4661c72cb6d)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@365965f](https://github.com/aspnet/MessagePack-CSharp/tree/365965f0d8c13c40ff8fde25882066b90f569c7a)*
- `src/cecil`  
*[dotnet/cecil@a1cb4f6](https://github.com/dotnet/cecil/tree/a1cb4f641df8967b3338e6f630f594ca4962b467)*
- `src/command-line-api`  
*[dotnet/command-line-api@02fe27c](https://github.com/dotnet/command-line-api/tree/02fe27cd6a9b001c8feb7938e6ef4b3799745759)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@5255d40](https://github.com/dotnet/deployment-tools/tree/5255d40e228ea1d4b624781b5b97ec16484a3b4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/tree/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[_git/dotnet-emsdk@e4d02ca](https://dev.azure.com/dnceng/internal/_git/dotnet-emsdk/?version=GCe4d02cab49b7d36b894a073247f90a6dcece276f)*
- `src/format`  
*[dotnet/format@a8be5ff](https://github.com/dotnet/format/tree/a8be5ff9c558f921f565c461dd7688905f84742a)*
- `src/fsharp`  
*[dotnet/fsharp@fc5e9ed](https://github.com/dotnet/fsharp/tree/fc5e9eda234e2b69aa479f4f83faddc31fdd4da7)*
- `src/installer`  
*[dotnet/installer@0f63c85](https://github.com/dotnet/installer/tree/0f63c859c6851e1c48dff83a41391bc3f555e909)*
- `src/msbuild`  
*[dotnet/msbuild@293f665](https://github.com/dotnet/msbuild/tree/293f66545288e700258d9d979e84b67b585a4f93)*
- `src/nuget-client`  
*[_git/NuGet-NuGet.Client-Trusted@60584b9](https://dev.azure.com/devdiv/DevDiv/_git/NuGet-NuGet.Client-Trusted/?version=GC60584b9c2451c0e324655f3968bc4ed1a1a8c85c)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@e137c03](https://github.com/dotnet/razor/tree/e137c0322ddb9e3d4f4a5de385647baa9719f9a6)*
- `src/roslyn`  
*[dotnet/roslyn@38896ab](https://github.com/dotnet/roslyn/tree/38896ab4e7cee896fcde8a4e26914a777c794e3b)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@0963af6](https://github.com/dotnet/roslyn-analyzers/tree/0963af6bfc2a3d5083faa2b7c1c91a4eee29a6f4)*
- `src/runtime`  
*[_git/dotnet-runtime@dfe48c5](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCdfe48c5f82448e8e9e51dbfa8eccf8af750c4f92)*
- `src/sdk`  
*[_git/dotnet-sdk@60eafac](https://dev.azure.com/dnceng/internal/_git/dotnet-sdk/?version=GC60eafacf90a5ee050fcd930a70f4e3078c0c617d)*
- `src/source-build-assets`  
*[dotnet/source-build-assets@57bfccd](https://github.com/dotnet/source-build-assets/tree/57bfccd8cd6091e138f487653e7eefcfef2e455a)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@669661e](https://github.com/dotnet/source-build-externals/tree/669661ea25e55b41167966df076c15ac70bf3718)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@ff96e25](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/ff96e2534408a4b7dfafc5c1edf4151c30affac9)*
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
*[dotnet/source-build-reference-packages@453ab23](https://github.com/dotnet/source-build-reference-packages/tree/453ab23d599ac904ded70bd9194959a76b05702f)*
- `src/sourcelink`  
*[dotnet/sourcelink@23d09f7](https://github.com/dotnet/sourcelink/tree/23d09f7a0e5cfd3829eafb8f5b7c87e98f29f5ef)*
- `src/symreader`  
*[dotnet/symreader@2c8079e](https://github.com/dotnet/symreader/tree/2c8079e2e8e78c0cd11ac75a32014756136ecdb9)*
- `src/templating`  
*[_git/dotnet-templating@312eb90](https://dev.azure.com/dnceng/internal/_git/dotnet-templating/?version=GC312eb90c8d8caa1c1bbd47bc80a55f23f2a4337d)*
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
