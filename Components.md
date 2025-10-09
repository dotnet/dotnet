﻿# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@f8c9a6d](https://github.com/dotnet/arcade/tree/f8c9a6d12e5a3b281661924da22d7de1cc6ab27d)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@ff66c26](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GCff66c263be7ed395794bdaf616322977b8ec897c)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@373af2e](https://github.com/google/googletest/tree/373af2e3df71599b87a40ce0e37164523849166b)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@788a8a7](https://github.com/dotnet/cecil/tree/788a8a7481c01a7d235110cdea2ca5bfb34210d4)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@78871c8](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC78871c83aac6c38eb5476c2f34aae98ef65314f5)*
- `src/emsdk`  
*[dotnet/emsdk@dc8e347](https://github.com/dotnet/emsdk/tree/dc8e3478c4aa5f6a103329333c2bdbcd07a07741)*
- `src/fsharp`  
*[dotnet/fsharp@47d4e3f](https://github.com/dotnet/fsharp/tree/47d4e3f91e4e5414b6dafbf14288b9c5a798ef99)*
- `src/msbuild`  
*[dotnet/msbuild@6be9954](https://github.com/dotnet/msbuild/tree/6be995463a50d9dd400ec9ee1218f82f5a7cc209)*
- `src/nuget-client`  
*[nuget/nuget.client@42bfb45](https://github.com/nuget/nuget.client/tree/42bfb4554167e1d2fc2b950728d9bd8164f806c1)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@d57c6d7](https://github.com/dotnet/razor/tree/d57c6d7b68578729e95c608105075c5759969f05)*
- `src/roslyn`  
*[dotnet/roslyn@dcf9af2](https://github.com/dotnet/roslyn/tree/dcf9af2b54e4469b94697b1b92d31354ce698f66)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@16865ea](https://github.com/dotnet/roslyn-analyzers/tree/16865ea61910500f1022ad2b96c499e5df02c228)*
- `src/runtime`  
*[_git/dotnet-runtime@893c2eb](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC893c2ebbd49952ca49e93298148af2d95a61a0a4)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@0898abb](https://github.com/dotnet/scenario-tests/tree/0898abbb5899ef400b8372913c2320295798a687)*
- `src/sdk`  
*[dotnet/sdk@fd333af](https://github.com/dotnet/sdk/tree/fd333afa3daa8929d934e62ee66070fb581f19fc)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@71dbdcc](https://github.com/dotnet/source-build-externals/tree/71dbdccd13f28cfd1a35649263b55ebbeab26ee7)*
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
*[dotnet/source-build-reference-packages@5511a69](https://github.com/dotnet/source-build-reference-packages/tree/5511a69718e9d581386105705152592cee268e37)*
- `src/sourcelink`  
*[dotnet/sourcelink@4e17620](https://github.com/dotnet/sourcelink/tree/4e176206614b345352885b55491aeb51bf77526b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@79d6a45](https://github.com/dotnet/templating/tree/79d6a45326074dbdc78f2ba288502618a09f893c)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@36322cf](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GC36322cf85c30a14acb5da40397df03216c44bced)*
- `src/winforms`  
*[_git/dotnet-winforms@9dc841c](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GC9dc841c74ab937e3a19c3d81590136067b3a15ed)*
- `src/wpf`  
*[_git/dotnet-wpf@5617669](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GC56176690719670a63582850619ed7d33e13083e3)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
