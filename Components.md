# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@532f956](https://github.com/dotnet/arcade/tree/532f956a119bce77ca279994054d08dbc24418f7)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@76a4e75](https://github.com/dotnet/aspnetcore/tree/76a4e7507352de5cbabfd8d0130b69012442fe3f)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@eff443c](https://github.com/google/googletest/tree/eff443c6ef5eb6ab598bfaae27f9427fdb4f6af7)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@9c8ea96](https://github.com/dotnet/cecil/tree/9c8ea966df62f764523b51772763e74e71040a92)*
- `src/command-line-api`  
*[dotnet/command-line-api@c96672b](https://github.com/dotnet/command-line-api/tree/c96672b8b84c307feb035fed6cbe9db85d5b87d3)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@596a11e](https://github.com/dotnet/deployment-tools/tree/596a11e185d2274531fe57a2c9e03bbe4f23064d)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/emsdk`  
*[dotnet/emsdk@bd79d3d](https://github.com/dotnet/emsdk/tree/bd79d3dd7ed2db36b3c3d4fa807c21a06d2024ec)*
- `src/fsharp`  
*[dotnet/fsharp@31fccb7](https://github.com/dotnet/fsharp/tree/31fccb72266fabbf4f82208fca4e0a5c1b687406)*
- `src/installer`  
*[dotnet/installer@b47f240](https://github.com/dotnet/installer/tree/b47f240aaa533ed91af636c59370902bd4564967)*
- `src/msbuild`  
*[dotnet/msbuild@7ca3c98](https://github.com/dotnet/msbuild/tree/7ca3c98fad986066bbf2802c863236b4a0f4e34a)*
- `src/nuget-client`  
*[nuget/nuget.client@fb50d1a](https://github.com/nuget/nuget.client/tree/fb50d1a45ed10b39b5f335bc3a4bdcaea9b951cf)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@c46ec54](https://github.com/dotnet/razor/tree/c46ec54964765f6a924276924265fb0e241812ed)*
- `src/roslyn`  
*[dotnet/roslyn@0288056](https://github.com/dotnet/roslyn/tree/0288056413b6de0d0941b0b1ed770c273d870fb0)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@e4d7ea6](https://github.com/dotnet/roslyn-analyzers/tree/e4d7ea6a967631713630cb1cf02efa7dfc35a8aa)*
- `src/runtime`  
*[dotnet/runtime@ffb2578](https://github.com/dotnet/runtime/tree/ffb257818db89672d84d538964cec1b54fb2a094)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@ddccfec](https://github.com/dotnet/scenario-tests/tree/ddccfec3ccd631fb8341c8b6e4e422e8cb339aa5)*
- `src/sdk`  
*[dotnet/sdk@54a101e](https://github.com/dotnet/sdk/tree/54a101e9c8682d7260c382b6de04653e21cfe0b9)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@bcd4473](https://github.com/dotnet/source-build-externals/tree/bcd44732882bc2b81b30146c778eb6ccb7fea793)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights-2.21.0`  
    *[microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/application-insights-2.22.0`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@c1c24e2](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/c1c24e29d5eeac2a2cd53fe0b5656924bdb69e3d)*
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
*[dotnet/sourcelink@ccd1118](https://github.com/dotnet/sourcelink/tree/ccd1118012ffc1a2813399a9f378ddf0b043d3ab)*
- `src/symreader`  
*[dotnet/symreader@01de94d](https://github.com/dotnet/symreader/tree/01de94d9718fd48c511cae276437edcd41b41fa4)*
- `src/templating`  
*[dotnet/templating@f508b3c](https://github.com/dotnet/templating/tree/f508b3c3afbc7c748d8b052c442c7173e3c73401)*
- `src/test-templates`  
*[dotnet/test-templates@3c37712](https://github.com/dotnet/test-templates/tree/3c37712adb7b45506e8445f349367222b35a97dd)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@1233632](https://github.com/dotnet/windowsdesktop/tree/1233632200ec6df0ca38fa1c7ad54f7d820fa9c5)*
- `src/winforms`  
*[dotnet/winforms@3c3f911](https://github.com/dotnet/winforms/tree/3c3f911e162ab3528fe14b3f221bcc4c4bd447ec)*
- `src/wpf`  
*[dotnet/wpf@f1ebf9d](https://github.com/dotnet/wpf/tree/f1ebf9d0652f379a98482b276860b0f5aa19627b)*
- `src/xdt`  
*[dotnet/xdt@48b69ff](https://github.com/dotnet/xdt/tree/48b69ffe49e1514dc123b4e5453d4ee1e998581c)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
