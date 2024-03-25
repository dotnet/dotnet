# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@c936d1b](https://github.com/dotnet/arcade/tree/c936d1bc358744730613d8ce54bc3e0294e5ea56)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@3f99d45](https://github.com/dotnet/aspnetcore/tree/3f99d45b0b7d8f0427a3d98acc63098694613362)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@eff443c](https://github.com/google/googletest/tree/eff443c6ef5eb6ab598bfaae27f9427fdb4f6af7)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@9c8ea96](https://github.com/dotnet/cecil/tree/9c8ea966df62f764523b51772763e74e71040a92)*
- `src/command-line-api`  
*[dotnet/command-line-api@5ea97af](https://github.com/dotnet/command-line-api/tree/5ea97af07263ea3ef68a18557c8aa3f7e3200bda)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@2a7741c](https://github.com/dotnet/deployment-tools/tree/2a7741c5a7cb49fbad797c4b2f7812d5620430ac)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/emsdk`  
*[dotnet/emsdk@5dd0620](https://github.com/dotnet/emsdk/tree/5dd0620274178dd73cac5049e5187c00e07ecf0c)*
- `src/fsharp`  
*[dotnet/fsharp@8d852e4](https://github.com/dotnet/fsharp/tree/8d852e43d35fdac96b1ba52e3bd4b35350035914)*
- `src/installer`  
*[dotnet/installer@987138b](https://github.com/dotnet/installer/tree/987138bf265c71429713475c27c988b994205046)*
- `src/msbuild`  
*[dotnet/msbuild@de77617](https://github.com/dotnet/msbuild/tree/de776177f6d540e656e6b0c6d5bb07f2ff518c19)*
- `src/nuget-client`  
*[nuget/nuget.client@1845d6b](https://github.com/nuget/nuget.client/tree/1845d6bd450a7453d573035371c9fec43683d1ef)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@9a645a8](https://github.com/dotnet/razor/tree/9a645a8f4ae7d7e48522f6dac442897b9a67690a)*
- `src/roslyn`  
*[dotnet/roslyn@b2a0a98](https://github.com/dotnet/roslyn/tree/b2a0a985202805db6160e6c070ac828e9e3a8094)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@29bdbf5](https://github.com/dotnet/roslyn-analyzers/tree/29bdbf5df540dc13d4fe440a1ca7076c6ed65864)*
- `src/runtime`  
*[dotnet/runtime@d8c3db7](https://github.com/dotnet/runtime/tree/d8c3db7d096da710a3f667d70c36e34c1bf63921)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@219a6fc](https://github.com/dotnet/sdk/tree/219a6fc9954d632d7c119b31d59ff1516ff04d98)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@b46b7e6](https://github.com/dotnet/source-build-externals/tree/b46b7e6859f4094cd7f3e00dc0471d62f5d8d051)*
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
*[dotnet/sourcelink@e57a410](https://github.com/dotnet/sourcelink/tree/e57a410525da38cf4f8012246b3b89117ecbd5d4)*
- `src/symreader`  
*[dotnet/symreader@0bb6063](https://github.com/dotnet/symreader/tree/0bb6063675fd0c8c60b6aa8f9e67cc15c81979d2)*
- `src/templating`  
*[dotnet/templating@93ceb4c](https://github.com/dotnet/templating/tree/93ceb4c974a00962833569f9849d0f022c7b8f1c)*
- `src/test-templates`  
*[dotnet/test-templates@3bda3d3](https://github.com/dotnet/test-templates/tree/3bda3d335edaf10ef6c5bc93c06c8c57db5ee9c4)*
- `src/vstest`  
*[microsoft/vstest@6957756](https://github.com/microsoft/vstest/tree/6957756d70d6ade74e239a38ad709db5cb39fe0d)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@6153872](https://github.com/dotnet/windowsdesktop/tree/6153872f19b1cd05b26c92d8080e56c1892ba55c)*
- `src/winforms`  
*[dotnet/winforms@e59c8ab](https://github.com/dotnet/winforms/tree/e59c8abcf6e29b8b99a2efefec4133b1f94559af)*
- `src/wpf`  
*[dotnet/wpf@6ffdf5c](https://github.com/dotnet/wpf/tree/6ffdf5ca7bdc28496c8b5533277705fba66483f4)*
- `src/xdt`  
*[dotnet/xdt@ea48469](https://github.com/dotnet/xdt/tree/ea48469ba1215ec764ceba3df6f0b13b465c68ad)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
