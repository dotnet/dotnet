# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e124063](https://github.com/dotnet/arcade/tree/e1240639569fad610705b52713d6d6b19f8fe433)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@324a351](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC324a351f7f1ae6c17b6a8661903e2a7921a7d75c)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@73a63ea](https://github.com/google/googletest/tree/73a63ea05dc8ca29ec1d2c1d66481dd0de1950f1)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@4fe10b2](https://github.com/dotnet/cecil/tree/4fe10b2349082f474928ac1b97ce207b70dc2307)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@001e1d3](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC001e1d31c562c1d246e5a6531b607bf6c851f688)*
- `src/emsdk`  
*[dotnet/emsdk@918f4ea](https://github.com/dotnet/emsdk/tree/918f4eac9e7d238562abcc364ec417be11b108f0)*
- `src/fsharp`  
*[dotnet/fsharp@47d4e3f](https://github.com/dotnet/fsharp/tree/47d4e3f91e4e5414b6dafbf14288b9c5a798ef99)*
- `src/msbuild`  
*[dotnet/msbuild@07da1b9](https://github.com/dotnet/msbuild/tree/07da1b9a89da6d00c5a5a6a385cdcebfdfed7110)*
- `src/nuget-client`  
*[_git/NuGet-NuGet.Client-Trusted@c921250](https://dev.azure.com/devdiv/DevDiv/_git/NuGet-NuGet.Client-Trusted/?version=GCc92125011405028c945371a89e1a1eb0e735456d)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@cff92f3](https://github.com/dotnet/razor/tree/cff92f3cc3f19a607ddbb7a0cddfbccf87a1c061)*
- `src/roslyn`  
*[dotnet/roslyn@fc52718](https://github.com/dotnet/roslyn/tree/fc52718eccdb37693a40a518b1178b1e23114e68)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5ef1abb](https://github.com/dotnet/roslyn-analyzers/tree/5ef1abb57ce3df89eae65ecadeb1ddbab323ae05)*
- `src/runtime`  
*[_git/dotnet-runtime@4250c83](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC4250c8399aa851d2d6a95efbdcc5c4c12311e024)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@0f750c5](https://github.com/dotnet/scenario-tests/tree/0f750c53b3052a593c6daacc0d60eb8c0d2d9cf1)*
- `src/sdk`  
*[dotnet/sdk@8283b53](https://github.com/dotnet/sdk/tree/8283b533a804f4e5f5fe58c04bdfd690e186b0ce)*
- `src/source-build-assets`  
*[dotnet/source-build-assets@8e19a1b](https://github.com/dotnet/source-build-assets/tree/8e19a1b4f607fcbecc4edbd322d77a60d4e25c3c)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@16c380d](https://github.com/dotnet/source-build-externals/tree/16c380d1ce5fa0b24e232251c31cb013bbf3365f)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
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
    - `src/source-build-externals/src/vs-solutionpersistence`  
    *[microsoft/vs-solutionpersistence@19c3ca7](https://github.com/microsoft/vs-solutionpersistence/tree/19c3ca7dc997dba2b3a86f6c666e9f717f34c8fe)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@a9cadb0](https://github.com/dotnet/source-build-reference-packages/tree/a9cadb09ddcc99b1e535efb0648047634f0c4f40)*
- `src/sourcelink`  
*[dotnet/sourcelink@657ade4](https://github.com/dotnet/sourcelink/tree/657ade4711e607cc4759e89e0943aa1ca8aadc63)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@7a3da72](https://github.com/dotnet/templating/tree/7a3da726823697767f6664d1386852a31f4d50cc)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@46c3072](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GC46c3072d4539c2323f276f6282a7574feaa6a2a5)*
- `src/winforms`  
*[_git/dotnet-winforms@fe62797](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GCfe627979c58f8a0b591850cfebb46adb9002c69b)*
- `src/wpf`  
*[_git/dotnet-wpf@a01db3a](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GCa01db3af68be8bfc869550b0be7cfc5881e4c685)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
