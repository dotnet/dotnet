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
*[dotnet/aspnetcore@cc5bc6b](https://github.com/dotnet/aspnetcore/tree/cc5bc6b56dc0a6e6f1ca103acff5afd9c71973ba)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b75ecf1](https://github.com/google/googletest/tree/b75ecf1bed2fcd416b66c86cb6fe79122abf132e)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@61250b0](https://github.com/dotnet/cecil/tree/61250b0ed403b3f9b69a33f7d8f66f311338d6a1)*
- `src/command-line-api`  
*[dotnet/command-line-api@e9ac4ff](https://github.com/dotnet/command-line-api/tree/e9ac4ff4293cf853f3d07eb9e747aef27f5be965)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@822ff26](https://github.com/dotnet/deployment-tools/tree/822ff266c5f999ab9ceb6928df59d79285ea4a4f)*
- `src/diagnostics`  
*[dotnet/diagnostics@b190e56](https://github.com/dotnet/diagnostics/tree/b190e56e28d67e4d5eb7848e705cb1d5e2bcae17)*
- `src/emsdk`  
*[dotnet/emsdk@2d3f1fe](https://github.com/dotnet/emsdk/tree/2d3f1fe4807a21879cedba9d3fde8cd329fb17f2)*
- `src/format`  
*[dotnet/format@642934d](https://github.com/dotnet/format/tree/642934d511abb9916d7da0d118a7357d35d4f2cb)*
- `src/fsharp`  
*[dotnet/fsharp@02bb351](https://github.com/dotnet/fsharp/tree/02bb351b606468bce98688fccda9d0c8adcd964d)*
- `src/installer`  
*[dotnet/installer@39605ca](https://github.com/dotnet/installer/tree/39605ca72003cbb09022a2acfd2ac38351bebb57)*
- `src/msbuild`  
*[dotnet/msbuild@d4cb14f](https://github.com/dotnet/msbuild/tree/d4cb14fe4d2e6df0327308feab18ccbb2046246c)*
- `src/nuget-client`  
*[nuget/nuget.client@63958aa](https://github.com/nuget/nuget.client/tree/63958aab19b7120862ff55eac32ab6a155596a59)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@67bd825](https://github.com/dotnet/razor/tree/67bd8253e308d06bf249a02911d06317d5d7b5b9)*
- `src/roslyn`  
*[dotnet/roslyn@2bfcc49](https://github.com/dotnet/roslyn/tree/2bfcc497086de48b15861bb56407324facef4c4a)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@ba8b7f2](https://github.com/dotnet/roslyn-analyzers/tree/ba8b7f2c3ae092d0301f0c5e49bd30340af553c8)*
- `src/runtime`  
*[dotnet/runtime@2106224](https://github.com/dotnet/runtime/tree/2106224d35cc81e04234bc4d4daaf8f944fceeca)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@3075c3b](https://github.com/dotnet/sdk/tree/3075c3bc8d8aa4e0ffcf2945459ef2ee383e3ae0)*
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
*[dotnet/sourcelink@01eadf6](https://github.com/dotnet/sourcelink/tree/01eadf6bfd22b6be33c6861a51c86bd552f0ece0)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@1a471b9](https://github.com/dotnet/templating/tree/1a471b9eebae68a24cbf7d32e5e9b1a23487d9a0)*
- `src/test-templates`  
*[dotnet/test-templates@1591b24](https://github.com/dotnet/test-templates/tree/1591b24326caa98288e04e18e5c1b75c36c917c1)*
- `src/vstest`  
*[microsoft/vstest@48d8e77](https://github.com/microsoft/vstest/tree/48d8e778c871315db0bad221b00f4843b06242c3)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@6cd8a23](https://github.com/dotnet/windowsdesktop/tree/6cd8a235fa93f02ae0400a78625dab312c4642f2)*
- `src/winforms`  
*[dotnet/winforms@852f7ef](https://github.com/dotnet/winforms/tree/852f7ef241402c2336e085c82dca6f70b7400c21)*
- `src/wpf`  
*[dotnet/wpf@1b67350](https://github.com/dotnet/wpf/tree/1b673503bc443f8272c50c4c14493fadb08a376f)*
- `src/xdt`  
*[dotnet/xdt@b503f91](https://github.com/dotnet/xdt/tree/b503f918c4ade1dbf33c310cb6848689e85b0b91)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
