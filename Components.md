# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@8cc6ecd](https://github.com/dotnet/arcade/tree/8cc6ecd76c24ef6665579a5c5e386a211a1e7c54)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@4442a18](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC4442a188f9200a57635373dcd640893c0e8dcc78)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@d144031](https://github.com/google/googletest/tree/d144031940543e15423a25ae5a8a74141044862f)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@72791f7](https://github.com/dotnet/cecil/tree/72791f7604750fb8af9249b94318547c5f1d6683)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@480480b](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC480480b57cd6e43fe5cab1b552ac0ef917bf3fe8)*
- `src/emsdk`  
*[dotnet/emsdk@4c9d1b1](https://github.com/dotnet/emsdk/tree/4c9d1b112c16716c2479e054e9ad4db8b5b8c70c)*
- `src/fsharp`  
*[dotnet/fsharp@fd29258](https://github.com/dotnet/fsharp/tree/fd29258f2eb7502b09a450a8250495adb6c5caee)*
- `src/msbuild`  
*[dotnet/msbuild@f7f4b08](https://github.com/dotnet/msbuild/tree/f7f4b086d628f5e6db8c660a6ea9d6c397386d1a)*
- `src/nuget-client`  
*[nuget/nuget.client@c097388](https://github.com/nuget/nuget.client/tree/c097388782da305f47c395f90bea2d7c83909b6d)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@e0c5e58](https://github.com/dotnet/razor/tree/e0c5e5892684a5ce8f4fa0dcc504a0af4c140ac7)*
- `src/roslyn`  
*[dotnet/roslyn@da7c6c4](https://github.com/dotnet/roslyn/tree/da7c6c4257b2f661024b9a506773372a09023eee)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5bfaf6a](https://github.com/dotnet/roslyn-analyzers/tree/5bfaf6aea5cf9d1c924d9adc69916eac3be07880)*
- `src/runtime`  
*[_git/dotnet-runtime@c8acea2](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCc8acea22626efab11c13778c028975acdc34678f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@1512fd8](https://github.com/dotnet/scenario-tests/tree/1512fd86b0eb245fe6d8efd7e833c37f5e290803)*
- `src/sdk`  
*[dotnet/sdk@f24cfbc](https://github.com/dotnet/sdk/tree/f24cfbc3c6edc6166fe8592448ecb47d84b04358)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@8847644](https://github.com/dotnet/source-build-externals/tree/884764492bf2cbc8d38037d9eee84f16960daa74)*
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
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/vs-solutionpersistence`  
    *[microsoft/vs-solutionpersistence@0b6f82a](https://github.com/microsoft/vs-solutionpersistence/tree/0b6f82a4073ce0ff0419991ea0cd6dd6898a51ac)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@f5fa796](https://github.com/dotnet/source-build-reference-packages/tree/f5fa796273e4e59926e3fab26e1ab9e7d577f5e5)*
- `src/sourcelink`  
*[dotnet/sourcelink@4e17620](https://github.com/dotnet/sourcelink/tree/4e176206614b345352885b55491aeb51bf77526b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@26067ca](https://github.com/dotnet/templating/tree/26067cac0b57cb1dde139fade47e8f8f444ad3a9)*
- `src/test-templates`  
*[dotnet/test-templates@8d99bca](https://github.com/dotnet/test-templates/tree/8d99bca98e3fd0c8f4e10eb200aed20d9665de0e)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@af254ce](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GCaf254ce17dd6827439a1e51006ddf804a0093a13)*
- `src/winforms`  
*[_git/dotnet-winforms@5a82e6f](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GC5a82e6f3c764285d3e2c5307e46fd5cbd546c7cd)*
- `src/wpf`  
*[_git/dotnet-wpf@f26d48c](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GCf26d48cd826cfd41bed05708d1cdc9e219bf8514)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
