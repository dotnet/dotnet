# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@abddd0b](https://github.com/dotnet/arcade/tree/abddd0bd5145578246dcadda264c7557f2a935a9)*
- `src/aspire`  
*[dotnet/aspire@66a1dd7](https://github.com/dotnet/aspire/tree/66a1dd77e4077592a587c1429c8814d1057dc474)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@52c7ee5](https://github.com/dotnet/aspnetcore/tree/52c7ee5a5c34631570b1b8e91debca8fdf960e0f)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@7c07a86](https://github.com/google/googletest/tree/7c07a863693b0c831f80473f7c6905d7e458682c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b8c2293](https://github.com/dotnet/cecil/tree/b8c2293cd1cbd9d0fe6f32d7b5befbd526b5a175)*
- `src/command-line-api`  
*[dotnet/command-line-api@ecd2ce5](https://github.com/dotnet/command-line-api/tree/ecd2ce5eafbba3008a7d4f5d04b025d30928c812)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@e56c69b](https://github.com/dotnet/deployment-tools/tree/e56c69b0610b50407d29fdc2dda2574712a7b94d)*
- `src/diagnostics`  
*[dotnet/diagnostics@82c9a13](https://github.com/dotnet/diagnostics/tree/82c9a134f8c13dcfe8a9c243a19ee1861bbcb8ea)*
- `src/emsdk`  
*[dotnet/emsdk@fd99e59](https://github.com/dotnet/emsdk/tree/fd99e59a43cfd6fe323c98ff267b620a67e71e67)*
- `src/format`  
*[dotnet/format@ed49f9c](https://github.com/dotnet/format/tree/ed49f9c7d60f259d435047f22f4eb7a57437caeb)*
- `src/fsharp`  
*[dotnet/fsharp@8d7795d](https://github.com/dotnet/fsharp/tree/8d7795d4a68a21010577f11084ba937e51daf9a3)*
- `src/installer`  
*[dotnet/installer@ae420bd](https://github.com/dotnet/installer/tree/ae420bd385b76cb4e430ad3ece4fc040487dc9b9)*
- `src/msbuild`  
*[dotnet/msbuild@6d97976](https://github.com/dotnet/msbuild/tree/6d97976719d4aefae595ee919b942da452e97e57)*
- `src/nuget-client`  
*[nuget/nuget.client@d55931a](https://github.com/nuget/nuget.client/tree/d55931a69dcda3dcb87ba46a09fe268e0febc223)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@45a7162](https://github.com/dotnet/razor/tree/45a71626e342c0694b2a56d856cda5498cd0d6e4)*
- `src/roslyn`  
*[dotnet/roslyn@3cd939f](https://github.com/dotnet/roslyn/tree/3cd939f76803da435c20b082a5cfcc844386fcfb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@e39798f](https://github.com/dotnet/roslyn-analyzers/tree/e39798fc8357615ab319c81b20acfb036ef7b513)*
- `src/runtime`  
*[dotnet/runtime@cd0a72c](https://github.com/dotnet/runtime/tree/cd0a72c86047559020d56c6a326af83d1cf25a13)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@36c82c0](https://github.com/dotnet/sdk/tree/36c82c0f7d47e17dba2857459d296c98fd8d64b5)*
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
*[dotnet/source-build-reference-packages@2249c9a](https://github.com/dotnet/source-build-reference-packages/tree/2249c9a81ec9b6cd86791c21e6e69fbc07099788)*
- `src/sourcelink`  
*[dotnet/sourcelink@8f63279](https://github.com/dotnet/sourcelink/tree/8f632790ea2ae7a6500ec0b16f13e39037ee9dcc)*
- `src/symreader`  
*[dotnet/symreader@b264fbe](https://github.com/dotnet/symreader/tree/b264fbe4dfb1e665a49f052940697bd6dabd561f)*
- `src/templating`  
*[dotnet/templating@74d4a78](https://github.com/dotnet/templating/tree/74d4a78aeb9518b5973ad3e710347e5aec583ddd)*
- `src/test-templates`  
*[dotnet/test-templates@81349c1](https://github.com/dotnet/test-templates/tree/81349c13c2b8e8babf1cdd4e7ab350fbb1b193a4)*
- `src/vstest`  
*[microsoft/vstest@53df73d](https://github.com/microsoft/vstest/tree/53df73d3373e7964f6fb37f4437bda2720a75ef2)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@eb71c8b](https://github.com/dotnet/windowsdesktop/tree/eb71c8b90efe3cec759da407a12c368ef3ba6a4c)*
- `src/winforms`  
*[dotnet/winforms@54fda6c](https://github.com/dotnet/winforms/tree/54fda6cd772e341c4aad720c8a118297da2f46fd)*
- `src/wpf`  
*[dotnet/wpf@34bea7b](https://github.com/dotnet/wpf/tree/34bea7b0eca5f62d09807a1d43b69266a4470aad)*
- `src/xdt`  
*[dotnet/xdt@a10f0a8](https://github.com/dotnet/xdt/tree/a10f0a85b91b1e2e18cbd2ea2537eae9c5a64ea9)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
