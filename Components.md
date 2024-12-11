# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@b41381d](https://github.com/dotnet/arcade/tree/b41381d5cd633471265e9cd72e933a7048e03062)*
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
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@471bcae](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC471bcaed1ece1855cce69d3c929ec55e52bd4274)*
- `src/emsdk`  
*[dotnet/emsdk@4c9d1b1](https://github.com/dotnet/emsdk/tree/4c9d1b112c16716c2479e054e9ad4db8b5b8c70c)*
- `src/fsharp`  
*[dotnet/fsharp@fd29258](https://github.com/dotnet/fsharp/tree/fd29258f2eb7502b09a450a8250495adb6c5caee)*
- `src/msbuild`  
*[dotnet/msbuild@ed8c6ae](https://github.com/dotnet/msbuild/tree/ed8c6aec5b774cfdad4e95033910c30aa0d93391)*
- `src/nuget-client`  
*[nuget/nuget.client@c097388](https://github.com/nuget/nuget.client/tree/c097388782da305f47c395f90bea2d7c83909b6d)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@b5256ab](https://github.com/dotnet/razor/tree/b5256abf658352440e61aa1372bbc0798e9ceb7d)*
- `src/roslyn`  
*[dotnet/roslyn@dfa7fc6](https://github.com/dotnet/roslyn/tree/dfa7fc6bdea31a858a402168384192b633c811fa)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5bfaf6a](https://github.com/dotnet/roslyn-analyzers/tree/5bfaf6aea5cf9d1c924d9adc69916eac3be07880)*
- `src/runtime`  
*[_git/dotnet-runtime@c8acea2](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCc8acea22626efab11c13778c028975acdc34678f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@1009e3b](https://github.com/dotnet/scenario-tests/tree/1009e3b6d23e049de56b91de82fe975fe84444f8)*
- `src/sdk`  
*[dotnet/sdk@cb83cd4](https://github.com/dotnet/sdk/tree/cb83cd4923f0973ae11dfb5602737493bf165210)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@ab46960](https://github.com/dotnet/source-build-externals/tree/ab469606a3e6b026dcac301e2dab96117c94faeb)*
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
*[dotnet/source-build-reference-packages@c81acaa](https://github.com/dotnet/source-build-reference-packages/tree/c81acaa80719d0ecfadfe41e3c0e3548bdc4e78d)*
- `src/sourcelink`  
*[dotnet/sourcelink@599ca6d](https://github.com/dotnet/sourcelink/tree/599ca6dd9547cef0c10c1363fade40b044fed3b7)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@b1003b4](https://github.com/dotnet/templating/tree/b1003b44bf4f77613101b9d31a55bb307515bb6d)*
- `src/test-templates`  
*[dotnet/test-templates@22f4800](https://github.com/dotnet/test-templates/tree/22f4800ed2fe5dbca64c982aa15fd6838016328e)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@af254ce](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GCaf254ce17dd6827439a1e51006ddf804a0093a13)*
- `src/winforms`  
*[_git/dotnet-winforms@5a82e6f](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GC5a82e6f3c764285d3e2c5307e46fd5cbd546c7cd)*
- `src/wpf`  
*[_git/dotnet-wpf@f26d48c](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GCf26d48cd826cfd41bed05708d1cdc9e219bf8514)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
