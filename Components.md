# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@f3bffca](https://github.com/dotnet/arcade/tree/f3bffca1f93573c88a7f9b79fac258dff76f6215)*
- `src/aspire`  
*[microsoft/aspire@5fa9337](https://github.com/microsoft/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@209bf5c](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC209bf5cfd6f0b329c31faa125790e1d1d820cdb5)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@3064a60](https://github.com/google/googletest/tree/3064a609c7a86ac9fb601a9cc5dea6ddb45ba49d)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@365965f](https://github.com/aspnet/MessagePack-CSharp/tree/365965f0d8c13c40ff8fde25882066b90f569c7a)*
- `src/cecil`  
*[dotnet/cecil@d98c55d](https://github.com/dotnet/cecil/tree/d98c55d1363411ee1a0ff0c76c6e5d798e8147ec)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@d45c648](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GCd45c6483aa13a1c4ecc6e9da427ce6826ffb45a5)*
- `src/emsdk`  
*[dotnet/emsdk@dd37900](https://github.com/dotnet/emsdk/tree/dd379007ea0e38b82e87ca5fd61bb43b432eaf7a)*
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
*[_git/dotnet-runtime@8381bdb](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC8381bdb01fe4a26e1f61370e779c78bb73ecc95b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@83d5019](https://github.com/dotnet/scenario-tests/tree/83d50199a618f84d9cb5d442dcc70c46023d1108)*
- `src/sdk`  
*[dotnet/sdk@fca77af](https://github.com/dotnet/sdk/tree/fca77afbd7a2555a57695646ee3ef85eb26b96c7)*
- `src/source-build-assets`  
*[dotnet/source-build-assets@eccb082](https://github.com/dotnet/source-build-assets/tree/eccb0828682858d8daa029ba29adce07fd70d9a7)*
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
*[_git/dotnet-sourcelink@1e7bb03](https://dev.azure.com/dnceng/internal/_git/dotnet-sourcelink/?version=GC1e7bb033fe99a4a968e44c38e64e903dd58a5430)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@d6bb682](https://github.com/dotnet/templating/tree/d6bb682832461366b9fe50dee7a72c7feeadbc41)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@4a2048b](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GC4a2048b77c7de671b08320e346c73da6fd6d9aeb)*
- `src/winforms`  
*[_git/dotnet-winforms@96d636b](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GC96d636b0e1c0e7b43433f6c5f4920c61e448955a)*
- `src/wpf`  
*[_git/dotnet-wpf@75bc144](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GC75bc144dfb125fb821e28adb648dd35618b23693)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
