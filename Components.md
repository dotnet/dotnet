# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@ace00d8](https://github.com/dotnet/arcade/tree/ace00d8719b8d1fdfd0cc05f71bb9af216338d27)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@d4eca39](https://github.com/dotnet/aspnetcore/tree/d4eca39c3fc1944b2c6431bf6b22036bdb176c0d)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@c231e6f](https://github.com/google/googletest/tree/c231e6f5b152029dbd5fa4a9e0c04095035aec3f)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@ba53c75](https://github.com/dotnet/cecil/tree/ba53c75483aa4980a332fa48e61076f80adfec40)*
- `src/command-line-api`  
*[dotnet/command-line-api@5ea97af](https://github.com/dotnet/command-line-api/tree/5ea97af07263ea3ef68a18557c8aa3f7e3200bda)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@2a7741c](https://github.com/dotnet/deployment-tools/tree/2a7741c5a7cb49fbad797c4b2f7812d5620430ac)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/emsdk`  
*[dotnet/emsdk@a5f4de7](https://github.com/dotnet/emsdk/tree/a5f4de78fca42544771977f8e8e04c4aa83e1d02)*
- `src/format`  
*[dotnet/format@91f6031](https://github.com/dotnet/format/tree/91f60316ebd9c75d6be8b7f9b7c201bab17240c9)*
- `src/fsharp`  
*[dotnet/fsharp@2c03643](https://github.com/dotnet/fsharp/tree/2c03643199368f07a3326709fc68abcbfc482a06)*
- `src/installer`  
*[dotnet/installer@09d6f38](https://github.com/dotnet/installer/tree/09d6f381e6917ee70cf6cbc6109c675c4a51bf77)*
- `src/msbuild`  
*[dotnet/msbuild@a95f582](https://github.com/dotnet/msbuild/tree/a95f5821794cd69f2da0ba14dda9440919637eda)*
- `src/nuget-client`  
*[nuget/nuget.client@1845d6b](https://github.com/nuget/nuget.client/tree/1845d6bd450a7453d573035371c9fec43683d1ef)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@eb9363d](https://github.com/dotnet/razor/tree/eb9363d4f10d99cb81e9695156beef20e04866c7)*
- `src/roslyn`  
*[dotnet/roslyn@98cd097](https://github.com/dotnet/roslyn/tree/98cd097bf122677378692ebe952b71ab6e5bb013)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@22a5b5a](https://github.com/dotnet/roslyn-analyzers/tree/22a5b5af1a402fbba34dfbbdeadeb5aa571d008e)*
- `src/runtime`  
*[dotnet/runtime@9e6ba1f](https://github.com/dotnet/runtime/tree/9e6ba1f68c6a9c7206dacdf1e4cac67ea19931eb)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@341421c](https://github.com/dotnet/sdk/tree/341421cb1fd159d4a8f6e4f5a9bd4ffe12eb7999)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@3167c0a](https://github.com/dotnet/source-build-externals/tree/3167c0a9379f52145af31057aca31f45528eb123)*
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
*[dotnet/sourcelink@f44b7a5](https://github.com/dotnet/sourcelink/tree/f44b7a556d1b44a70651054fd05566254c9bcaa2)*
- `src/symreader`  
*[dotnet/symreader@0bb6063](https://github.com/dotnet/symreader/tree/0bb6063675fd0c8c60b6aa8f9e67cc15c81979d2)*
- `src/templating`  
*[dotnet/templating@1151583](https://github.com/dotnet/templating/tree/11515832c24b5d0a2b46c846b7f64e8871368630)*
- `src/test-templates`  
*[dotnet/test-templates@492de6f](https://github.com/dotnet/test-templates/tree/492de6f8db8c7f537119e4169bc2a4453eda6ccd)*
- `src/vstest`  
*[microsoft/vstest@c609e2c](https://github.com/microsoft/vstest/tree/c609e2c022b0087b227436a4debf45525eed00e9)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@d29e67f](https://github.com/dotnet/windowsdesktop/tree/d29e67fd8b7e9d5385275f3a3a00e00903bb0c5c)*
- `src/winforms`  
*[dotnet/winforms@fd95fcd](https://github.com/dotnet/winforms/tree/fd95fcd9771ffe9270c8c0280a2950e5434c250d)*
- `src/wpf`  
*[dotnet/wpf@85d908c](https://github.com/dotnet/wpf/tree/85d908ce8599d12f5e16ce7d78083cfdc2e13af4)*
- `src/xdt`  
*[dotnet/xdt@e91258d](https://github.com/dotnet/xdt/tree/e91258db72df11e0532d90f25c8e8838f3b3736e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
