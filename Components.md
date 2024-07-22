# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@b4e499d](https://github.com/dotnet/arcade/tree/b4e499d1f6e6b3a981feabfed290d99261762382)*
- `src/aspire`  
*[dotnet/aspire@a6e341e](https://github.com/dotnet/aspire/tree/a6e341ebbf956bbcec0dda304109815fcbae70c9)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@0e1746b](https://github.com/dotnet/aspnetcore/tree/0e1746baca1748e2d197b17533af6e122dcfdc28)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b4aaf97](https://github.com/google/googletest/tree/b4aaf97d8f7eaffab79aa15e10a91b331b941fe2)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@eb33ea2](https://github.com/dotnet/Node-Externals/tree/eb33ea21fa1013d5e97951725e9d5c52c5a6a026)*
- `src/cecil`  
*[dotnet/cecil@7e4af02](https://github.com/dotnet/cecil/tree/7e4af02521473d89d6144b3da58fef253e498974)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@790d2fd](https://github.com/dotnet/deployment-tools/tree/790d2fd96379448c91306c67768b41521a4c130f)*
- `src/diagnostics`  
*[dotnet/diagnostics@33d8bf2](https://github.com/dotnet/diagnostics/tree/33d8bf23a6566cd3fb9055acfc9f1141391d5421)*
- `src/efcore`  
*[dotnet/efcore@66fc2aa](https://github.com/dotnet/efcore/tree/66fc2aa66ae1167cc4bccc748a5c128278d8c869)*
- `src/emsdk`  
*[dotnet/emsdk@99ea0c0](https://github.com/dotnet/emsdk/tree/99ea0c06b84d3084d090da537080dd35d2a193cf)*
- `src/fsharp`  
*[dotnet/fsharp@02adf13](https://github.com/dotnet/fsharp/tree/02adf13f8d69e0105fff4d68dbd5fb1d43bc0e17)*
- `src/msbuild`  
*[dotnet/msbuild@fb48b92](https://github.com/dotnet/msbuild/tree/fb48b92ae74b61c678d1406e6faa2cb73e264075)*
- `src/nuget-client`  
*[nuget/nuget.client@6b9e448](https://github.com/nuget/nuget.client/tree/6b9e4481b9c23665ceb3192b9964921bcbb67c30)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@cb90af2](https://github.com/dotnet/razor/tree/cb90af2a06b03b015e91b1df6d3997de56025075)*
- `src/roslyn`  
*[dotnet/roslyn@30edd04](https://github.com/dotnet/roslyn/tree/30edd04fd41dec9e8f9f48e698ebd5b80d9f7677)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@43709af](https://github.com/dotnet/roslyn-analyzers/tree/43709af7570da7140fb3e9a5237f55ffb24677e7)*
- `src/runtime`  
*[dotnet/runtime@1f70f0c](https://github.com/dotnet/runtime/tree/1f70f0cc663b5d45f77840e9728386e5fa7944f9)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@54700bb](https://github.com/dotnet/scenario-tests/tree/54700bbee86f660d37bd519a905b62bb50adc8c8)*
- `src/sdk`  
*[dotnet/sdk@1a84d18](https://github.com/dotnet/sdk/tree/1a84d18ec99eec5c48830bdee2be3476789fb123)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@26c52d0](https://github.com/dotnet/source-build-externals/tree/26c52d02b67816269e647cc584f6b5db9a91970f)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@2e7c701](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/2e7c701881d3d67aff7bf54f22063a49bc4727d2)*
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
*[dotnet/source-build-reference-packages@4d5ba72](https://github.com/dotnet/source-build-reference-packages/tree/4d5ba7206ed1d56612b36560334494652ed486b2)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@2f27fdc](https://github.com/dotnet/templating/tree/2f27fdc69f89f34e2f0672f322b6fc54a7e14f11)*
- `src/test-templates`  
*[dotnet/test-templates@c7852b8](https://github.com/dotnet/test-templates/tree/c7852b88d3f9c5249aef10661cdbca0a93c00576)*
- `src/vstest`  
*[microsoft/vstest@a1f5a65](https://github.com/microsoft/vstest/tree/a1f5a6500b8cfefa81adbb652a84ad0ba884c140)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@90f32e0](https://github.com/dotnet/windowsdesktop/tree/90f32e0fbb8fdcb9701942a0ea973ad4fe4883e7)*
- `src/winforms`  
*[dotnet/winforms@bb601b3](https://github.com/dotnet/winforms/tree/bb601b3242ccbf5d8a77a44af27a374f677be8d6)*
- `src/wpf`  
*[dotnet/wpf@8529feb](https://github.com/dotnet/wpf/tree/8529feb7ce7dcb68ef581a2a2e6f8e6bff7290e0)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
