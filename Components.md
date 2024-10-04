# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@1230437](https://github.com/dotnet/arcade/tree/1230437de1ab7b3e15fe7cdfe7ffce2f65449959)*
- `src/aspire`  
*[dotnet/aspire@137e8dc](https://github.com/dotnet/aspire/tree/137e8dcae0a7b22c05f48c4e7a5d36fe3f00a8d7)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@9d54585](https://github.com/dotnet/aspnetcore/tree/9d5458536b0e28996570b0ebbb1c2dbc5afedf56)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@0953a17](https://github.com/google/googletest/tree/0953a17a4281fc26831da647ad3fcd5e21e6473b)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@9c94433](https://github.com/dotnet/cecil/tree/9c9443396f8deacceb8edb169890e52aac25f311)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@c7035b3](https://github.com/dotnet/efcore/tree/c7035b3ccc485a63bd03a64da4d9f0ec18f27948)*
- `src/emsdk`  
*[dotnet/emsdk@8e660ff](https://github.com/dotnet/emsdk/tree/8e660ff41e91879977e3a9d837e068bd72234c26)*
- `src/fsharp`  
*[dotnet/fsharp@19610c0](https://github.com/dotnet/fsharp/tree/19610c0b654766eec49d044cb97ca6eaa2a63d16)*
- `src/msbuild`  
*[dotnet/msbuild@95c7bf0](https://github.com/dotnet/msbuild/tree/95c7bf011ac3b849c63cfaf5584878232d223b71)*
- `src/nuget-client`  
*[nuget/nuget.client@675e8a8](https://github.com/nuget/nuget.client/tree/675e8a81acb53958e40dc21be5f562d00239a449)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@f266ffb](https://github.com/dotnet/razor/tree/f266ffb956e3704b1473d572cd3015fc0ee8b3be)*
- `src/roslyn`  
*[dotnet/roslyn@dc30397](https://github.com/dotnet/roslyn/tree/dc303976c995da5871863b19d0f39e0e9b4c4654)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@2595aeb](https://github.com/dotnet/roslyn-analyzers/tree/2595aeb5e9a506f3f845c01be18d70ded045e33a)*
- `src/runtime`  
*[dotnet/runtime@5880118](https://github.com/dotnet/runtime/tree/58801184a1f0300d193517a77a054f51752aa3b2)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@76edac9](https://github.com/dotnet/scenario-tests/tree/76edac963b2c6296018c7bf1e04a03c8488076d3)*
- `src/sdk`  
*[dotnet/sdk@400deeb](https://github.com/dotnet/sdk/tree/400deebe954609bb60ba2855f87e9816e4f0620e)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@fbe8f0b](https://github.com/dotnet/source-build-externals/tree/fbe8f0b52ae0e083461d89db7229f6d70e874644)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
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
*[dotnet/source-build-reference-packages@9ac2fa7](https://github.com/dotnet/source-build-reference-packages/tree/9ac2fa767b57b99e84560ca78ef48bd1c76ab0dd)*
- `src/sourcelink`  
*[dotnet/sourcelink@da0cf0b](https://github.com/dotnet/sourcelink/tree/da0cf0ba63bd8f346ab6eff998d1c9bba092541a)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@a23da1c](https://github.com/dotnet/templating/tree/a23da1c15c737b5e121650cfa5a86805e74e34fc)*
- `src/test-templates`  
*[dotnet/test-templates@8a94286](https://github.com/dotnet/test-templates/tree/8a94286429bf4ac8019689cb6a56f1173839977e)*
- `src/vstest`  
*[microsoft/vstest@8c6ff8c](https://github.com/microsoft/vstest/tree/8c6ff8c6a8e74e74f91a221719ab93e00b5983e0)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@df03828](https://github.com/dotnet/windowsdesktop/tree/df0382842a58d4ef536472443a5b4ecc05c8cf90)*
- `src/winforms`  
*[dotnet/winforms@388decb](https://github.com/dotnet/winforms/tree/388decbda4d707d996bb1820f17d6fa805f6e47c)*
- `src/wpf`  
*[dotnet/wpf@f1d777f](https://github.com/dotnet/wpf/tree/f1d777f471b7ddaccf33ada60034bcf1a9dc06ec)*
- `src/xdt`  
*[dotnet/xdt@4ddd811](https://github.com/dotnet/xdt/tree/4ddd8113a29852380b7b929117bfe67f401ac320)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
