# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@4a7d983](https://github.com/dotnet/arcade/tree/4a7d983f833d6b86365ea1b2b4d6ee72fbdbf944)*
- `src/aspire`  
*[dotnet/aspire@a6e341e](https://github.com/dotnet/aspire/tree/a6e341ebbf956bbcec0dda304109815fcbae70c9)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@06533a6](https://github.com/dotnet/aspnetcore/tree/06533a6b6d2142fd083279e6e4c74d811a467476)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@1d17ea1](https://github.com/google/googletest/tree/1d17ea141d2c11b8917d2c7d029f1c4e2b9769b2)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@a1b1b1b](https://github.com/dotnet/Node-Externals/tree/a1b1b1bb01630a6109adf5767d9a2770c6dc5639)*
- `src/cecil`  
*[dotnet/cecil@7e4af02](https://github.com/dotnet/cecil/tree/7e4af02521473d89d6144b3da58fef253e498974)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@d882ae4](https://github.com/dotnet/deployment-tools/tree/d882ae4af9fb09a89e36487a9c8cb7dfde713927)*
- `src/diagnostics`  
*[dotnet/diagnostics@33d8bf2](https://github.com/dotnet/diagnostics/tree/33d8bf23a6566cd3fb9055acfc9f1141391d5421)*
- `src/efcore`  
*[dotnet/efcore@ec42520](https://github.com/dotnet/efcore/tree/ec42520f1717d410314bfca41aba9f4cd04f7ab3)*
- `src/emsdk`  
*[dotnet/emsdk@ffe9afd](https://github.com/dotnet/emsdk/tree/ffe9afdc046cf7a6f82cc7c5796aade54047af64)*
- `src/fsharp`  
*[dotnet/fsharp@02adf13](https://github.com/dotnet/fsharp/tree/02adf13f8d69e0105fff4d68dbd5fb1d43bc0e17)*
- `src/msbuild`  
*[dotnet/msbuild@06bb1c2](https://github.com/dotnet/msbuild/tree/06bb1c233fd4f2cd136ba3bbbfcb4b553a2eceb6)*
- `src/nuget-client`  
*[nuget/nuget.client@6c642c2](https://github.com/nuget/nuget.client/tree/6c642c2d63717acd4bf92050a42691928020eb89)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@ca5327e](https://github.com/dotnet/razor/tree/ca5327e7a8b4ab1da8caf2bddbea9e8f629d96c4)*
- `src/roslyn`  
*[dotnet/roslyn@19b5e96](https://github.com/dotnet/roslyn/tree/19b5e961ecb97b008106f1b646c077e0bffde4a7)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@43709af](https://github.com/dotnet/roslyn-analyzers/tree/43709af7570da7140fb3e9a5237f55ffb24677e7)*
- `src/runtime`  
*[dotnet/runtime@1d16fd1](https://github.com/dotnet/runtime/tree/1d16fd14fa772621c7b09ebd00f83610207b1abf)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@54700bb](https://github.com/dotnet/scenario-tests/tree/54700bbee86f660d37bd519a905b62bb50adc8c8)*
- `src/sdk`  
*[dotnet/sdk@810b22d](https://github.com/dotnet/sdk/tree/810b22d2c6a3129879c6ea55135f7c9ae9d8cd63)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@311ef7f](https://github.com/dotnet/source-build-externals/tree/311ef7fef52828f4a70a94d13e32c394fd3292ee)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@0183521](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/0183521b0f127a214aa28cfb8385acfef8c4aa22)*
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
*[dotnet/source-build-reference-packages@cc732c5](https://github.com/dotnet/source-build-reference-packages/tree/cc732c57199f725857c201da146525e3be6bc504)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@c3ab97f](https://github.com/dotnet/templating/tree/c3ab97f976154f6ddd0bd2fba89f52134a91c2cc)*
- `src/test-templates`  
*[dotnet/test-templates@381a52e](https://github.com/dotnet/test-templates/tree/381a52eabdfba03775f9f5887130803d7079d1aa)*
- `src/vstest`  
*[microsoft/vstest@a1f5a65](https://github.com/microsoft/vstest/tree/a1f5a6500b8cfefa81adbb652a84ad0ba884c140)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@258d957](https://github.com/dotnet/windowsdesktop/tree/258d9575485ff6511dbb749af5100eadc1da6968)*
- `src/winforms`  
*[dotnet/winforms@9319f17](https://github.com/dotnet/winforms/tree/9319f17101b6afd661e943248f8ea03886d1b8a6)*
- `src/wpf`  
*[dotnet/wpf@eb93169](https://github.com/dotnet/wpf/tree/eb93169ec7923e00e014c8863817ec92ac743cef)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
