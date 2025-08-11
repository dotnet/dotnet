# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e298236](https://github.com/dotnet/arcade/tree/e29823691315ed6b3acff20d5bdf3b0be7628283)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@215a587](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC215a587e52efa710de84138b0a3374b860b924d8)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@c67de11](https://github.com/google/googletest/tree/c67de117379f4d1c889c7581a0a76aa0979c2083)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@862b4c8](https://github.com/dotnet/cecil/tree/862b4c8bf05585cc44ceb32dc0fd060ceed06cd2)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@3f7d40e](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC3f7d40ec7be104358780955b3f0fea62495264dc)*
- `src/emsdk`  
*[dotnet/emsdk@0bcc3e6](https://github.com/dotnet/emsdk/tree/0bcc3e67026ea44a16fb018a50e4e134c06ab3d6)*
- `src/fsharp`  
*[dotnet/fsharp@47d4e3f](https://github.com/dotnet/fsharp/tree/47d4e3f91e4e5414b6dafbf14288b9c5a798ef99)*
- `src/msbuild`  
*[dotnet/msbuild@481b855](https://github.com/dotnet/msbuild/tree/481b855f7315f967f9c14b83d4391ae05b7be1af)*
- `src/nuget-client`  
*[nuget/nuget.client@42bfb45](https://github.com/nuget/nuget.client/tree/42bfb4554167e1d2fc2b950728d9bd8164f806c1)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@44568f7](https://github.com/dotnet/razor/tree/44568f7817cd7c6daf90c9340c64c3b69277b7f6)*
- `src/roslyn`  
*[dotnet/roslyn@4d68ef5](https://github.com/dotnet/roslyn/tree/4d68ef53bf037528cbe2066696b872aaa8b4fe36)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@16865ea](https://github.com/dotnet/roslyn-analyzers/tree/16865ea61910500f1022ad2b96c499e5df02c228)*
- `src/runtime`  
*[_git/dotnet-runtime@aae90fa](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GCaae90fa09086a9be09dac83fa66542232c7269d8)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@0898abb](https://github.com/dotnet/scenario-tests/tree/0898abbb5899ef400b8372913c2320295798a687)*
- `src/sdk`  
*[dotnet/sdk@6454c42](https://github.com/dotnet/sdk/tree/6454c42af8c425bd9e1efc0e24e3f0f759ca5b03)*
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
*[dotnet/source-build-reference-packages@cbafdd7](https://github.com/dotnet/source-build-reference-packages/tree/cbafdd754f3cd645081e32d30cb6a82300163288)*
- `src/sourcelink`  
*[dotnet/sourcelink@4e17620](https://github.com/dotnet/sourcelink/tree/4e176206614b345352885b55491aeb51bf77526b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@2a6351f](https://github.com/dotnet/templating/tree/2a6351f11c4a527a0e095c5ed749f56e75efc8e9)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@1d6033e](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GC1d6033e36c17c6c496302306563689885e416f48)*
- `src/winforms`  
*[_git/dotnet-winforms@dfa9922](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GCdfa9922eb72e517011ad36ed7318242bbe9f9005)*
- `src/wpf`  
*[_git/dotnet-wpf@c83d78f](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GCc83d78f58cbe40d4f5cd847461a233e5c85f5300)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
