# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@39839f3](https://github.com/dotnet/arcade/tree/39839f3007d9f3bbabf7a4b6a96ef5dd6be9e5ac)*
- `src/aspire`  
*[dotnet/aspire@1194762](https://github.com/dotnet/aspire/tree/11947620e257657946e4232085d8db8e2aa4a36e)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@1ee58ec](https://github.com/dotnet/aspnetcore/tree/1ee58ec0d8837ac310c9ef9e784a35bf3027dbd5)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@e1a38bc](https://github.com/google/googletest/tree/e1a38bc3707741d249fa22d2064552a08e37555b)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@ba53c75](https://github.com/dotnet/cecil/tree/ba53c75483aa4980a332fa48e61076f80adfec40)*
- `src/command-line-api`  
*[dotnet/command-line-api@5ea97af](https://github.com/dotnet/command-line-api/tree/5ea97af07263ea3ef68a18557c8aa3f7e3200bda)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@822ff26](https://github.com/dotnet/deployment-tools/tree/822ff266c5f999ab9ceb6928df59d79285ea4a4f)*
- `src/diagnostics`  
*[dotnet/diagnostics@5eb514a](https://github.com/dotnet/diagnostics/tree/5eb514a41f900ac1aa1e9a3e12b2931dcb064069)*
- `src/emsdk`  
*[dotnet/emsdk@a5f4de7](https://github.com/dotnet/emsdk/tree/a5f4de78fca42544771977f8e8e04c4aa83e1d02)*
- `src/format`  
*[dotnet/format@91f6031](https://github.com/dotnet/format/tree/91f60316ebd9c75d6be8b7f9b7c201bab17240c9)*
- `src/fsharp`  
*[dotnet/fsharp@212eaf7](https://github.com/dotnet/fsharp/tree/212eaf7fac2d837c51dc49e477a599ebea68338b)*
- `src/installer`  
*[dotnet/installer@f51a2d9](https://github.com/dotnet/installer/tree/f51a2d95a57550e190a9cd87d33f3337da2e7370)*
- `src/msbuild`  
*[dotnet/msbuild@276f781](https://github.com/dotnet/msbuild/tree/276f781fa868f553bed33a0a01fc54108c84c672)*
- `src/nuget-client`  
*[nuget/nuget.client@2fdd0d4](https://github.com/nuget/nuget.client/tree/2fdd0d41e33c3354de2750fe154b56751a6682aa)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@6c04d30](https://github.com/dotnet/razor/tree/6c04d3094f791ba68c30ae0db8aca39e2cf7c155)*
- `src/roslyn`  
*[dotnet/roslyn@c3565da](https://github.com/dotnet/roslyn/tree/c3565da812d99adf841cb96a764a27d8a93e22ef)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@d98dd32](https://github.com/dotnet/roslyn-analyzers/tree/d98dd32d7cd4274bea98b147032b73a4eb051f2c)*
- `src/runtime`  
*[dotnet/runtime@efa2b78](https://github.com/dotnet/runtime/tree/efa2b78175388a656893d0e9becc408d99afe445)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@9df94f0](https://github.com/dotnet/sdk/tree/9df94f0bab218a74fa3746da91f19949a7c74eea)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@472629e](https://github.com/dotnet/source-build-externals/tree/472629e451a5a87410ea3670606f7235a4dd5a02)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights-2.21.0`  
    *[microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/application-insights-2.22.0`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a607fa5](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/a607fa5e0005a6178cf1d2fed4fa0f8179cdb186)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.0`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.1`  
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
*[dotnet/source-build-reference-packages@c0b5d69](https://github.com/dotnet/source-build-reference-packages/tree/c0b5d69a1a1513528c77fffff708c7502d57c35c)*
- `src/sourcelink`  
*[dotnet/sourcelink@4af34e7](https://github.com/dotnet/sourcelink/tree/4af34e7dc1755e8a26fcbde95073895e279da09d)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@d09013f](https://github.com/dotnet/templating/tree/d09013f6c8c17e4c124869f467dada469829911e)*
- `src/test-templates`  
*[dotnet/test-templates@785fafc](https://github.com/dotnet/test-templates/tree/785fafc79e66b509ccc116867b84c21e494930b8)*
- `src/vstest`  
*[microsoft/vstest@3161673](https://github.com/microsoft/vstest/tree/316167369cea59e0ad6ece2a39d94a3a6d49cf12)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@44e12ec](https://github.com/dotnet/windowsdesktop/tree/44e12ece1bfc7800e8ec69f422eb60062de8b00e)*
- `src/winforms`  
*[dotnet/winforms@bfc23d4](https://github.com/dotnet/winforms/tree/bfc23d4c7b17ab3ece28faedd603501e2ada9d6c)*
- `src/wpf`  
*[dotnet/wpf@09e859d](https://github.com/dotnet/wpf/tree/09e859df29f1f8775155b65f39b3bbf9c27e0a02)*
- `src/xdt`  
*[dotnet/xdt@a86209b](https://github.com/dotnet/xdt/tree/a86209be9faeafe337633ab1746df7bc19a83538)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
