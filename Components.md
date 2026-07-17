# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@efce34e](https://github.com/dotnet/arcade/tree/efce34e9f9f25af27e2b471fbbf8c21f1ac2e318)*
- `src/aspire`  
*[microsoft/aspire@5fa9337](https://github.com/microsoft/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@a95d659](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GCa95d6599f37762ec8394ba9ca01e8263b2e36a26)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@7140cd4](https://github.com/google/googletest/tree/7140cd416cecd7462a8aae488024abeee55598e4)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9614e6f](https://github.com/aspnet/MessagePack-CSharp/tree/9614e6f396e959ad67e4ba655d5ab6e1311bed23)*
- `src/cecil`  
*[dotnet/cecil@5eaa92e](https://github.com/dotnet/cecil/tree/5eaa92e08ad36a380888b6e99b4f6c09a5e24815)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@0d9843b](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC0d9843b88793b08762c73ef59a9083357e602ef5)*
- `src/emsdk`  
*[dotnet/emsdk@8e475db](https://github.com/dotnet/emsdk/tree/8e475db5d45016c77714cce29077ae8c66d12d53)*
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
*[dotnet/roslyn-analyzers@b9b5452](https://github.com/dotnet/roslyn-analyzers/tree/b9b54526b7908ea519b503196100a34dd2e52374)*
- `src/runtime`  
*[_git/dotnet-runtime@d839c41](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCd839c41c85988aadc213e8e42269ecd7883a1790)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@623d5a2](https://github.com/dotnet/scenario-tests/tree/623d5a21e18d6d1f370b5b9d1ebaf8d687464c0b)*
- `src/sdk`  
*[dotnet/sdk@1935747](https://github.com/dotnet/sdk/tree/193574712694608533932d11010a65bc9c77945d)*
- `src/source-build-assets`  
*[dotnet/source-build-assets@898cae0](https://github.com/dotnet/source-build-assets/tree/898cae04468e9be69ab78fcc27bb70d676ae16b6)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@1c80016](https://github.com/dotnet/source-build-externals/tree/1c800169e674c7becf88434a0d2817453cf91dd7)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@ee19952](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/ee19952f21d5c170a778bdca901b67076a1cd8f8)*
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
*[dotnet/source-build-reference-packages@fe3794a](https://github.com/dotnet/source-build-reference-packages/tree/fe3794a68bd668d36d4d5014a9e6c9d22c0e6d86)*
- `src/sourcelink`  
*[dotnet/sourcelink@657ade4](https://github.com/dotnet/sourcelink/tree/657ade4711e607cc4759e89e0943aa1ca8aadc63)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@f5f6eb5](https://github.com/dotnet/templating/tree/f5f6eb5420398b365107b9be9e5cb6290626e39f)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@889bd3c](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GC889bd3cf7e1c61b6bdee233190da4931118b6e6d)*
- `src/winforms`  
*[_git/dotnet-winforms@fa8da0f](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GCfa8da0fffee2a42efa08aac643276cd0c482c49c)*
- `src/wpf`  
*[_git/dotnet-wpf@15c7bd7](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GC15c7bd7f92ae827be47bfb0ca16660f8cd376189)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
