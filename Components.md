# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@d5b02a4](https://github.com/dotnet/arcade/tree/d5b02a4900c4d521cb48b8f0d7e3f28175268f7c)*
- `src/aspire`  
*[dotnet/aspire@1dd4b32](https://github.com/dotnet/aspire/tree/1dd4b3265f01a50b20522fd3d7f3cd315db5be6b)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@9dc43ee](https://github.com/dotnet/aspnetcore/tree/9dc43ee05cba5cad16f7ba56cb88711e7096bf84)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b75ecf1](https://github.com/google/googletest/tree/b75ecf1bed2fcd416b66c86cb6fe79122abf132e)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@ca7e934](https://github.com/dotnet/cecil/tree/ca7e93445acbd94bfa696c16fa039f2a6130f2cb)*
- `src/command-line-api`  
*[dotnet/command-line-api@e9ac4ff](https://github.com/dotnet/command-line-api/tree/e9ac4ff4293cf853f3d07eb9e747aef27f5be965)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@822ff26](https://github.com/dotnet/deployment-tools/tree/822ff266c5f999ab9ceb6928df59d79285ea4a4f)*
- `src/diagnostics`  
*[dotnet/diagnostics@b190e56](https://github.com/dotnet/diagnostics/tree/b190e56e28d67e4d5eb7848e705cb1d5e2bcae17)*
- `src/emsdk`  
*[dotnet/emsdk@8afd924](https://github.com/dotnet/emsdk/tree/8afd92448d03a80001c9cac5f2acb53b336263a4)*
- `src/format`  
*[dotnet/format@642934d](https://github.com/dotnet/format/tree/642934d511abb9916d7da0d118a7357d35d4f2cb)*
- `src/fsharp`  
*[dotnet/fsharp@e74bc18](https://github.com/dotnet/fsharp/tree/e74bc18e6411ce3a6265aabd36ae6491ee4ebf5c)*
- `src/installer`  
*[dotnet/installer@1415dfd](https://github.com/dotnet/installer/tree/1415dfd3e2b46366fe09f11c4780bbb39f4278cf)*
- `src/msbuild`  
*[dotnet/msbuild@53c4f49](https://github.com/dotnet/msbuild/tree/53c4f49868e88ecd7407339a3f4dab9e75c7937f)*
- `src/nuget-client`  
*[nuget/nuget.client@8b658e2](https://github.com/nuget/nuget.client/tree/8b658e2eee6391936887b9fd1b39f7918d16a9cb)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@667c05c](https://github.com/dotnet/razor/tree/667c05ce82ca55f15ecb4578274df4aa0af9d204)*
- `src/roslyn`  
*[dotnet/roslyn@25aa74d](https://github.com/dotnet/roslyn/tree/25aa74d725e801b8232dbb3e5abcda0fa72da8c5)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@ab13ac7](https://github.com/dotnet/roslyn-analyzers/tree/ab13ac75d1195a27e3cf7fb990fc3a51615caa95)*
- `src/runtime`  
*[dotnet/runtime@cf56f72](https://github.com/dotnet/runtime/tree/cf56f726ba7b77690c79146726a57f7be573ead5)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@e24f8a1](https://github.com/dotnet/sdk/tree/e24f8a1c93050f622d483e3f3056f6139c7b5132)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@ddfb604](https://github.com/dotnet/source-build-externals/tree/ddfb60463c966af55fd0e222c2266170e83d1324)*
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
*[dotnet/source-build-reference-packages@539af5d](https://github.com/dotnet/source-build-reference-packages/tree/539af5d8ae183d4fe61e8b2f8f4a8505c8a765a7)*
- `src/sourcelink`  
*[dotnet/sourcelink@2860079](https://github.com/dotnet/sourcelink/tree/286007916f88fe58149cb5a42a4dbdb4b62c8850)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@f4befb9](https://github.com/dotnet/templating/tree/f4befb90954c2f34dcd9e5d5db9ede72bef7b6fb)*
- `src/test-templates`  
*[dotnet/test-templates@1591b24](https://github.com/dotnet/test-templates/tree/1591b24326caa98288e04e18e5c1b75c36c917c1)*
- `src/vstest`  
*[microsoft/vstest@c13b1f9](https://github.com/microsoft/vstest/tree/c13b1f9b2bb6acbb9785de003be3d9ace33c9d7c)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@eb467cd](https://github.com/dotnet/windowsdesktop/tree/eb467cd197ecaea413162f11116348cf6a4edf3a)*
- `src/winforms`  
*[dotnet/winforms@1fe403d](https://github.com/dotnet/winforms/tree/1fe403df392add1bb95dc576065bbff811109c23)*
- `src/wpf`  
*[dotnet/wpf@2633797](https://github.com/dotnet/wpf/tree/2633797b417f67cde154aa8a53d1990df82d1006)*
- `src/xdt`  
*[dotnet/xdt@b503f91](https://github.com/dotnet/xdt/tree/b503f918c4ade1dbf33c310cb6848689e85b0b91)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
