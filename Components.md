# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@0d52a8b](https://github.com/dotnet/arcade/tree/0d52a8b262d35fa2fde84e398cb2e791b8454bd2)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@ed74665](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GCed74665e773dd1ebea3289c5662d71c590305932)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@52204f7](https://github.com/google/googletest/tree/52204f78f94d7512df1f0f3bea1d47437a2c3a58)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@a833626](https://github.com/dotnet/cecil/tree/a8336269316c42f8164fe7bf45972dd8a81e52dc)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@6765359](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC6765359588e8b38bab2a7974db9398432703828f)*
- `src/emsdk`  
*[dotnet/emsdk@78f6f07](https://github.com/dotnet/emsdk/tree/78f6f07d38e8755e573039a8aa04e131d3e59b76)*
- `src/fsharp`  
*[dotnet/fsharp@47d4e3f](https://github.com/dotnet/fsharp/tree/47d4e3f91e4e5414b6dafbf14288b9c5a798ef99)*
- `src/msbuild`  
*[dotnet/msbuild@6c6276d](https://github.com/dotnet/msbuild/tree/6c6276d5998f8be9811497003566e5924141d600)*
- `src/nuget-client`  
*[nuget/nuget.client@42bfb45](https://github.com/nuget/nuget.client/tree/42bfb4554167e1d2fc2b950728d9bd8164f806c1)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@44568f7](https://github.com/dotnet/razor/tree/44568f7817cd7c6daf90c9340c64c3b69277b7f6)*
- `src/roslyn`  
*[dotnet/roslyn@3f5cf9f](https://github.com/dotnet/roslyn/tree/3f5cf9fbbd91f2047e988801a5142ca1cb6bab45)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@16865ea](https://github.com/dotnet/roslyn-analyzers/tree/16865ea61910500f1022ad2b96c499e5df02c228)*
- `src/runtime`  
*[_git/dotnet-runtime@e36e4d1](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCe36e4d1a8f8dfb08d7e3a6041459c9791d732c01)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@a11e0d6](https://github.com/dotnet/scenario-tests/tree/a11e0d6f227feb7d0e2b619b48556d041237e59c)*
- `src/sdk`  
*[dotnet/sdk@7a186ee](https://github.com/dotnet/sdk/tree/7a186eeaf06ad0cbdefe24a35a0898fdf77f4f01)*
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
*[dotnet/source-build-reference-packages@9859d82](https://github.com/dotnet/source-build-reference-packages/tree/9859d82ffce48f49b5e93fa46a38bdddc4ba26be)*
- `src/sourcelink`  
*[dotnet/sourcelink@4e17620](https://github.com/dotnet/sourcelink/tree/4e176206614b345352885b55491aeb51bf77526b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@a2da3a8](https://github.com/dotnet/templating/tree/a2da3a8446409db003c2868a6910c0323b113467)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@b141ff4](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GCb141ff423da53a66cda0be31dad3059317be52f6)*
- `src/winforms`  
*[_git/dotnet-winforms@711ee9f](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GC711ee9fe3398d64b89f2ae70b94c66d9d495cf72)*
- `src/wpf`  
*[_git/dotnet-wpf@6df813d](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GC6df813d45ebffd2739980b7f372767ac5fd68e2a)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
