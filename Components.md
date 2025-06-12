# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@ac63dcf](https://github.com/dotnet/arcade/tree/ac63dcf2791c32bf2102fd2b5ebb46479bd48a83)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@379bfc7](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC379bfc7b2559e7cc9f42f997a497b2f2dd8e12d2)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@04ee1b4](https://github.com/google/googletest/tree/04ee1b4f2aefdffb0135d7cf2a2c519fe50dabe4)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@bbb895e](https://github.com/dotnet/cecil/tree/bbb895e8e9f2d566eae04f09977b8c5f895057d2)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b2d5c0c](https://github.com/dotnet/deployment-tools/tree/b2d5c0c5841de4bc036ef4c84b5db3532504e5f3)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[_git/dotnet-efcore@8751e6d](https://dev.azure.com/dnceng/internal/_git/dotnet-efcore/?version=GC8751e6d519fda94d5154187358765311ed4a4e84)*
- `src/emsdk`  
*[dotnet/emsdk@b567cdb](https://github.com/dotnet/emsdk/tree/b567cdb6b8b461de79f2a2536a22ca3a67f2f33e)*
- `src/fsharp`  
*[dotnet/fsharp@47d4e3f](https://github.com/dotnet/fsharp/tree/47d4e3f91e4e5414b6dafbf14288b9c5a798ef99)*
- `src/msbuild`  
*[_git/DotNet-msbuild-Trusted@13c590f](https://dev.azure.com/devdiv/DevDiv/_git/DotNet-msbuild-Trusted/?version=GC13c590f8d138d6a30b899a5172cc6ec494db930b)*
- `src/nuget-client`  
*[nuget/nuget.client@95a470a](https://github.com/nuget/nuget.client/tree/95a470a557091cdbdc9f68a178b60bd19329942c)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@44568f7](https://github.com/dotnet/razor/tree/44568f7817cd7c6daf90c9340c64c3b69277b7f6)*
- `src/roslyn`  
*[dotnet/roslyn@3f5cf9f](https://github.com/dotnet/roslyn/tree/3f5cf9fbbd91f2047e988801a5142ca1cb6bab45)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@16865ea](https://github.com/dotnet/roslyn-analyzers/tree/16865ea61910500f1022ad2b96c499e5df02c228)*
- `src/runtime`  
*[_git/dotnet-runtime@1bafa97](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC1bafa9761541b20cded99398ce1a8630d09f43fd)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@45b8786](https://github.com/dotnet/scenario-tests/tree/45b878678cfbde6c460450ed1699de75a20d6e87)*
- `src/sdk`  
*[dotnet/sdk@59bc292](https://github.com/dotnet/sdk/tree/59bc2927ba20a32a2cbdf9643fba57a46d81aa13)*
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
*[dotnet/source-build-reference-packages@19eb5ea](https://github.com/dotnet/source-build-reference-packages/tree/19eb5ea4e5f9c4e5256843a92805c8c9e942207d)*
- `src/sourcelink`  
*[dotnet/sourcelink@4e17620](https://github.com/dotnet/sourcelink/tree/4e176206614b345352885b55491aeb51bf77526b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@0c96b36](https://github.com/dotnet/templating/tree/0c96b36ccec4428bbfdeafae392bda49c1667361)*
- `src/test-templates`  
*[dotnet/test-templates@47c90e1](https://github.com/dotnet/test-templates/tree/47c90e140b027225b799ca8413af10ee3d5f1126)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[_git/dotnet-windowsdesktop@c7cc0b3](https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop/?version=GCc7cc0b3d8bd0ee206eb06ba1ad7e201218fc3d32)*
- `src/winforms`  
*[_git/dotnet-winforms@f0fc61e](https://dev.azure.com/dnceng/internal/_git/dotnet-winforms/?version=GCf0fc61ead7dc20cbce05eb81a42089e28c90c409)*
- `src/wpf`  
*[_git/dotnet-wpf@620a203](https://dev.azure.com/dnceng/internal/_git/dotnet-wpf/?version=GC620a20376852ae35038cd4251552ea8425f6cfe6)*
- `src/xdt`  
*[dotnet/xdt@63ae811](https://github.com/dotnet/xdt/tree/63ae81154c50a1cf9287cc47d8351d55b4289e6d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
