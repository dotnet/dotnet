# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@abddd0b](https://github.com/dotnet/arcade/tree/abddd0bd5145578246dcadda264c7557f2a935a9)*
- `src/aspire`  
*[dotnet/aspire@1226bf2](https://github.com/dotnet/aspire/tree/1226bf2df28644de5f808dc1674f4a195fb333e8)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@6885598](https://github.com/dotnet/aspnetcore/tree/688559886575a7f0d84b771de5c17501d5daaedc)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@7c07a86](https://github.com/google/googletest/tree/7c07a863693b0c831f80473f7c6905d7e458682c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b8c2293](https://github.com/dotnet/cecil/tree/b8c2293cd1cbd9d0fe6f32d7b5befbd526b5a175)*
- `src/command-line-api`  
*[dotnet/command-line-api@ecd2ce5](https://github.com/dotnet/command-line-api/tree/ecd2ce5eafbba3008a7d4f5d04b025d30928c812)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b4f8847](https://github.com/dotnet/deployment-tools/tree/b4f8847a36543b3274dc252534d0175de35bd16c)*
- `src/diagnostics`  
*[dotnet/diagnostics@82c9a13](https://github.com/dotnet/diagnostics/tree/82c9a134f8c13dcfe8a9c243a19ee1861bbcb8ea)*
- `src/emsdk`  
*[dotnet/emsdk@e8ab136](https://github.com/dotnet/emsdk/tree/e8ab136db368ccb85c572d2c1541e3056883df3c)*
- `src/format`  
*[dotnet/format@4f1acf0](https://github.com/dotnet/format/tree/4f1acf059b7e5e4ee76125a5a1dd94ce45d87a14)*
- `src/fsharp`  
*[dotnet/fsharp@80003bb](https://github.com/dotnet/fsharp/tree/80003bb3f0516455a0046887aa169febf2c4d3a8)*
- `src/installer`  
*[dotnet/installer@5499f92](https://github.com/dotnet/installer/tree/5499f92df4a46606612c5d498fa7f7031cb3c9c1)*
- `src/msbuild`  
*[dotnet/msbuild@97651a2](https://github.com/dotnet/msbuild/tree/97651a25d833ebd284cc7bd8b69bb88630b85b8e)*
- `src/nuget-client`  
*[nuget/nuget.client@e4899ee](https://github.com/nuget/nuget.client/tree/e4899ee48ff3d7787ee345f546c818ce6b962807)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8e5fbd4](https://github.com/dotnet/razor/tree/8e5fbd463e970140b303fcb9a6268148cb4f51ad)*
- `src/roslyn`  
*[dotnet/roslyn@3cd939f](https://github.com/dotnet/roslyn/tree/3cd939f76803da435c20b082a5cfcc844386fcfb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@e39798f](https://github.com/dotnet/roslyn-analyzers/tree/e39798fc8357615ab319c81b20acfb036ef7b513)*
- `src/runtime`  
*[dotnet/runtime@1d1bf92](https://github.com/dotnet/runtime/tree/1d1bf92fcf43aa6981804dc53c5174445069c9e4)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@a484271](https://github.com/dotnet/sdk/tree/a484271b10c8bafb4f16c3c561e41840dbabd3a5)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@414a85b](https://github.com/dotnet/source-build-externals/tree/414a85bf970355c0e91d6a2de1ee183fafbfcecd)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
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
*[dotnet/source-build-reference-packages@e659f32](https://github.com/dotnet/source-build-reference-packages/tree/e659f328bf255d3e17e81296117c3aed1d461f2f)*
- `src/sourcelink`  
*[dotnet/sourcelink@8f63279](https://github.com/dotnet/sourcelink/tree/8f632790ea2ae7a6500ec0b16f13e39037ee9dcc)*
- `src/symreader`  
*[dotnet/symreader@b264fbe](https://github.com/dotnet/symreader/tree/b264fbe4dfb1e665a49f052940697bd6dabd561f)*
- `src/templating`  
*[dotnet/templating@e0ef000](https://github.com/dotnet/templating/tree/e0ef000d2c0429b7458c7ffbd43515419c367f42)*
- `src/test-templates`  
*[dotnet/test-templates@81349c1](https://github.com/dotnet/test-templates/tree/81349c13c2b8e8babf1cdd4e7ab350fbb1b193a4)*
- `src/vstest`  
*[microsoft/vstest@29768a4](https://github.com/microsoft/vstest/tree/29768a4cf9f7b9edd6963a25863298b8ccbba2f2)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@1354412](https://github.com/dotnet/windowsdesktop/tree/135441216b173df08c391beeb2d2a9a324eadf17)*
- `src/winforms`  
*[dotnet/winforms@54d6056](https://github.com/dotnet/winforms/tree/54d6056e0c17b4ddc3262e1c836d2b82ef22bd73)*
- `src/wpf`  
*[dotnet/wpf@1aac60e](https://github.com/dotnet/wpf/tree/1aac60e5b2b5021a035fb657cb108f438a0d322d)*
- `src/xdt`  
*[dotnet/xdt@a10f0a8](https://github.com/dotnet/xdt/tree/a10f0a85b91b1e2e18cbd2ea2537eae9c5a64ea9)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
