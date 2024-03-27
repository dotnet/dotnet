# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@b6fada3](https://github.com/dotnet/arcade/tree/b6fada3ec4fa37e08dcbafaa6ddf59213f3f8687)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@63c8031](https://github.com/dotnet/aspnetcore/tree/63c8031b6a6af5009b3c5bb4291fcc4c32b06b10)*
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
*[dotnet/deployment-tools@596a11e](https://github.com/dotnet/deployment-tools/tree/596a11e185d2274531fe57a2c9e03bbe4f23064d)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/emsdk`  
*[dotnet/emsdk@e4c6eab](https://github.com/dotnet/emsdk/tree/e4c6eabba09c9d8c4d1bfb40511cb9d494fb0626)*
- `src/fsharp`  
*[dotnet/fsharp@4a39419](https://github.com/dotnet/fsharp/tree/4a394198efadc455334ae272954ece372aea4de2)*
- `src/installer`  
*[dotnet/installer@afab34f](https://github.com/dotnet/installer/tree/afab34fd84b5bdbaa605548068caf86b231d1e5c)*
- `src/msbuild`  
*[dotnet/msbuild@bf82a13](https://github.com/dotnet/msbuild/tree/bf82a1313390df2692c23206e0846b205398188b)*
- `src/nuget-client`  
*[nuget/nuget.client@fb50d1a](https://github.com/nuget/nuget.client/tree/fb50d1a45ed10b39b5f335bc3a4bdcaea9b951cf)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@e832b1d](https://github.com/dotnet/razor/tree/e832b1d125c2d73f7ea7e17b81c0cb4a622257a9)*
- `src/roslyn`  
*[dotnet/roslyn@919d4db](https://github.com/dotnet/roslyn/tree/919d4dbfb0dffb35a702417e28ceea652d248bc6)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@e110f09](https://github.com/dotnet/roslyn-analyzers/tree/e110f0980519ea7b4cb7d50cb4dd030e83721bab)*
- `src/runtime`  
*[dotnet/runtime@c46410f](https://github.com/dotnet/runtime/tree/c46410f099b6358461d2de89f81393c91d2b40f7)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@ddccfec](https://github.com/dotnet/scenario-tests/tree/ddccfec3ccd631fb8341c8b6e4e422e8cb339aa5)*
- `src/sdk`  
*[dotnet/sdk@f4c1181](https://github.com/dotnet/sdk/tree/f4c11814251c66b510761dee510b9b16c956fe50)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@52d6569](https://github.com/dotnet/source-build-externals/tree/52d6569d44f86b5d442017f4a9eb3cda4c766afb)*
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
*[dotnet/sourcelink@0a7443b](https://github.com/dotnet/sourcelink/tree/0a7443b1ac4d9156e234d699d7c7604d9b91c58f)*
- `src/symreader`  
*[dotnet/symreader@01de94d](https://github.com/dotnet/symreader/tree/01de94d9718fd48c511cae276437edcd41b41fa4)*
- `src/templating`  
*[dotnet/templating@349df83](https://github.com/dotnet/templating/tree/349df83059993bc5e97bead5ab3ef476f1583040)*
- `src/test-templates`  
*[dotnet/test-templates@3bda3d3](https://github.com/dotnet/test-templates/tree/3bda3d335edaf10ef6c5bc93c06c8c57db5ee9c4)*
- `src/vstest`  
*[microsoft/vstest@6957756](https://github.com/microsoft/vstest/tree/6957756d70d6ade74e239a38ad709db5cb39fe0d)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@e9216a8](https://github.com/dotnet/windowsdesktop/tree/e9216a8057d1837fc2344fa36703ae8f9b49cc24)*
- `src/winforms`  
*[dotnet/winforms@9c9b2e0](https://github.com/dotnet/winforms/tree/9c9b2e06a998f65638053fdd32cbd65c4cd1a64f)*
- `src/wpf`  
*[dotnet/wpf@9ef9cfb](https://github.com/dotnet/wpf/tree/9ef9cfb01e493f617202fd47a09c3403327933a4)*
- `src/xdt`  
*[dotnet/xdt@ea48469](https://github.com/dotnet/xdt/tree/ea48469ba1215ec764ceba3df6f0b13b465c68ad)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
