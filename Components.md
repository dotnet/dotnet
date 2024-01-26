# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@abddd0b](https://github.com/dotnet/arcade/tree/abddd0bd5145578246dcadda264c7557f2a935a9)*
- `src/aspire`  
*[dotnet/aspire@8a902ec](https://github.com/dotnet/aspire/tree/8a902ec654d701cbcb47c5730bd006e50bd561ef)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@1d58b24](https://github.com/dotnet/aspnetcore/tree/1d58b24591820b2a134cfc0a5e3154bf9fe65cb4)*
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
*[dotnet/format@ed49f9c](https://github.com/dotnet/format/tree/ed49f9c7d60f259d435047f22f4eb7a57437caeb)*
- `src/fsharp`  
*[dotnet/fsharp@32898dc](https://github.com/dotnet/fsharp/tree/32898dc51efc669de98e7e47f57d521bc07ac4cc)*
- `src/installer`  
*[dotnet/installer@f7f056d](https://github.com/dotnet/installer/tree/f7f056d632df5b7bba74b8f6908e15af0d2eb4c6)*
- `src/msbuild`  
*[dotnet/msbuild@d51ae52](https://github.com/dotnet/msbuild/tree/d51ae5297cd0a24caa8cfe356442cc8634c3f087)*
- `src/nuget-client`  
*[nuget/nuget.client@e4899ee](https://github.com/nuget/nuget.client/tree/e4899ee48ff3d7787ee345f546c818ce6b962807)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@00a9bb1](https://github.com/dotnet/razor/tree/00a9bb181a4028e7fff09d989c5540cff677e411)*
- `src/roslyn`  
*[dotnet/roslyn@3cd939f](https://github.com/dotnet/roslyn/tree/3cd939f76803da435c20b082a5cfcc844386fcfb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@e39798f](https://github.com/dotnet/roslyn-analyzers/tree/e39798fc8357615ab319c81b20acfb036ef7b513)*
- `src/runtime`  
*[dotnet/runtime@b4aa81d](https://github.com/dotnet/runtime/tree/b4aa81d9da52616ac18e6eb584e37a08cfb2ba87)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@12a5a07](https://github.com/dotnet/sdk/tree/12a5a075f443f73f4db1abdb61a37cf17951b22d)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@6fc8c1a](https://github.com/dotnet/source-build-externals/tree/6fc8c1ac45220a4d9b4c59bf2ff187dafcb1da3f)*
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
*[dotnet/templating@647c87d](https://github.com/dotnet/templating/tree/647c87d68aeb36d008bd9395625c0c4e2f3257d2)*
- `src/test-templates`  
*[dotnet/test-templates@81349c1](https://github.com/dotnet/test-templates/tree/81349c13c2b8e8babf1cdd4e7ab350fbb1b193a4)*
- `src/vstest`  
*[microsoft/vstest@53df73d](https://github.com/microsoft/vstest/tree/53df73d3373e7964f6fb37f4437bda2720a75ef2)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@85b4932](https://github.com/dotnet/windowsdesktop/tree/85b4932a31a185105303259e70347309c5d1e39d)*
- `src/winforms`  
*[dotnet/winforms@6532984](https://github.com/dotnet/winforms/tree/653298452192c54eb6b62e8b5b9d44895c01dc16)*
- `src/wpf`  
*[dotnet/wpf@90a2728](https://github.com/dotnet/wpf/tree/90a2728da19afd0cca16f614c7574026499fbf8c)*
- `src/xdt`  
*[dotnet/xdt@a10f0a8](https://github.com/dotnet/xdt/tree/a10f0a85b91b1e2e18cbd2ea2537eae9c5a64ea9)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
